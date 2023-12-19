using System.Reflection;
using JewelryApp.Application.AppServices;
using JewelryApp.Application.Interfaces;
using JewelryApp.Application.Jobs;
using JewelryApp.Application.Mapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NCrontab;

namespace JewelryApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddHttpClient<IPriceApiService, PriceApiService>();
        services.AddSignalR();

        services.AddCronJob<UpdatePriceJob>(configuration);

        return services;
    }

    public static IServiceCollection AddCronJob<T>(this IServiceCollection services, IConfiguration configuration)
        where T : class, ICronJob
    {
        var cronExpression = configuration.GetSection("CronExpression").Value
                             ?? throw new ArgumentException("There is no cron expression");

        var cron = CrontabSchedule.TryParse(cronExpression)
                   ?? throw new ArgumentException("Invalid cron expression", nameof(cronExpression));

        var entry = new CronRegistryEntry(typeof(T), cron);

        services.AddHostedService<CronScheduler>();
        services.TryAddSingleton<T>();
        services.AddSingleton(entry);

        return services;
    }
}
