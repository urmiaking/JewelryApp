namespace JewelryApp.Application.Jobs;

public interface ICronJob
{
    Task Run(CancellationToken token = default);
}