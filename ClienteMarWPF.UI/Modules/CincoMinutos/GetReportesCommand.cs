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
            try
            {
                var nombre = new ReporteView().GetReporteNombre();
                if (nombre == "Suma De Ventas")
                {
                    ViewModel.NombreReporte = "SUMA DE VENTAS";
                    ViewModel.NombreBanca = "Lexus";
                    
                    //ViewModel.Fecha = DateTime.Now.ToString();
                    RPTSumaDeVentas(parametro);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private void RPTSumaDeVentas(object parametro)
        {
            try
            {
                var Reporte = ReportesService.EnviarReportes(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
                ObservableCollection<ReportesMostrarObservable> List = new ObservableCollection<ReportesMostrarObservable>() { };
                if (Reporte.Reglones.Length > 0)
                {
                    ViewModel.RPTSumaVentas = System.Windows.Visibility.Visible;
                    //////////////////////////////////////////// Aqui los datos del header//////////////////////////////////////////////////
                    var DiaSemanaActual = TraducirDiaSemana(DateTime.Now.DayOfWeek.ToString());
                    var DiaSemanaReporte = TraducirDiaSemana(Convert.ToDateTime(Reporte.Fecha).DayOfWeek.ToString());
                    ViewModel.FechaActualReport = "Del Dia " + DiaSemanaReporte + ", " + Convert.ToDateTime(Reporte.Fecha).ToString("dd-MMM-yyyy");
                    ViewModel.FechaReporte = DiaSemanaActual + ", " + DateTime.Now.ToString("dd-MMM-yyyy") + " " + DateTime.Now.ToShortTimeString();
                    ViewModel.NombreReporte = "SUMA DE VENTAS";
                    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

                            ReportesMostrarObservable objecto = new ModelObservable.ReportesMostrarObservable()
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
    
   

    } 
}
