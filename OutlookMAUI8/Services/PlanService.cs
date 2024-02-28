using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Outlook;
using OutlookMAUI8.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OutlookMAUI8.Services
{
    public class PlanService
    {
        private readonly IConfiguration _configuration;
        public required IServiceProvider _serviceProvider;
        [Inject]
        private OfficeService? _officeService { get; set; }
        private readonly ILocalStorageService _localStorage;
        protected OutlookMAUI8.Model.Actions Categories { get; set; } = new OutlookMAUI8.Model.Actions();

        public PlanService(IConfiguration configuration, IServiceProvider serviceProvider, ILocalStorageService localStorage)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _localStorage = localStorage;
        }

        public async Task<PlanModel> GeneratePlan(EmailContext message)
        {
            try
            {
                if (!string.IsNullOrEmpty(message?.EmailBody))
                {
                    var openAIService = _serviceProvider.GetRequiredService<OpenAIService>();
                    var prompt = await ConstructPlanPromptAsync(message);
                    if (!string.IsNullOrEmpty(prompt))
                    {
                        var jsonResponse = await openAIService.GetResponse(prompt);
                        await Console.Out.WriteLineAsync(jsonResponse);
                        var plan = JsonSerializer.Deserialize<PlanModel>(jsonResponse);
                        plan.Message = message;
                        return plan;
                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("Prompt is empty or invalid.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                await Console.Out.WriteLineAsync($"Error: {ex}");
            }

            return default!;
        }

        private async Task<string> ConstructPlanPromptAsync(EmailContext message)
        {
            var planPrompt = _configuration.GetValue<string>("Prompts:Plan")!;
            Categories.Category1 = await _localStorage.GetItemAsync<string>("Actions.Category1") ?? Categories.Category1;
            Categories.Category2 = await _localStorage.GetItemAsync<string>("Actions.Category2") ?? Categories.Category2;
            Categories.Category3 = await _localStorage.GetItemAsync<string>("Actions.Category3") ?? Categories.Category3;
            Categories.Category4 = await _localStorage.GetItemAsync<string>("Actions.Category4") ?? Categories.Category4;
            if (message != null && !string.IsNullOrEmpty(planPrompt))
            {
                bool isSentItem = string.Equals(message.SenderEmail ?? "", message.UserEmail ?? "", StringComparison.OrdinalIgnoreCase);
                return string.Format(planPrompt, message.UserName, isSentItem ? "Myself" : message?.UserEmail, isSentItem ? "Sent Item" : "Inbox", message?.EmailBody, Categories.Category1, Categories.Category2, Categories.Category3, Categories.Category4);
            }

            return string.Empty;
        }

  
    }
}
