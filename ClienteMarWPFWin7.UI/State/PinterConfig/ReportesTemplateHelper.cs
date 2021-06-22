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
using ClienteMarWPFWin7.UI.Modules.Sorteos;

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
        private static ReporteViewModel ViewModel;


        public static void PrintReporte(object value, IAuthenticator autenticador, ReporteViewModel viewModel, Boolean TicketGanadores = false, Boolean PagosRemotos = false, Boolean Totales = false)
        {
            ViewModel = viewModel;
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
            else if (value is MAR_RptVenta)
            {
                pd.PrintPage += TemplateRPTVentas;
            }
            else if (value is MAR_VentaNumero)
            {
                pd.PrintPage += TemplateRPTListNumeros;
            }

            else if (value is MAR_Ganadores && TicketGanadores == false)
            {
                pd.PrintPage += TemplateRPTListTickets;
            }
            else if (value is MAR_RptSumaVta2)
            {
                if (Totales == false)
                {
                    pd.PrintPage += TemplateRPTVentasFechaNoResumido;
                }
                if (Totales == true)
                {
                    pd.PrintPage += TemplateRPTVentasFechaResumido;
                }
            }
            else if (value is MAR_RptSumaVta)
            {
                pd.PrintPage += TemplateRPTSumaVentas;
            }
            else if (value is MAR_Ganadores && TicketGanadores == true)
            {
                pd.PrintPage += TemplateRPTTicketsGanadores;
            }
            else if (value is MAR_Pines)
            {
                pd.PrintPage += TemplateRPTListTarjeta;
            }
            else if (value is MAR_Ganadores && PagosRemotos == true)
            {
                pd.PrintPage += TemplateRPTPagosRemotosTickets;
            }
            pd.PrintController = new StandardPrintController();
            pd.DefaultPageSettings.PaperSize = paperSize;
            pd.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            pd.Print();

        }

        private static string ConversionNumeroNegativo(int NumeroConvert)
        {
            string conversion = "";
            if (NumeroConvert < 0)
            {
               conversion = "-"+NumeroConvert.ToString("C2");
            }
            else if (NumeroConvert >= 0){
               conversion = NumeroConvert.ToString("C2");
            }

            return conversion;
        }
        private static string ConvertirMonedaNegativos(int cantidad)
        {
            string AnteComa = "";
            string LuegoComa = "";
            string CantidadConvertido = "";
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            if (cantidad.ToString().Length == 2)
            {
                CantidadConvertido = cantidad.ToString();
            }
            if (cantidad.ToString().Length == 3)
            {
                CantidadConvertido = cantidad.ToString();
            }

            if (cantidad.ToString().Length == 4)
            {

                CantidadConvertido = cantidad.ToString();

            }
            if (cantidad.ToString().Length == 5)
            {
                AnteComa = cantidad.ToString().Substring(0, 2) + ",";
                LuegoComa = cantidad.ToString().Substring(2, (cantidad.ToString().Length) - 2);
                CantidadConvertido = AnteComa + LuegoComa;

            }
            if (cantidad.ToString().Length == 6)
            {
                AnteComa = cantidad.ToString().Substring(0, 3) + ",";
                LuegoComa = cantidad.ToString().Substring(3, cantidad.ToString().Length - 3);
                CantidadConvertido = AnteComa + LuegoComa;

            }
            if (cantidad.ToString().Length == 7)
            {
                var segundaComa = "";
                AnteComa = cantidad.ToString().Substring(0, 4) + ",";
                segundaComa = cantidad.ToString().Substring(4, 3);
                LuegoComa = cantidad.ToString().Substring(7, cantidad.ToString().Length - 7);
                CantidadConvertido = AnteComa + segundaComa;
            }
            if (cantidad.ToString().Length == 8)
            {
                var segundaComa = "";
                AnteComa = cantidad.ToString().Substring(0, 2) + ",";
                segundaComa = cantidad.ToString().Substring(2, 3) + ",";
                LuegoComa = cantidad.ToString().Substring(5, 3);
                CantidadConvertido = AnteComa + segundaComa + LuegoComa;
            }
            if (cantidad.ToString().Length == 9)
            {
                var segundaComa = "";
                AnteComa = cantidad.ToString().Substring(0, 3) + ",";
                segundaComa = cantidad.ToString().Substring(3, 3) + ",";
                LuegoComa = cantidad.ToString().Substring(6, 3);
                CantidadConvertido = AnteComa + segundaComa + LuegoComa;
            }
            if (cantidad.ToString().Length == 10)
            {
                var segundaComa = "";
                AnteComa = cantidad.ToString().Substring(0, 4) + ",";
                segundaComa = cantidad.ToString().Substring(4, 3) + ",";
                LuegoComa = cantidad.ToString().Substring(7, 3);
                CantidadConvertido = AnteComa + segundaComa + LuegoComa;
            }


            return "$" + CantidadConvertido + ".00";
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
            List<ConfigPrinterModel> valores = new List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();


            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel Ventas = new ConfigPrinterModel() { ConfigKey = "Ventas", ConfigValue = "Ventas" };
            ConfigPrinterModel Premios = new ConfigPrinterModel() { ConfigKey = "Premios", ConfigValue = "Premios" };
            ConfigPrinterModel Nulos = new ConfigPrinterModel() { ConfigKey = "Nulos", ConfigValue = "Nulos" };
            ConfigPrinterModel SinNulos = new ConfigPrinterModel() { ConfigKey = "SinNulos", ConfigValue = "SinNulos" };


            valores.Add(Header);
            valores.Add(Ventas);

            if (nombreLoteria != "Todas")
            {
                valores.Add(Premios);
            }

            if (Valor.TicketsNulos.Length > 0)
            {
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
                        WriteText(g, AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre + " ID:" + AUTENTICATION.BancaConfiguracion.BancaDto.BancaID, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "REPORTE DE VENTAS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Del Dia "+FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
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
                        var TotalPremiados = (Convert.ToInt32(Valor.MPrimero + Valor.MSegundo + Valor.MTercero) + Convert.ToInt32(Valor.MPales + Valor.MTripletas)).ToString("C2");

                        WriteTextColumn(g, new List<string> { "Numeros:      ", Valor.Numeros.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Numeros (RD$):", Valor.CntNumeros.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Pales:        ", Valor.Pales.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Tripletas:    ", Valor.Tripletas.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Total Ventas: ", TotalVentas.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Comision (0%):", Valor.Comision.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Ventas Netas: ", VentasNeta.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Numeros Premiados:      ", Convert.ToInt32(Valor.MPrimero + Valor.MSegundo + Valor.MTercero).ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
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
                            WriteTextColumn(g, new List<string> { "Perdida:", ConvertirMonedaNegativos(TotalGanancia) }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        }

                        break;
                    case "Premios":
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "PREMIOS", "CANTIDAD", "GANA" }, FontSize - 2, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "1RA.", Valor.Primero.ToString(), Valor.CPrimero.ToString("C2"), Valor.MPrimero.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "2DA.", Valor.Segundo.ToString(), Valor.CSegundo.ToString("C2"), Valor.MSegundo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "3RA.", Valor.Tercero.ToString(), Valor.CTercero.ToString("C2"), Valor.MTercero.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);


                        break;

                    case "Nulos":
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "Ticket", "Hora", "Vendio" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        foreach (var ticket in Valor.TicketsNulos)
                        {
                            WriteTextColumn(g, new List<string> { ticket.TicketNo, ticket.StrHora, ticket.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        }
                        WriteTextColumn(g, new List<string> { ".", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;

                    case "SinNulos":
                        WriteTextColumn(g, new List<string> { ".", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        WriteTextColumn(g, new List<string> { "--NO HAY TICKETS NULOS--" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { ".", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

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
            int FontSize = 8;
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

            try{
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
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Del Dia "+FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", "", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        break;

                    case "Numeros":
                        WriteTextColumn(g, new List<string> { "===Detalle de numeros vendidos===" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "Num-", "Cant", "Num-", "Cant", "Num-", "Cant" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        if (numerosQuinielas.ToList().Count > 0)
                        {
                                for (int i = 0; i < numerosQuinielas.Count(); i = i + 3){
                                    if (i + 2 <= numerosQuinielas.Count() - 1)
                                    {
                                        var Colunm1 = numerosQuinielas.ToArray()[i];
                                    var Colunm2 = numerosQuinielas.ToArray()[i + 1];
                                    var Colunm3 = numerosQuinielas.ToArray()[i + 2];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), Colunm3.Numero, Colunm3.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;
                                        
                                    continue;
                                }else
                                if (i + 1 <= numerosQuinielas.Count() - 1)
                                {
                                    var Colunm1 = numerosQuinielas.ToArray()[i];
                                    var Colunm2 = numerosQuinielas.ToArray()[i + 1];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;
                                        
                                    continue;
                                }
                                    else
                                if (i <= numerosQuinielas.Count() - 1)
                                {
                                    var Colunm1 = numerosQuinielas.ToArray()[i];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), "", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;
                                        
                                     continue;
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
                            for (int i = 0; i < numerosPales.Count(); i = i + 3)
                            {
                                if (i + 2 <= numerosPales.Count() - 1)
                                {
                                    var Colunm1 = numerosPales.ToArray()[i];
                                    var Colunm2 = numerosPales.ToArray()[i + 1];
                                    var Colunm3 = numerosPales.ToArray()[i + 2];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), Colunm3.Numero, Colunm3.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }else
                                if (i + 1 <= numerosPales.Count() - 1)
                                {
                                    var Colunm1 = numerosPales.ToArray()[i];
                                    var Colunm2 = numerosPales.ToArray()[i + 1];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }else
                                if (i <= numerosPales.Count() - 1)
                                {
                                    var Colunm1 = numerosPales.ToArray()[i];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), "", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }
                            }

                            var TotalPales = numerosPales.Sum(x => x.Costo);
                            WriteTextColumn(g, new List<string> { "---------------------------------------------------" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { "Pales:", " ", TotalPales.ToString("C2"), "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        }
                        if (numerosTripletas.ToList().Count > 0)
                        {

                            WriteTextColumn(g, new List<string> { "====Detalle de tripletas vendidos====" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { "Num-", "Cant", "Num-", "Cant", "Num-", "Cant" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                            for (int i = 0; i < numerosTripletas.Count(); i = i + 3)
                            {
                                if (i + 2 <= numerosTripletas.Count() - 1)
                                {
                                    var Colunm1 = numerosTripletas.ToArray()[i];
                                    var Colunm2 = numerosTripletas.ToArray()[i + 1];
                                    var Colunm3 = numerosTripletas.ToArray()[i + 2];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), Colunm3.Numero, Colunm3.Costo.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }else
                                if (i + 1 <= numerosTripletas.Count() - 1)
                                {
                                    var Colunm1 = numerosTripletas.ToArray()[i];
                                    var Colunm2 = numerosTripletas.ToArray()[i + 1];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), Colunm2.Numero, Colunm2.Costo.ToString("C2"), "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }else
                                if (i <= numerosTripletas.Count() - 1)
                                {
                                    var Colunm1 = numerosTripletas.ToArray()[i];
                                    WriteTextColumn(g, new List<string> { Colunm1.Numero, Colunm1.Costo.ToString("C2"), "", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft); ;

                                }
                            }
                            var TotalTripletas = numerosTripletas.Sum(x => x.Costo);
                            WriteTextColumn(g, new List<string> { "----------------------------------------------" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { "Tripletas:", " ", TotalTripletas.ToString("C2"), "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        }
                        WriteTextColumn(g, new List<string> { "." }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;
                    }

                } 
            }
            catch (Exception E)
            {
                Console.WriteLine(E.Message);
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

            var totalCosto = Valor.Tickets.Where(x => x.Nulo==false).Sum(x => x.Costo);
            var totalPago = Valor.Tickets.Sum(x => x.Pago);
            var totalValidor = Valor.Tickets.Where(x => x.Nulo == false).Count();
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
                        WriteText(g, "LISTADO DE TICKETS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g,  "Del Dia " + FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha), FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { ".", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;

                    case "Tickets":
                        WriteTextColumn(g, new List<string> { "Ticket", "Hora", "Vendio", "Saco" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        if (Valor.Tickets != null)
                        {
                            foreach (var ticket in Valor.Tickets)
                            {
                                if (ticket.Nulo == false)
                                {
                                    WriteTextColumn(g, new List<string> { ticket.TicketNo, ticket.StrHora, string.Format(CultureInfo.InvariantCulture, "{0:0,0}", ticket.Costo.ToString("C2")), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", ticket.Pago.ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                                }
                                else
                                {
                                    WriteTextColumn(g, new List<string> { ticket.TicketNo, ticket.StrHora, string.Format(CultureInfo.InvariantCulture, "{0:0,0}", ticket.Costo.ToString("C2")), "NULO" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                                }
                            }
                            WriteLineFinas(g);
                            WriteTextColumn(g, new List<string> { "Venta: " + string.Format(CultureInfo.InvariantCulture, "{0:0,0}", totalCosto.ToString("C2")), "Saco: " + string.Format(CultureInfo.InvariantCulture, "{0:0,0}", totalPago.ToString("C2")), "Validos: " + totalValidor, "Nulos: " + totalNulos }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                            WriteTextColumn(g, new List<string> { ".", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        }
                        else
                        {
                            WriteTextColumn(g, new List<string> { "Ningun ticket suyo salio ganador" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        }
                        break;
                }

            }
            positionWrite = 0;
            g.Dispose();

        }

        private static void TemplateRPTSumaVentas(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string AlignamenCenter = "center";
            string AlignamentLft = "left";
            Console.WriteLine(Value);
            var Valor = Value as MAR_RptSumaVta;
            List<ConfigPrinterModel> valores = new List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();

            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel SumaVentas = new ConfigPrinterModel() { ConfigKey = "SumasVentas", ConfigValue = "SumasVentas" };

            valores.Add(Header);
            valores.Add(SumaVentas);

            positionWrite = 0;

            var totalVenta = Valor.Reglones.Sum(x => x.VentaBruta);
            var totalComision = Valor.Reglones.Sum(x => x.Comision);
            var totalSaco = Valor.Reglones.Sum(x => x.Saco);
            var totalBalance = Valor.Reglones.Sum(x => x.Resultado);

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
                        WriteText(g, "SUMA DE VENTAS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Del Dia " +FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;
                    case "SumasVentas":
                        WriteTextColumn(g, new List<string> { "Concep.", "Venta", "Comis.", "Saco", "Balan." }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        if (Valor.Reglones != null)
                        {
                            foreach (var venta in Valor.Reglones)
                            {
                                WriteTextColumn(g, new List<string> { venta.Reglon, string.Format(CultureInfo.InvariantCulture, "{0:0,0}", venta.VentaBruta) , string.Format(CultureInfo.InvariantCulture, "{0:0,0}", venta.Comision), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", venta.Saco), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", venta.Resultado)}, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                            }
                            WriteLine(g);
                            WriteTextColumn(g, new List<string> { "Totales", string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", totalVenta), string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", totalComision), string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", totalSaco), string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", totalBalance) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { ".", "", " ", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        }

                        break;
                }

            }
            positionWrite = 0;
            g.Dispose();

        }

        private static void TemplateRPTTicketsGanadores(object sender, PrintPageEventArgs e)
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
            ConfigPrinterModel SumaVentas = new ConfigPrinterModel() { ConfigKey = "TicketsGanadores", ConfigValue = "TicketsGanadores" };

            valores.Add(Header);
            valores.Add(SumaVentas);

            positionWrite = 0;

            var TicketPendientePagos = Valor.Tickets.Where(ticket => ticket.Solicitud == 3);
            var TicketSinReclamar = Valor.Tickets.Where(ticket => ticket.Solicitud == 6);
            var TicketPagados = Valor.Tickets.Where(ticket => ticket.Solicitud == 5);

            var totalDeTicketPendientePagos = 0;
            var totalDeTicketSinReclamar = 0;
            var totalDeTicketPagados = 0;

            foreach (var TicketPendiente in TicketPendientePagos)
            {
                
                    totalDeTicketPendientePagos += Convert.ToInt32(TicketPendiente.Pago);
                
            };
            foreach (var TicketSinRecla in TicketSinReclamar)
            {
                
                    totalDeTicketSinReclamar += Convert.ToInt32(TicketSinRecla.Pago);
            };
            foreach (var TicketPagado in TicketPagados)
            {
                    totalDeTicketPagados += Convert.ToInt32(TicketPagado.Pago);
            };


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
                        WriteText(g, "TICKET GANADORES", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Del Dia " + FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha), FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;

                    case "TicketsGanadores":
                        WriteTextColumn(g, new List<string> { "Tickets", "Fecha", "Hora", "Monto" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        if (TicketPagados.Count() > 0)
                        {
                            WriteTextColumn(g, new List<string> { "-- Pagados ---" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                            foreach (var pagados in TicketPagados)
                            {
                                WriteTextColumn(g, new List<string> { pagados.TicketNo, FechaHelper.FormatFecha(Convert.ToDateTime(pagados.StrFecha), FechaHelper.FormatoEnum.FechaCorta), pagados.StrHora.ToString(), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", pagados.Pago.ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter); ;
                            }
                            WriteLineFinas(g);
                            WriteTextColumn(g, new List<string> { "Total:", "", "", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", totalDeTicketPagados.ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        }
                        if (TicketPendientePagos.Count() > 0)
                        {
                            WriteTextColumn(g, new List<string> { "-- Pendientes De Pago ---" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                            foreach (var pendientesPagos in TicketPendientePagos)
                            {
                                WriteTextColumn(g, new List<string> { pendientesPagos.TicketNo, FechaHelper.FormatFecha(Convert.ToDateTime(pendientesPagos.StrFecha), FechaHelper.FormatoEnum.FechaCorta), pendientesPagos.StrHora, string.Format(CultureInfo.InvariantCulture, "{0:0,0}", pendientesPagos.Pago.ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            }
                            WriteLineFinas(g);
                            WriteTextColumn(g, new List<string> { "Total:", "", "", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", totalDeTicketPendientePagos.ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        }
                        if (TicketSinReclamar.Count() > 0)
                        {
                            WriteTextColumn(g, new List<string> { "-- Sin Reclamar ---" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                            foreach (var sinReclamar in TicketSinReclamar)
                            {
                                WriteTextColumn(g, new List<string> { sinReclamar.TicketNo, FechaHelper.FormatFecha(Convert.ToDateTime(sinReclamar.StrFecha), FechaHelper.FormatoEnum.FechaCorta), sinReclamar.StrHora, string.Format(CultureInfo.InvariantCulture, "{0:0,0}", sinReclamar.Pago.ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            }
                            WriteLineFinas(g);
                            WriteTextColumn(g, new List<string> { "Total:", "", "", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", totalDeTicketSinReclamar.ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        }
                        WriteLine(g);
                        WriteTextColumn(g, new List<string> { "T.General:", "", "", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", (totalDeTicketPagados + totalDeTicketPendientePagos - totalDeTicketSinReclamar).ToString("C2")) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "." }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        break;
                }
            }
            positionWrite = 0;
            g.Dispose();
        }

        private static void TemplateRPTListTarjeta(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string AlignamenCenter = "center";
            string AlignamentLft = "left";
            Console.WriteLine(Value);
            var Valor = Value as MAR_Pines;
            List<ConfigPrinterModel> valores = new List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();

            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel Ventas = new ConfigPrinterModel() { ConfigKey = "ListTarjeta", ConfigValue = "ListTarjeta" };

            valores.Add(Header);
            valores.Add(Ventas);

            positionWrite = 0;

            var totalPrecio = Valor.Pines.Sum(x => x.Costo);

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
                        WriteText(g, "LISTADO DE TARJETAS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Del Dia " + FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha), FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;

                    case "ListTarjeta":
                        WriteTextColumn(g, new List<string> { "Suplidor", "Hora",  "Numero","Monto" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        if (Valor.Pines != null)
                        {
                            foreach (var tarjeta in Valor.Pines)
                            {
                                WriteTextColumn(g, new List<string> { tarjeta.Producto.Suplidor, tarjeta.StrHora, tarjeta.Serie.ToString(), tarjeta.Costo.ToString("C2") }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            }
                            WriteLineFinas(g);
                            WriteTextColumn(g, new List<string> { "Total:", "",""  , totalPrecio.ToString("C2")}, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        }
                        WriteTextColumn(g, new List<string> { ".", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);


                        break;
                }

            }
            positionWrite = 0;
            g.Dispose();

        }

        private static void TemplateRPTPagosRemotosTickets(object sender, PrintPageEventArgs e)
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
            ConfigPrinterModel Ventas = new ConfigPrinterModel() { ConfigKey = "TicketRemotos", ConfigValue = "TicketRemotos" };

            valores.Add(Header);
            valores.Add(Ventas);

            positionWrite = 0;

            var totalCosto = Valor.Tickets.Sum(x => x.Costo);
            var totalSaco = Valor.Tickets.Sum(x => x.Pago);

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
                        WriteText(g, "PAGOS REMOTOS", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy")),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Del Dia " + FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha), FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Loteria: " + nombreLoteria, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { "", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        break;

                    case "TicketRemotos":
                        WriteTextColumn(g, new List<string> { "Ticket", "Hora", "Vendio", "Saco" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        if (Valor.Tickets != null)
                        {
                            foreach (var remoto in Valor.Tickets)
                            {
                                WriteTextColumn(g, new List<string> { remoto.TicketNo, remoto.StrHora, remoto.Costo.ToString("C2"), remoto.Pago.ToString("C2") }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            }
                            WriteLineFinas(g);
                            WriteTextColumn(g, new List<string> { "", "", totalCosto.ToString("C2"), totalSaco.ToString("C2") }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        }
                        WriteTextColumn(g, new List<string> { ".", "", "", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        break;
                }
            }
            positionWrite = 0;
            g.Dispose();

        }

        private static void TemplateRPTVentasFechaNoResumido(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string AlignamenCenter = "center";
            string AlignamentLft = "left";
            Console.WriteLine(Value);
            var Valor = Value as MAR_RptSumaVta2;
            List<ConfigPrinterModel> valores = new List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();

            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel Ventas = new ConfigPrinterModel() { ConfigKey = "VentasFecha", ConfigValue = "VentasFecha" };

            valores.Add(Header);
            valores.Add(Ventas);

            positionWrite = 0;

            var totalofTotals = Valor.Reglones.Sum(x => x.VentaBruta);
            var allLoterias = Valor.Reglones.Select(x => x.Reglon).Distinct();

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
                        WriteText(g, "VENTAS POR FECHA", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Desde: " + FechaHelper.FormatFecha(Convert.ToDateTime(ViewModel.FechaInicio),
                                    FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Hasta: " + FechaHelper.FormatFecha(Convert.ToDateTime(ViewModel.FechaFin),
                                    FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { ".", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);

                        break;

                    case "VentasFecha":
                        WriteTextColumn(g, new List<string> { "Fecha", "Venta", "Comis.", "Saco", "Balan." }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        if (Valor.Reglones != null)
                        {
                            foreach (var loteria in allLoterias)
                            {
                                WriteLine(g);
                                WriteTextColumn(g, new List<string> { loteria }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                                WriteLine(g);

                                var VentasDeLoteria = Valor.Reglones.Where(x => x.Reglon.ToLower() == loteria.ToLower());
                                foreach (var ventaFecha in VentasDeLoteria)
                                {
                                    WriteTextColumn(g, new List<string> { FechaHelper.FormatFecha(Convert.ToDateTime(ventaFecha.Fecha), FechaHelper.FormatoEnum.FechaCorta), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", ventaFecha.VentaBruta), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", ventaFecha.Comision), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", ventaFecha.Saco), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", ventaFecha.Resultado) }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                                }
                                WriteLineFinas(g);
                                WriteTextColumn(g, new List<string> { "Total:", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", VentasDeLoteria.Sum(x => x.VentaBruta)) , string.Format(CultureInfo.InvariantCulture, "{0:0,0}", VentasDeLoteria.Sum(x => x.Comision)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", VentasDeLoteria.Sum(x => x.Saco)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", VentasDeLoteria.Sum(x => x.Resultado))}, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                                WriteTextColumn(g, new List<string> { ".", "", "", "", "" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                            }
                            WriteLine(g);
                            WriteTextColumn(g, new List<string> { "Total General:", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.VentaBruta)) , string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.Comision)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.Saco)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.Resultado))  }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { ".", "", "", "", "" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                        }
                        break;
                }

            }
            positionWrite = 0;
            g.Dispose();

        }

        private static void TemplateRPTVentasFechaResumido(object sender, PrintPageEventArgs e)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;
            Graphics g = e.Graphics;
            int FontSize = 11;
            string AlignamenCenter = "center";
            string AlignamentLft = "left";
            Console.WriteLine(Value);
            var Valor = Value as MAR_RptSumaVta2;
            List<ConfigPrinterModel> valores = new List<ConfigPrinterModel>() { };
            var nombreLoteria = new ReporteView().GetNombreLoteria();

            ConfigPrinterModel Header = new ConfigPrinterModel() { ConfigKey = "Header", ConfigValue = AUTENTICATION.BancaConfiguracion.BancaDto.BanNombre };
            ConfigPrinterModel Ventas = new ConfigPrinterModel() { ConfigKey = "VentasFecha", ConfigValue = "VentasFecha" };

            valores.Add(Header);
            valores.Add(Ventas);

            positionWrite = 0;

            var totalofTotals = Valor.Reglones.Sum(x => x.VentaBruta);
            var allLoterias = Valor.Reglones.Select(x => x.Reglon).Distinct();

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
                        WriteText(g, "VENTAS POR FECHA", FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, FechaHelper.FormatFecha(Convert.ToDateTime(Valor.Fecha),
                                     FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Desde: " + FechaHelper.FormatFecha(Convert.ToDateTime(ViewModel.FechaInicio),
                                    FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteText(g, "Hasta: " + FechaHelper.FormatFecha(Convert.ToDateTime(ViewModel.FechaFin),
                                    FechaHelper.FormatoEnum.FechaCortaDOW), FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        WriteTextColumn(g, new List<string> { ".", "", " ", "" }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                        break;

                    case "VentasFecha":
                        WriteTextColumn(g, new List<string> { "Fecha  ", "Venta", "Comis.", "Saco", "Balan." }, FontSize, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                        if (Valor.Reglones != null)
                        {
                            foreach (var loteria in allLoterias)
                            {
                                WriteLine(g);
                                WriteTextColumn(g, new List<string> { loteria }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamentLft);
                                WriteLine(g);
                                WriteTextColumn(g, new List<string> { "Total:", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Where(x => x.Reglon == loteria).Sum(x => x.VentaBruta)) , string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Where(x => x.Reglon == loteria).Sum(x => x.Comision)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Where(x => x.Reglon == loteria).Sum(x => x.Saco)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Where(x => x.Reglon == loteria).Sum(x => x.Resultado))  }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                                WriteTextColumn(g, new List<string> { ".", "", "", "", "" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);

                            }
                            WriteLine(g);
                            WriteTextColumn(g, new List<string> { "Total General:", string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.VentaBruta)) , string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.Comision)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.Saco)), string.Format(CultureInfo.InvariantCulture, "{0:0,0}", Valor.Reglones.Sum(x => x.Resultado))}, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
                            WriteTextColumn(g, new List<string> { ".", "", "", "", "" }, FontSize - 1, FontStyle.Regular.ToString().ToLower(), AlignamenCenter);
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
