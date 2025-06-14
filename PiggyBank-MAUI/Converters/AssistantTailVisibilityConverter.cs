using System;
using System.Globalization;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Converters
{
    public class AssistantTailVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string role)
            {
                return string.Equals(role, "assistant", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
