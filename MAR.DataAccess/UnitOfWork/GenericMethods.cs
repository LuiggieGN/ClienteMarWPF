using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MAR.AppLogic.MARHelpers;
using MAR.Config;
using MAR.DataAccess.Tables.Enums;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace MAR.DataAccess.UnitOfWork
{
    public class GenericMethods
    {
        public static T CallServicePostAction<T, P>(ProductosExternosEnums.ServiceMethod? pServiceMethod, P pMethodParameter, ProductosExternosEnums.HttpMethod pHttpMethod)
            where T : BaseResponse
            where P : BaseParameter
        {
            var theFullUri = String.Format("{0}{1}{2}",
                pMethodParameter.ServiceUrl,
                pServiceMethod == null ? String.Empty : pServiceMethod.ToString(),
                pMethodParameter.ServiceUrl.ToString().Substring(pMethodParameter.ServiceUrl.ToString().Length - 1, 1) == "/" ? String.Empty : "/");

            string theParams = JsonConvert.SerializeObject(pMethodParameter);

            var theClient = new HttpClient();
            theClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var theForm = new StringContent(theParams,
                                            Encoding.UTF8,
                                            "application/json");



            if (pMethodParameter.HeardersDictionary != null)
                foreach (var header in pMethodParameter.HeardersDictionary)
                {
                    theClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
                }


            Task<HttpResponseMessage> theGet;
            if (ProductosExternosEnums.HttpMethod.GET == pHttpMethod)
            {
                theGet = theClient.GetAsync(theFullUri);
            }
            else if (ProductosExternosEnums.HttpMethod.POST == pHttpMethod)
            {
                theGet = theClient.PostAsync(theFullUri, theForm);
            }
            else
            {
                theGet = theClient.PutAsync(theFullUri, theForm);
            }
            ////.... more here

            theGet.Wait();
            theGet.Result.EnsureSuccessStatusCode();
            var theJson = theGet.Result.Content.ReadAsStringAsync();
            theJson.Wait();

            return JsonConvert.DeserializeObject<T>(theJson.Result);
        }


        public static bool SendSMS()
        {
            //prueba parametros



            HttpClient MiCliente = new HttpClient();
            MiCliente.BaseAddress = new Uri("https://api.sms.to/v1/oauth/token");
            MiCliente.DefaultRequestHeaders.Accept.Clear();
            MiCliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));   // new MediaTypeWithQualityHeaderValue("application/json")


            //var byteArray = Encoding.ASCII.GetBytes("c6e23a029785154818fc137f373e8a10:7ada3a8cdfa889723553b69b3b6c4ac0");
            //MiCliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            try
            {
                Dictionary<string, string> parametros = new Dictionary<string, string>();

                Mensaje msj = new Mensaje
                {
                    grant_type = "password",
                    client_id = "122",
                    client_secret = "Bll0aUFNsLbl99ikM9nZSI4PgiK2rIc4qW49bNc9",
                    username = "jasiel06@gmail.com",
                    password = "Jasiel.6382",
                    scope = "*"
                };

                string data = MAR.AppLogic.MARHelpers.JSONHelper.SerializeToJSON(msj);

                var resultadoF = MiCliente.PostAsync(MiCliente.BaseAddress, new StringContent(data, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                var resultado = resultadoF.Result;

                var response = MAR.AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON<Respuesta>(resultado);

                if (true)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                string t = "";
                return false;
            }
        }


        public static bool SendSMSTest(string pToken)
        {
            //prueba parametros


            HttpClient MiCliente = new HttpClient();
            MiCliente.BaseAddress = new Uri("https://api.sms.to/v1/oauth/token");
            MiCliente.DefaultRequestHeaders.Accept.Clear();
            MiCliente.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));   // new MediaTypeWithQualityHeaderValue("application/json")

            //var byteArray = Encoding.ASCII.GetBytes("c6e23a029785154818fc137f373e8a10:7ada3a8cdfa889723553b69b3b6c4ac0");
            //MiCliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

            try
            {
                Dictionary<string, string> parametros = new Dictionary<string, string>();

                Mensaje msj = new Mensaje
                {
                    grant_type = "password",
                    client_id = "122",
                    client_secret = "Bll0aUFNsLbl99ikM9nZSI4PgiK2rIc4qW49bNc9",
                    username = "jasiel06@gmail.com",
                    password = "Jasiel.6382",
                    scope = "*"
                };

                string data = MAR.AppLogic.MARHelpers.JSONHelper.SerializeToJSON(msj);

                var resultadoF = MiCliente.PostAsync(MiCliente.BaseAddress, new StringContent(data, Encoding.UTF8, "application/json")).Result.Content.ReadAsStringAsync();
                var resultado = resultadoF.Result;

                var response = MAR.AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON<Respuesta>(resultado);

                if (true)
                {
                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                string t = "";
                return false;
            }
        }


        public class XmlMapper
        {
            public static T DeserializeXML<T>(string strXML)
            {
                XmlSerializer serial = new XmlSerializer(typeof(T));
                StringReader reader = new StringReader(strXML);
                T returnValue = default(T);
                try
                {
                    object result = serial.Deserialize(reader);


                    if (result != null && result is T)
                    {
                        returnValue = ((T)result);
                    }

                }
                catch (Exception exp)
                {

                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
                return returnValue /*(T)Convert.ChangeType(obj, typeof(T))*/;
            }
        }

        public class Mensaje
        {
            public string grant_type { get; set; }
            public string client_id { get; set; }
            public string client_secret { get; set; }
            public string username { get; set; }
            public string password { get; set; }
            public string scope { get; set; }
        }

        public class Respuesta
        {
            public Response respuesta { get; set; }
            public class Response
            {
                public string token_type { get; set; }
                public string expires_in { get; set; }
                public string access_token { get; set; }
                public string refresh_token { get; set; }
            }
        }
        public static string SetEfConnectionString(string pContextPath, ConfigEnums pConfigEnums)
        {
            //pContextPath ej: Namespace.ContextName  => Folder1.Folder2.MARContext
            var providerString = AppLogic.MARHelpers.DALHelper.ConfigReader.ReadString(pConfigEnums);
            return String.Format(
                @"metadata=res://*/{1}.csdl|res://*/{1}.ssdl|res://*/{1}.msl;provider=System.Data.SqlClient;provider connection string=""{0}""",
                providerString, pContextPath);
        }

        public static string SetEfConnectionString(ConfigEnums pConfigEnums)
        {
            return AppLogic.MARHelpers.DALHelper.ConfigReader.ReadString(pConfigEnums);
        }
    }


}
