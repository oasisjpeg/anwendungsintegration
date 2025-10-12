using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using WebApplication1.Domain.Repositories;

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
            ?? throw new InvalidOperationException("JWT Secret is missing from configuration.");

    }

    public async Task InvokeAsync(HttpContext context)
{
    if (!IsRewardPointsWebSocketRequest(context))
    {
        await _next(context);
        return;
    }

    var token = GetToken(context);
    if (!IsTokenValid(token, context)) return;

    if (token != null)
    {
        var principal = ValidateToken(token);
        if (!IsPrincipalValid(principal, context)) return;
        if (principal != null) context.User = principal;
    }

    var userId = GetUserId(context);
    if (!IsUserIdValid(userId, context)) return;

    using var scope = _serviceProvider.CreateScope();
    var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

    if (userId != null) await HandleWebSocketLoop(userId, userRepository, webSocket);
}

private static bool IsRewardPointsWebSocketRequest(HttpContext context) =>
    context.Request.Path == "/ws/rewardpoints" && context.WebSockets.IsWebSocketRequest;

private static string? GetToken(HttpContext context) =>
    context.Request.Query["access_token"].FirstOrDefault();

private static bool IsTokenValid(string? token, HttpContext context)
{
    if (string.IsNullOrEmpty(token))
    {
        context.Response.StatusCode = 401;
        return false;
    }
    return true;
}

private static bool IsPrincipalValid(ClaimsPrincipal? principal, HttpContext context)
{
    if (principal == null)
    {
        context.Response.StatusCode = 401;
        return false;
    }
    return true;
}

private static string? GetUserId(HttpContext context) =>
    context.User.FindFirstValue(ClaimTypes.NameIdentifier);

private static bool IsUserIdValid(string? userId, HttpContext context)
{
    if (string.IsNullOrEmpty(userId))
    {
        context.Response.StatusCode = 401;
        return false;
    }
    return true;
}

private static async Task HandleWebSocketLoop(string userId, IUserRepository userRepository, WebSocket webSocket)
{
    long lastSentPoints = -1;
    try
    {
        while (webSocket.State == WebSocketState.Open)
        {
            var userIdGuid = Guid.Parse(userId);
            var user = await userRepository.GetByIdAsync(userIdGuid);
            var currentPoints = user?.Points ?? 0;

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
                lastSentPoints = currentPoints;
            }
            await Task.Delay(5000);
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
