using CustomPrinterLib;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.UI.State.PinterConfig
{
    class PrintOutHelper
    {
        public static bool SendToPrinter(string pPrintText)
        {
            var ps = new PrinterSettings();
            RawPrinterHelper.SendStringToPrinter(ps.PrinterName, pPrintText);
            return true;
        }

        public static bool SendToPrinter(List<string[]> pPrintData)
        {

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < pPrintData.Count; i++)
            {
                for (int e = 0; e < pPrintData[i].Count(); e++)
                {
                    sb.AppendLine(pPrintData[i][0]);
                    break;
                }

            }

            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");
            sb.AppendLine("");

            sb.AppendLine();
            PrintOutHelper.SendToPrinter(sb.ToString());

            return true;
        }
    }
}
