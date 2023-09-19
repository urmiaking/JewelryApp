using JewelryApp.Common.Enums;

namespace JewelryApp.Common.DateFunctions;

public static class StringExtensions
{
    public static string ExtractTime(this string dateString)
    {
        DateTime date = ShamsiDateTime.ParseShamsiDateTime(dateString);
        TimeSpan time = date.TimeOfDay;
        return time.ToString(@"hh\:mm\:ss");
    }

    public static string GenerateBarcode(ProductType productType)
    {
        var random = new Random();
        var number = random.Next(0, 100000);
        return productType switch
        {
            ProductType.Jewelry => $"JF-{number:D5}-0",
            ProductType.Gold => $"GF-{number:D5}-0",
            _ => throw new ArgumentOutOfRangeException(nameof(productType), productType, null)
        };
    }
}