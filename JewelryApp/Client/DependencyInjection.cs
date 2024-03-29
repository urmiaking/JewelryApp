﻿using JewelryApp.Client.Security;
using JewelryApp.Client.Services;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using System.Reflection;
using JewelryApp.Shared.Extensions;
using JewelryApp.Client.Configurations;

namespace JewelryApp.Client;

public static class DependencyInjection
{
    public static IServiceCollection AddClient(this IServiceCollection services, IWebAssemblyHostEnvironment hostEnvironment)
    {
        services.DiscoverServices();
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddMudServices(opt =>
        {
            opt.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
        });
        services.AddBlazoredLocalStorage();

        services.AddScoped<AppAuthorizationMessageHandler>();
        services.AddAuthorizationCore();

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

    private static void DiscoverServices(this IServiceCollection services)
    {
        var assembliesToScan = new[]
        {
            Assembly.GetExecutingAssembly(),
            Assembly.Load("JewelryApp.Client.Services"),
            Assembly.Load("JewelryApp.Client")
        };

        services.DiscoverSingletonServices(assembliesToScan);
        services.DiscoverScopedServices(assembliesToScan);
    }
}