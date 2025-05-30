using Microsoft.Maui.Controls;
using System.Globalization;

namespace PiggyBank_MAUI.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isCompleted)
            {
                return isCompleted ? Color.FromArgb("#008259") : Color.FromArgb("#d20606"); // Verde-Principal or Rojo-Cancelar
            }
            return Color.FromArgb("#A0A0A0"); // Fallback
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}