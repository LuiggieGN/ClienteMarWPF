using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.UnitOfWork;

namespace MAR.DataAccess.ViewModels
{
    public class JuegaMasViewModel
    {
        public string TicNumero { get; set; }
        public string TicFecha { get; set; }
        public string Hora { get; set; }
        public int TicketID { get; set; }
        public int Sorteo { get; set; }
        public decimal TicCosto { get; set; }
        public decimal TicPago { get; set; }
        public string TicketMartlon { get; set; }
        public string TicketControl { get; set; }

        public bool TicNulo { get; set; }
        public string Pin { get; set; }
        public string SorteoNombre { get; set; }
        public JugadaJuegaMAS[] Jugadas { get; set; }
        public string Firma { get; set; }
        public string Estado { get; set; }
        public string NumeroSorteo { get; set; }
        public string FechaSorteo { get; set; }
        public string HoraSorteo { get; set; }
        public string Serial { get; set; }
        public static string Url { get; set; }
        public static class SuplidorMarlton
        {
            public static string Login { get; set; }
            public static string PassWord { get; set; }
            public static string RetailId { get; set; }
            public static string Url { get; set; }
        }

        //Class para la API de Billete Electronico en Marltom
        public class MarltonParameters : BaseParameter
        {
            public Detalle[] detalle { get; set; }
            public Header.BilleteHeader header { get; set; }
            public string Serial { get; set; }
            public string SerialTF { get; set; }
            public DateTime Fecha { get; set; }

            public class Detalle
            {
                public string Jugada { get; set; }
                public decimal Monto { get; set; }
                public string Serial { get; set; }
                public DateTime HoraRequest { get; set; }
                public DateTime HoraResponse { get; set; }
                public int NumeroSorteo { get; set; }
            }

            public static MarltonParameters Map_EnviarJuegaMas(VP_Transaccion bTicket)
            {

                var jugadas =
                    bTicket.VP_TransaccionDetalle.Where(
                        x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Jugada.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();
              
                var montos =
                    bTicket.VP_TransaccionDetalle.Where(
                        x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Monto.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

                JugadaJuegaMAS[] juegamas = new JugadaJuegaMAS[jugadas.Length];

                for (int i = 0; i < jugadas.Length; i++)
                {
                    juegamas[i] = (new JugadaJuegaMAS
                    {
                        Jugada = jugadas[i].ValorText,
                        Monto = double.Parse(montos[i].ValorText),
                    });
                }
                var config = DataAccess.EFRepositories.VpSuplidores.GetSuplidorProductoConfiguration("Marlton JuegaMas");
                return new MarltonParameters
                {
                    Login = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor,
                    Password = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor,
                    RetailId = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor,
                    TerminalId = bTicket.BancaID,
                    ServiceUrl = new Uri(config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("URL")).Valor),
                    detalle = (bTicket.VP_TransaccionDetalle == null ? null
                        : (from j in juegamas
                           select new Detalle
                           {
                               Jugada = j.Jugada,
                               Monto = decimal.Parse(j.Monto.ToString())
                           }).ToArray()
                    ),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor }, { "Password", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor }, { "TerminalId", bTicket.BancaID.ToString() }, { "RetailId", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor } }
                };
            }
           
            public static MarltonParameters Map_EnviarJuegaMasFree(VP_Transaccion bTicket)
            {
                var jugadas =
                    bTicket.VP_TransaccionDetalle.Where(
                        x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Jugada.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

                var montos =
                    bTicket.VP_TransaccionDetalle.Where(
                        x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Monto.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

                JugadaJuegaMAS[] juegamas = new JugadaJuegaMAS[jugadas.Length];

                for (int i = 0; i < jugadas.Length; i++)
                {
                    juegamas[i] = (new JugadaJuegaMAS
                    {
                        Jugada = jugadas[i].ValorText,
                        Monto = 25,
                    });
                }
                var config = DataAccess.EFRepositories.VpSuplidores.GetSuplidorProductoConfiguration("Marlton JuegaMas");
                return new MarltonParameters
                {
                    Login = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor,
                    Password = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor,
                    RetailId = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor,
                    TerminalId = bTicket.BancaID,
                    ServiceUrl = new Uri(config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("URL")).Valor),
                    SerialTF = bTicket.Referencia,//"571431517",
                    detalle = (bTicket.VP_TransaccionDetalle == null ? null
                        : (from j in juegamas
                           select new Detalle
                            {
                                Jugada = j.Jugada,
                                Monto = decimal.Parse(j.Monto.ToString())
                            }).ToArray()
                    ),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor }, { "Password", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor }, { "TerminalId", bTicket.ToString() }, { "RetailId", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor } }
                };
            }

