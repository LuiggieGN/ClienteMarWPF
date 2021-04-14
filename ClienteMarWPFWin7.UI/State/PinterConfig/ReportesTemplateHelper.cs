using System;
using System.Collections.Generic;
using System.Text;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System.Collections;
using System.Globalization;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.Modules.Reporte;

namespace ClienteMarWPFWin7.UI.State.PinterConfig
{
    public static class ReporteTemplateHelper
    {
        private static List<ConfigPrinterModel> ConfigData = new List<ConfigPrinterModel>();
        //private static List<ConfigPrinterModel> ConfigData = BancaLogic.GetBancaConfig(7);
        private static PrinterSettings ps = new PrinterSettings();
        private static PaperSize paperSize = new PaperSize();
        private static int positionWrite = 0;
        private static object Value;
        private static bool ImprimirCopias;
        private static int CantidadLoterias;
        private static IAuthenticator AUTENTICATION;
        
        public static void PrintReporte(object value, IAuthenticator autenticador)
        {
            AUTENTICATION = autenticador;
            Value = value;
            var papers = ps.PaperSizes.Cast<PaperSize>();
            PrintDocument pd = new PrintDocument();
            StringBuilder sb = new StringBuilder();
           
            //paperSize = papers.FirstOrDefault();
             paperSize = new PaperSize("nose", 280, 0);

            if (value is string)
            {
                pd.PrintPage += TemplateString;
            }
            else if (value is TicketValue)
            {
               
                pd.PrintPage += TemplateTicket;
            }
            else if (value is MAR_RptVenta)
            {
               
                pd.PrintPage += TemplateRPTVentas;
            }

            else if (value is MAR_VentaNumero)
            {

                pd.PrintPage += TemplateRPTListNumeros;
            }

            else if (value is MAR_Ganadores)
            {

                pd.PrintPage += TemplateRPTListTickets;
            }

            pd.PrintController = new StandardPrintController();
            //pd.DefaultPageSettings.Margins.Left = 5;
            //pd.DefaultPageSettings.Margins.Right = 5;
            //pd.DefaultPageSettings.Margins.Top = 5;
            //pd.DefaultPageSettings.Margins.Bottom = 5;
            pd.DefaultPageSettings.PaperSize = paperSize;
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            pd.Print();

        }

