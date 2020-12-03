using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using ClienteMarWPF.UI.Views.WindowsModals;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI.Modules.Sorteos
{
    /// <summary>
    /// Interaction logic for SorteosView.xaml
    /// </summary>
    public partial class SorteosView : UserControl
    {
        private List<SorteosObservable> SorteosBinding;
        private List<CombinationPossibility> combinations;
        private List<Jugada> ListJugadas;
        private DateTime lastKeyPress;

        public static readonly DependencyProperty RealizarApuestaCommandProperty = DependencyProperty.Register("RealizarApuestaCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));

        public ICommand RealizarApuestaCommand
        {
            get { return (ICommand)GetValue(RealizarApuestaCommandProperty); }
            set { SetValue(RealizarApuestaCommandProperty, value); }
        }

        public SorteosView()
        {
            InitializeComponent();

            Application.Current.Properties["Name"] = "name";

            ListJugadas = new List<Jugada>();
            var sorteos = new List<SorteosDTO> {
                new SorteosDTO(){ LoteriaID=1, Loteria="La Fecha Dia"},
                new SorteosDTO(){ LoteriaID=2, Loteria="La Fecha Noche"},
                new SorteosDTO(){ LoteriaID=3, Loteria="Loteka Dia"},
                new SorteosDTO(){ LoteriaID=4, Loteria="Loteka Noche" },
                new SorteosDTO(){ LoteriaID=5, Loteria="Nacional Dia"},
                new SorteosDTO(){ LoteriaID=6, Loteria="Nacional Noche"},
                new SorteosDTO(){ LoteriaID=7, Loteria="New York Dia"},
                new SorteosDTO(){ LoteriaID=8, Loteria="New York Noche"},
                new SorteosDTO(){ LoteriaID=9, Loteria="Pega 3 Dia"},
                new SorteosDTO(){ LoteriaID=10,Loteria="Pega 3 Noche"}
            };
            var combinationPossibilities = new List<CombinationPossibility>
            {
                new CombinationPossibility { Loteria1 = 1, Loteria2 = 2, LoteriaDestino = 11},
                new CombinationPossibility { Loteria1 = 3, Loteria2 = 4, LoteriaDestino = 12},
                new CombinationPossibility { Loteria1 = 5, Loteria2 = 6, LoteriaDestino = 13},
                new CombinationPossibility { Loteria1 = 7, Loteria2 = 8, LoteriaDestino = 14},
                new CombinationPossibility { Loteria1 = 9, Loteria2 = 10, LoteriaDestino = 15},
            };
            SorteosBinding = ConvertToObservables(sorteos);
            combinations = combinationPossibilities;
            listSorteo.DataContext = SorteosBinding;
            MostrarSorteos();
        }


        #region LOGICA PARA SORTEOS
        private void ValidateSelectOnlyTwo()
        {
            int count = 0;
            foreach (var item in SorteosBinding)
            {
                if (count <= 1 && item.IsSelected)
                {
                    count++;
                }
                else
                {
                    item.IsSelected = false;
                }
            }
        }
        private string SepararNumeros(string jugadaIn)
        {
            var jugada = jugadaIn;
            StringBuilder builder = new StringBuilder(jugada);
            var startIndex = builder.Length - (builder.Length % 2);
            for (int i = startIndex; i >= 2; i += -2)
            {
                if (i < 6)
                    builder.Insert(i, '-');
            }
            jugada = builder.ToString();
            if (jugada.Substring(jugada.Length - 1) == "-")
            {
                var newText = jugada.Substring(0, jugada.Length - 1);
                jugada = newText;
            }
            StringBuilder numerosBuilder = new StringBuilder(10);
            var numerosSplited = jugada.Split('-');
            var loopTo = jugada.Split('-').Length - 1;
            for (var index = 0; index <= loopTo; index++)
            {
                var t = numerosSplited[index].PadLeft(2, '0');
                numerosBuilder.Append(numerosSplited[index].PadLeft(2, '0'));
            }
            var startIndex2 = numerosBuilder.Length - (numerosBuilder.Length % 2);
            for (int i = startIndex2; i >= 2; i += -2)
            {
                if (i < 6)
                    numerosBuilder.Insert(i, '-');
            }
            jugada = numerosBuilder.ToString();
            if (jugada.Substring(jugada.Length - 1) == "-")
            {
                var newText = jugada.Substring(0, jugada.Length - 1);
                jugada = newText;
            }

            return jugada;
        }
        private string TipoJugada(int tipojujgada)
        {
            string tipojugada = tipojujgada == 1 ? "  Quiniela" : tipojujgada == 2 ? "  Pale" : tipojujgada == 3 ? "  Tripleta" : " ";
            return tipojugada;
        }
        private void MostrarSorteos()
        {
            var mostrarSorteos = ltJugada.Items.Count > 0;
            if (mostrarSorteos)
            {
                SorteosYLoterias.Visibility = Visibility.Visible;
                VentasYConsulta.Visibility = Visibility.Collapsed;
            }
            else
            {
                SorteosYLoterias.Visibility = Visibility.Collapsed;
                VentasYConsulta.Visibility = Visibility.Visible;
            }
        }
        private void LimpiarApuesta()
        {
            // Limpiando todo 
            ListJugadas = new List<Jugada>();
            foreach (var item in SorteosBinding)
            {
                item.IsSelected = false;
            }
            ltJugada.ItemsSource = new List<Jugada>();
            txtMontoTotal.Content = "$0.00";
            MostrarSorteos();
        }
        private List<SorteosObservable> ConvertToObservables(List<SorteosDTO> Sorteos)
        {
            var SorteosObservables = new List<SorteosObservable>();
            foreach (var item in Sorteos)
            {
                SorteosObservables.Add(new SorteosObservable { LoteriaID = item.LoteriaID, Loteria = item.Loteria, IsSelected = false, Date = DateTime.Now });
            }

            return SorteosObservables;
        }
        private List<SorteosDTO> ConvertToSorteos(List<SorteosObservable> SorteosObservable)
        {
            var Sorteos = new List<SorteosDTO>();
            foreach (var item in SorteosObservable)
            {
                Sorteos.Add(new SorteosDTO { LoteriaID = item.LoteriaID, Loteria = item.Loteria });
            }

            return Sorteos;
        }
        private int FindCombinations(List<int> LotteryIDs)
        {
            int response = 0;
            if (LotteryIDs.Count == 2)
            {
                int loteria1 = LotteryIDs.First();
                int loteria2 = LotteryIDs.Last();
                var hasCombinations = combinations.Where(x => (x.Loteria1 == loteria1 && x.Loteria2 == loteria2) || (x.Loteria1 == loteria2 && x.Loteria2 == loteria1));
                if (hasCombinations.Any())
                {
                    int combinationID = hasCombinations.Select(x => x.LoteriaDestino).FirstOrDefault();
                    response = combinationID;

                }
            }

            return response;
        }
        private void RefreshListJugadas()
        {
            ltJugada.ItemsSource = new List<Jugada>();
            ltJugada.ItemsSource = ListJugadas;
            ltJugada.Items.Refresh();
            string total = ListJugadas.Sum(x => x.Monto).ToString();
            txtMontoTotal.Content = (Decimal.Parse(total)).ToString("C");
        }
        private void AddItem()
        {
            if (!txtJugada.Text.Trim().Equals(string.Empty) && !txtMonto.Text.Trim().Equals(string.Empty))
            {
                string jugada = SepararNumeros(txtJugada.Text);
                int tipo = jugada.Split('-').Count();
                var nuevajugada = new Jugada { Jugadas = jugada, Monto = Convert.ToInt32(txtMonto.Text), TipoJugada = TipoJugada(tipo) };
                
                var existeJugada = ListJugadas.Where(x => x.Jugadas == nuevajugada.Jugadas).Any();
                if (existeJugada)
                {
                    Jugada jugadaExistente = ListJugadas.Where(x => x.Jugadas == nuevajugada.Jugadas).FirstOrDefault();
                    jugadaExistente.Monto += nuevajugada.Monto;
                }
                else
                {
                    ListJugadas.Add(nuevajugada);
                }
            }
            else
            {
                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("No ha realizado jugada o puesto monto", "Aviso");
            }
 
            RefreshListJugadas();

        }
        private void RemoveItem()
        {
            if (ltJugada.Items.Count > 0)
            {
                var jugada = ltJugada.SelectedItem;
                if (jugada != null)
                {
                    var JugadaModel = jugada as Jugada;
                    ListJugadas.Remove(JugadaModel);
                }
                else
                {
                    ListJugadas.Remove(ListJugadas.LastOrDefault());
                }
            }

            RefreshListJugadas();
        }
        private void SelectItem()
        {
            if (listSorteo.SelectedItem != null)
            {
                var item = listSorteo.SelectedItem as SorteosObservable;
                var sorteoChangeSelect = SorteosBinding.Where(x => x.LoteriaID == item.LoteriaID).Select(x => { x.IsSelected = !x.IsSelected; x.Date = DateTime.Now; return x; }).FirstOrDefault();
                //if (sorteoChangeSelect.IsSelected == false && CrearSuper.IsChecked == false)
                //{
                //    SorteosSave.LoteriasResponse.Remove(sorteoChangeSelect.LoteriaID);
                //}
                var sorteosIsSelect = SorteosBinding.Where(x => x.IsSelected == true).ToList();

                if (sorteosIsSelect.Count == 3 && CrearSuper.IsChecked == true)
                {

                    var listLastModify = SorteosBinding.Where(x => x.LoteriaID != item.LoteriaID && x.IsSelected == true).ToList();
                    var firstDate = listLastModify.First().Date.TimeOfDay;
                    var lastDate = listLastModify.Last().Date.TimeOfDay;


                    if (firstDate < lastDate)
                    {
                        listLastModify.First().IsSelected = false;
                    }
                    else if (lastDate < firstDate)
                    {
                        listLastModify.Last().IsSelected = false;
                    }
                    else
                    {
                        item.Date = DateTime.Now;
                    }

                }
                item.Date = DateTime.Now;
            }

            RefreshListJugadas();
        }
        private void RealizaApuesta()
        {
            var sorteos = SorteosBinding.Where(x => x.IsSelected == true).ToList();
            int Loteria = 0;
            if (sorteos.Count > 2)
            {
                Loteria = FindCombinations(sorteos.Select(x => x.LoteriaID).ToList());
            } else {

                Loteria = sorteos.Select(x => x.LoteriaID).FirstOrDefault();
            }

            if (RealizarApuestaCommand != null)
            {
                RealizarApuestaCommand.Execute(new { Jugadas = ListJugadas, LoteriaID = Loteria });
                LimpiarApuesta();
            }
        }

        #endregion


        private void PressTecla(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Subtract:
                    RemoveItem();
                    MostrarSorteos();
                    break;   
                    
                case Key.Add:
                    RealizaApuesta();
                    break;    
                    
                case Key.Multiply:
                   // OpenCombinacion();
                    break;    
                      
                case Key.F5:
                   // ShowConsultaTiket();
                    break;     
                    
                case Key.F11:
                    if (txtMonto.IsFocused || txtJugada.IsFocused)
                    {
                        listSorteo.Focus();
                        listSorteo.SelectedIndex = 0;
                    }
                    else
                    {
                        txtMonto.Focus();
                    }

                    break;

                case Key.Enter:
                     SelectItem();
                    break;

                case Key.Left:
                    TimeSpan lastTime = DateTime.Now.Subtract(lastKeyPress).Duration();
                    TimeSpan watingTime = TimeSpan.FromSeconds(1);

                    if (lastTime <= watingTime)
                    {
                        txtMonto.Focus();
                    }
                     lastKeyPress = DateTime.Now;
 
                    break;

                case Key.Right:
                    if (txtMonto.IsFocused || txtJugada.IsFocused)
                    {
                        listSorteo.Focus();
                        listSorteo.SelectedIndex = 0;
                    }
                    break;

                case Key.Up:
                    if (listSorteo.SelectedIndex != 0 && listSorteo.SelectedIndex != 1)
                    {
                        listSorteo.SelectedIndex = listSorteo.SelectedIndex - 2;
                    }
                    break;

                case Key.Space:
                    SelectItem();
                    break;

            }


        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void listSorteo_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            ValidateSelectOnlyTwo();
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            ValidateSelectOnlyTwo();
        }

        private void Regulares_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CrearSuper.IsChecked = false;
            foreach (var item in SorteosBinding)
            {
                item.IsSelected = false;
            }
        }

        private void SuperPales_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CrearSuper.IsChecked = true;
        }

        private void CrearSuper_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in SorteosBinding)
            {
                item.IsSelected = false;
            }
        }
        
        private void SelectCampo(object sender, RoutedEventArgs e)
        {
            txtMonto.Focus();
        }

        private void AgregaJugada(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    if (txtMonto.Text != "" && txtJugada.Text != "")
                    {
                        AddItem();
                        txtMonto.Text = "";
                        txtJugada.Text = "";
                    }
                    if (s.Name == "txtJugada")
                    {
                        txtMonto.Focus();
                    }
                    else if (s.Name == "txtMonto")
                    {
                        txtJugada.Focus();
                    }

                }
                e.Handled = true;
            }
           MostrarSorteos();
        }

        private void Quitar(object sender, RoutedEventArgs e)
        {
            RemoveItem();
            MostrarSorteos();
        }

        private void Vender(object sender, RoutedEventArgs e)
        {
           RealizaApuesta();
        }

        private void btnCombinar(object sender, RoutedEventArgs e)
        {
          //  OpenCombinacion();
        }
    }

}
