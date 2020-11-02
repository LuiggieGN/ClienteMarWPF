using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI.Modules.Configuracion
{
    /// <summary>
    /// Interaction logic for ConfiguracionView.xaml
    /// </summary>
    public partial class ConfiguracionView : UserControl
    {
        public ConfiguracionView()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int clave = Convert.ToInt32(txtClaveAutorizacion.Password);
            if (clave == 159753)
            {
                PanelAutoriacion.Visibility = Visibility.Collapsed;
                PanelConfiguracion.Visibility = Visibility.Visible;
            }
            else
            {
                PanelAutoriacion.Visibility = Visibility.Visible;
                PanelConfiguracion.Visibility = Visibility.Collapsed;
            }
        }
    }
}
