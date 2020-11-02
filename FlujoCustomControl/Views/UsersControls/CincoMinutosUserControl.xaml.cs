using Flujo.Entities.WpfClient.PublicModels;
using Flujo.Entities.WpfClient.RequestModel;
using FlujoCustomControl.Code.BussinessLogic;
using FlujoCustomControl.Helpers;
using System;
using System.Collections.Generic;
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
using static Flujo.Entities.WpfClient.RequestModel.CincoMinutosRequestModel;

namespace FlujoCustomControl.Views.UsersControls
{
    /// <summary>
    /// Lógica de interacción para ProcesarMovimientoUserControl.xaml
    /// </summary>
    public partial class CincoMinutosUserControl : UserControl
    {
       
        public CincoMinutosUserControl()
        {
          
            InitializeComponent();
            MainFlujoWindows.ProductoSelected =  Code.BussinessLogic.CincoMinutosLogic.SetProducto("CincoMinutos");
            GetVendidosHoy();
        }


        public void RefrescaBalance()
        {
            MainFlujoWindows parent = ((MainFlujoWindows)Window.GetWindow(this));
            try
            {
                int bancaid = parent.BancaID;
                parent.lbMain_BancaBalanceActual.Content = "|Balance : " + CajaLogic.GetBancaBalance(bancaid).ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            }
            catch (Exception ex)
            {
                parent.lbMain_BancaBalanceActual.Content = "|Balance : ...";
            }
        }
        //public variables
        List<TicketDetalle> TicketDetallesList = new List<TicketDetalle>();

        private void ShowConsultaTicketInicia(object serder, RoutedEventArgs e)
        {
            ShowConsultaTiket();
        }
        private void GetVendidosHoy()
        {
            var vendidosResponse = Code.BussinessLogic.CincoMinutosLogic.GetVendidosHoy();
            if (vendidosResponse.Tickets != null && vendidosResponse.Tickets.Any())
            {

                tbVentas.ItemsSource = vendidosResponse.Tickets.Select(x => new { x.TicketID, Hora = x.TicFecha, Monto = x.TicCosto, Activo = !x.TicNulo });
             
                   


                txtTotalVenta.Text = vendidosResponse.Tickets.Where(x => x.TicNulo == false).Sum(x => x.TicCosto).ToString("C0");
            }
            else
            {
                tbVentas.ItemsSource = null;
                txtTotalVenta.Text = "0.00";
            }
       
        }
        private void ShowConsultaTiket()
        {
            ConsultaApuesta consultaApuesta = new ConsultaApuesta();
            consultaApuesta.ShowDialog();
        }
        private void DefaultTextInputFocus(object sender, RoutedEventArgs e)
        {
            txtMonto.Focus();
           //RefrescaBalance();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
      

   

        private void AddItem()
        {


            int tipoJugadaId = 0;
            if ((txtJugada.Text.Length < 3))
            {
                tipoJugadaId =
                    1;
            }
            else if ((txtJugada.Text.Length < 5))
            {
                tipoJugadaId = 2;
            }
            else if ((txtJugada.Text.Length < 7))
            {
                tipoJugadaId = 3;
            }
            // Creando modelo
            var td = new TicketDetalle { Monto = Convert.ToInt32(txtMonto.Text), Jugada = SepararNumeros(txtJugada.Text, tipoJugadaId), TipoJugadaID = tipoJugadaId };
            // Agregando modelo a Vista
            TicketDetallesList.Add(td);
            // Sumando y Contando jugadas en base al modelo
            var SumaVariasJugadas = TicketDetallesList.Where(x => x.Jugada == td.Jugada.ToString()).Sum(x => x.Monto);
            var cantidadJugada = TicketDetallesList.Where(x => x.Jugada == td.Jugada.ToString()).Count();


            if (cantidadJugada > 1)
            {
                var jugada = TicketDetallesList.Where(x => x.Jugada == td.Jugada.ToString()).FirstOrDefault();
                jugada.Monto = SumaVariasJugadas;
                TicketDetallesList.Remove(td);
                //Vaciando y rellenando lista
                ltJugada.ItemsSource = null;
                ltJugada.ItemsSource = TicketDetallesList.Select(x => { return "$" + x.Monto + " al " + x.Jugada + TipoJugada(x.TipoJugadaID); }).ToList();
            }
            else
            {
                //Vaciando y rellenando lista
                ltJugada.ItemsSource = null;
                ltJugada.ItemsSource = TicketDetallesList.Select(x => { return "$" + x.Monto + " al " + x.Jugada + TipoJugada(x.TipoJugadaID); }).ToList();
            }

            //monto total de todas las jugadas
            //result.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
            txtMontoTotal.Text = TicketDetallesList.Sum(x => x.Monto).ToString("$#,##0", CultureInfo.CreateSpecificCulture("en-US"));
        }

        private void DeleteItem()
        {
            var jugadas = ltJugada.Items.Count;
            if (jugadas != 0)
            {
                var index = jugadas - 1;

                if (ltJugada.SelectedItem != null)
                {
                    //Obteniendo la jugada Selecionada
                    var jugadaSelect = ltJugada.SelectedItem.ToString().Split(' ');
                    RemoveJugada(jugadaSelect);

                }
                else
                {
                    //obteniendo la ultima jugada 
                    var jugadaIndex = ltJugada.Items[index].ToString().Split(' ');
                    RemoveJugada(jugadaIndex);

                }

            }
            else
            {
                MensajesAlerta("No hay mas Jugadas");
            }
        }

        private void RemoveJugada(string[] Jugada)
        {
            // encontrando la jugada en la lista
            var jugadaId = Jugada[2];
            var jugada = TicketDetallesList.Where(x => x.Jugada == jugadaId).FirstOrDefault();
            TicketDetallesList.Remove(jugada);
            //Vaciando y rellenando lista
            ltJugada.ItemsSource = null;
            ltJugada.ItemsSource = TicketDetallesList.Select(x => { return "$" + x.Monto + " al " + x.Jugada + TipoJugada(x.TipoJugadaID); }).ToList();
            //Recalculando monto
            if (TicketDetallesList.Sum(x => x.Monto) != 0)
            {
                txtMontoTotal.Text = TicketDetallesList.Sum(x => x.Monto).ToString("$#,##0", CultureInfo.CreateSpecificCulture("en-US"));

            }
            else
            {
                txtMontoTotal.Text = "$0.00";
            }
        }

        private string SepararNumeros(string jugadaIn, int tipojugadaIn)
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

        private void MensajesAlerta(string mensajeIn, int time = 3000)
        {
            lblMensaje.Text = mensajeIn;
            bxMensaje.Visibility = Visibility.Visible;

            var aTimer = new System.Timers.Timer(time);
            aTimer.Elapsed += (sender, e) => {

                Dispatcher.Invoke(new Action(() => { bxMensaje.Visibility = Visibility.Hidden; }), DispatcherPriority.ContextIdle);
            };
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private void RealizaApuesta()
        {
            var jugadas = ltJugada.Items.Count;


            CincoMinutosRequestModel.TicketModel ticketmodel = new CincoMinutosRequestModel.TicketModel();
            if (jugadas != 0)
            {
                try
                {
                    if (TicketDetallesList.Sum(x => x.Monto) > 499)
                    {
                        System.Windows.Forms.DialogResult result = System.Windows.Forms.MessageBox.Show("Esta segura del MONTO de: "
                                               + txtMontoTotal.Text + " ", "Confirmation", System.Windows.Forms.MessageBoxButtons.YesNo);
                        if (result == System.Windows.Forms.DialogResult.Yes)
                        {
                            ticketmodel.MontoOperacion = TicketDetallesList.Sum(x => x.Monto);
                            ticketmodel.TicketDetalles = TicketDetallesList;
                            var apuesta = Code.BussinessLogic.CincoMinutosLogic.Apuesta(ticketmodel);

                            if (apuesta.RespuestaApi.CodigoRespuesta == "100")
                            {
                                PrintOutHelper.SendToPrinter(apuesta.PrintData);
                                LimpiarApuesta();
                                MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta);
                                ticketmodel = null;
                                GetVendidosHoy();
                                //RefrescaBalance();
                            }
                            else
                            {
                                MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta);
                            }
                        }
                        else if (result == System.Windows.Forms.DialogResult.No)
                        {
                            //...
                        }
                        else
                        {
                            //...
                        }
                    }
                    else
                    {
                        ticketmodel.MontoOperacion = TicketDetallesList.Sum(x => x.Monto);
                        ticketmodel.TicketDetalles = TicketDetallesList;
                        var apuesta = Code.BussinessLogic.CincoMinutosLogic.Apuesta(ticketmodel);
                        if (apuesta == null)
                        {
                            MensajesAlerta("Error interno de aplicacion. Comunique el administrador");
                        }
                        else if (apuesta.RespuestaApi.CodigoRespuesta == "100")
                        {
                            PrintOutHelper.SendToPrinter(apuesta.PrintData);
                            LimpiarApuesta();
                            MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta);
                            ticketmodel = null;
                            GetVendidosHoy();
                            //RefrescaBalance();
                        }
                        else
                        {
                            MensajesAlerta(apuesta.RespuestaApi.MensajeRespuesta);
                        }
                    }





                }
                catch (Exception e)
                {

                    MensajesAlerta(e.StackTrace + "Fallo la transaccion de venta" + e.Message);
                }


            }
            else
            {
                MensajesAlerta("Agregue al menos una Jugada");
            }

        }

