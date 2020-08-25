using MAR.AppLogic.MARHelpers;
using MarConnectCliente.RequestModels;
using MarConnectCliente.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.PrintJobs
{
    class CincoMinutosPrintJob
    {
        internal static List<string[]> ImprimirCincoMinutos(int w, string pBanca, string pDireccion, string pRecibo, string pPin, bool pEsCopia, ApuestaRequestModel pRequestModel, 
            ApuestaResponseModel pResponseModel, string pFood, string pHead, int pBancaId,string pFirma)
        {
            var j = new List<string[]>();
            j.Add(new string[] { Center(TruncateString("-----------------------------------------", w), w) });
            if (!string.IsNullOrEmpty(pHead)) j.Add(new string[] { Center(pHead, w) });
            j.Add(new string[] { Center("** SORTEOS MAR **", w)});
            
            j.Add(new string[] { Center(TruncateString("-----------------------------------------", w), w) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w)});
            j.Add(new string[] { Center(pDireccion, w), "1" });
            j.Add(new string[] { Justify(AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(pResponseModel.FechaHoraRespuesta),
                AppLogic.MARHelpers.FechaHelper.FormatoEnum.FechaCortaDOW),Convert.ToDateTime(pResponseModel.FechaHoraRespuesta).ToShortTimeString(), w), "1" });

        
            j.Add(new string[] { Justify("Ticket: " + pRecibo, "Pin:" + pPin, w) });
  
            if (pEsCopia) j.Add(new string[] { Center("** COPIA REIMPRESA **", w)});
















          



            j.Add(new string[] { Justify("", "", w) });

            var sorteosNumeros = pResponseModel.TicketDetalle.Select(x => new { x.SorteoNumero, x.Sorteo,x.SorteoFecha}).Distinct();

            foreach (var item in sorteosNumeros)
            {
                j.Add(new string[] { Center("**" +item.Sorteo.ToUpper()+ "**", w) });

                j.Add(new string[] { Center("Sorteo#: " + item.SorteoNumero.ToUpper() + " Hora: " + AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(item.SorteoFecha),
                AppLogic.MARHelpers.FechaHelper.FormatoEnum.HoraCortaRegional), w)});
                //j.Add(new string[] { Justify("* Sorteo#: "+item.SorteoNumero,"Hora: "+ AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(item.SorteoFecha),
                //AppLogic.MARHelpers.FechaHelper.FormatoEnum.HoraCortaRegional), w) });
            }




            j.Add(new string[] { Justify("JUGADA", "MONTO", w) });
            var juegos = pRequestModel.DesgloseOperacion.Detalle.Juego.OrderBy(x => x.TipoJugadaID);
            var tiposJugadas = juegos.Select(x =>x.TipoJugadaID).Distinct();
            foreach (var tipo in tiposJugadas)
            {
                if (tipo == 1)
                {
                    j.Add(new string[] { Center(TruncateString("--QUINIELAS--", w), w) });
                    foreach (var item in juegos.Where(x => x.TipoJugadaID == 1))
                    {
                        j.Add(new string[] { Justify(item.Jugada, item.Monto.ToString("C2"), w) });
                    }
                }
                else if (tipo == 2)
                {
                    j.Add(new string[] { Center(TruncateString("--PALES--", w), w) });
                    foreach (var item in juegos.Where(x => x.TipoJugadaID == 2))
                    {
                        j.Add(new string[] { Justify(item.Jugada, item.Monto.ToString("C2"), w) });
                    }
                }
                else if (tipo == 3)
                {
                    j.Add(new string[] { Center(TruncateString("--TRIPLETAS--", w), w) });
                    foreach (var item in juegos.Where(x => x.TipoJugadaID == 3))
                    {
                        j.Add(new string[] { Justify(item.Jugada, item.Monto.ToString("C2"), w) });
                    }
                }
               
            }
          
            j.Add(new string[] { Justify("", "", w) });
            j.Add(new string[] { Center(TruncateString("-----------------------------------------",w), w) });



            j.Add(new string[] { Center( "TOTAL:$ " + pRequestModel.MontoOperacion.ToString("N2"), w)});


            j.Add(new string[] { Center(TruncateString("-----------------------------------------", w), w)});

            string dbName = MAR.AppLogic.MARHelpers.DALHelper.GetSqlConnection().Database.Replace("DATA","");
            j.Add(new string[] { Justify(TruncateString("Autenticacion", w), pResponseModel.NumeroAutentificacion, w) });
            j.Add(new string[] { Justify(TruncateString("Firma", w), dbName + pFirma, w) });
            if (!string.IsNullOrEmpty(pFood)) j.Add(new string[] { Center(pFood, w) });

            return j;
        }


        internal static List<string[]> ImprimirReciboPago(string pDireccion, string pBanca, int w, string pTicket, int pMonto, string pAutenticacion)
        {
            var j = new List<string[]>();

            j.Add(new string[] { Center("RECIBO DE PAGO", w) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });
            j.Add(new string[] { Center("FECHA DE PAGO", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            j.Add(new string[] { Center("-------------------------------", w) });
            j.Add(new string[] { Center("SORTEO CINCO MINUTOS", w) });
          
            j.Add(new string[] { (Justify("Monto Pagado: ", pMonto.ToString("C2"), w)) });
            j.Add(new string[] { (Justify(pTicket + " ", "Aprobado", w)) });
            j.Add(new string[] { (Justify("AUTORIZACION: " , pAutenticacion, w)) });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        static string[] SplitText(string pText, int pMaxLen)
        {

            int strLength = pText.Length;
            int strCount = (strLength + pMaxLen - 1) / pMaxLen;
            string[] result = new string[strCount];
            for (int i = 0; i < strCount; ++i)
            {
                result[i] = pText.Substring(i * pMaxLen, Math.Min(pMaxLen, strLength));
                strLength -= pMaxLen;
            }
            return result;

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


        private static string TruncateString(string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }


    }
}
