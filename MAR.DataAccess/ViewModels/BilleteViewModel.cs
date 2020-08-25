using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
using System.Linq;

using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.UnitOfWork;

namespace MAR.DataAccess.ViewModels
{
    public class BilleteViewModel
    {
        public string TicNumero { get; set; }
        public string TicFecha { get; set; }
        public string Hora { get; set; }
        public int TicketID { get; set; }
        public int Sorteo { get; set; }
        public decimal TicCosto { get; set; }
        public decimal TicPago { get; set; }
        public bool TicNulo { get; set; }
        public string Pin { get; set; }
        public string SorteoNombre { get; set; }
        public Jugada[] Jugadas { get; set; }
        public string Firma { get; set; }

        public static class SuplidorMarlton
        {
            public static string Login { get; set; }
            public static string PassWord { get; set; }
            public static string RetailId { get; set; }
            public static string Url { get; set; }
        }

        //Class para la API de Billete Electronico en Marltom
        public class MarltonParameters:BaseParameter
        {
            public Detalle[] detalle { get; set; }
            public Header.BilleteHeader header { get; set; }
            public string Serial { get; set; }

            public class Detalle
            {
                public int Loteria   { get; set; }
                public string Jugada { get; set; }
                public decimal Monto { get; set; }
                public string Serial { get; set; }
                public DateTime HoraRequest { get; set; }
                public DateTime HoraResponse { get; set; }
                public int NumeroSorteo { get; set; }
            }

            public static MarltonParameters Map_EnviarBillete(DTicket bTicket)
            {
                return new MarltonParameters
                {
                    Login = SuplidorMarlton.Login,
                    Password = SuplidorMarlton.PassWord,
                    RetailId = SuplidorMarlton.RetailId,
                    TerminalId = bTicket.BancaID,
                    ServiceUrl = new Uri(SuplidorMarlton.Url),
                    detalle = (bTicket.DTicketDetalles == null ? null
                        : (from j in bTicket.DTicketDetalles
                           select new Detalle
                           {
                               Loteria = bTicket.LoteriaID,
                               Jugada = j.TDeNumero,
                               Monto = j.TDePago,
                           }).ToArray()
                        ),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", SuplidorMarlton.Login }, { "Password", SuplidorMarlton.PassWord }, { "TerminalId", bTicket.BancaID.ToString() }, { "RetailId", SuplidorMarlton.RetailId } }
                };
            }

            public static MarltonParameters Map_GetQuickPick(int pBancaId)
            {
               
                return new MarltonParameters
                {
                    Login = SuplidorMarlton.Login,
                    Password = SuplidorMarlton.PassWord,
                    RetailId = SuplidorMarlton.RetailId,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(SuplidorMarlton.Url),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", SuplidorMarlton.Login }, { "Password", SuplidorMarlton.PassWord }, { "TerminalId", pBancaId.ToString()}, { "RetailId",SuplidorMarlton.RetailId } }
                };
            }

            //need session here
            public static MarltonParameters Map_QuickPick_Mar(int pBancaId, int pFracciones, int pCantidad)
            {
                var jugadas = GenerateJugadasQuickPick(pFracciones, pCantidad);

                return new MarltonParameters
                {
                    Login = SuplidorMarlton.Login,
                    Password = SuplidorMarlton.PassWord,
                    RetailId = SuplidorMarlton.RetailId,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(SuplidorMarlton.Url),
                    detalle = ((from j in jugadas
                           select new Detalle
                           {
                               Loteria = j.Loteria,
                               Jugada = j.Jugada,
                               Monto = j.Monto,
                               Serial = j.Serial,
                               HoraRequest = j.HoraRequest,
                               HoraResponse = j.HoraResponse
                           }).ToArray()
                        ),
                    HeardersDictionary = new Dictionary<string, string> { { "Login", SuplidorMarlton.Login}, { "Password", SuplidorMarlton.PassWord }, { "TerminalId", pBancaId.ToString() }, { "RetailId", SuplidorMarlton.RetailId } }
                };
            }


