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
        private List<Jugadas> ListJugadas;
        private DateTime lastKeyPress;

        public SorteosView()
        {
            InitializeComponent();

            ListJugadas = new List<Jugadas>();
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
        //private void AddItem(TicketDetalle ticketDetalle = null)
        //{


        //    int tipoJugadaId = 0;
        //    if ((txtJugada.Text.Length < 3))
        //    {
        //        tipoJugadaId =
        //            1;
        //    }
        //    else if ((txtJugada.Text.Length < 5))
        //    {
        //        tipoJugadaId = 2;
        //    }
        //    else if ((txtJugada.Text.Length < 7))
        //    {
        //        tipoJugadaId = 3;
        //    }

        //    var td = new TicketDetalle();

        //    if (ticketDetalle == null)
        //    {
        //        // Creando modelo
        //        td = new TicketDetalle { Monto = Convert.ToInt32(txtMonto.Text), Jugada = SepararNumeros(txtJugada.Text), TipoJugadaID = tipoJugadaId };

        //        ////Agregando A numeros jugados
        //        //var numeros = SepararNumeros(txtJugada.Text).Split('-');
        //        //foreach (var item in numeros)
        //        //{
        //        //    NumerosJugados.Add(item);
        //        //}

        //    }
        //    else if (ticketDetalle != null)
        //    {
        //        td = ticketDetalle;
        //        //Agregando A numeros jugados
        //        //var numeros = ticketDetalle.Jugada.Split('-');
        //        //foreach (var item in numeros)
        //        //{
        //        //    NumerosJugados.Add(item);
        //        //}
        //    }
        //    // Agregando modelo a Vista
        //    TicketDetallesList.Add(td);
        //    // Sumando y Contando jugadas en base al modelo
        //    var SumaVariasJugadas = TicketDetallesList.Where(x => x.Jugada == td.Jugada.ToString()).Sum(x => x.Monto);
        //    var cantidadJugada = TicketDetallesList.Where(x => x.Jugada == td.Jugada.ToString()).Count();


        //    if (cantidadJugada > 1)
        //    {
        //        var jugada = TicketDetallesList.Where(x => x.Jugada == td.Jugada.ToString()).FirstOrDefault();
        //        jugada.Monto = SumaVariasJugadas;
        //        TicketDetallesList.Remove(td);
        //        //Vaciando y rellenando lista
        //        ltJugada.ItemsSource = null;
        //        ltJugada.ItemsSource = TicketDetallesList.Select(x => { return new Jugadas { Monto = x.Monto, Jugada = x.Jugada, TipoJugada = TipoJugada(x.TipoJugadaID) }; }).ToList();
        //    }
        //    else
        //    {
        //        //Vaciando y rellenando lista
        //        ltJugada.ItemsSource = null;
        //        ltJugada.ItemsSource = TicketDetallesList.Select(x => { return new Jugadas { Monto = x.Monto, Jugada = x.Jugada, TipoJugada = TipoJugada(x.TipoJugadaID) }; }).ToList();
        //    }

        //    //monto total de todas las jugadas
        //    //result.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
        //    txtMontoTotal.Content = TicketDetallesList.Sum(x => x.Monto).ToString("$#,##0", CultureInfo.CreateSpecificCulture("en-US"));
        //}




        //private void DeleteItem()
        //{
        //    var jugadas = ltJugada.Items.Count;
        //    if (jugadas != 0)
        //    {
        //        var index = jugadas - 1;

        //        if (ltJugada.SelectedItem != null)
        //        {
        //            //Obteniendo la jugada Selecionada
        //            var jugadaSelect = ltJugada.SelectedItem as Jugadas;
        //            RemoveJugada(jugadaSelect);

        //        }
        //        else
        //        {
        //            //obteniendo la ultima jugada 
        //            var jugadaSelect = ltJugada.Items[index] as Jugadas;
        //            RemoveJugada(jugadaSelect);

        //        }

        //    }
        //    else
        //    {
        //        ((MainWindow)Window.GetWindow(this)).MensajesAlerta("No hay mas Jugadas", "Info", "#28A745");
        //        //MensajesAlerta("No hay mas Jugadas");
        //    }
        //}




        //private void RemoveJugada(Jugadas Jugada)
        //{
        //    // encontrando la jugada en la lista
        //    var jugada = TicketDetallesList.Where(x => x.Jugada == Jugada.Jugada).FirstOrDefault();
        //    TicketDetallesList.Remove(jugada);
        //    //Vaciando y rellenando lista
        //    ltJugada.ItemsSource = null;
        //    ltJugada.ItemsSource = TicketDetallesList.Select(x => { return new Jugadas { Monto = x.Monto, Jugada = x.Jugada, TipoJugada = TipoJugada(x.TipoJugadaID) }; }).ToList();
        //    //Recalculando monto
        //    if (TicketDetallesList.Sum(x => x.Monto) != 0)
        //    {
        //        txtMontoTotal.Content = TicketDetallesList.Sum(x => x.Monto).ToString("$#,##0", CultureInfo.CreateSpecificCulture("en-US"));

        //    }
        //    else
        //    {
        //        txtMontoTotal.Content = "$0.00";
        //    }
        //}
        private void LimpiarApuesta()
        {
            // Limpiando todo 
            //TicketDetallesList.Clear();
            ltJugada.ItemsSource = null;
            txtMontoTotal.Content = "$0.00";
        }

        private void GetVendidosHoy()
        {
            //*** DESCOMENTAR Y PONER REFERENCIAS


            //var vendidosResponse = Code.BussinessLogic.CincoMinutosLogic.GetVendidosHoy();
            //if (vendidosResponse.Tickets != null && vendidosResponse.Tickets.Any())
            //{
            //    tbVentas.ItemsSource = vendidosResponse.Tickets.Select(x => new { x.TicketID, Hora = x.TicFecha, Monto = x.TicCosto, Activo = !x.TicNulo });
            //    txtTotalVenta.Content = vendidosResponse.Tickets.Where(x => x.TicNulo == false).Sum(x => x.TicCosto).ToString("C0");
            //}
            //else
            //{
            //    tbVentas.ItemsSource = null;
            //    txtTotalVenta.Content = "0.00";
            //}

        }
        //private void RealizaApuesta()
        //{
        //    var jugadas = ltJugada.Items.Count;


        //   TicketModel ticketmodel = new TicketModel();
        //    if (jugadas != 0)
        //    {
        //        try
        //        {
        //            if (TicketDetallesList.Sum(x => x.Monto) > 499)
        //            {

        //                //System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Esta segura del MONTO de: "+ txtMontoTotal.Text + " ", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
        //                // result == System.Windows.Forms.DialogResult.Yes
        //                if (false)
        //                {
        //                    ticketmodel.MontoOperacion = TicketDetallesList.Sum(x => x.Monto);
        //                    ticketmodel.TicketDetalles = TicketDetallesList;
        //                    //*** ARREGLAR APUESTA ESTO SOLO ES DE PRUEBA EL VALIDO ES EL DE ABAJO
        //                    dynamic apuesta = new object();
        //                    //*** DESCOMENTAR Y PONER REFERENCIAS
        //                    // var apuesta = Code.BussinessLogic.CincoMinutosLogic.Apuesta(ticketmodel);

        //                    if (apuesta.RespuestaApi.CodigoRespuesta == "100")
        //                    {
        //                        //*** DESCOMENTAR Y PONER REFERENCIAS
        //                        //PrintOutHelper.SendToPrinter(apuesta.PrintData);
        //                        LimpiarApuesta();
        //                        ((MainWindow)Window.GetWindow(this)).MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta, "Info", "#28A745");
        //                        ticketmodel = null;
        //                        GetVendidosHoy();
        //                    }
        //                    else
        //                    {
        //                        ((MainWindow)Window.GetWindow(this)).MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta, "Info", "#28A745");
        //                    }
        //                }
        //                //result == System.Windows.Forms.DialogResult.No
        //                else if (false)
        //                {
        //                    //...
        //                }
        //                else
        //                {
        //                    //...
        //                }
        //            }
        //            else
        //            {
        //                ticketmodel.MontoOperacion = TicketDetallesList.Sum(x => x.Monto);
        //                ticketmodel.TicketDetalles = TicketDetallesList;
        //                //*** ARREGLAR APUESTA ESTO SOLO ES DE PRUEBA EL VALIDO ES EL DE ABAJO
        //                dynamic apuesta = new object();
        //                //*** DESCOMENTAR Y PONER REFERENCIAS
        //                // var apuesta = Code.BussinessLogic.CincoMinutosLogic.Apuesta(ticketmodel);
        //                if (apuesta == null)
        //                {
        //                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Error interno de aplicacion. Comunique el administrador", "Alerta", "#28A745");
        //                }
        //                else if (apuesta.RespuestaApi.CodigoRespuesta == "100")
        //                {
        //                    //PrintOutHelper.SendToPrinter(apuesta.PrintData);
        //                    LimpiarApuesta();
        //                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta, "Alerta", "#28A745");
        //                    ticketmodel = null;
        //                    GetVendidosHoy();
        //                }
        //                else
        //                {
        //                    ((MainWindow)Window.GetWindow(this)).MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta, "Alerta", "#28A745");
        //                }
        //            }





        //        }
        //        catch (Exception e)
        //        {
        //            ((MainWindow)Window.GetWindow(this)).MensajesAlerta(e.StackTrace + "Fallo la transaccion de venta" + e.Message, "Alerta", "#28A745");
        //        }
        //    }
        //    else
        //    {
        //        ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Agregue al menos una Jugada", "Alerta", "#28A745");
        //    }

        //}
        //private void OpenCombinacion()
        //{
        //    var NumerosJugado = new List<string>();
        //    if (ltJugada.Items.Count != 0)
        //    {
        //        var jugadas = TicketDetallesList.Select(x => x.Jugada).ToList();

        //        foreach (var item in jugadas)
        //        {
        //            if (item.Contains('-'))
        //            {
        //                NumerosJugado.AddRange(item.Split('-'));
        //            }
        //            else
        //            {
        //                NumerosJugado.Add(item);
        //            }
        //        }

        //    }

        //    CombinacionWindowsModal combinacion = new CombinacionWindowsModal(NumerosJugado.Distinct().ToList());
        //    combinacion.Jugadas += delegate (List<TicketDetalle> Jugadas)
        //    {
        //        foreach (var item in Jugadas)
        //        {
        //            AddItem(item);
        //        }

        //        MostrarSorteos();
        //    };

        //    combinacion.ShowDialog();
        //}
        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(this)).MensajesAlerta("Esto es una prueba de alertas", "Aviso");

        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            //int Count = 0;
            //int SorteoIndex = 0;

            switch (e.Key)
            {
                case Key.Subtract:
                    //DeleteItem();
                    break;   
                    
                case Key.Add:
                    //RealizaApuesta();
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
                    if (listSorteo.SelectedItem != null)
                    {
                        var item = listSorteo.SelectedItem as SorteosObservable;
                        SorteosBinding.Where(x => x.LoteriaID == item.LoteriaID).Select(x => { x.IsSelected = !x.IsSelected; return x; }).FirstOrDefault();
                    }
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
            //if (e.Key == Key.Space)
            //{
            //    txtMonto.Text = "";
            //    txtJugada.Text = "";
            //}
            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;
                if (s != null)
                {
                    if (txtMonto.Text != "" && txtJugada.Text != "")
                    {
                        //AddItem();
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
                    //else
                    //{
                    //    s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    //}
                }
                e.Handled = true;
            }
           MostrarSorteos();
        }

        private void Quitar(object sender, RoutedEventArgs e)
        {
            //DeleteItem();
            MostrarSorteos();
        }

        private void Vender(object sender, RoutedEventArgs e)
        {
           // RealizaApuesta();
        }

        private void btnCombinar(object sender, RoutedEventArgs e)
        {
          //  OpenCombinacion();
        }
    }

}
