using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CustomPrinterLib;
using InterchangeDataLib;

namespace TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private const string BetReceipt = "BetReceipt", CashReport = "CashReport",
            PaidReport = "PaidReport", AuthReport = "AuthReport";

        private void button1_Click(object sender, EventArgs e)
        {
            //StringConverterHelper.Create();
            try
            {
                // Load templates
                TemplateManager templateManager = new TemplateManager();
                templateManager.Init(@"xml\InternationalChars.xml");
                templateManager.LoadTemplate(@"xml\TemplateES.xml", "ES", BetReceipt);
                templateManager.LoadTemplate(@"xml\CashReportES.xml", "ES", CashReport);
                templateManager.LoadTemplate(@"xml\PaidReportES.xml", "ES", PaidReport);
                templateManager.LoadTemplate(@"xml\AuthES.xml", "ES", AuthReport);

                // Create printer engine (USB mode)
                CustomPrinterEngine engine = new CustomPrinterEngine("Custom KUBE 80mm (200dpi)");
                // Create printer engine (RS232 mode)
                //CustomPrinterEngine engine = new CustomPrinterEngine("COM11", 19200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
                ReceiptData receipt = new ReceiptData();
                receipt.BetType = "multiple";
                receipt.Amount = 10;
                receipt.FinalPrice = (decimal)12.5;
                receipt.PlaceDate = DateTime.Now;
                receipt.PossibleReturn = 50;
                receipt.ReceiptID = "123456789";
                receipt.TerminalID = "A010";
                receipt.UserName = "Alfredo";
                receipt.AddLeg(DateTime.Now, "Roma - Sampdoria", "1X2", "1", 5);
                receipt.AddLeg(DateTime.Now.AddDays(3).AddHours(10), "Mìlan - Juventus", "Odd/Even", "Odd", (decimal)2.5);
                receipt.AddLeg(DateTime.Now.AddDays(2).AddHours(8), "La Coroña - Real Madrid", "1X2", "2", (decimal)3.1);
                //receipt.AddLeg(DateTime.Now.AddDays(2).AddHours(8), "Aaàá ÁtÀ", "1X2", "2", (decimal)3.1);
                receipt.AddLeg(DateTime.Now.AddDays(2).AddHours(8), "Eeèé ÉtÈ", "1X2", "2", (decimal)3.1);
                //receipt.AddLeg(DateTime.Now.AddDays(2).AddHours(8), "Iiìí ÍtÌ", "1X2", "2", (decimal)3.1);
                //receipt.AddLeg(DateTime.Now.AddDays(2).AddHours(8), "Ooòó ÓtÒ", "1X2", "2", (decimal)3.1);
                //receipt.AddLeg(DateTime.Now.AddDays(2).AddHours(8), "Uuùú ÚtÙ", "1X2", "2", (decimal)3.1);
                Console.Out.WriteLine(receipt.ToString());
                engine.DataToPrint = receipt;
                engine.Print(templateManager, "ES", BetReceipt);
                //engine.Print(templateManager, "ES", BetReceipt, 3000);
                if (!engine.IsPrinted)
                {
                    MessageBox.Show("Ticket 1 no printed!");
                }

                CashReportData cashReport = new CashReportData();
                cashReport.PrintDate = DateTime.Now;
                cashReport.ShopID = "Cava Baja (Madrid)";
                cashReport.TerminalID = "A010";
                cashReport.UserName = "Alfredo";
                cashReport.CashOpen = 1000;
                cashReport.PlacedBet.Counter = 150;
                cashReport.PlacedBet.Amount = 2300;
                cashReport.CancelledBet.Counter = 2;
                cashReport.CancelledBet.Amount = 20;
                cashReport.PaidBet.Counter = 5;
                cashReport.PaidBet.Amount = 280;
                cashReport.CashClose = 3000;
                Console.Out.WriteLine(cashReport.ToString());
                engine.DataToPrint = cashReport;
                engine.Print(templateManager, "ES", CashReport);
                //engine.Print(templateManager, "ES", CashReport, 3000);
                if (!engine.IsPrinted)
                {
                    MessageBox.Show("Ticket 2 no printed!");
                }

                PaidReportData paidReport = new PaidReportData();
                paidReport.PrintDate = DateTime.Now;
                paidReport.ShopID = "Cava Baja (Madrid)";
                paidReport.TerminalID = "A010";
                paidReport.UserName = "Alfredo";
                paidReport.PaidList.Add("123456789012", 50);
                paidReport.PaidList.Add("123456789013", (decimal)60.76);
                paidReport.PaidList.Add("123456789014", (decimal)1458.98);
                paidReport.TotalPaid = (decimal)1569.74;
                Console.Out.WriteLine(paidReport.ToString());
                engine.DataToPrint = paidReport;
                engine.Print(templateManager, "ES", PaidReport);
                //engine.Print(templateManager, "ES", PaidReport, 3000);
                if (!engine.IsPrinted)
                {
                    MessageBox.Show("Ticket 3 no printed!");
                }

                AuthTicketData authReport = new AuthTicketData();
                authReport.Date = DateTime.Now;
                authReport.Firstname = "Alessandro";
                authReport.Lastname = "Lentini";
                authReport.ID = "1234567890";
                Console.Out.WriteLine(authReport.ToString());
                engine.DataToPrint = authReport;
                engine.Print(templateManager, "ES", AuthReport);
                //engine.Print(templateManager, "ES", AuthReport, 3000);
                if (!engine.IsPrinted)
                {
                    MessageBox.Show("Ticket 4 no printed!");
                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CustomPrinterEngine engine = new CustomPrinterEngine("COM11", 19200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            PrinterPaperStatusType status = engine.getPaperStatus();
            MessageBox.Show(status.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CustomPrinterEngine engine = new CustomPrinterEngine("COM11", 19200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            PrinterStatusType status = engine.getPrintStatus();
            MessageBox.Show(status.ToString());
        }
    }
}
