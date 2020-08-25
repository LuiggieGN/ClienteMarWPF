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
    class ReportsePrintJob
    {
        internal static List<string[]> ImprimirListaPremios(string pBanca, DateTime pFecha, int w, IList<ReportesViewModels.ReportePremios> premios)
        {
            var j = new List<string[]>();
            string printString = "";

            j.Add(new string[] { (Center("REPORTE DE PREMIOS",20)) });
            j.Add(new string[] { (Center(pBanca.ToUpper(),  20))});
            j.Add(new string[] { (Center("FECHA DE PREMIOS",  20)) });
          
            j.Add(new string[] {
              Justify(FechaHelper.FormatFecha(Convert.ToDateTime(pFecha),
                FechaHelper.FormatoEnum.FechaCortaDOW)," ", w)});

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { (Justify("LOTERIAS ", "1ra  2da  3ra", w)) });
            j.Add(new string[] {""});

            foreach (var item in premios)
            {
                j.Add(new string[] { (CortarString(item.Loteria,8) + " "+ item.Primera + "   " + item.Segunda + "   " + item.Tercera) });
            }

            j.Add(new string[] { ("------------------------------".PadLeft(0)) });
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            return j;
        }



        internal static List<string[]> ImprimirReportePagosServicios(int w, string pBanca, string pDireccion, DateTime pDesde, ReportesViewModels.ReportePagosServicios[] pTransacciones)
        {
            var j = new List<string[]>();
            decimal total = 0;
            j.Add(new string[] { Center("REPORTE PAGAFACIL", w) });

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });

            j.Add(new string[] { Center("FECHA IMPRESION", w) });
            j.Add(new string[] { Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                    FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});


            j.Add(new string[] { Center("FECHA DE REPORTE", w) });
            j.Add(new string[] { Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(pDesde),
                    FechaHelper.FormatoEnum.FechaCortaDOW), w)});

            j.Add(new string[] { ("------------PAGOS---------------") });
            j.Add(new string[] { (Justify("PROVEEDOR", "  MONTO", w)) });


            for (int i = 0; i < pTransacciones.Count(); i++)
            {
                j.Add(new string[] { (Justify(pTransacciones[i].Suplidor + " " + pTransacciones[i].Producto, pTransacciones[i].Monto, w)) });
                total += decimal.Parse(pTransacciones[i].Monto);
            }
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { Justify("TOTAL:", "$" + total.ToString("N2"), w), "2" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            return j;
        }

        internal static List<string[]> ImprimirReporteJuegaMas(int w, string pBanca, string pDireccion, DateTime pDesde, ReportesViewModels.ReporteJuegaMasCliente[] pTransacciones)
        {
            var j = new List<string[]>();
            decimal total = 0;
            j.Add(new string[] { Center("REPORTE JuegaMas", w) });

            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });

            j.Add(new string[] { Center("FECHA IMPRESION", w) });
            j.Add(new string[] { Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                    FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});

            j.Add(new string[] { Center("FECHA DE REPORTE", w) });
            j.Add(new string[] { Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(pDesde),
                    FechaHelper.FormatoEnum.FechaCortaDOW), w)});

            j.Add(new string[] { ("------------Jugadas---------------") });
            j.Add(new string[] { (Justify("Serial", "  MONTO", w)) });

            for (int i = 0; i < pTransacciones.Count(); i++)
            {
                j.Add(new string[] { (Justify(pTransacciones[i].Serial, pTransacciones[i].Monto, w)) });
                total += decimal.Parse(pTransacciones[i].Monto);
            }
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { Justify("TOTAL:", "$" + total.ToString("N2"), w) });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

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
