using Microsoft.AspNetCore.Mvc.Infrastructure;
using JewelryApp.Api.Common.Errors;
using JewelryApp.Core.Constants;
using JewelryApp.Core.Interfaces;
using JewelryApp.Core.Settings;
using JewelryApp.Core.Utilities;
using JewelryApp.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using JewelryApp.Api.Validators.Authentication;
using JewelryApp.Api.Validators.Products;
using JewelryApp.Shared.Requests.Authentication;
using JewelryApp.Shared.Requests.Products;
using JewelryApp.Shared.Requests.Customer;
using JewelryApp.Api.Validators.Customers;
using JewelryApp.Api.Validators.InvoiceItems;
using JewelryApp.Shared.Requests.InvoiceItems;
using JewelryApp.Shared.Requests.Invoices;
using JewelryApp.Api.Validators.Invoices;
using JewelryApp.Api.Validators.ProductCategories;
using JewelryApp.Shared.Requests.ProductCategories;

namespace JewelryApp.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();
        services.AddRazorPages();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
        services.AddSignalR();
        services.AddHttpContextAccessor();

        services.AddSingleton<ProblemDetailsFactory, AppProblemDetailsFactory>();

        services.AddScoped<IValidator<AuthenticationRequest>, AuthenticationRequestValidator>();
        services.AddScoped<IValidator<AddProductRequest>, AddProductRequestValidator>();
        services.AddScoped<IValidator<UpdateProductRequest>, UpdateProductRequestValidator>();
        services.AddScoped<IValidator<AddCustomerRequest>, AddCustomerRequestValidator>();
        services.AddScoped<IValidator<UpdateCustomerRequest>, UpdateCustomerRequestValidator>();
        services.AddScoped<IValidator<AddInvoiceItemRequest>, AddInvoiceItemRequestValidator>();
        services.AddScoped<IValidator<UpdateInvoiceItemRequest>, UpdateInvoiceItemRequestValidator>();
        services.AddScoped<IValidator<AddInvoiceRequest>, AddInvoiceRequestValidator>();
        services.AddScoped<IValidator<UpdateInvoiceRequest>, UpdateInvoiceRequestValidator>();
        services.AddScoped<IValidator<AddProductCategoryRequest>, AddProductCategoryRequestValidator>();
        services.AddScoped<IValidator<UpdateProductCategoryRequest>, UpdateProductCategoryRequestValidator>();

        return services;
    }

    public static IApplicationBuilder InitializeDatabase(this IApplicationBuilder app)
    {
        Assert.NotNull(app, nameof(app));

        using var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var appDbContext = scope.ServiceProvider.GetService<AppDbContext>();

        var retryCount = 0;

        try
        {
            appDbContext?.Database.Migrate();

            var dataInitializers = scope.ServiceProvider.GetServices<IDbInitializer>();

            foreach (var dataInitializer in dataInitializers)
            {
                dataInitializer.Initialize();
            }
        }
        catch (Exception e)
        {
            if (retryCount < AppConstants.RetryCount)
            {
                retryCount++;
                var loggerFactory = scope.ServiceProvider.GetService<ILoggerFactory>();
                var logger = loggerFactory?.CreateLogger<Program>();
                logger?.LogError(e.Message);
                InitializeDatabase(app);
            }
            throw;
        }

        return app;
    }
}
