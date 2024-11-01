using System.Net;

namespace GPTClient.RequestHandlers
{
    public interface IGptChatApiClient
    {
        public Task<string> Request(string message);
    }

    public class GptChatApiClient : IGptChatApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GptChatApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Request(string message)
        {
            var client = _httpClientFactory.CreateClient("gptClient");

            var payload = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                new { role = "user", content = message }
            }
            };

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