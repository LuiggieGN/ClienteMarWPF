﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Globalization;


namespace ClienteMarWPFWin7.UI.Converters
{
    public class negativeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            SolidColorBrush brush = new SolidColorBrush(Colors.LimeGreen);
            decimal doubleValue = 0;
            decimal.TryParse(value?.ToString()??"0",NumberStyles.Currency,null, out doubleValue);

            if (doubleValue < 0)
                brush = new SolidColorBrush(Colors.Red);

            return brush;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
