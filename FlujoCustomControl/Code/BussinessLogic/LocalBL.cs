using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.ServiceModel;
using System.Text;
using static MAR.AppLogic.MARHelpers.DapperDBHelper;

namespace FlujoCustomControl.Code.BussinessLogic
{
    public class LocalBL
    {
        string _serverAddress = ConfigReader.ReadString(MAR.Config.ConfigEnums.ServiceURL);
        string _serverBackUpAddress = ConfigReader.ReadString(MAR.Config.ConfigEnums.ServiceLocalURL);
        FlujoServices.mar_flujoSoapClient _soapFlujoClient;
        BingoServices.mar_bingoSoapClient _soapBingoClient;


        public int ClientTimeout  {     get; set;   }
        public string ServiceHostIP { get; set; }
        long MinuteDiff = 0;
        bool _HuboFraudeEnHora = false;
        bool _MdbCopyFailed = false;
        string _MdbWinPath = String.Empty;
        string cnstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\\mar\\lc5.mdb;Mode=Share Exclusive";
        OleDbConnection cn = new OleDbConnection();
        Dictionary<int, string> _Crosswalk;

        public FlujoServices.mar_flujoSoapClient GetFlujoServiceClient(bool pUseBackupConnection, int flujoTimeoutSeconds = 30)
        {

            try
            {
                if (_soapFlujoClient != null && _soapFlujoClient.State != CommunicationState.Closed)
                {
                    try { _soapFlujoClient.Close(); } catch { }
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





            //try
            //{
            //    if (_soapFlujoClient != null && _soapFlujoClient.State != CommunicationState.Closed)
            //    {
            //        try
            //        {
            //            _soapFlujoClient.Close();
            //        }
            //        catch (Exception ex)
            //        {
            //            // No big issue
            //        }
            //    }




            //    BasicHttpBinding binding = new BasicHttpBinding("mar_flujoSoap");
            //    binding.ReceiveTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
            //    binding.OpenTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
            //    binding.CloseTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
            //    binding.SendTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);

            //    EndpointAddress endpoint;
            //    string[] bingoAddress;

            //    _serverAddress = @"http://localhost:14217/mar-flujo.asmx";  

            //    if ((pUseBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
            //    {
            //        bingoAddress = _serverBackupAddress.Replace("localhost", ServiceHostIP).Split('/');
            //    }
            //    else
            //    {
            //        bingoAddress = _serverAddress.Split('/');
            //    }


            //    bingoAddress[bingoAddress.Length - 1] = "mar-flujo.asmx";
            //    endpoint = new EndpointAddress(string.Join("/", bingoAddress));
            //    _soapFlujoClient = new FlujoServices.mar_flujoSoapClient(binding, endpoint);
            //    return _soapFlujoClient;
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}



        }

        public BingoServices.mar_bingoSoapClient GetBingoServiceClient(bool pUseBackupConnection, int flujoTimeoutSeconds = 30)
        {

            if (_soapBingoClient != null && _soapBingoClient.State != CommunicationState.Closed)
            {
                try
                {
                    _soapBingoClient.Close();
                }
                catch (Exception ex)
                {
                    // No big issue
                }
            }
            BasicHttpBinding binding = new BasicHttpBinding("mar_bingoSoap");
            binding.ReceiveTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
            binding.OpenTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
            binding.CloseTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);
            binding.SendTimeout = new TimeSpan(0, 0, flujoTimeoutSeconds);

            EndpointAddress endpoint;
            string[] bingoAddress;


            if ((pUseBackupConnection && ServiceHostIP != null && ServiceHostIP.Length > 0))
            {
                bingoAddress = _serverBackUpAddress.Replace("localhost", ServiceHostIP).Split('/');
            }
            else
            {
                bingoAddress = _serverAddress.Split('/');
            }

            bingoAddress[bingoAddress.Length - 1] = "mar-bingo.asmx";
            endpoint = new EndpointAddress(string.Join("/", bingoAddress));
            _soapBingoClient = new BingoServices.mar_bingoSoapClient(binding, endpoint);
            return _soapBingoClient;

        }

    }
}
