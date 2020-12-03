using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Services.ReportesService
{
    public interface IReportesServices
    {
        public MAR_RptSumaVta ReporteSumVentas(MAR_Session session,string Fecha);
        public MAR_Ganadores ReportesGanadores(MAR_Session session,int Loteria,string Fecha);
        public MAR_RptSumaVta2 ReporteVentasPorFecha(MAR_Session session, string Desde, string Hasta);
        public MAR_Pines ReporteListaTarjetas(MAR_Session session, string Fecha);
        public MAR_Ganadores ReporteListaPremios(MAR_Session session, int Loteria, string Fecha);
        public MAR_VentaNumero ReporteListadoNumero(MAR_Session session, int Loteria, string Fecha);
        public MAR_RptVenta ReporteDeVentas(MAR_Session session, int Loteria, string Fecha);
        public MAR_Ganadores ReporteListaDeTicket(MAR_Session session, int Loteria, string Fecha);
        public MAR_Ganadores ReporteListaPagosRemotos(MAR_Session session, string Fecha);
    }
}
