using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace JewelryApp.Common.Enums;

public static class EnumExtensions
{
    public static IEnumerable<T> GetEnumValues<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        return Enum.GetValues(input.GetType()).Cast<T>();
    }

    public static IEnumerable<T> GetEnumFlags<T>(this T input) where T : struct
    {
        if (!typeof(T).IsEnum)
            throw new NotSupportedException();

        foreach (var value in Enum.GetValues(input.GetType()))
            if (((input as Enum)!).HasFlag((value as Enum)!))
                yield return (T)value;
    }

    public static string ToDisplay(this Enum value, DisplayProperty property = DisplayProperty.Name)
    {
        var attribute = value.GetType().GetField(value.ToString())!
            .GetCustomAttributes<DisplayAttribute>(false).FirstOrDefault();

        if (attribute == null)
            return value.ToString();

        var propValue = attribute.GetType().GetProperty(property.ToString())?.GetValue(attribute, null);
        return propValue?.ToString();
    }

    public static Dictionary<int, string> ToDictionary(this Enum value)
    {
        return Enum.GetValues(value.GetType()).Cast<Enum>().ToDictionary(Convert.ToInt32, q => ToDisplay(q));
    }

    public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
        where TAttribute : Attribute
    {
        var type = enumValue.GetType();
        var memberInfo = type.GetMember(enumValue.ToString());
        var attributes = memberInfo[0].GetCustomAttributes(typeof(TAttribute), false);
        return attributes.Length > 0 ? (TAttribute)attributes[0] : null;
    }
}

public enum DisplayProperty
{
    Description,
    GroupName,
    Name,
    Prompt,
    ShortName,
    Order
}