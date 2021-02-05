using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI.Modules.Configuracion
{
    /// <summary>
    /// Interaction logic for ConfiguracionView.xaml
    /// </summary>
    public partial class ConfiguracionView : Window
    {

        public Window ParentWindow;

        public ConfiguracionView(Window parent, object context)
        {
            DataContext = context;

            InitializeComponent();
            
            ParentWindow = parent;

        }

        private void ConfiguracionWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = ParentWindow.Left + (ParentWindow.Width - this.ActualWidth) / 2;
            this.Top = ParentWindow.Top + (ParentWindow.Height - this.ActualHeight) / 2;
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
