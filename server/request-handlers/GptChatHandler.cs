using GptClient.Models;

namespace GPTClient.RequestHandlers
{
    public interface IGptChatHandler
    {
        public Task<string> Chat(GptRequest gptRequest);
    }

    public class GptChatHandler : IGptChatHandler
    {
        private readonly IGptChatApiClient _gptChatApiClient;
        private readonly IContextHandler _contextHandler;

        public GptChatHandler(IGptChatApiClient gptChatApiClient, IContextHandler contextHandler)
        {
            _gptChatApiClient = gptChatApiClient;
            _contextHandler = contextHandler;
        }

        public async Task<string> Chat(GptRequest gptRequest)
        {
            if (gptRequest is null || string.IsNullOrEmpty(gptRequest.message))
                throw new ArgumentNullException("must populate GPT request with all arguments");

            var messageContent = await _contextHandler.GetContextualInfo(gptRequest.Context);

            var systemMessages = messageContent.Select(x => new Message("system", x)).ToList();

            return await _gptChatApiClient.Request(gptRequest.message, systemMessages);
        }
    }
}