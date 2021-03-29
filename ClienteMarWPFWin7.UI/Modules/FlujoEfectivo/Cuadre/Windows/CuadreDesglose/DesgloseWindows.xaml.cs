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

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreDesglose
{
    public partial class DesgloseWindows : Window
    {
        public Window ParentWindow;
        private CalcularDesgloseLogic calc;
        public int? Resultado { get; set; }

        public DesgloseWindows(Window parentWindow)
        {
            InitializeComponent();
            lblTotalContado.Text = "$0.00";
            Resultado = 0;
            calc = new CalcularDesgloseLogic();
            ParentWindow = parentWindow;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Left = ParentWindow.Left + (ParentWindow.Width - ActualWidth) / 2;
            Top = ParentWindow.Top + (ParentWindow.Height - ActualHeight) / 2;
        }

        private void EscribirNumeros(object sender, KeyEventArgs e)
        {

            if ((e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Back))
            {
                e.Handled = false;
                calc.Calcular(txtm1.Text, txtm5.Text, txtm10.Text, txtm25.Text, txtm50.Text, txtm100.Text, txtm200.Text, txtm500.Text, txtm1000.Text, txtm2000.Text);
                Resultado = calc.result;
                lblTotalContado.Text = Resultado?.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))??string.Empty;
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
            LimpiarTextBox();
            Resultado = null;
            Close();
        }

        private void ProcesarClick(object sender, RoutedEventArgs e)
        { 
            Close();
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
            Resultado = 0;
        }


    }// fin de clase



    class CalcularDesgloseLogic
    {
        private const int M1 = 1;
        private const int M5 = 5;
        private const int M10 = 10;
        private const int M25 = 25;
        private const int M50 = 50;
        private const int M100 = 100;
        private const int M200 = 200;
        private const int M500 = 500;
        private const int M1000 = 1000;
        private const int M2000 = 2000;

        public int result { get; set; }

        public void Calcular(string m1, string m5, string m10, string m25, string m50, string m100, string m200, string m500, string m1000, string m2000)
        {


            int md1 = m1 == "" ? 0 : M1 * int.Parse(m1);
            int md5 = m5 == "" ? 0 : M5 * int.Parse(m5);
            int md10 = m10 == "" ? 0 : M10 * int.Parse(m10);
            int md25 = m25 == "" ? 0 : M25 * int.Parse(m25);
            int md50 = m50 == "" ? 0 : M50 * int.Parse(m50);
            int md100 = m100 == "" ? 0 : M100 * int.Parse(m100);
            int md200 = m200 == "" ? 0 : M200 * int.Parse(m200);
            int md500 = m500 == "" ? 0 : M500 * int.Parse(m500);
            int md1000 = m1000 == "" ? 0 : M1000 * int.Parse(m1000);
            int md2000 = m2000 == "" ? 0 : M2000 * int.Parse(m2000);

            result = md1 + md5 + md10 + md25 + md50 + md100 + md200 + md500 + md1000 + md2000;
        }

    }// fin de clase










}//fin de namespace
