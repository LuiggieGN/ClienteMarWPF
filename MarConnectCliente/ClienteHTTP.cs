using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace MarConnectCliente
{
    public class ClienteHTTP
    {
        public static T CallService<T, P>(Enums.MetodosEnum.MetodoServicio? pServiceMethod, P pMethodParameter,
                                          Enums.MetodosEnum.HttpMethod pHttpMethod,
                                          AuthenticationHeaderValue pAuthenticationHeaderValue = null, bool pUsaCertificado = false, bool pIsCurlContent = false)
         where T : MarConnectCliente.ResponseModels.BaseResponseModel
         where P : MarConnectCliente.RequestModels.BaseRequestModel
        {
            try
            {
                var theFullUri = String.Format("{0}{1}",
           pMethodParameter.ServiceUrl,
           pServiceMethod == null ? String.Empty : pServiceMethod.ToString(),
           pMethodParameter.ServiceUrl.ToString().Substring(pMethodParameter.ServiceUrl.ToString().Length - 1, 1) == "/" ? String.Empty : "/");

                string theParams = JsonConvert.SerializeObject(pMethodParameter);

                ////GET CERTIFICADO
                //// Add the certificate
                //WebRequestHandler handler = new WebRequestHandler();
                //X509Certificate2 cert = GetMyCert();
                //if (cert != null)
                //{
                //    handler.ClientCertificates.Add(cert);
                //}
                
            
                var theClient = new HttpClient();
                if (pUsaCertificado)
                {
                    theClient =  httpClient;
                }
                //ME QUEDE PONIENDO SI USA SSL O NO A LA LLAMADA DE ESTE METODO


                //theClient.Timeout = new TimeSpan(0, 0, 15);

                //theClient.DefaultRequestHeaders.Accept.Clear();
                //theClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var theForm = new StringContent(theParams,
                                                Encoding.UTF8,
                                                "application/json");


                //pIsCurlContent = true;//para prueba lo fuerzo
                if (pIsCurlContent)
                {
                    theForm = new StringContent(pMethodParameter.CurlString,
                                   Encoding.UTF8,
                                   "application/x-www-form-urlencoded");
                }

                if (pAuthenticationHeaderValue != null)
                {
                    theClient.DefaultRequestHeaders.Authorization = pAuthenticationHeaderValue;
                }

                if (pMethodParameter.HeardersDictionary != null)
                    foreach (var header in pMethodParameter.HeardersDictionary)
                    {

                        theClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
                    }

                Task<HttpResponseMessage> theGet;

                ServicePointManager.FindServicePoint(pMethodParameter.ServiceUrl).ConnectionLeaseTimeout = (int)TimeSpan.FromMinutes(1).TotalMilliseconds;

                if (Enums.MetodosEnum.HttpMethod.GET == pHttpMethod)
                {
                    theGet = theClient.GetAsync(theFullUri);
                }
                else if (Enums.MetodosEnum.HttpMethod.POST == pHttpMethod)
                {
                    theGet = theClient.PostAsync(theFullUri, theForm);
                }
                else
                {
                    theGet = theClient.PutAsync(theFullUri, theForm);
                }
                ////.... more here
                FechaSolicitud = DateTime.Now;
                theGet.Wait();
                FechaRespuesta = DateTime.Now;
                //theGet.Result.EnsureSuccessStatusCode();
                var theJson = theGet.Result.Content.ReadAsStringAsync();
                theJson.Wait();

                return JsonConvert.DeserializeObject<T>(theJson.Result);
            }
            catch (Exception e)
            {
                Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories.BancaRepositorio.AgregaErrorPrueba(e.Message + e.StackTrace);
                throw e;
            }

        }

        private static HttpClient httpClient { get { return myHttpClient ?? GetHttpClient(); } set { } }
        private static HttpClient myHttpClient { get; set; }
        private static HttpClient GetHttpClient()
        {
            //GET CERTIFICADO
            WebRequestHandler handler = new WebRequestHandler();
            X509Certificate2 cert = GetMyCert();
            if (cert != null)
            {
                handler.ClientCertificates.Add(cert);
            }
            myHttpClient = new HttpClient(handler);
            myHttpClient.Timeout = new TimeSpan(0, 0, 15);
            myHttpClient.DefaultRequestHeaders.Accept.Clear();
            myHttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return myHttpClient;
        }

        public static DateTime FechaSolicitud { get; set; }
        public static DateTime FechaRespuesta { get; set; }
        public static X509Certificate2 GetMyCert()
        {
            var certValores = Flujo.DataAccess.Hacienda.CertificadoRepository.GetCertificado();
            String certFileName = certValores.Nombre;//@"C:\VFuera\CertHacienda\Mateo_Comunicaciones_SRL.pfx";
            String passCert = certValores.Password;// "PgpdUnvBg81/hnc3ABtwfg==";
            //Especifique la ruta fisica a donde se encuentra el Certificado Cliente en formato PKCS#12. 
            X509Certificate2 certificate = new X509Certificate2(certFileName, passCert);

            //Para manejar cualquier error dado en el Certificado Servidor 
           // ServicePointManager.CheckCertificateRevocationList = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = (a, b, c, d) => true;
            ServicePointManager.Expect100Continue = true;

            return certificate;
        }


    }


}
