﻿using ClienteMarWPFWin7.Domain.Models.Base;
using ClienteMarWPFWin7.Domain.Services.ReportesService;
using ClienteMarWPFWin7.UI.Modules.Reporte;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Reporte;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;
using System.Runtime.InteropServices.ComTypes;
using System.Windows.Controls;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using System.Runtime.Serialization;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System.Drawing;
using ClienteMarWPFWin7.Domain.Services.JuegaMasService;
using ClienteMarWPFWin7.Domain.JuegaMasService;
using MAR.AppLogic.MARHelpers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Reporte
{
    public class GetReportesCommand : ActionCommand
    {
        private readonly ReporteViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IReportesServices ReportesService;
        private readonly IJuegaMasService servicioJuegamas;
        DateTime FechaInicio;
        DateTime FechaFin;

        public GetReportesCommand(ReporteViewModel viewModel, IAuthenticator autenticador, IReportesServices reportesServices, IJuegaMasService juegaMasService)
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

            Action<object> comando = new Action<object>(EnviarReportes);
            base.SetAction(comando);
        }



        private void EnviarReportes(object parametro)
        {
            //Ocultar Templates de reportes
            ViewModel.RPTSumaVentasVisibility = Visibility.Hidden;
            ViewModel.RPTTicketGanadoresVisibility = Visibility.Hidden;
            ViewModel.RPTSumVentaFechaVisibility = Visibility.Hidden;
            ViewModel.RPTListTarjetasVisibility = Visibility.Hidden;
            ViewModel.RPTVentasVisibily = Visibility.Hidden;
            ViewModel.RPTLitTicketVisibility = Visibility.Hidden;
            ViewModel.RPTPagosRemotosVisibility = Visibility.Hidden;
            ViewModel.RPTListNumerosVisibility = Visibility.Hidden;
            ViewModel.RPTListPremioVisibility = Visibility.Hidden;
            ViewModel.PremiosVentas.MostrarPremios = Visibility.Hidden;
            ViewModel.PremiosVentas.NoMostrarPremios = Visibility.Hidden;
            ViewModel.ReportesListaNumeros.QuinielaVisibilty = Visibility.Hidden;
            ViewModel.ReportesListaNumeros.PaleVisibility = Visibility.Hidden;
            ViewModel.ReportesListaNumeros.TripletaVisibility = Visibility.Hidden;
            ViewModel.ReportesGanadores.MostrarNoHayGanadoresVisibity = Visibility.Hidden;

            try
            {
                var nombre = new ReporteView().GetReporteNombre();
                if (nombre == null || nombre == "") { nombre = "Reportes De Ventas"; }
                if (nombre == "Suma De Ventas")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Loading Reporte Suma De Ventas
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTSumaDeVentas(parametro); ViewModel.LoadingRPTSumaVentas = Visibility.Visible; ViewModel.IconRPTSumaVentas = Visibility.Collapsed; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }
                else if (nombre == "Reportes Ganadores")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Loading Reporte Ventas por Fecha
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTGanadores(parametro); ViewModel.LoadingRPTGanadores = Visibility.Visible; ViewModel.IconRPTGanadores = Visibility.Collapsed; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }
                else if (nombre == "Ventas por Fecha")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Loading Reporte Ventas por Fecha
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTVentasFecha(parametro); }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }
                else if (nombre == "Lista De Tarjetas")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Loading Reporte Lista De Tarjetas
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTListasTarjetas(parametro); ViewModel.LoadingRPTListTarjeta = Visibility.Visible; ViewModel.IconRPTListTarjeta = Visibility.Collapsed; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                } else if (nombre == "Lista De Premios")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Loading Reporte Lista De Premios
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTListaPremios(parametro); ViewModel.LoadingRPTListPremiosGanadores = Visibility.Visible; ViewModel.IconRPTListPremiosGanadores = Visibility.Collapsed; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }
                else if (nombre == "Lista De Numeros")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Loading Reporte List Numero
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTListaNumero(parametro); ViewModel.LoadingRPTListNumero = Visibility.Visible; ViewModel.IconRPTListNumero = Visibility.Collapsed; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }
                else if (nombre == "Reportes De Ventas")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Loading Reporte Ventas
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTVentas(parametro); ViewModel.IconRPTVentas = Visibility.Collapsed; ViewModel.LoadingRPTVentas = Visibility.Visible; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }
                else if (nombre == "Lista De Tickets")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Lista De Tickets
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTListTickets(parametro); ViewModel.LoadingRPTListTicket = Visibility.Visible; ViewModel.IconRPTListTicket = Visibility.Collapsed; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }
                else if (nombre == "Pagos Remotos")
                {
                    ViewModel.NombreBanca = "Lexus";
                    //Pagos Remotos
                    Task.Factory.StartNew(() => {
                        Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                            try { RPTPagosRemotos(parametro); ViewModel.LoadingRPTPagosRemotos = Visibility.Visible; ViewModel.IconRPTPagosRemotos = Visibility.Collapsed; }
                            catch
                            {
                                MessageBox.Show("No se pudo consultar al servidor,Revise su conexion(F1) e intente nuevamente!", "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
                            }
                        }));
                    });
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                MessageBox.Show("HA OCURRIDO UN ERROR,POR FAVOR COMUNICARSE CON EL DEPARTAMENTO DE SOPORTE!\n\n Error ocurrido: "+e.Message, "Cliente-MAR", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.ServiceNotification);
            }

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

        private void HeaderReporte(DateTime FechaRepote, string NombreReporte, string Loteria, DateTime Desde=new DateTime(), DateTime Hasta= new DateTime())
        {
            //////////////////////////////////////////// Aqui los datos del header//////////////////////////////////////////////////
            var DiaSemanaActual = TraducirDiaSemana(DateTime.Now.DayOfWeek.ToString());
            var DiaSemanaReporte = TraducirDiaSemana(FechaRepote.DayOfWeek.ToString());
            var mesAnnoActual = ObtenerMesEspanol(Convert.ToInt32(DateTime.Now.Month));
            var mesAnnoReporte = ObtenerMesEspanol(Convert.ToInt32(FechaRepote.Month.ToString()));
            ViewModel.NombreBanca = (Autenticador.BancaConfiguracion.BancaDto.BanContacto + "  ID:" + Autenticador.BancaConfiguracion.BancaDto.BancaID).ToUpper();

            ViewModel.NombreReporte = NombreReporte.ToUpper();

            ViewModel.FechaActualReport = ("Del Dia " + DiaSemanaReporte + " " + FechaRepote.Day + "-" + mesAnnoReporte + "-" + FechaRepote.Year ).ToUpper();
            ViewModel.FechaReporteLabel = ( DiaSemanaActual + ", " + DateTime.Now.Day + "-" + mesAnnoActual + "-" + DateTime.Now.Year + " " + DateTime.Now.ToShortTimeString()).ToUpper();
            ViewModel.LabelDesde = ("Desde " + TraducirDiaSemana(Desde.DayOfWeek.ToString()) + ", " + Desde.ToString("dd-MMM-yyyy")).ToUpper();
            ViewModel.LabelHasta = ("Hasta " + TraducirDiaSemana(Hasta.DayOfWeek.ToString()) + ", " + Hasta.ToString("dd-MMM-yyyy")).ToUpper();
            ViewModel.NombreLoteria = ("Loteria: " + Loteria).ToUpper();
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

        private void RPTSumaDeVentas(object parametro)
        {
            try
            {
                var Reporte = ReportesService.ReporteSumVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion,ViewModel.Fecha);
                ObservableCollection<ReportesSumVentasObservable> List = new ObservableCollection<ReportesSumVentasObservable>() { };
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                if (Reporte.Reglones != null)
                {
                    HeaderReporte(Convert.ToDateTime(Reporte.Fecha), "SUMA DE VENTAS", null);

                    ViewModel.RPTSumaVentasVisibility = System.Windows.Visibility.Visible;
                    var totalresultados = 0;
                    var totalsaco = 0;
                    var totalcomision = 0;
                    var totalbalance = 0;

                    if (Reporte.Reglones.Length > 0)
                    {
                        for (var i = 0; i <= Reporte.Reglones.Length - 1; i++)
                        {

                            var data = Reporte.Reglones[i];
                            var balancesporfilas = "";
                            totalresultados = Convert.ToInt32(totalresultados + data.VentaBruta);
                            totalsaco = Convert.ToInt32(totalsaco + data.Saco);
                            totalcomision = Convert.ToInt32(totalcomision + data.Comision);
                            totalbalance = Convert.ToInt32(totalbalance + (data.VentaBruta - data.Comision - data.Saco));
                            int balance = Convert.ToInt32(data.VentaBruta - data.Comision - data.Saco);
                            CultureInfo daDK = CultureInfo.CreateSpecificCulture("da-DK");


                            ReportesSumVentasObservable objecto = new ModelObservable.ReportesSumVentasObservable()
                            {
                                Concepto = data.Reglon,
                                Comision = (int) data.Comision,
                                Resultado = (int)(data.Resultado + data.Comision + data.Saco),
                                Saco = (int)data.Saco,
                                Balance = balance
                            };
                            List.Add(objecto);
                            if (i == Reporte.Reglones.Length - 1)
                            {
                                var BalanceFinal = "";
                                if (totalbalance >= 0) { BalanceFinal = string.Format(nfi, "{0:C}", totalbalance); }
                                else if (totalbalance < 0) { BalanceFinal = ConvertirMonedaNegativos(totalbalance); }

                                ViewModel.TotalComision = string.Format(nfi, "{0:C}", totalcomision);
                                ViewModel.TotalResultado = string.Format(nfi, "{0:C}", totalresultados);
                                ViewModel.TotalSaco = string.Format(nfi, "{0:C}", totalsaco);
                                ViewModel.TotalBalance ="$"+ string.Format(CultureInfo.InvariantCulture,"{0:0,0.00}",totalbalance);

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
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTSumaVentas = Visibility.Collapsed; ViewModel.IconRPTSumaVentas = Visibility.Visible; } catch { } })); });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Cliente Mar",MessageBoxButton.OK,MessageBoxImage.Error);
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTSumaVentas = Visibility.Collapsed; ViewModel.IconRPTSumaVentas = Visibility.Visible; } catch { } })); });

            }

        }

        private void RPTGanadores(object parametro)
        {
            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var Reporte = ReportesService.ReportesGanadores(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            if (Reporte.Err == null) { 
           
            ViewModel.ReportesGanadores = new EstadoDeTicketGanadores();
            ViewModel.ReportesGanadores.PendientesPagar = new ObservableCollection<ReportesGanadoresObservable>() { };
            ViewModel.ReportesGanadores.Pagados = new ObservableCollection<ReportesGanadoresObservable>() { };
            ViewModel.ReportesGanadores.SinReclamar = new ObservableCollection<ReportesGanadoresObservable>() { };

                if (Reporte.Tickets != null)
                {
                    if (Reporte.Tickets.Length > 0) { 
                    EstadoDeTicketGanadores Ganadores = new EstadoDeTicketGanadores() { };
                    NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat;

                    HeaderReporte(Convert.ToDateTime(Reporte.Fecha), "TICKETS GANADORES", new ReporteView().GetNombreLoteria());
                    ViewModel.RPTTicketGanadoresVisibility = Visibility.Visible;

                    var TicketPendientePagos = Reporte.Tickets.Where(ticket => ticket.Solicitud == 3);
                    if (Reporte.Tickets.Where(ticket => ticket.Solicitud == 3).ToList().Count() > 0)
                    {
                        TicketPendientePagos = Reporte.Tickets.Where(ticket => ticket.Solicitud == 3);
                    }else if (Reporte.Tickets.Where(ticket => ticket.Solicitud == 4).ToList().Count() > 0)
                    {
                        TicketPendientePagos = Reporte.Tickets.Where(ticket => ticket.Solicitud == 4);
                    }
                        
                        var TicketSinReclamar = Reporte.Tickets.Where(ticket => ticket.Solicitud == 6);
                        var TicketPagados = Reporte.Tickets.Where(ticket => ticket.Solicitud == 5);



                        if (TicketPagados.Count() > 0 && TicketPendientePagos.Count() > 0 && TicketSinReclamar.Count() > 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloPagados = 2;
                            ViewModel.ReportesGanadores.PosicionTablaPagados = 3;
                            ViewModel.ReportesGanadores.PosicionTituloPendientesPagos = 4;
                            ViewModel.ReportesGanadores.PosicionTablaPendientesPagos = 5;
                            ViewModel.ReportesGanadores.PosicionTituloSinReclamar = 6;
                            ViewModel.ReportesGanadores.PosicionTablaSinReclamar = 7;
                            ViewModel.ReportesGanadores.PosicionBalance = 8;
                        }
                        if (TicketPagados.Count() > 0 && TicketPendientePagos.Count() == 0 && TicketSinReclamar.Count() == 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloPagados = 2;
                            ViewModel.ReportesGanadores.PosicionTablaPagados = 3;
                            ViewModel.ReportesGanadores.PosicionBalance = 4;
                            ViewModel.ReportesGanadores.HeightTicketNoGanadores = 10;
                        }
                        if (TicketPagados.Count() == 0 && TicketPendientePagos.Count() == 0 && TicketSinReclamar.Count() == 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloPagados = 3;
                            ViewModel.ReportesGanadores.PosicionTablaPagados = 4;
                            ViewModel.ReportesGanadores.PosicionTituloPendientesPagos = 5;
                            ViewModel.ReportesGanadores.PosicionTablaPendientesPagos = 6;
                            ViewModel.ReportesGanadores.PosicionBalance = 9;
                            ViewModel.ReportesGanadores.MostrarNoHayGanadoresVisibity = Visibility.Visible;
                            ViewModel.ReportesGanadores.HeightTicketNoGanadores = 50;
                        }
                        if (TicketPagados.Count() > 0 && TicketPendientePagos.Count() > 0 && TicketSinReclamar.Count() == 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloPagados = 2;
                            ViewModel.ReportesGanadores.PosicionTablaPagados = 3;
                            ViewModel.ReportesGanadores.PosicionTituloPendientesPagos = 4;
                            ViewModel.ReportesGanadores.PosicionTablaPendientesPagos = 5;
                            ViewModel.ReportesGanadores.PosicionBalance = 6;
                        }
                        else
                        {
                            ViewModel.ReportesGanadores.MostrarNoHayGanadoresVisibity = Visibility.Hidden;
                            ViewModel.ReportesGanadores.HeightTicketNoGanadores = 10;
                        }
                        if (TicketPagados.Count() == 0 && TicketPendientePagos.Count() > 0 && TicketSinReclamar.Count() == 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloPendientesPagos = 2;
                            ViewModel.ReportesGanadores.PosicionTablaPendientesPagos = 3;
                            ViewModel.ReportesGanadores.PosicionBalance = 4;
                        }
                        if (TicketPagados.Count() == 0 && TicketPendientePagos.Count() == 0 && TicketSinReclamar.Count() > 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloSinReclamar = 2;
                            ViewModel.ReportesGanadores.PosicionTablaSinReclamar = 3;
                            ViewModel.ReportesGanadores.PosicionBalance = 4;
                        }
                        if (TicketPagados.Count() == 0 && TicketPendientePagos.Count() > 0 && TicketSinReclamar.Count() > 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloPendientesPagos = 2;
                            ViewModel.ReportesGanadores.PosicionTablaPendientesPagos = 3;
                            ViewModel.ReportesGanadores.PosicionTituloSinReclamar = 4;
                            ViewModel.ReportesGanadores.PosicionTablaSinReclamar = 5;
                            ViewModel.ReportesGanadores.PosicionBalance = 6;
                        }
                        if (TicketPagados.Count() == 0 && TicketPendientePagos.Count() == 0 && TicketSinReclamar.Count() > 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloSinReclamar = 2;
                            ViewModel.ReportesGanadores.PosicionTablaSinReclamar = 3;
                            ViewModel.ReportesGanadores.PosicionBalance = 4;
                        }
                        if (TicketPagados.Count() > 0 && TicketPendientePagos.Count() == 0 && TicketSinReclamar.Count() > 0)
                        {
                            ViewModel.ReportesGanadores.PosicionTituloPagados = 2;
                            ViewModel.ReportesGanadores.PosicionTablaPagados = 3;
                            ViewModel.ReportesGanadores.PosicionTituloSinReclamar = 4;
                            ViewModel.ReportesGanadores.PosicionTablaSinReclamar = 5;
                            ViewModel.ReportesGanadores.PosicionBalance = 6;
                        }

                        ViewModel.ReportesGanadores.PendientesPagar.Clear();
                        ViewModel.ReportesGanadores.SinReclamar.Clear();
                        ViewModel.ReportesGanadores.Pagados.Clear();

                        var TotalPendientePagos = 0;
                        var TotalPagados = 0;
                        var TotalSinReclamar = 0;

                        bool EliminarTicketPagos = false;
                        bool EliminarTicketPendientePagos = false;
                        bool EliminarTicketSinReclamar = false;

                        if (Reporte.Primero != null || Reporte.Segundo != null || Reporte.Tercero != null)
                        {
                            ViewModel.ReportesGanadores.MostrarPremiosVisibity = Visibility.Visible;
                            ViewModel.ReportesGanadores.NoMostrarPremiosVisibity = Visibility.Hidden;

                            if (Reporte.Primero != "") { ViewModel.ReportesGanadores.Primera = Reporte.Primero; } else { ViewModel.ReportesGanadores.Primera = "--"; }
                            if (Reporte.Segundo != "") { ViewModel.ReportesGanadores.Segunda = Reporte.Segundo; } else { ViewModel.ReportesGanadores.Segunda = "--"; }
                            if (Reporte.Tercero != "") { ViewModel.ReportesGanadores.Tercera = Reporte.Tercero; } else { ViewModel.ReportesGanadores.Tercera = "--"; }
                        }
                        else { ViewModel.ReportesGanadores.MostrarPremiosVisibity = Visibility.Hidden; ViewModel.ReportesGanadores.NoMostrarPremiosVisibity = Visibility.Visible; }

                        if (TicketPagados.Count() > 0) { ViewModel.ReportesGanadores.PagadosVisibility = Visibility.Visible; EliminarTicketPagos = true; }
                        else if (TicketPagados.Count() == 0) { ViewModel.ReportesGanadores.PagadosVisibility = Visibility.Hidden; }

                        if (TicketPendientePagos.Count() > 0) { ViewModel.ReportesGanadores.PendientePagarVisibility = Visibility.Visible; EliminarTicketPendientePagos = false; }
                        else if (TicketPendientePagos.Count() == 0) { ViewModel.ReportesGanadores.PendientePagarVisibility = Visibility.Hidden; }

                        if (TicketSinReclamar.Count() > 0) { ViewModel.ReportesGanadores.SinReclamarVisibility = Visibility.Visible; EliminarTicketSinReclamar = false; }
                        else if (TicketSinReclamar.Count() == 0) { ViewModel.ReportesGanadores.SinReclamarVisibility = Visibility.Hidden; }

                        new ReporteView().EliminandoTemplateGanadores(EliminarTicketPagos, EliminarTicketPendientePagos, EliminarTicketSinReclamar);
                    
                    
                    foreach (var pendientepago in TicketPendientePagos)
                    {
                        TotalPendientePagos = TotalPendientePagos + Convert.ToInt32(pendientepago.Pago);
                        
                            ReportesGanadoresObservable Modelo = new ReportesGanadoresObservable() { Fecha = Convert.ToDateTime(pendientepago.StrFecha).ToString("dd-MMM-yyyy") + " " + pendientepago.StrHora, Monto = (int)pendientepago.Pago, Tickets = pendientepago.TicketNo };
                            ViewModel.ReportesGanadores.PendientesPagar.Add(Modelo);
                        
                    }

                    ReportesGanadoresObservable ModeloTotalesPendientePagos = new ReportesGanadoresObservable() { Fecha = null, Tickets = "Total", Monto = TotalPendientePagos };
                    ViewModel.ReportesGanadores.PendientesPagar.Add(ModeloTotalesPendientePagos);

                    foreach (var pagadoGroup in TicketPagados)
                         {
                             TotalPagados = TotalPagados + Convert.ToInt32(pagadoGroup.Pago);
                             ReportesGanadoresObservable Modelo = new ReportesGanadoresObservable() { Fecha = Convert.ToDateTime(pagadoGroup.StrFecha).ToString("dd-MMM-yyyy") + " " + pagadoGroup.StrHora, Monto = (int)pagadoGroup.Pago, Tickets = pagadoGroup.TicketNo };
                             ViewModel.ReportesGanadores.Pagados.Add(Modelo);
                         }
                            ReportesGanadoresObservable ModeloTotalesPagados = new ReportesGanadoresObservable() { Fecha = null, Tickets = "Total", Monto = TotalPagados };
                            ViewModel.ReportesGanadores.Pagados.Add(ModeloTotalesPagados);

                    foreach (var sinreclamar in TicketSinReclamar)
                    {
                        
                            TotalSinReclamar = TotalSinReclamar + Convert.ToInt32(sinreclamar.Pago);
                            ReportesGanadoresObservable Modelo = new ReportesGanadoresObservable() { Fecha = Convert.ToDateTime(sinreclamar.StrFecha).ToString("dd-MMM-yyyy") + " " + sinreclamar.StrHora, Monto = (int)sinreclamar.Pago, Tickets = sinreclamar.TicketNo };
                            ViewModel.ReportesGanadores.SinReclamar.Add(Modelo);
                        
                    }
                    ReportesGanadoresObservable ModeloTotalesSinReclamar = new ReportesGanadoresObservable() { Fecha = null, Tickets = "Total", Monto = TotalSinReclamar };
                    ViewModel.ReportesGanadores.SinReclamar.Add(ModeloTotalesSinReclamar);



                    /* foreach (var pendientepago in TicketPendientePagos.GroupBy(x => x.TicketNo).ToList())
                         {
                             TotalPendientePagos = TotalPendientePagos + Convert.ToInt32(pendientepago.Sum(x => x.Items.Sum(y => y.Pago)));
                             foreach (var pendientepagado in pendientepago)
                             {
                                 ReportesGanadoresObservable Modelo = new ReportesGanadoresObservable() { Fecha = Convert.ToDateTime(pendientepagado.StrFecha).ToString("dd-MMM-yyyy") + " " + pendientepagado.StrHora, Monto = (int)pendientepagado.Items.Sum(x => x.Pago), Tickets = pendientepagado.TicketNo };
                                 ViewModel.ReportesGanadores.PendientesPagar.Add(Modelo);
                             }
                         }
                         ReportesGanadoresObservable ModeloTotalesPendientePagos = new ReportesGanadoresObservable() { Fecha = null, Tickets = "Total", Monto = TotalPendientePagos };
                         ViewModel.ReportesGanadores.PendientesPagar.Add(ModeloTotalesPendientePagos);



                     foreach (var pagados in TicketPagados.GroupBy(x => x.TicketNo))
                     {

                         foreach (var pagadoGroup in pagados)
                         {
                             TotalPagados = TotalPagados + Convert.ToInt32(pagadoGroup.Items.Sum(x => x.Pago));
                             ReportesGanadoresObservable Modelo = new ReportesGanadoresObservable() { Fecha = Convert.ToDateTime(pagadoGroup.StrFecha).ToString("dd-MMM-yyyy") + " " + pagadoGroup.StrHora, Monto = (int)pagadoGroup.Items.Sum(x => x.Pago), Tickets = pagadoGroup.TicketNo };
                             ViewModel.ReportesGanadores.Pagados.Add(Modelo);
                         }
                     }

                     ReportesGanadoresObservable ModeloTotalesPagados = new ReportesGanadoresObservable() { Fecha = null, Tickets = "Total", Monto = TotalPagados };
                     ViewModel.ReportesGanadores.Pagados.Add(ModeloTotalesPagados);

                     foreach (var sinreclamar in TicketSinReclamar.GroupBy(x => x.TicketNo))
                     {
                         foreach (var sinReclamarGroup in sinreclamar)
                         {
                             TotalSinReclamar = TotalSinReclamar + Convert.ToInt32(sinReclamarGroup.Items.Sum(x => x.Pago));
                             ReportesGanadoresObservable Modelo = new ReportesGanadoresObservable() { Fecha = Convert.ToDateTime(sinReclamarGroup.StrFecha).ToString("dd-MMM-yyyy") + " " + sinReclamarGroup.StrHora, Monto = (int)sinReclamarGroup.Items.Sum(x => x.Pago), Tickets = sinReclamarGroup.TicketNo };
                             ViewModel.ReportesGanadores.SinReclamar.Add(Modelo);
                         }
                     }
                     ReportesGanadoresObservable ModeloTotalesSinReclamar = new ReportesGanadoresObservable() { Fecha = null, Tickets = "Total", Monto = TotalSinReclamar };
                     ViewModel.ReportesGanadores.SinReclamar.Add(ModeloTotalesSinReclamar);*/

                        ViewModel.ReportesGanadores.TotalGanadores = string.Format(nfi, "{0:C}", TotalPagados + TotalPendientePagos - TotalSinReclamar);
                    }
                    else if (Reporte.Tickets.Length == 0)
                    {
                         MessageBox.Show("No existen tickets ganadores,pendientes de pago o sin reclamar correspondientes a la fecha y opcion de loteria seleccionada.", "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);
                        Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTGanadores = Visibility.Collapsed; ViewModel.IconRPTGanadores = Visibility.Visible; } catch { } })); });
                        return;
                    }
                }else if (Reporte.Tickets==null)
                {
                    MessageBox.Show("No existen tickets ganadores,pendientes de pago o sin reclamar correspondientes a la fecha y opcion de loteria seleccionada.", "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);
                    Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTGanadores = Visibility.Collapsed; ViewModel.IconRPTGanadores = Visibility.Visible; } catch { } })); });
                    return;
                }
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTGanadores = Visibility.Collapsed; ViewModel.IconRPTGanadores = Visibility.Visible; } catch { } })); });

            }
            else if (Reporte.Err != null)
            {
                MostrarMensajes(Reporte.Err, "MAR-Cliente", "INFO");
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTGanadores = Visibility.Collapsed; ViewModel.IconRPTGanadores = Visibility.Visible; } catch { } })); });
            }
        }

        private void RPTListasTarjetas(object parametros)
        {
            var reportes = ReportesService.ReporteListaTarjetas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);

            if (reportes.Err == null)
            {
                if (reportes.Pines != null && reportes.Pines.Length > 0)
                {
                    ObservableCollection<ReportesListaTajetasObservable> ListadoTarjetas = new ObservableCollection<ReportesListaTajetasObservable>() { };
                    var ReporteOdenado = reportes.Pines.OrderBy(tarjetas => Convert.ToDateTime(tarjetas.StrHora).Hour.ToString("tt", new System.Globalization.CultureInfo("en-US"))).ToArray();
                    int totalVendido = 0;
                    HeaderReporte(Convert.ToDateTime(reportes.Fecha), "LISTADO DE PINES", null);
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
                            ViewModel.TotalVentaListTarjeta = string.Format(nfi, "{0:C}", totalVendido);// + " en " + (ReporteOdenado.Length) + " tarjetas"
                        }
                    }

                    ViewModel.ReportesListaTarjetas = ListadoTarjetas;
                    ViewModel.RPTListTarjetasVisibility = System.Windows.Visibility.Visible;
                    Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListTarjeta = Visibility.Collapsed; ViewModel.IconRPTListTarjeta = Visibility.Visible; } catch { } })); });

                }
                else if (reportes.Pines == null)
                {
                    MostrarMensajes("No se encontraron tarjetas para la fecha seleccionada", "MAR-Cliente", "INFO");
                    Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListTarjeta = Visibility.Collapsed; ViewModel.IconRPTListTarjeta = Visibility.Visible; } catch { } })); });
                }
            } else if (reportes.Err != null)
            {
                MostrarMensajes(reportes.Err, "MAR-Cliente", "INFO");
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListTarjeta = Visibility.Collapsed; ViewModel.IconRPTListTarjeta = Visibility.Visible; } catch { } })); });
            }
        }

        private void RPTListaPremios(object parametros)
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
            if (reportes.Respuesta != null)
            {
                var resultado = JsonConvert.DeserializeObject<ClienteMarWPFWin7.Domain.JuegaMasService.MAR_JuegaMasResponse>(reportes.Respuesta);

                ViewModel.ObservableListadoPremios = "";
                if (reportes.Err == null)
                {
                    if (resultado.OK == true)
                    {
                        ViewModel.RPTListPremioVisibility = System.Windows.Visibility.Visible;
                        var arrayJeison = JsonConvert.DeserializeObject(reportes.Respuesta);
                        ModelSerializePremios ModeloPremios = JsonConvert.DeserializeObject<ModelSerializePremios>(arrayJeison.ToString());
                        printData = ModeloPremios.PrintData;
                        for (int o = 0; o < printData.Count(); o++)
                        {
                            var premio = printData[o];
                            var valor = premio[0].ToString();

                            ViewModel.ObservableListadoPremios = ViewModel.ObservableListadoPremios + "\n";
                            ViewModel.ObservableListadoPremios = ViewModel.ObservableListadoPremios + valor;
                        }
                    }
                    else
                    {
                        ViewModel.RPTListPremioVisibility = System.Windows.Visibility.Hidden;
                    }
                }
                else if (reportes.Err != null)
                {
                    MessageBox.Show(reportes.Err.ToString(), "MAR-Cliente", MessageBoxButton.OK, MessageBoxImage.Information);
                    Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListPremiosGanadores = Visibility.Collapsed; ViewModel.IconRPTListPremiosGanadores = Visibility.Visible; } catch { } })); });
                }
            }
            else
            {
                MessageBox.Show("Ha ocurrido un fallo al crear reporte de lista de premios!", "MAR-Cliente", MessageBoxButton.OK, MessageBoxImage.Information);
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListPremiosGanadores = Visibility.Collapsed; ViewModel.IconRPTListPremiosGanadores = Visibility.Visible; } catch { } })); });
            }
            Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListPremiosGanadores = Visibility.Collapsed; ViewModel.IconRPTListPremiosGanadores = Visibility.Visible; } catch { } })); });
        }
        //var data = DeserializarJuegaMas(respuesta);


        public static Object DeserializarString(string json)
        {
            return JsonConvert.DeserializeObject<Object>(json);
        }

        public static ModelSerializePremios DeserializarJuegaMas(string json)
        {
            return JsonConvert.DeserializeObject<ModelSerializePremios>(json);
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

        private string ConvertirMonedaNegativos(int cantidad)
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
                LuegoComa = cantidad.ToString().Substring(2, (cantidad.ToString().Length)-2);
                CantidadConvertido = AnteComa + LuegoComa;

            }
            if (cantidad.ToString().Length == 6)
            {
                AnteComa = cantidad.ToString().Substring(0, 3) + ",";
                LuegoComa = cantidad.ToString().Substring(3, cantidad.ToString().Length-3);
                CantidadConvertido = AnteComa + LuegoComa;

            }
            if (cantidad.ToString().Length == 7)
            {
                var segundaComa = "";
                AnteComa = cantidad.ToString().Substring(0, 4) + ",";
                segundaComa = cantidad.ToString().Substring(4, 3);
                LuegoComa = cantidad.ToString().Substring(7, cantidad.ToString().Length-7);
                CantidadConvertido = AnteComa + segundaComa;
            }
            if (cantidad.ToString().Length == 8)
            {
                var segundaComa = "";
                AnteComa = cantidad.ToString().Substring(0, 2) + ",";
                segundaComa = cantidad.ToString().Substring(2, 3)+",";
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


            return "$" +CantidadConvertido+".00";
        }

        private void RPTVentas(object parametro)
        {
            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var ReporteVenta = ReportesService.ReporteDeVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);

            if (ReporteVenta.Err == null)
            {
                ViewModel.RPTVentasVisibily = Visibility.Visible;
                HeaderReporte(Convert.ToDateTime(ReporteVenta.Fecha), "REPORTE DE VENTA",nombreLoteria);
                NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
                ViewModel.ReportesDeVentas.Numeros = Convert.ToInt32(ReporteVenta.Numeros);
                ViewModel.ReportesDeVentas.NumerosRD = string.Format(nfi, "{0:C}", ReporteVenta.Numeros);
                ViewModel.ReportesDeVentas.Pales = string.Format(nfi, "{0:C}", ReporteVenta.Pales);
                ViewModel.ReportesDeVentas.Tripletas = string.Format(nfi, "{0:C}", ReporteVenta.Tripletas);
                ViewModel.ReportesDeVentas.TotalVentas = string.Format(nfi, "{0:C}", (ReporteVenta.Numeros + ReporteVenta.Pales + ReporteVenta.Tripletas));
                ViewModel.ReportesDeVentas.Comision = string.Format(nfi, "{0:C}", (ReporteVenta.Comision));
                ViewModel.ReportesDeVentas.VentaNeta = string.Format(nfi, "{0:C}", (ReporteVenta.Numeros + ReporteVenta.Pales + ReporteVenta.Tripletas) - ReporteVenta.Comision);
                
                //La Parte De Premios
                ViewModel.PremiosVentas.Primera = ReporteVenta.Primero;
                ViewModel.PremiosVentas.Segunda = ReporteVenta.Segundo;
                ViewModel.PremiosVentas.Tercera = ReporteVenta.Tercero;
                ViewModel.PremiosVentas.Cantidad1RA = string.Format(nfi,"{0:C}",ReporteVenta.CPrimero);
                ViewModel.PremiosVentas.Cantidad2DA = string.Format(nfi, "{0:C}",ReporteVenta.CSegundo);
                ViewModel.PremiosVentas.Cantidad3RA = string.Format(nfi, "{0:C}", ReporteVenta.CTercero);
                ViewModel.PremiosVentas.Monto1RA = string.Format(nfi, "{0:C}", ReporteVenta.MPrimero);
                ViewModel.PremiosVentas.Monto2DA = string.Format(nfi, "{0:C}", ReporteVenta.MSegundo);
                ViewModel.PremiosVentas.Monto3RA = string.Format(nfi, "{0:C}", ReporteVenta.MTercero);


                if ((ReporteVenta.Primero != "") || (ReporteVenta.Segundo != "") || (ReporteVenta.Tercero != ""))
                {
                    ViewModel.PremiosVentas.MostrarPremios = Visibility.Visible;
                    ViewModel.PremiosVentas.NoMostrarPremios = Visibility.Hidden;
                    ViewModel.ReportesDeVentas.PosicionTicketNulo = 18;
                }
                else
                {
                    ViewModel.PremiosVentas.MostrarPremios = Visibility.Hidden;
                    ViewModel.PremiosVentas.NoMostrarPremios = Visibility.Visible;
                    ViewModel.ReportesDeVentas.PosicionTicketNulo = 10;
                    
                }
                //////////////////////////////////////////////////////////////////////////////////////

                if (ReporteVenta.TicketsNulos.Length == 0) {
                    ViewModel.ReportesDeVentas.PremiosDisponibles = Visibility.Visible;
                }
                if (ReporteVenta.TicketsNulos.Length > 0)
                 {
                     ViewModel.ReportesDeVentas.TicketNulosDisponibles = Visibility.Visible;
                     ViewModel.ReportesDeVentas.TicketNulosNoDisponibles = Visibility.Hidden;
                 }
                if (ReporteVenta.TicketsNulos.Length == 0)
                {
                    ViewModel.ReportesDeVentas.TicketNulosDisponibles = Visibility.Hidden;
                    ViewModel.ReportesDeVentas.TicketNulosNoDisponibles = Visibility.Visible;
                }

                var TotalGanancia = (Convert.ToInt32((ReporteVenta.Numeros + ReporteVenta.Pales + ReporteVenta.Tripletas) - ReporteVenta.Comision)) - (Convert.ToInt32((ReporteVenta.MPrimero+ReporteVenta.MSegundo+ReporteVenta.MTercero) + ReporteVenta.MPales + ReporteVenta.MTripletas));
                string TotalFormateado=null;
                if (TotalGanancia >= 0)
                {
                  TotalFormateado  = string.Format(nfi, "{0:C}", TotalGanancia);
                    ViewModel.ReportesDeVentas.GananciaOPerdida = "GANANCIA:";
                }
                else if (TotalGanancia < 0){
                    TotalFormateado = ConvertirMonedaNegativos(TotalGanancia);
                    ViewModel.ReportesDeVentas.GananciaOPerdida = "PERDIDA:";
                }
                ViewModel.PremiosVentas.TotalNumerosPremiados = string.Format(nfi,"{0:C}",Convert.ToInt32(ReporteVenta.MPrimero + ReporteVenta.MSegundo + ReporteVenta.MTercero));
                ViewModel.PremiosVentas.TotalPalesPremiados = string.Format(nfi,"{0:C}",ReporteVenta.MPales);
                ViewModel.PremiosVentas.TotalTripletaPremiados = string.Format(nfi,"{0:C}",ReporteVenta.MTripletas);
                ViewModel.PremiosVentas.TotalPremiados = string.Format(nfi,"{0:C}", Convert.ToInt32(ReporteVenta.MPrimero + ReporteVenta.MSegundo + ReporteVenta.MTercero) + Convert.ToInt32(ReporteVenta.MPales + ReporteVenta.MTripletas));
                ViewModel.PremiosVentas.TotalGanancia = TotalFormateado;

                ViewModel.TicketNulosReporteVentas = new ObservableCollection<ReporteListaTicketsObservable>() { };

                for (var i=0; i < ReporteVenta.TicketsNulos.Length;i++)
                {
                    var TicketMarBet = ReporteVenta.TicketsNulos[i];
                    ReporteListaTicketsObservable Ticket = new ReporteListaTicketsObservable() { Ticket = TicketMarBet.TicketNo, Hora = TicketMarBet.StrHora, MostrarNulos = Visibility.Hidden, Vendio =(int) TicketMarBet.Costo, Saco = "0", Nulo = false };
                    ViewModel.TicketNulosReporteVentas.Add(Ticket);
                }
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.IconRPTVentas = Visibility.Visible; ViewModel.LoadingRPTVentas = Visibility.Collapsed; } catch { } })); });
            }
            else
            {
                MostrarMensajes(ReporteVenta.Err, "MAR-Cliente", "INFO");
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.IconRPTVentas = Visibility.Visible; ViewModel.LoadingRPTVentas = Visibility.Collapsed; } catch { } })); });
            }
        }

        private void RPTVentasFecha(object parametro)
        {

            //Parte de reportes compleja creador Edison Eugenio Pena Ruiz para cualquier consulta //
            var Reporte = ReportesService.ReporteVentasPorFecha(Autenticador.CurrentAccount.MAR_Setting2.Sesion,FechaInicio, ViewModel.FechaFin);
            ObservableCollection<ReportesSumVentasFechaObservable> List = new ObservableCollection<ReportesSumVentasFechaObservable>() { };
            //Agregar el encabezado de reporte
            HeaderReporte(Convert.ToDateTime(Reporte.Fecha), "VENTAS POR FECHA", null,ViewModel.FechaInicio,ViewModel.FechaFin);
            //////////////////////////////////////
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            int venta=0, saco=0, comision=0, totalGeneralVenta=0, totalGeneralComision=0, totalGeneralSaco=0, totalGeneralBalance = 0;
            if (Reporte.Err == null)
            {

            
            if (ViewModel.SoloTotales == true)
            {
                var LoteriasExistentes = Reporte.Reglones.Select(reporte => reporte.Reglon).Distinct();
                if (LoteriasExistentes.Count() > 0)
                {
                    int balance = 0;
                    string formatbalance = "";
                    ViewModel.RPTSumVentaFechaVisibility = System.Windows.Visibility.Visible;
                    foreach (var loteria in LoteriasExistentes)
                    {
                        var datosloteria = Reporte.Reglones.Where(datos => datos.Reglon == loteria);
                        foreach (var info in datosloteria)
                        {
                            venta = venta + Convert.ToInt32(info.VentaBruta);
                            comision = comision + Convert.ToInt32(info.Comision);
                            saco = saco + Convert.ToInt32(info.Saco);
                        }

                        totalGeneralVenta = totalGeneralVenta + venta;
                        totalGeneralSaco = totalGeneralSaco + saco;
                        totalGeneralComision = totalGeneralComision + comision;
                        totalGeneralBalance = totalGeneralVenta - totalGeneralSaco - totalGeneralComision;
                        balance = venta - comision - saco;
                        if (balance < 0) { formatbalance = string.Format(nfi, "{0:C}", balance); }
                        else if (balance >= 0) { formatbalance = string.Format(nfi, "{0:C}", balance);}

                        ReportesSumVentasFechaObservable objectoSumaVentas = new ReportesSumVentasFechaObservable()
                        {
                            Fecha = "Total",
                            Concepto = loteria,
                            Resultado =  venta,
                            Balance = balance,
                            Saco =  saco,
                            Comision = comision
                        };
                        List.Add(objectoSumaVentas);
                        ViewModel.ReportesSumVentasPorFecha = List;
                        ViewModel.ReportesSumVentasPorFecha = null;
                        ViewModel.ReportesSumVentasPorFecha = List;
                        ViewModel.ReportesSumVentasPorFecha = null;
                        ViewModel.ReportesSumVentasPorFecha = List;
                        TotalesGeneralesFormat(totalGeneralVenta, totalGeneralComision, totalGeneralSaco, totalGeneralBalance);

                        ////////////////////////// Restablecer valores //////////////////////////////
                            venta = 0;comision =0;saco = 0;
                        ///////////////////////////////////////////////////////////////////////////
                    }
                }
               
            }
            else if(ViewModel.SoloTotales == false)
            {
                var LoteriasExistentes = Reporte.Reglones.Select(reporte => reporte.Reglon).Distinct();
                if (LoteriasExistentes.Count() > 0)
                {
                    int balance = 0;
                    string formatbalance = "";
                    ViewModel.RPTSumVentaFechaVisibility = System.Windows.Visibility.Visible;
                    foreach (var loteria in LoteriasExistentes)
                    {
                        var datosloteria = Reporte.Reglones.Where(datos => datos.Reglon == loteria).ToArray();
                        for (int i=0; i < datosloteria.Count();i++)
                        {
                            venta = venta + Convert.ToInt32(datosloteria[i].VentaBruta);
                            comision = comision + Convert.ToInt32(datosloteria[i].Comision);
                            saco = saco + Convert.ToInt32(datosloteria[i].Saco);
                           
                            balance = Convert.ToInt32(datosloteria[i].VentaBruta - datosloteria[i].Comision - datosloteria[i].Saco);
                            if (balance < 0) { formatbalance = ConvertirMonedaNegativos(balance); }
                            else if (balance >= 0) { formatbalance = string.Format(nfi, "{0:C}", balance); }
                            ReportesSumVentasFechaObservable objectoSumaVentas = new ReportesSumVentasFechaObservable()
                            {
                                Fecha = TraducirDiaSemanaResumido(Convert.ToDateTime(datosloteria[i].Fecha).DayOfWeek.ToString()) + " " + Convert.ToDateTime(datosloteria[i].Fecha).Day + " " + ObtenerMesEspanol(Convert.ToDateTime(datosloteria[i].Fecha).Month),
                                Concepto = datosloteria[i].Reglon,
                                Resultado = (int) datosloteria[i].VentaBruta,
                                Balance = balance,
                                Saco = (int)datosloteria[i].Saco,
                                Comision = (int) datosloteria[i].Comision
                            };
                            List.Add(objectoSumaVentas);
                            if (i == datosloteria.Length-1)
                            {
                                balance = venta - comision - saco;
                                if (balance < 0) { formatbalance = ConvertirMonedaNegativos(balance); }
                                else if (balance >= 0) { formatbalance = string.Format(nfi, "{0:C}", balance); }

                                ReportesSumVentasFechaObservable objectoSumaVentasF = new ReportesSumVentasFechaObservable()
                                {
                                    Fecha = "Total",
                                    Concepto = datosloteria[i].Reglon,
                                    Resultado = venta,
                                    Balance = balance,
                                    Saco = saco,
                                    Comision = comision
                                };
                                List.Add(objectoSumaVentasF);
                            }
                        }
                        totalGeneralVenta = totalGeneralVenta + venta;
                        totalGeneralSaco = totalGeneralSaco + saco;
                        totalGeneralComision = totalGeneralComision + comision;
                        totalGeneralBalance = totalGeneralVenta - totalGeneralSaco - totalGeneralComision;
                        TotalesGeneralesFormat(totalGeneralVenta, totalGeneralComision, totalGeneralSaco, totalGeneralBalance);

                        ViewModel.ReportesSumVentasPorFecha = List;
                        ViewModel.ReportesSumVentasPorFecha = null;
                        ViewModel.ReportesSumVentasPorFecha = List;
                        venta = 0; comision = 0; saco = 0;
                    }
                }
                }
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTVentasPorFecha = Visibility.Collapsed; } catch { } })); });
            }
            else if (Reporte.Err != null)
            {
                MessageBox.Show(Reporte.Err.ToString(), "MAR-Cliente", MessageBoxButton.OK, MessageBoxImage.Information);
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTVentasPorFecha = Visibility.Collapsed; } catch { } })); });
                
            }
            Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTVentasPorFecha = Visibility.Collapsed; } catch { } })); });

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
            else if (totalBalanceGeneral < 0) { ViewModel.TotalBalanSumVenFecha = ConvertirMonedaNegativos(totalBalanceGeneral); }

        }
        private void RPTListaNumero(object parametro)
        {
            
            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var Reporte = ReportesService.ReporteListadoNumero(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);

            if (Reporte.Err == null)
            {
                try {
                    if (Reporte.Numeros != null) {
                        if (Reporte.Numeros.Count() > 0)
                        {
                            ReporteListNumeroColumns ReporteListNumeros = new ReporteListNumeroColumns();
                            HeaderReporte(Convert.ToDateTime(Reporte.Fecha), "LISTADO DE NUMEROS", nombreLoteria);
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
                            {
                                ViewModel.RPTListNumerosVisibility = System.Windows.Visibility.Visible;
                            }

                            if (NumerosQuinielas.Count > 0 && NumerosPales.Count > 0 && NumerosTripleta.Count > 0)
                            {
                                ViewModel.ReportesListaNumeros.PosicionTituloQuiniela = 1;
                                ViewModel.ReportesListaNumeros.PosicionTablaQuiniela = 2;
                                ViewModel.ReportesListaNumeros.PosicionTotalesQuiniela = 3;
                                ViewModel.ReportesListaNumeros.PosicionTituloPale = 4;
                                ViewModel.ReportesListaNumeros.PosicionTablaPale = 5;
                                ViewModel.ReportesListaNumeros.PosicionTotalesPale = 6;
                                ViewModel.ReportesListaNumeros.PosicionTituloTripleta = 7;
                                ViewModel.ReportesListaNumeros.PosicionTablaTripleta = 8;
                                ViewModel.ReportesListaNumeros.PosicionTotalesTripleta = 9;
                            }
                            if (NumerosQuinielas.Count == 0 && NumerosPales.Count > 0 && NumerosTripleta.Count > 0)
                            {
                                ViewModel.ReportesListaNumeros.PosicionTituloPale = 1;
                                ViewModel.ReportesListaNumeros.PosicionTablaPale = 2;
                                ViewModel.ReportesListaNumeros.PosicionTotalesPale = 3;
                                ViewModel.ReportesListaNumeros.PosicionTituloTripleta = 4;
                                ViewModel.ReportesListaNumeros.PosicionTablaTripleta = 5;
                                ViewModel.ReportesListaNumeros.PosicionTotalesTripleta = 6;
                            }
                            if (NumerosQuinielas.Count == 0 && NumerosPales.Count == 0 && NumerosTripleta.Count > 0)
                            {
                                ViewModel.ReportesListaNumeros.PosicionTituloTripleta = 1;
                                ViewModel.ReportesListaNumeros.PosicionTablaTripleta = 2;
                                ViewModel.ReportesListaNumeros.PosicionTotalesTripleta = 3;
                            }
                            if (NumerosQuinielas.Count > 0 && NumerosPales.Count == 0 && NumerosTripleta.Count > 0)
                            {
                                ViewModel.ReportesListaNumeros.PosicionTituloQuiniela = 1;
                                ViewModel.ReportesListaNumeros.PosicionTablaQuiniela = 2;
                                ViewModel.ReportesListaNumeros.PosicionTotalesQuiniela = 3;
                                ViewModel.ReportesListaNumeros.PosicionTituloTripleta = 4;
                                ViewModel.ReportesListaNumeros.PosicionTablaTripleta = 5;
                                ViewModel.ReportesListaNumeros.PosicionTotalesTripleta = 6;
                            }
                            if (NumerosQuinielas.Count == 0 && NumerosPales.Count > 0 && NumerosTripleta.Count == 0)
                            {
                                ViewModel.ReportesListaNumeros.PosicionTituloPale = 1;
                                ViewModel.ReportesListaNumeros.PosicionTablaPale = 2;
                                ViewModel.ReportesListaNumeros.PosicionTotalesPale = 3;
                            }

                            if (NumerosQuinielas.Count > 1)
                            {
                                var totalCantidadQuiniela = 0;
                                var totalPagoQuiniela = 0;
                                ViewModel.ReportesListaNumeros.QuinielaVisibilty = Visibility.Visible;


                                for (int i = 0; i < NumerosQuinielas.Count; i = i + 3)
                                {

                                    if (i + 2 <= NumerosQuinielas.Count - 1)
                                    {
                                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                                            CantidadColumn1 = NumerosQuinielas[i].Cantidad.ToString("C2"),

                                            NumeroColumn2 = NumerosQuinielas[i + 1].Numero,
                                            CantidadColumn2 = NumerosQuinielas[i + 1].Cantidad.ToString("C2"),

                                            NumeroColumn3 = NumerosQuinielas[i + 2].Numero,
                                            CantidadColumn3 = NumerosQuinielas[i + 2].Cantidad.ToString("C2")
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
                                            CantidadColumn1 = NumerosQuinielas[i].Cantidad.ToString("C2"),
                                            NumeroColumn2 = NumerosQuinielas[i + 1].Numero,
                                            CantidadColumn2 = NumerosQuinielas[i + 1].Cantidad.ToString("C2"),

                                        };
                                        ReporteListNumeros.Quiniela.Add(objectoQuiniela);
                                        ViewModel.ReportesListaNumeros.Quiniela = ReporteListNumeros.Quiniela;
                                        continue;
                                    }

                                    if (i <= NumerosQuinielas.Count - 1)
                                    {
                                        ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosQuinielas[i].Numero,
                                            CantidadColumn1 = NumerosQuinielas[i].Cantidad.ToString("C2")
                                        };
                                        ReporteListNumeros.Quiniela.Add(objectoQuiniela);
                                        ViewModel.ReportesListaNumeros.Quiniela = ReporteListNumeros.Quiniela;
                                        continue;
                                    }
                                }
                                foreach (var quiniela in NumerosQuinielas) { totalCantidadQuiniela = totalCantidadQuiniela + Convert.ToInt32(quiniela.Cantidad); totalPagoQuiniela = totalPagoQuiniela + Convert.ToInt32(quiniela.Pago); }
                                ViewModel.ReportesListaNumeros.TotalCantidaQuiniela = string.Format(nfi, "{0:C}", totalCantidadQuiniela);
                                ViewModel.ReportesListaNumeros.TotalPagoQuiniela = string.Format(nfi, "{0:C}", totalPagoQuiniela);

                            }
                            else if (NumerosQuinielas.Count == 1)
                            {
                                ViewModel.ReportesListaNumeros.QuinielaVisibilty = Visibility.Visible;
                                ReportesListaNumerosObservable objectoQuiniela = new ReportesListaNumerosObservable()
                                {
                                    NumeroColumn1 = NumerosQuinielas[0].Numero,
                                    CantidadColumn1 = NumerosQuinielas[0].Cantidad.ToString("C2")

                                };
                                ReporteListNumeros.Quiniela.Add(objectoQuiniela);
                                ViewModel.ReportesListaNumeros.Quiniela = ReporteListNumeros.Quiniela;
                                ViewModel.ReportesListaNumeros.TotalCantidaQuiniela = string.Format(nfi, "{0:C}", NumerosQuinielas[0].Cantidad); ;
                                ViewModel.ReportesListaNumeros.TotalPagoQuiniela = string.Format(nfi, "{0:C}", NumerosQuinielas[0].Pago);

                            }
                            else if (NumerosQuinielas.Count == 0)
                            {
                                ViewModel.ReportesListaNumeros.QuinielaVisibilty = Visibility.Hidden;
                            }

                            if (NumerosPales.Count > 1)
                            {
                                var totalCantidadPale = 0;
                                var totalPagoPale = 0;
                                ViewModel.ReportesListaNumeros.PaleVisibility = Visibility.Visible;
                                for (int i = 0; i < NumerosPales.Count; i = i + 3)
                                {

                                    if (i + 2 <= NumerosPales.Count - 1)
                                    {
                                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosPales[i].Numero,
                                            CantidadColumn1 = NumerosPales[i].Cantidad.ToString("C2"),
                                            NumeroColumn2 = NumerosPales[i + 1].Numero,
                                            CantidadColumn2 = NumerosPales[i + 1].Cantidad.ToString("C2"),
                                            NumeroColumn3 = NumerosPales[i + 2].Numero,
                                            CantidadColumn3 = NumerosPales[i + 2].Cantidad.ToString("C2")
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
                                            CantidadColumn1 = NumerosPales[i].Cantidad.ToString("C2"),

                                            NumeroColumn2 = NumerosPales[i + 1].Numero,
                                            CantidadColumn2 = NumerosPales[i + 1].Cantidad.ToString("C2"),

                                        };
                                        ReporteListNumeros.Pale.Add(objectoPale);
                                        ViewModel.ReportesListaNumeros.Pale = ReporteListNumeros.Pale;
                                        continue;
                                    }
                                    if (i <= NumerosPales.Count - 1)
                                    {
                                        ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosPales[i].Numero,
                                            CantidadColumn1 = NumerosPales[i].Cantidad.ToString("C2")
                                        };
                                        ReporteListNumeros.Pale.Add(objectoPale);
                                        ViewModel.ReportesListaNumeros.Pale = ReporteListNumeros.Pale;
                                        continue;
                                    }
                                }
                                foreach (var pale in NumerosPales) { totalCantidadPale = totalCantidadPale + Convert.ToInt32(pale.Cantidad); totalPagoPale = totalPagoPale + Convert.ToInt32(pale.Pago); }

                                ViewModel.ReportesListaNumeros.TotalCantidadPale = string.Format(nfi, "{0:C}", totalCantidadPale); ;
                                ViewModel.ReportesListaNumeros.TotalPagoPale = string.Format(nfi, "{0:C}", totalPagoPale);
                            }
                            else if (NumerosPales.Count == 1)
                            {
                                ViewModel.ReportesListaNumeros.PaleVisibility = Visibility.Visible;
                                ReportesListaNumerosObservable objectoPale = new ReportesListaNumerosObservable()
                                {
                                    NumeroColumn1 = NumerosPales[0].Numero,
                                    CantidadColumn1 = NumerosPales[0].Cantidad.ToString("C2")

                                };
                                ReporteListNumeros.Pale.Add(objectoPale);
                                ViewModel.ReportesListaNumeros.Pale = ReporteListNumeros.Pale;
                                ViewModel.ReportesListaNumeros.TotalCantidadPale = string.Format(nfi, "{0:C}", NumerosPales[0].Cantidad); ;
                                ViewModel.ReportesListaNumeros.TotalPagoPale = string.Format(nfi, "{0:C}", NumerosPales[0].Pago);
                            }
                            else if (NumerosPales.Count == 0)
                            {
                                ViewModel.ReportesListaNumeros.PaleVisibility = Visibility.Hidden;
                            }
                            if (NumerosTripleta.Count == 1)
                            {
                                ViewModel.ReportesListaNumeros.TripletaVisibility = Visibility.Visible;
                                ReportesListaNumerosObservable ObjectoTripleta = new ReportesListaNumerosObservable()
                                {
                                    NumeroColumn1 = NumerosTripleta[0].Numero,
                                    CantidadColumn1 = NumerosTripleta[0].Cantidad.ToString("C2")
                                };
                                ReporteListNumeros.Tripleta.Add(ObjectoTripleta);
                                ViewModel.ReportesListaNumeros.Tripleta = ReporteListNumeros.Tripleta;
                                ViewModel.ReportesListaNumeros.TotalCantidadTripleta = string.Format(nfi, "{0:C}", NumerosTripleta[0].Cantidad); ;
                                ViewModel.ReportesListaNumeros.TotalPagoTripleta = string.Format(nfi, "{0:C}", NumerosTripleta[0].Pago);
                            }
                            else if (NumerosTripleta.Count > 1)
                            {
                                var totalCantidadTripleta = 0;
                                var totalPagoTripleta = 0;
                                ViewModel.ReportesListaNumeros.TripletaVisibility = Visibility.Visible;
                                for (int i = 0; i < NumerosTripleta.Count; i = i + 3)
                                {
                                    if (i + 2 <= NumerosTripleta.Count - 1)
                                    {
                                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosTripleta[i].Numero,
                                            CantidadColumn1 = NumerosTripleta[i].Cantidad.ToString("C2"),
                                            NumeroColumn2 = NumerosTripleta[i + 1].Numero,
                                            CantidadColumn2 = NumerosTripleta[i + 1].Cantidad.ToString("C2"),
                                            NumeroColumn3 = NumerosTripleta[i + 2].Numero,
                                            CantidadColumn3 = NumerosTripleta[i + 2].Cantidad.ToString("C2")
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
                                            CantidadColumn1 = NumerosTripleta[i].Cantidad.ToString("C2"),
                                            NumeroColumn2 = NumerosTripleta[i + 1].Numero,
                                            CantidadColumn2 = NumerosTripleta[i + 1].Cantidad.ToString("C2")
                                        };
                                        ReporteListNumeros.Tripleta.Add(objectoTripleta);
                                        ViewModel.ReportesListaNumeros.Tripleta = ReporteListNumeros.Tripleta;
                                        continue;
                                    }
                                    if (i <= NumerosTripleta.Count - 1)
                                    {
                                        ReportesListaNumerosObservable objectoTripleta = new ReportesListaNumerosObservable()
                                        {
                                            NumeroColumn1 = NumerosTripleta[i].Numero,
                                            CantidadColumn1 = NumerosTripleta[i].Cantidad.ToString("C2")
                                        };
                                        ReporteListNumeros.Tripleta.Add(objectoTripleta);
                                        ViewModel.ReportesListaNumeros.Tripleta = ReporteListNumeros.Tripleta;
                                        continue;
                                    }
                                }
                                foreach (var tripleta in NumerosTripleta) { totalCantidadTripleta = totalCantidadTripleta + Convert.ToInt32(tripleta.Cantidad); totalPagoTripleta = totalPagoTripleta + Convert.ToInt32(tripleta.Pago); }

                                ViewModel.ReportesListaNumeros.TotalCantidadTripleta = string.Format(nfi, "{0:C}", totalCantidadTripleta); ;
                                ViewModel.ReportesListaNumeros.TotalPagoTripleta = string.Format(nfi, "{0:C}", totalPagoTripleta);

                            }
                            else if (NumerosTripleta.Count == 0)
                            {
                                ViewModel.ReportesListaNumeros.TripletaVisibility = Visibility.Hidden;
                            }
                        }
                        else if(Reporte.Numeros.Count() == 0)
                        {
                            MessageBox.Show("No existen numeros correspondientes a la fecha y opcion de loteria seleccionada.", "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);

                        }
                    }
                    else if (Reporte.Err != null)
                    {
                        MessageBox.Show("No existen numeros correspondientes a la fecha y opcion de loteria seleccionada.", "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);
                        Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListNumero = Visibility.Collapsed; ViewModel.IconRPTListNumero = Visibility.Visible; } catch { } })); });
                        //MostrarMensajes(Reporte.Err,"MAR-Cliente","INFO");

                    }
                    Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListNumero = Visibility.Collapsed; ViewModel.IconRPTListNumero = Visibility.Visible; } catch { } })); });
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message,"MAR-Cliente",MessageBoxButton.OK,MessageBoxImage.Error);
                    Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListNumero = Visibility.Collapsed; ViewModel.IconRPTListNumero = Visibility.Visible; } catch { } })); });
                }
            }else if (Reporte.Err != null)
            {
                MessageBox.Show(Reporte.Err, "Cliente MAR", MessageBoxButton.OK, MessageBoxImage.Information);
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTListNumero = Visibility.Collapsed; ViewModel.IconRPTListNumero = Visibility.Visible; } catch { } })); });
                //MostrarMensajes(Reporte.Err,"MAR-Cliente","INFO");

            }
           }

        private void RPTListTickets(object parametros)
        {
            var NombreLoteria = new ReporteView().GetNombreLoteria();
            var ReporteTicket = ReportesService.ReporteListaDeTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            
            ObservableCollection<ReporteListaTicketsObservable> ListadoTicket = new ObservableCollection<ReporteListaTicketsObservable>() { };
            ObservableCollection<ReporteListaTicketsObservable> ListadoAllDataTicket = new ObservableCollection<ReporteListaTicketsObservable>() { };

            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            int totalVenta = 0; int totalSaco=0;
            DateTime Fecha = Convert.ToDateTime(ReporteTicket.Fecha);

            if (ReporteTicket.Err == null)
            {
                if (ReporteTicket.Tickets == null)
                {
                    MessageBox.Show("No existen tickets correpondientes a la fecha y loteria seleccionada.","Cliente MAR",MessageBoxButton.OK,MessageBoxImage.Information);
                    Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.IconRPTListTicket = Visibility.Visible; ViewModel.LoadingRPTListTicket = Visibility.Collapsed; } catch { } })); });
                    return;
                }
                else if (ReporteTicket.Tickets.Count() > 0)
                {
                  
                ViewModel.RPTLitTicketVisibility = Visibility.Visible;
                HeaderReporte(Fecha, "LISTADO DE TICKETS", NombreLoteria);
                if (ReporteTicket.Tickets != null && ReporteTicket.Tickets.Length > 0)
                {
                    ViewModel.CanChangeOptionListTicket = true;
                    var opcionSeleccionada = new ReporteView().ObtenerSeleccionTicket();
                    if (opcionSeleccionada == "Válidos") {
                        foreach (var ticket in ReporteTicket.Tickets.Where(ticket => ticket.Nulo == false))
                        {

                            ReporteListaTicketsObservable objectoTicket = new ReporteListaTicketsObservable()
                            { Ticket = ticket.TicketNo, Hora = ticket.StrHora, Vendio = (int)ticket.Costo, Saco = string.Format(CultureInfo.InvariantCulture, "{0:0,0.0}", ticket.Pago) };
                            objectoTicket.MostrarNulos = Visibility.Visible;
                            ListadoTicket.Add(objectoTicket);
                        }
                    }
                    if (opcionSeleccionada == "Nulos")
                    {
                        foreach (var ticket in ReporteTicket.Tickets.Where(ticket => ticket.Nulo == true))
                        {
                            ReporteListaTicketsObservable objectoTicket = new ReporteListaTicketsObservable()
                            { Ticket = ticket.TicketNo, Hora = ticket.StrHora, Vendio = (int)ticket.Costo, Saco = "Nulo" };
                            objectoTicket.MostrarNulos = Visibility.Visible;
                            ListadoTicket.Add(objectoTicket);
                        }
                    }
                    if (opcionSeleccionada == "Todos" || opcionSeleccionada == null)
                    {
                        foreach (var ticket in ReporteTicket.Tickets)
                        {
                            var saco = "";
                            if (ticket.Nulo == false) { saco = string.Format(CultureInfo.InvariantCulture, "{0:0,0.00}", ticket.Pago); };
                            if (ticket.Nulo == true) { saco = "Nulo"; }
                            ReporteListaTicketsObservable objectoTicket = new ReporteListaTicketsObservable()
                            { Ticket = ticket.TicketNo, Hora = ticket.StrHora, Vendio = (int)ticket.Costo, Saco = saco };
                            objectoTicket.MostrarNulos = Visibility.Visible;
                            ListadoTicket.Add(objectoTicket);
                        }
                    }

                    foreach (var ticket in ReporteTicket.Tickets)
                    {
                        if (ticket.Nulo == false)
                        {
                            totalVenta = totalVenta + Convert.ToInt32(ticket.Costo);
                            totalSaco = totalSaco + Convert.ToInt32(ticket.Pago);
                        }
                    }

                    foreach (var ticket in ReporteTicket.Tickets)
                    {
                        var saco = "";
                        if (ticket.Nulo == false) { saco = string.Format(CultureInfo.InvariantCulture, "{0:0,0.0}", ticket.Pago); }
                        if (ticket.Nulo == true) { saco = "Nulo"; }
                        ReporteListaTicketsObservable objectoTicket = new ReporteListaTicketsObservable()
                        { Ticket = ticket.TicketNo, Hora = ticket.StrHora, Vendio = (int)ticket.Costo, Saco = saco };
                        objectoTicket.MostrarNulos = Visibility.Visible;
                        ListadoAllDataTicket.Add(objectoTicket);
                        ViewModel.ReporteAllDataListTicket = ListadoAllDataTicket;
                    }

                    ViewModel.ReporteListTicket = ListadoTicket;
                    var TicketsNulo = ReporteTicket.Tickets.Where(ticket => ticket.Nulo == true).ToArray();
                    ViewModel.TotalesListTicket.CantidadNulos = TicketsNulo.Length.ToString();
                    ViewModel.TotalesListTicket.CantidadValidos = (ReporteTicket.Tickets.Length - TicketsNulo.Length).ToString();
                    ViewModel.TotalesListTicket.TotalVenta = string.Format(nfi, "{0:C}", totalVenta);
                    ViewModel.TotalesListTicket.TotalSaco = string.Format(nfi, "{0:C}", totalSaco);
                    var ListadoString = new List<string>() { "Titulo", "Parrado" };

                    //Muestra De Ticket
                    var NombreBanca = Autenticador.BancaConfiguracion.BancaDto.BanContacto + "ID: " + Autenticador.BancaConfiguracion.BancaDto.BancaID;
                    var Titulo = ViewModel.NombreReporte;
                    var FechaActual = ViewModel.FechaActualReport;
                    var FechaReporte = ViewModel.FechaReporteLabel;
                    var Loteria = ViewModel.NombreLoteria;
                    List<string> listadoImpreso = new List<string> { };

                    listadoImpreso.Add(NombreBanca);
                    listadoImpreso.Add(Titulo);
                    listadoImpreso.Add(FechaActual);
                    listadoImpreso.Add(FechaReporte);
                    listadoImpreso.Add(Loteria);

                    foreach (var Ticket in ListadoTicket)
                    {
                        listadoImpreso.Add(Ticket.Ticket + " " + Ticket.Hora + " " + Ticket.Vendio + " " + Ticket.Saco);
                    }
                }
                else
                {
                    ViewModel.CanChangeOptionListTicket = false;
                    ViewModel.ReporteListTicket = null;
                    ViewModel.ReporteAllDataListTicket = null;
                    ViewModel.TotalesListTicket.CantidadNulos = "0";
                    ViewModel.TotalesListTicket.CantidadValidos = "0";
                    ViewModel.TotalesListTicket.TotalVenta = "0";
                    ViewModel.TotalesListTicket.TotalSaco = "0";
                }
            }
            }
            else
            {
                MostrarMensajes(ReporteTicket.Err.ToString(), "MAR-Cliente", "INFO");
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.IconRPTListTicket = Visibility.Visible; ViewModel.LoadingRPTListTicket = Visibility.Collapsed; } catch { } })); });
            }
            Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.IconRPTListTicket = Visibility.Visible; ViewModel.LoadingRPTListTicket = Visibility.Collapsed; } catch { } })); });
        }

        private void RPTPagosRemotos(object parametros)
        {
            var PagosRemotos = ReportesService.ReporteListaPagosRemotos(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
            ObservableCollection<ReporteListaTicketsObservable> ListadoPagosRemotos = new ObservableCollection<ReporteListaTicketsObservable>() { };
            NumberFormatInfo nfi = new CultureInfo("en-US", false).NumberFormat; //Formato numero
            var PagosRemotoData = PagosRemotos.Tickets;
            var totalPagosRemotos = 0;
            int balance = 0;
            string totalbalance = "";

            if (PagosRemotos.Err == null)
            {
                if (PagosRemotoData.Length > 0 && PagosRemotos.Err == null)
                {
                    HeaderReporte(Convert.ToDateTime(PagosRemotos.Fecha), "TICKET PAGADOS REMOTAMENTE", null);
                    for (int i = 0; i < PagosRemotoData.Length; i++)
                    {
                        ViewModel.RPTPagosRemotosVisibility = System.Windows.Visibility.Visible;
                        totalPagosRemotos = totalPagosRemotos + Convert.ToInt32(PagosRemotoData[i].Pago);
                        if (PagosRemotoData[i].Pago >= 0) { balance = (int)PagosRemotoData[i].Pago; }
                        else if (PagosRemotoData[i].Pago < 0) { balance = (int)PagosRemotoData[i].Pago; }

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
                    ViewModel.TotalesPagosRemotos = ConvertirMonedaNegativos(totalPagosRemotos);
                }
            }
            else if (PagosRemotos.Err != null)
            {
                MostrarMensajes(PagosRemotos.Err, "MAR-Cliente", "INFO");
                Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTPagosRemotos = Visibility.Collapsed; ViewModel.IconRPTPagosRemotos = Visibility.Visible; } catch { } })); });
                
            }
            Task.Factory.StartNew(() => { Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => { try { ViewModel.LoadingRPTPagosRemotos = Visibility.Collapsed; ViewModel.IconRPTPagosRemotos = Visibility.Visible; } catch { } })); });
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