        //LIST TEMPLATE
        private static void TemplateTicket(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            //CONFIG PRINT TICKECT
            var GetConfigTicket = ConfigData.Where(x => x.ConfigKey == "BANCA_PRINTER_CONFIG_LINE").FirstOrDefault();
            //CONFIG PRINT BAR AND QRCODE
            var GetConfigIMG = ConfigData.Where(x => x.ConfigKey == "BANCA_PRINTER_IMAGES_CONFIG").FirstOrDefault();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

            var ValueTicket = JsonConvert.DeserializeObject<List<ConfigPrinterValue>>(GetConfigTicket.ConfigValue);
            var ValueIMG = JsonConvert.DeserializeObject<List<ConfigPrinterValue>>(GetConfigIMG.ConfigValue);
            //var code = JsonConvert.DeserializeObject<ConfigPrinterValueCode>(ValueIMG.FirstOrDefault().Value);
            positionWrite = 0;
            var TotalGenerales = 0;
            foreach (var item in ValueTicket)
            {
                var data = GetValueForProperty(Value, item.Content) == null ? "" : GetValueForProperty(Value, item.Content);
                
                switch (item.Content)
                {
                    case "Logo":
                        WriteImage(g, data.ToString(), item.Size);
                        break;
                    case "BanNombre":
                        WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                        break;
                    //case "Loteria":
                    //    WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                    //    break;
                    case "Direccion":
                        WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                        break;
                    case "FechaActual":

                        string FechaActual = string.Empty;
                        if (data != null && data.ToString() != string.Empty)
                        {
                            FechaActual = "" + data.ToString();
                        }
                        WriteText(g, FechaActual, item.Size, item.FontStyle, item.Aligment);
                        break;
                    case "Telefono":

                        string Telefono = string.Empty;
                        if (data != null && data.ToString() != string.Empty)
                        {
                            Telefono = "Telefono: " + data.ToString();
                        }
                        WriteText(g, Telefono, item.Size, item.FontStyle, item.Aligment);
                        WriteText(g, "", item.Size, item.FontStyle, item.Aligment);
                        WriteText(g, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), 11, "bold", "center");

                        break;
                    case "Firma":

                        string Firma = string.Empty;
                        if (data != null && data.ToString() != string.Empty)
                        {
                            Firma = "Firma: " + data.ToString();
                        }
                        WriteText(g, Firma, item.Size, item.FontStyle, item.Aligment);
                        break;
                    case "AutorizacionHacienda":

                        string AutorizacionHacienda = string.Empty;
                        if (data != null && data.ToString() != string.Empty)
                        {
                            AutorizacionHacienda = "Autorizacion Hacienda:" + data.ToString();
                        }
                        WriteText(g, AutorizacionHacienda, item.Size, item.FontStyle, item.Aligment);
                        break;
                    //case "Ticket":

                    //string Ticket = string.Empty;
                    //if (data != null && data.ToString() != string.Empty)
                    //{
                    //    Ticket = "Ticket: " + data.ToString();
                    //}
                    //WriteText(g, Ticket, item.Size, item.FontStyle, item.Aligment);
                    //    break;
                    case "Qrcode":
                        WriteCode(g, GetValueCodeString(ValueIMG, TypeGenerateCode.QrCode), item.Size, TypeGenerateCode.QrCode);
                        break;
                    case "Barcode":
                        WriteCode(g, GetValueCodeString(ValueIMG, TypeGenerateCode.BarCode), item.Size, TypeGenerateCode.BarCode);

                        break;
                   
                    case "Jugadas":
                        WriteTextColumn(g, new List<string> { "JUGADAS", "MONTO" }, item.Size, item.FontStyle, item.Aligment);

                        var TicketJugadas = data as List<TicketJugadas>;
                        var Quiniela = TicketJugadas.Where(y => y.TipoJudaga == "Q").ToList();
                        var Pales = TicketJugadas.Where(y => y.TipoJudaga == "P").ToList();
                        var Tripleta = TicketJugadas.Where(y => y.TipoJudaga == "T").ToList();
                        TotalGenerales = TicketJugadas.Sum(x => x.Jugada.Monto);
                        TotalGenerales = TotalGenerales * CantidadLoterias;
                        if (true)
                        {

                        }
                        if (Quiniela.Count() > 0)
                        {
                            WriteText(g, "----Quinielas----", item.Size, item.FontStyle, item.Aligment);
                            WriteJugadas(g, Quiniela, item.Size, item.FontStyle, item.Aligment);
                        }
                       
                        if (Pales.Count() > 0)
                        {
                            WriteText(g, "----Pales----", item.Size, item.FontStyle, item.Aligment);
                            WriteJugadas(g, Pales, item.Size, item.FontStyle, item.Aligment);
                        }

                        if (Tripleta.Count() > 0) {
                            WriteText(g, "----Tripletas----", item.Size, item.FontStyle, item.Aligment);
                            WriteJugadas(g, Tripleta, item.Size, item.FontStyle, item.Aligment);
                        }
                        break; 
                        case "Total":

                        string Total = string.Empty;
                       
                        if (data != null && data.ToString() != string.Empty)
                        {
                            Total = "Total: $" + string.Format(nfi, "{0:C}", TotalGenerales);
                        }
                        WriteText(g, Total, item.Size, item.FontStyle, item.Aligment);
                        break;
                    //case "Pin":

                    //string Pin = string.Empty;
                    //if (data != null && data.ToString() != string.Empty)
                    //{
                    //    Pin = "Pin: " + data.ToString();
                    //}
                    //WriteText(g, Pin, item.Size, item.FontStyle, item.Aligment);
                    //    break;                        
                    case "LoteriaTicketPin":

                        if (data != null)
                        {
                            var loteriaTicketPin = data as List<LoteriaTicketPin>;
                            if (loteriaTicketPin.Count > 1)
                            {
                                
                                switch (item.FormatAnySorteo)
                                {

                                    case 1:
                                        foreach (var dataMore in loteriaTicketPin)
                                        {
                                            WriteText(g, dataMore.Loteria, item.Size, item.FontStyle, item.Aligment);
                                            WriteTextColumn(g, new List<string> { "T: " + dataMore.Ticket, "P: " + dataMore.Pin }, item.Size, item.FontStyle, item.Aligment);
                                        }
                                        break;

                                    case 2:
                                        WriteTextColumn(g, new List<string> { "Loteria", "Ticket", "Pin" }, item.Size, item.FontStyle, item.Aligment);
                                        foreach (var dataMore in loteriaTicketPin)
                                        {
                                            WriteTextColumn(g, new List<string> {  dataMore.Loteria, dataMore.Ticket, dataMore.Pin }, item.Size, item.FontStyle, item.Aligment);
                                        }
                                        break;

                                    default:
                                        foreach (var dataMore in loteriaTicketPin)
                                        {
                                            WriteText(g, dataMore.Loteria, item.Size, item.FontStyle, item.Aligment);
                                            WriteTextColumn(g, new List<string> { "T: " + dataMore.Ticket, "P: " + dataMore.Pin }, item.Size, item.FontStyle, item.Aligment);
                                        }
                                        break;
                                }
                                //WriteTextColumn(g, new List<string> { "Loteria", "Ticket", "Pin" }, (item.Size-5), item.FontStyle, item.Aligment);
                                //foreach (var dataMore in loteriaTicketPin)
                                //{
                                //    WriteTextColumn(g, new List<string> { dataMore.Loteria, dataMore.Ticket, dataMore.Pin }, (item.Size - 5), item.FontStyle, item.Aligment);
                                //}
                            }
                            else if (loteriaTicketPin.Count == 1)
                            {
                                     string mensajeCopiar = null;
                                     if (ImprimirCopias == true)
                                      {
                                        mensajeCopiar = "** C O P I A  R E I M P R E S A **";
                                       }
                                switch (item.FormatOneSorteo)
                                {
                                    case 1:

                                        var FormatOne = loteriaTicketPin.FirstOrDefault();
                                        WriteText(g, mensajeCopiar + FormatOne.Loteria, 10, item.FontStyle, item.Aligment);
                                        WriteText(g, "L: " + FormatOne.Loteria, item.Size, item.FontStyle, item.Aligment);
                                        WriteText(g, "T: " + FormatOne.Ticket, item.Size, item.FontStyle, item.Aligment);
                                        WriteText(g, "P: " + FormatOne.Pin, item.Size, item.FontStyle, item.Aligment);
                                        break;

                                    case 2:
                                        var FormatTwo = loteriaTicketPin.FirstOrDefault();
                                        WriteText(g, FormatTwo.Loteria, item.Size, item.FontStyle, item.Aligment);
                                        WriteTextColumn(g, new List<string> { "T: " + FormatTwo.Ticket, "P: " + FormatTwo.Pin }, item.Size, item.FontStyle, item.Aligment);
                                        break;

                                    default:
                                        var defaults = loteriaTicketPin.FirstOrDefault();
                                        WriteText(g, mensajeCopiar , 10, item.FontStyle, item.Aligment);
                                        WriteText(g, "L: " + defaults.Loteria, item.Size, item.FontStyle, item.Aligment);
                                        WriteText(g, "T: " + defaults.Ticket, item.Size, item.FontStyle, item.Aligment);
                                        WriteText(g, "P: " + defaults.Pin, item.Size, item.FontStyle, item.Aligment);
                                        break;
                                }

                            }
                        }

                        break;
                    case "Texto":
                        WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                        break;
                    case "Linea":
                        WriteLine(g);
                        break;
                }

            }
            positionWrite = 0;
            g.Dispose();
        }

