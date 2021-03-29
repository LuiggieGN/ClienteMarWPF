using ClienteMarWPFWin7.UI.State.Authenticators;
using MAR.AppLogic.MARHelpers;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace ClienteMarWPFWin7.UI.State.PinterConfig
{
    class PrintJobs
    {
        private readonly IAuthenticator Autenticador;
        public PrintJobs(IAuthenticator authenticator)
        {
            Autenticador = authenticator;
        }
        internal static List<string[]> FromReporteDeGanadores(MAR_Ganadores ganadores,ReportesIndexGanadores reporte,IAuthenticator autenticador)
        {
            var j = new List<string[]>();
            string printString = "";


            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanContacto, autenticador.BancaConfiguracion.BancaDto.BanContacto.Length + 7) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length + 7) + Environment.NewLine;
            printString += Center("TICKETS GANADORES", 18) + Environment.NewLine;

            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(reporte.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 24) + Environment.NewLine;

            printString += Center("LOTERIA " + reporte.Sorteo, 20) + Environment.NewLine+ Environment.NewLine;

            if (reporte.Primero != null && reporte.Primero.Trim().Length > 0) printString += Center("Premios " + "1ra:" + reporte.Primero + ", 2da:" + reporte.Segundo + ", 3ra:" + reporte.Tercero, 30) + Environment.NewLine;

            if (ganadores.Tickets != null)
            {
                var rt = Environment.NewLine;
                printString += Justify("Tickets             Fecha               Monto","  ", 40).PadLeft(30,' ') + rt;
                Double total_s = 0, total_n = 0, total_o = 0, total_r = 0;
                Double lastGroup = 0;

                int ganadoresTicketsCount = ganadores.Tickets.Count();

                for (int n = 0; n < ganadoresTicketsCount; n++)
                {
                    var ticket = ganadores.Tickets[n];
                    if (lastGroup != ticket.Solicitud)
                    {
                        if (n > 0)
                        {
                            if (lastGroup == 3) printString += Justify("", "$" + total_n.ToString("N2"), 30) + rt;
                            if (lastGroup == 4) printString += Justify("Pendientes por pagar", "$" + total_o.ToString("N2"), 30) + rt;
                            if (lastGroup == 5) printString += Justify("", "$" + total_s.ToString("N2"), 30) + rt;
                            if (lastGroup == 6) printString += Justify("", "$" + total_r.ToString("N2"), 30) + rt;
                            printString += rt;
                        }
                        if (ticket.Solicitud == 3) printString += Center("Pendientes por Pagar", 20) + rt;
                        if (ticket.Solicitud == 5) printString += Center("Tickets Pagados", 15) + rt;
                        if (ticket.Solicitud == 6) printString += Center("Premios Sin Reclamar", 20) + rt;
                        lastGroup = ticket.Solicitud;
                    }
                    var fechaCorta = FechaHelper.FormatFecha(DateTime.Parse(ticket.StrFecha), FechaHelper.FormatoEnum.FechaCorta);
                    if (lastGroup == 3)
                    {
                        total_n += ticket.Pago;
                        printString += Justify(ticket.TicketNo.PadRight(15, ' ') + fechaCorta + "      ", "$" + ticket.Pago.ToString("N0"), 40).PadLeft(20, ' ') + rt;
                    }
                    else if (lastGroup == 4)
                    {
                        total_o += ticket.Pago;
                    }
                    else if (lastGroup == 5)
                    {
                        printString += Justify(ticket.TicketNo.PadRight(15, ' ') + fechaCorta + " ", "$" + ticket.Pago.ToString("N0"), 40).PadLeft(30, ' ') + rt;
                        total_s += ticket.Pago;
                    }
                    else if (lastGroup == 6)
                    {
                        printString += Justify(ticket.TicketNo.PadRight(15, ' ') + fechaCorta + " ", "$" + ticket.Pago.ToString("N0"), 40).PadLeft(30,' ') + rt;
                        total_r += ticket.Pago;
                    }
                }
                if (lastGroup == 3) printString += Justify("", "$" + total_n.ToString("N2"), 55) + rt;
                if (lastGroup == 4) printString += Justify("Pendientes por pagar", "$" + total_o.ToString("N2"), 40) + rt;
                if (lastGroup == 5) printString += Justify("", "$" + total_s.ToString("N2"), 55) + rt;
                if (lastGroup == 6) printString += Justify("", "$" + total_r.ToString("N2"), 55) + rt;
                printString += rt;

                printString += Justify("Balance Ganadores", (total_n + total_s + total_o - total_r).ToString("N2"), 20) + rt;
                j.Add(new string[] { printString });
            }
            else
            {
                j.Add(new string[] { "Ningun ticket suyo resulto ganador!" });
            }
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            return j;
        }


        internal static List<string[]> FromMultiTicket(List<VentasIndexTicket> pTickets, bool pEsCopia = false)
        {
            var j = new List<string[]>();
            var w = 35;
            MAR_Session session = new MAR_Session();

            j.Add(new string[] { Center("NO DISPONIBLE".ToUpper(), w), "2" });
            j.Add(new string[] { Center("DIRECCION NO DISPONIBLE", w), "1" });
            j.Add(new string[] { Justify(
                FechaHelper.FormatFecha(Convert.ToDateTime(pTickets[0].Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW),
                pTickets[0].Hora, w), "1" });
            j.Add(new string[] { Center("Tel: " + "TELEFONO NO DISPONIBLE", w), "1" });
            if (!string.IsNullOrEmpty(session.PrinterHeader)) j.Add(new string[] { Center(session.PrinterHeader, w), "1" });
            j.Add(new string[] { "-".PadRight(w, '-'), "1" });
            j.Add(new string[] { Justify("Loteria", " Ticket     Pin  ", w), "1" });

            var theQ = new List<VentasIndexTicket.Jugada>();
            var theP = new List<VentasIndexTicket.Jugada>();
            var theT = new List<VentasIndexTicket.Jugada>();
            Double theGrandTotal = 0;
            for (var i = 0; i < pTickets.Count; i++)
            {
                j.Add(new string[] { Justify(pTickets[i].SorteoNombre.Substring(0, Math.Min(pTickets[i].SorteoNombre.Length, w - 20)), pTickets[i].TicketNo + " " + pTickets[i].Pin, w), "1" });
                var theAddedNums = theQ.Select(x => x.Numero).Distinct().ToList();
                theAddedNums.AddRange(theP.Select(x => x.Numero).Distinct());
                theAddedNums.AddRange(theT.Select(x => x.Numero).Distinct());
                theQ.AddRange(pTickets[i].Jugadas.Where(x => x.Tipo.Equals("Q"))
                                    .Where(x => !theAddedNums.Contains(x.Numero)));
                theP.AddRange(pTickets[i].Jugadas.Where(x => x.Tipo.Equals("P"))
                                    .Where(x => !theAddedNums.Contains(x.Numero)));
                theT.AddRange(pTickets[i].Jugadas.Where(x => x.Tipo.Equals("T"))
                                    .Where(x => !theAddedNums.Contains(x.Numero)));
                theGrandTotal += pTickets[i].Costo;
            }
            if (pEsCopia) j.Add(new string[] { Center("** COPIA REIMPRESA **", w), "1" });
            j.Add(new string[] { "-".PadRight(w, '-'), "1" });

            for (var i = 0; i < theQ.Count; i++)
            {

                if (i == 0) j.Add(new string[] { "-- NUMEROS --", "2" });
                j.Add(new string[] { Justify(theQ[i].Numero.Trim().PadRight(2, ' '), "$" +
                                             String.Format("{0:0.00}", theQ[i].Total),w), "2" });
            }

            for (var i = 0; i < theP.Count; i++)
            {
                if (i == 0) j.Add(new string[] { "-- PALES --", "2" });
                j.Add(new string[] { Justify(theP[i].Numero.Trim().Substring(0,2) + "-" +
                                             theP[i].Numero.Trim().Substring(2,2), "$" +
                                             String.Format("{0:0.00}", theP[i].Total), w), "2" });
            }

            for (var i = 0; i < theT.Count; i++)
            {
                if (i == 0) j.Add(new string[] { "-- TRIPLETAS --", "2" });
                j.Add(new string[] { Justify(theT[i].Numero.Trim().Substring(0,2) + "-" +
                                             theT[i].Numero.Trim().Substring(2,2) + "-" +
                                             theT[i].Numero.Trim().Substring(4,2)   , "$" +
                                             String.Format("{0:0.00}", theT[i].Total), w), "2" });
            }
            j.Add(new string[] { "-".PadRight(w, '-'), "2" });
            j.Add(new string[] { Center(String.Format("TOTAL ${0:0.00}", theGrandTotal), w), "2" });
            if (!string.IsNullOrEmpty(pTickets[0].Firma)) j.Add(new string[] { Center(String.Format("Firma: {0}", pTickets[0].Firma), w), "1" });
            if (!string.IsNullOrEmpty(session.PrinterFooter)) j.Add(new string[] { Center(session.PrinterFooter, w), "1" });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> FromReporteVentas(VentasIndexTicket pTck, bool pEsCopia = false)
        {
            var j = new List<string[]>();
            var w = 35;
            MAR_Session session = new MAR_Session();
            

            var theSorteoOculto = !pTck.TicketNo.Contains("-");
            if (theSorteoOculto)
            {
                j.Add(new string[] { "-".PadRight(w, '-'), "2" });
                j.Add(new string[] { Center(pTck.SorteoNombre, w), "2" });
                j.Add(new string[] { pTck.TicketNo, "1" });
            }
            else
            {
                j.Add(new string[] { Center("NO DISPONIBLE".ToUpper(), w), "2" });
                j.Add(new string[] { Center(pTck.SorteoNombre, w), "2" });
                j.Add(new string[] { Center("NO DISPONIBLE", w), "1" });
                j.Add(new string[] { Justify(pTck.TicketNo, "TELEFONO NO DISPONIBLE", w), "1" });
            }
            j.Add(new string[] { Justify(
                FechaHelper.FormatFecha(Convert.ToDateTime(pTck.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW),
                pTck.Hora, w), "1" });

            if (!string.IsNullOrEmpty(pTck.Pin)) j.Add(new string[] { Center(String.Format("Pin: {0}", pTck.Pin), w), "1" });
            if (!string.IsNullOrEmpty(session.PrinterHeader)) j.Add(new string[] { Center(session.PrinterHeader, w), "1" });
            if (pEsCopia) j.Add(new string[] { Center("** COPIA REIMPRESA **", w), "2" });
            j.Add(new string[] { "-".PadRight(w, '-'), "2" });
            var theQ = pTck.Jugadas.Where(x => x.Tipo.Equals("Q")).ToArray();
            var theP = pTck.Jugadas.Where(x => x.Tipo.Equals("P")).ToArray();
            var theT = pTck.Jugadas.Where(x => x.Tipo.Equals("T")).ToArray();

            for (var i = 0; i < theQ.Length; i++)
            {
                string delIntermedio = "";
                string delInicio = "";
                string cantidad = theQ[i].Cantidad.ToString();
                if (theQ[i].Cantidad > 9999)
                {
                    delInicio = "Del ";
                    delIntermedio = "";
                    cantidad = "";
                }
                else if (theQ[i].Cantidad > 99 && theQ[i].Cantidad < 10000)
                {
                    delInicio = "";
                    delIntermedio = "";
                }
                else
                {
                    delInicio = "";
                    delIntermedio = " del ";
                }
                if (i == 0) j.Add(new string[] { "-- NUMEROS --", "0" });
                j.Add(new string[] { Justify(delInicio + cantidad.Trim().PadRight(6, ' ') +  delIntermedio +
                                             theQ[i].Numero.Trim().PadRight(2, ' '), "$" +
                                             String.Format("{0:0.00}", theQ[i].Total),w), "2" });
            }

            for (var i = 0; i < theP.Length; i++)
            {
                if (i == 0) j.Add(new string[] { "-- PALES --", "2" });
                j.Add(new string[] { Justify(theP[i].Numero.Trim().Substring(0,2) + "-" +
                                             theP[i].Numero.Trim().Substring(2,2), "$" +
                                             String.Format("{0:0.00}", theP[i].Total), w), "2" });
            }

            for (var i = 0; i < theT.Length; i++)
            {
                if (i == 0) j.Add(new string[] { "-- TRIPLETAS --", "2" });
                j.Add(new string[] { Justify(theT[i].Numero.Trim().Substring(0,2) + "-" +
                                             theT[i].Numero.Trim().Substring(2,2) + "-" +
                                             theT[i].Numero.Trim().Substring(4,2)   , "$" +
                                             String.Format("{0:0.00}", theT[i].Total), w), "2" });
            }
            j.Add(new string[] { "-".PadRight(w, '-'), "2" });
            var theGrandTotal = pTck.Jugadas.Sum(x => x.Total);
            j.Add(new string[] { Center(String.Format("TOTAL ${0:0.00}", theGrandTotal), w), "2" });
            if (!string.IsNullOrEmpty(pTck.Firma)) j.Add(new string[] { Center(String.Format("Firma: {0}", pTck.Firma), w), "1" });
            if (!string.IsNullOrEmpty(session.PrinterFooter)) j.Add(new string[] { Center(session.PrinterFooter, w), "1" });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
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

      

        internal static List<string[]> FromReporteDeGanadores(MAR_Ganadores ganadores, ReportesIndexGanadores reporte, List<Ganadores> pRFGanadores)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";

            printString += Center("NO DISPONIBLE".ToUpper(), w) + Environment.NewLine;
            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("TICKETS GANADORES", w) + Environment.NewLine;

            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(reporte.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w) + Environment.NewLine;

            printString += Center("LOTERIA " + reporte.Sorteo, w) + Environment.NewLine;

            if (reporte.Primero != null && reporte.Primero.Trim().Length > 0) printString += Center("Premios " + "1ra:" + reporte.Primero + ", 2da:" + reporte.Segundo + ", 3ra:" + reporte.Tercero, w) + Environment.NewLine;

            if (ganadores.Tickets != null)
            {
                var rt = Environment.NewLine;
                printString += Justify("Tickets     Fecha", "Monto", w).PadRight(0) + rt;
                Double total_s = 0, total_n = 0, total_o = 0, total_r = 0;
                Double lastGroup = 0;

                int ganadoresTicketsCount = ganadores.Tickets.Count();

                for (int n = 0; n < ganadoresTicketsCount; n++)
                {
                    var ticket = ganadores.Tickets[n];
                    if (lastGroup != ticket.Solicitud)
                    {
                        if (n > 0)
                        {
                            if (lastGroup == 3) printString += Justify("", "$" + total_n.ToString("N2"), w) + rt;
                            if (lastGroup == 4) printString += Justify("Pendientes por pagar", "$" + total_o.ToString("N2"), w) + rt;
                            if (lastGroup == 5) printString += Justify("", "$" + total_s.ToString("N2"), w) + rt;
                            if (lastGroup == 6) printString += Justify("", "$" + total_r.ToString("N2"), w) + rt;
                            printString += rt;
                        }
                        if (ticket.Solicitud == 3) printString += Center("Pendientes por Pagar", w) + rt;
                        if (ticket.Solicitud == 5) printString += Center("Tickets Pagados", w) + rt;
                        if (ticket.Solicitud == 6) printString += Center("Premios Sin Reclamar", w) + rt;
                        lastGroup = ticket.Solicitud;
                    }
                    var fechaCorta = FechaHelper.FormatFecha(DateTime.Parse(ticket.StrFecha), FechaHelper.FormatoEnum.FechaCorta);
                    if (lastGroup == 3)
                    {
                        total_n += ticket.Pago;
                        printString += Justify(ticket.TicketNo.PadRight(11, ' ') + fechaCorta + " " + ticket.StrHora, "$" + ticket.Pago.ToString("N0"), w) + rt;
                    }
                    else if (lastGroup == 4)
                    {
                        total_o += ticket.Pago;
                    }
                    else if (lastGroup == 5)
                    {
                        printString += Justify(ticket.TicketNo.PadRight(12, ' ') + fechaCorta + " " + ticket.StrHora, "$" + ticket.Pago.ToString("N0"), w) + rt;
                        total_s += ticket.Pago;
                    }
                    else if (lastGroup == 6)
                    {
                        printString += Justify(ticket.TicketNo.PadRight(12, ' ') + fechaCorta + " " + ticket.StrHora, "$" + ticket.Pago.ToString("N0"), w) + rt;
                        total_r += ticket.Pago;
                    }
                }

                List<string> loterias = pRFGanadores.Select(x => x.Loteria).Distinct().ToList();
                decimal? totalSacoRF = pRFGanadores.Sum(x => x.Saco);
                foreach (var item in loterias)
                {
                    printString += Justify("--" + item + "--", "", w).PadRight(0) + rt;
                    foreach (var g in pRFGanadores)
                    {
                        if (item == g.Loteria)
                        {
                            int saco = Convert.ToInt32(g.Saco);
                            printString += Justify(g.Ticket + " " + (g.TicPagado == true ? "Pagado" : ""), saco.ToString(), w).PadRight(0) + rt;
                            if (g.TicPagado)
                            {
                                totalSacoRF -= saco;
                            }
                        }
                    }
                }



                if (lastGroup == 3) printString += Justify("", "$" + total_n.ToString("N2"), w) + rt;
                if (lastGroup == 4) printString += Justify("Pendientes por pagar", "$" + total_o.ToString("N2"), w) + rt;
                if (lastGroup == 5) printString += Justify("", "$" + total_s.ToString("N2"), w) + rt;
                if (lastGroup == 6) printString += Justify("", "$" + total_r.ToString("N2"), w) + rt;
                printString += rt;

                printString += Justify("Balance Ganadores", ((total_n + total_s + total_o + double.Parse(totalSacoRF.ToString()) - total_r)).ToString("N2"), w) + rt;
                j.Add(new string[] { printString });
            }
            else
            {
                j.Add(new string[] { "Ningun ticket suyo resulto ganador!" });
            }
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> FromReporteSumaVenta(MAR_RptSumaVta theSumaVenta,IAuthenticator autenticador)
        {
            MAR_Session session = new MAR_Session();
            var j = new List<string[]>();
            var jd = new List<string>();
            var w =  20 ;
            
            j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanContacto , autenticador.BancaConfiguracion.BancaDto.BanContacto.Length+7) + Environment.NewLine });
            j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length + 7) + Environment.NewLine });
            j.Add(new string[] { Center("SUMA DE VENTAS", 15) });
            j.Add(new string[] { Center("FECHA DEL IMPRESION", 20) });
            j.Add(new string[] { Center(
              FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
              FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            j.Add(new string[] { Center("FECHA DEL REPORTE", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(theSumaVenta.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW).Trim(), 27)});

            j.Add(new string[] { Center("Concep.  ".PadRight(0) + "Venta    Comis.    Saco    Balan", 50) });

            double comision = 0, venta = 0, resultado = 0, saco = 0;

            DateTime fechaPagoDesde = Convert.ToDateTime(theSumaVenta.Fecha).Date;
            DateTime fechaPagoHasta = Convert.ToDateTime(theSumaVenta.Fecha).Date.AddDays(1);


            foreach (var rgn in theSumaVenta.Reglones)
            {
                if (rgn.VentaBruta > 0 || rgn.Resultado > 0)
                    if (Math.Round(rgn.VentaBruta, 0) == 0)
                    {
                        if (rgn.Reglon == "Pago Remoto") rgn.Reglon = "Ganador pagado remoto";
                        if (rgn.Reglon == "Sin Reclamar") rgn.Reglon = "Ganador sin reclamar";
                        j.Add(new string[]
                                {
                                    CortarString(rgn.Reglon, 22).PadRight(22,' ') + " " + rgn.Resultado.ToString().PadLeft(6)
                                });
                    }
                    else
                    {
                        j.Add(new string[]
                                {
                                   Justify((CortarString(rgn.Reglon, 7).TrimStart().PadRight(12,' ')).Trim(' ')+ " " + rgn.VentaBruta.ToString("N0").TrimStart().PadRight(12,' ') + rgn.Comision.ToString().TrimStart().PadRight(12,' ')
                                                 + rgn.Saco.ToString().PadRight(7,' ') + rgn.Resultado.ToString().TrimEnd().PadRight(0,' ')," ",40)
                                });
                    }

                comision += rgn.Comision;
                venta += rgn.VentaBruta;
                resultado += rgn.Resultado;
                saco += rgn.Saco;
            }

            j.Add(new string[] { Center("-------------------------------", 50) });
            j.Add(new string[]
                    {
                       "Total=> "+ " " +  Math.Round(venta, 0).ToString().PadLeft(10) + comision.ToString().PadLeft(10)
                     + saco.ToString().PadLeft(10) + resultado.ToString().PadLeft(10)
                    });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", 50) });
            return j;
        }

        internal static List<string[]> FromReporteVentaPorFecha(MAR_RptSumaVta2 venta, string fDes, string fHas, bool resumido,IAuthenticator autenticador)
        {
            var j = new List<string[]>();
            MAR_Session session = new MAR_Session();
            var w = 35;
            string printString = "";


            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanContacto, autenticador.BancaConfiguracion.BancaDto.BanContacto.Length + 7) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length + 7) + Environment.NewLine;
            printString += Center("VENTAS POR FECHA", 18) + Environment.NewLine;

            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t") + Environment.NewLine, 24) + Environment.NewLine;



            printString += Justify("Desde:", FechaHelper.FormatFecha(Convert.ToDateTime(fDes),
                FechaHelper.FormatoEnum.FechaCortaDOW), 15);

            printString += Justify("Hasta:", FechaHelper.FormatFecha(Convert.ToDateTime(fHas),
             FechaHelper.FormatoEnum.FechaCortaDOW), 15) + Environment.NewLine + Environment.NewLine;

            // Mar_ReglonVta rg = new Mar_ReglonVta();
            double tCom = 0, tVen = 0, tRes = 0, tSac = 0, stCom = 0, stVen = 0, stRes = 0, stSac = 0;
            string oldCon = "OLDSTRING";
            string Fec = "";

            printString +=  Justify("FECHA VENTA COMIS. SACO BAL.".TrimStart()," ",20) + Environment.NewLine;

            foreach (var rg in venta.Reglones)
            {
                if (rg.VentaBruta > 0 || rg.Resultado != 0)
                {
                    if (rg.Reglon == "Pago Remoto") rg.Reglon = "Ganador pagado remoto";
                    if (rg.Reglon == "Sin Reclamar") rg.Reglon = "Ganador sin reclamar";
                    if (rg.Reglon != oldCon)
                    {
                        if (oldCon != "OLDSTRING")
                        {
                            printString += ("SUMA==>" + stVen.ToString("N0").PadLeft(0, ' ').PadRight(8,' ') + stCom.ToString("N0").PadLeft(0, ' ').PadRight(8,' ') + stSac.ToString("N0").PadLeft(0, ' ').PadRight(8,' ') + stRes.ToString("N0").PadLeft(0, ' ').PadRight(8,' ')) + Environment.NewLine + Environment.NewLine;
                            stCom = 0;
                            stVen = 0;
                            stRes = 0;
                            stSac = 0;
                        }
                        printString += Justify("CONCEPTO: ", rg.Reglon, 30) + Environment.NewLine;
                        oldCon = rg.Reglon;
                    }
                    Fec = Justify(FechaHelper.FormatFecha(Convert.ToDateTime(rg.Fecha), FechaHelper.FormatoEnum.FechaCortaRegional), "", w);

                    //   Fec = Strings.Left(FormatFecha(rg.Fecha, 3), 2) & " " & Strings.Mid(FormatFecha(rg.Fecha, 3), InStr(FormatFecha(rg.Fecha, 3), ",") + 2, 6).Replace("-", "")

                    if (!resumido)
                    {
                        printString += Justify(Fec.PadLeft(0, ' '), rg.VentaBruta.ToString("N0").PadLeft(14, ' ') + rg.Comision.ToString("N0").PadLeft(14, ' ') + rg.Saco.ToString("N0").PadLeft(14, ' ') + rg.Resultado.ToString("N0").PadRight(14, ' '), 40) + Environment.NewLine;
                    }

                    tCom += rg.Comision;
                    tVen += rg.VentaBruta;
                    tRes += rg.Resultado;
                    tSac += rg.Saco;
                    stCom += rg.Comision;
                    stVen += rg.VentaBruta;
                    stRes += rg.Resultado;
                    stSac += rg.Saco;
                }
            }

            printString += ("SUMA==>" + stVen.ToString("N0").PadLeft(0, ' ') + stCom.ToString("N0").PadLeft(3, ' ') + stSac.ToString("N0").PadLeft(3, ' ') + stRes.ToString("N0").PadLeft(3, ' ')) + Environment.NewLine;
            printString += Justify("-------------------------------", "", 40)+Environment.NewLine;
            printString += ("TOTAL=>" + tVen.ToString("N0").PadLeft(8, ' ') + tCom.ToString("N0").PadLeft(8, ' ') + tSac.ToString("N0").PadLeft(8, ' ') + tRes.ToString("N0").PadLeft(8, ' ')) + Environment.NewLine;

            if (venta.ISRRetenido + venta.RifDescuento > 0)
            {
                printString += Justify("-------------------------------", "", w);
            }

            if (venta.ISRRetenido > 0)
            {
                printString += Justify("Impuesto Retenido=>", Math.Round(venta.ISRRetenido, 2).ToString("C2"), w);
            }

            if (venta.RifDescuento > 0)
            {
                printString += Justify("Descuento Acumulado=>", Math.Round(venta.RifDescuento, 2).ToString("C2"), w);
            }

            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> FromReporteVenta(MAR_RptVenta venta, string sorteo,IAuthenticator autenticador)
        {
            var j = new List<string[]>();
            var w = 14;
            string printString = "", loter = "";
            
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanContacto, autenticador.BancaConfiguracion.BancaDto.BanContacto.Length + 7) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length+7) + Environment.NewLine;
            printString += Center("REPORTE DE VENTA", 17) + Environment.NewLine;
            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(venta.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 27) + Environment.NewLine;

            printString += Center("Loteria: "+sorteo, 19) + Environment.NewLine;




            if (venta.Loteria < 3 || venta.Loteria > 4)
            {
                printString += Justify("NUMEROS:" , venta.CntNumeros.ToString("N0"),47) + Environment.NewLine;

                printString += Justify("NUMEROS (RD$):", Math.Round(venta.Numeros).ToString("C2"),40) + Environment.NewLine;
                printString += Justify("PALES:", Math.Round(venta.Pales, 2).ToString("C2"),50).Trim() + Environment.NewLine;
                printString += Justify("TRIPLETAS:", Math.Round(venta.Tripletas).ToString("C2").Trim(),47) + Environment.NewLine;
            }
            else
            {
                printString += Justify("Pega3 COMBO:", venta.Numeros.ToString("C2"),47) + Environment.NewLine;
                printString += Justify("Pega3 FIJO", Math.Round(venta.Pales, 2).ToString("C2"),47) + Environment.NewLine;
            }


            printString += Justify("TOTAL VENTA ===>", Math.Round(venta.Numeros + venta.Pales + venta.Tripletas).ToString("C2"),40) + Environment.NewLine;

            printString += Justify("COMISION (" + Math.Round((venta.ComisionPorcQ + venta.ComisionPorcP + venta.ComisionPorcT) / 3, 2).ToString() + "%):", Math.Round(venta.Comision, 0).ToString("C0"),48) + Environment.NewLine;


            printString += Justify("VENTA NETA ===>", (Math.Round(venta.Numeros + venta.Pales + venta.Tripletas, 0) - venta.Comision).ToString("C2"),40) + Environment.NewLine + Environment.NewLine; ;


            if (venta.Primero.Trim() != "")
            {
                printString += Justify("PREMIOS      CANTIDAD", "GANA",37) + Environment.NewLine;

                if (venta.Loteria < 3 || venta.Loteria > 4)
                {
                    printString += ("1ra. " + venta.Primero + venta.CPrimero.ToString("N2").PadLeft(20, ' ') + venta.MPrimero.ToString("C0").PadLeft(16, ' ')) + Environment.NewLine;
                    printString += ("2da. " + venta.Segundo + venta.CSegundo.ToString("N2").PadLeft(19, ' ') + venta.MSegundo.ToString("C0").PadLeft(17, ' ')) + Environment.NewLine;
                    printString += ("3ra. " + venta.Tercero + venta.CTercero.ToString("N2").PadLeft(20, ' ') + venta.MTercero.ToString("C0").PadLeft(16, ' ')) + Environment.NewLine + Environment.NewLine;
                    printString += ("NUMEROS PREMIADOS:" + Math.Round(venta.MTercero + venta.MPrimero + venta.MSegundo, 0).ToString("C2").PadLeft(17, ' ')) + Environment.NewLine;
                    printString += ("PALES PREMIADOS:  " + Math.Round(venta.MPales, 0).ToString("C2").PadLeft(20, ' ')) + Environment.NewLine;
                    printString += ("TRIPLETA PREMIADA:" + Math.Round(venta.MTripletas, 0).ToString("C2").PadLeft(20, ' ')) + Environment.NewLine;
                    printString += ("TOTAL PREMIADOS ==>" + Math.Round(venta.MTercero + venta.MPrimero + venta.MSegundo + venta.MPales + venta.MTripletas, 0).ToString("C2").PadLeft(17, ' ')) + Environment.NewLine;
                }
                else
                {
                    printString += Justify("PREMIOS PEGA3:", (venta.Primero + "-" + venta.Segundo + "-" + venta.Tercero).ToString(),40) + Environment.NewLine;
                    printString += Justify("COMBOS PREMIADOS:", Math.Round(venta.MPrimero, 0).ToString("C2"),47) + Environment.NewLine;
                    printString += Justify("FIJOS PREMIADOS: ", Math.Round(venta.MPales, 0).ToString("C2"),47) + Environment.NewLine;
                    printString += Justify("TOTAL PREMIADOS: ", Math.Round(venta.MTercero + venta.MPrimero + venta.MSegundo + venta.MPales + venta.MTripletas, 0).ToString("C2"),40) + Environment.NewLine;
                }


                double GP = Math.Round(venta.Numeros + venta.Pales + venta.Tripletas, 0) - (venta.Comision + venta.MTercero + venta.MPrimero + venta.MSegundo + venta.MPales + venta.MTripletas);


                if (GP < 0)
                {
                    printString += Justify("PERDIDA  ===> ", Math.Round(GP, 0).ToString("C2"),40) + Environment.NewLine;
                }
                else
                {
                    printString += Justify("GANANCIA ===> ", Math.Round(GP, 0).ToString("C2"),40) + Environment.NewLine;
                }

            }
            else
            {
                printString += Center("Los premios no estan disponibles", 32) + Environment.NewLine;
                
            }

            if (venta.TicketsNulos.Any())
            {
                double TCostNulos = 0;
                printString += Environment.NewLine + Center("--- LISTA DE TICKETS NULOS ---", 20) + Environment.NewLine;
                printString += "  Ticket #      Hora      Precio" + Environment.NewLine;

                for (int n = 0; n < venta.TicketsNulos.Count(); n++)
                {
                    printString += Justify(venta.TicketsNulos[n].TicketNo + "   " + venta.TicketsNulos[n].StrHora, Math.Round(venta.TicketsNulos[n].Costo, 2).ToString("C2"), w) + Environment.NewLine;
                    TCostNulos += venta.TicketsNulos[n].Costo;
                }

                printString += Justify("Tickets nulos: " + venta.TicketsNulos.Count(), Math.Round(TCostNulos, 2).ToString("C2"), w) + Environment.NewLine;
            }
            else
            {
                printString += Environment.NewLine + Center("No hay tickets nulos", 22) + Environment.NewLine;
            }

            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
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


        internal static List<string[]> FromReporteListadoDePines(MAR_Pines thePines,IAuthenticator autenticador)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";


            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanContacto, autenticador.BancaConfiguracion.BancaDto.BanContacto.Length + 7) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length + 7) + Environment.NewLine;
            printString += Center("LISTADO DE PINES", 16) + Environment.NewLine;
            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(thePines.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 40) + Environment.NewLine + Environment.NewLine;

              
            printString += Justify("Suplidor        ".PadRight(0) + "Hora      Precio              Serie", "", 40) + Environment.NewLine;



            double total = 0;


            if (thePines.Pines != null)
            {
                foreach (var pine in thePines.Pines)
                {
                    printString += Justify((pine.Producto.Suplidor.PadLeft(1, ' ').PadRight(14,' ').TrimStart() + String.Format("{0,8}", pine.StrHora.Replace(" ", "").PadRight(10,' ')) + String.Format("{0,6}", pine.Costo).TrimStart().PadLeft(5,' ').PadRight(8,' ') + String.Format("{0,11}", pine.Serie)),"   ",40) + Environment.NewLine;

                    total += pine.Costo;
                }
                printString += Center("", w) + Environment.NewLine;
                printString += Justify("Venta: ".PadRight(5,' ') + total.ToString("C2") + " en " + thePines.Pines.Count() + " Targetas", "", 20) + Environment.NewLine;

            }
            else
            {
                printString += Center("NO HAY DATA DISPONIBLE", 22) + Environment.NewLine;
            }
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", 20) });
            return j;

        }

        internal static List<string[]> FromReporteListadoDeTickets(MAR_Ganadores theTickets, string loter,IAuthenticator autenticador)
        {
            var j = new List<string[]>();
            string printString = "";


            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanContacto, autenticador.BancaConfiguracion.BancaDto.BanContacto.Length + 7) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length + 7) + Environment.NewLine;
            printString += Center("LISTADO DE TICKETS", 19) + Environment.NewLine;
            printString += Center(
                   FechaHelper.FormatFecha(Convert.ToDateTime(theTickets.Fecha),
                   FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 27) + Environment.NewLine;

            printString += Center("LOTERIA " + loter, 17) + Environment.NewLine + Environment.NewLine;


            int i = 0, validos = 0, nulos = 0;
            double total = 0, vendido = 0;

            if (theTickets.Tickets != null)
            {
                printString += Justify("Tickets       ".PadRight(2,' ') + "  Hora        Vendio         Saco", "", 45) + Environment.NewLine;

                for (int n = 0; n < theTickets.Tickets.Count(); n++)
                {
                    string ThisLinea = String.Empty;
                    ThisLinea = theTickets.Tickets[n].TicketNo.Trim().PadRight(12,' ').PadLeft(0).TrimStart() +
                                 theTickets.Tickets[n].StrHora.Replace(" ", "").PadRight(12, ' ') +
                                 theTickets.Tickets[n].Costo.ToString("N2").PadRight(14,' ');

                    if (theTickets.Tickets[n].Nulo)
                    {
                        ThisLinea += " Nulo".PadLeft(10, ' ');
                        nulos += 1;
                    }
                    else
                    {
                        ThisLinea += theTickets.Tickets[n].Pago.ToString().PadLeft(00);
                        vendido += theTickets.Tickets[n].Costo;
                        total += theTickets.Tickets[n].Pago;
                        validos += 1;
                    }
                    printString += Justify(ThisLinea, " ", 80).PadRight(0) + Environment.NewLine;
                }

                printString += Environment.NewLine;
                printString += Justify("Venta: " + vendido.ToString("C2").PadLeft(2, ' ') + validos.ToString().PadLeft(10, ' ') + " tkts validos", "", 20) + Environment.NewLine;
                printString += Justify("Saco: " + total.ToString("C2").PadLeft(0, ' ') + nulos.ToString().PadLeft(5, ' ') + " tkts nulos", "", 30) + Environment.NewLine;
            }
            else
            {
                printString += Justify("Ningun ticket suyo resulto ganador!", " ", 32).PadRight(0) + Environment.NewLine;
            }
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", 30) });
            return j;
        }

        internal static List<string[]> FromImprimirRecarga(RecargasIndexRecarga recarga)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";

            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;

            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w) + Environment.NewLine;

            printString += Center("Recarga " + recarga.Suplidor, w) + Environment.NewLine;
            printString += Justify("Numero: ".PadRight(0) + recarga.Numero + ("  Monto: " + recarga.Monto.ToString("C0")), "", w) + Environment.NewLine;

            printString += Justify("Serie: ".PadRight(0) + recarga.Serie, "", w) + Environment.NewLine;

            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;

        }

     
        internal static List<string[]> FromListaDeNumeros(VentasIndexTicket pTck,IAuthenticator atenticador)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";

            var theSorteoOculto = !pTck.TicketNo.Contains("-");
            if (theSorteoOculto)
            {
                printString += "-".PadRight(w, '-') + Environment.NewLine;
                printString += Center(pTck.SorteoNombre, w) + Environment.NewLine;
                printString += pTck.TicketNo + Environment.NewLine;
            }
            else
            {
                printString += Center("NO DISPONIBLE", w) + Environment.NewLine;

                printString += Center(pTck.SorteoNombre, w) + Environment.NewLine;
                printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
                printString += Justify(pTck.TicketNo,"NO DISPONIBLE" , w) + Environment.NewLine;
                

            }

            printString += Justify(pTck.TicketNo, "NO DISPONIBLE", w) + Environment.NewLine;

            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> FromPagosRemoto(MAR_Ganadores ganadores, string fecha,IAuthenticator autenticador)
        {
            var j = new List<string[]>();
            string printString = "";
            double total = 0;
            MAR_Session Session;


            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanContacto, autenticador.BancaConfiguracion.BancaDto.BanContacto.Length + 7) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length + 7) + Environment.NewLine;
            printString += Center("TICKETS PAGADOS REMOTAMENTE", 30) + Environment.NewLine;
            printString += Center(
                 FechaHelper.FormatFecha(Convert.ToDateTime(fecha),
                 FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 20) + Environment.NewLine + Environment.NewLine;


            if (ganadores.Tickets.Any())
            {
                printString += Justify("Ticket", "  Hora       Balance       Banca",50) + Environment.NewLine;

                for (int n = 0; n < ganadores.Tickets.Count(); n++)
                {
                    printString += Justify(ganadores.Tickets[n].TicketNo.PadRight(11, ' ') + ganadores.Tickets[n].StrHora.PadRight(8, ' ').Replace(" P","P").Replace(" A","A") + ganadores.Tickets[n].Pago.ToString("N0").PadLeft(10, ' ') +Center(ganadores.Tickets[n].Cedula.Replace("Por","").Replace(" ","").Replace("De","").PadLeft(14,' ').TrimEnd(),14)," ",50) + Environment.NewLine;
                    total += ganadores.Tickets[n].Pago;
                }
                printString += Environment.NewLine + "Balance: " + Math.Round(total).ToString("###,##0").PadLeft(5, ' ') + " en " + ganadores.Tickets.Count() + " tickets" + Environment.NewLine;
                
            }

            printString += Center("No vendio ninguna tarjeta este dia!", 40) + Environment.NewLine;
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", 20) });
            return j;
        }

        internal static List<string[]> FromListaDeNumeros(MAR_VentaNumero ventanum, string fecha, string loter,IAuthenticator autenticador)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "", oldQP = "";
            double TVQ = 0, TCQ = 0, TPQ = 0, TPP = 0, TVP = 0, TVT = 0, TCT = 0, TPT = 0, TCP = 0;
            int m = 0;


            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanContacto, autenticador.BancaConfiguracion.BancaDto.BanContacto.Length + 7) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length + 7) + Environment.NewLine;
            printString += Center("LISTA DE NUMEROS", w) + Environment.NewLine; ;
            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w) + Environment.NewLine + Environment.NewLine;

            printString += Center(loter, w) + Environment.NewLine + Environment.NewLine;
            string Lineas = "";


            if (ventanum.Numeros.Any())
            {
                for (int n = 0; n < ventanum.Numeros.Count(); n++)
                {
                    if (oldQP != ventanum.Numeros[n].QP)
                    {

                        oldQP = ventanum.Numeros[n].QP;
                        if (ventanum.Loteria < 3 || ventanum.Loteria > 4)
                        {
                            if (ventanum.Numeros[n].QP == "Q")
                            {
                                printString += Center("DETALLE DE NUMEROS VENDIDOS", 27) + Environment.NewLine;
                                printString += Center("Num->Cant Num->Cant Num->Cant", 30) + Environment.NewLine;
                            }
                            if (ventanum.Numeros[n].QP == "P")
                            {
                                printString += Environment.NewLine + "------------------------" + Environment.NewLine;
                                if (TVQ > 0)
                                {
                                    printString += "NUMEROS:" + TCQ.ToString("N0").PadLeft(13, ' ') + TPQ.ToString("C0").PadLeft(11, ' ') + Environment.NewLine + Environment.NewLine;
                                    TVQ = 0;
                                }

                                printString += Center("DETALLE DE PALES VENDIDOS", 25) + Environment.NewLine;
                                printString += Center("Pale-Cant Pale-Cant Pale-Cant Pale-Cant", 30) + Environment.NewLine;
                            }
                            if (ventanum.Numeros[n].QP == "T")
                            {
                                printString += Environment.NewLine + "------------------------" + Environment.NewLine;
                                if (TVQ > 0)
                                {
                                    printString += "NUMEROS:" + TCQ.ToString("N0").PadLeft(15, ' ') + TPQ.ToString("C0").PadLeft(13, ' ') + Environment.NewLine + Environment.NewLine;
                                    TVQ = 0;
                                }
                                if (TVP > 0)
                                {
                                    printString += "PALE: " + TVP.ToString("N0").PadLeft(15, ' ') + TPP.ToString("C0").PadLeft(13, ' ') + Environment.NewLine + Environment.NewLine;
                                    TVQ = 0;
                                }

                                printString += Center("DETALLE DE TRIPLETAS VENDIDOS", 29) + Environment.NewLine;
                                printString += Center(" Trip-->Cant  Trip-->Cant  Trip-->Cant", 27) + Environment.NewLine;
                            }
                            m = 0;
                        }
                        else
                        {
                            printString += "3COMBO CANTIDAD   APOSTADO   PAGADO" + Environment.NewLine;
                        }
                    }

                    
                    if (ventanum.Numeros[n].QP == "Q")
                    {
                        int espaciosDeCantidad = 0;
                        //if(ventanum.Numeros[n].Cantidad.ToString().Length())
                        //{
                        //    espac
                        //}
                        Lineas += ventanum.Numeros[n].Numero.ToString().TrimStart()  + ventanum.Numeros[n].Cantidad.ToString();
                        if (n % 3==0 && n > 0) 
                        {
                            printString += Lineas; Lineas = "";
                        }
                        
                        TCQ += ventanum.Numeros[n].Cantidad;
                        TVQ += ventanum.Numeros[n].Costo;
                        TPQ += ventanum.Numeros[n].Pago;
                    }
                    else
                    {
                        if (ventanum.Numeros[n].QP == "P")
                        {
                            printString += ventanum.Numeros[n].Numero.Trim().PadLeft(4, ' ') + ventanum.Numeros[n].Cantidad.ToString().Trim().PadLeft(5, ' ');
                            TCP += ventanum.Numeros[n].Cantidad;
                            TVP += ventanum.Numeros[n].Costo;
                            TPP += ventanum.Numeros[n].Pago;
                        }

                        else
                        {
                            printString += ventanum.Numeros[n].Numero.Trim().PadLeft(6, ' ') + ventanum.Numeros[n].Cantidad.ToString().Trim().PadLeft(6, ' ');
                            TCT += ventanum.Numeros[n].Cantidad;
                            TVT += ventanum.Numeros[n].Costo;
                            TPT += ventanum.Numeros[n].Pago;
                        }
                    }


                    if ((((m + 1) / ((ventanum.Numeros[n].QP == "T") ? 3 : 4)) == (((m + 1) / ((ventanum.Numeros[n].QP == "T") ? 3 : 4)))))
                    {
                        if (n % 3 == 0 && n > 0)
                        {
                            printString += Environment.NewLine;
                        }
                    }
                    else
                    {
                        printString += " ";
                    }
                    m += 1;
                }
                printString += Environment.NewLine + "------------------------" + Environment.NewLine;

                if (TVQ > 0)
                {

                    printString += "NUMEROS:" + TCQ.ToString("N0").PadLeft(12, ' ') + TPQ.ToString("C0").PadLeft(12, ' ') + Environment.NewLine;

                }
                if (TVP > 0)
                {
                    printString += "PALE:   " + TVP.ToString("N0").PadLeft(12, ' ') + TPP.ToString("C0").PadLeft(12, ' ') + Environment.NewLine;

                }
                if (TVT > 0)
                {
                    printString += "TRIPLETA:" + TVT.ToString("N0").PadLeft(10, ' ') + TPT.ToString("C0").PadLeft(12, ' ') + Environment.NewLine;

                }
            }
            else
            {
                printString += Center("No hay numeros vendidos", w) + Environment.NewLine;
            }


            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static string CortarString(string text, int length)
        {
            if (text.Length > length)
            {
                text = text.Substring(0, length);
            }
            else
            {
                for (int i = text.Length; i < 8; i++)
                {
                    text += " ";
                }
            }
            return text;
        }

        internal static List<string[]> FromTicket(VentasIndexTicket pTck,IAuthenticator autenticador, bool pEsCopia = false)
        {
            var j = new List<string[]>();
            
            var theSorteoOculto = !pTck.TicketNo.Contains("-");


            if (theSorteoOculto)
            {
                j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanNombre.ToUpper(), autenticador.BancaConfiguracion.BancaDto.BanNombre.ToUpper().Length) });
                j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion.ToUpper().ToUpper(), autenticador.BancaConfiguracion.BancaDto.BanDireccion.ToUpper().ToUpper().Length) });
                j.Add(new string[] { Center(pTck.SorteoNombre, pTck.SorteoNombre.Length )});
                j.Add(new string[] { "-".PadRight(20, '-')});
                j.Add(new string[] { Center(pTck.SorteoNombre, pTck.SorteoNombre.Length) });
                j.Add(new string[] { Justify (
                MAR.AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(pTck.Fecha),
                MAR.AppLogic.MARHelpers.FechaHelper.FormatoEnum.FechaCortaDOW),
                pTck.Hora, 20 )});
                j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanTelefono, autenticador.BancaConfiguracion.BancaDto.BanTelefono.Length) });
                j.Add(new string[] { pTck.TicketNo});
            }
            else
            {
                j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanNombre.ToUpper(), autenticador.BancaConfiguracion.BancaDto.BanNombre.ToUpper().Length) });
                j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion.ToUpper().ToUpper(), autenticador.BancaConfiguracion.BancaDto.BanDireccion.ToUpper().ToUpper().Length) });
                j.Add(new string[] { Center(pTck.SorteoNombre, pTck.SorteoNombre.Length) });
                j.Add(new string[] { Justify(
                MAR.AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(pTck.Fecha),
                MAR.AppLogic.MARHelpers.FechaHelper.FormatoEnum.FechaCortaDOW),
                pTck.Hora, 20) });
                j.Add(new string[] { Center("Tel."+autenticador.BancaConfiguracion.BancaDto.BanTelefono,("Tel." + autenticador.BancaConfiguracion.BancaDto.BanTelefono).Length) });
                j.Add(new string[] { String.Format("Ticket: " + pTck.TicketNo)});
            }


            if (!string.IsNullOrEmpty(pTck.Pin)) j.Add(new string[] { String.Format("Pin:    " + pTck.Pin)});
            if (!string.IsNullOrEmpty(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterHeader))
            {
                var header = SplitText(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterHeader, autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterHeader.Length);
                for (int i = 0; i < header.Count(); i++)
                {
                    j.Add(new string[] { Center(header[i], 20) });
                }
            }
            if (pEsCopia) j.Add(new string[] { Center("** COPIA REIMPRESA **", 10)});
            j.Add(new string[] { "-".PadRight(10, '-')});
            var theQ = pTck.Jugadas.Where(x => x.Tipo.Equals("Q")).ToArray();
            var theP = pTck.Jugadas.Where(x => x.Tipo.Equals("P")).ToArray();
            var theT = pTck.Jugadas.Where(x => x.Tipo.Equals("T")).ToArray();
            var theG = pTck.Jugadas.Where(x => x.Tipo.Equals("G")).ToArray();
            var theB = pTck.Jugadas.Where(x => x.Tipo.Equals("B")).ToArray();


            for (var i = 0; i < theG.Length; i++)
            {
                if (i == 0) j.Add(new string[] { Center("-- JuegaMas PegaMas --", "-- JuegaMas PegaMas --".Length) });

                j.Add(new string[] { Justify(theG[i].Numero.Trim().PadRight(2, ' '), "$" +
                                             String.Format("{0:0.00}", theG[i].Total),theG[i].Total.ToString().Length)});
            }
            for (var i = 0; i < theB.Length; i++)
            {
                if (i == 0) j.Add(new string[] { Center("-- Billetes --", "-- Billetes --".Length) });

                j.Add(new string[] { Justify(theB[i].Numero.Trim().PadRight(2, ' '), "$" +
                                             String.Format("{0:0.00}", theB[i].Total),theB[i].Total.ToString().Length)});
            }
            for (var i = 0; i < theQ.Length; i++)
            {
                if (i == 0) j.Add(new string[] { "-- NUMEROS --"});

                j.Add(new string[] { Justify(theQ[i].Numero.Trim().PadRight(2, ' '), "$" +
                                             String.Format("{0:0.00}", theQ[i].Precio), theQ[i].Precio.ToString().Length)});
            }

            for (var i = 0; i < theP.Length; i++)
            {
                if (i == 0) j.Add(new string[] { "-- PALES --"});
                j.Add(new string[] { Justify(theP[i].Numero.Trim().Substring(0,2) + "-" +
                                             theP[i].Numero.Trim().Substring(2,2), "$" +
                                             String.Format("{0:0.00}", theP[i].Total), theP[i].Total.ToString().Length)});
            }

            for (var i = 0; i < theT.Length; i++)
            {
                if (i == 0) j.Add(new string[] { "-- TRIPLETAS --"});
                j.Add(new string[] { Justify(theT[i].Numero.Trim().Substring(0,2) + "-" +
                                             theT[i].Numero.Trim().Substring(2,2) + "-" +
                                             theT[i].Numero.Trim().Substring(4,2)   , "$" +
                                             String.Format("{0:0.00}", theT[i].Total), theT[i].Total.ToString().Length) });
            }
            j.Add(new string[] { "-".PadRight(10, '-')});
            var theGrandTotal = pTck.Jugadas.Sum(x => x.Precio);
            j.Add(new string[] { Center(String.Format("TOTAL ${0:0.00}", theGrandTotal),12) });
            if (!string.IsNullOrEmpty(pTck.Firma)) j.Add(new string[] { Center(String.Format("Firma: {0}", pTck.Firma), pTck.Firma.ToString().Length) });
            if (!string.IsNullOrEmpty(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterFooter))
            {
                var footer = SplitText(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterFooter, autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterFooter.Length);
                for (int i = 0; i < footer.Count(); i++)
                {
                    j.Add(new string[] { Center(footer[i], 10) });
                }
            }
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", "-------------------------------".Length) });
            return j;
        }

        internal static List<string[]> FromMultiTicket(List<VentasIndexTicket> pTickets, IAuthenticator autenticador, bool pEsCopia = false)
        {
            var j = new List<string[]>();
            var w = autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterSize;

            j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanNombre.ToUpper(), w), "2" });
            j.Add(new string[] { Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion.ToUpper(), w), "1" });
            j.Add(new string[] { Justify(
                MAR.AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(pTickets[0].Fecha),
                MAR.AppLogic.MARHelpers.FechaHelper.FormatoEnum.FechaCortaDOW),
                pTickets[0].Hora, w), "1" });
            j.Add(new string[] { Center("Tel: " + autenticador.BancaConfiguracion.BancaDto.BanTelefono.ToUpper(), w), "1" });
            if (!string.IsNullOrEmpty(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterHeader)) j.Add(new string[] { Center(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterHeader, w), "1" });
            j.Add(new string[] { "-".PadRight(w, '-'), "1" });
            j.Add(new string[] { Justify("Loteria", " Ticket     Pin  ", w), "1" });

            var theQ = new List<VentasIndexTicket.Jugada>();
            var theP = new List<VentasIndexTicket.Jugada>();
            var theT = new List<VentasIndexTicket.Jugada>();
            Double theGrandTotal = 0;
            for (var i = 0; i < pTickets.Count; i++)
            {
                j.Add(new string[] { Justify(pTickets[i].SorteoNombre.Substring(0, Math.Min(pTickets[i].SorteoNombre.Length, w)), pTickets[i].TicketNo + " " + pTickets[i].Pin, w), "1" });
                var theAddedNums = theQ.Select(x => x.Numero).Distinct().ToList();
                theAddedNums.AddRange(theP.Select(x => x.Numero).Distinct());
                theAddedNums.AddRange(theT.Select(x => x.Numero).Distinct());
                theQ.AddRange(pTickets[i].Jugadas.Where(x => x.Tipo.Equals("Q"))
                                    .Where(x => !theAddedNums.Contains(x.Numero)));
                theP.AddRange(pTickets[i].Jugadas.Where(x => x.Tipo.Equals("P"))
                                    .Where(x => !theAddedNums.Contains(x.Numero)));
                theT.AddRange(pTickets[i].Jugadas.Where(x => x.Tipo.Equals("T"))
                                    .Where(x => !theAddedNums.Contains(x.Numero)));
                theGrandTotal += pTickets[i].Costo;
            }
            if (pEsCopia) j.Add(new string[] { Center("** COPIA REIMPRESA **", w), "1" });
            j.Add(new string[] { "-".PadRight(w, '-'), "1" });

            for (var i = 0; i < theQ.Count; i++)
            {

                if (i == 0) j.Add(new string[] { "-- NUMEROS --", "2" });
                j.Add(new string[] { Justify(theQ[i].Numero.Trim().PadRight(2, ' '), "$" +
                                             String.Format("{0:0.00}", theQ[i].Total),w), "2" });
            }

            for (var i = 0; i < theP.Count; i++)
            {
                if (i == 0) j.Add(new string[] { "-- PALES --", "2" });
                j.Add(new string[] { Justify(theP[i].Numero.Trim().Substring(0,2) + "-" +
                                             theP[i].Numero.Trim().Substring(2,2), "$" +
                                             String.Format("{0:0.00}", theP[i].Total), w), "2" });
            }

            for (var i = 0; i < theT.Count; i++)
            {
                if (i == 0) j.Add(new string[] { "-- TRIPLETAS --", "2" });
                j.Add(new string[] { Justify(theT[i].Numero.Trim().Substring(0,2) + "-" +
                                             theT[i].Numero.Trim().Substring(2,2) + "-" +
                                             theT[i].Numero.Trim().Substring(4,2)   , "$" +
                                             String.Format("{0:0.00}", theT[i].Total), w), "2" });
            }
            j.Add(new string[] { "-".PadRight(w, '-'), "2" });
            j.Add(new string[] { Center(String.Format("TOTAL ${0:0.00}", theGrandTotal), w), "2" });
            if (!string.IsNullOrEmpty(pTickets[0].Firma)) j.Add(new string[] { Center(String.Format("Firma: {0}", pTickets[0].Firma), w), "1" });
            if (!string.IsNullOrEmpty(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterFooter)) j.Add(new string[] { Center(autenticador.CurrentAccount.MAR_Setting2.Sesion.PrinterFooter, w), "1" });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> FromImprimirRecarga(RecargasIndexRecarga recarga, IAuthenticator autenticador)
        {
            var j = new List<string[]>();
        
            string printString = "";

            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanNombre.ToUpper(), autenticador.BancaConfiguracion.BancaDto.BanNombre.ToUpper().Length) + Environment.NewLine;

            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion.ToUpper(), autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length) + Environment.NewLine;
            printString += Center(
                MAR.AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                MAR.AppLogic.MARHelpers.FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 35) + Environment.NewLine;

            printString += Center("Recarga ", "Recarga".Length) + Environment.NewLine;
            printString += Environment.NewLine;
           printString += Justify("Numero: ".PadRight(0) + recarga.Numero + ("  Monto: " + recarga.Monto.ToString("C0")), "", 30) + Environment.NewLine;
            printString += Environment.NewLine;
            printString += Justify("Serie: ".PadRight(0) + recarga.Serie, "", recarga.Serie.Length) + Environment.NewLine;

            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", "-------------------------------".Length) });
            return j;

        }

        internal static List<string[]> FromPagoGanador(string mensaje, int aprobado, IAuthenticator autenticador)
        {
            var j = new List<string[]>();
           string printString = "";

            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanNombre, autenticador.BancaConfiguracion.BancaDto.BanNombre.Length) + Environment.NewLine;
            printString += Center(autenticador.BancaConfiguracion.BancaDto.BanDireccion, autenticador.BancaConfiguracion.BancaDto.BanDireccion.Length) + Environment.NewLine;
            printString +=  Environment.NewLine;
            printString +=  Environment.NewLine;
            printString += Center(
               MAR.AppLogic.MARHelpers.FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
               MAR.AppLogic.MARHelpers.FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 35) + Environment.NewLine;

            printString += Justify(mensaje, "", mensaje.Length) + Environment.NewLine;
            printString += Center("Aprobacion: " + aprobado,30) + Environment.NewLine;
            printString += Environment.NewLine;
            printString += Center(DateTime.Now.ToString("dd-MMM-yyyy     hh:mm:ss tt"),30) + Environment.NewLine;
            j.Add(new string[] { printString });
           
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", "-------------------------------".Length) });
            return j;
        }


    }
}

