using System.Globalization;

namespace JewelryApp.Common.DateFunctions;

public static class ShamsiDateTime
{
    public static DateTime ParseShamsiDateTime(string shamsiDateTime)
    {
        // Split the Shamsi date and time components
        var shamsiParts = shamsiDateTime.Split(new[] { ' ', '-' });

        // Parse the year, month, and day components using the PersianCalendar class
        var pc = new PersianCalendar();
        var year = int.Parse(shamsiParts[0]);
        var month = int.Parse(shamsiParts[1]);
        var day = int.Parse(shamsiParts[2]);
        var georgianDate = pc.ToDateTime(year, month, day, 0, 0, 0, 0);

        // Parse the time component using the DateTime.Parse method
        var time = TimeSpan.Parse(shamsiParts[3]);

        // Combine the date and time components into a single DateTime object
        return georgianDate.Add(time);
    }

    public static string ToShamsiDateString(this DateTime dateTime)
    {
        // Get the Persian calendar
        var persianCalendar = new PersianCalendar();

        // Convert the current date and time to a Persian date
        var persianYear = persianCalendar.GetYear(dateTime);
        var persianMonth = persianCalendar.GetMonth(dateTime);
        var persianDay = persianCalendar.GetDayOfMonth(dateTime);
        var persianDateString = $"{persianYear:D4}/{persianMonth:D2}/{persianDay:D2}";

        return persianDateString;
    }
    public static DateTime ToGregorianDateTime(this string shamsiDateTime)
    {
        try
        {
            var persianCalendar = new PersianCalendar();

            var shamsiDateParts = shamsiDateTime.Split('/');
            var year = int.Parse(shamsiDateParts[0]);
            var month = int.Parse(shamsiDateParts[1]);
            var day = int.Parse(shamsiDateParts[2]);

            var gregorianDateTime = persianCalendar.ToDateTime(year, month, day, 0, 0, 0, 0);
            return gregorianDateTime;
        }
        catch
        {
            return DateTime.MinValue;
        }
        
    }
}