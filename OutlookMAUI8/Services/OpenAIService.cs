using Microsoft.Extensions.Configuration;
using Blazored.LocalStorage;
using Azure.AI.OpenAI;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using OutlookMAUI8.Model;


namespace OutlookMAUI8.Services
{
    public class OpenAIService
    {
        private string? url;
        public int emailbodyLen = 4000;
        private string? useDefault;
        private string? key;
        private string? deployment;
        private string? systemPrompt;
        private bool isLocal;
        private OpenAIClient _openAIClient;
        private ChatCompletionsOptions _completionOptions;
        private readonly IConfiguration _configuration;
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _client;
        private readonly LocalLLMModel _localLLMModel;
        public Task Initialize { get; }
        public bool Initiated { get; private set; }
        public OutlookMAUI8.Model.Actions Categories { get; private set; } = new OutlookMAUI8.Model.Actions();

        public OpenAIService(IConfiguration configuration, ILocalStorageService localStorage, HttpClient client)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _localStorage = localStorage;
            Initialize = InitializeOpenAI();
            _client = client;
            _client.Timeout = TimeSpan.FromMinutes(3);
            _localLLMModel = InitializeLocalCompletionOptions();
        }


        private ChatCompletionsOptions InitializeCompletionOptions()
        {
            return new ChatCompletionsOptions
            {
                MaxTokens = 1400,
                Temperature = 0.6f,
                FrequencyPenalty = 0.0f,
                PresencePenalty = 0.0f,
                NucleusSamplingFactor = 0.95f,
            };
        }
        private LocalLLMModel InitializeLocalCompletionOptions()
        {
            return new LocalLLMModel
            {
                model= "llama2",
                messages = new()
            };
        }

        public async Task<string> GetResponse(string input, bool rememberResponse = false)
        {
            await Initialize;
            if (Initiated)
            {
                if (string.IsNullOrWhiteSpace(input))
                    throw new ArgumentException("Input cannot be null or whitespace.", nameof(input));
                if (!isLocal)
                    return await GetResponseOpenAI(input, rememberResponse);
                else
                    return await GetResponseLocal(input, rememberResponse);
            }
                return default;
            }

        private async Task<string> GetResponseLocal(string input, bool rememberResponse)
        {
            if (rememberResponse)
            {
                _localLLMModel.messages.Clear();
            }
            _localLLMModel.messages.Add(new Messages()
            {
                role = "user",
                content = input
            });
            var content = new StringContent(JsonSerializer.Serialize(_localLLMModel), Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("v1/chat/completions",content);
            response.EnsureSuccessStatusCode();
            //var responseContent = await response.Content.ReadAsStringAsync();
            var responseNode = await JsonNode.ParseAsync(await response.Content.ReadAsStreamAsync());
            var message = responseNode["choices"][0]["message"]["content"].Deserialize<string>();
            return message;
           
        }

        public async Task<string> GetResponseOpenAI(string input, bool rememberResponse = false)
        {
            if (rememberResponse)
            {
                _completionOptions.Messages.Clear();
            }
            _completionOptions.Messages.Add(new ChatMessage(ChatRole.User, input));
            ChatCompletions response = await _openAIClient.GetChatCompletionsAsync(deployment, _completionOptions);
               var responseMessage = response.Choices[0].Message;
               _completionOptions.Messages.Add(responseMessage);
                return responseMessage.Content;
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
            _localLLMModel.model = deployment;
            _localLLMModel.messages.Add(new Messages()
            {
                role = "system",
                content = systemPrompt
            });
            _client.BaseAddress = endpoint;
            isLocal = key.ToLower() == "local";
            Initiated = true;
        }
    }

    class LocalLLMModel
    {
        public string model { get; set; }
        public List<Messages> messages { get; set; }

    }

    class Messages
    {
        public string role { get; set; }
        public string content { get; set; }
    }


}
