using BlazingJewelry.Application.Services;
using JewelryApp.Business.Interfaces;
using JewelryApp.Business.Jobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NCrontab;
using System.Reflection;
using JewelryApp.Business.AppServices;
using JewelryApp.Business.Mapper;
using FluentValidation;
using JewelryApp.Business.Validators.Authentication;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Business.Validators.Products;

namespace JewelryApp.Business;

public static class DependencyInjection
{
    public static IServiceCollection AddBusiness(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
    {
        services.AddAutoMapper(typeof(MappingProfile));

        services.AddHttpClient<IPriceApiService, PriceApiService>();
        services.AddScoped<IPriceService, PriceService>();

        services.AddSignalR();

        var cronExpression = configuration.GetSection("CronExpression").Value
                             ?? throw new ArgumentException("There is no cron expression");

        services.AddCronJob<UpdatePriceJob>(cronExpression);

        services.AddScoped<IValidator<AuthenticationRequest>, AuthenticationRequestValidator>();
        services.AddScoped<IValidator<AddProductRequest>, AddProductValidator>();
        services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductValidator>();

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
