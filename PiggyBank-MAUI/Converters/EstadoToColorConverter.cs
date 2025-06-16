using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Converters
{
    class EstadoToColorConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string estado)
            {
                return estado == "Pendiente" ? Color.FromHex("#FEE2E2") : Color.FromHex("#BBF7D0");
            }
            return Color.FromHex("#FEE2E2"); // Default to Pendiente color
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
