using MAR.DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.Tables.DTOs;
using System.Globalization;

namespace MAR.BusinessLogic.Code.PrintJobs
{
    class JuegaMasPrintJob
    {
        internal static List<string[]> ImprimirVentaJuegaMas(int w, JuegaMasViewModel.MarltonResponse pResponse, JuegaMasViewModel viewModel, string pBanca, string pDireccion)
        {
            var j = new List<string[]>();
            string printString = "";

      

            j.Add(new string[] { Center("JuegaMas PegaMas", w), "2" });
            j.Add(new string[] { Center("LOTERIA NACIONAL", w), "2" });

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w)});
            j.Add(new string[] { Center(pDireccion, w)});
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            if (pResponse.Monto_TicketPagadoFree != null)
            {
                j.Add(new string[] { Center("Jugada Ticket Free", w), "2" });
            }
            j.Add(new string[] { ("--------------------------------")});
            j.Add(new string[] { ("Ticket No. ".PadLeft(0)) + viewModel.TicketMartlon });
            j.Add(new string[] { ("Sorteo:       " + viewModel.NumeroSorteo.PadLeft(0)) });
            j.Add(new string[] { ("Fecha Sorteo: " + FechaHelper.FormatFecha(DateTime.ParseExact(viewModel.FechaSorteo, "dd-MM-yyyy", CultureInfo.InvariantCulture), FechaHelper.FormatoEnum.FechaCortaRegional).PadLeft(0)) });
            //j.Add(new string[] { ("Hora Sorteo:  " + FechaHelper.FormatFecha(DateTime.Parse(pResponse.Hora_Sorteo.ToString()), FechaHelper.FormatoEnum.HoraCortaRegional).PadLeft(0)) });
            j.Add(new string[] { ("Hora Sorteo:  " + viewModel.HoraSorteo) });
            j.Add(new string[] { (Center("JUGADAS", w)) });
            j.Add(new string[] { (Justify("AZUL  ROJO  EXTRA", "  MONTO", w)), "2" }); 
            foreach (var item in viewModel.Jugadas)
            {
                string jugadaCompleta = item.Jugada.Replace("x", " ");
                int fullLenght = jugadaCompleta.Length;
                int boloExtraIniLenght = jugadaCompleta.Length - 3;
                string tomboUnoYdos = jugadaCompleta.Substring(0, boloExtraIniLenght);
                string tomboExtra = jugadaCompleta.Split('-')[3];
                string jugadaFormateada = tomboUnoYdos + "  " + tomboExtra;
                j.Add(new string[] { (Justify(jugadaFormateada, item.Monto.ToString("N2"), w)), "2" });
            }

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { (Justify("TOTAL: ", "$"+ viewModel.TicCosto,w)), "2" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { ("Serial:  " + (viewModel.Serial) + "") });
            j.Add(new string[] { ("Tik MAR: " + viewModel.TicketID + " Pin:" + viewModel.Pin) });
            j.Add(new string[] { (viewModel.TicketControl) });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { ("Presentar su ticket al cobrar.") ,"1"});
            j.Add(new string[] { ("Prohibida la venta a menores de 18 anios. Ticket ganador prescribe a los 60 dias. Mas de 10 ganadores del premio mayor 3,000,000.00 entre los ganadores"), "1" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { (Center("Buena suerte!...", w)) });
            j.Add(new string[] { ("".PadLeft(0)) });
            j.Add(new string[] { ("".PadLeft(0)) });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }


        internal static List<string[]> ReImprimirVentaJuegaMas(int w, JuegaMasViewModel.MarltonResponse pResponse, JuegaMasViewModel pDTicket, string pBanca, string pDireccion, string pPin)
        {
            var j = new List<string[]>();
            string printString = "";



            j.Add(new string[] { Center("JuegaMas PegaMas", w), "2" });

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });
            j.Add(new string[] { Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                    FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            if (pResponse.Monto_TicketPagadoFree != null)
            {
                j.Add(new string[] { Center("Jugada Ticket Free", w), "2" });
            }
            j.Add(new string[] { ("--------------------------------") });
            j.Add(new string[] { ("Ticket No. ".PadLeft(0)) + pDTicket.TicketID });
            j.Add(new string[] { ("Sorteo:       " + pResponse.Numero_Sorteo.ToString().PadLeft(0)) });
            j.Add(new string[] { ("Fecha Sorteo: " + FechaHelper.FormatFecha(DateTime.ParseExact(pResponse.Fecha_Sorteo, "dd-MM-yyyy", CultureInfo.InvariantCulture), FechaHelper.FormatoEnum.FechaCortaRegional).PadLeft(0)) });
            //j.Add(new string[] { ("Hora Sorteo:  " + FechaHelper.FormatFecha(DateTime.Parse(pResponse.Hora_Sorteo.ToString()), FechaHelper.FormatoEnum.HoraCortaRegional).PadLeft(0)) });
            j.Add(new string[] { ("Hora Sorteo:  " + pResponse.Hora_Sorteo.ToString()) });
            j.Add(new string[] { ("-------------JUGADAS-----------".PadLeft(w)), "2" });
            j.Add(new string[] { (Justify("AZUL  ROJO  EXTRA", "  MONTO", w)), "2" });
            foreach (var item in pDTicket.Jugadas)
            {
                string jugadaCompleta = item.Jugada.Replace("x", " ");
                int fullLenght = jugadaCompleta.Length;
                int boloExtraIniLenght = jugadaCompleta.Length - 3;
                string tomboUnoYdos = jugadaCompleta.Substring(0, boloExtraIniLenght);
                string tomboExtra = jugadaCompleta.Split('-')[3];
                string jugadaFormateada = tomboUnoYdos + "  " + tomboExtra;
                j.Add(new string[] { (Justify(jugadaFormateada, item.Monto.ToString("N2"), w)), "2" });
            }

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { (Justify("TOTAL: ", "$" + pResponse.Monto.ToString(), w)), "2" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { ("Serial:  " + (pResponse.Serial) + "") });
            j.Add(new string[] { ("Tik MAR: " + pDTicket.TicketID + " Pin:" + pPin) });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { ("Presentar su ticket al cobrar."), "1" });
            j.Add(new string[] { ("Prohibida la venta a menores de 18 anios. Ticket ganador prescribe a los 60 dias. Mas de 10 ganadores del premio mayor 3,000,000.00 entre los ganadores"), "1" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { (Center("Buena suerte!...", w)) });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> ImprimirReporteBilletesPorFecha(int w, ICollection<DTicket> pDTickets, string pBanca, string pDireccion, DateTime pFechaData)
        {
            var j = new List<string[]>();
            string printString = "";

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });
            j.Add(new string[] { Center("FECHA DE REPORTE", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            j.Add(new string[] { Center("FECHA DE VENTA", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(pFechaData),
                FechaHelper.FormatoEnum.FechaCortaDOW), w)});

            j.Add(new string[] { ("--------REPORTE JuegaMas--------".PadLeft(w)), "2" });

            decimal total = 0;
            j.Add(new string[] { (Justify("HORA        SERIE", " MONTO", w)) });
            foreach (var ticket in pDTickets)
            {
                j.Add(new string[] { (ticket.TicFecha.ToString("t").PadRight(12, ' ') + ticket.TicSolicitud.ToString().PadRight(10, ' ') + ticket.TicCosto.ToString("N2").PadLeft(10, ' ')) });
                total += ticket.TicCosto;
            }
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { ("MONTO TOTAL: $" + total.ToString("N2").PadLeft(0)), "2" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        public static List<string[]> ImprimirGanadoresJuegaMas(int w, string pGanadores, DateTime pFechaData)
        {
            var j = new List<string[]>();

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center("Ganadores JuegaMas", w) });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center("FECHA SORTEO",w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(pFechaData),
                FechaHelper.FormatoEnum.FechaCortaDOW), w)});


            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pGanadores, w) });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }


        internal static List<string[]> ImprimirPagoGanador(string pBanca, int w, string mensaje, int aprobado, string pDireccion)
        {
            var j = new List<string[]>();
            string printString = "";

            j.Add(new string[] { ("-----RECIBO DE JuegaMas-----".PadLeft(w)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });
            j.Add(new string[] { Center("FECHA DE PAGO", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { (mensaje) });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { (Justify("Aprobacion: " + aprobado, "", w)) });
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        private static string CortarString(string text, int length)
        {
            if (text.Length > length)
            {
                text = text.Substring(0, length);
            }
            else
            {
                for (int i = text.Length; i < 8; i++)
                {
                    text += " ";
                }
            }
            return text;
        }

        private static string Center(string pText, int pWidth)
        {
            if (pText == null) pText = string.Empty;
            pText = pText.Trim();
            if (pText.Length > pWidth)
            {
                pText = pText.Substring(0, pWidth);
            }

            return pText.PadLeft(Convert.ToInt32((pWidth - pText.Length) / 2) + pText.Length, ' ').PadRight(pWidth, ' ');
        }

        private static string Justify(string pText1, string pText2, int pWidth)
        {
            if (pText1 == null) pText1 = string.Empty;
            if (pText2 == null) pText2 = string.Empty;
            pText1 = pText1.Trim();
            pText2 = pText2.Trim();
            var theLen = pWidth - pText1.Length - pText2.Length;
            if (theLen <= 0) theLen = 1;
            return pText1.PadRight(pText1.Length + theLen, ' ') + pText2;
        }

      
    }
}
