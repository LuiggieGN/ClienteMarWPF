using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.PrintJobs
{
    class Pega4PrintJob
    {
        internal static List<string[]> ImprimirPaga4(int w, string pBanca, string pDireccion, string pRecibo, string pPin, bool pEsCopia, Models.ResponseModel.Pega4ResponseModel.TicketViewModel pTrans, string pFood, string pHead, int pBancaId,string pFirma)
        {
            var j = new List<string[]>();

            j.Add(new string[] { Center(pBanca.ToUpper(), w), "2" });
            j.Add(new string[] { Center(pDireccion, w), "1" });
            j.Add(new string[] { Justify(AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(pTrans.TicFecha),
                AppLogic.MARHelpers.FechaHelper.FormatoEnum.FechaCortaDOW),Convert.ToDateTime(pTrans.TicFecha).ToShortTimeString(), w), "1" });

            if (!string.IsNullOrEmpty(pHead)) j.Add(new string[] { Center(pHead, w), "1" });
            j.Add(new string[] { Justify("Ticket: " + pRecibo, "Pin:" + pPin, w), "1" });
  
            if (pEsCopia) j.Add(new string[] { Center("** COPIA REIMPRESA **", w), "2" });

            List<string> tipos = pTrans.Jugadas.Select(x => x.Sorteo).Distinct().OrderByDescending(line => Regex.Matches(line, "Dia").Count).ToList();

            foreach (var item in tipos)
            {
                j.Add(new string[] { "--" + item + TruncateString("-----------------------", w - (item.Length + 2)), "2" });

                foreach (var jug in pTrans.Jugadas.Where(x => x.Sorteo == item))
                {
                    j.Add(new string[] { Justify(jug.Jugada, jug.Monto.ToString("C2"), w), "2" });
                }
            }

            j.Add(new string[] { Center(TruncateString("-----------------------------------------",w), w), "2" });



            j.Add(new string[] { Center( "TOTAL:$ " + pTrans.TicCosto.ToString("N2"), w), "2" });


            j.Add(new string[] { Center(TruncateString("-----------------------------------------", w), w), "2" });

            string dbName = MAR.AppLogic.MARHelpers.DALHelper.GetSqlConnection().Database.Replace("DATA","");
            j.Add(new string[] { Justify(TruncateString("Firma", w), dbName + pFirma, w) });
            if (!string.IsNullOrEmpty(pFood)) j.Add(new string[] { Center(pFood, w) });

            return j;
        }


        internal static List<string[]> ImprimirReciboPago(string pDireccion, string pBanca, int w, int pRecibo, List<MAR.DataAccess.EFRepositories.RFRepositories.RFPagosRepository.PagoRealizado> tReferencias)
        {
            var j = new List<string[]>();

            j.Add(new string[] { Center("RECIBO DE PAGO", w) });
            j.Add(new string[] { Center(pBanca.ToUpper(), w) });
            j.Add(new string[] { Center(pDireccion, w) });
            j.Add(new string[] { Center("FECHA DE PAGO", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            j.Add(new string[] { ("--------------------------------".PadLeft(0)) });
     
            for (int i = 0; i < tReferencias.Count; i++)
            {
                j.Add(new string[] { (Justify(tReferencias[i].ReferenciaTrans + " ", "Aprobado", w)) });
            }
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
