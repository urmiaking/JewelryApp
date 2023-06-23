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
        switch (productType)
        {
            case ProductType.Jewelry:
                return $"JF-{number:D5}";
            case ProductType.Gold:
                return $"GF-{number:D5}";
            default:
                throw new ArgumentOutOfRangeException(nameof(productType), productType, null);
        }
        
        
    }
}