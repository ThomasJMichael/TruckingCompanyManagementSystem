using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace TCMS.GUI.Utilities
{
    public class StringToDecimalConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion from decimal to string
            if (value is decimal decimalValue)
            {
                // Optionally format the string here if you want to control decimal places, etc.
                return decimalValue.ToString("G29", culture); // "G29" prevents trailing zeros
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion from string to decimal
            string stringValue = value as string;
            if (decimal.TryParse(stringValue, NumberStyles.Any, culture, out decimal result))
            {
                return result;
            }
            return null; // Return null if conversion fails
        }
    }
}
