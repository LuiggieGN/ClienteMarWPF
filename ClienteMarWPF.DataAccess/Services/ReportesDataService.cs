using ClienteMarWPF.DataAccess.Services.Helpers;
using ClienteMarWPF.Domain.Services.ReportesService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ClienteMarWPF.DataAccess.Services
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
            return clientePuntoDeVenta.RptSumaVta(session,Fecha);
        }
        public MAR_Ganadores ReportesGanadores(MAR_Session session,int Loteria ,string Fecha)
        {
            return clientePuntoDeVenta.Ganadores3(session,Loteria,Fecha);
        }

        public MAR_RptSumaVta2 ReporteVentasPorFecha(MAR_Session session, string Desde, string Hasta)
        {
            return clientePuntoDeVenta.RptSumaVtaFec2(session,Desde,Hasta);
        }

        public MAR_Pines ReporteListaTarjetas(MAR_Session session, string Fecha)
        {
            return clientePuntoDeVenta.ListaPines(session,Fecha);
        }

        public MAR_Ganadores ReporteListaPremios(MAR_Session session,int loteria, string Fecha)
        {
            return clientePuntoDeVenta.Ganadores3(session,loteria,Fecha);
            
        }

        public MAR_VentaNumero ReporteListadoNumero(MAR_Session session, int loteria, string Fecha) {

            return clientePuntoDeVenta.VentaNumero(session, loteria, Fecha);
        }
        public MAR_RptVenta ReporteDeVentas(MAR_Session session, int loteria, string Fecha)
        {
            return clientePuntoDeVenta.RptVenta(session, loteria, Fecha);
        }
        public MAR_Ganadores ReporteListaDeTicket(MAR_Session session, int loteria, string Fecha)
        {
            return clientePuntoDeVenta.ListaTickets(session, loteria, Fecha);
        }
        public MAR_Ganadores ReporteListaPagosRemotos(MAR_Session session, string Fecha)
        {
            return clientePuntoDeVenta.ListaTicketPagoRemoto(session, Fecha);
        }



    }
}
