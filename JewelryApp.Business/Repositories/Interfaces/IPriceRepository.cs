using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos.Common;

namespace JewelryApp.Business.Repositories.Interfaces;

public interface IPriceRepository
{
    Task<PriceDto> GetPriceAsync(CancellationToken token = default);

    Task<LineChartDto> GetCaretChartDataAsync(CaretChartType caretChartType);
    Task<PriceDto> UpdatePriceAsync(CancellationToken token = default);
}