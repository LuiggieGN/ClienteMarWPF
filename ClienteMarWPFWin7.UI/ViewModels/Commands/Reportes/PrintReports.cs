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
            var Reporte = ReportesService.ReporteSumVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
            MAR_RptSumaVta ventasprint = new MAR_RptSumaVta() { Dia = Reporte.Dia, Err = Reporte.Err, Hora = Reporte.Hora, Fecha = Reporte.Fecha, Reglones = Reporte.Reglones };
            List<string[]> ImprimirSumVenta = PrintJobs.FromReporteSumaVenta(ventasprint,Autenticador);

            ReporteTemplateHelper.PrintReporte(ventasprint,Autenticador,ViewModel);
        }

        private void PrintVentasFecha(object parametro)
        {
            MAR_RptSumaVta2 ventasfechaprint = new MAR_RptSumaVta2() { };
            var Reporte = ReportesService.ReporteVentasPorFecha(Autenticador.CurrentAccount.MAR_Setting2.Sesion, FechaInicio, FechaFin);
            List<string[]> ImprimirSumVentaFecha = new List<string[]>() { };
            if (ViewModel.SoloTotales==true)
            {
                 ventasfechaprint = new MAR_RptSumaVta2() { Dia = Reporte.Dia, Err = Reporte.Err, Hora = Reporte.Hora, Fecha = Reporte.Fecha, Reglones = Reporte.Reglones, ISRRetenido = Reporte.ISRRetenido, RifDescuento = Reporte.RifDescuento };
                ImprimirSumVentaFecha = PrintJobs.FromReporteVentaPorFecha(ventasfechaprint, ViewModel.FechaInicio.ToString(), ViewModel.FechaFin.ToString(), ViewModel.SoloTotales,Autenticador);
                
            }
            else if (ViewModel.SoloTotales == false)
            {
                 ventasfechaprint = new MAR_RptSumaVta2() { Dia = Reporte.Dia, Err = Reporte.Err, Hora = Reporte.Hora, Fecha = Reporte.Fecha, Reglones = Reporte.Reglones, ISRRetenido = Reporte.ISRRetenido, RifDescuento = Reporte.RifDescuento };
                ImprimirSumVentaFecha = PrintJobs.FromReporteVentaPorFecha(ventasfechaprint, ViewModel.FechaInicio.ToString(), ViewModel.FechaFin.ToString(), ViewModel.SoloTotales,Autenticador);
               
            }
            ReporteTemplateHelper.PrintReporte(ventasfechaprint,Autenticador,ViewModel,false,false, ViewModel.SoloTotales);
        }
        private void PrintReporteGanadores(object Parametro)
        {
            
            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var Reporte = ReportesService.ReportesGanadores(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);

            MAR_Ganadores ModelMarGanadores = new MAR_Ganadores() { Dia = Reporte.Dia, Err = Reporte.Err, Fecha = Reporte.Fecha, Hora = Reporte.Hora, Primero = Reporte.Primero, Segundo = Reporte.Segundo, Tercero = Reporte.Tercero, Tickets = Reporte.Tickets };
            ReportesIndexGanadores indexGanadores = new ReportesIndexGanadores() { Loteria = ViewModel.LoteriaID, Fecha = Convert.ToDateTime(Reporte.Fecha), Primero = Reporte.Primero, Segundo = Reporte.Segundo, Tercero = Reporte.Tercero, Sorteo = nombreLoteria };
            //List<string[]> reporteGanadoresPrint = PrintJobs.FromReporteDeGanadores(ModelMarGanadores, indexGanadores,Autenticador);
            ReporteTemplateHelper.PrintReporte(ModelMarGanadores,Autenticador,ViewModel,true);
        }

        private void PrintListaTarjetas(object Parametro)
        {
            var reportes = ReportesService.ReporteListaTarjetas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha);
            MAR_Pines Pines = new MAR_Pines() { Dia = reportes.Dia, Err = reportes.Err, Fecha = reportes.Fecha, Hora = reportes.Hora, Pines = reportes.Pines };
            //List<string[]> PrintPines = PrintJobs.FromReporteListadoDePines(Pines,Autenticador);
            ReporteTemplateHelper.PrintReporte(Pines,Autenticador,ViewModel);
        }

        private void PrintListaNumeros(object Parametro)
        {
            try { 
            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var Reporte = ReportesService.ReporteListadoNumero(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            MAR_VentaNumero ListaNumero = new MAR_VentaNumero() { Dia = Reporte.Dia, Err = Reporte.Err, Fecha = Reporte.Fecha, Hora = Reporte.Hora, Loteria = Reporte.Loteria, Numeros = Reporte.Numeros };
            //List<string[]> PrintNumero = PrintJobs.FromListaDeNumeros(ListaNumero, Reporte.Fecha, nombreLoteria,Autenticador);
            ReporteTemplateHelper.PrintReporte(ListaNumero,Autenticador,ViewModel);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message,"Cliente MAR",MessageBoxButton.OK,MessageBoxImage.Error);
            }
         }
        private void PrintVentas(object Parametro)
        {
           
            var nombreLoteria = new ReporteView().GetNombreLoteria();
            var ReporteVenta = ReportesService.ReporteDeVentas(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            
            MAR_RptVenta TemplatePrintVenta = new MAR_RptVenta(){
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

            //List<string[]> PrintRPTVentas = PrintJobs.FromReporteVenta(TemplatePrintVenta, nombreLoteria,Autenticador);
            ReporteTemplateHelper.PrintReporte(TemplatePrintVenta,Autenticador,ViewModel);
        }

        private void PrintListaTicket(object Parametro)
        {
            var NombreLoteria = new ReporteView().GetNombreLoteria();
            
            var ReporteTicket = ReportesService.ReporteListaDeTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.LoteriaID, ViewModel.Fecha);
            MAR_Ganadores ModelMarGanadores = new MAR_Ganadores() { Dia = ReporteTicket.Dia, Err = ReporteTicket.Err, Fecha = ReporteTicket.Fecha, Hora = ReporteTicket.Hora, Primero = ReporteTicket.Primero, Segundo = ReporteTicket.Segundo, Tercero = ReporteTicket.Tercero, Tickets = ReporteTicket.Tickets };
            //List<string[]> reporteTicketPrint = PrintJobs.FromReporteListadoDeTickets(ModelMarGanadores, NombreLoteria,Autenticador);
            ReporteTemplateHelper.PrintReporte(ModelMarGanadores,Autenticador,ViewModel);
        }
        private void PrintPagosRemotos(object Parametro)
        {
            var PagosRemotos = ReportesService.ReporteListaPagosRemotos(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Fecha); 
            MAR_Ganadores ganadores = new MAR_Ganadores() { Primero = PagosRemotos.Primero, Segundo = PagosRemotos.Segundo, Tercero = PagosRemotos.Tercero, Dia = PagosRemotos.Dia, Hora = PagosRemotos.Hora, Fecha = PagosRemotos.Fecha, Err = PagosRemotos.Err, Tickets = PagosRemotos.Tickets };
            ReporteTemplateHelper.PrintReporte(ganadores, Autenticador,ViewModel, false, true);
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
            printData[1] = new string[] {printData[0][0].ToString()+"\r\n"+printData[1][0].ToString() };
            printData[0] = new string[] {""};
            
            TicketTemplateHelper.PrintTicket(printData);
        }

    }
}
