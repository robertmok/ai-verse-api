using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace AiAPI.Controllers
{
    public class OllamaChatRequest
    {
        public required string model { get; set; }
        public required List<Message> messages { get; set; }
        public required bool stream { get; set; }
    }
    public class Message
    {
        public required string role { get; set; } //system, user or assistant
        public required string content { get; set; }

    }

    public class ChatResponse
    {
        public bool done { get; set; }
        public required string model { get; set; }
        public required Message message { get; set; }
        public int prompt_eval_count { get; set; }
        public int eval_count { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AiController : ControllerBase
    {
        private readonly IConfiguration Configuration;
        private string llmHost;
        private HttpClient _client;

        public AiController(IConfiguration configuration)
        {
            Configuration = configuration;

            llmHost = Configuration["llmHost"] ?? "http://localhost:11434";
            System.Diagnostics.Debug.WriteLine(">>> " + llmHost);

            _client = new HttpClient();
        }

        [HttpPost("postStreamChat")]
        public async IAsyncEnumerable<ChatResponse?> PostOllamaStreamChatAsync(List<Message> history, string model)
        {
            //System.Diagnostics.Debug.WriteLine(">>>> " + history[0].content);
            var chatRequest = new OllamaChatRequest()
            {
                messages = history,
                model = model,
                stream = true
            };
            var request = new HttpRequestMessage(HttpMethod.Post, llmHost + "/api/chat")
            {
                Content = new StringContent(JsonConvert.SerializeObject(chatRequest), Encoding.UTF8, "application/json")
            };
            var completion = HttpCompletionOption.ResponseHeadersRead;

            var response = await _client.SendAsync(request, completion);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync();
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream)
            {
                string line = await reader.ReadLineAsync() ?? "";
                System.Diagnostics.Debug.WriteLine(line);

                var streamedResponse = JsonConvert.DeserializeObject<ChatResponse>(line);

                yield return streamedResponse;
            }
        }
    }
}
