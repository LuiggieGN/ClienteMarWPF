using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClienteMarWPF.UI.State.PinterConfig
{
    class ModelsPrinJobs
    {
    }

    public class ReportesIndexGanadores
    {
        public int Loteria { get; set; }
        public DateTime Fecha { get; set; }
        public string Primero { get; set; }
        public string Segundo { get; set; }
        public string Tercero { get; set; }
        public string Sorteo { get; set; }

    }
    public class Ganadores
    {
        public string Loteria { get; set; }
        public string Premio { get; set; }
        public string Ticket { get; set; }
        public decimal? Saco { get; set; }
        public bool TicPagado { get; set; }
    }
    public class RecargasIndexRecarga
    {
        public int UsuarioId { get; set; }
        public int SuplidorId { get; set; }
        public string Suplidor { get; set; }
        public int Solicitud { get; set; }
        public double Monto { get; set; }
        public string Numero { get; set; }
        public string Clave { get; set; }
        public string Serie { get; set; }
    }
    public class VentasIndexTicket
    {
        public string TicketNo { get; set; }
        public string Fecha { get; set; }
        public string Hora { get; set; }
        public int Ticket { get; set; }
        public int Sorteo { get; set; }
        public double Costo { get; set; }
        public double Pago { get; set; }
        public bool Nulo { get; set; }
        public string Pin { get; set; }
        public string SorteoNombre { get; set; }
        public Jugada[] Jugadas { get; set; }

        private static Dictionary<int, string> GetCrosswalk()
        {
            return ("*0#Q*1#V*2#C*3#0*4#H*5#5*6#M*7#R*8#W*9#D*10#1*11#6*12#N*13#S*14#X*15#E*16#2*17#J*18#7*19#T*20#A*21#Y*22#F*23#3*24#K*25#8*26#P*27#U*28#B*29#L*30#G*31#4*32#9")
                       .Split('*').Where(x => x.Length > 1)
                       .Select(xx => new { key = Convert.ToInt32(xx.Split('#')[0]), value = xx.Split('#')[1] })
                       .ToDictionary(x => x.key, y => y.value);
        }

        public class Jugada
        {
            public double Cantidad { get; set; }
            public string Numero { get; set; }
            public double Precio { get; set; }
            public string Tipo { get; set; }
            public double Total { get; set; }
        }

        internal static VentasIndexTicket FromBet(MAR_Bet pBet, bool pWithPin = true, MAR_Loteria2[] pSorteos = null)
        {
            return new VentasIndexTicket
            {
                TicketNo = pBet.TicketNo,
                Ticket = pBet.Ticket,
                Fecha = pBet.StrFecha,
                Hora = pBet.StrHora,
                Costo = pBet.Costo,
                Pago = pBet.Pago,
                Nulo = pBet.Nulo,
                Sorteo = pBet.Loteria,
                Pin = (pWithPin && pBet.Solicitud > 0 ? GeneraPinGanador(Convert.ToInt32(pBet.Solicitud)) : string.Empty),
                Firma = (pWithPin && pBet.Solicitud > 0 ? GeneraFirma(pBet.StrFecha, pBet.StrHora, pBet.TicketNo, pBet.Items) : string.Empty),
                SorteoNombre = (pSorteos != null ? (from s in pSorteos
                                                    where s.Numero == pBet.Loteria
                                                    select s.Nombre).FirstOrDefault() : string.Empty),
                Jugadas = (pBet.Items == null ? null : (from j in pBet.Items
                                                        select new Jugada
                                                        {
                                                            Cantidad = j.Cantidad,
                                                            Numero = j.Numero,
                                                            Precio = Math.Round(j.Cantidad > 0 ? j.Costo / j.Cantidad : 0, 2),
                                                            Tipo = j.QP,
                                                            Total = j.Costo
                                                        }).ToArray()
                           )
            };
        }

        private static string GeneraFirma(string pFecha, string pHora, string pTicket, MAR_BetItem[] pJugadas)
        {
            try
            {
                var rdn = (new Random(DateTime.Now.Millisecond)).Next(31) + 1;
                var Cadena = String.Format("{0}{1}{2}{3}{4}21", pFecha, pHora, pTicket, "Banca", rdn);
                long acum1 = 0;
                for (var i = 1; i <= Cadena.Length; i++)
                {
                    acum1 += (i * (int)Cadena[i - 1]);
                }

                var Source = (acum1 % 100000).ToString().PadLeft(5, '0');

                acum1 = 0;
                for (var j = 1; j <= pJugadas.Length; j++)
                {
                    Cadena = String.Format("{0}{1}{2}3", pJugadas[j - 1].Numero,
                                            rdn,
                                            Convert.ToInt32(Math.Ceiling(pJugadas[j - 1].Cantidad)));
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

        private static string GeneraPinGanador(int pSol)
        {

            var sb = new System.Text.StringBuilder();
            int ConfirmK = 0;
            var rdn = new Random(pSol);
            for (var n = 1; n < 4; n++)
            {
                var iK = rdn.Next(9);
                sb.Append(iK.ToString());
                ConfirmK += iK;
            }

            int seed = 1;
            var sb2 = new System.Text.StringBuilder();
            for (var x = 0; x < pSol.ToString().Length; x++)
            {
                seed = seed + (ConfirmK + Convert.ToInt32(pSol.ToString()[x].ToString()));
                var iK = seed % 10;
                sb2.Append(iK.ToString());
            }
            return sb.ToString() + sb2.ToString().Substring(sb2.Length - 5);

        }

        public string Firma { get; set; }
    }
}
