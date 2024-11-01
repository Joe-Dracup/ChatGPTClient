using GptClient;

namespace GPTClient.RequestHandlers
{
    public interface IGptChatHandler
    {
        public Task<string> Chat(GptRequest gptRequest);
    }

    public class GptChatHandler : IGptChatHandler
    {
        private readonly IGptChatApiClient _gptChatApiClient;

        public GptChatHandler(IGptChatApiClient gptChatApiClient)
        {
            _gptChatApiClient = gptChatApiClient;
        }

        public async Task<string> Chat(GptRequest gptRequest)
        {
            if (gptRequest is null || string.IsNullOrEmpty(gptRequest.message))
                throw new ArgumentNullException("must populate GPT request with all arguments");

            return await _gptChatApiClient.Request(gptRequest.message);
        }
    }
}