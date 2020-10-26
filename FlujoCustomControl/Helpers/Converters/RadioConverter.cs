using System;
using System.Globalization;
using System.Windows.Data;

namespace FlujoCustomControl
{
    public class RadioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null) {
                return false;
            }

            string valorCheck  =  value.ToString();
            string valorTarget = parameter.ToString();

            bool r = valorCheck.Equals(valorTarget, StringComparison.InvariantCultureIgnoreCase);

            return r;
        }

        public object ConvertBack(object value , Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
            {
                return null;
            }

            bool usaValor = (bool)value;

            if (usaValor) {
                return parameter.ToString();                
            }

            return null;
        }

    }
}
