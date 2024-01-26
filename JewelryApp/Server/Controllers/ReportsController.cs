using JewelryApp.Shared.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace JewelryApp.Api.Controllers;

public class ReportsController : ApiController
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetReportFile(string fileName, CancellationToken cancellationToken)
    {
        var result = await _reportService.GetReportFileAsync(fileName, cancellationToken);

        return result.Match(Ok, Problem);
    }
}

