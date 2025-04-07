using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickCompose.Model
{
    public class ChatGptRequest
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 4000;

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.7;

        [JsonPropertyName("frequency_penalty")]
        public double FrequencyPenalty { get; set; } = 0;

        [JsonPropertyName("presence_penalty")]
        public double PresencePenalty { get; set; } = 0;

        [JsonPropertyName("top_p")]
        public double TopP { get; set; } = 0.95;

        [JsonPropertyName("stop")]
        public string[] Stop { get; set; } = new string[] { "<|im_end|>" };


        public static string Serialize(ChatGptRequest request)
        {
            var options = new JsonSerializerOptions { IgnoreNullValues = true };
            return JsonSerializer.Serialize(request, options);
        }

        public static ChatGptRequest Deserialize(string json)
        {
            return JsonSerializer.Deserialize<ChatGptRequest>(json);
        }
    }
}
