using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin
{
    public partial class CuadreLoginView : Window
    {
        public Window ParentWindow;

        public CuadreLoginView(Window parent)
        {
            InitializeComponent();
            ParentWindow = parent;
        }

        private void CancelarCuadre(object sender, RoutedEventArgs e)
        {
           Close();
        }

        private void CuadreLoginWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = ParentWindow.Left + (ParentWindow.Width - this.ActualWidth) / 2;
            this.Top = ParentWindow.Top + (ParentWindow.Height - this.ActualHeight) / 2;
        }
    }
}
