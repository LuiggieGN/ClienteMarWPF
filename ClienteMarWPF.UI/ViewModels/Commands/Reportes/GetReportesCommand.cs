using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Services.ReportesService;
using ClienteMarWPF.UI.Modules.Reporte;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.UI.ViewModels.Commands.Reporte;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Controls;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Data;
using System.Collections.Immutable;
using System.Globalization;
using System.Runtime.Serialization;

namespace ClienteMarWPF.UI.ViewModels.Commands.Reporte
{
    public class GetReportesCommand : ActionCommand
    {
        private readonly ReporteViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IReportesServices ReportesService;


        public GetReportesCommand(ReporteViewModel viewModel, IAuthenticator autenticador, IReportesServices reportesServices)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            ReportesService = reportesServices;
           
            Action<object> comando = new Action<object>(EnviarReportes);
            base.SetAction(comando);

        }


        private string TraducirDiaSemana(string DiaDeHoy)
        {
            if (DiaDeHoy == "Monday")
            { DiaDeHoy = "Lunes"; }
            else if (DiaDeHoy == "Tuesday")
            { DiaDeHoy = "Martes"; }
            else if (DiaDeHoy == "Wednesday")
            { DiaDeHoy = "Miercoles"; }
            else if (DiaDeHoy == "Thursday")
            { DiaDeHoy = "Jueves"; }
            else if (DiaDeHoy == "Friday")
            { DiaDeHoy = "Viernes"; }
            else if (DiaDeHoy == "Saturday")
            { DiaDeHoy = "Sabado"; }
            else if (DiaDeHoy == "Sunday")
            { DiaDeHoy = "Domingo"; }

            return DiaDeHoy;
        }

        private string TraducirDiaSemanaResumido(string DiaDeHoy)
        {
            if (DiaDeHoy == "Monday")
            { DiaDeHoy = "Lu"; }
            else if (DiaDeHoy == "Tuesday")
            { DiaDeHoy = "Ma"; }
            else if (DiaDeHoy == "Wednesday")
            { DiaDeHoy = "Mi"; }
            else if (DiaDeHoy == "Thursday")
            { DiaDeHoy = "Ju"; }
            else if (DiaDeHoy == "Friday")
            { DiaDeHoy = "Vi"; }
            else if (DiaDeHoy == "Saturday")
            { DiaDeHoy = "Sa"; }
            else if (DiaDeHoy == "Sunday")
            { DiaDeHoy = "Do"; }

            return DiaDeHoy;
        }

