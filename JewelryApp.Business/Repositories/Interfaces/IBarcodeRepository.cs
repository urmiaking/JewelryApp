using JewelryApp.Data.Models;

namespace JewelryApp.Business.Repositories.Interfaces;

public interface IBarcodeRepository
{
    Task<string> GetBarcodeAsync(Product product);
}