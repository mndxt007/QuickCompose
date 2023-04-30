using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using QuickCompose.Model;
using System;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;

namespace QuickCompose.Pages
{
    public partial class PlayGround : IAsyncDisposable
    {
        private bool loading = false;
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IHttpClientFactory ClientFactory { get; set; }
        [Inject]
        public IConfiguration Configuration { get; set; }
        public IJSObjectReference? JSModule { get; set; }
        public string? ChatGptResponse { get; set; }
        public string? Status { get; set; } = string.Empty;
        public string? Prompt { get; set; }
        public string? OldConversation { get; set; } = string.Empty;
        private async Task GenerateResponse()
        {
            loading = true;

            var filteredPrompt = bool.Parse(Configuration["EnableAnonymizer"]) ? await AnonymizeInput(Prompt) : Prompt;

            try
            {
                ChatGptResponse = await GetChatGptResponse(filteredPrompt);
            }
            catch (Exception ex)
            {
                Status = ex.Message;
                Console.WriteLine($"Error: {ex.Message}");
            }

            OldConversation += $"Human:{Prompt}\nChatGPT:{ChatGptResponse}\n";
            loading = false;
            StateHasChanged();
        }

        public async Task<string?> GetChatGptResponse(string prompt)
        {
            var client = ClientFactory.CreateClient("ChatGpt");

            var data = new ChatGptRequest
            {
                Prompt = $"<|im_start|>\n Here is our previous convesation {OldConversation}<|im_end|>\n<|im_start|>\n Now answer the following question \n {prompt}<|im_end|><|im_start|>assistant",
            };

            var response = await client.PostAsJsonAsync("", data);
            var chatGptResponse = await response.Content.ReadFromJsonAsync<ChatGptResponse>();
            return chatGptResponse?.Choices.FirstOrDefault()?.Text;
        }

        public async Task<string?> AnonymizeInput(string input)
        {
            var client = ClientFactory.CreateClient("Anonymizer");
            var response = await client.PostAsJsonAsync("Anonymizer", input);

            var anonymizerResult = await response.Content.ReadAsStringAsync();
            return anonymizerResult;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (JSModule is not null)
            {
                await JSModule.DisposeAsync();
            }
        }
    }
}