namespace JewelryApp.Client.Services;

public class BarcodeService
{
    public static string Generate()
    {
        var random = new Random();

        var randomNumber = random.Next(10000, 100000) * 10;
        return randomNumber.ToString();
    }

    public static string IncrementByOne(string barcode)
    {
        var newBarcode = int.Parse(barcode);
        newBarcode += 1;
        return newBarcode.ToString();
    }
}
