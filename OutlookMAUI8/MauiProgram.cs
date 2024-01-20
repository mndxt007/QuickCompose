using Microsoft.Extensions.Logging;
using OutlookAddInMAUI8.Services;

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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddSingleton<OutlookCOM>();
            builder.Logging.AddDebug();

#endif

            return builder.Build();
        }
    }
}
