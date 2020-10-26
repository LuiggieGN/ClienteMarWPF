using FlujoCustomControl.Code.BussinessLogic;
using FlujoCustomControl.Helpers;
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
    public partial class DesgloseWindowsTab : Window
    {

        //public event Action<string> SetMontoContado;
        //public event Action<int> SetMontoFaltante;
        
        private CalcularDesgloseLogic calc;
        private int result { get; set; }

        public DesgloseWindowsTab()
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

        private void ImprimirClick(object sender, RoutedEventArgs e)
        {

            string comentario = txtNota.Text ?? string.Empty;

            int ValorM1 = ParseInt(txtm1.Text);
            int ValorM5 = ParseInt(txtm5.Text);
            int ValorM10 = ParseInt(txtm10.Text);
            int ValorM25 = ParseInt(txtm25.Text);
            int ValorM50 = ParseInt(txtm50.Text);
            int ValorM100 = ParseInt(txtm100.Text);
            int ValorM200 = ParseInt(txtm200.Text);
            int ValorM500 = ParseInt(txtm500.Text);
            int ValorM1000 = ParseInt(txtm1000.Text);
            int ValorM2000 = ParseInt(txtm2000.Text);

            string template;

            template = $@"

  --------------------------------------------
   DESGLOSE DE CAJA  
  --------------------------------------------
          M/B   |  Cantidad  
  --------------------------------------------

         $ 1  -> {String.Format("{0:n}", ValorM1)}
         $ 5  -> {String.Format("{0:n}", ValorM5)}
        $ 10  -> {String.Format("{0:n}", ValorM10)} 
        $ 25  -> {String.Format("{0:n}", ValorM25)}  
        $ 50  -> {String.Format("{0:n}", ValorM50)}      
       $ 100  -> {String.Format("{0:n}", ValorM100)}    
       $ 200  -> {String.Format("{0:n}", ValorM200)} 
       $ 500  -> {String.Format("{0:n}", ValorM500)} 
      $ 1000  -> {String.Format("{0:n}", ValorM1000)} 
      $ 2000  -> {String.Format("{0:n}", ValorM2000)} 

  --------------------------------------------
   Nota: {comentario}
  Fecha: {DateTime.Now.ToString("D", new CultureInfo("es-ES"))}

   Total Contado: {result.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"))}
  --------------------------------------------
";



            PrintOutHelper.SendToPrinter(template);
            LimpiarTextBox();

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
            txtNota.Clear();
            lblTotalContado.Text = "$0.00";
            result = 0;
        }




        public static int ParseInt(string value, int defaultIntValue = 0)
        {
            int parsedInt;
            if (int.TryParse(value, out parsedInt))
            {
                return parsedInt;
            }

            return defaultIntValue;
        }



    }
}
