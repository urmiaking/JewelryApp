namespace JewelryApp.Client.Extensions;

public static class StringExtensions
{
    public static string ToCurrency(this double value)
    {
        return value.ToString("C0").Replace("ریال", "") + " تومان";
    }
}