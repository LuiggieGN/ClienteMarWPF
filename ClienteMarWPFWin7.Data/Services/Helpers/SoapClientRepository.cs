using System;
using System.ServiceModel;
using System.Threading.Tasks;
using ClienteMarWPFWin7.Domain.HaciendaService;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.FlujoService;
using ClienteMarWPFWin7.Domain.JuegaMasService;
using ClienteMarWPFWin7.Domain.BingoService;
using MAR.AppLogic.Encryption;


namespace ClienteMarWPFWin7.Data.Services.Helpers
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
        JuegaMasSoapClient juegamascliente;
        mar_bingoSoapClient bingocliente;


        public PtoVtaSoapClient GetMarServiceClient(bool useBackupConnection, int timeoutInSeconds = 30)
        {
            try
            {
                if (marcliente != null && marcliente.State != CommunicationState.Closed)
                {
                    try { marcliente.Close(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

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

                marcliente = new PtoVtaSoapClient(binding, endpoint);
            }
            catch(Exception ex)
            {
                marcliente = null;
            }

            return marcliente;
        }
        public mar_flujoSoapClient GetCashFlowServiceClient(bool useBackupConnection, int timeoutInSeconds = 1200)
        {
            try
            {
                if (controlefectivocliente != null && controlefectivocliente.State != CommunicationState.Closed)
                {
                    try { controlefectivocliente.Close(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

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
                    try { haciendacliente.Close(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

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
        public JuegaMasSoapClient GetJuegaMasServiceClient(bool useBackupConnection, int timeoutInSeconds = 30)
        {
            try
            {
                if (juegamascliente != null && juegamascliente.State != CommunicationState.Closed)
                {
                    try { juegamascliente.Close(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

                if ((useBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
                {
                    splitaddress = serverbackup.Replace("localhost", ServiceHostIP).Split('/');
                }
                else
                {
                    splitaddress = serveraddress.Split('/');
                }

                splitaddress[splitaddress.Length - 1] = "mar-juegamas.asmx";

                endpoint = new EndpointAddress(string.Join("/", splitaddress));

                binding = new BasicHttpBinding(endpoint.Uri.Scheme.ToLower() == "http" ? BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);
                binding.ReceiveTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.OpenTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.CloseTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.SendTimeout = new TimeSpan(0, 0, timeoutInSeconds);

                juegamascliente = new JuegaMasSoapClient(binding, endpoint);

            }
            catch
            {
                juegamascliente = null;
            }

            return juegamascliente;
        }


        public mar_bingoSoapClient GetBingoServiceClient(bool useBackupConnection, int timeoutInSeconds = 30)
        {
            try
            {
                if (bingocliente != null && bingocliente.State != CommunicationState.Closed)
                {
                    try { bingocliente.Close(); } catch { }
                }

                BasicHttpBinding binding;
                EndpointAddress endpoint;
                string[] splitaddress;

                if ((useBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
                {
                    splitaddress = serverbackup.Replace("localhost", ServiceHostIP).Split('/');
                }
                else
                {
                    splitaddress = serveraddress.Split('/');
                }

                splitaddress[splitaddress.Length - 1] = "mar-bingo.asmx";

                endpoint = new EndpointAddress(string.Join("/", splitaddress));

                binding = new BasicHttpBinding(endpoint.Uri.Scheme.ToLower() == "http" ? BasicHttpSecurityMode.None : BasicHttpSecurityMode.Transport);
                binding.ReceiveTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.OpenTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.CloseTimeout = new TimeSpan(0, 0, timeoutInSeconds);
                binding.SendTimeout = new TimeSpan(0, 0, timeoutInSeconds);

                bingocliente = new mar_bingoSoapClient(binding, endpoint);

            }
            catch
            {
                bingocliente = null;
            }

            return bingocliente;
        }







    }
}



