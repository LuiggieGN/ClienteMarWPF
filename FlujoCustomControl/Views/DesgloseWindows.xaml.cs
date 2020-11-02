using FlujoCustomControl.Code.BussinessLogic;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for DesgloseWindows.xaml
    /// </summary>
    public partial class DesgloseWindows : Window
    {

        public event Action<string> SetMontoContado;
        public event Action<int> SetMontoFaltante;
        
        private CalcularDesgloseLogic calc;
        private int result { get; set; }

        public DesgloseWindows()
        {
            InitializeComponent();
            lblTotalContado.Text = "$0.00";
            calc = new CalcularDesgloseLogic();
        }


       private void EscribirNumeros(object sender, KeyEventArgs e)
       {

            if (  (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9 ) || ( e.Key == Key.Back))
            {
                 e.Handled = false;
                 calc.Calcular(txtm1.Text, txtm5.Text, txtm10.Text, txtm25.Text, txtm50.Text, txtm100.Text, txtm200.Text, txtm500.Text, txtm1000.Text, txtm2000.Text);
                 result = calc.result;
                 lblTotalContado.Text =   result.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            }
            else
            {
                e.Handled = true;
            }

            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;


                if (s != null)
                {
                    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                }

                e.Handled = true;
            }

        }


        private void BorrarNumeros(object sender, KeyEventArgs e)
        {

            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;

            }



        }


        private void LimpiarCampos(object sender, RoutedEventArgs e)
        {
            LimpiarTextBox();
        }


        private void CerrarVentana(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProcesarClick(object sender, RoutedEventArgs e)
        {

            SetMontoContado?.Invoke(result.ToString());
            SetMontoFaltante?.Invoke(result);



            //CuadreWindows.TotalContado = result;
            this.Close();
        }

        private void LimpiarTextBox()
        {
            txtm1.Clear();
            txtm5.Clear();
            txtm10.Clear();
            txtm25.Clear();
            txtm50.Clear();
            txtm100.Clear();
            txtm200.Clear();
            txtm500.Clear();
            txtm1000.Clear();
            txtm2000.Clear();
            lblTotalContado.Text = "$0.00";
            result = 0;
        }


    }
}
