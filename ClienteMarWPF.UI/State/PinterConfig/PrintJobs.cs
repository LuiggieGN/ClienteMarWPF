using MAR.AppLogic.MARHelpers;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;

namespace ClienteMarWPF.UI.State.PinterConfig
{
    class PrintJobs
    {
        internal static List<string[]> FromReporteDeGanadores(MAR_Ganadores ganadores,ReportesIndexGanadores reporte)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";

            printString += Center("BANCA NO DISPONIBLE".ToUpper(), w) + Environment.NewLine;
            printString += Center("DIRECCION NO DISPONIBLE", w) + Environment.NewLine;
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
                if (lastGroup == 3) printString += Justify("", "$" + total_n.ToString("N2"), w) + rt;
                if (lastGroup == 4) printString += Justify("Pendientes por pagar", "$" + total_o.ToString("N2"), w) + rt;
                if (lastGroup == 5) printString += Justify("", "$" + total_s.ToString("N2"), w) + rt;
                if (lastGroup == 6) printString += Justify("", "$" + total_r.ToString("N2"), w) + rt;
                printString += rt;

                printString += Justify("Balance Ganadores", (total_n + total_s + total_o - total_r).ToString("N2"), w) + rt;
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

        internal static List<string[]> FromReporteSumaVenta(MAR_RptSumaVta theSumaVenta)
        {
            MAR_Session session = new MAR_Session();
            var j = new List<string[]>();
            var jd = new List<string>();
            var w =  20 ;

            j.Add(new string[] { Center("SUMA DE VENTAS", w) });
            j.Add(new string[] { Center("NO DISPONIBLE", w) });
            j.Add(new string[] { Center("NO DISPONIBLE", w) });
            j.Add(new string[] { Center("FECHA DEL IMPRESION", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w)});
            j.Add(new string[] { Center("FECHA DEL REPORTE", w) });
            j.Add(new string[] { Center(
               FechaHelper.FormatFecha(Convert.ToDateTime(theSumaVenta.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW).Trim(), w)});

            j.Add(new string[] { Center("Concepto ".PadRight(0) + "Venta Comis. Saco Balan.", w) });

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
                                    CortarString(rgn.Reglon, 7).PadRight(rgn.Reglon.Length,' ') + " " + rgn.VentaBruta.ToString("N0").PadLeft(6) + rgn.Comision.ToString().PadLeft(6)
                                                 + rgn.Saco.ToString().PadLeft(6) + rgn.Resultado.ToString().PadLeft(6)
                                });
                    }

