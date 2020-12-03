using System;
using System.Collections.Generic;
using System.Text;
using ClienteMarWPF.Domain.Models.Dtos;
using Newtonsoft.Json;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace ClienteMarWPF.UI.State.PinterConfig
{
    public static class TicketTemplateHelper
    {
        private static List<ConfigPrinterModel> ConfigData = new List<ConfigPrinterModel>();
        //private static List<ConfigPrinterModel> ConfigData = BancaLogic.GetBancaConfig(7);
        private static PrinterSettings ps = new PrinterSettings();
        private static PaperSize paperSize = new PaperSize();
        private static int positionWrite = 0;
        private static object Value;

        public static void PrintTicket(object value, List<ConfigPrinterModel> configs = null)
        {
            var papers = ps.PaperSizes.Cast<PaperSize>();
            PrintDocument pd = new PrintDocument();
            StringBuilder sb = new StringBuilder();
            configs = configs ?? new List<ConfigPrinterModel>();
            Value = value;
            //paperSize = papers.FirstOrDefault();
            paperSize = new PaperSize("nose", 300, 0);

            if (value is string)
            {
                pd.PrintPage += TemplateString;
            }
            else if (value is TicketValue && configs.Any())
            {
                ConfigData = configs;
                pd.PrintPage += TemplateTicket;
            }
            else if (value is List<string>)
            {
                pd.PrintPage += TemplateListString;
            }

            pd.PrintController = new StandardPrintController();
            pd.DefaultPageSettings.Margins.Left = 5;
            pd.DefaultPageSettings.Margins.Right = 5;
            pd.DefaultPageSettings.Margins.Top = 5;
            pd.DefaultPageSettings.Margins.Bottom = 5;
            pd.DefaultPageSettings.PaperSize = paperSize;
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

            var ValueTicket = JsonConvert.DeserializeObject<List<ConfigPrinterValue>>(GetConfigTicket.ConfigValue);
            var ValueIMG = JsonConvert.DeserializeObject<List<ConfigPrinterValue>>(GetConfigIMG.ConfigValue);
            //var code = JsonConvert.DeserializeObject<ConfigPrinterValueCode>(ValueIMG.FirstOrDefault().Value);
            positionWrite = 0;

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
                    case "Total":

                        string Total = string.Empty;
                        if (data != null && data.ToString() != string.Empty)
                        {
                            Total = "Total: $" + data.ToString();
                        }
                        WriteText(g, Total, item.Size, item.FontStyle, item.Aligment);
                        break;
                    case "Jugadas":
                        WriteTextColumn(g, new List<string> { "JUGADAS", "MONTO" }, item.Size, item.FontStyle, item.Aligment);

                        var TicketJugadas = data as List<TicketJugadas>;
                        var Quiniela = TicketJugadas.Where(y => y.TipoJudaga == "Quiniela").ToList();
                        var Pales = TicketJugadas.Where(y => y.TipoJudaga == "Pale").ToList();
                        var Tripleta = TicketJugadas.Where(y => y.TipoJudaga == "Tripleta").ToList();

                        if (true)
                        {

                        }
                        WriteText(g, "----Quiniela----", item.Size, item.FontStyle, item.Aligment);
                        WriteJugadas(g, Quiniela, item.Size, item.FontStyle, item.Aligment);

                        WriteText(g, "----Pale----", item.Size, item.FontStyle, item.Aligment);
                        WriteJugadas(g, Pales, item.Size, item.FontStyle, item.Aligment);

                        WriteText(g, "----Tripleta----", item.Size, item.FontStyle, item.Aligment);
                        WriteJugadas(g, Tripleta, item.Size, item.FontStyle, item.Aligment);
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
                                            WriteTextColumn(g, new List<string> { dataMore.Loteria, dataMore.Ticket, dataMore.Pin }, item.Size, item.FontStyle, item.Aligment);
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

                                switch (item.FormatOneSorteo)
                                {

                                    case 1:

                                        var FormatOne = loteriaTicketPin.FirstOrDefault();
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
        private static void TemplateListString(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            positionWrite = 50;

            foreach (var item in Value as List<string>)
            {
                WriteText(g, item);
            }


        }
        private static void TemplateString(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            Font font = GetFontStyle(10, "regular");
            RectangleF rect = new RectangleF(0, 20, paperSize.Width, paperSize.Height);
            SolidBrush sb = new SolidBrush(Color.Black);
            StringFormat sf = new StringFormat();
            sf.Alignment = GetAlignmentText("center");
            g.DrawString(Value.ToString(), font, sb, rect, sf);
        }

        private static void WriteJugadas(Graphics graphics, List<TicketJugadas> jugadas, int fontSize = 8, string fontStyle = "regular", string alignment = "center")
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
        private static void WriteText(Graphics graphics, string data, int fontSize = 8, string fontStyle = "regular", string alignment = "center")
        {
            if (data != null && data != string.Empty)
            {

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
        private static void WriteTextColumn(Graphics graphics, List<string> data, int fontSize = 8, string fontStyle = "regular", string alignment = "center")
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
            string nameFont = "Courier New";

            var font = FontFamily.GenericMonospace;

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
