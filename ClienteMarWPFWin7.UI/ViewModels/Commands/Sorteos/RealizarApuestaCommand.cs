using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ClienteMarWPFWin7.UI.ViewModels.Commands.MainWindow;
using System.Globalization;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
{
    public class RealizarApuestaCommand: ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        List<LoteriaTicketPin> loteriatickpin = new List<LoteriaTicketPin>() { };
        private int contadorTIcket=0;
        public RealizarApuestaCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;
            

            Action<object> comando = new Action<object>(RealizarApuestas);
            base.SetAction(comando);
        }

        private void RealizarApuestas(object parametro)
        {
            var data = parametro as ApuestaResponse;
            if (data.Jugadas.Count == 1)
            {
                OnlyBet(data);
            }
            else if(data.Jugadas.Count > 1)
            {
                MultiBet(data);
            }

            SessionGlobals.GenerateNewSolicitudID(Autenticador.CurrentAccount.MAR_Setting2.Sesion.Sesion, true);
        }

        private void OnlyBet(ApuestaResponse apuesta)
        {
            var bet = new MAR_Bet();
            var itemBet = new List<MAR_BetItem>();

            foreach (var item in apuesta.Jugadas)
            {
                itemBet.Add(new MAR_BetItem
                {
                    Loteria = apuesta.LoteriaID,
                    Numero = item.Jugadas.Replace("-",""),
                    Costo = item.Monto,
                    Cantidad = item.Monto,
                    QP = item.TipoJugada.TrimStart().Substring(0, 1)

                });

                bet.Costo = item.Monto;
            }

            bet.Items = itemBet.ToArray();
            bet.Solicitud = SessionGlobals.SolicitudID;
           
            bet.Loteria = apuesta.LoteriaID;

            var MarBetResponse = SorteosService.RealizarApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, bet, SessionGlobals.SolicitudID, true);
            if (MarBetResponse.Err == null)
            {
                var ticket = new ArrayOfInt() { MarBetResponse.Ticket };
                SorteosView sorteo = new SorteosView();

                try
                {
                    //Window d = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("vistaSorteo")).FirstOrDefault();
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    SorteosService.ConfirmarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ticket);
                    ( Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta("Jugada realizada satisfactoriamente.", "Excelente");
                    //MessageBox.Show("Jugada realizada satisfactoriamente", "Confirmacion");
                    /////////////////////////////////////////////////////////////////////////////////////////////////////
                    try
                    {
                        ImprimirTickets(MarBetResponse, null);
                    }catch( Exception e)
                    {
                        (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(e.Message, "Aviso");
                    }
                    
                }catch(Exception e)
                {
                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(e.Message, "Aviso");

                }
                //TemplateTicketHelper.
                //SorteosService.ConfirmarApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion);
            }
            else
            {
                MessageBox.Show(MarBetResponse.Err,"Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }   

        }

        public void ImprimirTickets(MAR_Bet MarBetResponse, MAR_MultiBet multi)
        {
            if (MarBetResponse != null) { 
                List<JugadasTicketModels> jugadasNuevoSinPrinter = new List<JugadasTicketModels>() { };
                List<TicketJugadas> jugadasTicket = new List<TicketJugadas>() { };
                
                List<MAR_BetItem> JugadasForTicketPrecargado = new List<MAR_BetItem>() { };


                //ConfigPrinterValue
                List<ConfigPrinterModel> configmodel = new List<ConfigPrinterModel>() { };

                foreach (var jugada in MarBetResponse.Items)
                {
                    ////Ticket para impresion con configuracion de printer
                    JugadaPrinter jugadaprinter = new JugadaPrinter() { Monto = Convert.ToInt32(jugada.Costo), Numeros = jugada.Numero };
                    TicketJugadas jugadass = new TicketJugadas() { Jugada = jugadaprinter, TipoJudaga = jugada.QP };
                    jugadasTicket.Add(jugadass);

                    //// Ticket para impresion sin coniguracion de printer
                    JugadasTicketModels jugad = new JugadasTicketModels() { Costo = Convert.ToInt32(jugada.Costo), Numero = jugada.Numero, TipoJugada = jugada.QP };
                    jugadasNuevoSinPrinter.Add(jugad);

                    //// Ticket para los ticket pre cargados
                    MAR_BetItem jugadasTicketForPrecargados = new MAR_BetItem() { Numero=jugada.Numero,Costo=jugada.Costo,Cantidad=jugada.Cantidad,Loteria=jugada.Loteria,Pago=jugada.Pago,QP=jugada.QP };
                    JugadasForTicketPrecargado.Add(jugadasTicketForPrecargados);
                }


                List<JugadasTicketModels> jugadaTransform = jugadasNuevoSinPrinter.ToList();

                List<ConfigPrinterModel> listaConfiguraciones = new List<ConfigPrinterModel>() { };


                var MoreOptions = Autenticador.CurrentAccount.MAR_Setting2.MoreOptions.ToList();
                bool ExistPrinterCOnfig = false;
                int CantidadLoterias = ViewModel.LoteriasMultiples.Count;
                
                var firma = VentasIndexTicket.GeneraFirma(MarBetResponse.StrFecha, MarBetResponse.StrHora, MarBetResponse.TicketNo, MarBetResponse.Items);
                var datosTicket = SessionGlobals.LoteriasTodas.Where(x => x.Numero == MarBetResponse.Loteria).ToList();
                var NombreLoteria = datosTicket[0].Nombre;
                var Pin = VentasIndexTicket.GeneraPinGanador(Convert.ToInt32(MarBetResponse.Solicitud));
                var NumeroTicket = MarBetResponse.TicketNo;
                
                LoteriaTicketPin ticketPin = new LoteriaTicketPin() { Loteria = NombreLoteria, Pin = Pin, Ticket = NumeroTicket };
                if (contadorTIcket <= CantidadLoterias) { 
                    loteriatickpin.Add(ticketPin);
                    contadorTIcket = contadorTIcket + 1;
                }
                
                SorteosTicketModels TICKET = new SorteosTicketModels
                {
                    Costo = Convert.ToInt32(MarBetResponse.Costo),
                    Fecha = MarBetResponse.StrFecha,
                    TicketNo = MarBetResponse.TicketNo,
                    Nulo = MarBetResponse.Nulo,
                    Hora = MarBetResponse.StrHora,
                    Ticket = MarBetResponse.Ticket,
                    Loteria =NombreLoteria,
                    LoteriaID = MarBetResponse.Loteria,
                    Jugadas = jugadaTransform,
                    Pago = Convert.ToInt32(MarBetResponse.Pago),
                    Firma = firma,
                    Pin =loteriatickpin,
                    BanNombre=Autenticador.BancaConfiguracion.BancaDto.BanNombre,
                    BanDireccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion,
                    Telefono=Autenticador.BancaConfiguracion.BancaDto.BanTelefono,
                    TextReviseJugada="Revise su jugada. Buena Suerte!"
                };

                TicketValue ticketr = new TicketValue() { BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre, Direccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion, FechaActual = MarBetResponse.StrFecha, Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono, Jugadas = jugadasTicket, LoteriaTicketPin = loteriatickpin, Firma = firma, Texto = "Revise su jugada. Buena Suerte!", Total = "Total", AutorizacionHacienda = null, Logo = null };

                //Agregando ticket a listado ticket precargado
                MAR_Bet ticketForTicketRecargados = new MAR_Bet() { Items=MarBetResponse.Items,Pago=MarBetResponse.Pago,Loteria=MarBetResponse.Loteria,Costo=MarBetResponse.Costo,Cedula=MarBetResponse.Cedula,Cliente=MarBetResponse.Cliente,Grupo=MarBetResponse.Grupo,Nulo=MarBetResponse.Nulo,Err=MarBetResponse.Err,Solicitud=MarBetResponse.Solicitud,StrFecha=MarBetResponse.StrFecha, StrHora=MarBetResponse.StrHora,Ticket=MarBetResponse.Ticket,TicketNo=MarBetResponse.TicketNo };
                ViewModel.ListadoTicketsPrecargados.Add(ticketForTicketRecargados);

                ///////////////////////////////////////////
                for (var i = 0; i < MoreOptions.Count; i++)
                {
                    var ConfigValue = MoreOptions[i];
                    var ArrayValue = ConfigValue.Split('|');
                    if (ArrayValue[0] == "BANCA_PRINTER_CONFIG_LINE")
                    {
                        ConfigPrinterModel configuracionConfigLine = new ConfigPrinterModel { ConfigKey = ArrayValue[0].ToString(), ConfigValue = ArrayValue[1].Replace('"', Convert.ToChar("'")).ToString() };
                        configuracionConfigLine.ConfigValue.Replace(Convert.ToChar("'"), '"').ToString();
                        ExistPrinterCOnfig = true;
                        listaConfiguraciones.Add(configuracionConfigLine);
                    }
                    if (ArrayValue[0] == "BANCA_PRINTER_IMAGES_CONFIG")
                    {
                        ConfigPrinterModel configuracionConfigImage = new ConfigPrinterModel { ConfigKey = ArrayValue[0].ToString(), ConfigValue = ArrayValue[1].Replace('"', Convert.ToChar("'")).ToString() };
                        configuracionConfigImage.ConfigValue.Replace(Convert.ToChar("'"), '"').ToString();
                        ExistPrinterCOnfig = true;
                        listaConfiguraciones.Add(configuracionConfigImage);
                    }

                }
                if (ExistPrinterCOnfig == true)
                {
                    if (contadorTIcket==CantidadLoterias) { 
                        TicketTemplateHelper.PrintTicket(ticketr, listaConfiguraciones,false,CantidadLoterias);
                        loteriatickpin = new List<LoteriaTicketPin>();
                        contadorTIcket = 0;
                    }
                }
                if (ExistPrinterCOnfig == false)
                {
                    //TicketValue ticketr = new TicketValue() { BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre, Direccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion, FechaActual = MarBetResponse.StrFecha, Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono, Jugadas = jugadasTicket, LoteriaTicketPin = loteriatickpin, Firma = "firma", Texto = "ok", Total = "Total", AutorizacionHacienda = null, Logo = null };

                    //List<string[]> ImprimirTicket = PrintJobs.FromTicket(TICKET, Autenticador, false);
                    //TicketTemplateHelper.PrintTicket(TICKET, listaConfiguraciones);
                    if (contadorTIcket == CantidadLoterias)
                    {
                        TicketTemplateHelper.PrintTicket(TICKET,null,false, CantidadLoterias);
                        loteriatickpin = new List<LoteriaTicketPin>();
                        contadorTIcket = 0;
                    }
                }
                string total = ViewModel.ListadoTicketsPrecargados.Sum(x => x.Costo).ToString("C", CultureInfo.CurrentCulture);
                ViewModel.TotalVentas = total;

            }
            if (multi != null)
            {
              
                List<VentasIndexTicket.Jugada> jugadas = new List<VentasIndexTicket.Jugada>() { };
                List<JugadasTicketModels> jugadasNuevoSinPrinter = new List<JugadasTicketModels>() { };
                List<VentasIndexTicket> listMulti = new List<VentasIndexTicket>() { };
                List<TicketJugadas> jugadasTicket = new List<TicketJugadas>() { };
                //List<LoteriaTicketPin> loteriatickpin = new List<LoteriaTicketPin>() { };
                //ConfigPrinterValue
                List<ConfigPrinterModel> configmodel = new List<ConfigPrinterModel>() { };
                List<MAR_BetItem> JugadasForTicketPrecargado = new List<MAR_BetItem>() { };

                foreach (var jugada in multi.Items)
                {

                    JugadaPrinter jugadaprinter = new JugadaPrinter() { Monto = Convert.ToInt32(jugada.Costo), Numeros = jugada.Numero };

                    Jugada jugadaYY = new Jugada() { Jugadas = jugada.Numero, Monto = Convert.ToInt32(jugada.Costo), TipoJugada = jugada.QP };
                    TicketJugadas jugadass = new TicketJugadas() { Jugada = jugadaprinter, TipoJudaga = jugada.QP };
                    jugadasTicket.Add(jugadass);

                    //VentasIndexTicket.Jugada jugad = new VentasIndexTicket.Jugada() { Total = jugada.Pago, Cantidad = jugada.Cantidad, Numero = jugada.Numero, Precio = jugada.Costo, Tipo = jugada.QP };
                    //jugadas.Add(jugad);

                    JugadasTicketModels jugad = new JugadasTicketModels() { Costo = Convert.ToInt32(jugada.Costo), Numero = jugada.Numero, TipoJugada = jugada.QP };
                    jugadasNuevoSinPrinter.Add(jugad);

                    //// Ticket para los ticket pre cargados
                    MAR_BetItem jugadasTicketForPrecargados = new MAR_BetItem() { Numero = jugada.Numero, Costo = jugada.Costo, Cantidad = jugada.Cantidad, Loteria = jugada.Loteria, Pago = jugada.Pago, QP = jugada.QP };
                    JugadasForTicketPrecargado.Add(jugadasTicketForPrecargados);
                }
                //VentasIndexTicket.Jugada[] jugadaTransform = jugadas.ToArray();
                List<JugadasTicketModels> jugadaTransform = jugadasNuevoSinPrinter.ToList();

                List<ConfigPrinterModel> listaConfiguraciones = new List<ConfigPrinterModel>() { };
                int CantidadLoterias = ViewModel.LoteriasMultiples.Count;


                var MoreOptions = Autenticador.CurrentAccount.MAR_Setting2.MoreOptions.ToList();
                bool ExistPrinterCOnfig = false;
                var Headers = multi.Headers[0];

                var firma = VentasIndexTicket.GeneraFirma(Headers.StrFecha, Headers.StrHora, Headers.TicketNo, multi.Items);
                var datosTicket = SessionGlobals.LoteriasTodas.Where(x => x.Numero == Headers.Loteria).ToList();
                var NombreLoteria = datosTicket[0].Nombre;
                var Pin = VentasIndexTicket.GeneraPinGanador(Convert.ToInt32(Headers.Solicitud));
                var NumeroTicket = Headers.TicketNo;
                
                LoteriaTicketPin ticketPin = new LoteriaTicketPin() { Loteria = NombreLoteria, Pin = Pin, Ticket = NumeroTicket };
              
                if (contadorTIcket <= CantidadLoterias)
                {
                    loteriatickpin.Add(ticketPin);
                    contadorTIcket = contadorTIcket + 1;
                }

                //VentasIndexTicket TICKET = new VentasIndexTicket()
                //{
                //    Costo = Headers.Costo,
                //    Fecha = Headers.StrFecha,
                //    TicketNo = Headers.TicketNo,
                //    Nulo = Headers.Nulo,
                //    Hora = Headers.StrHora,
                //    Ticket = Headers.Ticket,
                //    SorteoNombre = NombreLoteria,
                //    Sorteo = Headers.Loteria,
                //    Jugadas = jugadaTransform,
                //    Pago = Headers.Pago,
                //    Firma = "Firma",
                //    Pin = Pin
                //};

                SorteosTicketModels TICKET = new SorteosTicketModels
                {
                    Costo = Convert.ToInt32(Headers.Costo),
                    Fecha = Headers.StrFecha,
                    TicketNo = Headers.TicketNo,
                    Nulo = Headers.Nulo,
                    Hora = Headers.StrHora,
                    Ticket = Headers.Ticket,
                    Loteria = NombreLoteria,
                    LoteriaID = Headers.Loteria,
                    Jugadas = jugadaTransform,
                    Pago = Convert.ToInt32(Headers.Pago),
                    Firma = firma,
                    Pin = loteriatickpin,
                    BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre,
                    BanDireccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion,
                    Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono,
                    TextReviseJugada = "Revise su jugada. Buena Suerte!"
                };
                

                TicketValue ticketr = new TicketValue() { BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre, Direccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion, FechaActual = Headers.StrFecha, Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono, Jugadas = jugadasTicket, LoteriaTicketPin = loteriatickpin, Firma = firma, Texto = "Revise su jugada. Buena Suerte!", Total = "Total", AutorizacionHacienda = null, Logo = null };

                MAR_Bet ticketForTicketRecargados = new MAR_Bet() { Items = multi.Items, Pago = Headers.Pago, Loteria = Headers.Loteria, Costo = Headers.Costo, Cedula = Headers.Cedula, Cliente = Headers.Cliente, Grupo = Headers.Grupo, Nulo = Headers.Nulo, Err = null, Solicitud = Headers.Solicitud, StrFecha = Headers.StrFecha, StrHora = Headers.StrHora, Ticket = Headers.Ticket, TicketNo = Headers.TicketNo };
                ViewModel.ListadoTicketsPrecargados.Add(ticketForTicketRecargados);

                for (var i = 0; i < MoreOptions.Count; i++)
                {
                    var ConfigValue = MoreOptions[i];
                    var ArrayValue = ConfigValue.Split('|');
                    if (ArrayValue[0] == "BANCA_PRINTER_CONFIG_LINE")
                    {
                        ConfigPrinterModel configuracionConfigLine = new ConfigPrinterModel { ConfigKey = ArrayValue[0].ToString(), ConfigValue = ArrayValue[1].Replace('"', Convert.ToChar("'")).ToString() };
                        configuracionConfigLine.ConfigValue.Replace(Convert.ToChar("'"), '"').ToString();
                        ExistPrinterCOnfig = true;
                        listaConfiguraciones.Add(configuracionConfigLine);
                    }
                    if (ArrayValue[0] == "BANCA_PRINTER_IMAGES_CONFIG")
                    {
                        ConfigPrinterModel configuracionConfigImage = new ConfigPrinterModel { ConfigKey = ArrayValue[0].ToString(), ConfigValue = ArrayValue[1].Replace('"', Convert.ToChar("'")).ToString() };
                        configuracionConfigImage.ConfigValue.Replace(Convert.ToChar("'"), '"').ToString();
                        ExistPrinterCOnfig = true;
                        listaConfiguraciones.Add(configuracionConfigImage);
                    }

                }
                if (ExistPrinterCOnfig == true)
                {
                    if (contadorTIcket == CantidadLoterias)
                    {
                        TicketTemplateHelper.PrintTicket(ticketr, listaConfiguraciones,false,CantidadLoterias);
                        loteriatickpin = new List<LoteriaTicketPin>();
                        contadorTIcket = 0;
                    }
                }
                if (ExistPrinterCOnfig == false)
                {
                    //TicketValue ticketr = new TicketValue() { BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre, Direccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion, FechaActual = MarBetResponse.StrFecha, Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono, Jugadas = jugadasTicket, LoteriaTicketPin = loteriatickpin, Firma = "firma", Texto = "ok", Total = "Total", AutorizacionHacienda = null, Logo = null };

                    //List<string[]> ImprimirTicket = PrintJobs.FromTicket(TICKET, Autenticador, false);
                    //TicketTemplateHelper.PrintTicket(TICKET, listaConfiguraciones);
                    if (contadorTIcket == CantidadLoterias)
                    {
                        TicketTemplateHelper.PrintTicket(TICKET,null,false,CantidadLoterias);
                        loteriatickpin = new List<LoteriaTicketPin>();
                        contadorTIcket = 0;
                    }
                }
                string total = ViewModel.ListadoTicketsPrecargados.Sum(x => x.Costo).ToString("C", CultureInfo.CurrentCulture);
                ViewModel.TotalVentas = total;
            }
        }

        private void MultiBet(ApuestaResponse apuestas)
        {

            var multi = new MAR_MultiBet();
            var itemBet = new List<MAR_BetItem>();
            
            foreach (var item in apuestas.Jugadas)
            {
                itemBet.Add(new MAR_BetItem
                {
                    Loteria = apuestas.LoteriaID,
                    Numero = item.Jugadas.Replace("-",""),
                    Costo = item.Monto,
                    QP = item.TipoJugada.TrimStart().Substring(0, 1),
                    Cantidad = item.Monto

                });
            }
            multi.Items = itemBet.ToArray();
            multi.Headers = new MAR_BetHeader[] {
                new MAR_BetHeader {
                    Solicitud = SessionGlobals.SolicitudID,
                    Loteria = apuestas.LoteriaID,
                    Costo = apuestas.Jugadas.Sum(x => x.Monto),
                    StrFecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    StrHora=DateTime.Now.ToString("hh:mm:ss")

        } };
            
            var MultiBetResponse = SorteosService.RealizarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, multi);
            if (MultiBetResponse.Err == null)
            {
                var ticket = new ArrayOfInt();
                var headerResponse = MultiBetResponse.Headers.OfType<MAR_BetHeader>().ToList();
                if (headerResponse.Count > 0)
                {
                    foreach (var item in headerResponse)
                    {
                        ticket.Add(item.Ticket);
                    }

                }

                /////////////////// Asignado cmapos faltantes en multi /////////////
                multi.Headers[0].TicketNo = MultiBetResponse.Headers[0].TicketNo;
                multi.Headers[0].Ticket = MultiBetResponse.Headers[0].Ticket;
                multi.Headers[0].Cedula = MultiBetResponse.Headers[0].Cedula;
                multi.Headers[0].Nulo = MultiBetResponse.Headers[0].Nulo;
                multi.Headers[0].StrHora = MultiBetResponse.Headers[0].StrHora;
                multi.Headers[0].StrFecha = MultiBetResponse.Headers[0].StrFecha;
                multi.Headers[0].Pago = MultiBetResponse.Headers[0].Pago;
                multi.Headers[0].Costo = MultiBetResponse.Headers[0].Costo;
                multi.Headers[0].Cliente = MultiBetResponse.Headers[0].Cliente;
                ///////////////////////////////////////////////////////////////////
                try { 
                    SorteosService.ConfirmarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ticket);

                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta("Jugada realizada satisfactoriamente.", "Excelente");
                    ImprimirTickets(null, multi);

                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
            }
            else
            {
                MessageBox.Show(MultiBetResponse.Err, "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }


        }

    }
}
