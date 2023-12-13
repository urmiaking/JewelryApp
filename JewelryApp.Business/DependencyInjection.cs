using System.Reflection;
using FluentValidation;
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

        services.AddScoped<IRefreshTokenService, RefreshTokenService>();
        services.AddHttpClient<IPriceApiService, PriceApiService>();
        services.AddScoped<IPriceService, PriceService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IInvoiceService, InvoiceService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IInvoiceItemService, InvoiceItemService>();

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