            public static MarltonParameters Map_GetQuickPick(int pBancaId)
            {
                var config = DataAccess.EFRepositories.VpSuplidores.GetSuplidorProductoConfiguration("Marlton JuegaMas");
                return new MarltonParameters
                {
                    Login = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor,
                    Password = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor,
                    RetailId = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("URL")).Valor),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor }, { "Password", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor }, { "TerminalId", pBancaId.ToString() }, { "RetailId", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor } }
                };
            }

            //need session here
            public static MarltonParameters Map_QuickPick_Mar(int pBancaId, int pFracciones, int pCantidad, int pLoteriaId)
            {
                var jugadas = GenerateJugadasQuickPick(pFracciones, pCantidad, pLoteriaId);
                var config = DataAccess.EFRepositories.VpSuplidores.GetSuplidorProductoConfiguration("Marlton JuegaMas");
                return new MarltonParameters
                {
                    Login = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor,
                    Password = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor,
                    RetailId = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("URL")).Valor),
                    detalle = ((from j in jugadas
                                select new Detalle
                                {
                                    Jugada = j.Jugada,
                                    Monto = j.Monto,
                                    Serial = j.Serial,
                                    HoraRequest = j.HoraRequest,
                                    HoraResponse = j.HoraResponse
                                }).ToArray()
                        ),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor }, { "Password", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor }, { "TerminalId", pBancaId.ToString() }, { "RetailId", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor } }
                };
            }

            public static MarltonParameters Map_Cancelar_O_Validar_O_Pagar(string serial, int pBancaId)
            {
                var config = DataAccess.EFRepositories.VpSuplidores.GetSuplidorProductoConfiguration("Marlton JuegaMas");
                return new MarltonParameters
                {
                    Login = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor,
                    Password = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor,
                    RetailId = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("URL")).Valor),
                    Serial = serial,
                    HeardersDictionary = new Dictionary<string, string> { { "Login", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor }, { "Password", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor }, { "TerminalId", pBancaId.ToString() }, { "RetailId", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor } }
                };
            }

            public static MarltonParameters Map_GetGanadoresJuegaMas(DateTime pFecha, int pBancaId)
            {
                var config = DataAccess.EFRepositories.VpSuplidores.GetSuplidorProductoConfiguration("Marlton JuegaMas");
                return new MarltonParameters
                {
                    Login = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor,
                    Password = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor,
                    RetailId = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("URL")).Valor),
                    Fecha = pFecha,
                    HeardersDictionary = new Dictionary<string, string> { { "Login", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor }, { "Password", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor }, { "TerminalId", pBancaId.ToString() }, { "RetailId", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor } }
                };
            }

            public static MarltonParameters Map_RegistraBancaJuegaMas(int pBancaId)
            {
                var config = DataAccess.EFRepositories.VpSuplidores.GetSuplidorProductoConfiguration("Marlton JuegaMas");
                return new MarltonParameters
                {
                    Login = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor,
                    Password = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor,
                    RetailId = config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("URL")).Valor),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("LOGIN")).Valor }, { "Password", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("PASSWORD")).Valor }, { "TerminalId", pBancaId.ToString() }, { "RetailId", config.FirstOrDefault(x => x.Opcion.ToUpper().Contains("RETAILID")).Valor } }
                };
            }
            internal static ICollection<Detalle> GenerateJugadasQuickPick(int pFracciones, int pCantidad, int pLoteriaId)
            {
                Random rnd = new Random();
                ICollection<Detalle> ticketDetalles = new List<Detalle>();
                for (int i = 0; i < pCantidad; i++)
                {
                    var quickpick = rnd.Next(99999).ToString().PadLeft(5, '0') + "-" + "01" + "-" +
                                       pFracciones.ToString().PadLeft(2, '0');

                    Detalle detalle = new Detalle
                    {
                        Jugada = quickpick,
                        Monto = pFracciones * 20,
                        HoraRequest = DateTime.Now
                    };
                    ticketDetalles.Add(detalle);
                }
                return ticketDetalles;
            }
        }

        public class MarltonResponse : BaseResponse
        {
            public string Serial { get; set; }
            public string NewSerial { get; set; }
            public string TicketNo { get; set; }
            public decimal? Monto { get; set; }
            public string Fecha { get; set; }// no cambiar esta prop a DateTime, la api esta devolviendo un string
            public string Fecha_Sorteo { get; set; }
            public string Hora_Sorteo { get; set; }
            public bool Jugado_Free { get; set; }
            
            public decimal? monto_jugada_Free { get; set; }
            public Int64? Numero_Sorteo { get; set; }
            public Detalle[] detalle { get; set; }
            public string Serial_TicketPagadoFree { get; set; }
            public string Monto_TicketPagadoFree { get; set; }
            public string BilleteQP { get; set; }
            public string statusTicket { get; set; }
            public string Ticket_Control { get; set; }
            
            public bool Estadopago { get; set; }
            public bool TicketFree { get; set; }
            public int Num_Jug_Free { get; set; }
            public string fechaPagado { get; set; } // no cambiar esta prop a DateTime, la api esta devolviendo un string
            public double? monpag { get; set; }
            public string WinnerNumber { get; set; }

            public Status status { get; set; }
            public bool Sorteo_efectuado { get; set; }

            public class Status
            {
                public int? Code { get; set; }
                public string Message { get; set; }
                public string StatusMessage { get; set; }
            }
            public class Detalle
            {
                public int? Loteria { get; set; }
                public string Jugada { get; set; }
                public double? Monto { get; set; }
            }

            public static MarltonResponse Exec_CancelarJuegaMas(string pSerial, int pBancaId)
            {
                var parameters = MarltonParameters.Map_Cancelar_O_Validar_O_Pagar(pSerial, pBancaId);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.CancelTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
            }
            public static MarltonResponse Exec_PagarJuegaMas(string pSerial, int pBancaId)
            {
                var parameters = MarltonParameters.Map_Cancelar_O_Validar_O_Pagar(pSerial, pBancaId);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.PagoTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
            }

            public static MarltonResponse Exec_QuickPickMarlton(int pBancaId)
            {
                var parameters = MarltonParameters.Map_GetQuickPick(pBancaId);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.QuicPick, parameters, ProductosExternosEnums.HttpMethod.GET);
            }

            public static MarltonResponse Exec_ValidarJuegaMas(string pSerial, int pBancaId)
            {
                var parameters = MarltonParameters.Map_Cancelar_O_Validar_O_Pagar(pSerial, pBancaId);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.EstadoTicket, parameters, ProductosExternosEnums.HttpMethod.POST);
            }
            public static MarltonResponse EnviarJugada_JuegaMas(VP_Transaccion dTickets)
            {
                var parameters = MarltonParameters.Map_EnviarJuegaMas(dTickets);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.SellTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
            }

            public static MarltonResponse EnviarJugada_JuegaMasFree(VP_Transaccion dTickets)
            {
                var parameters = MarltonParameters.Map_EnviarJuegaMasFree(dTickets);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.SellTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
            }
            public static MarltonResponse Exec_GetGanadoresJuegaMas(DateTime pFecha, int pBancaId)
            {
                var parameters = MarltonParameters.Map_GetGanadoresJuegaMas(pFecha, pBancaId);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.WinningNumber, parameters, ProductosExternosEnums.HttpMethod.POST);
            }
            public static MarltonResponse Exec_RegistraBancaJuegaMas(int pBancaId)
            {
                var parameters = MarltonParameters.Map_RegistraBancaJuegaMas(pBancaId);
                return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.Ping, parameters, ProductosExternosEnums.HttpMethod.POST);
            }


        }

        private static Dictionary<int, string> GetCrosswalk()
        {
            return ("*0#Q*1#V*2#C*3#0*4#H*5#5*6#M*7#R*8#W*9#D*10#1*11#6*12#N*13#S*14#X*15#E*16#2*17#J*18#7*19#T*20#A*21#Y*22#F*23#3*24#K*25#8*26#P*27#U*28#B*29#L*30#G*31#4*32#9")
                       .Split('*').Where(x => x.Length > 1)
                       .Select(xx => new { key = Convert.ToInt32(xx.Split('#')[0]), value = xx.Split('#')[1] })
                       .ToDictionary(x => x.key, y => y.value);
        }

        public class JugadaJuegaMAS
        {
            public double Cantidad { get; set; }
            public string Jugada { get; set; }
            public double Monto { get; set; }
            public string Codigo { get; set; }
        }

        public static List<JuegaMasViewModel> MapFromTransacciones(IEnumerable<VP_Transaccion> pTransaccion, bool pWithPin = true, bool pConJugadas = true)
        {
            List<JuegaMasViewModel> juegaMasList = new List<JuegaMasViewModel>();

            foreach (var item in pTransaccion)
            {
                var jugadas =
                    item.VP_TransaccionDetalle.Where(
                        x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Jugada.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();
                JugadaJuegaMAS[] juegamas = new JugadaJuegaMAS[jugadas.Length];

                var cantidades =
                    item.VP_TransaccionDetalle.Where(
                        x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Cantidad.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();
                var montos =
                    item.VP_TransaccionDetalle.Where(
                        x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Monto.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

                if (pConJugadas)
                {
                    for (int i = 0; i < jugadas.Length; i++)
                    {
                        juegamas[i] = (new JugadaJuegaMAS
                        {
                            Jugada = jugadas[i].ValorText,
                            Cantidad = double.Parse(cantidades[i].ValorText),
                            Monto = double.Parse(montos[i].ValorText),
                        });
                    }
                }
                else
                {
                    juegamas = null;
                }
                juegaMasList.Add(new JuegaMasViewModel
                {
                    TicNumero = item.TransaccionID.ToString(),
                    TicketID = item.TransaccionID,
                    TicFecha = item.FechaIngreso.ToString("t"),
                    Hora = item.FechaIngreso.ToString("t"),
                    TicCosto = item.Ingresos,
                    TicNulo = !item.Activo,
                    Pin = (pWithPin && item.TransaccionID > 0 ? BaseViewModel.GeneraPinGanador(Convert.ToInt32(item.TransaccionID)) : string.Empty),
                    Firma = (pWithPin && item.TransaccionID > 0 ? GeneraFirma(item.FechaIngreso.ToShortDateString(), item.FechaIngreso.ToString("t"), item.TransaccionID.ToString(), item.VP_TransaccionDetalle) : string.Empty),
                    TicketMartlon = item.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString())?.ValorText,
                    HoraSorteo = item.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString())?.ValorText,
                    NumeroSorteo = item.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString())?.ValorText,
                    FechaSorteo = item.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString())?.ValorText,
                    Serial = item.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString())?.ValorText,
                    TicketControl = item.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString())?.ValorText,
                    Estado = item.Estado,
                    Jugadas = juegamas
                });
            }
            return juegaMasList;
        }

      
        public static JuegaMasViewModel MapFromTransaccion(VP_Transaccion pTransaccion, bool pWithPin)
        {

            var jugadas =
                pTransaccion.VP_TransaccionDetalle.Where(
                    x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Jugada.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

            var cantidades =
                pTransaccion.VP_TransaccionDetalle.Where(
                    x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Cantidad.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();
            var montos =
                pTransaccion.VP_TransaccionDetalle.Where(
                    x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Monto.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

           

            JugadaJuegaMAS[] juegamas = new JugadaJuegaMAS[jugadas.Length];

            for (int i = 0; i < jugadas.Length; i++)
            {
                juegamas[i] = (new JugadaJuegaMAS
                {
                    Jugada = jugadas[i].ValorText,
                    Cantidad = double.Parse(cantidades[i].ValorText),
                    Monto = double.Parse(montos[i].ValorText),
                });
            }
           

            return new JuegaMasViewModel
            {
                TicNumero = pTransaccion.TransaccionID.ToString(),
                TicketID = pTransaccion.TransaccionID,
                TicFecha = pTransaccion.FechaIngreso.ToString("t"),
                Hora = pTransaccion.FechaIngreso.ToString("t"),
                TicCosto = pTransaccion.Ingresos,
                TicNulo = !pTransaccion.Activo,
                Pin = (pWithPin && pTransaccion.TransaccionID > 0 ? BaseViewModel.GeneraPinGanador(Convert.ToInt32(pTransaccion.TransaccionID)) : string.Empty),
                Firma = (pWithPin && pTransaccion.TransaccionID > 0 ? GeneraFirma(pTransaccion.FechaIngreso.ToShortDateString(), pTransaccion.FechaIngreso.ToString("t"), pTransaccion.TransaccionID.ToString(), pTransaccion.VP_TransaccionDetalle) : string.Empty),
                TicketMartlon = pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString())?.ValorText,
                HoraSorteo = pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString())?.ValorText,
                NumeroSorteo = pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString())?.ValorText,
                FechaSorteo = pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString())?.ValorText,
                Serial = pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString())?.ValorText,
                TicketControl = pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString())?.ValorText,

                Estado = pTransaccion.Estado,
                Jugadas = juegamas

            };
        }

        public static JuegaMasViewModel MapFromHTransaccion(VP_HTransaccion pHTransaccion, bool pWithPin)
        {
            var jugadas =
                pHTransaccion.VP_HTransaccionDetalle.Where(
                    x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Jugada.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

            var cantidades =
                pHTransaccion.VP_HTransaccionDetalle.Where(
                    x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Cantidad.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();
            var montos =
                pHTransaccion.VP_HTransaccionDetalle.Where(
                    x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Monto.ToString()).OrderByDescending(x => x.TransaccionDetalleID).ToArray();

            JugadaJuegaMAS[] juegamas = new JugadaJuegaMAS[jugadas.Length];

            for (int i = 0; i < jugadas.Length; i++)
            {
                juegamas[i] = (new JugadaJuegaMAS
                {
                    Jugada = jugadas[i].ValorText,
                    Cantidad = double.Parse(cantidades[i].ValorText),
                    Monto = double.Parse(montos[i].ValorText),
                });
            }

            return new JuegaMasViewModel
            {
                TicNumero = pHTransaccion.TransaccionID.ToString(),
                TicketID = pHTransaccion.TransaccionID,
                TicFecha = pHTransaccion.FechaIngreso.ToString("t"),
                Hora = pHTransaccion.FechaIngreso.ToString("t"),
                TicCosto = pHTransaccion.Ingresos,
                TicNulo = !pHTransaccion.Activo,
                Pin = (pWithPin && pHTransaccion.TransaccionID > 0 ? BaseViewModel.GeneraPinGanador(Convert.ToInt32(pHTransaccion.TransaccionID)) : string.Empty),
                Firma = (pWithPin && pHTransaccion.TransaccionID > 0 ? GeneraFirma(pHTransaccion.FechaIngreso.ToShortDateString(), pHTransaccion.FechaIngreso.ToString("t"), pHTransaccion.TransaccionID.ToString(), new List<VP_TransaccionDetalle>()) : string.Empty),
                TicketMartlon = pHTransaccion.VP_HTransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString())?.ValorText,
                HoraSorteo = pHTransaccion.VP_HTransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString())?.ValorText,
                NumeroSorteo = pHTransaccion.VP_HTransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString())?.ValorText,
                FechaSorteo = pHTransaccion.VP_HTransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString())?.ValorText,
                Serial = pHTransaccion.VP_HTransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString())?.ValorText,
                TicketControl = pHTransaccion.VP_HTransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString())?.ValorText,

                Estado = pHTransaccion.Estado,
                Jugadas = juegamas

            };
        }



      
        private static string GeneraFirma(string pFecha, string pHora, string pTicket, ICollection<VP_TransaccionDetalle> pJugadas)
        {
            try
            {
                var rdn = (new Random(DateTime.Now.Millisecond)).Next(31) + 1;
                var Cadena = String.Format("{0}{1}{2}{3}{4}21", pFecha, pHora, pTicket, pJugadas, rdn);
                long acum1 = 0;
                for (var i = 1; i <= Cadena.Length; i++)
                {
                    acum1 += (i * (int)Cadena[i - 1]);
                }

                var Source = (acum1 % 100000).ToString().PadLeft(5, '0');

                acum1 = 0;
                foreach (var item in pJugadas)
                {
                    Cadena = String.Format("{0}{1}{2}3", pJugadas,
                                            rdn,
                                            Convert.ToInt32(1));
                    for (var i = 0; i < Cadena.Length; i++)
                    {
                        acum1 += (int)Cadena[i];
                    }

                }


                Source = String.Format("{0}-{1}", Source, (acum1 % 10000000).ToString().PadLeft(7, '1'));

                var theCrosswalk = GetCrosswalk();
                var theResult = string.Empty;
                for (var cnt = 1; cnt <= 13; cnt++)
                {
                    var iSrcNo = 0;
                    var iSrc = Source[cnt - 1].ToString();
                    if (cnt == 6)
                    {
                        iSrcNo = rdn;
                    }
                    else
                    {
                        iSrcNo = theCrosswalk.Where(x => x.Value.Equals(iSrc)).Select(x => x.Key).FirstOrDefault();
                        iSrcNo = (iSrcNo + rdn + cnt) % 33;
                    }
                    theResult += theCrosswalk[iSrcNo];
                }
                return theResult;
            }
            catch
            {
                return "- no disponible -";
            }
        }



    }
}