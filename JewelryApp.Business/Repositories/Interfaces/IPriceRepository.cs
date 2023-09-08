using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.Repositories.Interfaces;

public interface IPriceRepository
{
    Task<PriceDto> GetPriceAsync(CancellationToken token = default);
    Task AddPriceAsync(PriceDto priceDto, CancellationToken token = default);

    Task<LineChartDto> GetCaretChartDataAsync(CaretChartType caretChartType);
    Task UpdatePriceAsync(CancellationToken token = default);
}