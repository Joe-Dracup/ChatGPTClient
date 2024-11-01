using System.Text.Json;
using System.Text.Json.Serialization;

namespace GptClient.Models
{
    public class ChatRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; }

        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }

        [JsonPropertyName("temperature")]
        public double? Temperature { get; set; }

        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }

        [JsonPropertyName("top_p")]
        public double? TopP { get; set; }

        [JsonPropertyName("frequency_penalty")]
        public double? FrequencyPenalty { get; set; }

        [JsonPropertyName("presence_penalty")]
        public double? PresencePenalty { get; set; }

        public ChatRequest(string model, List<Message> messages, double? temperature = null, int? maxTokens = null, double? topP = null, double? frequencyPenalty = null, double? presencePenalty = null)
        {
            Model = model;
            Messages = messages;
            Temperature = temperature;
            MaxTokens = maxTokens;
            TopP = topP;
            FrequencyPenalty = frequencyPenalty;
            PresencePenalty = presencePenalty;
        }

        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}