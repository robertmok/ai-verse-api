using AiAPI.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace SignalRWebpack.Hubs;

public class AiChatHub : Hub
{
    private readonly IConfiguration Configuration;
    private AiController controller;

    public AiChatHub(IConfiguration configuration)
    {
        Configuration = configuration;
        controller = new AiController(Configuration);
    }

    public async Task SendAiMessage(List<Message> message, string model)
    {
        var aiMessage = controller.PostOllamaStreamChatAsync(message, model);

        if (aiMessage != null) {
            await foreach (ChatResponse? response in aiMessage)
            {
                System.Diagnostics.Debug.WriteLine(response);
                await Clients.All.SendAsync("ReceiveAiMessage", response);
            }
        }
    }
}