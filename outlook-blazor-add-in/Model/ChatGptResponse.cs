using System.Text.Json;
using System.Text.Json.Serialization;

namespace QuickCompose.Model
{
	public class ChatGptResponse
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("object")]
		public string Object { get; set; }

		[JsonPropertyName("created")]
		public long Created { get; set; }

		[JsonPropertyName("model")]
		public string Model { get; set; }

		[JsonPropertyName("choices")]
		public Choice[] Choices { get; set; }

		[JsonPropertyName("usage")]
		public Usage Usage { get; set; }

		public static ChatGptRequest Deserialize(string json)
		{
			return JsonSerializer.Deserialize<ChatGptRequest>(json);
		}
	}

	public class Choice
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }

		[JsonPropertyName("index")]
		public int Index { get; set; }

		[JsonPropertyName("finish_reason")]
		public string FinishReason { get; set; }

		[JsonPropertyName("logprobs")]
		public object LogProbs { get; set; }
	}

	public class Usage
	{
		[JsonPropertyName("completion_tokens")]
		public int CompletionTokens { get; set; }

		[JsonPropertyName("prompt_tokens")]
		public int PromptTokens { get; set; }

		[JsonPropertyName("total_tokens")]
		public int TotalTokens { get; set; }
	}
}
