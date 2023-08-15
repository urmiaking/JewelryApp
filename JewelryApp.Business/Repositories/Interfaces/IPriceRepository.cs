using JewelryApp.Common.Enums;
using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.Repositories.Interfaces;

public interface IPriceRepository
{
    Task<PriceDto> GetPriceAsync();
    Task AddPriceAsync(PriceDto priceDto);

    Task<LineChartDto> GetCaretChartDataAsync(CaretChartType caretChartType);
}