using ClienteMarWPFWin7.Domain.Services.JuegaMasService;
using ClienteMarWPFWin7.Domain.Services.ReportesService;
using ClienteMarWPFWin7.UI.Modules.Reporte;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Linq;
using ClienteMarWPFWin7.Data;
using System.Collections.ObjectModel;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Reportes
{
    class PrintReports : ActionCommand
    {

        private readonly ReporteViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IReportesServices ReportesService;
        private readonly IJuegaMasService servicioJuegamas;
        DateTime FechaInicio;
        DateTime FechaFin;

        public PrintReports(ReporteViewModel viewModel, IAuthenticator autenticador, IReportesServices reportesServices, IJuegaMasService juegaMasService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            ReportesService = reportesServices;
            servicioJuegamas = juegaMasService;

            DateTime fInicio;
            DateTime fFin;

            bool fInicioEsValido = DateTime.TryParse(ViewModel.FechaInicio.ToString(), CultureInfo.CreateSpecificCulture("es-DO"), DateTimeStyles.None, out fInicio);
            bool fFinEsValido = DateTime.TryParse(ViewModel.FechaFin.ToString(), CultureInfo.CreateSpecificCulture("es-DO"), DateTimeStyles.None, out fFin);

            if (!fInicioEsValido || !fFinEsValido)
            {
                fInicio = DateTime.MinValue;
                fFin = DateTime.MinValue;
                return;
            }

            if (DateTime.Compare(fFin, fInicio) < 0)
            {//fecha fin es  menor que  fecha inicio

                var swap = fInicio;
                fInicio = fFin;
                fFin = swap;
            }

            FechaInicio = fInicio;
            FechaFin = fFin;

            Action<object> comando = new Action<object>(PrinterReportes);
            base.SetAction(comando);

        }
        private void PrinterReportes(object parametro)
        {


            try
            {
                var nombre = new ReporteView().GetReporteNombre();
                if (nombre == null || nombre == "") { nombre = "Reportes De Ventas"; }
                if (nombre == "Suma De Ventas")
                {
                    PrintSumaDeVentas(parametro);
                }
                else if (nombre == "Reportes Ganadores")
                {
                    PrintReporteGanadores(parametro);
                }
                else if (nombre == "Ventas por Fecha")
                {
                    PrintVentasFecha(parametro);
                }
                else if (nombre == "Lista De Tarjetas")
                {
                    PrintListaTarjetas(parametro);
                }
                else if (nombre == "Lista De Premios")
                {
                    PrintListdoPremio(parametro);
                }
                else if (nombre == "Lista De Numeros")
                {
                    PrintListaNumeros(parametro);
                }
                else if (nombre == "Reportes De Ventas")
                {
                    PrintVentas(parametro);

                }
                else if (nombre == "Lista De Tickets")
                {
                    PrintListaTicket(parametro);
                }
                else if (nombre == "Pagos Remotos")
                {
                    PrintPagosRemotos(parametro);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void PrintSumaDeVentas(object parametro)
        {
            try
            {
                var Reporte = ReportesService.ReporteSumVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
                
                var MoreOptions = Autenticador.CurrentAccount.MAR_Setting2.MoreOptions;
                /*if (MoreOptions.Count() > 1)
                {
                    MAR_RptSumaVta ventasprint = new MAR_RptSumaVta() { Dia = Reporte.Dia, Err = Reporte.Err, Hora = Reporte.Hora, Fecha = Reporte.Fecha, Reglones = Reporte.Reglones };
                    List<string[]> SumaDeVentas = PrintJobs.FromReporteSumaVenta(ventasprint, Autenticador);
                    ReporteTemplateHelper.PrintReporte(ventasprint, Autenticador, ViewModel);
                }else {*/
                //List<string[]> ImprimirSumVenta = PrintJobs.FromReporteSumaVenta(ventasprint, Autenticador);
                    string[] array = new string[5];
                    string[] arrayHeader = new string[5];
                    string[] arrayTotals = new string[0];

                    int totalVenta = 0; int totalComision = 0; int totalSaco = 0; int totalBalance = 0;

                    List<string[]> ListArray = new List<string[]>();

                    arrayHeader.SetValue("Concp.", 0); arrayHeader.SetValue("Venta", 1); arrayHeader.SetValue("Comis.", 2); arrayHeader.SetValue("Saco", 3); arrayHeader.SetValue("Balan.", 4);

                    for (var i=0;i < Reporte.Reglones.Length;i++)
                    {
                    array = new string[5];
                    if (Reporte.Reglones[i].Reglon.Length > 8 && Reporte.Reglones[i].Reglon.Contains(" ")) {
                        var Division = Reporte.Reglones[i].Reglon.Split(' ');
                        var nomPart1 = Division[0].Substring(0, 3);
                        var nomPart2 = Division[1].Substring(0, 3);
                        array.SetValue(nomPart1+nomPart2.ToString(), 0);
                    }
                    else
                    {
                        array.SetValue(Reporte.Reglones[i].Reglon.Trim(), 0);
                    }
                     array.SetValue(Reporte.Reglones[i].VentaBruta.ToString("N"), 1); array.SetValue(Reporte.Reglones[i].Comision.ToString("N"), 2); array.SetValue(Reporte.Reglones[i].Saco.ToString("N"), 3); array.SetValue(Reporte.Reglones[i].Resultado.ToString("N"), 4); ListArray.Add(array);
                        array = new string[5];
                        totalVenta += Convert.ToInt32(Reporte.Reglones[i].VentaBruta); totalComision += Convert.ToInt32(Reporte.Reglones[i].Comision); totalSaco += Convert.ToInt32(Reporte.Reglones[i].Saco); totalBalance += Convert.ToInt32(Reporte.Reglones[i].Resultado);
                    }
                array = new string[5];
                array.SetValue("---------", 0);
                array.SetValue("---------", 1);
                array.SetValue("---------", 2);
                array.SetValue("---------", 3);
                array.SetValue("---------", 4);
                ListArray.Add(array);
                array = new string[5];
                array.SetValue("Total", 0); array.SetValue(totalVenta.ToString("N"), 1); array.SetValue(totalComision.ToString("N"), 2); array.SetValue(totalSaco.ToString("N"), 3); array.SetValue(totalBalance.ToString("N"), 4); ListArray.Add(array);
              
                //arrayTotals.SetValue("Total", 0); arrayTotals.SetValue(totalVenta.ToString(), 1); arrayTotals.SetValue(totalComision.ToString(), 2); arrayTotals.SetValue(totalSaco.ToString(), 3); arrayTotals.SetValue(totalBalance.ToString(), 4);
                    ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "SUMA DE VENTAS", Data = ListArray, FechaReporte = Convert.ToDateTime(Reporte.Fecha), Headers = arrayHeader, Totals = arrayTotals };
                    List<string[,]> ImprimirSumVentaGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador);
                    ReporteTemplateHelper.PrintReporte(ImprimirSumVentaGeneral, Autenticador, ViewModel);
                /*}*/
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void PrintVentasFecha(object parametro)
        {
            MAR_RptSumaVta2 ventasfechaprint = new MAR_RptSumaVta2() { };
            var Reporte = ReportesService.ReporteVentasPorFecha(Autenticador.CurrentAccount.MAR_Setting2.Sesion, FechaInicio, ViewModel.FechaFin);
            List<string[]> ImprimirSumVentaFecha = new List<string[]>() { };
            if (ViewModel.SoloTotales == true)
            {
                string[] array = new string[5];
                string[] arrayHeader = new string[5];
                string[] arrayTotals = new string[0];
                List<string[]> ListArray = new List<string[]>();

                arrayHeader.SetValue("Fecha", 0); arrayHeader.SetValue("Venta", 1); arrayHeader.SetValue("Comision", 2); arrayHeader.SetValue("Saco", 3); arrayHeader.SetValue("Balance", 4);
                array = new string[5];
                var datasAgrupadas = Reporte.Reglones.GroupBy(x => x.Reglon);
                for (var i=0;i < datasAgrupadas.Count();i++)
                {
                    array = new string[5];
                    array.SetValue("--------", 0);
                    array.SetValue("--------", 1);
                    array.SetValue("--------", 2);
                    array.SetValue("--------", 3);
                    array.SetValue("--------", 4);
                    ListArray.Add(array);
                    array = new string[1];
                        array.SetValue(datasAgrupadas.ToArray()[i].Key.ToString(),0);
                        ListArray.Add(array);
                    
                    array = new string[5];
                    var dataLoteria = Reporte.Reglones.Where(x => x.Reglon == datasAgrupadas.ToArray()[i].Key);
                        array.SetValue("Total", 0);
                        array.SetValue(dataLoteria.Sum(x => x.VentaBruta).ToString("N"), 1);
                        array.SetValue(dataLoteria.Sum(x => x.Comision).ToString("N"), 2);
                        array.SetValue(dataLoteria.Sum(x => x.Saco).ToString("N"), 3);
                        array.SetValue(dataLoteria.Sum(x => x.Resultado).ToString("N"), 4);
                        ListArray.Add(array);
                        array = new string[5];
                }
                array = new string[5];
                array.SetValue("--------", 0);
                array.SetValue("--------", 1);
                array.SetValue("--------", 2);
                array.SetValue("--------", 3);
                array.SetValue("--------", 4);
                ListArray.Add(array);
                array = new string[5];
                array.SetValue("Totales", 0);
                array.SetValue(Reporte.Reglones.Sum(x => x.VentaBruta).ToString("N"), 1);
                array.SetValue(Reporte.Reglones.Sum(x => x.Comision).ToString("N"), 2);
                array.SetValue(Reporte.Reglones.Sum(x => x.Saco).ToString("N"), 3);
                array.SetValue(Reporte.Reglones.Sum(x => x.Resultado).ToString("N"), 4);
                ListArray.Add(array);
                ventasfechaprint = new MAR_RptSumaVta2() { Dia = Reporte.Dia, Err = Reporte.Err, Hora = Reporte.Hora, Fecha = Reporte.Fecha, Reglones = Reporte.Reglones, ISRRetenido = Reporte.ISRRetenido, RifDescuento = Reporte.RifDescuento };
                //ImprimirSumVentaFecha = PrintJobs.FromReporteVentaPorFecha(ventasfechaprint, ViewModel.FechaInicio.ToString(), ViewModel.FechaFin.ToString(), ViewModel.SoloTotales, Autenticador);
                ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "VENTAS POR FECHA", Data = ListArray, FechaReporte = Convert.ToDateTime(Reporte.Fecha), Headers = arrayHeader, Totals = arrayTotals,Desde=FechaInicio,Hasta=ViewModel.FechaFin };
                List<string[,]> ImprimirSumVentaGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador,true);
                ReporteTemplateHelper.PrintReporte(ImprimirSumVentaGeneral, Autenticador, ViewModel);
            }
            else if (ViewModel.SoloTotales == false)
            {
                string[] array = new string[5];
                string[] arrayHeader = new string[5];
                string[] arrayTotals = new string[0];
                List<string[]> ListArray = new List<string[]>();
                array = new string[5];
                var datasAgrupadas = Reporte.Reglones.GroupBy(x => x.Reglon);
                for (var i = 0; i < datasAgrupadas.Count(); i++)
                {
                    array = new string[5];
                    array.SetValue("--------", 0);
                    array.SetValue("--------", 1);
                    array.SetValue("--------", 2);
                    array.SetValue("--------", 3);
                    array.SetValue("--------", 4);
                    ListArray.Add(array);
                    array = new string[1];
                    array.SetValue(datasAgrupadas.ToArray()[i].Key.ToString(), 0);
                    ListArray.Add(array);
                    array = new string[5];
                    array.SetValue("--------", 0);
                    array.SetValue("--------", 1);
                    array.SetValue("--------", 2);
                    array.SetValue("--------", 3);
                    array.SetValue("--------", 4);
                    ListArray.Add(array);
                    array = new string[5];

                    var dataLoteria = Reporte.Reglones.Where(x => x.Reglon == datasAgrupadas.ToArray()[i].Key);
                    foreach (var data in dataLoteria)
                    {
                        array.SetValue(Convert.ToDateTime(data.Fecha).ToString("dd/M/yy"), 0);
                        array.SetValue(data.VentaBruta.ToString("N"), 1);
                        array.SetValue(data.Comision.ToString("N"), 2);
                        array.SetValue(data.Saco.ToString("N"), 3);
                        array.SetValue(data.Resultado.ToString("N"), 4);
                        ListArray.Add(array);
                        array = new string[5];
                    }
                    array.SetValue("Total", 0);
                    array.SetValue(dataLoteria.Sum(x => x.VentaBruta).ToString("N"), 1);
                    array.SetValue(dataLoteria.Sum(x => x.Comision).ToString("N"), 2);
                    array.SetValue(dataLoteria.Sum(x => x.Saco).ToString("N"), 3);
                    array.SetValue(dataLoteria.Sum(x => x.Resultado).ToString("N"), 4);
                    ListArray.Add(array);
                    array = new string[5];
                }
                array = new string[5];
                array.SetValue("---------", 0);
                array.SetValue("---------", 1);
                array.SetValue("---------", 2);
                array.SetValue("---------", 3);
                array.SetValue("---------", 4);
                ListArray.Add(array);
                array = new string[5];
                array.SetValue("Totales", 0);
                array.SetValue(Reporte.Reglones.Sum(x => x.VentaBruta).ToString("N"), 1);
                array.SetValue(Reporte.Reglones.Sum(x => x.Comision).ToString("N"), 2);
                array.SetValue(Reporte.Reglones.Sum(x => x.Saco).ToString("N"), 3);
                array.SetValue(Reporte.Reglones.Sum(x => x.Resultado).ToString("N"), 4);
                ListArray.Add(array);

                array = new string[1];
                array.SetValue(".", 0);
                ListArray.Add(array);
                array = new string[1];
                array.SetValue(".", 0);
                ListArray.Add(array);

                ventasfechaprint = new MAR_RptSumaVta2() { Dia = Reporte.Dia, Err = Reporte.Err, Hora = Reporte.Hora, Fecha = Reporte.Fecha, Reglones = Reporte.Reglones, ISRRetenido = Reporte.ISRRetenido, RifDescuento = Reporte.RifDescuento };

                arrayHeader.SetValue("Fecha", 0); arrayHeader.SetValue("Venta", 1); arrayHeader.SetValue("Comision", 2); arrayHeader.SetValue("Saco", 3); arrayHeader.SetValue("Balance", 4);
                ventasfechaprint = new MAR_RptSumaVta2() { Dia = Reporte.Dia, Err = Reporte.Err, Hora = Reporte.Hora, Fecha = Reporte.Fecha, Reglones = Reporte.Reglones, ISRRetenido = Reporte.ISRRetenido, RifDescuento = Reporte.RifDescuento };
                ImprimirSumVentaFecha = PrintJobs.FromReporteVentaPorFecha(ventasfechaprint, ViewModel.FechaInicio.ToString(), ViewModel.FechaFin.ToString(), ViewModel.SoloTotales, Autenticador);
                ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "VENTAS POR FECHA", Data = ListArray, FechaReporte = Convert.ToDateTime(Reporte.Fecha), Headers = arrayHeader, Totals = arrayTotals,Desde=FechaInicio,Hasta=ViewModel.FechaFin };
                List<string[,]> ImprimirSumVentaGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador,true);
                ReporteTemplateHelper.PrintReporte(ImprimirSumVentaGeneral, Autenticador, ViewModel);
            }
                
            //ReporteTemplateHelper.PrintReporte(ImprimirSumVentaFecha, Autenticador, ViewModel, false, false, ViewModel.SoloTotales);
        }
        private void PrintReporteGanadores(object Parametro)
        {

            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var Reporte = ReportesService.ReportesGanadores(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            var MoreOptions = Autenticador.CurrentAccount.MAR_Setting2.MoreOptions.ToList();
            /*if (MoreOptions.Any())
            {
                MAR_Ganadores ModelMarGanadores = new MAR_Ganadores() { Dia = Reporte.Dia, Err = Reporte.Err, Fecha = Reporte.Fecha, Hora = Reporte.Hora, Primero = Reporte.Primero, Segundo = Reporte.Segundo, Tercero = Reporte.Tercero, Tickets = Reporte.Tickets };

                ReporteTemplateHelper.PrintReporte(ModelMarGanadores, Autenticador, ViewModel);
            }
            else
            {*/
                MAR_Ganadores ModelMarGanadores = new MAR_Ganadores() { Dia = Reporte.Dia, Err = Reporte.Err, Fecha = Reporte.Fecha, Hora = Reporte.Hora, Primero = Reporte.Primero, Segundo = Reporte.Segundo, Tercero = Reporte.Tercero, Tickets = Reporte.Tickets };
                ReportesIndexGanadores indexGanadores = new ReportesIndexGanadores() { Loteria = ViewModel.LoteriaID, Fecha = Convert.ToDateTime(Reporte.Fecha), Primero = Reporte.Primero, Segundo = Reporte.Segundo, Tercero = Reporte.Tercero, Sorteo = nombreLoteria };
                string[] array = new string[3];
                string[] arrayHeader = new string[3];
                string[] arrayTotals = new string[0];
                List<string[]> ListArray = new List<string[]>();

                var TicketPendientePagos = Reporte.Tickets.Where(ticket => ticket.Solicitud == 3 || ticket.Solicitud == 4) ;
                var TicketSinReclamar = Reporte.Tickets.Where(ticket => ticket.Solicitud == 6);
                var TicketPagados = Reporte.Tickets.Where(ticket => ticket.Solicitud == 5);
                 
                arrayHeader.SetValue("Tickets", 0); arrayHeader.SetValue("Fecha", 1); arrayHeader.SetValue("Monto", 2);
                if (TicketPendientePagos.Count() > 0)
                {
                    array = new string[1];
                    array.SetValue("---Tickets Pendientes De Pago---", 0);
                    ListArray.Add(array);
                    array = new string[3];
                for (var i=0;i < TicketPendientePagos.Count();i++)
                    {
                        array.SetValue(TicketPendientePagos.ToList()[i].TicketNo, 0);
                        array.SetValue(Convert.ToDateTime(TicketPendientePagos.ToList()[i].StrFecha).ToString("dd-MMM-yyyy"), 1);
                        array.SetValue(TicketPendientePagos.ToList()[i].Pago.ToString("C2"), 2);
                        ListArray.Add(array);
                        array = new string[3];
                    }
                    array.SetValue("Total", 0);
                    array.SetValue(" ", 1);
                    array.SetValue(TicketPendientePagos.Sum(x => x.Pago).ToString("C2"), 2);
                    ListArray.Add(array);
                    array = new string[3];


                }
                if (TicketPagados.Count() > 0)
                {
                    array = new string[1];
                    array.SetValue("----Tickets Pagados----", 0);
                    ListArray.Add(array);
                    array = new string[3];
                for (var i = 0; i < TicketPagados.Count(); i++)
                    {
                        array.SetValue(TicketPagados.ToList()[i].TicketNo, 0);
                        array.SetValue(Convert.ToDateTime(TicketPagados.ToList()[i].StrFecha).ToString("dd-MMM-yyyy"), 1);
                        array.SetValue(TicketPagados.ToList()[i].Pago.ToString("C2"), 2);
                        ListArray.Add(array);
                        array = new string[3];
                    }
                    
                    array.SetValue("Total", 0);
                    array.SetValue(" ", 1);
                    array.SetValue(TicketPagados.Sum(x => x.Pago).ToString("C2"), 2);
                    ListArray.Add(array);
                    array = new string[3];
                }
                if (TicketSinReclamar.Count() > 0)
                {
                    array = new string[1];
                    array.SetValue("---Tickets Sin Reclamar---", 0);
                    ListArray.Add(array);
                    array = new string[3];
                for (var i = 0; i < TicketSinReclamar.Count(); i++)
                    {
                        array.SetValue(TicketSinReclamar.ToList()[i].TicketNo, 0);
                        array.SetValue(Convert.ToDateTime(TicketSinReclamar.ToList()[i].StrFecha).ToString("dd-MMM-yyyy"), 1);
                        array.SetValue(TicketSinReclamar.ToList()[i].Pago.ToString("C2"), 2);
                        ListArray.Add(array);
                        array = new string[3];
                    }
                    array.SetValue("Total", 0);
                    array.SetValue(" ", 1);
                    array.SetValue(TicketSinReclamar.Sum(x => x.Pago).ToString("C2"), 2);
                    ListArray.Add(array);
                    array = new string[3];

                }
                array = new string[3];
                array.SetValue("--------------", 0);
                array.SetValue("--------------", 1);
                array.SetValue("------------", 2);
                ListArray.Add(array);
                array = new string[1];
                array.SetValue("Balance Ganadores:"+ (TicketSinReclamar.Sum(x => x.Pago) + TicketPagados.Sum(x => x.Pago) + TicketPendientePagos.Sum(x => x.Pago)).ToString("N"), 0);
                ListArray.Add(array);
                array = new string[1];
                array.SetValue(".", 0);
                ListArray.Add(array);
                array = new string[1];
                array.SetValue(".", 0);
                ListArray.Add(array);

            ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "TICKETS GANADORES", Data = ListArray, FechaReporte = Convert.ToDateTime(Reporte.Fecha), Headers = arrayHeader, Totals = arrayTotals, Loteria = nombreLoteria };
                List<string[,]> ImprimirSumVentaGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador);

            
                //List<string[]> reporteGanadoresPrint = PrintJobs.FromReporteDeGanadores(ModelMarGanadores, indexGanadores,Autenticador);
                ReporteTemplateHelper.PrintReporte(ImprimirSumVentaGeneral, Autenticador, ViewModel, true);
            //}

                
        }

        private void PrintListaTarjetas(object Parametro)
        {
            var reportes = ReportesService.ReporteListaTarjetas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
            MAR_Pines Pines = new MAR_Pines() { Dia = reportes.Dia, Err = reportes.Err, Fecha = reportes.Fecha, Hora = reportes.Hora, Pines = reportes.Pines };
            string[] array = new string[4];
            string[] arrayHeader = new string[4];
            string[] arrayTotals = new string[0];
            List<string[]> ListArray = new List<string[]>();
            var nombreLoteria = new ReporteView().GetNombreLoteria();

            arrayHeader.SetValue("Suplidor", 0); arrayHeader.SetValue("Hora", 1); arrayHeader.SetValue("Numero", 2); arrayHeader.SetValue("Monto", 3);

            for (var i=0;i < reportes.Pines.Count();i++)
            {
                array.SetValue(reportes.Pines[i].Producto.Suplidor.ToString(), 0);
                array.SetValue(Convert.ToDateTime(reportes.Pines[i].StrHora).ToString("hh:mm t"), 1);
                array.SetValue(reportes.Pines[i].Serie.ToString(), 2);
                array.SetValue(reportes.Pines[i].Costo.ToString("C2"), 3);
                ListArray.Add(array);
                array = new string[4];
            }
            array = new string[3];
            array.SetValue("--------------", 0);
            array.SetValue("--------------", 1);
            array.SetValue("------------", 2);
            ListArray.Add(array);
            array = new string[4];
            array.SetValue("Venta:", 0);
            array.SetValue(" ", 1);
            array.SetValue(" ", 2);
            array.SetValue(reportes.Pines.Sum(x => x.Costo).ToString("C2"), 3);
            ListArray.Add(array);
            array = new string[1];
            array.SetValue(".", 0);
            ListArray.Add(array);
            array = new string[1];
            array.SetValue(".", 0);
            ListArray.Add(array);

            ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "LISTADO DE PINES", Data = ListArray, FechaReporte = Convert.ToDateTime(reportes.Fecha), Headers = arrayHeader, Totals = arrayTotals, Loteria = nombreLoteria };
            List<string[,]> ImprimirSumVentaGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador);

            //List<string[]> PrintPines = PrintJobs.FromReporteListadoDePines(Pines,Autenticador);
            ReporteTemplateHelper.PrintReporte(ImprimirSumVentaGeneral, Autenticador, ViewModel);
        }

        private void PrintListaNumeros(object Parametro)
        {
            try
            {
                var nombreLoteria = new ReporteView().GetNombreLoteria();
                var Reporte = ReportesService.ReporteListadoNumero(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
                MAR_VentaNumero ListaNumero = new MAR_VentaNumero() { Dia = Reporte.Dia, Err = Reporte.Err, Fecha = Reporte.Fecha, Hora = Reporte.Hora, Loteria = Reporte.Loteria, Numeros = Reporte.Numeros };
                var ListadoNumero = TemplateListaNumero(Reporte);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void PrintVentas(object Parametro)
        {

            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var ReporteVenta = ReportesService.ReporteDeVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);

            MAR_RptVenta TemplatePrintVenta = new MAR_RptVenta()
            {
                CntNumeros = ReporteVenta.CntNumeros,
                CntTarjetas = ReporteVenta.CntTarjetas,
                Comision = ReporteVenta.Comision,
                ComisionPorcP = ReporteVenta.ComisionPorcP,
                ComisionPorcQ = ReporteVenta.ComisionPorcP,
                ComisionPorcT = ReporteVenta.ComisionPorcT,
                CPrimero = ReporteVenta.CPrimero,
                CSegundo = ReporteVenta.CSegundo,
                CTercero = ReporteVenta.CTercero,
                Dia = ReporteVenta.Dia,
                Err = ReporteVenta.Err,
                Fecha = ReporteVenta.Fecha,
                Hora = ReporteVenta.Hora,
                Loteria = ReporteVenta.Loteria,
                MPales = ReporteVenta.MPales,
                MPrimero = ReporteVenta.MPrimero,
                MSegundo = ReporteVenta.MSegundo,
                MTercero = ReporteVenta.MTercero,
                MTripletas = ReporteVenta.MTripletas,
                Numeros = ReporteVenta.Numeros,
                Pales = ReporteVenta.Pales,
                Primero = ReporteVenta.Primero,
                Segundo = ReporteVenta.Segundo,
                Tercero = ReporteVenta.Tercero
               ,
                TicketsNulos = ReporteVenta.TicketsNulos,
                Tripletas = ReporteVenta.Tripletas
            };

            string[] array = new string[2];
            string[] arrayHeader = new string[0];
            string[] arrayTotals = new string[0];

            int totalVenta = 0; int totalComision = 0; int totalSaco = 0; int totalBalance = 0;

            List<string[]> ListArray = new List<string[]>();

            //arrayHeader.SetValue("Concepto", 0); arrayHeader.SetValue("Venta", 1); arrayHeader.SetValue("Comision", 2); arrayHeader.SetValue("Saco", 3); arrayHeader.SetValue("Balance", 4);

            array.SetValue("Numeros:", 0);
            array.SetValue(TemplatePrintVenta.CntNumeros.ToString(),1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Numeros ($RD):", 0);
            array.SetValue(TemplatePrintVenta.CntNumeros.ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Pales:", 0);
            array.SetValue(TemplatePrintVenta.Pales.ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Tripletas:", 0);
            array.SetValue(TemplatePrintVenta.Tripletas.ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Total Venta:", 0);
            array.SetValue((TemplatePrintVenta.CntNumeros + TemplatePrintVenta.Pales + TemplatePrintVenta.Tripletas).ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Comision:", 0);
            array.SetValue(TemplatePrintVenta.Comision.ToString("C2"), 1);
            ListArray.Add(array);
           
            array = new string[2];
            array.SetValue("Venta Neta:", 0);
            array.SetValue(((TemplatePrintVenta.CntNumeros + TemplatePrintVenta.Pales + TemplatePrintVenta.Tripletas) -(TemplatePrintVenta.Comision)).ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("----------------------", 0);
            array.SetValue("----------------", 1);
            ListArray.Add(array);
            if (nombreLoteria == "Todas")
            {
                if (TemplatePrintVenta.TicketsNulos.Count() > 0)
                {
                    array = new string[1];
                    array.SetValue("------ Tickets Anulados ------", 0);
                    ListArray.Add(array);
                    array = new string[3];
                    array.SetValue("Ticket", 0);
                    array.SetValue("Hora", 1);
                    array.SetValue("Precios", 2);
                    ListArray.Add(array);
                    array = new string[3];
                    foreach (var TicketNulo in TemplatePrintVenta.TicketsNulos)
                    {
                        array.SetValue(TicketNulo.TicketNo.ToString(), 0);
                        array.SetValue(TicketNulo.StrHora.ToString(), 1);
                        array.SetValue(TicketNulo.Costo.ToString("C2"), 2);
                        ListArray.Add(array);
                    }

                }
            }
            if (nombreLoteria != "Todas") {
                array = new string[2];
                array.SetValue("--------------", 0);
                array.SetValue("--------------", 1);
                array = new string[4];
            array.SetValue(" ", 0);
            array.SetValue("Premios ", 1);
            array.SetValue("Cantidad", 2);
            array.SetValue("Gana", 3);
            ListArray.Add(array);
            array = new string[4];
            array.SetValue("1RA. ", 0);
            array.SetValue(TemplatePrintVenta.Primero.ToString(), 1);
            array.SetValue(TemplatePrintVenta.MPrimero.ToString("C2"), 2);
            array.SetValue(TemplatePrintVenta.CPrimero.ToString("C2"), 3);
            ListArray.Add(array);
            array = new string[4];
            array.SetValue("2DA. ", 0);
            array.SetValue(TemplatePrintVenta.Segundo.ToString(), 1);
            array.SetValue(TemplatePrintVenta.MSegundo.ToString("C2"), 2);
            array.SetValue(TemplatePrintVenta.CSegundo.ToString("C2"), 3);
            ListArray.Add(array);
            array = new string[4];
            array.SetValue("3RA. ", 0);
            array.SetValue(TemplatePrintVenta.Tercero.ToString(), 1);
            array.SetValue(TemplatePrintVenta.MTercero.ToString("C2"), 2);
            array.SetValue(TemplatePrintVenta.CTercero.ToString("C2"), 3);
            ListArray.Add(array);
                array = new string[4];
                array.SetValue("--------------", 0);
                array.SetValue("--------------", 1);
                array.SetValue("--------------", 2);
                array.SetValue("--------------", 3);
                array = new string[2];
            array.SetValue("Numeros Premiados:", 0);
            array.SetValue(((TemplatePrintVenta.MPrimero)+(TemplatePrintVenta.MSegundo)+(TemplatePrintVenta.MTercero)).ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Pales Premiados:", 0);
            array.SetValue(TemplatePrintVenta.MPales.ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Tripletas Premiados:", 0);
            array.SetValue(TemplatePrintVenta.MTripletas.ToString("C2"), 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("---------------------", 0);
            array.SetValue("---------------------", 1);
            ListArray.Add(array);
            array = new string[2];
            array.SetValue("Total Premiados:", 0);
            array.SetValue((((TemplatePrintVenta.MPrimero) + (TemplatePrintVenta.MSegundo) + (TemplatePrintVenta.MTercero)) + (TemplatePrintVenta.MPales) + (TemplatePrintVenta.MTripletas)).ToString("C2"), 1);
            ListArray.Add(array);
            if (((TemplatePrintVenta.CntNumeros + TemplatePrintVenta.Pales + TemplatePrintVenta.Tripletas) - (TemplatePrintVenta.Comision))-((TemplatePrintVenta.MPrimero) + (TemplatePrintVenta.MSegundo) + (TemplatePrintVenta.MTercero)) + (TemplatePrintVenta.MPales) + (TemplatePrintVenta.MTripletas) >= 0)
            {
                array = new string[2];
                array.SetValue("Ganancia:", 0);
                array.SetValue(((TemplatePrintVenta.CntNumeros + TemplatePrintVenta.Pales + TemplatePrintVenta.Tripletas) - (TemplatePrintVenta.Comision)).ToString("C2"), 1);
                ListArray.Add(array);
            }
            else
            {
                array = new string[2];
                array.SetValue("Perdida:", 0);
                array.SetValue(((TemplatePrintVenta.CntNumeros + TemplatePrintVenta.Pales + TemplatePrintVenta.Tripletas) - (TemplatePrintVenta.Comision)).ToString("C2"), 1);
                ListArray.Add(array);
            }
            if (TemplatePrintVenta.TicketsNulos.Count() > 0)
            {
                array = new string[1];
                array.SetValue("----------- Tickets Anulados -----------", 0);
                ListArray.Add(array);
                array = new string[3];
                array.SetValue("Ticket", 0);
                array.SetValue("Hora", 1);
                array.SetValue("Precios", 2);
                ListArray.Add(array);
                array = new string[3];
                foreach (var TicketNulo in TemplatePrintVenta.TicketsNulos) {
                    array.SetValue(TicketNulo.TicketNo.ToString(), 0);
                    array.SetValue(TicketNulo.StrHora.ToString(), 1);
                    array.SetValue(TicketNulo.Costo.ToString("C2"), 2);
                    ListArray.Add(array);
            }
                }
                else
                {
                    array = new string[1];
                    array.SetValue("---- No Hay Tickets Nulos ------", 0);
                    ListArray.Add(array);
                }
            }
            else
            {
                array = new string[1];
                array.SetValue("---- Premios No Disponibles ------", 0);
                ListArray.Add(array);
                array = new string[1];
                array.SetValue(".", 0);
                ListArray.Add(array);
                if (TemplatePrintVenta.TicketsNulos.Count() == 0)
                {
                    array = new string[1];
                    array.SetValue("---- No Hay Tickets Nulos ------", 0);
                    ListArray.Add(array);
                }
                array = new string[1];
                array.SetValue(".", 0);
                ListArray.Add(array);

            }
            var loteriaNombre = SessionGlobals.LoteriasTodas.Where(y => y.Numero == ReporteVenta.Loteria).Select(x => x.Nombre);
            //arrayTotals.SetValue("Total", 0); arrayTotals.SetValue(totalVenta.ToString("C3"), 1); arrayTotals.SetValue(totalComision.ToString("C3"), 2); arrayTotals.SetValue(totalSaco.ToString("C3"), 3); arrayTotals.SetValue(totalBalance.ToString("C3"), 4);
            ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "REPORTE DE VENTAS", Data = ListArray, FechaReporte = Convert.ToDateTime(ReporteVenta.Fecha), Headers = arrayHeader, Totals = arrayTotals,Loteria=nombreLoteria };
            List<string[,]> ImprimirSumVentaGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador);

            //List<string[]> PrintRPTVentas = PrintJobs.FromReporteVentas(TemplatePrintVenta, nombreLoteria,Autenticador);
            ReporteTemplateHelper.PrintReporte(ImprimirSumVentaGeneral , Autenticador, ViewModel);
        }

        private void PrintListaTicket(object Parametro)
        {
            var NombreLoteria = new ReporteView().GetNombreLoteria();

            var ReporteTicket = ReportesService.ReporteListaDeTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            MAR_Ganadores ModelMarGanadores = new MAR_Ganadores() { Dia = ReporteTicket.Dia, Err = ReporteTicket.Err, Fecha = ReporteTicket.Fecha, Hora = ReporteTicket.Hora, Primero = ReporteTicket.Primero, Segundo = ReporteTicket.Segundo, Tercero = ReporteTicket.Tercero, Tickets = ReporteTicket.Tickets };

            string[] array = new string[4];
            string[] arrayHeader = new string[4];
            string[] arrayTotals = new string[0];
            List<string[]> ListArray = new List<string[]>();

            arrayHeader.SetValue("Ticket", 0);arrayHeader.SetValue("Hora",1);arrayHeader.SetValue("Vendio", 2);arrayHeader.SetValue("Saco", 3);
            

            for (var i=0; i < ModelMarGanadores.Tickets.Count();i++)
            {
                array.SetValue(ModelMarGanadores.Tickets[i].TicketNo, 0);
                array.SetValue(ModelMarGanadores.Tickets[i].StrHora, 1);
                array.SetValue(ModelMarGanadores.Tickets[i].Costo.ToString("N"), 2);
                if (ModelMarGanadores.Tickets[i].Nulo==true) { array.SetValue("NULO", 3); }
                if (ModelMarGanadores.Tickets[i].Nulo == false) { array.SetValue(ModelMarGanadores.Tickets[i].Pago.ToString("N"), 3); }
                
                ListArray.Add(array);
                array = new string[4];
            }
            array = new string[4];
            array.SetValue("----------", 0);
            array.SetValue("----------", 1);
            array.SetValue("----------", 2);
            array.SetValue("----------", 3);
            ListArray.Add(array);

            array = new string[4];
            array.SetValue("Venta:", 0);
            array.SetValue("Saco:", 1);
            array.SetValue("Validos:", 2);
            array.SetValue("Nulos:", 3);
            ListArray.Add(array);
            array = new string[4];
            array.SetValue(ModelMarGanadores.Tickets.Where(x => x.Nulo == false).Sum(x => x.Costo).ToString("C2"), 0);
            array.SetValue(ModelMarGanadores.Tickets.Where(x => x.Nulo == false).Sum(x => x.Pago).ToString("C2"), 1);
            array.SetValue(ModelMarGanadores.Tickets.Where(x => x.Nulo == false).Count().ToString(), 2);
            array.SetValue(ModelMarGanadores.Tickets.Where(x => x.Nulo == true).Count().ToString(), 3);
            ListArray.Add(array);
            array = new string[1];
            array.SetValue(".", 0);
            ListArray.Add(array);

            ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "LISTADO DE TICKETS", Data = ListArray, FechaReporte = Convert.ToDateTime(ModelMarGanadores.Fecha), Headers = arrayHeader, Totals = arrayTotals, Loteria = NombreLoteria };
            List<string[,]> ImprimirListTicketGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador);
            //List<string[]> reporteTicketPrint = PrintJobs.FromReporteListadoDeTickets(ModelMarGanadores, NombreLoteria,Autenticador);
            ReporteTemplateHelper.PrintReporte(ImprimirListTicketGeneral, Autenticador, ViewModel);
        }
        private void PrintPagosRemotos(object Parametro)
        {
            var PagosRemotos = ReportesService.ReporteListaPagosRemotos(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
            List<string[]> reportePagosRemostos = PrintJobs.FromPagosRemoto(PagosRemotos, ViewModel.Fecha.ToString(), Autenticador);
            string[] array = new string[4];
            string[] arrayHeader = new string[4];
            string[] arrayTotals = new string[0];
            List<string[]> ListArray = new List<string[]>();

            arrayHeader.SetValue("Ticket", 0); arrayHeader.SetValue("Hora", 1); arrayHeader.SetValue("Balance", 2); arrayHeader.SetValue("Banca", 3);

            foreach (var pagosremotos in PagosRemotos.Tickets)
            {
                array.SetValue(pagosremotos.TicketNo, 0);
                array.SetValue(pagosremotos.StrHora,1);
                array.SetValue(pagosremotos.Pago.ToString("N"),2);
                array.SetValue(pagosremotos.Cliente,3);
                ListArray.Add(array);
                array = new string[4];
            }
            array = new string[4];
            array.SetValue("----------", 0);
            array.SetValue("----------", 1);
            array.SetValue("----------", 2);
            array.SetValue("----------", 3);
            ListArray.Add(array);
            array = new string[4];
            array.SetValue("Balance:", 0);
            array.SetValue(" ", 1);
            array.SetValue(PagosRemotos.Tickets.Sum(x => x.Pago).ToString("N"), 2);
            array.SetValue(" ", 3);
            ListArray.Add(array);
            array = new string[1];
            array.SetValue(".", 0);
            ListArray.Add(array);

            ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "TICKETS PAGADOS REMOTAMENTE", Data = ListArray, FechaReporte = Convert.ToDateTime(PagosRemotos.Fecha), Headers = arrayHeader, Totals = arrayTotals, Loteria = null };
            List<string[,]> ImprimirListTicketGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador);
            ReporteTemplateHelper.PrintReporte(ImprimirListTicketGeneral, Autenticador, ViewModel);
        }

        private void PrintListdoPremio(object Parametro)
        {
            var marSesion = Autenticador.CurrentAccount.MAR_Setting2.Sesion;

            var juegaMasSesion = new ClienteMarWPFWin7.Domain.JuegaMasService.MAR_Session();
            juegaMasSesion.Sesion = marSesion.Sesion;
            juegaMasSesion.LastPin = marSesion.LastPin;
            juegaMasSesion.LastTck = marSesion.LastTck;
            juegaMasSesion.PrinterFooter = marSesion.PrinterFooter;
            juegaMasSesion.PrinterHeader = marSesion.PrinterHeader;
            juegaMasSesion.Banca = marSesion.Banca;
            juegaMasSesion.Usuario = marSesion.Usuario;
            juegaMasSesion.Err = marSesion.Err;
            var reportes = servicioJuegamas.LeerReporteEstadoDePremiosJuegaMas(juegaMasSesion, ViewModel.Fecha);

            //           var Deserealizado2 = DeserializarString(SinCorchetes.ToString());
            //var Deserializado = DeserializarJuegaMas(SinComillas.ToString());
            List<string[]> printData = new List<string[]>() { };
            var resultado = JsonConvert.DeserializeObject<ClienteMarWPFWin7.Domain.JuegaMasService.MAR_JuegaMasResponse>(reportes.Respuesta);
            var arrayJeison = JsonConvert.DeserializeObject(reportes.Respuesta);
            ModelSerializePremios ModeloPremios = JsonConvert.DeserializeObject<ModelSerializePremios>(arrayJeison.ToString());
            printData = ModeloPremios.PrintData;
            printData[1] = new string[] { printData[0][0].ToString() + "\r\n" + printData[1][0].ToString() };
            printData[0] = new string[] { "" };

            TicketTemplateHelper.PrintTicket(printData);
        }

        private List<string[]> TemplateListaNumero(MAR_VentaNumero Reporte)
        {

            var nombreLoteria = new ReporteView().GetNombreLoteria();
            string[] array = new string[6];
            string[] arrayHeader = new string[0];
            string[] arrayTotals = new string[0];
            List<string[]> ListArray = new List<string[]>();

            if (Reporte.Err == null)
            {
                try
                {
                    if (Reporte.Numeros != null)
                    {
                        if (Reporte.Numeros.Count() > 0)
                        {
                            ReporteListNumeroColumns ReporteListNumeros = new ReporteListNumeroColumns();
                            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
                            var Numeros = Reporte.Numeros;
                            var NumerosQuinielas = Reporte.Numeros.Where(ticket => ticket.QP == "Q").ToList();
                            var NumerosPales = Reporte.Numeros.Where(ticket => ticket.QP == "P").ToList();
                            var NumerosTripleta = Reporte.Numeros.Where(ticket => ticket.QP == "T").ToList();
                            ReporteListNumeros.Quiniela = new ObservableCollection<ReportesListaNumerosObservable>() { };
                            ReporteListNumeros.Tripleta = new ObservableCollection<ReportesListaNumerosObservable>() { };
                            ReporteListNumeros.Pale = new ObservableCollection<ReportesListaNumerosObservable>() { };

                            
                            if (NumerosQuinielas.Count == 0 && NumerosPales.Count == 0 && NumerosTripleta.Count == 0)
                            {
                                MessageBox.Show("No existen numeros correspondientes a la fecha y opcion de loteria seleccionada.", "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            
                            if (NumerosQuinielas.Count > 1)
                            {
                                var totalCantidadQuiniela = 0;
                                var totalPagoQuiniela = 0;

                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Detalles De Numeros Vendidos", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[6];
                                array.SetValue("Num", 0); array.SetValue("Cant", 1); array.SetValue("Num", 2); array.SetValue("Cant", 3); array.SetValue("Num", 4); array.SetValue("Cant", 5);
                                ListArray.Add(array);

                                for (int i = 0; i < NumerosQuinielas.Count; i = i + 3)
                                {
                                    if (i + 2 <= NumerosQuinielas.Count - 1)
                                    {
                                        array = new string[6];
                                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                                            CantidadColumn1 = NumerosQuinielas[i].Cantidad.ToString("N"),

                                            NumeroColumn2 = NumerosQuinielas[i + 1].Numero,
                                            CantidadColumn2 = NumerosQuinielas[i + 1].Cantidad.ToString("N"),

                                            NumeroColumn3 = NumerosQuinielas[i + 2].Numero,
                                            CantidadColumn3 = NumerosQuinielas[i + 2].Cantidad.ToString("N")
                                        };
                                        array.SetValue(objectoQuiniela.NumeroColumn1, 0);
                                        array.SetValue(objectoQuiniela.CantidadColumn1, 1);
                                        array.SetValue(objectoQuiniela.NumeroColumn2, 2);
                                        array.SetValue(objectoQuiniela.CantidadColumn2, 3);
                                        array.SetValue(objectoQuiniela.NumeroColumn3, 4);
                                        array.SetValue(objectoQuiniela.CantidadColumn3, 5);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                    if (i + 1 <= NumerosQuinielas.Count - 1)
                                    {
                                        array = new string[6];
                                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                                            CantidadColumn1 = NumerosQuinielas[i].Cantidad.ToString("N"),
                                            NumeroColumn2 = NumerosQuinielas[i + 1].Numero,
                                            CantidadColumn2 = NumerosQuinielas[i + 1].Cantidad.ToString("N"),

                                        };
                                        array.SetValue(objectoQuiniela.NumeroColumn1, 0);
                                        array.SetValue(objectoQuiniela.CantidadColumn1, 1);
                                        array.SetValue(objectoQuiniela.NumeroColumn2, 2);
                                        array.SetValue(objectoQuiniela.CantidadColumn2, 3);
                                        array.SetValue(" ", 4);
                                        array.SetValue(" ", 5);
                                        ListArray.Add(array);
                                        continue;
                                    }

                                    if (i <= NumerosQuinielas.Count - 1)
                                    {
                                        array = new string[6];
                                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                                            CantidadColumn1 = NumerosQuinielas[i].Cantidad.ToString("N")
                                        };
                                        array.SetValue(objectoQuiniela.NumeroColumn1, 0);
                                        array.SetValue(objectoQuiniela.CantidadColumn1, 1);
                                        array.SetValue(" ", 2);
                                        array.SetValue(" ", 3);
                                        array.SetValue(" ", 4);
                                        array.SetValue(" ", 5);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                }
                                foreach (var quiniela in NumerosQuinielas) { totalCantidadQuiniela = totalCantidadQuiniela + Convert.ToInt32(quiniela.Cantidad); totalPagoQuiniela = totalPagoQuiniela + Convert.ToInt32(quiniela.Pago); }
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Ventas: " + NumerosQuinielas.Sum(x => x.Costo).ToString("N") + "    " + NumerosQuinielas.Sum(x => x.Pago).ToString("N"), 0);
                                ListArray.Add(array);
                            }
                            else if (NumerosQuinielas.Count == 1)
                            {
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Detalles De Numeros Vendidos", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[6];
                                array.SetValue("Num", 0); array.SetValue("Cant", 1); array.SetValue("Num", 2); array.SetValue("Cant", 3); array.SetValue("Num", 4); array.SetValue("Cant", 5);
                                ListArray.Add(array);
                                array = new string[6];
                                ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                                {
                                    NumeroColumn1 = NumerosQuinielas[0].Numero,
                                    CantidadColumn1 = NumerosQuinielas[0].Cantidad.ToString("N")

                                };
                                array.SetValue(objectoQuiniela.NumeroColumn1, 0);
                                array.SetValue(objectoQuiniela.CantidadColumn1, 1);
                                array.SetValue(" ", 2);
                                array.SetValue(" ", 3);
                                array.SetValue(" ", 4);
                                array.SetValue(" ", 5);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Ventas: " + NumerosQuinielas.Sum(x => x.Costo).ToString("N") + "    " + NumerosQuinielas.Sum(x => x.Pago).ToString("N"), 0);
                                ListArray.Add(array);

                            }
                            

                            if (NumerosPales.Count > 1)
                            {
                                var totalCantidadPale = 0;
                                var totalPagoPale = 0;
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Detalles De Pales Vendidos", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[6];
                                array.SetValue("Num", 0); array.SetValue("Cant", 1); array.SetValue("Num", 2); array.SetValue("Cant", 3); array.SetValue("Num", 4); array.SetValue("Cant", 5);
                                ListArray.Add(array);
                                for (int i = 0; i < NumerosPales.Count; i = i + 3)
                                {

                                    if (i + 2 <= NumerosPales.Count - 1)
                                    {
                                        array = new string[6];
                                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosPales[i].Numero,
                                            CantidadColumn1 = NumerosPales[i].Cantidad.ToString("N"),
                                            NumeroColumn2 = NumerosPales[i + 1].Numero,
                                            CantidadColumn2 = NumerosPales[i + 1].Cantidad.ToString("N"),
                                            NumeroColumn3 = NumerosPales[i + 2].Numero,
                                            CantidadColumn3 = NumerosPales[i + 2].Cantidad.ToString("N")
                                        };
                                        array.SetValue(objectoPale.NumeroColumn1, 0);
                                        array.SetValue(objectoPale.CantidadColumn1, 1);
                                        array.SetValue(objectoPale.NumeroColumn2, 2);
                                        array.SetValue(objectoPale.CantidadColumn2, 3);
                                        array.SetValue(objectoPale.NumeroColumn3, 4);
                                        array.SetValue(objectoPale.CantidadColumn3, 5);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                    if (i + 1 <= NumerosPales.Count - 1)
                                    {
                                        array = new string[6];
                                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosPales[i].Numero,
                                            CantidadColumn1 = NumerosPales[i].Cantidad.ToString("N"),

                                            NumeroColumn2 = NumerosPales[i + 1].Numero,
                                            CantidadColumn2 = NumerosPales[i + 1].Cantidad.ToString("N"),

                                        };
                                        array.SetValue(objectoPale.NumeroColumn1, 0);
                                        array.SetValue(objectoPale.CantidadColumn1, 1);
                                        array.SetValue(objectoPale.NumeroColumn2, 2);
                                        array.SetValue(objectoPale.CantidadColumn2, 3);
                                        array.SetValue(" ", 4);
                                        array.SetValue(" ", 5);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                    if (i <= NumerosPales.Count - 1)
                                    {
                                        array = new string[6];
                                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosPales[i].Numero,
                                            CantidadColumn1 = NumerosPales[i].Cantidad.ToString("N")
                                        };
                                        array.SetValue(objectoPale.NumeroColumn1, 0);
                                        array.SetValue(objectoPale.CantidadColumn1, 1);
                                        array.SetValue(" ", 2);
                                        array.SetValue(" ", 3);
                                        array.SetValue(" ", 4);
                                        array.SetValue(" ", 5);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                }
                                foreach (var pale in NumerosPales) { totalCantidadPale = totalCantidadPale + Convert.ToInt32(pale.Cantidad); totalPagoPale = totalPagoPale + Convert.ToInt32(pale.Pago); }
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Ventas: " + NumerosPales.Sum(x => x.Costo).ToString("N") + "    " + NumerosPales.Sum(x => x.Pago).ToString("N"), 0);
                                ListArray.Add(array);
                            }
                            else if (NumerosPales.Count == 1)
                            {
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Detalles De Pales Vendidos", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[6];
                                array.SetValue("Num", 0); array.SetValue("Cant", 1); array.SetValue("Num", 2); array.SetValue("Cant", 3); array.SetValue("Num", 4); array.SetValue("Cant", 5);
                                ListArray.Add(array);
                                array = new string[6];
                                ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                                {
                                    NumeroColumn1 = NumerosPales[0].Numero,
                                    CantidadColumn1 = NumerosPales[0].Cantidad.ToString("N")

                                };
                                array.SetValue(objectoPale.NumeroColumn1, 0);
                                array.SetValue(objectoPale.CantidadColumn1, 1);
                                array.SetValue(" ", 2);
                                array.SetValue(" ", 3);
                                array.SetValue(" ", 4);
                                array.SetValue(" ", 5);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Ventas: " + NumerosPales.Sum(x => x.Costo).ToString("N") + "    " + NumerosPales.Sum(x => x.Pago).ToString("N"), 0);
                                ListArray.Add(array);
                            }
                            
                            if (NumerosTripleta.Count == 1)
                            {
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Detalles De Tripletas Vendidos", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[6];
                                array.SetValue("Num", 0); array.SetValue("Cant", 1); array.SetValue("Num", 2); array.SetValue("Cant", 3); array.SetValue("Num", 4); array.SetValue("Cant", 5);
                                ListArray.Add(array);
                                ReportesListaNumerosObservable ObjectoTripleta = new ReportesListaNumerosObservable()
                                {
                                    NumeroColumn1 = NumerosTripleta[0].Numero,
                                    CantidadColumn1 = NumerosTripleta[0].Cantidad.ToString("N")
                                };
                                array.SetValue(ObjectoTripleta.NumeroColumn1, 0);
                                array.SetValue(ObjectoTripleta.CantidadColumn1, 1);
                                array.SetValue(" ", 2);
                                array.SetValue(" ", 3);
                                array.SetValue(" ", 4);
                                array.SetValue(" ", 5);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Ventas: " + NumerosTripleta.Sum(x => x.Costo).ToString("N") + "    " + NumerosTripleta.Sum(x => x.Pago).ToString("N"), 0);
                                ListArray.Add(array);
                            }
                            else if (NumerosTripleta.Count > 1)
                            {
                                var totalCantidadTripleta = 0;
                                var totalPagoTripleta = 0;
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Detalles De Tripletas Vendidos", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[6];
                                array.SetValue("Num", 0); array.SetValue("Cant", 1); array.SetValue("Num", 2); array.SetValue("Cant", 3); array.SetValue("Num", 4); array.SetValue("Cant", 5);
                                ListArray.Add(array);
                                for (int i = 0; i < NumerosTripleta.Count; i = i + 3)
                                {
                                    if (i + 2 <= NumerosTripleta.Count - 1)
                                    {
                                        array = new string[6];
                                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosTripleta[i].Numero,
                                            CantidadColumn1 = NumerosTripleta[i].Cantidad.ToString("N"),
                                            NumeroColumn2 = NumerosTripleta[i + 1].Numero,
                                            CantidadColumn2 = NumerosTripleta[i + 1].Cantidad.ToString("N"),
                                            NumeroColumn3 = NumerosTripleta[i + 2].Numero,
                                            CantidadColumn3 = NumerosTripleta[i + 2].Cantidad.ToString("N")
                                        };
                                        array.SetValue(objectoTripleta.NumeroColumn1, 0);
                                        array.SetValue(objectoTripleta.CantidadColumn1, 1);
                                        array.SetValue(objectoTripleta.NumeroColumn2, 2);
                                        array.SetValue(objectoTripleta.CantidadColumn2, 3);
                                        array.SetValue(objectoTripleta.NumeroColumn3, 4);
                                        array.SetValue(objectoTripleta.CantidadColumn3, 5);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                    if (i + 1 <= NumerosTripleta.Count - 1)
                                    {
                                        array = new string[4];
                                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosTripleta[i].Numero,
                                            CantidadColumn1 = NumerosTripleta[i].Cantidad.ToString("N"),
                                            NumeroColumn2 = NumerosTripleta[i + 1].Numero,
                                            CantidadColumn2 = NumerosTripleta[i + 1].Cantidad.ToString("N")
                                        };
                                        array.SetValue(objectoTripleta.NumeroColumn1, 0);
                                        array.SetValue(objectoTripleta.CantidadColumn1, 1);
                                        array.SetValue(objectoTripleta.NumeroColumn2, 2);
                                        array.SetValue(objectoTripleta.CantidadColumn2, 3);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                    if (i <= NumerosTripleta.Count - 1)
                                    {
                                        array = new string[2];
                                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosTripleta[i].Numero,
                                            CantidadColumn1 = NumerosTripleta[i].Cantidad.ToString("N")
                                        };
                                        array.SetValue(objectoTripleta.NumeroColumn1, 0);
                                        array.SetValue(objectoTripleta.CantidadColumn1, 1);
                                        ListArray.Add(array);
                                        continue;
                                    }
                                }
                                foreach (var tripleta in NumerosTripleta) { totalCantidadTripleta = totalCantidadTripleta + Convert.ToInt32(tripleta.Cantidad); totalPagoTripleta = totalPagoTripleta + Convert.ToInt32(tripleta.Pago); }
                                array = new string[1];
                                array.SetValue("-------------------------", 0);
                                ListArray.Add(array);
                                array = new string[1];
                                array.SetValue("Ventas: " + NumerosTripleta.Sum(x => x.Costo).ToString("N") + "    " + NumerosTripleta.Sum(x => x.Pago).ToString("N"), 0);
                                ListArray.Add(array);
                            }
                            ReportesGeneralesReportes reporte = new ReportesGeneralesReportes() { NombreReporte = "LISTADO DE NUMERO", Data = ListArray, FechaReporte = Convert.ToDateTime(Reporte.Fecha), Headers = arrayHeader, Totals = arrayTotals, Loteria = null };
                            List<string[,]> ImprimirListTicketGeneral = PrintJobs.PrintGeneralReportes(reporte, Autenticador);
                            ReporteTemplateHelper.PrintReporte(ImprimirListTicketGeneral, Autenticador, ViewModel);
                        }
                        else if (Reporte.Numeros.Count() == 0)
                        {
                            MessageBox.Show("No existen numeros correspondientes a la fecha y opcion de loteria seleccionada.", "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                    }
                    else if (Reporte.Err != null)
                    {
                        MessageBox.Show("No existen numeros correspondientes a la fecha y opcion de loteria seleccionada.", "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);
                        //MostrarMensajes(Reporte.Err,"MAR-Cliente","INFO");

                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "MAR-Cliente", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else if (Reporte.Err != null)
            {
                MessageBox.Show(Reporte.Err, "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);
                //MostrarMensajes(Reporte.Err,"MAR-Cliente","INFO");

            }
            return ListArray;
        }

    }
}
