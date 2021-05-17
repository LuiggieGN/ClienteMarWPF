using ClienteMarWPFWin7.Data.Services.Helpers;
using ClienteMarWPFWin7.Domain.Services.ReportesService;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ClienteMarWPFWin7.Data.Services
{
    public class ReportesDataService:IReportesServices
    {
        public static SoapClientRepository SoapClientesRepository;
        private static PtoVtaSoapClient clientePuntoDeVenta;
        
        static ReportesDataService()
        {
            SoapClientesRepository = new SoapClientRepository();
            clientePuntoDeVenta = SoapClientesRepository.GetMarServiceClient(false);
        }

        public MAR_RptSumaVta ReporteSumVentas(MAR_Session session, string Fecha)
        {
            var fecha = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd");
            return clientePuntoDeVenta.RptSumaVta(session,fecha.ToString());
        }
        public MAR_Ganadores ReportesGanadores(MAR_Session session,int Loteria ,string Fecha)
        {
            var fecha = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd");
            return clientePuntoDeVenta.Ganadores2(session,Loteria,fecha);
        }

        public MAR_RptSumaVta2 ReporteVentasPorFecha(MAR_Session session, string Desde, string Hasta)
        {
            var f1 = Convert.ToDateTime(Desde).Date.ToString("MM-dd-yyyy", CultureInfo.CreateSpecificCulture("en-Us"));
            var f2 = Convert.ToDateTime(Hasta).Date.ToString("MM-dd-yyyy", CultureInfo.CreateSpecificCulture("en-Us"));

            return clientePuntoDeVenta.RptSumaVtaFec2(session,f1,f2);
        }

        public MAR_Pines ReporteListaTarjetas(MAR_Session session, string Fecha)
        {
            var fecha = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd");
            return clientePuntoDeVenta.ListaPines(session,fecha);
        }

        public MAR_VentaNumero ReporteListadoNumero(MAR_Session session, int loteria, string Fecha) {
            var fecha = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd");
            return clientePuntoDeVenta.VentaNumero(session, loteria, fecha);
        }
        public MAR_RptVenta ReporteDeVentas(MAR_Session session, int loteria, string Fecha)
        {
            var fecha = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd");
            return clientePuntoDeVenta.RptVenta(session, loteria, fecha);
        }
        public MAR_Ganadores ReporteListaDeTicket(MAR_Session session, int loteria, string Fecha)
        {
            var fecha = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd");
            return clientePuntoDeVenta.ListaTickets(session, loteria, fecha);
        }
        public MAR_Ganadores ReporteListaPagosRemotos(MAR_Session session, string Fecha)
        {
            var fecha = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd");
            return clientePuntoDeVenta.ListaTicketPagoRemoto(session, fecha);
        }



    }
}
