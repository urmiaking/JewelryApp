using JewelryApp.Common.Enums;
using JewelryApp.Data.Models;
using JewelryApp.Models.Dtos.Invoice;
using JewelryApp.Models.Dtos.Product;

namespace JewelryApp.Business.AppServices;

public interface IProductService
{
    Task<Product> SetProductAsync(Models.Dtos.Product.ProductDto productDto);
    Task<IEnumerable<ProductTableItemDto>> GetProductsAsync(int page, int pageSize, string sortDirection, string sortLabel, string searchString, CancellationToken cancellationToken);
    Task<DeleteResult> DeleteProductAsync(int id, CancellationToken cancellationToken);
    Task<int> GetTotalProductsCount(CancellationToken cancellationToken);
    Task<InvoiceItemDto> GetProductByBarcodeAsync(string barcodeText);
}