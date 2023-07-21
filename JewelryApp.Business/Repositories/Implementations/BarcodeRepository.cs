using JewelryApp.Business.Repositories.Interfaces;
using JewelryApp.Common.DateFunctions;
using JewelryApp.Data;
using JewelryApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace JewelryApp.Business.Repositories.Implementations;

public class BarcodeRepository : IBarcodeRepository
{
    private readonly AppDbContext _db;

    public BarcodeRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<string> GetBarcodeAsync(Product product)
    {
        var repeatedProduct = await _db.Products.AsNoTracking()
            .OrderByDescending(a => a.AddedDateTime)
            .FirstOrDefaultAsync(a => a.Name == product.Name);

        if (repeatedProduct is null) 
            return StringExtensions.GenerateBarcode(product.ProductType);

        var barcodeArray = repeatedProduct.BarcodeText.Split("-");

        var index = int.Parse(barcodeArray[2]);
        index++;
        barcodeArray[2] = index.ToString();
        var newBarcode = string.Join("-", barcodeArray);

        return newBarcode;

    }
}