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

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
{
    public class RealizarApuestaCommand : ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        List<LoteriaTicketPin> loteriatickpin = new List<LoteriaTicketPin>() { };
        private int contadorTIcket = 0;
        public List<SorteosObservable> SorteosBinding;
        public SorteosView sorteo;
        public static List<string> loteriasNoDisponiblesParaApuesta = new List<string>();
        //public List<int> ticketsJugados = new List<int>();
        public static string loteriasNoDisponiblesParaMostrar { get; set; }
        public List<double> almacenandoMontos = new List<double>();
        //public List<Tuple<double, int>> precioYdiaQ = new List<Tuple<double, int>>();
        public List<Tuple<double, int>> precioYdia = new List<Tuple<double, int>>();
        //public List<Tuple<double, int>> precioYdiaP = new List<Tuple<double, int>>();
        //public List<Tuple<double, int>> precioYdiaT = new List<Tuple<double, int>>();

        public RealizarApuestaCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;
            sorteo = new SorteosView();
            Action<object> comando = new Action<object>(RealizarApuestas);
            base.SetAction(comando);
        }

        public double GetPrecioPorDia(string tipoJugada, int loteria)
        {
            var collecion = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria);
            var diaActual = DateTime.Now.DayOfWeek.ToString();

            switch (diaActual)
            {
                case "Monday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieLun)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieLun)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieLun)).ToList(); }
                    break;

                case "Tuesday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieMar)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieMar)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieMar)).ToList(); }
                    break;

                case "Wednesday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieMie)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieMie)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieMie)).ToList(); }
                    break;

                case "Thursday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieJue)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieJue)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieJue)).ToList(); }
                    break;

                case "Friday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieVie)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieVie)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieVie)).ToList(); }
                    break;

                case "Saturday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieSab)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieSab)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieSab)).ToList(); }
                    break;

                case "Sunday":
                    if (tipoJugada == "  Quiniela" || tipoJugada == "Quiniela") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieDom)).ToList(); }
                    else if (tipoJugada == "  Pale" || tipoJugada == "Pale") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieDom)).ToList(); }
                    else if (tipoJugada == "  Tripleta" || tipoJugada == "Tripleta") { precioYdia = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieDom)).ToList(); }
                    break;

            }

            return precioYdia.ToArray()[0].Item1;
        }

        //// Metodo que obtiene precios de pale
        //#region Pale
        //public void ObtenerPrecioPales(int loteria)
        //{
        //    var numeroLoteria = 0;

            
        //    if (ViewModel.loteriasMultiples.Count > 0)
        //    {
        //        //foreach (var loteria in ViewModel.loteriasMultiples)
        //        //{
        //        //    numeroLoteria = loteria;
        //        //}

        //        var collecion = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria);

        //        if (collecion.ToArray()[0].Numero == loteria)
        //        {
        //            var diaActual = DateTime.Now.DayOfWeek.ToString();

        //            switch (diaActual)
        //            {
        //                case "Monday":
        //                    precioYdiaP = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieLun)).ToList();
        //                    break;

        //                case "Tuesday":
        //                    precioYdiaP = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieMar)).ToList();
        //                    break;

        //                case "Wednesday":
        //                    precioYdiaP = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieMie)).ToList();
        //                    break;

        //                case "Thursday":
        //                    precioYdiaP = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieJue)).ToList();
        //                    break;

        //                case "Friday":
        //                    precioYdiaP = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieVie)).ToList();
        //                    break;

        //                case "Saturday":
        //                    precioYdiaP = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieSab)).ToList();
        //                    break;

        //                case "Sunday":
        //                    precioYdiaP = collecion.Select(x => new Tuple<double, int>(x.PrecioP, x.CieDom)).ToList();
        //                    break;

        //            }

        //        }

        //    }
            
        //}
        //#endregion

        //// Metodo que obtiene precios de quiniela
        //#region Quiniela
        //public void ObtenerPrecioQuinielas(int loteria)
        //{
        //    var numeroLoteria = 0;

        //    if (ViewModel.loteriasMultiples.Count > 0)
        //    {
        //        //foreach (var loteria in ViewModel.loteriasMultiples)
        //        //{
        //        //    numeroLoteria = loteria;
        //        //}

        //        var collecion = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria);

        //        if (collecion.ToArray()[0].Numero == loteria)
        //        {
        //            var diaActual = DateTime.Now.DayOfWeek.ToString();

        //            switch (diaActual)
        //            {
        //                case "Monday":
        //                    precioYdiaQ = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieLun)).ToList();
        //                    break;

        //                case "Tuesday":
        //                    precioYdiaQ = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieMar)).ToList();
        //                    break;

        //                case "Wednesday":
        //                    precioYdiaQ = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieMie)).ToList();
        //                    break;

        //                case "Thursday":
        //                    precioYdiaQ = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieJue)).ToList();
        //                    break;

        //                case "Friday":
        //                    precioYdiaQ = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieVie)).ToList();
        //                    break;

        //                case "Saturday":
        //                    precioYdiaQ = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieSab)).ToList();
        //                    break;

        //                case "Sunday":
        //                    precioYdiaQ = collecion.Select(x => new Tuple<double, int>(x.PrecioQ, x.CieDom)).ToList();
        //                    break;

        //            }

        //        }

        //    }
            
        //}
        //#endregion

        //// Metodo que obtiene precios de tripletas
        //#region Tripleta
        //public void ObtenerPrecioTripleta(int loteria)
        //{
        //    var numeroLoteria = 0;
        //    if (ViewModel.loteriasMultiples.Count > 0)
        //    {
        //        //foreach (var loteria in ViewModel.loteriasMultiples)
        //        //{
        //        //    numeroLoteria = loteria;
        //        //}

        //        var collecion = SessionGlobals.LoteriasYSupersDisponibles.Where(x => x.Numero == loteria);

        //        if (collecion.ToArray()[0].Numero == loteria)
        //        {
        //            var diaActual = DateTime.Now.DayOfWeek.ToString();

        //            switch (diaActual)
        //            {
        //                case "Monday":
        //                    precioYdiaT = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieLun)).ToList();
        //                    break;

        //                case "Tuesday":
        //                    precioYdiaT = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieMar)).ToList();
        //                    break;

        //                case "Wednesday":
        //                    precioYdiaT = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieMie)).ToList();
        //                    break;

        //                case "Thursday":
        //                    precioYdiaT = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieJue)).ToList();
        //                    break;

        //                case "Friday":
        //                    precioYdiaT = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieVie)).ToList();
        //                    break;

        //                case "Saturday":
        //                    precioYdiaT = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieSab)).ToList();
        //                    break;

        //                case "Sunday":
        //                    precioYdiaT = collecion.Select(x => new Tuple<double, int>(x.PrecioT, x.CieDom)).ToList();
        //                    break;

        //            }

        //        }

        //    }
            
        //}
        //#endregion

        private void RealizarApuestas(object parametro)
        {
            var data = parametro as ApuestaResponse;

            if (ViewModel.loteriasMultiples.Count == 1)
            {
                OnlyBet(data);
            }
            else if (ViewModel.loteriasMultiples.Count > 1)
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
                    Numero = item.Jugadas.Replace("-", ""),
                    Costo = item.Monto * GetPrecioPorDia(item.TipoJugada,apuesta.LoteriaID),
                    Cantidad = item.Monto * GetPrecioPorDia(item.TipoJugada, apuesta.LoteriaID),
                    QP = item.TipoJugada.TrimStart().Substring(0, 1)

                });

                bet.Costo += item.Monto * GetPrecioPorDia(item.TipoJugada, apuesta.LoteriaID);

            }

            bet.Items = itemBet.ToArray();
            bet.Solicitud = SessionGlobals.SolicitudID;

            bet.Loteria = apuesta.LoteriaID;

            var MarBetResponse = SorteosService.RealizarApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, bet, SessionGlobals.SolicitudID, false);
            if (MarBetResponse.Err == null)
            {
                var ticket = new ArrayOfInt() { MarBetResponse.Ticket };
                SorteosView sorteo = new SorteosView();

                try
                {
                    //Window d = Application.Current.Windows.OfType<Window>().Where(w => w.Name.Equals("vistaSorteo")).FirstOrDefault();
                    ////////////////////////////////////////////////////////////////////////////////////////////////////
                    SorteosService.ConfirmarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ticket);
                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta("Jugada realizada satisfactoriamente.", "Excelente");
                    //MessageBox.Show("Jugada realizada satisfactoriamente", "Confirmacion");
                    /////////////////////////////////////////////////////////////////////////////////////////////////////
                    try
                    {
                        ImprimirTickets(MarBetResponse, null);
                    }
                    catch (Exception e)
                    {
                        (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(e.Message, "Aviso");
                    }

                }
                catch (Exception e)
                {
                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(e.Message, "Aviso");

                }
                //TemplateTicketHelper.
                //SorteosService.ConfirmarApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion);
            }
            else
            {
                MessageBox.Show(MarBetResponse.Err, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        public void ImprimirTickets(MAR_Bet MarBetResponse, MAR_MultiBet multi)
        {
            if (MarBetResponse != null)
            {
                List<JugadasTicketModels> jugadasNuevoSinPrinter = new List<JugadasTicketModels>() { };
                List<TicketJugadas> jugadasTicket = new List<TicketJugadas>() { };

                List<MAR_BetItem> JugadasForTicketPrecargado = new List<MAR_BetItem>() { };

                TicketDTO MarBetResponseCopy = new TicketDTO();
                MarBetResponseCopy.TicketNo = MarBetResponse.TicketNo;
                MarBetResponseCopy.Ticket = MarBetResponse.Ticket;
                MarBetResponseCopy.Nulo = MarBetResponse.Nulo;
                MarBetResponseCopy.Costo = MarBetResponse.Costo;
                MarBetResponseCopy.Pago = MarBetResponse.Pago;
                MarBetResponseCopy.Loteria = MarBetResponse.Loteria;
                MarBetResponseCopy.Err = MarBetResponse.Err;
                
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
                    MAR_BetItem jugadasTicketForPrecargados = new MAR_BetItem() { Numero = jugada.Numero, Costo = jugada.Costo, Cantidad = jugada.Cantidad, Loteria = jugada.Loteria, Pago = jugada.Pago, QP = jugada.QP };
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
                if (contadorTIcket <= CantidadLoterias)
                {
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
                    Loteria = NombreLoteria,
                    LoteriaID = MarBetResponse.Loteria,
                    Jugadas = jugadaTransform,
                    Pago = Convert.ToInt32(MarBetResponse.Pago),
                    Firma = firma,
                    Pin = loteriatickpin,
                    BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre,
                    BanDireccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion,
                    Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono,
                    TextReviseJugada = "Revise su jugada. Buena Suerte!"
                };

                TicketValue ticketr = new TicketValue() { BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre, Direccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion, FechaActual = MarBetResponse.StrFecha, Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono, Jugadas = jugadasTicket, LoteriaTicketPin = loteriatickpin, Firma = firma, Texto = "Revise su jugada. Buena Suerte!", Total = "Total", AutorizacionHacienda = null, Logo = null };

                //Agregando ticket a listado ticket precargado
               TicketDTO ticketForTicketRecargados = new TicketDTO() { Items = MarBetResponseCopy.Items, Pago = MarBetResponseCopy.Pago, Loteria = MarBetResponseCopy.Loteria, Costo = MarBetResponse.Costo, Nulo = MarBetResponseCopy.Nulo, Err = MarBetResponse.Err, Ticket = MarBetResponse.Ticket, TicketNo = MarBetResponse.TicketNo,Solicitud=MarBetResponse.Solicitud,Fecha=Convert.ToDateTime(MarBetResponse.StrFecha) };
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
                    if (contadorTIcket == CantidadLoterias)
                    {
                        TicketTemplateHelper.PrintTicket(TICKET, null, false, CantidadLoterias);
                        loteriatickpin = new List<LoteriaTicketPin>();
                        contadorTIcket = 0;
                    }
                }
                if (ExistPrinterCOnfig == false)
                {
                    
                     if (contadorTIcket == CantidadLoterias)
                    {
                        var TemplateTicketTextOnly = CreateTemplateTextOnlyTicket(TICKET, false);//Metodo que crea el tamplate Text Only

                        TicketTemplateHelper.PrintTicket(TemplateTicketTextOnly, null, false, CantidadLoterias);
                        loteriatickpin = new List<LoteriaTicketPin>();
                        contadorTIcket = 0;
                    }
                }

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
                TicketDTO multiCopy = new TicketDTO();
                multiCopy.Items = new JugadasDTO[multi.Items.Length];
                for (var i = 0; i < multi.Items.Length; i++)
                {
                    multiCopy.Items[i]= new JugadasDTO {Loteria=multi.Items[i].Loteria,Cantidad=multi.Items[i].Cantidad,Costo=multi.Items[i].Costo,Numero=multi.Items[i].Numero,Pago=multi.Items[i].Pago,QP=multi.Items[i].QP };
                    
                }

                TicketValue ticketr = new TicketValue() { BanNombre = Autenticador.BancaConfiguracion.BancaDto.BanNombre, Direccion = Autenticador.BancaConfiguracion.BancaDto.BanDireccion, FechaActual = Headers.StrFecha, Telefono = Autenticador.BancaConfiguracion.BancaDto.BanTelefono, Jugadas = jugadasTicket, LoteriaTicketPin = loteriatickpin, Firma = firma, Texto = "Revise su jugada. Buena Suerte!", Total = "Total", AutorizacionHacienda = null, Logo = null };

                TicketDTO ticketForTicketRecargados = new TicketDTO() { Items = multiCopy.Items, Pago = Headers.Pago, Loteria = Headers.Loteria, Costo = Headers.Costo, Nulo = Headers.Nulo, Err = null, Ticket = Headers.Ticket, TicketNo = Headers.TicketNo };
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
                        TicketTemplateHelper.PrintTicket(ticketr, listaConfiguraciones, false, CantidadLoterias);
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
                        var TemplateTicketTextOnly = CreateTemplateTextOnlyTicket(TICKET, true);
                        TicketTemplateHelper.PrintTicket(TemplateTicketTextOnly, null, false, CantidadLoterias);
                        loteriatickpin = new List<LoteriaTicketPin>();
                        contadorTIcket = 0;
                    }
                }
            }
        }

        private List<string[,]> CreateTemplateTextOnlyTicket(SorteosTicketModels TICKET,bool multiples)
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
                    TicketPinArray.SetValue(loteriatickpin[i].Ticket , 1);
                    TicketPinArray.SetValue(loteriatickpin[i].Pin, 2);
                    ListTicketPin.Add(TicketPinArray);
                    TicketPinArray = new string[3];
                }
                ReportesGeneralesJugadas JugadasTicketSinConfig = new ReportesGeneralesJugadas() { Fecha = Convert.ToDateTime(TICKET.Fecha), Mensaje = TICKET.TextReviseJugada, Firma = TICKET.Firma, onlyLoteria = TICKET.Loteria, onlyPin = TICKET.Pin[0].Pin.ToString(), onlyTicket = TICKET.TicketNo, Jugadas = Jugadas, Multiples = multiples, Reimprimir = false, TicketAndPin = ListTicketPin, Total = "Total:  " + Total.ToString("C2"), Hora = Convert.ToDateTime(TICKET.Hora) };
                ImprimirTicket = PrintJobs.PrintGeneralJugadas(JugadasTicketSinConfig, Autenticador);
                return ImprimirTicket;
            }catch (Exception e) { (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(e.Message, "ERROR","FF0000"); }

            return ImprimirTicket;

        }

        private void MultiBet(ApuestaResponse apuestas)
        {


            var loteriasApostando = SessionGlobals.LoteriasTodas.Where(x => x.Numero == apuestas.LoteriaID);
            var nombreLoteria = loteriasApostando.ToArray()[0].Nombre;

            var multi = new MAR_MultiBet();
            var itemBet = new List<MAR_BetItem>();

            foreach (var item in apuestas.Jugadas)
            {

                itemBet.Add(new MAR_BetItem
                {
                    Loteria = apuestas.LoteriaID,
                    Numero = item.Jugadas.Replace("-", ""),
                    Costo = item.Monto * GetPrecioPorDia(item.TipoJugada,apuestas.LoteriaID),
                    QP = item.TipoJugada.TrimStart().Substring(0, 1),
                    Cantidad = item.Monto * GetPrecioPorDia(item.TipoJugada, apuestas.LoteriaID)

                });

                multi.Items = itemBet.ToArray();
                multi.Headers = new MAR_BetHeader[] {
                    new MAR_BetHeader {
                    Solicitud = SessionGlobals.SolicitudID,
                    Loteria = apuestas.LoteriaID,
                    Costo = apuestas.Jugadas.Sum(x => x.Monto * GetPrecioPorDia(item.TipoJugada,apuestas.LoteriaID)),
                    StrFecha = DateTime.Now.ToString("dd/MM/yyyy"),
                    StrHora=DateTime.Now.ToString("hh:mm:ss")
                }

                    };


            }

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
                //try
                //{

                if (ticket.ToArray()[0] != 0)
                {

                    SorteosService.ConfirmarMultiApuesta(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ticket);
                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta("Jugada realizada satisfactoriamente.", "Excelente");
                    ImprimirTickets(null, multi);

                    if (ViewModel.loteriasMultiples.ToArray()[ViewModel.loteriasMultiples.Count - 1] == apuestas.LoteriaID && loteriasNoDisponiblesParaApuesta.Count() > 0)
                    {
                        MessageBox.Show($"La(s) siguiente(s) loteria(s) no se pudieron incluir en la apuesta: \n { loteriasNoDisponiblesParaMostrar } \n --------------------------------------------------------------- \nProbablemente algunas jugadas o sorteos no estaban disponibles", "Loteria(s) NO disponibles");
                        loteriasNoDisponiblesParaApuesta.Clear();
                        loteriasNoDisponiblesParaMostrar = string.Empty;
                    }
                }
                else
                {
                    loteriasNoDisponiblesParaApuesta.Add(nombreLoteria);
                    loteriasNoDisponiblesParaMostrar += "\t* " + nombreLoteria.ToUpper() + "\n";

                    if (ViewModel.loteriasMultiples.ToArray()[ViewModel.loteriasMultiples.Count - 1] == apuestas.LoteriaID && loteriasNoDisponiblesParaApuesta.Count() > 0)
                    {
                        MessageBox.Show($"La(s) siguiente(s) loteria(s) no se pudieron incluir en la apuesta: \n { loteriasNoDisponiblesParaMostrar } \n --------------------------------------------------------------- \nProbablemente algunas jugadas no estaban disponibles", "Loteria(s) NO disponibles");
                        loteriasNoDisponiblesParaApuesta.Clear();
                        loteriasNoDisponiblesParaMostrar = string.Empty;

                    }


                }

                //}
                //catch (Exception e)
                //{
                //    Console.WriteLine(e.Message);
                //}
            }
            else
            {
                MessageBox.Show(MultiBetResponse.Err, "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }


        }

    }
}
