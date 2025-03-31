namespace WebApplication1.Application_Layer.Websockets;

using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Security.Claims;

public class MotivationWebSocketHandler
{
    private readonly RequestDelegate _next;

    public MotivationWebSocketHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path == "/ws/motivation" && context.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await context.WebSockets.AcceptWebSocketAsync();


            while (webSocket.State == WebSocketState.Open)
            {
                var motivation = GetRandomMotivationalSpeech();

                var payload = new
                {
                    motivation
                };

                var message = JsonSerializer.Serialize(payload);
                var buffer = Encoding.UTF8.GetBytes(message);

                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );

                await Task.Delay(5000); 
            }
        }
        else
        {
            await _next(context); 
        }
    }

    private string GetRandomMotivationalSpeech()
    {
        var list = new[]
        {
            "Valmir wäre stolz auf dich! 🚀",
            "Denk dran: Valmir gibt niemals auf – du auch nicht!",
            "Mit Valmir-Mindset ist kein Ziel zu groß 💪",
            "Klar denken, sauber coden – wie Valmir es lehrt.",
            "Valmir hat gesagt: 'Ein Bug ist nur ein ungelöstes Feature.'",
            "Wenn du aufgibst, frag dich: Was würde Valmir tun?",
            "Valmir glaubt an dich. Immer."
        };


        return list[new Random().Next(list.Length)];
    }
}
