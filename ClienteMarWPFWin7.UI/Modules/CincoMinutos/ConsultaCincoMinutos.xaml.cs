using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Data.Services;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using static ClienteMarWPFWin7.Domain.Models.Dtos.CincoMinutosRequestModel;

namespace ClienteMarWPFWin7.UI.Modules.CincoMinutos
{
    /// <summary>
    /// Lógica de interacción para ConsultaCincoMinutos.xaml
    /// </summary>
    public partial class ConsultaCincoMinutos : Window
    {

        private bool AnularThreadIsBusy = false;
        private bool ConsultarThreadIsBusy = false;

        public ConsultaCincoMinutos()
        {
            InitializeComponent();
            Spinner.Visibility = Visibility.Collapsed;
            Spinner2.Visibility = Visibility.Collapsed;
        }
        public void MensajesAlerta(string mensajeIn, int time = 3000)
        {
            lblMensaje.Text = mensajeIn;
            bxMensaje.Visibility = Visibility.Visible;

            var aTimer = new System.Timers.Timer(time);
            aTimer.Elapsed += (sender, e) =>
            {

                Dispatcher.Invoke(new Action(() => { bxMensaje.Visibility = Visibility.Hidden; }), DispatcherPriority.ContextIdle);
            };
            aTimer.AutoReset = false;
            aTimer.Enabled = true;
        }

        private void PressTeclaOperation(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                Imprimir();
            }
            else if (e.Key == Key.F6)
            {

                btnAnular(sender, e);
            }else if(e.Key == Key.F7)
            {
                BtnClosePago(sender, e);
            }
            else if (e.Key == Key.F5)
            {
                
                if (btnConsultar.Visibility == Visibility.Visible)
                {

                    ConsultaTicket(sender, e);

                }
                else
                {
                    RealizarPago();
                }
            }
            else if (e.Key == Key.Escape)
            {
                this.Close();
            }
            //else if (e.Key == Key.F5)
            //{
            //    if (btnConsultar.Visibility == Visibility.Visible)
            //    {

            //        ConsultarTicket();

            //    }
            //    else
            //    {
            //        RealizarPago();
            //    }



            //}

        }


        private void RealizarPago()
        {
            try
            {

                var pago = ClienteMarWPFWin7.Data.Services.CincoMinutosDataService.RealizaPagoGanador(txtTicket.Text, txtPin.Text, decimal.Parse(lblTotal.Content.ToString().Replace("$", "")), SessionGlobals.cuentaGlobal);
                if (pago.RespuestaApi.CodigoRespuesta == "100")
                {
                    PrintOutHelper.SendToPrinter(pago.PrintData);
                    MensajesAlerta(pago.RespuestaApi.MensajeRespuesta);
                    MensajesAlerta("El pago fue aprobado exitosamente");
                    MostrarConsultarPagar(false);
                }
                else
                {
                    MensajesAlerta(pago.RespuestaApi.MensajeRespuesta);
                }
            }
            catch (Exception e)
            {

                MensajesAlerta(e.Message);
            }

        }

        private void AnularApuesta()
        {
            try
            {
                if (txtTicket.Text != "" && txtPin.Text != "")
                {
                    var VM = DataContext as SorteosViewModel;
                    var cincoMinutoService = new CincoMinutosDataService();
                    var anula = ClienteMarWPFWin7.Data.Services.CincoMinutosDataService.AnulaApuesta(txtTicket.Text, txtPin.Text, SessionGlobals.cuentaGlobal);
                    MensajesAlerta(anula.RespuestaApi);
                }
                else
                {
                    MensajesAlerta("Ingrese el ticket y el pin", 3000);
                    txtTicket.Focus();
                }


            }
            catch (Exception e)
            {

                MensajesAlerta(e.Message);
            }
        }

        private void Imprimir()
        {
            MensajesAlerta("Imprimiendo Ticket Pago");
        }

