
using System.Windows;
using System.Windows.Controls;

using System.Windows.Input;
using System.Windows.Threading;

using System;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin
{
    public partial class CuadreLoginView : Window
    {
        public Window ParentWindow;
        public CuadreLoginView(Window parent)
        {
            InitializeComponent();

            ParentWindow = parent;

            SetFocusEnTiempo(PassGestorPin, TimeSpan.FromMilliseconds(777));
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

        private void CuandoTeclaSube_InputIdGestor(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var vm = DataContext as CuadreLoginViewModel;

                if (vm != null)
                {
                    vm.SeleccionarGestor?.Execute(new object[] { PassGestorPin, PassTokenTarjeta, CuadreLoginWindow });
                }
            }
        }

        private void CuandoTeclaSube_VentanaPrincipal(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                CancelarCuadre(sender, e);
            }
        }

        private void SetFocusEnTiempo(Control control, TimeSpan tiempo)
        {
            var timer = new DispatcherTimer();
            timer.Interval = tiempo;
            timer.Tick += (sender, args) => {
                timer.Stop();
                if (control != null)
                {
                    control.Focus();
                }
            };
            timer.Start();
        }








    }//fin de clase
}//fin de namespace
