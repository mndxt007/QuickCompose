using QuickCompose.Model;
using System.Net.Http.Json;

namespace QuickCompose.Services
{
    public class OpenAIService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public OpenAIService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string?> GetResponse(string prompt)
        {
            var client = _httpClientFactory.CreateClient("ChatGpt");

           

            var data = new ChatGptRequest
            {
                Prompt = prompt,
            };

            var response = await client.PostAsJsonAsync("", data);
            var chatGptResponse = await response.Content.ReadFromJsonAsync<ChatGptResponse>();
            return chatGptResponse?.Choices.FirstOrDefault()?.Text;
        }
    }
}
