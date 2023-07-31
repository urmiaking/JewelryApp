using JewelryApp.Models.Dtos;

namespace JewelryApp.Business.Repositories.Interfaces;

public interface IPriceRepository
{
    Task<PriceDto> GetPriceAsync();
}