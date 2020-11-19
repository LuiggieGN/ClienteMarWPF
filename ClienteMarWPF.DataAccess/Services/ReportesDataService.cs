﻿using ClienteMarWPF.DataAccess.Services.Helpers;
using ClienteMarWPF.Domain.Services.ReportesService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
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



    }
}