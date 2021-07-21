using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
{
    class ReimprimirTicketCommand : ActionCommand
    {

        private readonly ValidarPagoTicketViewModel ViewModel;
        private readonly SorteosViewModel ViewModelSorteo;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        List<LoteriaTicketPin> loteriatickpin = new List<LoteriaTicketPin>() { };

        public ReimprimirTicketCommand(ValidarPagoTicketViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService,SorteosViewModel viewModelSorteo) 
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;
            ViewModelSorteo = viewModelSorteo;

            Action<object> comando = new Action<object>(ConsultarReimpresion);
            base.SetAction(comando);
        }

        private void ConsultarReimpresion(object parametro)
        {
            ViewModel.SetMensajeToDefaultSate();

            var numero = ViewModel.TicketNumero;
            var pin = ViewModel.TicketPin;
            var data = parametro as TicketCopiadoResponse;

            var MoreOptions = Autenticador.CurrentAccount.MAR_Setting2.MoreOptions.ToList();//Configuraciones de printer

            if ((!InputHelper.InputIsBlank(ViewModel.TicketNumero)))
            {

                bool ExistPrinterCOnfig = false;
                var contenedorTicketID = ViewModelSorteo.ListadoTicketsPrecargados.Where(x => x.TicketNo == ViewModel.TicketNumero).Select(x => x.Ticket);
                if (contenedorTicketID.Count() == 1)
                {
                    var TicketID = Convert.ToInt32(contenedorTicketID.ToArray()[0]);
                    var ReimprimirResponse = SorteosService.ReimprimirTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, TicketID);
                    if (ReimprimirResponse.Err == null) {
                        var jugadas = ReimprimirResponse;
                        TicketDTO jugadasCopy = new TicketDTO() { Costo = ReimprimirResponse.Costo, Err = ReimprimirResponse.Err, Fecha = Convert.ToDateTime(ReimprimirResponse.StrFecha), Loteria = ReimprimirResponse.Loteria, TicketNo = ReimprimirResponse.TicketNo, Nulo = ReimprimirResponse.Nulo, Pago = ReimprimirResponse.Pago, Solicitud = ReimprimirResponse.Solicitud, Ticket = ReimprimirResponse.Ticket };
                        jugadasCopy.Items = new JugadasDTO[ReimprimirResponse.Items.Length];
                        for (var i = 0; i < ReimprimirResponse.Items.Length; i++)
                        {
                            jugadasCopy.Items[i] = new JugadasDTO { QP = ReimprimirResponse.Items[i].QP, Cantidad = ReimprimirResponse.Items[i].Cantidad, Costo = ReimprimirResponse.Items[i].Costo, Loteria = ReimprimirResponse.Items[i].Loteria, Numero = ReimprimirResponse.Items[i].Numero, Pago = ReimprimirResponse.Items[i].Pago };

                        }

                        if (jugadas.Err == null)
                        {
                            var datosTicket = SessionGlobals.LoteriasTodas.Where(x => x.Numero == jugadas.Loteria).ToList();
                            var NombreLoteria = datosTicket[0].Nombre;

                            List<ConfigPrinterModel> listaConfiguraciones = new List<ConfigPrinterModel>() { };

                            var Pin = VentasIndexTicket.GeneraPinGanador(Convert.ToInt32(jugadas.Solicitud));

                            LoteriaTicketPin ticketPin = new LoteriaTicketPin() { Loteria = NombreLoteria, Pin = Pin, Ticket = jugadas.TicketNo };
                            loteriatickpin.Add(ticketPin);

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

                            List<JugadasTicketModels> jugadasNuevoSinPrinter = new List<JugadasTicketModels>() { };
                            List<TicketJugadas> listTicketJugdas = new List<TicketJugadas> { };
                            var firma = VentasIndexTicket.GeneraFirma(jugadas.StrFecha, jugadas.StrHora, jugadas.TicketNo, ReimprimirResponse.Items);

                            foreach (var jugada in jugadas.Items)
                            {
                                //Para Reimprimir ticket con configuraciones de printer

                                JugadaPrinter jugadaPrinter = new JugadaPrinter() { Numeros = jugada.Numero, Monto = (int)(jugada.Costo) };
                                TicketJugadas ticketJugadas = new TicketJugadas() { Jugada = jugadaPrinter, TipoJudaga = jugada.QP };
                                listTicketJugdas.Add(ticketJugadas);
                                ///////////////////////////////////////

                                JugadasTicketModels jugadaSinPrinter = new JugadasTicketModels() { Costo = Convert.ToInt32(jugada.Costo), Numero = jugada.Numero, TipoJugada = jugada.QP };
                                jugadasNuevoSinPrinter.Add(jugadaSinPrinter);
                            }

                            if (ExistPrinterCOnfig == true)
                            {
                                TicketValue ticketr = new TicketValue() { BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre, Direccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion, FechaActual = jugadas.StrFecha, Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono, Jugadas = listTicketJugdas, LoteriaTicketPin = loteriatickpin, Firma = firma, Texto = "Revise su jugada. Buena Suerte!", Total = "Total", AutorizacionHacienda = null, Logo = null };
                                TicketTemplateHelper.PrintTicket(ticketr, listaConfiguraciones, true);
                            }
                            else if (ExistPrinterCOnfig == false)
                            {
                                List<JugadasTicketModels> jugadaTransform = jugadasNuevoSinPrinter.ToList();

                                SorteosTicketModels TICKET = new SorteosTicketModels
                                {
                                    Costo = Convert.ToInt32(jugadas.Costo),
                                    Fecha = jugadas.StrFecha,
                                    TicketNo = jugadas.TicketNo,
                                    Nulo = jugadas.Nulo,
                                    Hora = jugadas.StrHora,
                                    Ticket = jugadas.Ticket,
                                    Loteria = NombreLoteria,
                                    LoteriaID = jugadas.Loteria,
                                    Jugadas = jugadaTransform,
                                    Pago = Convert.ToInt32(jugadas.Pago),
                                    Firma = firma,
                                    Pin = loteriatickpin,
                                    BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre,
                                    BanDireccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion,
                                    Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono,
                                    TextReviseJugada = "Revise su jugada. Buena Suerte!"
                                };
                                var multiples = TICKET.Pin.Count() > 1 ? true : false;
                                var TicketTemplate = CreateTemplateTextOnlyTicket(TICKET, multiples,true);
                                TicketTemplateHelper.PrintTicket(TicketTemplate, null, true);
                            }


                            ViewModel.SetMensaje(mensaje: "La reimpresion del ticket fue completada exitosamente.", icono: "Check", background: "#28A745", puedeMostrarse: true);

                        } else
                        {
                            //ViewModel.ListadoJugada = listJugadas;
                            Thread.Sleep(700);
                            //ViewModel.CerrarValidarPagoTicketCommand.Execute(null);
                            ViewModel.SetMensaje(mensaje: ReimprimirResponse.Err,
                                                   icono: "Check",
                                                   background: "#DC3545",
                                                   puedeMostrarse: true);

                        }
                    }
                    else
                    {
                        ViewModel.SetMensaje(mensaje:ReimprimirResponse.Err, icono: "Error", background: "#DC3545", puedeMostrarse: true);
                    }
                } else {
                    ViewModel.SetMensaje(mensaje: "El ticket introducido no existe o ya pasó el tiempo permitido para reimpresion", icono: "Error", background: "#DC3545", puedeMostrarse: true);
                }
            }

        }

        private List<string[,]> CreateTemplateTextOnlyTicket(SorteosTicketModels TICKET, bool multiples,bool Reimprimir=false)
        {
            List<string[]> Jugadas = new List<string[]>(); string[] arrayString = new string[1];
            List<string[]> QuinielaList = new List<string[]>(); List<string[]> PaleList = new List<string[]>(); List<string[]> TripletaList = new List<string[]>();
            List<string[]> ListTicketPin = new List<string[]>();
            string[] QuinielaArray = new string[1]; string[] PaleArray = new string[1]; string[] TripletaArray = new string[1];
            string[] TicketPinArray = new string[3];
            string[,] JugadasAndMontoArray = new string[1, 1];
            List<string[,]> ImprimirTicket = new List<string[,]>();
            try
            {
                double Total = 0;
                var posicionquiniela = 0; var posicionpale = 0; var posiciontripleta = 0;
                for (var i = 0; i < TICKET.Jugadas.Count(); i++)
                {
                    Total = Total + Convert.ToDouble(TICKET.Jugadas[i].Costo);
                    if (TICKET.Jugadas[i].TipoJugada == "Q") { if (posicionquiniela == 0) { QuinielaArray.SetValue("------ Quiniela ------", 0); posicionquiniela++; QuinielaList.Add(QuinielaArray); QuinielaArray = new string[2]; }; QuinielaArray.SetValue(TICKET.Jugadas[i].Numero, 0); QuinielaArray.SetValue(TICKET.Jugadas[i].Costo.ToString("C2"), 1); QuinielaList.Add(QuinielaArray); QuinielaArray = new string[2]; }
                    if (TICKET.Jugadas[i].TipoJugada == "P") { if (posicionpale == 0) { PaleArray.SetValue("-------- Pale --------", 0); posicionpale++; PaleList.Add(PaleArray); PaleArray = new string[2]; }; PaleArray.SetValue(TICKET.Jugadas[i].Numero, 0); PaleArray.SetValue(TICKET.Jugadas[i].Costo.ToString("C2"), 1); PaleList.Add(PaleArray); PaleArray = new string[2]; }
                    if (TICKET.Jugadas[i].TipoJugada == "T") { if (posiciontripleta == 0) { TripletaArray.SetValue("-------- Tripleta --------", 0); posiciontripleta++; TripletaList.Add(TripletaArray); TripletaArray = new string[2]; }; TripletaArray.SetValue(TICKET.Jugadas[i].Numero, 0); TripletaArray.SetValue(TICKET.Jugadas[i].Costo.ToString("C2"), 1); TripletaList.Add(TripletaArray); TripletaArray = new string[2]; }

                }
                if (QuinielaList.Count() > 0) { foreach (var quinielaJugada in QuinielaList) { Jugadas.Add(quinielaJugada); } };
                if (PaleList.Count() > 0) { foreach (var paleJugada in PaleList) { Jugadas.Add(paleJugada); } };
                if (TripletaList.Count() > 0) { foreach (var tripletaJugada in TripletaList) { Jugadas.Add(tripletaJugada); } };
                //Encabezados de seccion de ticket y pin
                TicketPinArray = new string[3];
                TicketPinArray.SetValue("   Loterias", 0);
                TicketPinArray.SetValue("Tickets ", 1);
                TicketPinArray.SetValue("Pin", 2);
                ListTicketPin.Add(TicketPinArray);
                /////////////////////////////////////////////
                TicketPinArray = new string[3];
                for (var i = 0; i < loteriatickpin.Count(); i++)
                {
                    TicketPinArray.SetValue(loteriatickpin[i].Loteria, 0);
                    TicketPinArray.SetValue(loteriatickpin[i].Ticket, 1);
                    TicketPinArray.SetValue(loteriatickpin[i].Pin, 2);
                    ListTicketPin.Add(TicketPinArray);
                    TicketPinArray = new string[3];
                }
                ReportesGeneralesJugadas JugadasTicketSinConfig = new ReportesGeneralesJugadas() { Fecha = Convert.ToDateTime(TICKET.Fecha), Mensaje = TICKET.TextReviseJugada, Firma = TICKET.Firma, onlyLoteria = TICKET.Loteria, onlyPin = TICKET.Pin[0].Pin.ToString(), onlyTicket = TICKET.TicketNo, Jugadas = Jugadas, Multiples = multiples, Reimprimir = false, TicketAndPin = ListTicketPin, Total = "Total:  " + Total.ToString("C2"), Hora = Convert.ToDateTime(TICKET.Hora) };
                ImprimirTicket = PrintJobs.PrintGeneralJugadas(JugadasTicketSinConfig, Autenticador,true);
                return ImprimirTicket;
            }
            catch (Exception e) { (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(e.Message, "ERROR", "FF0000"); }

            return ImprimirTicket;

        }

    }


}
