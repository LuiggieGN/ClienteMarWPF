using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Services.ReportesService
{
    public interface IReportesServices
    {
        public MAR_RptSumaVta EnviarReportes(MAR_Session session,string Fecha);
    }
}
