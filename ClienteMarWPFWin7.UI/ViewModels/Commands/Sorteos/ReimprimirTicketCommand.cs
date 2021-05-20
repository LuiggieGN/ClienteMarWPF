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
                    var jugadas = ReimprimirResponse;
                    if (jugadas.Err == null)
                    {
                        List<LoteriaTicketPin> loteriatickpin = new List<LoteriaTicketPin>() { };
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
                        var firma = VentasIndexTicket.GeneraFirma(jugadas.StrFecha, jugadas.StrHora, jugadas.TicketNo, jugadas.Items);

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
                            TicketTemplateHelper.PrintTicket(TICKET, null, true);
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
                } else {
                    ViewModel.SetMensaje(mensaje: "El ticket introducido no existe o ya pasó el tiempo permitido para reimpresion", icono: "Error", background: "#DC3545", puedeMostrarse: true);
                }
            }

        }

    }


}
