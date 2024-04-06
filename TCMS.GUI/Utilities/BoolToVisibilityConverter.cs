using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TCMS.GUI.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool && (bool)value)
            {
                return Visibility.Visible;
            }
            else
            {
                // Optionally, use parameter to control whether to collapse or hide
                // the control when the boolean is false.
                return parameter?.ToString().ToLower() == "hidden" ? Visibility.Hidden : Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility && (Visibility)value == Visibility.Visible;
        }
    }
}

