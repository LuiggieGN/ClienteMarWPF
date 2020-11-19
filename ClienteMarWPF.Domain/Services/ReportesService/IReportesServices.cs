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

    }
}
