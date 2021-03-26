using ClienteMarWPFWin7.Data.Services.Helpers;
using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.Data.Services
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

        public MAR_Pin GetRecarga(MAR_Session _Session, int user, string clave, int suplidor, string Numero, double Monto, int Solicitud)
        {
         
             return clientePuntoDeVenta.GetRecarga(_Session, user, clave, suplidor, Numero, Monto, Solicitud);
         
        }

        public void ConfirmRecarga(MAR_Session _Session)
        {
            clientePuntoDeVenta.ConfirmRecarga(_Session);
        }


       
    }
}
