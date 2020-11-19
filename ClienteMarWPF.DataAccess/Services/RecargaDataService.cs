using ClienteMarWPF.DataAccess.Services.Helpers;
using ClienteMarWPF.Domain.Services.RecargaService;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.DataAccess.Services
{
    public class RecargaDataService: IRecargaService
    {
        public static SoapClientRepository SoapClientesRepository;
        private static PtoVtaSoapClient clientePuntoDeVenta;
        static  RecargaDataService()
        {
            SoapClientesRepository = new SoapClientRepository();
            clientePuntoDeVenta = SoapClientesRepository.GetMarServiceClient(false);
        }

        public MAR_Suplidor[] GetSuplidor(MAR_Session _Session)
        {
           return clientePuntoDeVenta.GetSuplidores(_Session);

        }

       
    }
}
