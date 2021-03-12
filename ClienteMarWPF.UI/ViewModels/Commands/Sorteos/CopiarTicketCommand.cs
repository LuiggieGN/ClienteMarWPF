using ClienteMarWPF.DataAccess;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.PinterConfig;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class CopiarTicketCommand : ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ValidarPagoTicketViewModel ViewModelValidar;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        private List<Jugada> listadoJugadas;
        private bool reimprimir = false;
        private string NOTicket = "";
        public CopiarTicketCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService, bool Reimprimir = false, ValidarPagoTicketViewModel viewModelValidar = null)
        {
            ViewModel = viewModel;
            ViewModelValidar = viewModelValidar;
            Autenticador = autenticador;
            SorteosService = sorteosService;
            reimprimir = Reimprimir;

            Action<object> comando = new Action<object>(CopiarTicket);
            base.SetAction(comando);
        }

        public SorteosView DataContext { get; private set; }

        private void CopiarTicket(object parametro)
        {
            MAR_Bet jugadas = null;
            
            //ObservableCollection<MAR_Bet> jugadas = new ObservableCollection<MAR_Bet>() { };
            List<LoteriaTicketPin> loteriatickpin = new List<LoteriaTicketPin>() { };
            var MoreOptions = Autenticador.CurrentAccount.MAR_Setting2.MoreOptions.ToList();//Configuraciones de printer
            bool ExistPrinterCOnfig = false;
            var NumeroTicket = "";

            var ticketReimprimir = ViewModelValidar.TicketNumero;
            var data = parametro as TicketCopiadoResponse;
            if (data != null || ticketReimprimir != null) { 
                if (ticketReimprimir != null) { NumeroTicket = ticketReimprimir; }
                if (data != null) { NumeroTicket = data.TicketNo; ticketReimprimir = null; }
                MAR_Bet Ticket = null;




            //MAR_BetItem JugadaTicket = new MAR_BetItem() { Cantidad = Jugada.Cantidad, Costo =}
            //                    }
            //{


            var ObteniendoJugadaPrecargada = ViewModel.ListadoTicketsPrecargados.Where(x => x.TicketNo == NumeroTicket);
            var posicion = 0;
            MAR_Bet NuevaJugada = null;
            bool resetearJugdas = false;
            foreach (var jugada in ObteniendoJugadaPrecargada)
            {
                
                if (jugada.Items == null)
                {
                    resetearJugdas = true;
                    posicion = ViewModel.ListadoTicketsPrecargados.IndexOf(jugada);
                    
                    List<MAR_BetItem> ListadoJugadasTicket = new List<MAR_BetItem>() { };
                    var jugadasPorTickets = SorteosService.ConsultarTicketSinPin(Autenticador.CurrentAccount.MAR_Setting2.Sesion, NumeroTicket);

                    foreach (var Jugada in jugadasPorTickets.Items) {
                            MAR_BetItem JugadaTicket = new MAR_BetItem() { Cantidad = Jugada.Cantidad, Costo = Jugada.Costo, Loteria = Jugada.Loteria, Numero = Jugada.Numero, Pago = Jugada.Pago, QP = Jugada.QP };
                            ListadoJugadasTicket.Add(JugadaTicket);
                    }
                    NuevaJugada = new MAR_Bet() { TicketNo = jugada.TicketNo, Cedula = jugada.Cedula, Cliente = jugada.Cliente, Costo = jugada.Costo, Err = jugada.Err, Grupo = jugada.Grupo, Items = ListadoJugadasTicket.ToArray(), Loteria = jugada.Loteria, Nulo = jugada.Nulo, Pago = jugada.Pago, Solicitud = jugadasPorTickets.Solicitud, StrFecha = jugada.StrFecha, StrHora = jugada.StrHora, Ticket = jugada.Ticket };
                    
                    jugadas = new MAR_Bet() { Cedula = jugada.Cedula, Err = jugada.Err, Costo = jugada.Costo, Cliente = jugada.Cliente, Grupo = jugada.Grupo, Items = NuevaJugada.Items, Loteria = jugada.Loteria, Nulo = jugada.Nulo, Pago = jugada.Pago, Solicitud = NuevaJugada.Solicitud, StrFecha = jugada.StrFecha, StrHora = jugada.StrHora, Ticket = jugada.Ticket, TicketNo = jugada.TicketNo };
                 }
                    else { 
                    jugadas = new MAR_Bet() { Cedula = jugada.Cedula, Err = jugada.Err, Costo = jugada.Costo, Cliente = jugada.Cliente, Grupo = jugada.Grupo, Items = jugada.Items, Loteria = jugada.Loteria, Nulo = jugada.Nulo, Pago = jugada.Pago, Solicitud = jugada.Solicitud, StrFecha = jugada.StrFecha, StrHora = jugada.StrHora, Ticket = jugada.Ticket, TicketNo = jugada.TicketNo };
                }
                    
            }

                if (resetearJugdas == true) { 
                    ViewModel.ListadoTicketsPrecargados.RemoveAt(posicion);
                    ViewModel.ListadoTicketsPrecargados.Add(NuevaJugada);
                }
                //jugadas = SorteosService.ConsultarTicketSinPin(Autenticador.CurrentAccount.MAR_Setting2.Sesion, data.TicketNo);
                //var TicketSeleccionado = ViewModel.ListadoTicketsPrecargados.Where(x => x.TicketNo == data.TicketNo);
                List<Jugada> listJugadas = new List<Jugada> { };
                //jugadas = (MAR_Bet)TicketSeleccionado[0];

                //////////////////// Para la reimpresion de ticket ////////////////
                List<TicketJugadas> listTicketJugdas = new List<TicketJugadas> { };
                List<JugadasTicketModels> jugadasNuevoSinPrinter = new List<JugadasTicketModels>() { };

                var firma = VentasIndexTicket.GeneraFirma(jugadas.StrFecha, jugadas.StrHora, jugadas.TicketNo, jugadas.Items);
                var datosTicket = SessionGlobals.LoteriasTodas.Where(x => x.Numero == jugadas.Loteria).ToList();
                var NombreLoteria = datosTicket[0].Nombre;
                var Pin = VentasIndexTicket.GeneraPinGanador(Convert.ToInt32(jugadas.Solicitud));

                LoteriaTicketPin ticketPin = new LoteriaTicketPin() { Loteria = NombreLoteria, Pin = Pin, Ticket = jugadas.TicketNo };
                loteriatickpin.Add(ticketPin);
                ////////////////////////////////////////////////////////////////////////
                listJugadas = new List<Jugada>() { };
                foreach (var jugada in jugadas.Items)
                {

                    Jugada jugadaConPrinter = new Jugada { Jugadas = jugada.Numero, Monto = (int)(jugada.Costo), TipoJugada = jugada.QP };


                    //Para Reimprimir ticket con configuraciones de printer

                    JugadaPrinter jugadaPrinter = new JugadaPrinter() { Numeros = jugada.Numero, Monto = (int)(jugada.Costo) };
                    TicketJugadas ticketJugadas = new TicketJugadas() { Jugada = jugadaPrinter, TipoJudaga = jugada.QP };
                    listTicketJugdas.Add(ticketJugadas);
                    ///////////////////////////////////////

                    listJugadas.Add(jugadaConPrinter);

                    //Para Reimprimir ticket sin configuraciones de printer
                    JugadasTicketModels jugadaSinPrinter = new JugadasTicketModels() { Costo = Convert.ToInt32(jugada.Costo), Numero = jugada.Numero, TipoJugada = jugada.QP };
                    jugadasNuevoSinPrinter.Add(jugadaSinPrinter);

                }

                if (reimprimir == true)
                {
                    List<ConfigPrinterModel> listaConfiguraciones = new List<ConfigPrinterModel>() { };
                    for (var i = 0; i < MoreOptions.Count; i++)
                    {
                        var ConfigValue = MoreOptions[i];
                        var ArrayValue = ConfigValue.Split("|");
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
                    ///Configuraciones printer
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
                }
                else
                {
                    ViewModel.ListadoJugada = listJugadas;
                    Thread.Sleep(700);
                    ViewModelValidar.CerrarValidarPagoTicketCommand.Execute(null);
                    
                }
            }

            //jugadas = TicketSeleccionado[0].;

            else
            {
                var TicketSeleccionado = ViewModel.TicketSeleccionado;

                // jugadas = SorteosService.ConsultarTicketSinPin(Autenticador.CurrentAccount.MAR_Setting2.Sesion,ViewModelValidar.TicketNumero); 
            }

        }

    }
}
