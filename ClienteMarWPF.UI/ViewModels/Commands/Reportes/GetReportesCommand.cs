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

        private void EnviarReportes(object parametro)
        {
            //Ocultar Templates de reportes
            ViewModel.RPTSumaVentas = Visibility.Hidden;
            ViewModel.RPTTicketGanadores = Visibility.Hidden;

            try
            {
                var nombre = new ReporteView().GetReporteNombre();
                if (nombre == "Suma De Ventas")
                {
                    ViewModel.NombreBanca = "Lexus";
                    
                    //ViewModel.Fecha = DateTime.Now.ToString();
                    RPTSumaDeVentas(parametro);
                }
                else if(nombre == "Reportes Ganadores")
                {
                    ViewModel.NombreBanca = "Lexus";
                    RPTGanadores(parametro);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void HeaderReporte(string FechaRepote,string NombreReporte,string Loteria)
        {
            //////////////////////////////////////////// Aqui los datos del header//////////////////////////////////////////////////
            var DiaSemanaActual = TraducirDiaSemana(DateTime.Now.DayOfWeek.ToString());
            var DiaSemanaReporte = TraducirDiaSemana(Convert.ToDateTime(FechaRepote).DayOfWeek.ToString());
            ViewModel.FechaActualReport = "Del Dia " + DiaSemanaReporte + ", " + Convert.ToDateTime(FechaRepote).ToString("dd-MMM-yyyy");
            ViewModel.FechaReporte = DiaSemanaActual + ", " + DateTime.Now.ToString("dd-MMM-yyyy") + " " + DateTime.Now.ToShortTimeString();
            ViewModel.NombreReporte = NombreReporte;

            if (NombreReporte == "TICKETS GANADORES") { ViewModel.NombreLoteria ="Loteria: "+Loteria;}
            
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        }

        private void RPTSumaDeVentas(object parametro)
        {
            try
            {
                var Reporte = ReportesService.ReporteSumVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
                ObservableCollection<ReportesSumVentasObservable> List = new ObservableCollection<ReportesSumVentasObservable>() { };
                if (Reporte.Reglones.Length > 0)
                {
                    HeaderReporte(Reporte.Fecha, "SUMA DE VENTAS", null);

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
                                Renglon = data.Reglon,
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
                        
                        System.Windows.MessageBox.Show(Reporte.Err.ToString(), "No se encontraron ventas", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show(Reporte.Err.ToString(), "MAR-Cliente", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Information);
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
            var Reporte = ReportesService.ReportesGanadores(Autenticador.CurrentAccount.MAR_Setting2.Sesion, loteriaId, ViewModel.Fecha);

            if (Reporte.Tickets.Length > 0 || Reporte.Tickets != null) { 
            ObservableCollection<ReportesGanadoresObservable> Lista = new ObservableCollection<ReportesGanadoresObservable>() { };
            HeaderReporte(Reporte.Fecha, "TICKETS GANADORES",new ReporteView().GetNombreLoteria());
                ViewModel.RPTTicketGanadores = System.Windows.Visibility.Visible;
                var estado = "";
                int numero = 0;
                int Total = 0;
            foreach (var ticket in Reporte.Tickets.OrderBy(Reporte => Reporte.Solicitud))
            {
                if (ticket.Solicitud==3){ estado = "Pendientes Por Pagar"; }
                else if (ticket.Solicitud == 5) { estado = "Tickets Pagados"; }
                else if (ticket.Solicitud == 6) { estado = "Tickets Sin Reclamar"; }

                ReportesGanadoresObservable objectoTicket = new ReportesGanadoresObservable()
                {
                    Tickets = ticket.TicketNo, 
                    Fecha = ticket.StrFecha + " "+ticket.StrHora, 
                    Monto = (int)ticket.Pago,
                    Categoria = estado

                };
                Total = Total + Convert.ToInt32(ticket.Pago);
                Lista.Add(objectoTicket);

                if (numero < Reporte.Tickets.Length -1) { 
                    if (Reporte.Tickets[numero].Solicitud != Reporte.Tickets[numero + 1].Solicitud)
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
                    if (numero == Reporte.Tickets.Length - 1)
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
           
          }
        }
    } 
}
