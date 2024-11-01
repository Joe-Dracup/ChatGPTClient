using System.Net;
using GptClient.Models;

namespace GPTClient.RequestHandlers
{
    public interface IGptChatApiClient
    {
        public Task<string> Request(string userMessage, List<Message> messages);
    }

    public class GptChatApiClient : IGptChatApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GptChatApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Request(string userMessage, List<Message> messages)
        {
            var client = _httpClientFactory.CreateClient("gptClient");

            var payload = new ChatRequest("gpt-4o-mini", messages);

            payload.Messages.Add(new Message("user", userMessage));

            var result = await client.PostAsJsonAsync(
                "v1/chat/completions",
                payload
            );

            if (!result.IsSuccessStatusCode)
            {
                var error = await result.Content.ReadAsStringAsync();
                throw new WebException($"API Error: {result.StatusCode} - {error}");
            }

            return await result.Content.ReadAsStringAsync();
        }
    }
}