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

namespace ClienteMarWPFWin7.UI.State.PinterConfig
{
    public static class TicketTemplateHelper
    {
        private static List<ConfigPrinterModel> ConfigData = new List<ConfigPrinterModel>();
        //private static List<ConfigPrinterModel> ConfigData = BancaLogic.GetBancaConfig(7);
        private static PrinterSettings ps = new PrinterSettings();
        private static PaperSize paperSize = new PaperSize();
        private static int positionWrite = 0;
        private static object Value;
        private static bool ImprimirCopias;
        private static int CantidadLoterias;
        private static int WidthPaper=0;

        public static void PrintTicket(object value ,List<ConfigPrinterModel> configs = null,bool ImprimirCopia=false,int cantidadLoterias=1)
        {
            var papers = ps.PaperSizes.Cast<PaperSize>();
            PrintDocument pd = new PrintDocument();
            StringBuilder sb = new StringBuilder();
            configs = configs ?? new List<ConfigPrinterModel>();
            Value = value;
            ImprimirCopias = ImprimirCopia;
            CantidadLoterias = cantidadLoterias;
            
            var width = pd.PrinterSettings.DefaultPageSettings.PaperSize.Width;
            WidthPaper = width;
            
            //paperSize = papers.FirstOrDefault();
             paperSize = new PaperSize("nose",width, 0);
            //WidthPaper = width;

            if (value is string)
            {
                pd.PrintPage += TemplateString;
            }
            else if (value is TicketValue && configs.Any())
            {
                ConfigData = configs;
                pd.PrintPage += TemplateTicket;
            }
            else if (value is SorteosTicketModels)
            {
               
                pd.PrintPage += TemplateTicketSinPrinterConfig;
            }
            else if (value is List<string>)
            {
                pd.PrintPage += TemplateListString;
            }
            else if (value is List<string[]>)
            {
                pd.PrintPage += TemplateListArrayString;
            }
            else if (value is List<string[,]>)
            {
                pd.PrintPage += TemplateListBiArrayString;
            }

            pd.PrintController = new StandardPrintController();
            
            pd.DefaultPageSettings.PaperSize = pd.DefaultPageSettings.PaperSize;
            //pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            pd.DefaultPageSettings.Margins = new Margins(pd.DefaultPageSettings.Margins.Left, pd.DefaultPageSettings.Margins.Top, pd.DefaultPageSettings.Margins.Right, pd.DefaultPageSettings.Margins.Bottom);
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
            var soloText = ConfigData.Where(x => x.ConfigKey == "BANCA_PRINTER_TEXT_ONLY").FirstOrDefault();
            var ValueTicket = JsonConvert.DeserializeObject<List<ConfigPrinterValue>>(GetConfigTicket.ConfigValue);
            ValueTicket.RemoveAt(3);
            ValueTicket.Insert(4,new ConfigPrinterValue { Key = "FechaActual", Size = 11, Aligment = "Center", FontStyle = "regular",Content="FechaActual" });
            
            //var ValueIMG = JsonConvert.DeserializeObject<List<ConfigPrinterValue>>(GetConfigIMG.ConfigValue);
            //var code = JsonConvert.DeserializeObject<ConfigPrinterValueCode>(ValueIMG.FirstOrDefault().Value);
            positionWrite = 5;

            //var TicketJugadas = ValueTicket.Where(x => x.Content == "Jugadas");
            //if (true)
            //{

            //}
            if (soloText != null)
            {
                if (soloText.ConfigValue == "TRUE")
                {
                    ValueTicket = JsonConvert.DeserializeObject<List<ConfigPrinterValue>>(GetConfigTicket.ConfigValue);
                    foreach (var item in ValueTicket)
                    {
                        var data = GetValueForProperty(Value, item.Content) == null ? "" : GetValueForProperty(Value, item.Content);

                        switch (item.Content)
                        {
                            case "Logo":
                                WriteImage(g, data.ToString(), item.Size);
                                break;
                            /*case "Qrcode":
                                WriteCode(g, GetValueCodeString(ValueIMG, TypeGenerateCode.QrCode), item.Size, TypeGenerateCode.QrCode);
                                break;
                            case "Barcode":
                                WriteCode(g, GetValueCodeString(ValueIMG, TypeGenerateCode.BarCode), item.Size, TypeGenerateCode.BarCode);
                                break;*/
                            case "Texto":
                                var json_data = JsonConvert.DeserializeObject<List<string[]>>(data.ToString());
                                if (json_data != null)
                                {

                                    for (int i = 0; i < json_data.Count; i++)
                                    {
                                        int size = item.Size;
                                        if (json_data[i].Length > 1)  // pregunto si la propiedad a imprimir tiene tamano en la posision 1 del array
                                        {
                                            size = item.Size + int.Parse(json_data[i][1]);
                                        }
                                        WriteText(g, json_data[i][0], size, item.FontStyle, item.Aligment);
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            else
            {
                foreach (var item in ValueTicket)
                {
                    var data = GetValueForProperty(Value, item.Content) == null ? "" : GetValueForProperty(Value, item.Content);

                    switch (item.Content)
                    {
                        case "Logo":
                            WriteImage(g, data.ToString(), item.Size);
                            break;
                        case "BanNombre":

                            if (data.ToString().Length > 0)
                            {
                                WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                            }
                            break;
                        //case "Loteria":
                        //    WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                        //    break;
                        case "Direccion":
                            if (data.ToString().Length > 0)
                            {
                                WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                            }

                            break;
                        case "FechaActual":
                            if (data.ToString().Length > 0)
                            {
                                string FechaActual = Convert.ToDateTime(data).ToString("dd/MMM/yyyy  hh:mm tt");
                                if (data != null && data.ToString() != string.Empty)
                                {
                                    FechaActual = Convert.ToDateTime(data).ToString("dd-MM-yyyy hh:mm tt");
                                }
                                WriteText(g, FechaActual, item.Size, item.FontStyle, item.Aligment);
                            }

                            break;
                        case "Telefono":

                            string Telefono = string.Empty;
                            if (data != null && data.ToString() != string.Empty)
                            {
                                Telefono = "Telefono: " + data.ToString();
                            }
                            WriteText(g, Telefono, item.Size, item.FontStyle, item.Aligment);
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
                        /*case "Qrcode":
                            WriteCode(g, GetValueCodeString(ValueIMG, TypeGenerateCode.QrCode), item.Size, TypeGenerateCode.QrCode);
                            break;
                        case "Barcode":
                            WriteCode(g, GetValueCodeString(ValueIMG, TypeGenerateCode.BarCode), item.Size, TypeGenerateCode.BarCode);

                            break;*/
                        case "Total":

                            string Total = string.Empty;
                            if (data != null && data.ToString() != string.Empty)
                            {
                                Total = "Total: $" + data.ToString();
                            }
                            WriteText(g, Total, item.Size, item.FontStyle, item.Aligment);
                            break;
                        case "Jugadas":
                            var TicketJugadas = data as List<TicketJugadas>;

                            if (TicketJugadas.Any())
                            {
                                WriteTextColumn(g, new List<string> { "JUGADAS", "MONTO" }, item.Size, item.FontStyle, item.Aligment);

                                var Quiniela = TicketJugadas.Where(y => y.TipoJudaga == "Q").ToList();
                                var Pales = TicketJugadas.Where(y => y.TipoJudaga == "P").ToList();
                                var Tripleta = TicketJugadas.Where(y => y.TipoJudaga == "T").ToList();
                                if (Quiniela.Any())
                                {
                                    WriteText(g, "----Quiniela----", item.Size, item.FontStyle, item.Aligment);
                                    WriteJugadas(g, Quiniela, item.Size, item.FontStyle, item.Aligment);
                                }

                                if (Pales.Any())
                                {
                                    WriteText(g, "----Pale----", item.Size, item.FontStyle, item.Aligment);
                                    WriteJugadas(g, Pales, item.Size, item.FontStyle, item.Aligment);
                                }
                                if (Tripleta.Any())
                                {
                                    WriteText(g, "----Tripleta----", item.Size, item.FontStyle, item.Aligment);
                                    WriteJugadas(g, Tripleta, item.Size, item.FontStyle, item.Aligment);
                                }
                            }

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
                                            WriteTextColumn(g, new List<string> { "Loteria", "Ticket", "Pin" }, item.Size, item.FontStyle, item.Aligment);
                                            foreach (var dataMore in loteriaTicketPin)
                                            {
                                                WriteTextColumn(g, new List<string> { dataMore.Loteria, dataMore.Ticket, dataMore.Pin }, item.Size, item.FontStyle, item.Aligment);
                                            }
                                            break;

                                        case 2:
                                            WriteTextColumn(g, new List<string> { "Loteria", "Ticket", "Pin" }, item.Size, item.FontStyle, item.Aligment);
                                            foreach (var dataMore in loteriaTicketPin)
                                            {
                                                WriteTextColumn(g, new List<string> { dataMore.Loteria, dataMore.Ticket, dataMore.Pin }, item.Size, item.FontStyle, item.Aligment);
                                            }
                                            break;

                                        default:
                                            WriteTextColumn(g, new List<string> { "Loteria", "Ticket", "Pin" }, item.Size, item.FontStyle, item.Aligment);
                                            foreach (var dataMore in loteriaTicketPin)
                                            {
                                                WriteTextColumn(g, new List<string> { dataMore.Loteria, dataMore.Ticket, dataMore.Pin }, item.Size, item.FontStyle, item.Aligment);
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

                                    switch (item.FormatOneSorteo)
                                    {

                                        case 1:

                                            var FormatOne = loteriaTicketPin.FirstOrDefault();
                                            WriteText(g, "L:" + FormatOne.Loteria, item.Size, item.FontStyle, item.Aligment);
                                            WriteTextColumn(g, new List<string> { "T:" + FormatOne.Ticket, "P:" + FormatOne.Pin }, item.Size, item.FontStyle, item.Aligment);
                                            
                                            break;

                                        case 2:
                                            var FormatTwo = loteriaTicketPin.FirstOrDefault();
                                            WriteText(g, "L:" + FormatTwo.Loteria, item.Size, item.FontStyle, item.Aligment);
                                            WriteTextColumn(g, new List<string> { "T:" + FormatTwo.Ticket, "P:" + FormatTwo.Pin }, item.Size, item.FontStyle, item.Aligment);
                                            break;

                                        default:
                                            var defaults = loteriaTicketPin.FirstOrDefault();
                                            WriteText(g, "L:" + defaults.Loteria, item.Size, item.FontStyle, item.Aligment);
                                            WriteTextColumn(g, new List<string> { "T:" + defaults.Ticket, "P:" + defaults.Pin }, item.Size, item.FontStyle, item.Aligment);
                                            
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
            }
            positionWrite = 0;
            g.Dispose();
        }

        private static void TemplateTicketSinPrinterConfig(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string Alignament = "center";

            Console.WriteLine(Value);
            var Valor = Value as SorteosTicketModels;
            List<ConfigPrinterModel> valores = new  List<ConfigPrinterModel>() { };
            
            ConfigPrinterModel nombreBanca = new ConfigPrinterModel() { ConfigKey = "BanNombre", ConfigValue = Valor.BanNombre };
            ConfigPrinterModel direccionBanca = new ConfigPrinterModel() { ConfigKey = "Direccion", ConfigValue = Valor.BanDireccion };
            ConfigPrinterModel fecha = new ConfigPrinterModel() { ConfigKey = "FechaActual", ConfigValue = Valor.Fecha };
            ConfigPrinterModel telefono = new ConfigPrinterModel() { ConfigKey = "Telefono", ConfigValue = Valor.Telefono };
            ConfigPrinterModel Linea = new ConfigPrinterModel() { ConfigKey = "Linea", ConfigValue = "" };
            ConfigPrinterModel loteriaticket = new ConfigPrinterModel() { ConfigKey = "LoteriaTicketPin", ConfigValue = "" };
            
            ConfigPrinterModel jugadass = new ConfigPrinterModel() { ConfigKey = "Jugadas", ConfigValue = "" };
            ConfigPrinterModel textoRevise = new ConfigPrinterModel() { ConfigKey = "Texto", ConfigValue = Valor.TextReviseJugada };
            ConfigPrinterModel firma = new ConfigPrinterModel() { ConfigKey = "Firma", ConfigValue = Valor.Firma };
            ConfigPrinterModel total = new ConfigPrinterModel() { ConfigKey = "Total", ConfigValue = "Total" };

            valores.Add(nombreBanca);
            valores.Add(direccionBanca);
            valores.Add(telefono);
            valores.Add(Linea);
            valores.Add(loteriaticket);
            valores.Add(Linea);
            valores.Add(jugadass);
            valores.Add(Linea);

            valores.Add(total);
            
            valores.Add(firma);
            valores.Add(textoRevise);
            

            positionWrite = 0;
            var TotalGenerales = 0;
            
            foreach (var item in valores)
            {
                var data = GetValueForProperty(Value, item.ConfigKey) == null ? "" : GetValueForProperty(Value, item.ConfigValue);
                

                switch (item.ConfigKey)
                {
                    case "Logo":
                        WriteImage(g, data.ToString(), 12);
                        break;
                    case "BanNombre":
                        WriteText(g, Valor.BanNombre, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        break;
                    //case "Loteria":
                    //    WriteText(g, data.ToString(), item.Size, item.FontStyle, item.Aligment);
                    //    break;
                    case "Direccion":
                        WriteText(g, Valor.BanDireccion,FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        break;
                    case "FechaActual":

                        string FechaActual = string.Empty;
                        if (data != null && data.ToString() != string.Empty)
                        {
                            FechaActual = "" +Valor.Fecha;
                        }
                        WriteText(g, Valor.Fecha,FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        break;
                    case "Telefono":

                        string Telefono = string.Empty;
                        
                        Telefono = "Telefono: " + Valor.Telefono;
                        WriteText(g, Telefono, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                       
                       
                        WriteText(g, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        
                        break;
                    case "Firma":

                        string Firma = string.Empty;
                        if (Valor.Firma != null && Valor.Firma.ToString() != string.Empty)
                        {
                            Firma = "Firma: " + Valor.Firma;
                        }
                        WriteText(g, Firma, FontSize-1, FontStyle.Regular.ToString().ToLower(), Alignament);
                        break;
                    case "AutorizacionHacienda":

                        string AutorizacionHacienda = string.Empty;
                        if (data != null && data.ToString() != string.Empty)
                        {
                            AutorizacionHacienda = "Autorizacion Hacienda:" + data.ToString();
                        }
                        WriteText(g, AutorizacionHacienda, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        break;
                    //case "Ticket":

                    //string Ticket = string.Empty;
                    //if (data != null && data.ToString() != string.Empty)
                    //{
                    //    Ticket = "Ticket: " + data.ToString();
                    //}
                    //WriteText(g, Ticket, item.Size, item.FontStyle, item.Aligment);
                    //    break;
                    

                    case "Jugadas":
                        WriteTextColumn(g, new List<string> { "JUGADAS", "MONTO" }, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        List<TicketJugadas> jugadas = new List<TicketJugadas>() { };
                        foreach(var jugada in Valor.Jugadas)
                        {
                            JugadaPrinter jugadaPrint = new JugadaPrinter() { Numeros = jugada.Numero, Monto = jugada.Costo };
                            TicketJugadas jugadp = new TicketJugadas() { Jugada = jugadaPrint, TipoJudaga = jugada.TipoJugada };
                            jugadas.Add(jugadp);
                        }
                        var TicketJugadas =jugadas;

                        var Quiniela = TicketJugadas.Where(y => y.TipoJudaga == "Q").ToList();
                        var Pales = TicketJugadas.Where(y => y.TipoJudaga == "P").ToList();
                        var Tripleta = TicketJugadas.Where(y => y.TipoJudaga == "T").ToList();
                        TotalGenerales = TicketJugadas.Sum(x => x.Jugada.Monto);
                        TotalGenerales = TotalGenerales * CantidadLoterias;
                        for (var i = 0; i <Pales.Count; i++){Pales[i].Jugada.Numeros= Pales[i].Jugada.Numeros.Insert(2, "-"); }
                        for (var i = 0; i < Tripleta.Count; i++) { Tripleta[i].Jugada.Numeros = Tripleta[i].Jugada.Numeros.Insert(2, "-"); Tripleta[i].Jugada.Numeros = Tripleta[i].Jugada.Numeros.Insert(5, "-"); }

                        if (true)
                        {

                        }
                        if (Quiniela.Count() > 0)
                        {
                            WriteText(g, "----Quinielas----",FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                            WriteJugadas(g, Quiniela, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        }

                        if (Pales.Count() > 0)
                        {
                            WriteText(g, "----Pales----", FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                            WriteJugadas(g, Pales, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        }

                        if (Tripleta.Count() > 0)
                        {
                            WriteText(g, "----Tripletas----", FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                            WriteJugadas(g, Tripleta, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                        }
                        break;
                    case "Total":

                        string Total = string.Empty;

                        Total = "Total: " + string.Format(nfi, "{0:C}", TotalGenerales);
                        WriteText(g, Total, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
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
                            var loteriaTicketPin = Valor.Pin as List<LoteriaTicketPin>;
                            if (loteriaTicketPin.Count > 1)
                            {
                                WriteTextColumn(g, new List<string> { "Loteria" ,"Ticket", "Pin" }, 10, FontStyle.Regular.ToString().ToLower(), Alignament);
                                foreach (var dataMore in loteriaTicketPin)
                                {
                                    WriteTextColumn(g, new List<string> { dataMore.Loteria, dataMore.Ticket, dataMore.Pin }, 9, FontStyle.Regular.ToString().ToLower(), Alignament);
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

                                var FormatOne = loteriaTicketPin.FirstOrDefault();
                                WriteText(g, mensajeCopiar, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                                WriteText(g, "L: " + FormatOne.Loteria, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                                WriteText(g, "T: " + FormatOne.Ticket,FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                                WriteText(g, "P: " + FormatOne.Pin, FontSize, FontStyle.Regular.ToString().ToLower(), Alignament);
                            }

                        }

                        break;
                    case "Texto":
                        WriteText(g, Valor.TextReviseJugada, (FontSize-2), FontStyle.Regular.ToString().ToLower(), Alignament);
                        WriteText(g, ".", (FontSize - 2), FontStyle.Regular.ToString().ToLower(), Alignament);
                        break;
                    case "Linea":
                        WriteLineFinas(g);
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
                WriteTextColumn(graphics, new List<string> { y.Jugada.Numeros, "$" + y.Jugada.Monto.ToString("N2") }, fontSize, fontStyle, alignment);
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
        private static void TemplateListBiArrayString(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            positionWrite = 30;

            foreach (var item in Value as List<string[,]>)
            {
                WriteTextColumnBiArray(g, item);
            }
        }

        private static void WriteTextColumnBiArray(Graphics graphics, string[,] data, int fontSize = 11, string fontStyle = "regular", string alignment = "center")
        {

            int spaceWidth = 0;
            var totalHeight = new List<int>();
            Font font = GetFontStyle(fontSize, fontStyle);
            // Measure string.
            SizeF stringSize = new SizeF();

            SolidBrush sb = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat();
            sf.Alignment = GetAlignmentText(alignment);


            for (int x = 0; x < data.GetLength(0); x++)
            {

                for (int y = 0; y < data.GetLength(1); y++)
                {
                    stringSize = graphics.MeasureString(Justify(data[x, y]," ",60).ToString(), font, (paperSize.Width / data.GetLength(1)));
                    RectangleF rect = new RectangleF(spaceWidth, positionWrite, (WidthPaper / data.GetLength(1)), Convert.ToInt32(stringSize.Height));
                    graphics.DrawString(data[x, y], font, sb, rect, sf);
                    spaceWidth += (WidthPaper / data.GetLength(1));
                    totalHeight.Add(Convert.ToInt32(rect.Height));
                }
                positionWrite += totalHeight.Max();
            }

        }


    }
}
