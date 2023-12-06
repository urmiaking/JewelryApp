using BlazingJewelry.Application.Services;
using JewelryApp.Business.Interfaces;
using JewelryApp.Business.Jobs;
using JewelryApp.Business.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NCrontab;
using System.Reflection;

namespace JewelryApp.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        services.AddAutoMapper(config =>
        {
            config.AddCustomMappingProfile();
            config.Advanced.BeforeSeal(configProvider =>
            {
                configProvider.CompileMappings();
            });
        }, assemblies);

        services.AddHttpClient<IPriceApiService, PriceApiService>();
        services.AddScoped<IPriceService, PriceService>();

        services.AddSignalR();

        var cronExpression = configuration.GetSection("CronExpression").Value
                             ?? throw new ArgumentException("There is no cron expression");

        services.AddCronJob<UpdatePriceJob>(cronExpression);

        return services;
    }

    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, string cronExpression)
        where T : class, ICronJob
    {
        var cron = CrontabSchedule.TryParse(cronExpression)
                   ?? throw new ArgumentException("Invalid cron expression", nameof(cronExpression));

        var entry = new CronRegistryEntry(typeof(T), cron);

        services.AddHostedService<CronScheduler>();
        services.TryAddSingleton<T>();
        services.AddSingleton(entry);

        return services;
    }
}
