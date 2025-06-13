using System.Globalization;
namespace PiggyBank_MAUI.Converters
{
    public class RoleToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!string.Equals(value?.ToString(), "assistant", StringComparison.OrdinalIgnoreCase))
                return "Tú";
            return "Piggy";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
