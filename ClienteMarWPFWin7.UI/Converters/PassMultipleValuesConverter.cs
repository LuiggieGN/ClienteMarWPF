
using System.Windows.Data;
using System.Globalization;
using System;
using System.Linq;

namespace ClienteMarWPFWin7.UI.Converters
{
    public class PassMultipleValuesConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length == 0)
            {
                return null;
            }
            else
            {
                return values.ToArray();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