        private void FillTableUltimosSorteos()
        {
            //aqui pongo los ulimos sorteos hora, numero de sorteo premioss
            //tbSorteos.ItemsSource = lista;
        }
       
        private void LimpiarApuesta()
        {
            // Limpiando todo 
            TicketDetallesList.Clear();
            ltJugada.ItemsSource = null;
            txtMontoTotal.Text = "$0.00";
        }
        private string TipoJugada(int tipojujgada)
        {
            string tipojugada = tipojujgada == 1 ? "  Quiniela" : tipojujgada == 2 ? "  Pale" : tipojujgada == 3 ? "  Tripleta" : " ";
            return tipojugada;
        }

        private void DeleteItemList(object sender, RoutedEventArgs e)
        {
            DeleteItem();
        }

        private void AgregaJugada(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                txtMonto.Text = "";
                txtJugada.Text = "";
            }
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
                    else
                    {
                        s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                    }
                }
                e.Handled = true;
            }
        }

     

        private void Vender(object sender, RoutedEventArgs e)
        {
            RealizaApuesta();
            //MensajesAlerta("nose nada", 10000);
        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Subtract)
            {
                DeleteItem();
            }

            if (e.Key == Key.Add)
            {
                RealizaApuesta();
            }
            if (e.Key == Key.F5)
            {
                ShowConsultaTiket();
            }
        }

        private void TbVentas_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            //string t = "";
            //var model = (TicketResponseModel.Ticket)e.Row.Item;
            //if (e.Row["Activo"] == true)
            //{
            //    e.Row.IsEnabled = false;
            //    e.Row.Background = Brushes.LightGray;
            //}
            //else
            //{
            //    e.Row.IsEnabled = true;
            //    e.Row.Background = Brushes.White;
            //}
        }
    }
}
