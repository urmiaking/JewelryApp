using ErrorOr;
using JewelryApp.Shared.Abstractions;
using JewelryApp.Shared.Attributes;
using JewelryApp.Shared.Errors;

namespace JewelryApp.Application.AppServices;

[ScopedService<IReportService>]

public class ReportService : IReportService
{
    public async Task<ErrorOr<byte[]>> GetReportFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine("wwwroot", "reports", fileName);

        if (!File.Exists(filePath))
            return Errors.Report.FileNotFound;

        var fileBytes = await File.ReadAllBytesAsync(filePath, cancellationToken);
        return fileBytes;
    }
}