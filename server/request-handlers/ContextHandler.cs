using GptClient.Models;

namespace GPTClient.RequestHandlers
{
    public interface IContextHandler
    {
        public Task<List<string>> GetContextualInfo(Context context);
    }

    public class ContextHandler : IContextHandler
    {
        private readonly ILogger<ContextHandler> _logger;
        public ContextHandler(ILogger<ContextHandler> logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> GetContextualInfo(Context context)
        {
            _logger.LogInformation("fetching from context {context}", context);

            var markdownDirectory = Path.Combine(AppContext.BaseDirectory, "content", context.ToString());

            List<string> content = [];

            string[] markdownFiles = Directory.GetFiles(markdownDirectory, "*.md");

            foreach (var filePath in markdownFiles)
            {
                if (File.Exists(filePath))
                {
                    string markdownContent = await File.ReadAllTextAsync(filePath);
                    content.Add(markdownContent);
                }
            }

            return content;
        }
    }
}
