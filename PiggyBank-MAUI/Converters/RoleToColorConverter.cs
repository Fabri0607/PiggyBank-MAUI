using System.Globalization;

namespace PiggyBank_MAUI.Converters
{
    public class RoleToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var role = value as string;
            if (role == "user")
            {
                return Color.FromHex("#E1F5FE");
            }
            else
            {
                return Color.FromHex("#F0FFF0");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
