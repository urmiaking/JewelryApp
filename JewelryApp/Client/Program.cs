using Blazored.LocalStorage;
using JewelryApp.Client;
using JewelryApp.Client.Security;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using System.Net.Http.Headers;
using JewelryApp.Client.Extensions;
using JewelryApp.Client.Services;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices(opt =>
{
    opt.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
});
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped<AppAuthorizationMessageHandler>();
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<IAccessTokenProvider, AppAccessTokenProvider>();
builder.Services.AddScoped<AuthenticationStateProvider, AppAuthStateProvider>();

builder.Services.AddHttpClient("AuthorizedClient", client =>
{
    var baseAddress = builder.HostEnvironment.BaseAddress;
    client.BaseAddress = new Uri(baseAddress);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
}).AddHttpMessageHandler<AppAuthorizationMessageHandler>();

builder.Services.AddHttpClient("UnauthorizedClient", client =>
{
    var baseAddress = builder.HostEnvironment.BaseAddress;
    client.BaseAddress = new Uri(baseAddress);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));
builder.Services.AddScoped<SignalRService>();

builder.ConfigureCulture();

await builder.Build().RunAsync();
