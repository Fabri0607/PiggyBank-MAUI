using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Converters
{
    public class DateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("d MMM yyyy", CultureInfo.InvariantCulture); // e.g., "15 May 2025"
            }
            return value?.ToString() ?? "Sin fecha";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}