namespace JewelryApp.Business.Jobs;

public interface ICronJob
{
    Task Run(CancellationToken token = default);
}