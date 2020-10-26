using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace FlujoCustomControl
{
    public class CellTipoMovimientoConverter  : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string Categoria = "" + values[0];
            double EntradaOSalida = (double)values[1];

            if (Categoria.Equals("Ingreso") && EntradaOSalida > 0)
            {
                return Brushes.Green;
            }
            else if (Categoria.Equals("Egreso") && EntradaOSalida > 0)
            {
                return Brushes.Red;
            }
            else
            {
                return DependencyProperty.UnsetValue;

            }

        }

       public  object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