        private static void TemplateRPTVentas(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string AlignamenCenter = "center";
            string AlignamentLft = "left";
            Console.WriteLine(Value);
            var Valor = Value as MAR_RptVenta;
            List<ConfigPrinterModel> valores = new  List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();


            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel Ventas = new ConfigPrinterModel() { ConfigKey = "Ventas", ConfigValue = "Ventas" };
            ConfigPrinterModel Premios = new ConfigPrinterModel() { ConfigKey = "Premios", ConfigValue = "Premios" };
            ConfigPrinterModel Nulos = new ConfigPrinterModel() { ConfigKey = "Nulos", ConfigValue = "Nulos" };
            ConfigPrinterModel SinNulos = new ConfigPrinterModel() { ConfigKey = "SinNulos", ConfigValue = "SinNulos" };


            valores.Add(Header);
            valores.Add(Ventas);

            if (nombreLoteria != "Todas") { 
                valores.Add(Premios);
            }

            if (Valor.TicketsNulos.Length > 0) {
                valores.Add(Nulos);
            }
            else
            {
                valores.Add(SinNulos);
            }

            positionWrite = 0;
           
            foreach (var item in valores)
            {
                var data = GetValueForProperty(Value, item.ConfigKey) == null ? "" : GetValueForProperty(Value, item.ConfigValue);
                

                switch (item.ConfigKey)
                {
                    case "Logo":
                        WriteImage(g, data.ToString(), 12);
                        break;
                    case "Header":
                        WriteText(g, AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre+" ID:"+ AUTENTICATION.BancaConfiguracion.BancaDto.BancaID , FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "REPORTE DE VENTAS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: "+nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;
                    //case "Loteria":
                    //    WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                    //    break;
                    

                    case "Ventas":

                        var TotalVentas = 0;
                        var VentasNeta = 0;

                        TotalVentas = Convert.ToInt32(Valor.Numeros + Valor.Pales + Valor.Tripletas);
                        VentasNeta = Convert.ToInt32(TotalVentas - Valor.Comision);
                        var TotalGanancia = (Convert.ToInt32((Valor.Numeros + Valor.Pales + Valor.Tripletas) - Valor.Comision)) - (Convert.ToInt32((Valor.MPrimero + Valor.MSegundo + Valor.MTercero) + Valor.MPales + Valor.MTripletas));
                        var TotalPremiados = string.Format(nfi, "{0:C}", Convert.ToInt32(Valor.MPrimero + Valor.MSegundo + Valor.MTercero) + Convert.ToInt32(Valor.MPales + Valor.MTripletas));

                        WriteTextColumn(g, new List<string> { "Numeros:      ",Valor.Numeros.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Numeros (RD$):", Valor.CntNumeros.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Pales:        ", Valor.Pales.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Tripletas:    ", Valor.Tripletas.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Total Ventas: ", TotalVentas.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Comision (0%):", Valor.Comision.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Ventas Netas: ", VentasNeta.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Numeros Premiados:      ",Convert.ToInt32(Valor.MPrimero + Valor.MSegundo + Valor.MTercero).ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Pales Premiados:", Valor.MPales.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Tripleta Premiados:", Valor.MTripletas.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "-------------------" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Total Premiados:", TotalPremiados }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        if (TotalGanancia >= 0)
                        {
                            WriteTextColumn(g, new List<string> { "Ganancia:", TotalGanancia.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        }
                        else if (TotalGanancia < 0)
                        {
                            WriteTextColumn(g, new List<string> { "Perdida:", TotalGanancia.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        }

                        break;
                    case "Premios":
                        WriteTextColumn(g, new List<string> { "", ""," ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "PREMIOS","CANTIDAD","GANA"}, FontSize-2, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "1RA.", Valor.Primero.ToString(),Valor.CPrimero.ToString("C2"), Valor.MPrimero.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "2DA.", Valor.Segundo.ToString(), Valor.CSegundo.ToString("C2"), Valor.MSegundo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "3RA.", Valor.Tercero.ToString(), Valor.CTercero.ToString("C2"), Valor.MTercero.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);


                        break;

                    case "Nulos":
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Ticket", "Hora", "Vendio" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        foreach (var ticket in Valor.TicketsNulos) {
                            WriteTextColumn(g, new List<string> { ticket.TicketNo, ticket.StrHora, ticket.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        }
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;

                    case "SinNulos":
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "--NO HAY TICKETS NULOS--" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;
                }

            }
            positionWrite = 0;
            g.Dispose();

        }

        private static void TemplateRPTListNumeros(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string AlignamenCenter = "center";
            string AlignamentLft = "left";
            var Valor = Value as MAR_VentaNumero;
            List<ConfigPrinterModel> valores = new List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();


            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel Numeros = new ConfigPrinterModel() { ConfigKey = "Numeros", ConfigValue = "Numeros" };
            ConfigPrinterModel Totales = new ConfigPrinterModel() { ConfigKey = "Totales", ConfigValue = "Totales" };

            valores.Add(Header);
            valores.Add(Numeros);

            positionWrite = 0;

            foreach (var item in valores)
            {
                var data = GetValueForProperty(Value, item.ConfigKey) == null ? "" : GetValueForProperty(Value, item.ConfigValue);

                var numerosQuinielas = Valor.Numeros.Where(x => x.QP.ToUpper() == "Q");
                var numerosPales = Valor.Numeros.Where(x => x.QP.ToUpper() == "P");
                var numerosTripletas = Valor.Numeros.Where(x => x.QP.ToUpper() == "T");

                switch (item.ConfigKey)
                {
                    case "Logo":
                        WriteImage(g, data.ToString(), 12);
                        break;
                    case "Header":
                        WriteText(g, AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre + " ID:" + AUTENTICATION.BancaConfiguracion.BancaDto.BancaID, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "LISTA DE NUMEROS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "","","","","","" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        break;
                   
                   case "Numeros":
                        WriteTextColumn(g, new List<string> { "===Detalle de numeros vendidos===" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Num-", "Cant", "Num-", "Cant", "Num-", "Cant" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        if (numerosQuinielas.ToList().Count > 0) { 
                        for (var i=0; i < numerosQuinielas.ToList().Count;i=+3)
                        {
                            if (i+3 <= numerosQuinielas.ToList().Count) {
                                var Colunm1 = numerosQuinielas.ToArray()[i];
                                var Colunm2 = numerosQuinielas.ToArray()[i+1];
                                var Colunm3 = numerosQuinielas.ToArray()[i+2];
                                WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), Colunm3.Numero, Colunm3.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                            }
                            if (i + 2 <= numerosQuinielas.ToList().Count)
                            {
                                var Colunm1 = numerosQuinielas.ToArray()[i];
                                var Colunm2 = numerosQuinielas.ToArray()[i+1];
                                WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                            }
                            if (i <= numerosQuinielas.ToList().Count)
                            {
                                var Colunm1 = numerosQuinielas.ToArray()[i];
                               
                            }
                        }
                        var TotalNumeros = numerosQuinielas.Sum(x => x.Costo);
                        WriteTextColumn(g, new List<string> { "----------------------------------------------------" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Numeros:", " ", TotalNumeros.ToString("C2"), "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        }
                        if (numerosPales.ToList().Count > 0)
                        {
                            WriteTextColumn(g, new List<string> { "====Detalle de pales vendidos====" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { "Num-", "Cant", "Num-", "Cant", "Num-", "Cant" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                            for (var i = 0; i < numerosPales.ToList().Count; i = +3)
                            {
                                if (i + 3 <= numerosPales.ToList().Count)
                                {
                                    var Colunm1 = numerosPales.ToArray()[i];
                                    var Colunm2 = numerosPales.ToArray()[i + 1];
                                    var Colunm3 = numerosPales.ToArray()[i + 2];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), Colunm3.Numero, Colunm3.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }
                                if (i + 2 <= numerosPales.ToList().Count)
                                {
                                    var Colunm1 = numerosPales.ToArray()[i];
                                    var Colunm2 = numerosPales.ToArray()[i + 1];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }
                                if (i <= numerosPales.ToList().Count)
                                {
                                    var Colunm1 = numerosPales.ToArray()[i];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), "", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }
                            }

                            var TotalPales = numerosPales.Sum(x => x.Costo);
                            WriteTextColumn(g, new List<string> { "---------------------------------------------------" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { "Pales:", " ", TotalPales.ToString("C2"), "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        }
                        if (numerosTripletas.ToList().Count > 0) { 
                        
                        WriteTextColumn(g, new List<string> { "====Detalle de tripletas vendidos====" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Num-", "Cant", "Num-", "Cant", "Num-", "Cant" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        for (var i = 0; i < numerosTripletas.ToList().Count; i = +3)
                        {
                            if (i + 3 <= numerosTripletas.ToList().Count)
                            {
                                var Colunm1 = numerosTripletas.ToArray()[i];
                                var Colunm2 = numerosTripletas.ToArray()[i + 1];
                                var Colunm3 = numerosTripletas.ToArray()[i + 2];
                                WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), Colunm3.Numero, Colunm3.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                            }
                            if (i + 2 <= numerosTripletas.ToList().Count)
                            {
                                var Colunm1 = numerosTripletas.ToArray()[i];
                                var Colunm2 = numerosTripletas.ToArray()[i + 1];
                                WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                            }
                            if (i <= numerosTripletas.ToList().Count)
                            {
                                var Colunm1 = numerosTripletas.ToArray()[i];
                                WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), "", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                            }
                        }
                        var TotalTripletas = numerosTripletas.Sum(x => x.Costo);
                        WriteTextColumn(g, new List<string> { "----------------------------------------------" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Tripletas:", " ", TotalTripletas.ToString("C2"), "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        }
                        WriteTextColumn(g, new List<string> { "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;
                   
                }

            }
            positionWrite = 0;
            g.Dispose();

        }

        private static void TemplateRPTListTickets(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string AlignamenCenter = "center";
            string AlignamentLft = "left";
            Console.WriteLine(Value);
            var Valor = Value as MAR_Ganadores;
            List<ConfigPrinterModel> valores = new List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();

            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel Ventas = new ConfigPrinterModel() { ConfigKey = "Tickets", ConfigValue = "Tickets" };
            
            valores.Add(Header);
            valores.Add(Ventas);

            positionWrite = 0;

            var totalCosto = Valor.Tickets.Sum(x => x.Costo);
            var totalPago = Valor.Tickets.Sum(x => x.Pago);
            var totalValidor = Valor.Tickets.Where(x => x.Nulo==false).Count();
            var totalNulos = Valor.Tickets.Where(x => x.Nulo == true).Count();

            foreach (var item in valores)
            {
                var data = GetValueForProperty(Value, item.ConfigKey) == null ? "" : GetValueForProperty(Value, item.ConfigValue);

                switch (item.ConfigKey)
                {
                    case "Logo":
                        WriteImage(g, data.ToString(), 12);
                        break;
                    case "Header":
                        WriteText(g, AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre + " ID:" + AUTENTICATION.BancaConfiguracion.BancaDto.BancaID, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "REPORTE DE LISTADO DE TICKETS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;
                 
                    case "Tickets":
                        WriteTextColumn(g, new List<string> { "Ticket", "Hora", "Vendio", "Saco" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        if (Valor.Tickets != null) {
                            foreach (var ticket in Valor.Tickets)
                            {
                                if (ticket.Nulo==false) { 
                                     WriteTextColumn(g, new List<string> { ticket.TicketNo, ticket.StrHora, ticket.Costo.ToString("C2"), ticket.Pago.ToString("C2") }, FontSize-1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                                }
                                else
                                {
                                    WriteTextColumn(g, new List<string> { ticket.TicketNo, ticket.StrHora, ticket.Costo.ToString("C2"), "NULO" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                                }
                            }
                            WriteTextColumn(g, new List<string> { "----------------------------------------------" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { "Venta: "+totalCosto.ToString("C2"), "Saco: "+totalPago.ToString("C2"), "Validos: "+totalValidor, "Nulos: "+totalNulos }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        }
                        else
                        {
                            WriteTextColumn(g, new List<string> { "Ningun ticket suyo salio ganador"}, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        }
                        break;
                }

            }
            positionWrite = 0;
            g.Dispose();

        }
        private static void TemplateListString(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            positionWrite = 50;

            foreach (var item in Value as List<string>)
            {
                WriteText(g, item);
            }


        }

        private static void TemplateListArrayString(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            positionWrite = 30;

            foreach (var item in Value as List<string[]>)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    var Data = item[i];
                    WriteText(g, Data);
                }
               
            }


        }
        private static void TemplateString(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = GetFontStyle(20, "regular");
            RectangleF rect = new RectangleF(0, 20, paperSize.Width, paperSize.Height);
            SolidBrush sb = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat();
            sf.Alignment = GetAlignmentText("center");
            g.DrawString(Value.ToString(), font, sb, rect, sf);
        }

        private static void WriteJugadas(Graphics graphics, List<TicketJugadas> jugadas, int fontSize = 12, string fontStyle = "regular", string alignment = "center")
        {
            foreach (var y in jugadas)
            {
                //y.NombreResumidoLoteria = y.NombreResumidoLoteria.Substring(2);

                //if (y.NombreResumidoLoteria.Length > 5)
                //{
                //    y.NombreResumidoLoteria = y.NombreResumidoLoteria.Substring(0, 8);
                //}
                WriteTextColumn(graphics, new List<string> { y.Jugada.Numeros, "$" + y.Jugada.Monto }, fontSize, fontStyle, alignment);
            }
        }

        //METHODS GENERIC
        private static bool HasProperty(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName) != null;
        }
        private static object GetValueForProperty(this object obj, string propertyName)
        {
            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(propertyName);
            if (pi != null)
            {
                return pi.GetValue(obj, null);
            }

            return string.Empty;

        }
        private static object GetNameProperty(this object obj, string propertyName)
        {
            System.Reflection.PropertyInfo pi = obj.GetType().GetProperty(propertyName);
            if (pi != null)
            {
                return pi.Name;
            }

            return string.Empty;

        }
        private static void WriteText(Graphics graphics, string data, int fontSize = 10, string fontStyle = "regular", string alignment = "center")
        {
            if (data != null && data != string.Empty)
            {
                if (data.Contains("LISTA DE PREMIOS")) { alignment = "left"; }//Alineamiento a la izquierda para reporte de listado de premios
                Font font = GetFontStyle(fontSize, fontStyle);
                // Measure string.
                SizeF stringSize = new SizeF();
                
                stringSize = graphics.MeasureString(data, font, paperSize.Width);
                RectangleF rect = new RectangleF(0, positionWrite, paperSize.Width, Convert.ToInt32(stringSize.Height));
                //RectangleF rect = new RectangleF(0, positionWrite, paperSize.Width, 100);
                SolidBrush sb = new SolidBrush(Color.Black);
                StringFormat sf = new StringFormat();
                sf.Alignment = GetAlignmentText(alignment);
                graphics.DrawString(data, font, sb, rect, sf);
                positionWrite += Convert.ToInt32(rect.Height);
            }

        }
        private static void WriteTextColumn(Graphics graphics, List<string> data, int fontSize = 11, string fontStyle = "regular", string alignment = "center")
        {
            if (data.Count != 0)
            {
                int spaceWidth = 0;
                var totalHeight = new List<int>();
                Font font = GetFontStyle(fontSize, fontStyle);
                // Measure string.
                SizeF stringSize = new SizeF();

                SolidBrush sb = new SolidBrush(Color.Black);
                StringFormat sf = new StringFormat();
                sf.Alignment = GetAlignmentText(alignment);


                foreach (var item in data)
                {
                    stringSize = graphics.MeasureString(item, font, (paperSize.Width / data.Count));
                    RectangleF rect = new RectangleF(spaceWidth, positionWrite, (paperSize.Width / data.Count), Convert.ToInt32(stringSize.Height));
                    graphics.DrawString(item, font, sb, rect, sf);
                    spaceWidth += (paperSize.Width / data.Count);
                    totalHeight.Add(Convert.ToInt32(rect.Height));
                }
                positionWrite += totalHeight.Max();
            }

        }
        private static void WriteLine(Graphics graphics)
        {
            var pen = new Pen(Color.Black, 2);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            positionWrite += 10;
            graphics.DrawLine(pen, 0, positionWrite, paperSize.Width, positionWrite);
            positionWrite += 10;
        }
        private static void WriteLineFinas(Graphics graphics)
        {
            var pen = new Pen(Color.Black, 1);
            pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            positionWrite += 10;
            graphics.DrawLine(pen, 0, positionWrite, paperSize.Width, positionWrite);
            positionWrite += 10;
        }
        private static void WriteCode(Graphics graphics, string data, int size, TypeGenerateCode typeGenerate = TypeGenerateCode.BarCode)
        {
            //RectangleF rectImg = new RectangleF(((paperSize.Width - size) / 2), positionWrite, size, size);
            positionWrite += 10;
            switch (typeGenerate)
            {
                case TypeGenerateCode.QrCode:
                    RectangleF rectImgQR = new RectangleF(((paperSize.Width - size) / 2), positionWrite, size, size);
                    graphics.DrawImage(BarAndQrCode.GetQrCode(data), rectImgQR);
                    positionWrite += (size + 10);
                    break;
                case TypeGenerateCode.BarCode:
                    RectangleF rectImgBar = new RectangleF(5, positionWrite, (paperSize.Width - 10), (size / 2));
                    graphics.DrawImage(BarAndQrCode.GetBarCode(data), rectImgBar);
                    positionWrite += (size / 2) + 20;
                    break;
            }


        }
        private static void WriteImage(Graphics graphics, string url, int size)
        {
            if (url != null && url != string.Empty)
            {
                try
                {
                    var imagen = Image.FromFile(url);
                    int newsize = size < 50 ? 50 : size;
                    var imagennew = ResizeImage(newsize, imagen);



                    RectangleF rectImg = new RectangleF(((paperSize.Width - imagennew.Width) / 2), positionWrite, imagennew.Width, imagennew.Height);
                    graphics.DrawImage(imagen, rectImg);
                    positionWrite += (Convert.ToInt32(imagennew.Height) + 20);
                }
                catch (Exception)
                {


                }

            }

        }

        private static string GetValueCodeString(List<ConfigPrinterValue> configs, TypeGenerateCode typeGenerate)
        {
            List<string> result = new List<string>();
            switch (typeGenerate)
            {
                case TypeGenerateCode.QrCode:
                    var dataQR = configs.Where(x => x.Key == "BANCA_QR").FirstOrDefault();
                    foreach (var item in dataQR.Value)
                    {
                        var value = GetValueForProperty(Value, item.Content);
                        if (value is List<LoteriaTicketPin>)
                        {
                            //foreach (var data in value as List<LoteriaTicketPin>)
                            //{
                            //    result.Add("{Ticket:" + data.Ticket+ ", Pin:" + data.Pin + "}");
                            //}
                            var data = value as List<LoteriaTicketPin>;
                            if (data.Count == 1)
                            {
                                var first = data.First();
                                result.Add("{Ticket:" + first.Ticket + ", Pin:" + first.Pin + "}");
                            }
                            else
                            {
                                var first = data.First();
                                var last = data.Last();
                                result.Add("{Ticket:" + first.Ticket + ", Pin:" + first.Pin + "}");
                                result.Add("{Ticket:" + last.Ticket + ", Pin:" + last.Pin + "}");
                            }

                        }
                        else
                        {
                            result.Add("{" + item.Content + ":" + value + "}");
                        }
                    }
                    break;
                case TypeGenerateCode.BarCode:
                    var dataBC = configs.Where(x => x.Key == "BANCA_BarCode").FirstOrDefault();
                    //foreach (var item in dataBC.Value)
                    //{
                    //    string value = item.Content + " : " + GetValueForProperty(Value, item.Content);
                    //    result += value;
                    //}
                    foreach (var item in dataBC.Value)
                    {
                        var value = GetValueForProperty(Value, item.Content);
                        if (value is List<LoteriaTicketPin>)
                        {
                            //foreach (var data in value as List<LoteriaTicketPin>)
                            //{
                            //    result.Add("{Loteria:" + data.Loteria + ", Ticket:" + data.Ticket + ", Pin:" + data.Pin + "}");
                            //}
                            var data = value as List<LoteriaTicketPin>;
                            if (data.Count == 1)
                            {
                                var first = data.First();
                                result.Add("{Ticket:" + first.Ticket + ", Pin:" + first.Pin + "}");
                            }
                            else
                            {
                                var first = data.First();
                                var last = data.Last();
                                result.Add("{Ticket:" + first.Ticket + ", Pin:" + first.Pin + "}");
                                result.Add("{Ticket:" + last.Ticket + ", Pin:" + last.Pin + "}");
                            }
                        }
                        else
                        {
                            result.Add("{" + item.Content + ":" + value + "}");
                        }
                    }
                    break;
            }

            var DataFinal = JsonConvert.SerializeObject(result);
            return DataFinal;
        }

        private static Image ResizeImage(int newSize, Image originalImage)
        {
            if (originalImage.Width <= newSize)
                newSize = originalImage.Width;

            var newHeight = originalImage.Height * newSize / originalImage.Width;

            if (newHeight > newSize)
            {
                // Resize with height instead
                newSize = originalImage.Width * newSize / originalImage.Height;
                newHeight = newSize;
            }

            return originalImage.GetThumbnailImage(newSize, newHeight, null, IntPtr.Zero);
        }
        private static StringAlignment GetAlignmentText(string alignment)
        {
            switch (alignment)
            {
                case "center":
                    return StringAlignment.Center;
                    break;
                case "left":
                    return StringAlignment.Near;
                    break;
                case "right":
                    return StringAlignment.Far;
                    break;
                default:
                    return StringAlignment.Center;
                    break;
            }

        }
        private static Font GetFontStyle(int fontSize, string fontStyle)
        {
            //   string nameFont = "Courier New";
            string nameFont = "Arial";

            var font = FontFamily.GenericSansSerif;

            switch (fontStyle)
            {
                case "bold":
                    return new Font(nameFont, fontSize, System.Drawing.FontStyle.Bold);
                    break;
                case "regular":
                    return new Font(nameFont, fontSize, System.Drawing.FontStyle.Regular);
                    break;
                case "italic":
                    return new Font(nameFont, fontSize, System.Drawing.FontStyle.Italic);
                    break;
                default:
                    //return new Font(FontFamily.GenericMonospace, 8, System.Drawing.FontStyle.Regular);
                    return new Font(nameFont, 8, System.Drawing.FontStyle.Regular);
                    break;

            }


        }


    }
}
