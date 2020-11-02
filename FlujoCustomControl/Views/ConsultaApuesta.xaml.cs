using FlujoCustomControl.Code.BussinessLogic;
using FlujoCustomControl.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace FlujoCustomControl.Views
{

 

    /// <summary>
    /// Interaction logic for ConsultaApuesta.xaml
    /// </summary>
    public partial class ConsultaApuesta : Window
    {
        public ConsultaApuesta()
        {
            InitializeComponent();
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

        private void FillTableApuesta(Flujo.Entities.WpfClient.RequestModel.CincoMinutosRequestModel.TicketModel pTicketModel)
        {
            tbApuesta.ItemsSource = pTicketModel.TicketDetalles.Select(x => new { x.Jugada, x.Monto });
        }

        private void PressTeclaOperation(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F12)
            {
                Imprimir();
            }
            else if (e.Key == Key.F6)
            {
                AnularApuesta();
               
            }
            else if (e.Key == Key.F5)
            {
                if (btnConsultar.Visibility == Visibility.Visible)
                {

                  ConsultarTicket();

                }
                else
                {
                    RealizarPago();
                }


 
            }

            }

       
        private void RealizarPago()
        {
            try
            {
                var pago = CincoMinutosLogic.RealizaPagoGanador(txtTicket.Text, txtPin.Text, decimal.Parse(lblTotal.Content.ToString().Replace("$", "")));
                if (pago.RespuestaApi.CodigoRespuesta == "100")
                {
                    PrintOutHelper.SendToPrinter(pago.PrintData);
                    MensajesAlerta(pago.RespuestaApi.MensajeRespuesta);
                    MensajesAlerta("El Pago fue Aprobado Exitosamente");
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
                var anula = CincoMinutosLogic.AnulaApuesta(txtTicket.Text, txtPin.Text);
                MensajesAlerta(anula.RespuestaApi);
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
                
                var consultaPago = CincoMinutosLogic.ConsultaPagoGanador(txtTicket.Text, txtPin.Text);

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
                    if (consultaPago.Ticket.Jugadas.Any())
                    {
                        var tabla = consultaPago.RespuestaApi.TicketDetalle.OrderBy(x => x.SorteoNumero).Select(x => new { Cantidad = x.Aposto, Jugada = x.Jugada, Sorteo = x.SorteoNumero });
                        tbApuesta.ItemsSource = tabla;//consultaPago.Ticket.Jugadas.Select(x => new { x.Cantidad, x.Jugada });
                        txtTotalApuesta.Text = consultaPago.Ticket.Jugadas.Sum(x => x.Monto).ToString("C0");
                        
                    }
                    switch (consultaPago.RespuestaApi.TicketEstado)
                    {
                        case Flujo.Entities.WpfClient.Enums.CincoMinutosEnum.TicketEstado.NoValido:
                            MensajesAlerta("NO PAGUE. El ticket no es valido.");
                            break;
                        case Flujo.Entities.WpfClient.Enums.CincoMinutosEnum.TicketEstado.JugadoNoSorteo:
                            MensajesAlerta("NO PAGUE. No han salido todos los sorteos del Ticket");
                            break;
                        case Flujo.Entities.WpfClient.Enums.CincoMinutosEnum.TicketEstado.JugadoNoGanador:
                            MensajesAlerta("NO PAGUE. El ticket NO es ganador");
                            break;
                        case Flujo.Entities.WpfClient.Enums.CincoMinutosEnum.TicketEstado.JugadoGanadorNoPago:
                            MostrarPagos(true);
                            decimal bancaBalance = CuadreLogic.GetBancaACuadrarMontoReal(MainFlujoWindows.MarSession.Banca);
                            var saco = consultaPago.RespuestaApi.TicketDetalle.Sum(x => x.Saco);
                          

                            MainFlujoWindows.DetalleJugadas.Juego = new List<Flujo.Entities.WpfClient.RequestModel.CincoMinutosRequestModel.JuegoPago>();
                            foreach (var item in consultaPago.RespuestaApi.TicketDetalle)
                            {
                                if (item.Jugada.Length<3)
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
                                MainFlujoWindows.DetalleJugadas.Juego.Add(new Flujo.Entities.WpfClient.RequestModel.CincoMinutosRequestModel.JuegoPago
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
                                MensajesAlerta("SACO: "+ saco.ToString("C2") +" Pero NO cuenta con suficiente Balance para PAGAR.",10000);
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
                        case Flujo.Entities.WpfClient.Enums.CincoMinutosEnum.TicketEstado.JugadoGanadorPagado:
                            MensajesAlerta("NO PAGUE. El ticket ya fue pagado anteriormente");
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                MensajesAlerta("Ingrese el Ticket y el Pin",3000);
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

                  ConsultarTicket();

                    if (s.Name == "txtPin")
                    {
                        txtTicket.Focus();

                    }
                    else
                    {
                        s.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

                    }

                }

                e.Handled = true;
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
            AnularApuesta();
        }

        private void ConsultaTicket(object sender, RoutedEventArgs e)
        {
            ConsultarTicket();
        }

        private void btnPago(object sender, RoutedEventArgs e)
        {
            RealizarPago();
        }

       
    }
}