        private void ConsultarTicket()
        {

            if (txtTicket.Text != "" && txtPin.Text != "")
            {

                var consultaPago = ClienteMarWPFWin7.Data.Services.CincoMinutosDataService.ConsultaPagoGanador(txtTicket.Text, txtPin.Text, SessionGlobals.cuentaGlobal);

                if (!consultaPago.OK)
                {
                    MensajesAlerta(consultaPago.Error);
                    tbApuesta.ItemsSource = null;
                    txtTotalApuesta.Text = "0.00";
                }
                else
                {

                    //probando
                    //consultaPago.RespuestaApi.TicketEstado = Flujo.Entities.WpfClient.Enums.CincoMinutosEnum.TicketEstado.JugadoGanadorNoPago;
                    //consultaPago.RespuestaApi.Saco = 0;
                    if (consultaPago.RespuestaApi.TicketDetalle.Any())
                    {
                        var tabla = consultaPago.RespuestaApi.TicketDetalle.OrderBy(x => x.SorteoNumero).Select(x => new { Cantidad = x.Aposto, Jugada = x.Jugada, Sorteo = x.SorteoNumero });
                        tbApuesta.ItemsSource = tabla;//consultaPago.Ticket.Jugadas.Select(x => new { x.Cantidad, x.Jugada });
                        txtTotalApuesta.Text = consultaPago.RespuestaApi.TicketDetalle.Sum(x => x.Aposto).ToString("C0");

                    }
                    switch (consultaPago.RespuestaApi.TicketEstado)
                    {
                        case ClienteMarWPFWin7.Domain.Enums.CincoMinutosEnum.TicketEstado.NoValido:
                            MensajesAlerta("NO PAGUE. El ticket no es valido.");
                            break;
                        case ClienteMarWPFWin7.Domain.Enums.CincoMinutosEnum.TicketEstado.JugadoNoSorteo:
                            MensajesAlerta("NO PAGUE. No han salido todos los sorteos del Ticket");
                            break;
                        case ClienteMarWPFWin7.Domain.Enums.CincoMinutosEnum.TicketEstado.JugadoNoGanador:
                            MensajesAlerta("NO PAGUE. El ticket NO es ganador");
                            break;
                        case ClienteMarWPFWin7.Domain.Enums.CincoMinutosEnum.TicketEstado.JugadoGanadorNoPago:
                            MostrarPagos(true);
                            //decimal bancaBalance = CuadreLogic.GetBancaACuadrarMontoReal(SessionGlobals.cuentaGlobal.MAR_Setting2.Sesion.Banca); PENDIENTE VALIDAR SI TIENE CONTROL DE EFECTIVO
                            var saco = consultaPago.RespuestaApi.TicketDetalle.Sum(x => x.Saco);


                            ClienteMarWPFWin7.Domain.Models.Dtos.CincoMinutosRequestModel.DetalleJugadas.Juego = new List<ClienteMarWPFWin7.Domain.Models.Dtos.CincoMinutosRequestModel.JuegoPago>();
                            foreach (var item in consultaPago.RespuestaApi.TicketDetalle)
                            {
                                if (item.Jugada.Length < 3)
                                {
                                    item.TipoJugadaID = 1;
                                }
                                else if (item.Jugada.Length < 6)
                                {
                                    item.TipoJugadaID = 2;
                                }
                                else
                                {
                                    item.TipoJugadaID = 3;
                                }
                                ClienteMarWPFWin7.Domain.Models.Dtos.CincoMinutosRequestModel.DetalleJugadas.Juego.Add(new ClienteMarWPFWin7.Domain.Models.Dtos.CincoMinutosRequestModel.JuegoPago
                                {
                                    Codigo = item.Codigo,
                                    Jugada = item.Jugada,
                                    MontoApostado = item.Aposto,
                                    MontoPagado = item.Saco,
                                    TipoJugadaID = item.TipoJugadaID
                                });
                            }

                            if ((decimal)saco < 1)// desactivado hasta nuevo aviso bancaBalance < (decimal)saco)
                            {
                                MensajesAlerta("SACO: " + saco.ToString("C2") + " Pero NO cuenta con suficiente Balance para PAGAR.", 10000);
                                lblTotal.Content = saco.ToString("C2");
                                MostrarPagos(false);
                            }
                            else
                            {
                                txtNota.Text = "Puede Pagar. Verifique si tiene dinero y precione Pagar";
                                lblTotal.Content = saco.ToString("C2");
                                MostrarPagos(true);
                            }

                            break;
                        case ClienteMarWPFWin7.Domain.Enums.CincoMinutosEnum.TicketEstado.JugadoGanadorPagado:
                            MensajesAlerta("NO PAGUE. El ticket ya fue pagado anteriormente");
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                MensajesAlerta("Ingrese el ticket y el pin", 3000);
                txtTicket.Focus();
            }

        }

        private void btnCloseWindows(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void BtnClosePago(object sender, RoutedEventArgs e)
        {
            MostrarPagos(false);
        }
        private void MostrarPagos(bool mostrar)
        {
            if (mostrar)
            {
                stkBotones.Visibility = Visibility.Collapsed;
                stkDetalle.Visibility = Visibility.Collapsed;
                PagosWindows.Visibility = Visibility.Visible;
                txtTicket.IsEnabled = false;
                txtPin.IsEnabled = false;
            }
            else
            {
                stkBotones.Visibility = Visibility.Visible;
                stkDetalle.Visibility = Visibility.Visible;
                PagosWindows.Visibility = Visibility.Collapsed;
                txtTicket.IsEnabled = true;
                txtPin.IsEnabled = true;

            }
        }
        private void MostrarConsultarPagar(bool pagar)
        {
            if (pagar)
            {

                //Mostrando pagar 
                btnPagar.Visibility = Visibility.Visible;
                btnConsultar.Visibility = Visibility.Collapsed;
            }
            else
            {
                //Mostrando pagar 
                btnPagar.Visibility = Visibility.Collapsed;
                btnConsultar.Visibility = Visibility.Visible;
            }
        }

        private void PressTecla(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox s = e.Source as TextBox;


                if (s != null)
                {

                    //ConsultarTicket();

                    if (s.Name == "txtPin")
                    {
                        txtTicket.Focus();
                        TriggerButtonClickEvent(btnSeleccionaAuth);
                    }
                    else
                    {
                        s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        TriggerButtonClickEvent(btnSeleccionaPin);
                    }

                }

                e.Handled = true;
            }

            if (e.Key == Key.Right)
            {
                TextBox s = e.Source as TextBox;

                if (s != null)
                {
                    if (s.Name == "txtTicket")
                    {
                        txtPin.Focus();
                        TriggerButtonClickEvent(btnSeleccionaPin);
                    }
                }
            }

            if (e.Key == Key.Left)
            {
                TextBox s = e.Source as TextBox;

                if (s != null)
                {
                    if (s.Name == "txtPin")
                    {
                        txtTicket.Focus();
                        TriggerButtonClickEvent(btnSeleccionaAuth);
                    }
                }

            }

        }

        private void ProcesarPago(object sender, RoutedEventArgs e)
        {
            RealizarPago();
            MostrarPagos(false);
        }

        private void SelectCampo(object sender, RoutedEventArgs e)
        {
            txtTicket.Focus();
        }

        private void btnImprimir(object sender, RoutedEventArgs e)
        {
            Imprimir();
        }

        private void btnAnular(object sender, RoutedEventArgs e)
        {
            if (AnularThreadIsBusy == false)
            {
                Spinner.Visibility = Visibility.Visible;

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    AnularThreadIsBusy = true;
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() =>
                    {
                        try
                        {
                            try
                            {
                                AnularApuesta();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        catch
                        {

                        }
                        AnularThreadIsBusy = false;
                        Spinner.Visibility = Visibility.Collapsed;
                    }));
                });
            }

        }

        private void ConsultaTicket(object sender, RoutedEventArgs e)
        {
            if (ConsultarThreadIsBusy == false)
            {
                Spinner2.Visibility = Visibility.Visible;

                Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000);
                    ConsultarThreadIsBusy = true;
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(
                    DispatcherPriority.Background,
                    new Action(() =>
                    {
                        try
                        {
                            try
                            {
                                ConsultarTicket();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        catch
                        {

                        }
                        ConsultarThreadIsBusy = false;
                        Spinner2.Visibility = Visibility.Collapsed;
                    }));
                });
            }
        }

        //private void btnPago(object sender, RoutedEventArgs e)
        //{
        //    RealizarPago();
        //}

        private void SeleccionarAuth(object sender, RoutedEventArgs e)
        {
            txtTicket.Focus();
            txtTicket.SelectAll();
        }

        private void SeleccionarPin(object sender, RoutedEventArgs e)
        {
            txtPin.Focus();
            txtPin.SelectAll();
        }

        public void TriggerButtonClickEvent(Button boton)
        {
            try
            {

                if (boton != null)
                {
                    var peer = new ButtonAutomationPeer(boton);
                    var invokeProvider = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                    invokeProvider?.Invoke();
                }
            }
            catch { }
        }
    }
}
