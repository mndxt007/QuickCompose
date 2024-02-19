using Microsoft.Extensions.Configuration;
using Blazored.LocalStorage;
using Azure.AI.OpenAI;


namespace OutlookMAUI8.Services
{
    public class OpenAIService
    {
        private string? url;
        private string? useDefault;
        private string? key;
        private string? deployment;
        private string? systemPrompt;
        private OpenAIClient _openAIClient;
        private ChatCompletionsOptions _completionOptions;
        private readonly IConfiguration _configuration;
        private readonly ILocalStorageService _localStorage;
        public Task Initialize { get; }
        public bool Initiated { get; private set; }
        public OutlookMAUI8.Model.Actions Categories { get; private set; } = new OutlookMAUI8.Model.Actions();

        public OpenAIService(IConfiguration configuration, ILocalStorageService localStorage)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _localStorage = localStorage;
            Initialize = InitializeOpenAI();

        }


        private ChatCompletionsOptions InitializeCompletionOptions()
        {
            return new ChatCompletionsOptions
            {
                MaxTokens = 800,
                Temperature = 0.6f,
                FrequencyPenalty = 0.0f,
                PresencePenalty = 0.0f,
                NucleusSamplingFactor = 0.95f
            };
        }

        public async Task<string> GetResponse(string input, bool rememberResponse = false)
        {
            await Initialize;
            if (Initiated)
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new ArgumentException("Input cannot be null or whitespace.", nameof(input));
                if(rememberResponse)
                {
                    _completionOptions.Messages.Clear();
                }
                _completionOptions.Messages.Add(new ChatMessage(ChatRole.User, input));
                ChatCompletions response = await _openAIClient.GetChatCompletionsAsync(deployment, _completionOptions);

                var responseMessage = response.Choices[0].Message;
                _completionOptions.Messages.Add(responseMessage);
                return responseMessage.Content;
            }
            return default;
        }
        private async Task InitializeOpenAI()
        {
            useDefault = await _localStorage.GetItemAsync<string>("OpenAI.UseDefault");
            if (string.IsNullOrEmpty(useDefault) || useDefault == "True")
            {
                url = _configuration.GetValue<string>("OpenAI:Endpoint") ?? throw new InvalidOperationException("Endpoint not configured");
                key = _configuration.GetValue<string>("OpenAI:Key") ?? throw new InvalidOperationException("Key not configured");
                deployment = _configuration.GetValue<string>("OpenAI:Deployment") ?? throw new InvalidOperationException("Deployment not configured");
            }
            else
            {
                url = await _localStorage.GetItemAsync<string>("OpenAI.Endpoint") ?? throw new InvalidOperationException("Endpoint not configured");
                key = await _localStorage.GetItemAsync<string>("OpenAI.Key") ?? throw new InvalidOperationException("Key not configured");
                deployment = await _localStorage.GetItemAsync<string>("OpenAI.Deployment") ?? throw new InvalidOperationException("Deployment not configured");
            }

            //Setup actions for prompts
            Categories.Category1 = await _localStorage.GetItemAsync<string>("Actions.Category1") ?? Categories.Category1;
            Categories.Category2 = await _localStorage.GetItemAsync<string>("Actions.Category2") ?? Categories.Category2;
            Categories.Category3 = await _localStorage.GetItemAsync<string>("Actions.Category3") ?? Categories.Category3;
            Categories.Category4 = await _localStorage.GetItemAsync<string>("Actions.Category4") ?? Categories.Category4;

            systemPrompt = _configuration.GetValue<string>("Prompts:SystemPrompt") ?? throw new InvalidOperationException("SystemPrompt not configured");
            var endpoint = new Uri(url);
            var credentials = new Azure.AzureKeyCredential(key);
            _openAIClient = new OpenAIClient(endpoint, credentials);
            _completionOptions = InitializeCompletionOptions();
            _completionOptions.Messages.Add(new ChatMessage(ChatRole.System, systemPrompt));
            Initiated = true;
        }
    }
}
