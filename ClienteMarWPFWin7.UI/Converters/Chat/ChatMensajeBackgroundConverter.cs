

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;


namespace ClienteMarWPFWin7.UI.Converters.Chat
{
    public class ChatMensajeBackgroundConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Application.Current.FindResource("EsMiMensajeBrush") : Application.Current.FindResource("NoEsMiMensajeBrush");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }


    }
}



