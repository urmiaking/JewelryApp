namespace JewelryApp.Common.Utilities;

public static class IntExtensions
{
    public static int RemoveLeadingZero(this int value)
    {
        var numberString = value.ToString();
        numberString = numberString.Remove(numberString.Length - 1);
        return int.Parse(numberString);
    }
}