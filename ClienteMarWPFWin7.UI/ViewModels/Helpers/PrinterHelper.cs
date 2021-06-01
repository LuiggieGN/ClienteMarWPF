using CustomPrinterLib;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Helpers
{
    public static class PrinterHelper
    {
        static string eClear;
        static string eDrawer;
        static string eCut;
        static string LineCut;

        static PrinterHelper()
        {
            eClear = ((char)27) + "@";
            eDrawer = eClear + ((char)27) + "p" + ((char)0) + ".}";
            eCut = ((char)27) + "i" + ((char)13) + ((char)10);
            LineCut = "\n\n" + eCut + eDrawer;

        }

        public static bool SendToPrinter(string pPrintText)
        {

            var ps = new PrinterSettings();

            if (pPrintText == null || pPrintText == string.Empty)
            {
                return false;
            }
            RawPrinterHelper.SendStringToPrinter(ps.PrinterName, pPrintText+LineCut);
            //if (PrinterSettings.InstalledPrinters.Count == 0 || ps.PrinterName == null)
            //{
            //    //    Imprimir al puerto LPT1
            //     FileStream fs = new FileStream(@"\mar\Last.PRN", FileMode.Create);
            //     StreamWriter FileWriter = new StreamWriter(fs);
            //     FileWriter.WriteLine(pPrintText);
            //    FileWriter.Close();
            //    fs = new FileStream(@"\mar\Last.bat", FileMode.Create);
            //    FileWriter = new StreamWriter(fs);
            //    FileWriter.WriteLine("@ECHO OFF");

            //    FileWriter.WriteLine(@"TYPE \mar\last.prn >prn");
            //    FileWriter.WriteLine(@"DEL \mar\last.prn");
            //    FileWriter.Close();
            //    //Shell(@"\mar\last.bat", AppWinStyle.Hide, false)
            //}
            //else
            //{

            //}

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

            SendToPrinter(sb.ToString());

            return true;
        }



    }
}// fin de clase
