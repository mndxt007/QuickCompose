using Microsoft.Extensions.Logging;
using OutlookMAUI8.Services;

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
            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddSingleton<OutlookCOM>();
            builder.Services.AddScoped<ClipboardService>();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();

#endif
            return builder.Build();
        }
    }
}
