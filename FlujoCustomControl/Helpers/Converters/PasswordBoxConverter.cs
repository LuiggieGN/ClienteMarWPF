using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace FlujoCustomControl
{

    public class GroupedPasswordBox
    {
       public PasswordBox SecurePin     { get; set; }
       public PasswordBox SecureCedula { get; set; }
    }

    public class PasswordBoxConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            GroupedPasswordBox group = new GroupedPasswordBox();

            group.SecurePin =     (PasswordBox)values[0];
            group.SecureCedula = (PasswordBox)values[1];

            return group;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
