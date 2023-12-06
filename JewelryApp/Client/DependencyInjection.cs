using JewelryApp.Client.Security;
using JewelryApp.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;

namespace JewelryApp.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClient(this IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment)
    {
        services.AddMudServices(opt =>
        {
            opt.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
        });
        services.AddBlazoredLocalStorage();

        services.AddScoped<AppAuthorizationMessageHandler>();
        services.AddAuthorizationCore();

        services.AddScoped<IAccessTokenProvider, AppAccessTokenProvider>();
        services.AddScoped<AuthenticationStateProvider, AppAuthStateProvider>();

        services.AddHttpClient("AuthorizedClient", client =>
        {
            var baseAddress = hostEnvironment.BaseAddress;
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }).AddHttpMessageHandler<AppAuthorizationMessageHandler>();

        services.AddHttpClient("UnauthorizedClient", client =>
        {
            var baseAddress = hostEnvironment.BaseAddress;
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        });

        services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("AuthorizedClient"));
        services.AddScoped<SignalRService>();

        return services;
    }
}