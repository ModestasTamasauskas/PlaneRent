using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;

namespace PlaneRental.Admin.Support
{
    public class PlaneStatusConverter : IValueConverter
    {
        object IValueConverter.Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool currentlyRented = (bool)value;

            return (currentlyRented ? "Currently Rented" : "Available");
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}