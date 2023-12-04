using System.Globalization;

namespace JewelryApp.Common.DateFunctions;

public static class ShamsiDateTime
{
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

    public static string ToShamsiDateTimeString(this DateTime dateTime)
    {
        // Get the Persian calendar
        var persianCalendar = new PersianCalendar();

        // Convert the current date and time to a Persian date
        var persianYear = persianCalendar.GetYear(dateTime);
        var persianMonth = persianCalendar.GetMonth(dateTime);
        var persianDay = persianCalendar.GetDayOfMonth(dateTime);
        var persianDateString = $"{dateTime.Hour:00}:{dateTime.Minute:00}:{dateTime.Second:00} {persianYear:D4}/{persianMonth:D2}/{persianDay:D2} ";

        return persianDateString;
    }
}