                comision += rgn.Comision;
                venta += rgn.VentaBruta;
                resultado += rgn.Resultado;
                saco += rgn.Saco;
            }

            j.Add(new string[] { Center("-------------------------------", w) });
            j.Add(new string[]
                    {
                       "Total=> "+ " " +  Math.Round(venta, 0).ToString().PadLeft(6) + comision.ToString().PadLeft(5)
                     + saco.ToString().PadLeft(6) + resultado.ToString().PadLeft(6)
                    });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> FromReporteVentaPorFecha(MAR_RptSumaVta2 venta, string fDes, string fHas, bool resumido)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";

            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("VENTAS POR FECHA", w) + Environment.NewLine;

            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(DateTime.Now),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t") + Environment.NewLine, w) + Environment.NewLine;



            printString += Justify("Desde:", FechaHelper.FormatFecha(Convert.ToDateTime(fDes),
                FechaHelper.FormatoEnum.FechaCortaDOW), w);

            printString += Justify("Hasta:", FechaHelper.FormatFecha(Convert.ToDateTime(fHas),
             FechaHelper.FormatoEnum.FechaCortaDOW), w) + Environment.NewLine + Environment.NewLine;

            // Mar_ReglonVta rg = new Mar_ReglonVta();
            double tCom = 0, tVen = 0, tRes = 0, tSac = 0, stCom = 0, stVen = 0, stRes = 0, stSac = 0;
            string oldCon = "OLDSTRING";
            string Fec = "";

            printString += "FECHA   VENTA COMIS. SACO   BAL." + Environment.NewLine;

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
                            printString += ("SUMA==>" + stVen.ToString("N0").PadLeft(7, ' ') + stCom.ToString("N0").PadLeft(5, ' ') + stSac.ToString("N0").PadLeft(6, ' ') + stRes.ToString("N0").PadLeft(7, ' ')) + Environment.NewLine + Environment.NewLine;
                            stCom = 0;
                            stVen = 0;
                            stRes = 0;
                            stSac = 0;
                        }
                        printString += Justify("CONCEPTO: ", rg.Reglon, w) + Environment.NewLine;
                        oldCon = rg.Reglon;
                    }
                    Fec = Justify(FechaHelper.FormatFecha(Convert.ToDateTime(rg.Fecha), FechaHelper.FormatoEnum.FechaCortaRegional), "", w);

                    //   Fec = Strings.Left(FormatFecha(rg.Fecha, 3), 2) & " " & Strings.Mid(FormatFecha(rg.Fecha, 3), InStr(FormatFecha(rg.Fecha, 3), ",") + 2, 6).Replace("-", "")

                    if (!resumido)
                    {
                        printString += Justify(Fec.PadLeft(15, ' '), rg.VentaBruta.ToString("N0").PadLeft(6, ' ') + rg.Comision.ToString("N0").PadLeft(5, ' ') + rg.Saco.ToString("N0").PadLeft(6, ' ') + rg.Resultado.ToString("N0").PadLeft(7, ' '), w) + Environment.NewLine;
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

            printString += ("SUMA==>" + stVen.ToString("N0").PadLeft(7, ' ') + stCom.ToString("N0").PadLeft(5, ' ') + stSac.ToString("N0").PadLeft(6, ' ') + stRes.ToString("N0").PadLeft(7, ' ')) + Environment.NewLine;
            printString += Justify("-------------------------------", "", w);
            printString += ("TOTAL=>" + tVen.ToString("N0").PadLeft(6, ' ') + tCom.ToString("N0").PadLeft(5, ' ') + tSac.ToString("N0").PadLeft(6, ' ') + tRes.ToString("N0").PadLeft(7, ' ')) + Environment.NewLine;

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

        internal static List<string[]> FromReporteVenta(MAR_RptVenta venta, string sorteo)
        {
            var j = new List<string[]>();
            var w = 14;
            string printString = "", loter = "";
            printString += Center("NO DISPONIBLE", 13) + Environment.NewLine;


            printString += Center("NO DISPONIBLE", 13) + Environment.NewLine;
            printString += Center("REPORTE DE VENTA", 17) + Environment.NewLine;
            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(venta.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), 27) + Environment.NewLine;

            printString += Center("Loteria: "+sorteo, 19) + Environment.NewLine;




            if (venta.Loteria < 3 || venta.Loteria > 4)
            {
                printString += Justify("NUMEROS:", venta.CntNumeros.ToString("N0"), 27) + Environment.NewLine;

                printString += Justify("NUMEROS (RD$):", Math.Round(venta.Numeros, 2).ToString("C2"), 27) + Environment.NewLine;
                printString += Justify("PALES:", Math.Round(venta.Pales, 2).ToString("C2"), 27) + Environment.NewLine;
                printString += Justify("TRIPLETAS:", Math.Round(venta.Tripletas, 2).ToString("C2"), 27) + Environment.NewLine;
            }
            else
            {
                printString += Justify("Pega3 COMBO:", venta.Numeros.ToString("C2"), 27) + Environment.NewLine;
                printString += Justify("Pega3 FIJO", Math.Round(venta.Pales, 2).ToString("C2"), 27) + Environment.NewLine;
            }


            printString += Justify("TOTAL VENTA ===>", Math.Round(venta.Numeros + venta.Pales + venta.Tripletas, 2).ToString("C2"), 27) + Environment.NewLine;

            printString += Justify("COMISION (" + Math.Round((venta.ComisionPorcQ + venta.ComisionPorcP + venta.ComisionPorcT) / 3, 2).ToString() + "%):", Math.Round(venta.Comision, 0).ToString("C0"), 27) + Environment.NewLine;


            printString += Justify("VENTA NETA ===>", (Math.Round(venta.Numeros + venta.Pales + venta.Tripletas, 0) - venta.Comision).ToString("C2"), 27) + Environment.NewLine + Environment.NewLine; ;


            if (venta.Primero.Trim() != "")
            {
                printString += Justify("PREMIOS      CANTIDAD", "GANA", w) + Environment.NewLine;

                if (venta.Loteria < 3 || venta.Loteria > 4)
                {
                    printString += ("1ra. " + venta.Primero + venta.CPrimero.ToString("N2").PadLeft(11, ' ') + venta.MPrimero.ToString("C0").PadLeft(13, ' ')) + Environment.NewLine;
                    printString += ("2da. " + venta.Segundo + venta.CSegundo.ToString("N2").PadLeft(11, ' ') + venta.MSegundo.ToString("C0").PadLeft(13, ' ')) + Environment.NewLine;
                    printString += ("3ra. " + venta.Tercero + venta.CTercero.ToString("N2").PadLeft(11, ' ') + venta.MTercero.ToString("C0").PadLeft(13, ' ')) + Environment.NewLine + Environment.NewLine;
                    printString += ("NUMEROS PREMIADOS:" + Math.Round(venta.MTercero + venta.MPrimero + venta.MSegundo, 0).ToString("C2").PadLeft(14, ' ')) + Environment.NewLine;
                    printString += ("PALES PREMIADOS:  " + Math.Round(venta.MPales, 0).ToString("C2").PadLeft(14, ' ')) + Environment.NewLine;
                    printString += ("TRIPLETA PREMIADA:" + Math.Round(venta.MTripletas, 0).ToString("C2").PadLeft(14, ' ')) + Environment.NewLine;
                    printString += ("TOTAL PREMIADOS ==>" + Math.Round(venta.MTercero + venta.MPrimero + venta.MSegundo + venta.MPales + venta.MTripletas, 0).ToString("C2").PadLeft(13, ' ')) + Environment.NewLine;
                }
                else
                {
                    printString += Justify("PREMIOS PEGA3:", (venta.Primero + "-" + venta.Segundo + "-" + venta.Tercero).ToString(), w) + Environment.NewLine;
                    printString += Justify("COMBOS PREMIADOS:", Math.Round(venta.MPrimero, 0).ToString("C2"), w) + Environment.NewLine;
                    printString += Justify("FIJOS PREMIADOS: ", Math.Round(venta.MPales, 0).ToString("C2"), w) + Environment.NewLine;
                    printString += Justify("TOTAL PREMIADOS: ", Math.Round(venta.MTercero + venta.MPrimero + venta.MSegundo + venta.MPales + venta.MTripletas, 0).ToString("C2"), w) + Environment.NewLine;
                }


                double GP = Math.Round(venta.Numeros + venta.Pales + venta.Tripletas, 0) - (venta.Comision + venta.MTercero + venta.MPrimero + venta.MSegundo + venta.MPales + venta.MTripletas);


                if (GP < 0)
                {
                    printString += Justify("PERDIDA  ===> ", Math.Round(GP, 0).ToString("C2"), w) + Environment.NewLine;
                }
                else
                {
                    printString += Justify("GANANCIA ===> ", Math.Round(GP, 0).ToString("C2"), w) + Environment.NewLine;
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


        internal static List<string[]> FromReporteListadoDePines(MAR_Pines thePines)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";

            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("LISTADO DE PINES", w) + Environment.NewLine;
            printString += Center(
                FechaHelper.FormatFecha(Convert.ToDateTime(thePines.Fecha),
                FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w) + Environment.NewLine + Environment.NewLine;


            printString += Justify("Suplidor ".PadRight(0) + "Hora  Precio   Serie", "", w) + Environment.NewLine;



            double total = 0;


            if (thePines.Pines != null)
            {
                foreach (var pine in thePines.Pines)
                {
                    printString += (pine.Producto.Suplidor.PadRight(0) + String.Format("{0,8}", pine.StrHora.Replace(" ", "")) + String.Format("{0,6}", pine.Costo).PadRight(0) + String.Format("{0,11}", pine.Serie).PadLeft(0)) + Environment.NewLine;

                    total += pine.Costo;
                }
                printString += Center("", w) + Environment.NewLine;
                printString += Justify("Venta: ".PadRight(0) + total.ToString("C2") + " en " + thePines.Pines.Count() + " Targetas", "", w) + Environment.NewLine;

            }
            else
            {
                printString += Center("NO HAY DATA DISPONIBLE", w) + Environment.NewLine;
            }
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;

        }

        internal static List<string[]> FromReporteListadoDeTickets(MAR_Ganadores theTickets, string loter)
        {
            var j = new List<string[]>();
            var w = 20;
            string printString = "";

            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("LISTADO DE TICKETS", w) + Environment.NewLine;
            printString += Center(
                   FechaHelper.FormatFecha(Convert.ToDateTime(theTickets.Fecha),
                   FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w) + Environment.NewLine;

            printString += Center("LOTERIA " + loter, w) + Environment.NewLine;


            int i = 0, validos = 0, nulos = 0;
            double total = 0, vendido = 0;

            if (theTickets.Tickets != null)
            {
                printString += Justify("Tickets    ".PadRight(0) + "Hora  Vendido   Saco", "", 10) + Environment.NewLine;



                for (int n = 0; n < theTickets.Tickets.Count(); n++)
                {
                    string ThisLinea = String.Empty;
                    ThisLinea = theTickets.Tickets[n].TicketNo.Trim().PadRight(14,' ') +
                                 theTickets.Tickets[n].StrHora.Replace(" ", "").PadRight(14, ' ') +
                                 theTickets.Tickets[n].Costo.ToString("N2");

                    if (theTickets.Tickets[n].Nulo)
                    {
                        ThisLinea += " Nulo".PadLeft(6, ' ');
                        nulos += 1;
                    }
                    else
                    {
                        ThisLinea += theTickets.Tickets[n].Pago.ToString().PadLeft(5);
                        vendido += theTickets.Tickets[n].Costo;
                        total += theTickets.Tickets[n].Pago;
                        validos += 1;
                    }
                    printString += Justify(ThisLinea, " ", w).PadRight(0) + Environment.NewLine;
                }

                printString += Environment.NewLine;
                printString += Justify("Venta: " + vendido.ToString("C2").PadLeft(0, ' ') + validos.ToString().PadLeft(3, ' ') + " tkts validos", "", w) + Environment.NewLine;
                printString += Justify("Saco: " + total.ToString("C2").PadLeft(0, ' ') + nulos.ToString().PadLeft(5, ' ') + " tkts nulos", "", w) + Environment.NewLine;
            }
            else
            {
                printString += Justify("Ningun ticket suyo resulto ganador!", " ", w).PadRight(0) + Environment.NewLine;
            }
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
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

     
        internal static List<string[]> FromListaDeNumeros(VentasIndexTicket pTck)
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

        internal static List<string[]> FromPagosRemoto(MAR_Ganadores ganadores, string fecha)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "";
            double total = 0;
            MAR_Session Session;

            printString += Center("NO DISPONIBLE".ToUpper() + "-" + "NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("NO DISPONIBLE", w) + Environment.NewLine;
            printString += Center("TICKETS PAGADOS REMOTAMENTE", w) + Environment.NewLine;
            printString += Center(
                 FechaHelper.FormatFecha(Convert.ToDateTime(fecha),
                 FechaHelper.FormatoEnum.FechaCortaDOW) + " " + DateTime.Now.ToString("t"), w) + Environment.NewLine;


            if (ganadores.Tickets.Any())
            {
                printString += Justify("Ticket", "   Hora   Balance   Banca", w) + Environment.NewLine;

                for (int n = 0; n < ganadores.Tickets.Count(); n++)
                {
                    printString += ganadores.Tickets[n].TicketNo.PadRight(11, ' ') + ganadores.Tickets[n].StrHora.PadRight(7, ' ') + ganadores.Tickets[n].Pago.ToString("N0").PadRight(7, ' ') + ganadores.Tickets[n].Cedula.PadRight(12, ' ') + Environment.NewLine;
                    total += ganadores.Tickets[n].Pago;
                }

                printString += Environment.NewLine + "Balance: " + Math.Round(total).ToString("###,##0").PadLeft(10, ' ') + " en " + ganadores.Tickets.Count() + " tickets" + Environment.NewLine;
            }

            printString += Center("No vendio ninguna tarjeta este dia!", w) + Environment.NewLine;
            j.Add(new string[] { printString });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { " " });
            j.Add(new string[] { Center("-------------------------------", w) });
            return j;
        }

        internal static List<string[]> FromListaDeNumeros(MAR_VentaNumero ventanum, string fecha, string loter)
        {
            var j = new List<string[]>();
            var w = 35;
            string printString = "", oldQP = "";
            double TVQ = 0, TCQ = 0, TPQ = 0, TPP = 0, TVP = 0, TVT = 0, TCT = 0, TPT = 0, TCP = 0;
            int m = 0;


            printString += (Center("NO DISPONIBLE".ToUpper() + "-" + "NO DISPONIBLE".ToUpper(), w)) + Environment.NewLine + Environment.NewLine;
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

    }
}