            internal static ICollection<Detalle> GenerateJugadasQuickPick(int pFracciones, int pCantidad)
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
                        Loteria = 13,
                        HoraRequest = DateTime.Now
                    };
                    ticketDetalles.Add(detalle);
                }
                return ticketDetalles;
            }

            public static MarltonParameters Map_Cancelar_O_Validar_O_Pagar(string serial, int pBancaId)
            {
                return new MarltonParameters
                {
                    Login = SuplidorMarlton.Login,
                    Password = SuplidorMarlton.PassWord,
                    RetailId = SuplidorMarlton.RetailId,
                    TerminalId = pBancaId,
                    ServiceUrl = new Uri(SuplidorMarlton.Url),
                    Serial = serial,
                    HeardersDictionary = new Dictionary<string, string> { { "Login", SuplidorMarlton.Login }, { "Password", SuplidorMarlton.PassWord}, { "TerminalId", pBancaId.ToString() }, { "RetailId", SuplidorMarlton.RetailId } }
                };
            }
        }

        public class MarltonResponse : BaseResponse
        {
            public string Serial { get; set; }
            public string NewSerial { get; set; }
            public string TicketNo { get; set; }
            public decimal? Monto { get; set; }
            public string Fecha { get; set; }// no cambiar esta prop a DateTime, la api esta devolviendo un string
            public DateTime? Fecha_Sorteo { get; set; }
            public DateTime? Hora_Sorteo { get; set; }
            public Int64? Numero_Sorteo { get; set; }
            public Detalle[] detalle { get; set; }
            public string BilleteQP { get; set; }
            public string statusTicket { get; set; }
            public bool Estadopago { get; set; }
            public string fechaPagado { get; set; } // no cambiar esta prop a DateTime, la api esta devolviendo un string
            public double? monpag { get; set; }

            public Status status { get; set; }

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

                public static MarltonResponse Exec_CancelarBillete(string pSerial, int pBancaId)
                {
                    var parameters = MarltonParameters.Map_Cancelar_O_Validar_O_Pagar(pSerial, pBancaId);
                    return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.CancelTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
                }
                public static MarltonResponse Exec_PagarBillete(string pSerial, int pBancaId)
                {
                    var parameters = MarltonParameters.Map_Cancelar_O_Validar_O_Pagar(pSerial, pBancaId);
                    return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.PagoTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
                }

                public static MarltonResponse Exec_QuickPickMarlton(int pBancaId)
                {
                    var parameters = MarltonParameters.Map_GetQuickPick(pBancaId);
                    return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.QuicPick, parameters, ProductosExternosEnums.HttpMethod.GET);
                }

                public static MarltonResponse Exec_ValidarBillete(string pSerial, int pBancaId)
                {
                    var parameters = MarltonParameters.Map_Cancelar_O_Validar_O_Pagar(pSerial, pBancaId);
                    return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.EstadoTicket, parameters, ProductosExternosEnums.HttpMethod.POST);
                }
                public static MarltonResponse EnviarJugada_Billete(DTicket dTicket)
                {
                    var parameters = MarltonParameters.Map_EnviarBillete(dTicket);
                    return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.SellTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
                }

                public static MarltonResponse Exec_QuickPickMar(DTicket dTicket)
                {
                    var parameters = MarltonParameters.Map_EnviarBillete(dTicket);
                    return GenericMethods.CallServicePostAction<MarltonResponse, MarltonParameters>(ProductosExternosEnums.ServiceMethod.SellTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
                }
        }
       
        private static Dictionary<int, string> GetCrosswalk()
        {
            return ("*0#Q*1#V*2#C*3#0*4#H*5#5*6#M*7#R*8#W*9#D*10#1*11#6*12#N*13#S*14#X*15#E*16#2*17#J*18#7*19#T*20#A*21#Y*22#F*23#3*24#K*25#8*26#P*27#U*28#B*29#L*30#G*31#4*32#9")
                       .Split('*').Where(x => x.Length > 1)
                       .Select(xx => new { key = Convert.ToInt32(xx.Split('#')[0]), value = xx.Split('#')[1] })
                       .ToDictionary(x => x.key, y => y.value);
        }

        public class Jugada
        {
            public double TDeCantidad { get; set; }
            public string TDeNumero { get; set; }
            public double Precio { get; set; }
            public string TDeQP { get; set; }
            public decimal Total { get; set; }
        }

        public static BilleteViewModel MapFromBet(DTicket bTicket, bool pWithPin = true)
        {
                return new BilleteViewModel
                {
                    TicNumero = bTicket.TicNumero,
                    TicketID = bTicket.TicketID,
                    TicFecha = bTicket.TicFecha.ToString("t"),
                    Hora = bTicket.TicFecha.ToString("t"),
                    TicCosto = bTicket.TicCosto,
                    TicPago = bTicket.TicPago,
                    TicNulo = bTicket.TicNulo,
                    Sorteo = bTicket.LoteriaID,
                    Pin = (pWithPin && bTicket.TicSolicitud > 0 ? BaseViewModel.GeneraPinGanador(Convert.ToInt32(bTicket.TicSolicitud)) : string.Empty),
                    Firma = (pWithPin && bTicket.TicSolicitud > 0 ? GeneraFirma(bTicket.TicFecha.ToShortDateString(), bTicket.TicFecha.ToString("t"), bTicket.TicNumero, bTicket.DTicketDetalles) : string.Empty),

                    Jugadas = (bTicket.DTicketDetalles == null ? null : (from j in bTicket.DTicketDetalles
                        select new Jugada
                        {
                            TDeCantidad = j.TDeCantidad,
                            TDeNumero = j.TDeNumero,
                            Precio = Math.Round(j.TDeCantidad > 0 ? double.Parse(j.TDeCosto.ToString()) / j.TDeCantidad : 0, 2),
                            TDeQP = j.TDeQP,
                            Total = j.TDeCosto
                        }).ToArray()
                        )
                };
        }

        public static DTicket Map_TicketFromDetalles(List<MarltonParameters.Detalle> pDetalles, int pBancaId, int pUsuarioId, int pRiferoId)
        {

             return new DTicket
             {
                   RiferoID = pRiferoId,
                   TicNulo = false,
                   BancaID = pBancaId,
                   UsuarioID = pUsuarioId,
                   LoteriaID = 13,
                   GrupoID = 1,
                   TicFecha = DateTime.Now,
                   TicCliente = "1",
                   TicCedula = "0",
                   TicPagado = false,
                   PagoID = 0,
                   TicCosto = pDetalles.Sum(x => x.Monto),
                   TicSolicitud = decimal.Parse(pDetalles.Select(x => x.Serial).FirstOrDefault()),

                   DTicketDetalles = (from j in pDetalles
                                    select new DTicketDetalle
                 {
                     TDeCantidad = 1,
                     TDeCosto = j.Monto,
                     TDeQP = "B",
                     TDeNumero = j.Jugada,
                     TDePagoTipo = "0",
                     TDePago = j.Monto,
                     DBilleteDetalles = (from p in pDetalles select new DBilleteDetalle
                     {
                         Serial = j.Serial,
                         HoraResponse = j.HoraResponse,
                         HoraRequest = j.HoraRequest,
                         NumeroSorteo = j.NumeroSorteo
                     }).Take(1).ToArray()
                 }).ToArray()

             };
        }
       
        private static string GeneraFirma(string pFecha, string pHora, string pTicket, ICollection<DTicketDetalle> pJugadas)
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
                                            Convert.ToInt32(Math.Ceiling(item.TDeCantidad)));
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

        //public static string GeneraPinGanador(int pSol)
        //{
        //    var sb = new System.Text.StringBuilder();
        //    int ConfirmK = 0;
        //    var rdn = new Random(pSol);
        //    for (var n = 1; n < 4; n++)
        //    {
        //        var iK = rdn.Next(9);
        //        sb.Append(iK.ToString());
        //        ConfirmK += iK;
        //    }

        //    int seed = 1;
        //    var sb2 = new System.Text.StringBuilder();
        //    for (var x = 0; x < pSol.ToString().Length; x++)
        //    {
        //        seed = seed + (ConfirmK + Convert.ToInt32(pSol.ToString()[x].ToString()));
        //        var iK = seed % 10;
        //        sb2.Append(iK.ToString());
        //    }
        //    return sb.ToString() + sb2.ToString().Substring(sb2.Length - 5);
        //}

        //public static bool ComparaPinGanador(int pSol, string pPin)
        //{
        //    var stringBuilder = new System.Text.StringBuilder();
        //    int ConfirmK = 0;
        //    var rdn = new Random(pSol);
        //    for (var n = 1; n < 4; n++)
        //    {
        //        var iK = rdn.Next(9);
        //        stringBuilder.Append(iK.ToString());
        //        ConfirmK += iK;
        //    }

        //    int seed = 1;
        //    var stringBuilder2 = new System.Text.StringBuilder();
        //    for (var x = 0; x < pSol.ToString().Length; x++)
        //    {
        //        seed = seed + (ConfirmK + Convert.ToInt32(pSol.ToString()[x].ToString()));
        //        var iK = seed % 10;
        //        stringBuilder2.Append(iK.ToString());
        //    }
        //    string keyGen = stringBuilder + stringBuilder2.ToString().Substring(stringBuilder2.Length - 5);
        //    return (pPin == keyGen);
        //}

    }
}