using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Converters
{
    public class PercentageToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal percentage && parameter is string maxWidthStr && double.TryParse(maxWidthStr, out double maxWidth))
            {
                // Convert percentage (0-100) to width (0-maxWidth)
                return (double)(percentage / 100m * (decimal)maxWidth);
            }
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
