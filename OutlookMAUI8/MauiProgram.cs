using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OutlookMAUI8.Services;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Reflection;

namespace OutlookMAUI8
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                })
                ;
            builder.Services.AddLogging(config =>
            {
                config.AddFilter("Microsoft.AspNetCore.Components.WebView", LogLevel.Trace);
                config.SetMinimumLevel(LogLevel.Trace);
                config.AddDebug();
            });
            var config = "OutlookMAUI8.appsettings.json";
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream(config);
            builder.Configuration.AddJsonStream(stream);
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<OfficeService>();
            builder.Services.AddScoped<ClipboardService>();
            builder.Services.AddTransient<OpenAIService>();
            builder.Services.AddTransient<PlanService>();
            builder.Services.AddBlazoredLocalStorage();


            //builder.Services.AddHttpClient("Anonymizer", httpClient =>
            //{
            //    httpClient.BaseAddress = new Uri(Configuration.GetValue<string>("AnonymizerEndpoint")!);
            //});
            builder.Services.AddHttpClient();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();

#endif
            return builder.Build();
        }
    }
}
