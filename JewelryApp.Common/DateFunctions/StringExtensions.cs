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

    public static string GenerateBarcode()
    {
        var random = new Random();

        var randomNumber = random.Next(10000, 100000) * 10;
        return randomNumber.ToString();
    }
}