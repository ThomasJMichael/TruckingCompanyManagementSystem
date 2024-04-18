using System;
using System.Globalization;
using System.Windows.Data;

public class BooleanToYesNoConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Yes" : "No";
        }

        return "No"; // Default to "No" if value isn't a boolean
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            return stringValue.Equals("Yes", StringComparison.OrdinalIgnoreCase);
        }

        return false;
    }
}

