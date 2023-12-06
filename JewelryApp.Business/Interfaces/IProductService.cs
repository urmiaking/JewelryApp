using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.InvoiceDtos;
using JewelryApp.Models.Dtos.ProductDtos;

namespace JewelryApp.Business.Interfaces;

public interface IProductService
{
    Task<Product> SetProductAsync(ProductDto productDto);
    Task<IEnumerable<ProductTableItemDto>> GetProductsAsync(int page, int pageSize, string sortDirection, string sortLabel,
        string searchString, CancellationToken cancellationToken);
    Task<DeleteResult> DeleteProductAsync(int id, CancellationToken cancellationToken);
    Task<int> GetTotalProductsCount(CancellationToken cancellationToken);
    Task<InvoiceItemDto> GetProductByBarcodeAsync(string barcodeText);
}