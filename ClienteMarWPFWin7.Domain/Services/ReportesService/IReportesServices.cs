using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.Domain.Services.ReportesService
{
    public interface IReportesServices
    {
        MAR_RptSumaVta ReporteSumVentas(MAR_Session session, string Fecha);
        MAR_Ganadores ReportesGanadores(MAR_Session session, int Loteria, string Fecha);
        MAR_RptSumaVta2 ReporteVentasPorFecha(MAR_Session session, string Desde, string Hasta);
        MAR_Pines ReporteListaTarjetas(MAR_Session session, string Fecha);
        MAR_VentaNumero ReporteListadoNumero(MAR_Session session, int Loteria, string Fecha);
        MAR_RptVenta ReporteDeVentas(MAR_Session session, int Loteria, string Fecha);
        MAR_Ganadores ReporteListaDeTicket(MAR_Session session, int Loteria, string Fecha);
        MAR_Ganadores ReporteListaPagosRemotos(MAR_Session session, string Fecha);
    }
}
