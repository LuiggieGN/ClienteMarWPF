using CustomPrinterLib;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Helpers
{
    public static class PrinterHelper
    {

        public static bool SendToPrinter(string pPrintText)
        {
            var ps = new PrinterSettings();
            RawPrinterHelper.SendStringToPrinter(ps.PrinterName, pPrintText);
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
