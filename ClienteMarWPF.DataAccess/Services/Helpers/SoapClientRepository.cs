using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MAR.Config;
using MAR.AppLogic.Encryption;

 

namespace ClienteMarWPF.DataAccess.Services.Helpers
{
    public class SoapClientRepository
    {
        string _serverAddress = Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(MAR.Config.ConfigEnums.ServiceURL));
        string _serverBackUpAddress = Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(MAR.Config.ConfigEnums.ServiceLocalURL));

        // Clientes Soap
        MarPuntoVentaServiceReference.PtoVtaSoapClient _clientePuntoDeVenta;
        FlujoService.mar_flujoSoapClient _clienteFlujoEfectivo;


        public int ClientTimeout { get; set; }
        public string ServiceHostIP { get; set; }


        public MarPuntoVentaServiceReference.PtoVtaSoapClient GetPuntoDeVentaServiceClient(bool pUseBackupConnection, int flujoTimeoutSeconds = 30)
        {
            try
            {
                if (_clientePuntoDeVenta != null && _clientePuntoDeVenta.State != CommunicationState.Closed)
                {
                    try { _clientePuntoDeVenta.CloseAsync(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

                _serverAddress = @"http://pruebasmar.ddns.net/mar-svr5/mar-ptovta.asmx";  //Remover esta linea al realizar el pase a Produccion :: OJO pendiente

                if ((pUseBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
                {
                    splitaddress = _serverBackUpAddress.Replace("localhost", ServiceHostIP).Split('/');
                }
                else
                {
                    splitaddress = _serverAddress.Split('/');
                }

          


                splitaddress[splitaddress.Length - 1] = "mar-ptovta.asmx";

                endpoint = new EndpointAddress(string.Join("/", splitaddress));

                binding = new BasicHttpBinding(endpoint.Uri.Scheme.ToLower() == "http" ? BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);

                binding.ReceiveTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
                binding.OpenTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
                binding.CloseTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
                binding.SendTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);

                _clientePuntoDeVenta = new MarPuntoVentaServiceReference.PtoVtaSoapClient(binding, endpoint);        
            
            }
            catch 
            {
                _clientePuntoDeVenta = null;
            }

            return _clientePuntoDeVenta;

        }//fin de metodo GetPuntoDeVentaServiceClient()

        public FlujoService.mar_flujoSoapClient GetFlujoEfectivoServiceClient(bool pUseBackupConnection, int flujoTimeoutSeconds = 30)
        {
            try
            {
                if (_clienteFlujoEfectivo != null && _clienteFlujoEfectivo.State != CommunicationState.Closed)
                {
                    try { _clienteFlujoEfectivo.CloseAsync(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

                _serverAddress = @"http://localhost:14217/mar-flujo.asmx";  //Remover esta linea al realizar el pase a Produccion :: OJO pendiente

                if ((pUseBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
                {
                    splitaddress = _serverBackUpAddress.Replace("localhost", ServiceHostIP).Split('/');
                }
                else
                {
                    splitaddress = _serverAddress.Split('/');
                }

                splitaddress[splitaddress.Length - 1] = "mar-flujo.asmx";

                endpoint = new EndpointAddress(string.Join("/", splitaddress));

                binding = new BasicHttpBinding(endpoint.Uri.Scheme.ToLower() == "http" ? BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);

                binding.ReceiveTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
                binding.OpenTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
                binding.CloseTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
                binding.SendTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);

                _clienteFlujoEfectivo = new FlujoService.mar_flujoSoapClient(binding, endpoint);

            }
            catch  
            {
                _clienteFlujoEfectivo = null;
            }

            return _clienteFlujoEfectivo;

        }//fin de metodo GetFlujoEfectivoServiceClient()




    }
}
