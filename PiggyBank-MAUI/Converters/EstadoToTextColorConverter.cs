using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Converters
{
    class EstadoToTextColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string estado)
            {
                return estado == "Pendiente" ? Color.FromHex("#991B1B") : Color.FromHex("#166534");
            }
            return Color.FromHex("#991B1B"); // Default to Pendiente text color
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
