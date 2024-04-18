using System;
using System.Globalization;
using System.Windows.Data;
using TCMS.Common.enums;

public class ShippingTypeToLabelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ShipmentDirection shippingType)
        {
            switch (shippingType)
            {
                case ShipmentDirection.Inbound:
                    return "Source Company";
                case ShipmentDirection.Outbound:
                    return "Destination Company";
                default:
                    return "Unknown Type";
            }
        }
        else if (value is string strValue)
        {
            return strValue.Equals("Incoming", StringComparison.OrdinalIgnoreCase) ? "Source Company" : "Destination Company";
        }

        return "Invalid Type";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException("Converting from string to ShippingType is not supported.");
    }
}

