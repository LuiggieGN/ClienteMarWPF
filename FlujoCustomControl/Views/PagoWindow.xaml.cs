using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlujoCustomControl.Views
{
    /// <summary>
    /// Interaction logic for Pago.xaml
    /// </summary>
    public partial class PagoWindow : Window
    {
        public PagoWindow(string pMensaje, double pMontoAPagar)
        {
            InitializeComponent();
            txtNota.Text = pMensaje;
            lblTotal.Content = pMontoAPagar.ToString("N0");
        }

        private void Border_MouseMove(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void BtnCancel(object sender, RoutedEventArgs e)
        {
            CancelarProceso();
        }

        private void BtnProcesarPago(object sender, RoutedEventArgs e)
        {
            ProcesarPago();
        }



        private void PressTeclaOperation(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F6)
            {
                CancelarProceso();

            }
            else if (e.Key == Key.F5)
            {
                ProcesarPago();
            }

        }

        private void CancelarProceso()
        {
            this.Close();
        }

        private void ProcesarPago()
        {
           
        }
    }
}
