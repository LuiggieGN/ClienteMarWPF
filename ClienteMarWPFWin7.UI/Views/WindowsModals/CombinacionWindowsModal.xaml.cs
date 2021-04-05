using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.Views.WindowsModals
{
    /// <summary>
    /// Interaction logic for CombinacionWindowsModal.xaml
    /// </summary>
    public partial class CombinacionWindowsModal : Window
    {
        public event Action<List<Jugada>> Jugadas;
        private List<Jugada> ListJugadas { get; set; }
        private List<string> NumerosJugados { get; set; }

        public CombinacionWindowsModal(List<String> _numerosJugados)
        {
            InitializeComponent();

            txtNumCom1.Focus();
            ListJugadas = new List<Jugada>();
            NumerosJugados = _numerosJugados;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LlenarCamposConNumerosJugados();
        }

        private void LlenarCamposConNumerosJugados()
        {


            if (NumerosJugados.Count > 0)
            {
                var data = NumerosJugados.ToArray();
                int positionData = 0;
                int NumberTxt = 1;
                foreach (TextBox tb in FindVisualChildren<TextBox>(this))
                {
                    string NameTextBox = "txtNumCom" + NumberTxt;

                    if (tb.Name == NameTextBox && positionData < NumerosJugados.Count)
                    {
                        tb.Text = data[positionData];
                        positionData++;
                        NumberTxt++;
                    }
                }

            }

        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void NextTextBox(object sender, KeyEventArgs e)
        {
            TextBox s = e.Source as TextBox;
            if (s != null && (s.Text.Length == 2 && s.Text != string.Empty) || e.Key == Key.Right)
            {
                s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
            else if (e.Key == Key.Left)
            {
                s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
            }
            else if (e.Key == Key.Down)
            {
                s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
            }
            else if (e.Key == Key.Up)
            {
                s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
            }
            e.Handled = true;

            //if (e.Key == Key.Enter)
            //{
            //    if (s != null)
            //    {
            //        s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            //    }
            //    e.Handled = true;
            //}


            var data = new List<string>();
            ListJugadas = new List<Jugada>();


            if (txtTripleta.Text != "")
            {
                data.AddRange(GetTripletasCombinacion(txtTripleta.Text));
            }

            if (txtPales.Text != "")
            {
                data.AddRange(GetPalesCombinacion(txtPales.Text));
            }
            if (txtPuntos.Text != "")
            {
                data.AddRange(GetPuntosCombinacion(txtPuntos.Text));
            }

            ltCombinadas.ItemsSource = data;


        }

        private void CloseButton(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AgregarButton(object sender, RoutedEventArgs e)
        {

            Jugadas?.Invoke(ListJugadas);
            this.Close();
        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F7)
            {
               Jugadas?.Invoke(ListJugadas);
                this.Close();
            }

            if (e.Key == Key.F9)
            {
                this.Close();
            }
        }

        private List<int> GetNumberTextBox()
        {
            List<int> storageNumber = new List<int>();
            foreach (TextBox tb in FindVisualChildren<TextBox>(this))
            {
                for (int i = 1; i < 21; i++)
                {
                    string NameTextBox = "txtNumCom" + i;
                    if (tb.Name == NameTextBox)
                    {
                        if (!tb.Text.Equals(""))
                        {
                            int number = Convert.ToInt32(tb.Text);
                            storageNumber.Add(number);
                        }
                    }
                }
            }

            if (storageNumber.Count > 1)
            {
                return storageNumber;
            }
            else
            {
                return new List<int>();
            }

        }

        private List<string> GetPalesCombinacion(string precio)
        {
            var numbers = GetNumberTextBox();
            //OBTENIENDO PALES
            var pales = new List<string>();
            foreach (var perm in GetPermutations(numbers.OrderBy(x => x).ToList(), 2))
            {
                string pale = "";
                foreach (var c in perm)
                {
                    if (Convert.ToInt32(c) < 10)
                    {
                        pale += ("0" + c + "-");

                    }
                    else
                    {
                        pale += c + "-";
                    }
                }
                string dataClean = pale == "" ? "" : "Pale ->" + pale.Substring(0, 5) + " de $" + precio;

                ListJugadas.Add(new Jugada { Monto = Convert.ToInt32(precio), Jugadas = pale.Substring(0, 5), TipoJugada = "Pale" });

                pales.Add(dataClean);
            }

            return pales;
        }

        private List<string> GetTripletasCombinacion(string precio)
        {
            var numbers = GetNumberTextBox();
            //OBTENIENDO TRIPLETAS
            var tripletas = new List<string>();
            foreach (var perm in GetPermutations(numbers, 3))
            {
                string tripleta = "";
                foreach (var c in perm)
                {
                    if (Convert.ToInt32(c) < 10)
                    {
                        tripleta += ("0" + c + "-");
                    }
                    else
                    {
                        tripleta += c + "-";
                    }


                }
                string dataClean = tripleta == "" ? "" : "Tripleta ->" + tripleta.Substring(0, 8) + " de $" + precio;
                ListJugadas.Add(new Jugada { Monto = Convert.ToInt32(precio), Jugadas = tripleta.Substring(0, 8), TipoJugada = "Tripleta" });
                tripletas.Add(dataClean);
            }

            return tripletas;
        }

        private List<string> GetPuntosCombinacion(string precio)
        {
            var numbers = GetNumberTextBox();
            //OBTENIENDO PUNTOS
            var puntos = new List<string>();
            foreach (var c in numbers)
            {
                string punto = "";
                if (Convert.ToInt32(c) < 10)
                {
                    punto += ("0" + c);
                }
                else
                {
                    punto += c;
                }
                ListJugadas.Add(new Jugada { Monto = Convert.ToInt32(precio), Jugadas = punto, TipoJugada = "Quiniela" });

                puntos.Add(precio + " puntos del " + punto);
            }

            return puntos;
        }

        private IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetPermutations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
    }
}
