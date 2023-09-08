using JewelryApp.Business.Hubs;
using JewelryApp.Business.Repositories.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace JewelryApp.Business.Jobs;

public class UpdatePriceJob : ICronJob
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UpdatePriceJob> _logger;
    private readonly IHubContext<PriceHub> _hubContext;

    public UpdatePriceJob(IServiceProvider serviceProvider, ILogger<UpdatePriceJob> logger, IHubContext<PriceHub> hubContext)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _hubContext = hubContext;
    }

    public async Task Run(CancellationToken token = default)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var priceRepository = scope.ServiceProvider.GetRequiredService<IPriceRepository>();
        var price = await priceRepository.UpdatePriceAsync(token);

        var json = JsonConvert.SerializeObject(price);

        await _hubContext.Clients.All.SendAsync("PriceUpdate", json, cancellationToken: token);

        _logger.LogInformation($"Price Has been Updated in {DateTime.Now}");
    }
}