        private void EnviarReportes(object parametro)
        {
            //Ocultar Templates de reportes
            ViewModel.RPTSumaVentas = Visibility.Hidden;
            ViewModel.RPTTicketGanadores = Visibility.Hidden;
            ViewModel.RPTSumVentaFecha = Visibility.Hidden;
            ViewModel.RPTListTarjetas = Visibility.Hidden;
            ViewModel.RPTVentas = Visibility.Hidden;
            ViewModel.RPTLitTicket = Visibility.Hidden;
            ViewModel.RPTPagosRemotos = Visibility.Hidden;

            try
            {
                var nombre = new ReporteView().GetReporteNombre();
                if (nombre == null || nombre == ""){nombre = "Reportes De Ventas";}
                if (nombre == "Suma De Ventas")
                {
                    ViewModel.NombreBanca = "Lexus";

                    //ViewModel.Fecha = DateTime.Now.ToString();
                    RPTSumaDeVentas(parametro);
                }
                else if (nombre == "Reportes Ganadores")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTGanadores(parametro);
                }
                else if (nombre == "Ventas por Fecha")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTVentasFecha(parametro);
                }
                else if (nombre == "Lista De Tarjetas")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTListasTarjetas(parametro);
                }else if (nombre == "Lista De Premios")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTListaPremios(parametro);
                }
                else if(nombre == "Lista De Numeros")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTListaNumero(parametro);
                }
                else if (nombre == "Reportes De Ventas")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTVentas(parametro);
                }
                else if (nombre == "Lista De Tickets")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTListTickets(parametro);
                }
                else if (nombre == "Pagos Remotos")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTPagosRemotos(parametro);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void HeaderReporte(string FechaRepote, string NombreReporte, string Loteria,string Desde,string Hasta)
        {
            //////////////////////////////////////////// Aqui los datos del header//////////////////////////////////////////////////
            var DiaSemanaActual = TraducirDiaSemana(DateTime.Now.DayOfWeek.ToString());
            var DiaSemanaReporte = TraducirDiaSemana(Convert.ToDateTime(FechaRepote).DayOfWeek.ToString());
            var mesAnnoActual = ObtenerMesEspanol(Convert.ToInt32(DateTime.Now.Month));
            var mesAnnoReporte = ObtenerMesEspanol(Convert.ToInt32(Convert.ToDateTime(FechaRepote).Month.ToString()));

            ViewModel.NombreReporte = NombreReporte;

                ViewModel.FechaActualReport = "Del Dia " + DiaSemanaReporte + ", " + Convert.ToDateTime(FechaRepote).Day + "-"+ mesAnnoReporte + "-"+ Convert.ToDateTime(FechaRepote).Year;
                ViewModel.FechaReporte = DiaSemanaActual + ", " + DateTime.Now.Day +"-"+mesAnnoActual +"-"+ DateTime.Now.Year + " " + DateTime.Now.ToShortTimeString();
                ViewModel.LabelDesde = "Desde " + TraducirDiaSemana(Convert.ToDateTime(Desde).DayOfWeek.ToString()) + ", " + Convert.ToDateTime(Desde).ToString("dd-MMM-yyyy");
                ViewModel.LabelHasta = "Hasta " + TraducirDiaSemana(Convert.ToDateTime(Hasta).DayOfWeek.ToString()) + ", " + Convert.ToDateTime(Hasta).ToString("dd-MMM-yyyy");
                ViewModel.NombreLoteria = "Loteria: "+ Loteria;
            
            if (NombreReporte == "TICKETS GANADORES") { ViewModel.NombreLoteria = "Loteria: " + Loteria; }

            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

        private void RPTSumaDeVentas(object parametro)
        {
            try
            {
                var Reporte = ReportesService.ReporteSumVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
                ObservableCollection<ReportesSumVentasObservable> List = new ObservableCollection<ReportesSumVentasObservable>() { };
                if (Reporte.Reglones != null)
                {
                    HeaderReporte(Reporte.Fecha, "SUMA DE VENTAS", null,null,null);

                    ViewModel.RPTSumaVentas = System.Windows.Visibility.Visible;
                    var totalresultados = 0;
                    var totalsaco = 0;
                    var totalcomision = 0;
                    var totalbalance = 0;

                    if (Reporte.Reglones.Length > 0)
                    {
                        for (var i = 0; i <= Reporte.Reglones.Length - 1; i++)
                        {

                            var data = Reporte.Reglones[i];
                            totalresultados = Convert.ToInt32(totalresultados + data.VentaBruta);
                            totalsaco = Convert.ToInt32(totalsaco + data.Saco);
                            totalcomision = Convert.ToInt32(totalcomision + data.Comision);
                            totalbalance = Convert.ToInt32(totalbalance + (data.VentaBruta - data.Comision - data.Saco));

                            ReportesSumVentasObservable objecto = new ModelObservable.ReportesSumVentasObservable()
                            {
                                Concepto = data.Reglon,
                                Comision = (int)data.Comision,
                                Resultado = (int)(data.Resultado + data.Comision + data.Saco),
                                Saco = (int)data.Saco,
                                Balance = Convert.ToInt32(data.VentaBruta - data.Comision - data.Saco)
                            };
                            List.Add(objecto);
                            if (i == Reporte.Reglones.Length - 1)
                            {

                                ViewModel.TotalComision = totalcomision;
                                ViewModel.TotalResultado = totalresultados;
                                ViewModel.TotalSaco = totalsaco;
                                ViewModel.TotalBalance = totalbalance;

                            }
                        }

                        ViewModel.InformacionesReportes = List;
                    }
                    else
                    {
                        MostrarMensajes(Reporte.Err.ToString(), "MAR-Cliente", "INFO");
                    }
                }
                else
                {
                    MostrarMensajes(Reporte.Err.ToString(), "MAR-Cliente", "INFO");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void RPTGanadores(object parametro)
        {

            int loteriaId = new ReporteView().GetLoteriaID();
            var Reporte = ReportesService.ReportesGanadores(Autenticador.CurrentAccount.MAR_Setting2.Sesion, loteriaId,ViewModel.Fecha);
            var ReporteOrdenado = Reporte.Tickets.OrderBy(reporte => reporte.Solicitud).ToArray();

            if (Reporte.Tickets != null)
            {
                if (ReporteOrdenado.Length > 0 || ReporteOrdenado != null)
                {
                    ObservableCollection<ReportesGanadoresObservable> Lista = new ObservableCollection<ReportesGanadoresObservable>() { };
                    HeaderReporte(Reporte.Fecha, "TICKETS GANADORES", new ReporteView().GetNombreLoteria(), null, null);
                    ViewModel.RPTTicketGanadores = System.Windows.Visibility.Visible;
                    var estado = "";
                    int numero = 0;
                    int Total = 0;
                    for (int i = 0; i < ReporteOrdenado.Length; i++) {
                        if (ReporteOrdenado[i].Solicitud == 3) { estado = "Pendientes Por Pagar"; }
                        else if (ReporteOrdenado[i].Solicitud == 5) { estado = "Tickets Pagados"; }
                        else if (ReporteOrdenado[i].Solicitud == 6) { estado = "Tickets Sin Reclamar"; }

                        ReportesGanadoresObservable objectoTicket = new ReportesGanadoresObservable()
                        {
                            Tickets = ReporteOrdenado[i].TicketNo,
                            Fecha = Convert.ToDateTime(ReporteOrdenado[i].StrFecha).ToString("dd-MMM-yyyy") + " " + ReporteOrdenado[i].StrHora,
                            Monto = (double)ReporteOrdenado[i].Pago,
                            Categoria = estado

                        };
                        Total = Total + Convert.ToInt32(ReporteOrdenado[i].Pago);
                        Lista.Add(objectoTicket);

                        if (numero < ReporteOrdenado.Length - 1)
                        {
                            if (ReporteOrdenado[numero].Solicitud != ReporteOrdenado[numero + 1].Solicitud)
                            {
                                ReportesGanadoresObservable Totales = new ReportesGanadoresObservable()
                                {
                                    Tickets = "Total",
                                    Fecha = null,
                                    Monto = Total,
                                    Categoria = estado
                                };
                                Lista.Add(Totales);
                                Total = 0;
                            }
                        }
                        if (numero == ReporteOrdenado.Length - 1)
                        {

                            ReportesGanadoresObservable Totales = new ReportesGanadoresObservable()
                            {
                                Tickets = "Total",
                                Fecha = null,
                                Monto = Total,
                                Categoria = estado
                            };
                            Lista.Add(Totales);
                            Total = 0;
                        }
                        numero = numero + 1;
                    }
                    ViewModel.ReportesGanadores = Lista;
                    ViewModel.ReportesGanadores = null;
                    ViewModel.ReportesGanadores = Lista;
                }else if (ReporteOrdenado.Length==0)
                {
                    MostrarMensajes("No se encontraron Tickets Ganadores en la fecha y loteria seleccionadas", "MAR-Cliente", "INFO");
                }
            }else if (Reporte.Err != null)
            {
                MostrarMensajes(Reporte.Err, "MAR-Cliente", "INFO");
            }
        }

        private  void RPTListasTarjetas(object parametros)
        {
            var reportes = ReportesService.ReporteListaTarjetas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);

            if (reportes.Pines != null)
            {
                if (reportes.Pines.Length > 0)
                {
                    ObservableCollection<ReportesListaTajetasObservable> ListadoTarjetas = new ObservableCollection<ReportesListaTajetasObservable>() { };
                    var ReporteOdenado = reportes.Pines.OrderBy(tarjetas => Convert.ToDateTime(tarjetas.StrHora).Hour.ToString("tt", new System.Globalization.CultureInfo("en-US"))).ToArray();
                    int totalVendido = 0;
                    HeaderReporte(reportes.Fecha, "LISTADO DE PINES", null, null, null);
                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                    var Tarjetas = reportes.Pines;
                    for (int i = 0; i < ReporteOdenado.Length; i++)
                    {
                        ReportesListaTajetasObservable objecto = new ReportesListaTajetasObservable
                        {
                            Suplidor = ReporteOdenado[i].Producto.Suplidor,
                            Hora = ReporteOdenado[i].StrHora,
                            Precio = string.Format(nfi, "{0:C}", Convert.ToDecimal(ReporteOdenado[i].Costo.ToString().ToString())).ToString(),
                            Serie = ReporteOdenado[i].Serie.ToString()
                        };

                        totalVendido = totalVendido + Convert.ToInt32(ReporteOdenado[i].Costo);
                        ListadoTarjetas.Add(objecto);

                        if (i == ReporteOdenado.Length - 1)
                        {
                            ViewModel.TotalVentaListTarjeta = string.Format(nfi, "{0:C}", totalVendido) + " en " + (ReporteOdenado.Length - 1) + " tarjetas";
                        }
                    }

                    ViewModel.ReportesListaTarjetas = ListadoTarjetas;
                    ViewModel.RPTListTarjetas = System.Windows.Visibility.Visible;
                }
                else if(reportes.Pines.Length == 0)
                {
                    MostrarMensajes("No se encontraron tarjetas para la fecha seleccionada", "MAR-Cliente", "ERR");
                }
            }else if (reportes.Err != null)
            {
                MostrarMensajes("No vendio ninguna tarjeta este dia!", "MAR-Cliente", "INFO");
            }
        }

        private void RPTListaPremios(object parametros)
        {
            var reportes = ReportesService.ReporteListaPremios(Autenticador.CurrentAccount.MAR_Setting2.Sesion, 1, ViewModel.Fecha);
        }

        private string ObtenerMesEspanol(int mes)
        {
            var mesEspanol = "";
            if (mes == 1) {mesEspanol = "Ene";}
            else if (mes == 2) { mesEspanol = "Feb"; }
            else if (mes == 3) { mesEspanol = "Mar"; }
            else if (mes == 4) { mesEspanol = "Abri"; }
            else if (mes == 5) { mesEspanol = "May"; }
            else if (mes == 6) { mesEspanol = "Jun"; }
            else if (mes == 7) { mesEspanol = "Jul"; }
            else if (mes == 8) { mesEspanol = "Ago"; }
            else if (mes == 9) { mesEspanol = "Sep"; }
            else if (mes == 10) { mesEspanol = "Oct"; }
            else if (mes == 11) { mesEspanol = "Nov"; }
            else if (mes == 12) { mesEspanol = "Dic"; }
            return mesEspanol;
        }

        private void RPTVentas(object parametro)
        {
            var ReporteVenta = ReportesService.ReporteDeVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);

            if (ReporteVenta.Err == null)
            {
                ViewModel.RPTVentas = Visibility.Visible;
                HeaderReporte(ReporteVenta.Fecha, "REPORTE DE VENTA", "Todas",null,null);
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
                ViewModel.ReportesDeVentas.Numeros = Convert.ToInt32(ReporteVenta.Numeros);
                ViewModel.ReportesDeVentas.NumerosRD = string.Format(nfi, "{0:C}", ReporteVenta.Numeros);
                ViewModel.ReportesDeVentas.Pales = string.Format(nfi, "{0:C}", ReporteVenta.Pales);
                ViewModel.ReportesDeVentas.Tripletas = string.Format(nfi, "{0:C}", ReporteVenta.Tripletas);
                ViewModel.ReportesDeVentas.TotalVentas = string.Format(nfi, "{0:C}", (ReporteVenta.Numeros + ReporteVenta.Pales + ReporteVenta.Tripletas));
                ViewModel.ReportesDeVentas.Comision = string.Format(nfi, "{0:C}", (ReporteVenta.Comision));
                ViewModel.ReportesDeVentas.VentaNeta = string.Format(nfi, "{0:C}", (ReporteVenta.Numeros + ReporteVenta.Pales + ReporteVenta.Tripletas) - ReporteVenta.Comision);

                if (ReporteVenta.TicketsNulos.Length == 0) {
                    ViewModel.ReportesDeVentas.PremiosDisponibles = Visibility.Visible;
                }
                if (ReporteVenta.Primero == "" && ReporteVenta.Segundo == "" && ReporteVenta.Tercero=="")
                {
                    ViewModel.ReportesDeVentas.TicketNulosDisponibles = Visibility.Visible;
                }
            }

        }

        private void RPTVentasFecha(object parametro)
        {
            //Parte de reportes compleja creador Edison Eugenio Pena Ruiz para cualquier consulta //
            var Reporte = ReportesService.ReporteVentasPorFecha(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.FechaInicio, ViewModel.FechaFin);
            ObservableCollection<ReportesSumVentasFechaObservable> List = new ObservableCollection<ReportesSumVentasFechaObservable>() { };
            var ReporteOrdenado = Reporte.Reglones.OrderBy(datos => datos.Reglon).ToArray();
            //Agregar el encabezado de reporte
            HeaderReporte(Reporte.Fecha, "VENTAS POR FECHA", null,ViewModel.FechaInicio,ViewModel.FechaFin);
            //////////////////////////////////////
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            int totales=0, venta=0, saco=0, comision=0, totalGeneralVenta=0, totalGeneralComision=0, totalGeneralSaco=0, totalGeneralBalance = 0;
            int conteo = 0;
            if (ViewModel.SoloTotales == true)
            {
                if (ReporteOrdenado.Length > 0)
                {
                    ViewModel.RPTSumVentaFecha = System.Windows.Visibility.Visible;
                    while (conteo <= ReporteOrdenado.Length-1)
                    {
                            //////////////////////////// Suma de conteos //////////////////////////////
                            venta = venta + Convert.ToInt32(ReporteOrdenado[conteo].VentaBruta);
                            comision = comision + Convert.ToInt32(ReporteOrdenado[conteo].Comision);
                            saco = saco + Convert.ToInt32(ReporteOrdenado[conteo].Saco);
                        ///////////////////////////////////////////////////////////////////////////

                        if (ReporteOrdenado.Length > 1)
                        {
                            if (conteo == 0 && ReporteOrdenado[conteo].Reglon != ReporteOrdenado[conteo + 1].Reglon)
                            {
                                totales = venta - comision - saco;
                                var TotalFormateado = "";
                                if (totales < 0) { TotalFormateado = "$" + totales + ".00"; }
                                else if (totales > 0) { TotalFormateado = string.Format(nfi, "{0:C}", totales).ToString(); }

                                ReportesSumVentasFechaObservable objectoSumaVentas = new ReportesSumVentasFechaObservable()
                                {
                                    Fecha = "Total",
                                    Concepto = ReporteOrdenado[conteo].Reglon,
                                    Resultado = string.Format(nfi, "{0:C}", venta),
                                    Balance = string.Format(nfi, "{0:C}", venta - comision - saco),
                                    Saco = string.Format(nfi, "{0:C}", saco),
                                    Comision = string.Format(nfi, "{0:C}", comision)
                                };
                                List.Add(objectoSumaVentas);
                                //////////////// Calculando totales generales ///////////////////////
                                totalGeneralVenta = totalGeneralVenta + venta;
                                totalGeneralSaco = totalGeneralSaco + saco;
                                totalGeneralComision = totalGeneralComision + comision;
                                totalGeneralBalance = totalGeneralVenta - totalGeneralSaco - totalGeneralComision;
                                /////////////////////////////////////////////////////////////////////
                                venta = 0; comision = 0; saco = 0;
                            }
                        }

                        if (conteo == 0 && ReporteOrdenado.Length==1) {
                            totales = venta - comision - saco;
                            var TotalFormateado = "";
                            if (totales < 0) { TotalFormateado = "$" + totales + ".00"; }
                            else if (totales > 0) { TotalFormateado = string.Format(nfi, "{0:C}", totales).ToString(); }

                            ReportesSumVentasFechaObservable objectoSumaVentas = new ReportesSumVentasFechaObservable()
                            {
                                Fecha = "Total",
                                Concepto = ReporteOrdenado[conteo].Reglon,
                                Resultado = string.Format(nfi, "{0:C}", venta),
                                Balance = string.Format(nfi, "{0:C}", venta - comision - saco),
                                Saco = string.Format(nfi, "{0:C}", saco),
                                Comision = string.Format(nfi, "{0:C}", comision)
                            };
                            List.Add(objectoSumaVentas);
                            //////////////// Calculando totales generales ///////////////////////
                            totalGeneralVenta = totalGeneralVenta + venta;
                            totalGeneralSaco = totalGeneralSaco + saco;
                            totalGeneralComision = totalGeneralComision + comision;
                            totalGeneralBalance = totalGeneralVenta - totalGeneralSaco - totalGeneralComision;
                            /////////////////////////////////////////////////////////////////////
                            TotalesGeneralesFormat(totalGeneralVenta,totalGeneralComision,totalGeneralSaco,totalGeneralBalance);
                            venta = 0; comision = 0; saco = 0;
                        }

                        if ((conteo > 0) && (ReporteOrdenado[conteo].Reglon != ReporteOrdenado[conteo - 1].Reglon) || (conteo == ReporteOrdenado.Length - 1))
                        {
                            //////////////////////////// Suma de conteos //////////////////////////////
                            if (conteo != ReporteOrdenado.Length - 1) { venta = venta - Convert.ToInt32(ReporteOrdenado[conteo].VentaBruta); } //Si conteo es diferente a cantidad de datos  

                            comision = comision - Convert.ToInt32(ReporteOrdenado[conteo].Comision);
                            saco = saco - Convert.ToInt32(ReporteOrdenado[conteo].Saco);
                            ///////////////////////////////////////////////////////////////////////////
                            totales = venta - comision - saco;
                            var TotalFormateado = "";
                            if (totales < 0) { TotalFormateado = "$" + totales + ".00"; }
                            else if (totales > 0) { TotalFormateado = string.Format(nfi, "{0:C}", totales).ToString(); }

                            //////////////// Calculando totales generales ///////////////////////
                            totalGeneralVenta = totalGeneralVenta + venta;
                            totalGeneralSaco = totalGeneralSaco + saco;
                            totalGeneralComision = totalGeneralComision + comision;
                            totalGeneralBalance = totalGeneralVenta - totalGeneralSaco - totalGeneralComision;
                            /////////////////////////////////////////////////////////////////////

                            if (conteo < ReporteOrdenado.Length-1 && ReporteOrdenado.Length > 1) { 
                                ReportesSumVentasFechaObservable objectoSumaVentas = new ReportesSumVentasFechaObservable()
                                {
                                    Fecha = "Total",
                                    Concepto = ReporteOrdenado[conteo-1].Reglon,
                                    Resultado = string.Format(nfi, "{0:C}", venta),
                                    Balance = TotalFormateado,
                                    Saco = string.Format(nfi, "{0:C}", saco),
                                    Comision = string.Format(nfi, "{0:C}", comision)
                                };
                                List.Add(objectoSumaVentas);
                            }
                            //////////////////////////// Restablecer valores //////////////////////////////
                            venta = Convert.ToInt32(ReporteOrdenado[conteo].VentaBruta);comision =Convert.ToInt32(ReporteOrdenado[conteo].Comision);saco = Convert.ToInt32(ReporteOrdenado[conteo].Saco);
                            ///////////////////////////////////////////////////////////////////////////
                        }
                       
                        TotalesGeneralesFormat(totalGeneralVenta, totalGeneralComision, totalGeneralSaco, totalGeneralBalance);
                       
                        conteo = conteo + 1;
                        ViewModel.ReportesSumVentasPorFecha = List;
                        ViewModel.ReportesSumVentasPorFecha = null;
                        ViewModel.ReportesSumVentasPorFecha = List;

                    }
                    
                }
            }
            else if(ViewModel.SoloTotales == false)
            {
                var Datos = ReporteOrdenado;
                if (Datos.Length > 0) {
                    ViewModel.RPTSumVentaFecha = System.Windows.Visibility.Visible;
                    for (int i =0; i < Datos.Length;i++) {

                        venta = venta + Convert.ToInt32(Datos[i].VentaBruta);
                        comision = comision + Convert.ToInt32(Datos[i].Comision);
                        saco = saco + Convert.ToInt32(Datos[i].Saco);

                        if (Datos[i].VentaBruta > 0) { totales = totales + Convert.ToInt32(Datos[i].VentaBruta); } 
                        else if (Datos[i].VentaBruta < 0) { totales = totales - Convert.ToInt32(Datos[i].VentaBruta); }

                       var balance = Datos[i].VentaBruta - Datos[i].Comision - Datos[i].Saco;
                       var balanceFormat = "";

                       if (balance > 0){balanceFormat = string.Format(nfi, "{0:C}", balance);}
                       else if (balance < 0) { balanceFormat = "$" + balance; };

                        ReportesSumVentasFechaObservable objectoSumaVentas = new ReportesSumVentasFechaObservable()
                        {
                        Fecha = TraducirDiaSemanaResumido(Convert.ToDateTime(Datos[i].Fecha).DayOfWeek.ToString()) + " " + Convert.ToDateTime(Datos[i].Fecha).Day + " " + ObtenerMesEspanol(Convert.ToDateTime(Datos[i].Fecha).Month),
                        Concepto = Datos[i].Reglon,
                        Resultado = string.Format(nfi, "{0:C}", Datos[i].VentaBruta),
                        Balance = balanceFormat,
                        Saco = string.Format(nfi, "{0:C}", Datos[i].Saco),
                        Comision = string.Format(nfi, "{0:C}", Datos[i].Comision)

                    };

                        if (Datos.Length == 1)
                        {
                            totalGeneralVenta = totalGeneralVenta + venta;
                            totalGeneralComision = totalGeneralComision + comision;
                            totalGeneralSaco = totalGeneralSaco + saco;
                            totalGeneralBalance = totalGeneralVenta - totalGeneralComision - totalGeneralSaco;
                            TotalesGeneralesFormat(totalGeneralVenta, totalGeneralComision, totalGeneralSaco, totalGeneralBalance);
                        }
                    List.Add(objectoSumaVentas);

                        if (i < Datos.Length - 1) {
                            if (Datos[i].Reglon != Datos[i + 1].Reglon)
                            {
                                totalGeneralVenta = totalGeneralVenta + venta;
                                totalGeneralComision = totalGeneralComision + comision;
                                totalGeneralSaco = totalGeneralSaco + saco;
                                totalGeneralBalance = totalGeneralVenta - totalGeneralComision - totalGeneralSaco;

                                var TotalFormateado = "";
                                totales = venta - comision - saco;
                                if (totales < 0) { TotalFormateado = "$" + totales + ".00"; }
                                else if (totales > 0) { TotalFormateado = string.Format(nfi, "{0:C}", totales).ToString(); }
                                ReportesSumVentasFechaObservable objectoTotalSumaVentas = new ReportesSumVentasFechaObservable()
                                { Concepto = Datos[i].Reglon, Fecha = "Total", Balance = string.Format(nfi, "{0:C}", TotalFormateado), Comision = string.Format(nfi, "{0:C}", comision), Saco = string.Format(nfi, "{0:C}", saco), Resultado = string.Format(nfi, "{0:C}", venta) };
                                venta = 0; comision = 0; saco = 0; totales = 0;
                                List.Add(objectoTotalSumaVentas);
                            }

                        } else
                        {
                            if (Datos.Length > 1)
                            {
                                if (Datos[i].Reglon == Datos[i - 1].Reglon)
                                {
                                    totalGeneralVenta = totalGeneralVenta + venta;
                                    totalGeneralComision = totalGeneralComision + comision;
                                    totalGeneralSaco = totalGeneralSaco + saco;
                                    totalGeneralBalance = totalGeneralVenta - totalGeneralComision - totalGeneralSaco;

                                    ReportesSumVentasFechaObservable objectoTotalSumaVentass = new ReportesSumVentasFechaObservable()
                                    { Concepto = Datos[i].Reglon, Fecha = "Total", Balance = string.Format(nfi, "{0:C}", totales), Comision = string.Format(nfi, "{0:C}", comision), Saco = string.Format(nfi, "{0:C}", saco), Resultado = string.Format(nfi, "{0:C}", venta) };
                                    venta = 0; comision = 0; saco = 0; balance = 0;
                                    List.Add(objectoTotalSumaVentass);
                                }
                                else
                                {
                                    totalGeneralVenta = totalGeneralVenta + venta;
                                    totalGeneralComision = totalGeneralComision + comision;
                                    totalGeneralSaco = totalGeneralSaco + saco;
                                    totalGeneralBalance = totalGeneralVenta - totalGeneralComision - totalGeneralSaco;


                                    totales = Convert.ToInt32(Datos[i].VentaBruta) - Convert.ToInt32(Datos[i].Saco) - Convert.ToInt32(Datos[i].Comision);
                                    ReportesSumVentasFechaObservable objectoTotalSumaVentas = new ReportesSumVentasFechaObservable()
                                    {
                                        Concepto = Datos[i].Reglon,
                                        Fecha = "Total",
                                        Balance = string.Format(nfi, "{0:C}", totales),
                                        Comision = string.Format(nfi, "{0:C}", comision),
                                        Saco = string.Format(nfi, "{0:C}", saco),
                                        Resultado = string.Format(nfi, "{0:C}", venta)
                                    };
                                    venta = 0; comision = 0; saco = 0; balance = 0;
                                    List.Add(objectoTotalSumaVentas);
                                }
                            }
                            else {
                                
                                totales = Convert.ToInt32(Datos[i].VentaBruta) - Convert.ToInt32(Datos[i].Saco) - Convert.ToInt32(Datos[i].Comision);
                                ReportesSumVentasFechaObservable objectoTotalSumaVentas = new ReportesSumVentasFechaObservable()
                                {
                                    Concepto = Datos[i].Reglon,
                                    Fecha = "Total",
                                    Balance = string.Format(nfi, "{0:C}", totales),
                                    Comision = string.Format(nfi, "{0:C}", comision),
                                    Saco = string.Format(nfi, "{0:C}", saco),
                                    Resultado = string.Format(nfi, "{0:C}", venta)
                                };
                                venta = 0; comision = 0; saco = 0; balance = 0;
                                List.Add(objectoTotalSumaVentas);
                            }

                        }
                        ViewModel.ReportesSumVentasPorFecha = List;
                        ViewModel.ReportesSumVentasPorFecha = null;
                        ViewModel.ReportesSumVentasPorFecha = List;

                        TotalesGeneralesFormat(totalGeneralVenta, totalGeneralComision, totalGeneralSaco, totalGeneralBalance);
                }
            }
                    
           }
        }

        private void TotalesGeneralesFormat(int totalVentaGeneral,int totalComisionGeneral,int totalSacoGeneral,int totalBalanceGeneral)
        {
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            if (totalVentaGeneral >= 0){ViewModel.TotalVentasSumVenFecha = string.Format(nfi, "{0:C}", totalVentaGeneral);}
            else if (totalVentaGeneral <= 0) { ViewModel.TotalVentasSumVenFecha = "$" + totalVentaGeneral; }

            if (totalComisionGeneral >= 0) { ViewModel.TotalComisSumVenFecha = string.Format(nfi, "{0:C}", totalComisionGeneral); }
            else if (totalComisionGeneral <= 0) { ViewModel.TotalComisSumVenFecha = "$" + totalComisionGeneral; }

            if (totalSacoGeneral >= 0) { ViewModel.TotalSacoSumVenFecha = string.Format(nfi, "{0:C}", totalSacoGeneral); }
            else if (totalSacoGeneral <= 0) { ViewModel.TotalSacoSumVenFecha = "$" + totalSacoGeneral; }

            if (totalBalanceGeneral >= 0) { ViewModel.TotalBalanSumVenFecha = string.Format(nfi, "{0:C}", totalBalanceGeneral); }
            else if (totalBalanceGeneral <= 0) { ViewModel.TotalBalanSumVenFecha = "$" + totalBalanceGeneral; }

        }

        private void RPTListaNumero(object parametro)
        {
            int loteriaId = new ReporteView().GetLoteriaID();
            var Reporte = ReportesService.ReporteListadoNumero(Autenticador.CurrentAccount.MAR_Setting2.Sesion, loteriaId, ViewModel.Fecha);
            ReporteListNumeroColumns ReporteListNumeros = new ReporteListNumeroColumns();
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            var Numeros = Reporte.Numeros;
            var NumerosQuinielas = Reporte.Numeros.Where(ticket => ticket.QP == "Q").ToList();
            var NumerosPales = Reporte.Numeros.Where(ticket => ticket.QP == "P").ToList();
            var NumerosTripleta = Reporte.Numeros.Where(ticket => ticket.QP == "T").ToList();
            ReporteListNumeros.Quiniela = new ObservableCollection<ReportesListaNumerosObservable>() { };
            ReporteListNumeros.Tripleta = new ObservableCollection<ReportesListaNumerosObservable>() { };
            ReporteListNumeros.Pale = new ObservableCollection<ReportesListaNumerosObservable>() { };

            if (NumerosQuinielas.Count == 1)
            {
                ReportesListaNumerosObservable ObjectoQuiniela = new ReportesListaNumerosObservable()
                {
                    NumeroColumn1 = NumerosQuinielas[0].Numero,
                    CantidadColumn1 = string.Format(nfi,"{0:C}",NumerosQuinielas[0].Cantidad),
                    
                };
                ReporteListNumeros.Quiniela.Add(ObjectoQuiniela);
                ViewModel.ReportesListaNumeros.Quiniela = ReporteListNumeros.Quiniela;
            }
            else
            {
                for (int i = 0; i < NumerosQuinielas.Count; i = i + 2)
                {

                    if (i + 2 <= NumerosQuinielas.Count - 1)
                    {
                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                            CantidadColumn1 = string.Format(nfi,"{0:C}",NumerosQuinielas[i].Cantidad),

                            NumeroColumn2 = NumerosQuinielas[i + 1].Numero,
                            CantidadColumn2 = string.Format(nfi, "{0:C}",NumerosQuinielas[i + 1].Cantidad),

                            NumeroColumn3 = NumerosQuinielas[i + 2].Numero,
                            CantidadColumn3 = string.Format(nfi, "{0:C}",NumerosQuinielas[i + 2].Cantidad)
                        };
                        ReporteListNumeros.Quiniela.Add(objectoQuiniela);
                        ViewModel.ReportesListaNumeros.Quiniela = ReporteListNumeros.Quiniela;
                        continue;
                    }
                    if (i + 1 <= NumerosQuinielas.Count - 1)
                    {
                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}",NumerosQuinielas[i].Cantidad),
                            TipoJugada = NumerosQuinielas[i].QP,
                            NumeroColumn2 = NumerosQuinielas[i + 1].Numero,
                            CantidadColumn2 = string.Format(nfi, "{0:C}",NumerosQuinielas[i +1].Cantidad),

                        };
                        ReporteListNumeros.Quiniela.Add(objectoQuiniela);
                        ViewModel.ReportesListaNumeros.Quiniela = ReporteListNumeros.Quiniela;
                        continue;
                    }

                    if (i  <= NumerosQuinielas.Count-1)
                    {
                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}",NumerosQuinielas[i])
                        };
                        ReporteListNumeros.Quiniela.Add(objectoQuiniela);
                        ViewModel.ReportesListaNumeros.Quiniela = ReporteListNumeros.Quiniela;
                        continue;
                    }
                }
            }
            if (NumerosPales.Count == 1)
            {
                ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                {
                    NumeroColumn1 = NumerosPales[0].Numero,
                    CantidadColumn1 = NumerosPales[0].Cantidad.ToString()
                   
                };
                ReporteListNumeros.Pale.Add(objectoPale);
                ViewModel.ReportesListaNumeros.Pale = ReporteListNumeros.Pale;
            }
            else {
                for (int i = 0; i < NumerosPales.Count; i = i + 2)
                {
                    if (i + 2 <= NumerosPales.Count - 1)
                    {
                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosPales[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}", NumerosPales[i].Cantidad),
                            NumeroColumn2 = NumerosPales[i + 1].Numero,
                            CantidadColumn2 = string.Format(nfi, "{0:C}",NumerosPales[i + 1].Cantidad),
                            NumeroColumn3 = NumerosPales[i + 2].Numero,
                            CantidadColumn3 = string.Format(nfi, "{0:C}",NumerosPales[i + 2].Cantidad)
                        };
                        ReporteListNumeros.Pale.Add(objectoPale);
                        ViewModel.ReportesListaNumeros.Pale = ReporteListNumeros.Pale;
                        continue;
                    }
                    if (i + 1 <= NumerosPales.Count - 1)
                    {
                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosPales[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}", NumerosPales[i].Cantidad),

                            NumeroColumn2 = NumerosPales[i + 1].Numero,
                            CantidadColumn2 = string.Format(nfi, "{0:C}", NumerosPales[i + 1].Cantidad),

                        };
                        ReporteListNumeros.Pale.Add(objectoPale);
                        ViewModel.ReportesListaNumeros.Pale = ReporteListNumeros.Pale;
                        continue;
                    }
                    if (i  <= NumerosPales.Count - 1)
                    {
                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosPales[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}",NumerosPales[i].Cantidad)
                        };
                        ReporteListNumeros.Pale.Add(objectoPale);
                        ViewModel.ReportesListaNumeros.Pale = ReporteListNumeros.Pale;
                        continue;
                    }
                }
            }
            if (NumerosTripleta.Count == 1)
            {
                ReportesListaNumerosObservable ObjectoTripleta = new ReportesListaNumerosObservable()
                {
                    NumeroColumn1 = NumerosTripleta[0].Numero,
                    CantidadColumn1 = string.Format(nfi, "{0:C}",NumerosTripleta[0].Cantidad)
                };
                ReporteListNumeros.Tripleta.Add(ObjectoTripleta);
                ViewModel.ReportesListaNumeros.Tripleta = ReporteListNumeros.Tripleta;
            }
            else
            {
                for (int i = 0; i < NumerosTripleta.Count; i = i + 2)
                {
                    if (i + 2 <= NumerosTripleta.Count - 1)
                    {
                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosTripleta[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}",NumerosTripleta[i].Cantidad),
                            NumeroColumn2 = NumerosTripleta[i + 1].Numero,
                            CantidadColumn2 = string.Format(nfi, "{0:C}",NumerosPales[i + 1].Cantidad),
                            NumeroColumn3 = NumerosTripleta[i + 2].Numero,
                            CantidadColumn3 = string.Format(nfi, "{0:C}",NumerosTripleta[i + 2].Cantidad)
                        };
                        ReporteListNumeros.Tripleta.Add(objectoTripleta);
                        ViewModel.ReportesListaNumeros.Tripleta = ReporteListNumeros.Tripleta;
                        continue;
                    }
                    if (i + 1 <= NumerosTripleta.Count - 1)
                    {
                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosTripleta[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}",NumerosTripleta[i].Cantidad),
                            NumeroColumn2 = NumerosTripleta[i + 1].Numero,
                            CantidadColumn2 = string.Format(nfi, "{0:C}",NumerosTripleta[i + 1].Cantidad)
                        };
                        ReporteListNumeros.Tripleta.Add(objectoTripleta);
                        ViewModel.ReportesListaNumeros.Tripleta = ReporteListNumeros.Tripleta;
                        continue;
                    }
                    if (i <= NumerosTripleta.Count-1)
                    {
                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                        {
                            NumeroColumn1 = NumerosTripleta[i].Numero,
                            CantidadColumn1 = string.Format(nfi, "{0:C}",NumerosTripleta[i].Cantidad)
                        };
                        ReporteListNumeros.Tripleta.Add(objectoTripleta);
                        ViewModel.ReportesListaNumeros.Tripleta = ReporteListNumeros.Tripleta;
                        continue;
                    }
                }
            }
            
           }

        private void RPTListTickets(object parametros)
        {
            MessageBoxResult opcionTicketAnulados = MessageBox.Show("Desea listar todos los tickets?,Reponda NO para listar solamente los tickets nulos","MAR-Cliente",MessageBoxButton.YesNo);
            var ReporteTicket = ReportesService.ReporteListaDeTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            ObservableCollection<ReporteListaTicketsObservable> ListadoTicket = new ObservableCollection<ReporteListaTicketsObservable>() { };
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            int totalVenta = 0; int totalSaco=0;

            if (ReporteTicket.Tickets.Length > 0)
            {
                ViewModel.RPTLitTicket = Visibility.Visible;
                HeaderReporte(ReporteTicket.Fecha, "LISTADO DE TICKETS", "Todas", null, null);

            switch (opcionTicketAnulados)
            {
                case MessageBoxResult.Yes:
                    foreach (var ticket in ReporteTicket.Tickets)
                    {
                            var saco = "";
                            if (ticket.Nulo == true) { saco = "Nulo"; } else if (ticket.Nulo == false) { saco = string.Format(nfi, "{0:C}", ticket.Pago); } 
                                ReporteListaTicketsObservable objectoTicket = new ReporteListaTicketsObservable()
                                { Ticket = ticket.TicketNo, Hora = ticket.StrHora, Vendio = string.Format(nfi,"{0:C}",ticket.Costo), Saco = saco};
                                ListadoTicket.Add(objectoTicket);
                    }
                    
                    break;
                case MessageBoxResult.No:
                  var TicketsFiltrados = ReporteTicket.Tickets.Where(ticket => ticket.Nulo == true).ToArray();
                    foreach (var ticket in TicketsFiltrados)
                    {
                        ReporteListaTicketsObservable objectoTicket = new ReporteListaTicketsObservable()
                        { Ticket = ticket.TicketNo, Hora = ticket.StrHora, Vendio = string.Format(nfi,"{0:C}",ticket.Costo), Saco = "Nulo" };
                         objectoTicket.MostrarNulos = Visibility.Visible;
                         ListadoTicket.Add(objectoTicket);
                    }
                break;    
            }
                foreach (var ticket in ReporteTicket.Tickets)
                {
                    if (ticket.Nulo == false) { 
                        totalVenta = totalVenta + Convert.ToInt32(ticket.Costo);
                        totalSaco = totalSaco + Convert.ToInt32(ticket.Pago);
                    }
                }

                ViewModel.ReporteListTicket = ListadoTicket;
                var TicketsNulo = ReporteTicket.Tickets.Where(ticket => ticket.Nulo == true).ToArray();
                ViewModel.TotalesListTicket.CantidadNulos = TicketsNulo.Length.ToString();
                ViewModel.TotalesListTicket.CantidadValidos = (ReporteTicket.Tickets.Length - TicketsNulo.Length).ToString();
                ViewModel.TotalesListTicket.TotalVenta = string.Format(nfi,"{0:C}",totalVenta);
                ViewModel.TotalesListTicket.TotalSaco = string.Format(nfi, "{0:C}", totalSaco);
            }
        }

        private void RPTPagosRemotos(object parametros)
        {
            var PagosRemotos = ReportesService.ReporteListaPagosRemotos(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
            ObservableCollection<ReporteListaTicketsObservable> ListadoPagosRemotos = new ObservableCollection<ReporteListaTicketsObservable>() { };
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            var PagosRemotoData = PagosRemotos.Tickets;
            var totalPagosRemotos = 0;
            string balance = "";
            string totalbalance = "";

            if (PagosRemotos.Err == null)
            {
                if (PagosRemotoData.Length > 0 && PagosRemotos.Err == null)
                {
                    HeaderReporte(PagosRemotos.Fecha, "TICKET PAGADOS REMOTAMENTE", null, null, null);
                    for (int i = 0; i < PagosRemotoData.Length; i++)
                    {
                        ViewModel.RPTPagosRemotos = System.Windows.Visibility.Visible;
                        totalPagosRemotos = totalPagosRemotos + Convert.ToInt32(PagosRemotoData[i].Pago);
                        if (PagosRemotoData[i].Pago >= 0) { balance = string.Format(nfi, "{0:C}", PagosRemotoData[i].Pago);}
                        else if (PagosRemotoData[i].Pago < 0) { balance = "$" + PagosRemotoData[i].Pago + ".00"; };

                        ReporteListaTicketsObservable ObjectPagoRemoto = new ReporteListaTicketsObservable()
                        {
                            Ticket = PagosRemotoData[i].TicketNo,
                            Hora = PagosRemotoData[i].StrHora,
                            Vendio = balance,
                            Saco = PagosRemotoData[i].Cliente

                        };
                        ListadoPagosRemotos.Add(ObjectPagoRemoto);
                    }
                    if (totalPagosRemotos >= 0) { totalbalance = string.Format(nfi, "{0:C}", totalPagosRemotos); }
                    else if (totalPagosRemotos < 0) {totalbalance = "$"+totalPagosRemotos+".00"; }
                    ViewModel.ReportePagosRemotos = ListadoPagosRemotos;
                    ViewModel.TotalesPagosRemotos = totalbalance + " en " + PagosRemotoData.Length + " tickets";
                }

            }
            else if (PagosRemotos.Err != null)
            {
                MostrarMensajes(PagosRemotos.Err, "MAR-Cliente", "INFO");
            }
        }

        private void MostrarMensajes(string mensaje,string Titulo,string TipoMensaje)
        {
            if (TipoMensaje == "ERR")
            {
                System.Windows.MessageBox.Show(mensaje, Titulo, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }else if (TipoMensaje == "INFO")
            {
                System.Windows.MessageBox.Show(mensaje, Titulo, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
            }
            else if (TipoMensaje == "Warning")
            {
                System.Windows.MessageBox.Show(mensaje, Titulo, System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
            }

        }
    }

}
