using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using QuickCompose.Model;
using System.Net.Http.Json;
using System.Text.Json;

namespace QuickCompose.Pages
{
    [Authorize]
    public partial class Compose : IAsyncDisposable
    {
        private bool loading = false;
        private ChatGptConfig chatGptConfig;

        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IConfiguration Configuration { get; set; }
        [Inject]
        public IHttpClientFactory ClientFactory { get; set; }
        [Inject]
        public IOptions<ChatGptConfig> ChatGptConfig { get; set; }
        public IJSObjectReference JSModule { get; set; }
        public bool IncludeFullConversation { get; set; }
        public string? ChatGptResponse { get; set; }
        public string? Status { get; set; } = string.Empty;
        public string? CustomInstruction { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            chatGptConfig = ChatGptConfig.Value;
        }

        private async Task ReplyAll(string ChatGPTResponse)
        {
            JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Compose.razor.js");
            await JSModule.InvokeAsync<string>("replyAll", ChatGPTResponse);
        }

        private async Task GenerateResponse(string operation = "")
        {
            JSModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Pages/Compose.razor.js");

            try
            {
                Status = "Reading Email";
                loading = true;
                StateHasChanged();

                var outlookContext = await GetEmailData();

                var emailBody = bool.Parse(Configuration["EnableAnonymizer"]) ? await AnonymizeInput(outlookContext.EmailBody) : outlookContext.EmailBody;

                var userName = outlookContext.UserName;

                if (!string.IsNullOrEmpty(emailBody))
                {
                    Status = "Generating Suggestion";
                    StateHasChanged();
                    emailBody = emailBody.Length > 8000 ? emailBody[..8000] : emailBody;

                    ChatGptResponse = await GetChatGptResponse(emailBody, userName);
                }
            }
            catch (Exception ex)
            {
                Status = ex.Message;
                Console.WriteLine($"Error: {ex.Message}");
            }

            loading = false;
            StateHasChanged();
        }

        private async Task<OutlookContext?> GetEmailData()
        {
            var response = await JSModule.InvokeAsync<string>("getEmailData", IncludeFullConversation);
            return JsonSerializer.Deserialize<OutlookContext>(response);
        }

        public async Task<string?> GetChatGptResponse(string emailBody, string userName)
        {
            var client = ClientFactory.CreateClient("ChatGpt");

            var input = string.IsNullOrEmpty(CustomInstruction) ? chatGptConfig.Instruction + Environment.NewLine + emailBody
                : chatGptConfig.Instruction + CustomInstruction + Environment.NewLine + emailBody;

            var data = new ChatGptRequest
            {
                Prompt = string.Format(chatGptConfig.Prompt, userName, input),
            };

            var response = await client.PostAsJsonAsync("", data);
            var chatGptResponse = await response.Content.ReadFromJsonAsync<ChatGptResponse>();
            return chatGptResponse?.Choices.FirstOrDefault()?.Text;
        }

        public async Task<string?> AnonymizeInput(string emailBody)
        {
            var client = ClientFactory.CreateClient("Anonymizer");
            var response = await client.PostAsJsonAsync("Anonymizer", emailBody);

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