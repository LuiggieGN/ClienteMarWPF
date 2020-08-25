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
    class PagaFacilPrintJob
    {
        internal static List<string[]> ImprimirPagoServicio(int w, string pBanca, string pDireccion,string pRef, decimal pMonto, string pNic, string xid, string pPin, string pProveedor, bool pEsCopia)
        {
            var j = new List<string[]>();
          
            j.Add(new string[] { Center("PAGAFACIL", w), "2" });
            j.Add(new string[] { Center("PAGO DE SERVICIO", w), "2" });
            j.Add(new string[] { Center(pProveedor, w), "2" });
            if (pEsCopia)
            {
                j.Add(new string[] { Center("** COPIA REIMPRESA **", w), "2" });
            }
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w)});
            j.Add(new string[] { Center(pDireccion, w)});
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});

            j.Add(new string[] { ("--------------------------------")});
            j.Add(new string[] { ("Factura No." + xid + " Pin: " + pPin) });
            j.Add(new string[] { ("------------PAGOS---------------") });
            j.Add(new string[] { (Justify("Referencia", "  MONTO", w)) });
            j.Add(new string[] { (Justify(pRef, pMonto.ToString("N2"), w)) });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { ("NIC:" + pNic.PadLeft(0)), "2" });
            j.Add(new string[] { Justify("TOTAL:", "$" + pMonto.ToString("N2"),w), "2" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { (Center("Gracias por su pago!...", w)) });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> ImprimirReportePagosServicios(int w, string pBanca, string pDireccion, DateTime pDesde, VP_Transaccion[] pTransacciones)
        {
            var j = new List<string[]>();
            decimal total = 0;
            j.Add(new string[] { Center("REPORTE PAGAFACIL", w), "2" });
           
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });

            j.Add(new string[] { Center("FECHA IMPRESION", w), "2" });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});


            j.Add(new string[] { Center("FECHA DE REPORTE", w), "2" });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});

            j.Add(new string[] { ("--------------------------------") });
            j.Add(new string[] { ("------------PAGOS---------------") });
            j.Add(new string[] { (Justify("PROVEEDOR", "  MONTO", w)) });

     
            for (int i = 0; i < pTransacciones.Count(); i++)
            {
                j.Add(new string[] { (Justify(pTransacciones[i].VP_Suplidor.Nombre, pTransacciones[i].Ingresos.ToString("N2"), w)) });
                total =+ pTransacciones[i].Ingresos;
            }
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });

            j.Add(new string[] { Justify("TOTAL:", "$" + total.ToString("N2"), w), "2" });
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
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
