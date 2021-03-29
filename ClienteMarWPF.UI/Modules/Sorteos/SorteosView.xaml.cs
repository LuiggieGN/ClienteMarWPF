using ClienteMarWPF.DataAccess;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.ViewModels.Commands;
using ClienteMarWPF.UI.ViewModels.Commands.Sorteos;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using ClienteMarWPF.UI.Views.WindowsModals;
using MarPuntoVentaServiceReference;
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
using System.Windows.Threading;
using static ClienteMarWPF.Domain.Models.Dtos.SorteosDisponibles;

namespace ClienteMarWPF.UI.Modules.Sorteos
{
    /// <summary>
    /// Interaction logic for SorteosView.xaml
    /// </summary>
    public partial class SorteosView : UserControl
    {
        private List<SorteosObservable> SorteosBinding;
        private List<SuperPaleDisponible> combinations;
        private List<Jugada> ListJugadas;
        private DateTime lastKeyPress;
        private List<string> NumerosJugados;
        private static string ticketSeleccionado;


        //ConfirmarMontoWindow modal = new ConfirmarMontoWindow();


        public static readonly DependencyProperty RealizarApuestaCommandProperty = DependencyProperty.Register("RealizarApuestaCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));
        //public static readonly DependencyProperty GetListadoTicketsCommandProperty = DependencyProperty.Register("GetListadoTicketsCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));
        public static readonly DependencyProperty ValidarPagoTicketCommandProperty = DependencyProperty.Register("ValidarPagoTicketCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));
        public static readonly DependencyProperty CopiarTicketCommandProperty = DependencyProperty.Register("CopiarTicketCommand", typeof(ICommand), typeof(SorteosView), new PropertyMetadata(null));

        public ICommand RealizarApuestaCommand
        {
            get { return (ICommand)GetValue(RealizarApuestaCommandProperty); }
            set { SetValue(RealizarApuestaCommandProperty, value); }
        }
        //public ICommand GetListadoTicketsCommand
        //{
        //    get { return (ICommand)GetValue(GetListadoTicketsCommandProperty); }
        //    set { SetValue(GetListadoTicketsCommandProperty, value); }
        //}        
        public ICommand ValidarPagoTicketCommand
        {
            get { return (ICommand)GetValue(ValidarPagoTicketCommandProperty); }
            set { SetValue(ValidarPagoTicketCommandProperty, value); }
        }
        public ICommand CopiarTicketCommand
        {
            get { return (ICommand)GetValue(CopiarTicketCommandProperty); }
            set { SetValue(CopiarTicketCommandProperty, value); }
        }


        public SorteosView()
        {
            InitializeComponent();
            ListJugadas = new List<Jugada>();
            NumerosJugados = new List<string>();
            SorteosBinding = ConvertToObservables(SessionGlobals.LoteriasYSupersDisponibles);
            combinations = SessionGlobals.SuperPaleDisponibles;
            listSorteo.DataContext = SorteosBinding;

            
            
            //MostrarSorteos();
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
            NumerosJugados = new List<string>();
            RemoveAllSeletion();
            ltJugada.ItemsSource = new List<Jugada>();
            txtMontoTotal.Content = "$0.00";
            //MostrarSorteos();
        }
        private List<SorteosObservable> ConvertToObservables(List<MAR_Loteria2> Sorteos)
        {
            
            var SorteosObservables = new List<SorteosObservable>();

            foreach (var item in Sorteos)
            {
                SorteosObservables.Add(new SorteosObservable { LoteriaID = item.Numero, Loteria = item.Nombre, IsSelected = false, Date = DateTime.Now });
            }


            return SorteosObservables;
        }
        private List<MAR_Loteria2> ConvertToSorteos(List<SorteosObservable> SorteosObservable)
        {
            var Sorteos = new List<MAR_Loteria2>();
            foreach (var item in SorteosObservable)
            {
                Sorteos.Add(new MAR_Loteria2 { LoteriaKey = item.LoteriaID, Nombre = item.Loteria });
            }

            return Sorteos;
        }
        private int FindCombinations(List<int> LotteryIDs)
        {
            int response = -1;

            if (combinations.Any())
            {
                if (LotteryIDs.Count == 2)
                {
                    int loteria1 = LotteryIDs.First();
                    int loteria2 = LotteryIDs.Last();
                    var hasCombinations = combinations.Where(x => (x.LoteriaID1 == loteria1 && x.LoteriaID2 == loteria2) || (x.LoteriaID1 == loteria2 && x.LoteriaID2 == loteria1));
                    if (hasCombinations.Any())
                    {
                        int combinationID = hasCombinations.Select(x => x.LoteriaIDDestino).FirstOrDefault();
                        response = combinationID;

                    }
                }
            }


            return response;
        }
        private void FindCombinationsNotExist()
        {
            if (CrearSuper.IsChecked == true)
            {
                var sorteos = SessionGlobals.LoteriasDisponibles;
                var CombinationsAllSorteo = GetCombinations(sorteos.Select(x => x.Numero).ToList(), 2);
                var combinationNotPossibility = new List<List<int>>();
                
                string combinationNotPossibilityString = "Lista de combinaciones no disponibles, favor solicitar:" + Environment.NewLine + Environment.NewLine;

                foreach (var item in CombinationsAllSorteo)
                {
                    var first = item.First();
                    var last = item.Last();
                    var hasCombinations = combinations.Where(x => (x.LoteriaID1 == first && x.LoteriaID2 == last) || (x.LoteriaID1 == last && x.LoteriaID2 == first));
                    if (!hasCombinations.Any())
                    {
                        combinationNotPossibility.Add(new List<int> { first, last });

                    }
                }

                if (combinationNotPossibility.Count > 0)
                {
                    foreach (var combo in combinationNotPossibility)
                    {
                        string loteria1 = sorteos.Where(x => x.Numero == combo.First()).Select(x => x.Nombre).FirstOrDefault();
                        string loteria2 = sorteos.Where(x => x.Numero == combo.Last()).Select(x => x.Nombre).FirstOrDefault();
                        combinationNotPossibilityString += "-- " + loteria1 + " + " + loteria2 + " --";
                    }

                    MessageBox.Show(combinationNotPossibilityString, "  Aviso  ", MessageBoxButton.OK, MessageBoxImage.Information);
                }



            }
        }
        public void RefreshListJugadas()
        {
            ltJugada.ItemsSource = new List<Jugada>();
            ltJugada.ItemsSource = ListJugadas;
            ltJugada.Items.Refresh();
            string total = ListJugadas.Sum(x => x.Monto).ToString();
            txtMontoTotal.Content = (Decimal.Parse(total)).ToString("C");
        }
        public void AddItem(Jugada NuevaJugada = null)
        {
            if (!txtJugada.Text.Trim().Equals(string.Empty) && !txtMonto.Text.Trim().Equals(string.Empty))
            {
                string jugada = SepararNumeros(txtJugada.Text);
                var numeros = jugada.Split('-');
                jugada = "";
                Array.Sort(numeros);
                for (var i=0; i< numeros.Length;i++)
                {
                    jugada = jugada + numeros[i];
                    if (i+1 != numeros.Length )
                    {
                        jugada = jugada + "-";
                    }
                }
                int tipo = numeros.Count();
                var nuevajugada = new Jugada { Jugadas = jugada, Monto = Convert.ToInt32(txtMonto.Text), TipoJugada = TipoJugada(tipo) };
                

                foreach (var item in numeros)
                {
                    NumerosJugados.Add(item);
                    
                }
                
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
            else if (NuevaJugada != null)
            {
                var existeJugada = ListJugadas.Where(x => x.Jugadas == NuevaJugada.Jugadas).Any();
                if (existeJugada)
                {
                    Jugada jugadaExistente = ListJugadas.Where(x => x.Jugadas == NuevaJugada.Jugadas).FirstOrDefault();
                    jugadaExistente.Monto += NuevaJugada.Monto;
                }
                else
                {
                    ListJugadas.Add(NuevaJugada);
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
            var VM = DataContext as SorteosViewModel;

            if (ltJugada.Items.Count > 0)
            {
                var sorteos = SorteosBinding.Where(x => x.IsSelected == true).ToList();
                var Loteria = 0;
                if (sorteos.Count > 1 && CrearSuper.IsChecked == true)
                {
                    int sorteoComb = FindCombinations(sorteos.Select(x => x.LoteriaID).ToList());
                    if (sorteoComb < 0)
                    {
                        FindCombinationsNotExist();
                    }
                    else
                    {
                        Loteria = sorteoComb;
                    }
                }
                else if (VM.LoteriasMultiples.Count > 0 && CrearSuper.IsChecked == false)
                {

                    Loteria = sorteos.Select(x => x.LoteriaID).FirstOrDefault();
                    if (RealizarApuestaCommand != null)
                    {
                        foreach (var LoteriaID in VM.LoteriasMultiples) { 
                            RealizarApuestaCommand.Execute(new ApuestaResponse { Jugadas = ListJugadas, LoteriaID = LoteriaID });
                        }
                        VM.LoteriasMultiples = new List<int>();
                   
                    }
                }


                //if (GetListadoTicketsCommand != null)
                //{
                //    GetListadoTicketsCommand.Execute(null);
                //}

                LimpiarApuesta();
                RefreshListJugadas();
            }
            else
            {
                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("No hay jugadas en la lista o debe seleccionar una loteria.", "Aviso");

            }

        }

        //public void Mensaje()
        //{
        //    try
        //    {
        //        ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Jugada realizada satisfactoriamente.", "Excelente");
        //    }
        //    catch( Exception e)
        //    {
        //        MessageBox.Show(e.Message);
        //    }
            
        //}

        //public void MensajeError()
        //{
        //    ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Hubo un error en el proceso, intentelo de nuevo.", "Aviso");
        //}


        private void OpenCombinacion()
        {
            var NumerosJugado = new List<string>();
            if (ltJugada.Items.Count != 0)
            {
                NumerosJugado = NumerosJugados.Distinct().ToList();
            }

            var combinacion = new CombinacionWindowsModal(NumerosJugado);
            combinacion.Jugadas += delegate (List<Jugada> Jugadas)
            {
                foreach (var item in Jugadas)
                {
                    AddItem(item);
                }

               // MostrarSorteos();
            };

            combinacion.ShowDialog();
        }
        private void MostrarSuper(bool visible = true)
        {

            var sorteosDisponibles = ConvertToSorteos(ConvertToObservables(SessionGlobals.LoteriasYSupersDisponibles));
            var sorteosRegularesIds = sorteosDisponibles.Select(x => x.LoteriaKey).Except(combinations.Select(x => x.LoteriaIDDestino)).ToList();
            var sorteosResult = new List<MAR_Loteria2>();
            foreach (var item in sorteosRegularesIds)
            {
                var sorteo = sorteosDisponibles.Where(x => x.LoteriaKey == item).FirstOrDefault();
                sorteosResult.Add(sorteo);
            }

            if (visible)
            {
                listSorteo.DataContext = SorteosBinding;
            }
            else
            {
                SorteosBinding = ConvertToObservables(sorteosResult);
                listSorteo.DataContext = SorteosBinding;
            }
        }
        private void RemoveAllSeletion()
        {
            foreach (var item in SorteosBinding)
            {
                item.IsSelected = false;
            }
            listSorteo.Focus();

        }
        private IEnumerable<IEnumerable<T>> GetCombinations<T>(IEnumerable<T> items, int count)
        {
            int i = 0;
            foreach (var item in items)
            {
                if (count == 1)
                    yield return new T[] { item };
                else
                {
                    foreach (var result in GetCombinations(items.Skip(i + 1), count - 1))
                        yield return new T[] { item }.Concat(result);
                }

                ++i;
            }
        }
        #endregion


        private void PressTecla(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Subtract:
                    RemoveItem();
                    //MostrarSorteos();
                    break;

                case Key.Multiply:
                    OpenCombinacion();
                    break;

                case Key.Add:
                    Vender(sender, e);
                    break;    
            
                case Key.F5:
                    ticketSeleccionado = null;
                    ValidarPagoTicketCommand.Execute(null);
                    break;   
                    
                case Key.F9:
                    RemoveItem();
                    //MostrarSorteos();
                    break;    
                    
                case Key.F12:
                    Vender(sender,e);
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
                    //if (txtMonto.IsFocused || txtJugada.IsFocused)
                    //{
                    //    listSorteo.Focus();
                    //    listSorteo.SelectedIndex = 0;
                    //}
                    if (txtJugada.IsFocused)
                    {
                        listSorteo.Focus();
                        listSorteo.SelectedIndex = 0;
                    }else if( txtMonto.IsFocused )
                    {
                        txtJugada.Focus();
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
            MostrarSuper();
            RemoveAllSeletion();
        }
        private void CrearSuper_Checked(object sender, RoutedEventArgs e)
        {
            MostrarSuper(false);
        }
        private void SelectCampo(object sender, RoutedEventArgs e)
        {
            txtMonto.Focus();
            if (DataContext != null)
            {
                var vm = DataContext as SorteosViewModel;
                vm.SorteoViewClass = this;
            }
        }

        
        private void AgregaJugada(object sender, KeyEventArgs e)
        {

            var modal = new ConfirmarMontoWindow();

            if (e.Key == Key.Enter)
            {
              
                TextBox s = e.Source as TextBox;
                if (s != null)
                {

                    if ( txtMonto.Text != "0" || txtMonto.Text != "00" )
                    {
                        if (txtMonto.Text != "" && txtJugada.Text != "")
                        {


                            if (Convert.ToInt32(txtMonto.Text) <= 500)
                            {

                                AddItem();
                                txtMonto.Text = ""; 
                                txtJugada.Text = "";

                            }
                            else
                            {
                                modal.Owner = Application.Current.MainWindow;

                                modal.ShowDialog();
                                modal.Mostrar(txtMonto.Text);

                                if (modal.Confirmar)
                                {
                                    AddItem();
                                    txtMonto.Text = "";
                                    txtJugada.Text = "";

                                }
                            }

                        }
                        else
                        {
                            if(txtMonto.Text != "" && txtJugada.Text == "")
                            {
                                txtJugada.Focus();
                                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("El campo de jugada no puede estar vacio.", "Aviso");
                            }
                            else if(txtJugada.Text != "" && txtMonto.Text == "")
                            {
                                txtMonto.Focus();
                                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("El campo monto no puede estar vacio.", "Aviso");
                            }
                            else if(txtMonto.Text == "" && txtJugada.Text == "")
                            {
                                txtMonto.Focus();
                                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Los campos no pueden estar vacios.", "Aviso");
                            }
                        }
                    }
                    else
                    {
                        ((MainWindow)Window.GetWindow(this)).MensajesAlerta("El monto debe ser mayor a 0.", "Aviso");
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
           //MostrarSorteos();
        }
        private void Quitar(object sender, RoutedEventArgs e)
        {
            RemoveItem();
            //MostrarSorteos();
        }
        private void Vender(object sender, RoutedEventArgs e)
        {
            if( SorteosBinding != null && SorteosBinding.Count > 0 && (SorteosBinding.Any(x => x.IsSelected == true)  ))
            {
                if( ltJugada.Items.Count > 0)
                {
                    RealizaApuesta();
                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Jugada realizada satisfactoriamente.", "Excelente");
                }
                else
                {
                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta("No hay jugadas en la lista.", "Aviso");
                }

            }
            else
            {
                ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Debe seleccionar al menos un sorteo.", "Aviso");
            }
           
        }
        private void btnCombinar(object sender, RoutedEventArgs e)
        {
            OpenCombinacion();
        }
        //public void GetTicketSeleccionado(object sender, MouseButtonEventArgs e)
        //{
        //    if (tbVentas.SelectedItem != null) { 
        //        MAR_Bet Ticket = (MAR_Bet)tbVentas.SelectedItem;
        //        ticketSeleccionado = Ticket.TicketNo;
        //        if (CopiarTicketCommand != null)
        //        {
        //            CopiarTicketCommand.Execute(new TicketCopiadoResponse { TicketNo = Ticket.TicketNo });
                
        //        }
        //        GetJugadasTicket();
        //    }
        //}

        //public void GetPrecargarTicketSeleccionado(object sender, MouseButtonEventArgs e)
        //{
        //    if (tbVentas.SelectedItem != null)
        //    {
        //        MAR_Bet Ticket = (MAR_Bet)tbVentas.SelectedItem;
        //        ticketSeleccionado = Ticket.TicketNo.ToString();

        //        ValidarPagoTicketCommand.Execute(null);
        //    }

        //}

        //public string GetTicketNumeroPrecargar()
        //{
        //    return ticketSeleccionado;
        //}

        public void GetJugadasTicket()
        {

            var Vm = DataContext as SorteosViewModel;

            if (Vm != null)
            {
                var jugadas = Vm.ListadoJugada;
                ltJugada.ItemsSource= new List<Jugada>();
                ListJugadas = new List<Jugada>();
                foreach (var jugada in jugadas)
                {
                    txtJugada.Text = jugada.Jugadas;
                    txtMonto.Text = jugada.Monto.ToString();
                    AddItem(jugada);
                    RefreshListJugadas();

                }
                txtJugada.Text = "";
                txtMonto.Text = "";
                //MostrarSorteos();
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ticketSeleccionado = null;
        }

        private void CheckBox_OnlyLoteria(object sender, RoutedEventArgs e)
        {
            var loteria = (sender as CheckBox);
            var loteriaSeleccionada = loteria.DataContext as SorteosObservable;
            foreach (var item in SorteosBinding)
            {
                if (loteriaSeleccionada.LoteriaID == item.LoteriaID)
                {
                    if (item.IsSelected==true)
                    {
                        item.IsSelected = false;
                    }
                    else if (item.IsSelected == false)
                    {
                        item.IsSelected = true;
                    }
                }
            }
        }

        private void CheckBox_ClickOne(object sender, RoutedEventArgs e)
        {
            var VM = DataContext as SorteosViewModel;

            if (VM != null)
            {
                var objectoLoteria = (sender as Control);
                SorteosObservable sorteo = objectoLoteria.DataContext as SorteosObservable;
                if (sorteo.IsSelected==true) {
                    var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(sorteo.LoteriaID);
                    if (posicionLoteriaEliminar == -1)
                    {
                        VM.LoteriasMultiples.Add(sorteo.LoteriaID);
                    }
                   
                }else if (sorteo.IsSelected == false)
                {
                    var posicionLoteriaEliminar = VM.LoteriasMultiples.IndexOf(sorteo.LoteriaID);
                    if (posicionLoteriaEliminar != -1) { 
                        VM.LoteriasMultiples.RemoveAt(posicionLoteriaEliminar);
                    }
                }
                
            }
            //var CheckBox = e.Source as CheckBox;
            //var item = CheckBox.DataContext as SorteosObservable;
            //if (item.IsSelected)
            //{
            //    AddLoteriaMultiples();
            //}
            //else
            //{
            //    RemoveItem(item.LoteriaID);
            //}
        }

        //private void AddLoteriaMultiples()
        //{
        //    var sorteosCheck = SorteosBinding.Where(x => x.IsSelected == true).ToList();

        //    if (sorteosCheck.Count != 0 && CrearSuper.IsChecked == false)
        //    {

        //        var hasSorteo = sorteosCheck.Select(x => x.LoteriaID).ToList();
        //        if (hasSorteo.Count != 0)
        //        {
        //            foreach (var item in hasSorteo)
        //            {
        //                SorteosSave.LoteriasResponse.Add(item);
        //            }

        //        }

        //    }
        //    else if (sorteosCheck.Count != 0 && sorteosCheck.Count == 2 && CrearSuper.IsChecked == true)
        //    {
        //        var combinationID = FindCombinations(sorteosCheck.Select(x => x.LoteriaID).ToList());
        //        if (combinationID != 0)
        //        {
        //            var sorteoIsSave = SorteosSave.LoteriasResponse.Where(x => x == combinationID).Any();
        //            if (!sorteoIsSave)
        //            {
        //                SorteosSave.LoteriasResponse.Add(combinationID);
        //                RemoveAllSeletion();
        //            }

        //        }
        //        else
        //        {
        //            MessageBox.Show("Esta combinacion ya esta en la lista o no esta diponible", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }


        //    RefreshListSorteosSelect();

        //}

        //private void RemoveItem(int LoteriaID = 0)
        //{
        //    dynamic selectItem = listSorteo.SelectedItem;
        //    if (selectItem == null && LoteriaID == 0)
        //    {
        //        if (listSorteo.Items.Count > 0)
        //        {
        //            dynamic sorteosInList = listSorteo.ItemsSource;
        //            var LoteriasName = new List<string>();
        //            foreach (var items in sorteosInList)
        //            {
        //                LoteriasName.Add(items.Sorteo);
        //            }
        //            var item = LoteriasName.Last();

        //            if (item.Contains('+'))
        //            {
        //                var SorteosSplit = item.Split('+').ToList();
        //                SorteosSplit.Remove("+");
        //                var LoteriaIds = FindLotteryIDs(SorteosSplit);
        //                int CombinationID = FindCombinations(LoteriaIds);
        //                SorteosSave.LoteriasResponse.Remove(CombinationID);
        //            }
        //            else
        //            {
        //                int sorteoID = ConvertToSorteos(SorteosBinding).Where(x => x.Loteria == item).Select(x => x.LoteriaID).FirstOrDefault();
        //                SorteosSave.LoteriasResponse.Remove(sorteoID);
        //                var sorteo = SorteosBinding.Where(x => x.LoteriaID == sorteoID).FirstOrDefault();
        //                sorteo.IsSelected = false;
        //            }

        //        }
        //        else
        //        {
        //            MessageBox.Show("No hay sorteos en la lista.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }

        //    }
        //    else if (LoteriaID != 0)
        //    {
        //        SorteosSave.LoteriasResponse.Remove(LoteriaID);
        //        var sorteo = SorteosBinding.Where(x => x.LoteriaID == LoteriaID).FirstOrDefault();
        //        sorteo.IsSelected = false;
        //    }
        //    else if (selectItem != null)
        //    {
        //        string sorteoselect = selectItem.Sorteo;
        //        if (sorteoselect.Contains('+'))
        //        {
        //            var SorteosSplit = sorteoselect.Split('+').ToList();
        //            SorteosSplit.Remove("+");
        //            var LoteriaIds = FindLotteryIDs(SorteosSplit);
        //            int CombinationID = FindCombinations(LoteriaIds);
        //            SorteosSave.LoteriasResponse.Remove(CombinationID);
        //        }
        //        else
        //        {
        //            int sorteoID = ConvertToSorteos(SorteosBinding).Where(x => x.Loteria == selectItem.Sorteo).Select(x => x.LoteriaID).FirstOrDefault();
        //            SorteosSave.LoteriasResponse.Remove(sorteoID);
        //            var sorteo = SorteosBinding.Where(x => x.LoteriaID == sorteoID).FirstOrDefault();
        //            sorteo.IsSelected = false;
        //        }

        //    }


        //    RefreshListSorteosSelect();
        //    listLoteriasSelect.Focus();
        //    listLoteriasSelect.SelectedIndex = 0;
        //}
        //private void RefreshListSorteosSelect()
        //{
        //    sorteosselect.Content = "0";
        //    listLoteriasSelect.ItemsSource = new List<string>();
        //    var Sorteos = FindNameLottery(SorteosSave.LoteriasResponse);
        //    var values = from str in Sorteos select new { Sorteo = str };
        //    listLoteriasSelect.ItemsSource = values.ToList();
        //    sorteosselect.Content = SorteosSave.LoteriasResponse.Count;
        //    listLoteriasSelect.Items.Refresh();
        //}

        //private List<string> FindNameLottery(List<int> LotteryIDs)
        //{
        //    var nameLotterys = new List<string>();
        //    var sorteos = ConvertToSorteos(SorteosBinding);
        //    if (LotteryIDs.Count != 0)
        //    {

        //        foreach (var item in LotteryIDs)
        //        {
        //            string result = sorteos.Where(x => x.LoteriaID == item).Select(x => x.Loteria).FirstOrDefault();
        //            var resultSuper = combinations.Where(x => x.LoteriaDestino == item).FirstOrDefault();

        //            if (result != null && resultSuper != null)
        //            {
        //                if (CrearSuper.IsChecked == true)
        //                {
        //                    string loteria1 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria1).Select(x => x.Loteria).FirstOrDefault();
        //                    string loteria2 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria2).Select(x => x.Loteria).FirstOrDefault();
        //                    nameLotterys.Add(loteria1 + " + " + loteria2);
        //                }
        //                else
        //                {
        //                    nameLotterys.Add(result);
        //                }

        //            }
        //            else if (result == null && resultSuper != null)
        //            {
        //                string loteria1 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria1).Select(x => x.Loteria).FirstOrDefault();
        //                string loteria2 = sorteos.Where(x => x.LoteriaID == resultSuper.Loteria2).Select(x => x.Loteria).FirstOrDefault();
        //                nameLotterys.Add(loteria1 + " + " + loteria2);

        //            }
        //            else if (result != null && resultSuper == null)
        //            {
        //                nameLotterys.Add(result);

        //            }

        //        }

        //    }

        //    return nameLotterys;
        //}
    }

}
