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

        public MAR_RptSumaVta ReporteSumVentas(MAR_Session session, DateTime Fecha)
        {

            var fecha = Fecha.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            return clientePuntoDeVenta.RptSumaVta(session,fecha.ToString());
        }
        public MAR_Ganadores ReportesGanadores(MAR_Session session,int Loteria ,DateTime Fecha)
        {
            var fecha = Fecha.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            return clientePuntoDeVenta.Ganadores2(session,Loteria,fecha);
        }

        public MAR_RptSumaVta2 ReporteVentasPorFecha(MAR_Session session, DateTime Desde, DateTime Hasta)
        {
            var f1 = Desde.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            var f2 = Hasta.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];

            return clientePuntoDeVenta.RptSumaVtaFec2(session,f1,f2);
        }

        public MAR_Pines ReporteListaTarjetas(MAR_Session session, DateTime Fecha)
        {
            var fecha =Fecha.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            return clientePuntoDeVenta.ListaPines(session,fecha);
        }

        public MAR_VentaNumero ReporteListadoNumero(MAR_Session session, int loteria, DateTime Fecha) {
            var fecha = Fecha.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            return clientePuntoDeVenta.VentaNumero(session, loteria, fecha);
        }
        public MAR_RptVenta ReporteDeVentas(MAR_Session session, int loteria, DateTime Fecha)
        {
            var fecha = Fecha.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            return clientePuntoDeVenta.RptVenta(session, loteria, fecha);
        }
        public MAR_Ganadores ReporteListaDeTicket(MAR_Session session, int loteria, DateTime Fecha)
        {
            var fecha = Fecha.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            return clientePuntoDeVenta.ListaTickets(session, loteria, fecha);
        }
        public MAR_Ganadores ReporteListaPagosRemotos(MAR_Session session, DateTime Fecha)
        {
            var fecha = Fecha.GetDateTimeFormats(CultureInfo.InvariantCulture)[24];
            return clientePuntoDeVenta.ListaTicketPagoRemoto(session, fecha);
        }



    }
}
