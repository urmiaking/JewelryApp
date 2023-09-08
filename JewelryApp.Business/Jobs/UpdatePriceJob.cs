using JewelryApp.Business.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace JewelryApp.Business.Jobs;

public class UpdatePriceJob : ICronJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UpdatePriceJob> _logger;

    public UpdatePriceJob(IServiceProvider serviceProvider, ILogger<UpdatePriceJob> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task Run(CancellationToken token = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var priceRepository = scope.ServiceProvider.GetRequiredService<IPriceRepository>();
        await priceRepository.UpdatePriceAsync(token);
        _logger.LogInformation("Price Has been Updated");
    }
}