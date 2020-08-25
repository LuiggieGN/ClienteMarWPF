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

namespace MAR.BusinessLogic.Code.PrintJobs
{
    class BilletePrintJob
    {
        internal static List<string[]> ImprimirVentaBillete(int w, BilleteViewModel.MarltonResponse pResponse, DTicket pDTicket, string pBanca, string pDireccion, string pPin)
        {
            var j = new List<string[]>();

            WebClient webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData("http://www.google.com/images/logos/ps_logo2.png");

            j.Add(new string[] { Center("BILLETE MILLONARIO", w) });

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w)});
            j.Add(new string[] { Center(pDireccion, w)});
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});

            j.Add(new string[] { ("--------------------------------")});
            j.Add(new string[] { ("Ticket No. ".PadLeft(0)) + pResponse.TicketNo });
            j.Add(new string[] { ("Sorteo:       " + pResponse.Numero_Sorteo.ToString().PadLeft(0)) });
            j.Add(new string[] { ("Fecha Sorteo: " + FechaHelper.FormatFecha(DateTime.Parse(pResponse.Fecha_Sorteo.ToString()), FechaHelper.FormatoEnum.FechaCortaRegional).PadLeft(0)) });
            j.Add(new string[] { ("Hora Sorteo:  " + FechaHelper.FormatFecha(DateTime.Parse(pResponse.Hora_Sorteo.ToString()), FechaHelper.FormatoEnum.HoraCortaRegional).PadLeft(0)) });
            j.Add(new string[] { ("-------------BILLETES-----------".PadLeft(w)), "2" });
            j.Add(new string[] { (Justify("BILLETE   FRACCIONES", "  MONTO", w)) });
            foreach (var item in pDTicket.DTicketDetalles)
            {
                string fraIni = item.TDeNumero.Split('-')[1];
                string fraFin = " AL " + item.TDeNumero.Split('-')[2];
                if (fraFin == fraIni)
                {
                    fraFin = string.Empty;  
                }
                j.Add(new string[] { (item.TDeNumero.Split('-')[0].PadLeft(5, '0').PadRight(11, ' ') + fraIni + fraFin + item.TDeCosto.ToString("N2").PadLeft(13, ' ')) });
            }

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { ("TOTAL: $" + pResponse.Monto.ToString().PadLeft(0)), "2" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { ("Serial:  " + (pResponse.Serial) + "") });
            j.Add(new string[] { ("Tik MAR: " + pDTicket.TicNumero + " Pin: " + pPin) });
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

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });
            j.Add(new string[] { Center("FECHA DE REPORTE", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            j.Add(new string[] { Center("FECHA DE VENTA",w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(pFechaData),
                FechaHelper.FormatoEnum.FechaCortaDOW), w)});

            j.Add(new string[] { ("--------REPORTE BILLETES--------".PadLeft(w)), "2" });

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


        internal static List<string[]> ImprimirPagoGanador(string pBanca, int w, string mensaje, int aprobado, string pDireccion)
        {
            var j = new List<string[]>();
            string printString = "";

            j.Add(new string[] { ("-----RECIBO PAGO DE BILLETE-----".PadLeft(w)) });
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
