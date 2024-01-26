using ErrorOr;

namespace JewelryApp.Shared.Abstractions;

public interface IReportService
{
    Task<ErrorOr<byte[]>> GetReportFileAsync(string fileName, CancellationToken cancellationToken = default);
}