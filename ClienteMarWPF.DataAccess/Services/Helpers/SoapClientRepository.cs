using System;
using System.ServiceModel;
using System.Threading.Tasks;
using HaciendaService;
using MarPuntoVentaServiceReference;
using FlujoService;

using MAR.AppLogic.Encryption;


namespace ClienteMarWPF.DataAccess.Services.Helpers
{
    public class SoapClientRepository
    {
        string serveraddress = Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(MAR.Config.ConfigEnums.ServiceURL));
        string serverbackup = Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(MAR.Config.ConfigEnums.ServiceLocalURL));

        public int ClientTimeout { get; set; }   
        public string ServiceHostIP { get; set; }

        PtoVtaSoapClient marcliente;
        mar_flujoSoapClient controlefectivocliente;
        mar_haciendaSoapClient haciendacliente;


        public  PtoVtaSoapClient GetMarServiceClient(bool useBackupConnection, int timeoutInSeconds = 30)
        {
            try
            {
                if (marcliente != null && marcliente.State != CommunicationState.Closed)
                {
                    try { marcliente.CloseAsync().Wait(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

              
                serveraddress = @"http://pruebasmar.ddns.net/mar-svr5/mar-ptovta.asmx";
               // serveraddress = @"http://localhost/MarSrv/mar-ptovta.asmx";  //Remover esta linea al realizar el pase a Produccion :: OJO pendiente


                if ((useBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
                {
                    splitaddress = serverbackup.Replace("localhost", ServiceHostIP).Split('/');
                }
                else
                {
                    splitaddress = serveraddress.Split('/');
                }
                
                splitaddress[splitaddress.Length - 1] = "mar-ptovta.asmx";

                endpoint = new EndpointAddress(string.Join("/", splitaddress));

                binding = new BasicHttpBinding(endpoint.Uri.Scheme.ToLower() == "http" ? BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);
                binding.ReceiveTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.OpenTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.CloseTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.SendTimeout = new TimeSpan(0, 0, timeoutInSeconds);

                marcliente = new  PtoVtaSoapClient(binding, endpoint);               
            }
            catch 
            {
                marcliente = null;
            }

            return marcliente;
        } 

        public  mar_flujoSoapClient GetCashFlowServiceClient(bool useBackupConnection, int timeoutInSeconds = 30)
        {
            try
            {
                if (controlefectivocliente != null && controlefectivocliente.State != CommunicationState.Closed)
                {
                    try { controlefectivocliente.CloseAsync().Wait(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

                serveraddress = @"http://localhost:14217/mar-flujo.asmx";  //Remover esta linea al realizar el pase a Produccion :: OJO pendiente

                if ((useBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
                {
                    splitaddress = serverbackup.Replace("localhost", ServiceHostIP).Split('/');
                }
                else
                {
                    splitaddress = serveraddress.Split('/');
                }

                splitaddress[splitaddress.Length - 1] = "mar-flujo.asmx";

                endpoint = new EndpointAddress(string.Join("/", splitaddress));
                binding = new BasicHttpBinding(endpoint.Uri.Scheme.ToLower() == "http" ? BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);
                binding.ReceiveTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.OpenTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.CloseTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.SendTimeout = new TimeSpan(0, 0, timeoutInSeconds);

                controlefectivocliente = new mar_flujoSoapClient(binding, endpoint);
            }
            catch  
            {
                controlefectivocliente = null;
            }

            return controlefectivocliente;
        }

        public mar_haciendaSoapClient GetHaciendaServiceClient(bool useBackupConnection, int timeoutInSeconds = 30)
        {
            try
            {
                if (haciendacliente != null && haciendacliente.State != CommunicationState.Closed)
                {
                    try { haciendacliente.CloseAsync().Wait(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

                serveraddress = @"http://pruebasmar.ddns.net/mar-svr5/mar-hacienda.asmx";
                //serveraddress = @"http://localhost:14217/mar-hacienda.asmx";  //Remover esta linea al realizar el pase a Produccion :: OJO pendiente

                if ((useBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
                {
                    splitaddress = serverbackup.Replace("localhost", ServiceHostIP).Split('/');
                }
                else
                {
                    splitaddress = serveraddress.Split('/');
                }

                splitaddress[splitaddress.Length - 1] = "mar-hacienda.asmx";

                endpoint = new EndpointAddress(string.Join("/", splitaddress));
                binding = new BasicHttpBinding(endpoint.Uri.Scheme.ToLower() == "http" ? BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);
                binding.ReceiveTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.OpenTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.CloseTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.SendTimeout = new TimeSpan(0, 0, timeoutInSeconds);

                haciendacliente = new mar_haciendaSoapClient(binding, endpoint);
            }
            catch
            {
                haciendacliente = null;
            }

            return haciendacliente;
        }



    }
}



    