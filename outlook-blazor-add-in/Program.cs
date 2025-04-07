using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using QuickCompose;
using QuickCompose.Graph;
using QuickCompose.Model;
using QuickCompose.Services;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
var Configuration = builder.Configuration;

builder.Services.Configure<ChatGptConfig>(builder.Configuration.GetSection("ChatGpt"));

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddHttpClient("ChatGpt", httpClient =>
{
    httpClient.BaseAddress = new Uri(Configuration["ChatGpt:Endpoint"]);
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    httpClient.DefaultRequestHeaders.Add("api-key", Configuration["ChatGpt:ApiKey"]);
});

builder.Services.AddHttpClient("Anonymizer", httpClient =>
{
    httpClient.BaseAddress = new Uri(Configuration["AnonymizerEndpoint"]);
});

builder.Services.AddScoped<IClipboardService, ClipboardService>();

builder.Services.AddMsalAuthentication<RemoteAuthenticationState, RemoteUserAccount>(options =>
{
    var scopes = builder.Configuration.GetValue<string>("GraphScopes");
    if (string.IsNullOrEmpty(scopes))
    { 
        scopes = "User.Read";
    }

    foreach (var scope in scopes.Split(';'))
    {
        Console.WriteLine($"Adding {scope} to requested permissions");
        options.ProviderOptions.DefaultAccessTokenScopes.Add(scope);
    }

    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.LoginMode = "redirect";
})
.AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, GraphUserAccountFactory>();

builder.Services.AddScoped<OpenAIService>();
builder.Services.AddScoped<GraphClientFactory>();

await builder.Build().RunAsync();
