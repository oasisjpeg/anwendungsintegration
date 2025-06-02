using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Infrastructure.MySqlRepositories;

namespace WebApplication1.Application_Layer.Websockets;

public class RewardPointsWebSocketHandler
{
    private readonly RequestDelegate _next;
    private readonly IServiceProvider _serviceProvider;
    private readonly string _jwtSecret;

    public RewardPointsWebSocketHandler(
        RequestDelegate next,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _next = next;
        _serviceProvider = serviceProvider;
        _jwtSecret = configuration["JwtSettings:Secret"] 
            ?? throw new ArgumentNullException("JWT Secret is missing");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws/rewardpoints" 
            && context.WebSockets.IsWebSocketRequest)
        {
            // Extract token from query string
            var token = context.Request.Query["access_token"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                return;
            }

            // Validate token manually
            var principal = ValidateToken(token);
            if (principal == null)
            {
                context.Response.StatusCode = 401;
                return;
            }
            context.User = principal;

            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                context.Response.StatusCode = 401;
                return;
            }

            using var scope = _serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider
                .GetRequiredService<MySqlDbContext>();

            var webSocket = await context.WebSockets.AcceptWebSocketAsync();
            long lastSentPoints = -1; // Initial state

            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var currentPoints = await dbContext.Users
                        .AsNoTracking()
                        .Where(u => u.Id == userId)
                        .Select(u => u.Points)
                        .FirstOrDefaultAsync();

                    if (currentPoints != lastSentPoints)
                    {
                        var payload = new { points = currentPoints };
                        var message = JsonSerializer.Serialize(payload);
                        var buffer = Encoding.UTF8.GetBytes(message);

                        await webSocket.SendAsync(
                            new ArraySegment<byte>(buffer),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None
                        );

                        lastSentPoints = currentPoints; // Update last sent value
                    }

                    await Task.Delay(5000); // Check every 5 seconds
                }
            }
            finally
            {
                if (webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Connection closed",
                        CancellationToken.None);
                }
            }
        }
        else
        {
            await _next(context);
        }
    }

    private ClaimsPrincipal? ValidateToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSecret);

        try
        {
            return tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        catch
        {
            return null;
        }
    }
}
