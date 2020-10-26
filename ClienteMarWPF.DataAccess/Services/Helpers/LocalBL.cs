using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.ServiceModel;
using System.Text;
using MAR.Config;
using MAR.AppLogic.Encryption;

//using static MAR.AppLogic.MARHelpers.DapperDBHelper;

namespace ClienteMarWPF.DataAccess.Services.Helpers
{
    public class LocalBL
    {
        string _serverAddress = Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(MAR.Config.ConfigEnums.ServiceURL));
        string _serverBackUpAddress = Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(MAR.Config.ConfigEnums.ServiceLocalURL));

        /// Instancias de clientes
        FlujoServices.mar_flujoSoapClient _soapFlujoClient;

        public int ClientTimeout { get; set; }
        public string ServiceHostIP { get; set; }


        public FlujoServices.mar_flujoSoapClient GetFlujoServiceClient(bool pUseBackupConnection, int flujoTimeoutSeconds = 30)
        {
            try
            {
                if (_soapFlujoClient != null && _soapFlujoClient.State != CommunicationState.Closed)
                {
                    try { _soapFlujoClient.CloseAsync(); } catch { }
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

                _soapFlujoClient = new FlujoServices.mar_flujoSoapClient(binding, endpoint);

                return _soapFlujoClient;
            }
            catch (Exception excepcion)
            {
                throw excepcion;
            }

        }//fin de metodo GetFlujoServiceClient()




    }
}
