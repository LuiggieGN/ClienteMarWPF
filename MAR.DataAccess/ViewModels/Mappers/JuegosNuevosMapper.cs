using JuegosTemplate.DataAccess.Servicios.LoteriaServicio.Utilidades;
using MAR.DataAccess.WSLotteryVIP;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;


namespace MAR.DataAccess.ViewModels.Mappers
{
   public class LotteryVIPMapper
    {
        public enum SalidaEn { JSON, XML }

        [Serializable, XmlRoot("RespuestaAuth")]
        public class LotteryVIP_Response
        {
            public string Autorizacion { get; set; }
            public string Referencia { get; set; }
            public string NumeroTicketPozo { get; set; }
            public string CodResp { get; set; }
            public string DescResp { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("Sorteo")]
            public SorteoGanador[] Sorteo { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("CartaBingo")]
            public CartaBingo CartaBingo { get; set; }
        }

        [Serializable, XmlRoot("RespConsultaBingoTicket")]
        public class Bingo_Response
        {
            public string Autorizacion { get; set; }
            public string Referencia { get; set; }
            public string TotalaPagar { get; set; }
            public string CodResp { get; set; }
            public string DescResp { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("Sorteo")]
            public Ganador[] Ganador { get; set; }

            [System.Xml.Serialization.XmlElementAttribute("CartaBingo")]
            public CartaBingo CartaBingo { get; set; }
        }

        [System.SerializableAttribute()]
        public partial class CartaBingo
        {
            public string NumeroCarta { get; set; }
            public string NumeroSerie { get; set; }
            public string CodSorteo { get; set; }
            public string NumeroSorteo { get; set; }
            public string FechaSorteo { get; set; }
            public string ReferenciaBingo { get; set; }
            public string linea1 { get; set; }
            public string linea2 { get; set; }
            public string linea3 { get; set; }
            public string linea4 { get; set; }
            public string linea5 { get; set; }

        }


        [System.SerializableAttribute()]
        public partial class Ganador
        {
            public string fechasorteo { get; set; }
            public string numerojuego { get; set; }
            public string figura { get; set; }
            public string cartaganadora { get; set; }
            public string valorpremio { get; set; }
            public string valorpremiocarta { get; set; }
            public string local { get; set; }
            public string consorcio { get; set; }
        }

        [System.SerializableAttribute()]
        public partial class SorteoGanador
        {
            public string Fecha { get; set; }
            public string Tanda { get; set; }
            public string Primera { get; set; }
            public string Segunda { get; set; }
            public string Tercera { get; set; }
            public string CodResp { get; set; }
            public string DescResp { get; set; }
        }
        /*****************************************************************************************************************************************************AutorizaJugada|(Async)***/

        public static LotteryVIP_Response AutorizaJugada(LoteriaServicioParametros param, SalidaEn output)
        {
            string resultado = "", Ticket_En_XML = "";
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
          
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);
            if (output == SalidaEn.XML)
            {
                StringWriter sw = new StringWriter();
                XmlWriter xmlWriter = XmlWriter.Create(sw);
                XmlDocument doc = param.Ticket_En_XML();
                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();
                Ticket_En_XML = sw.GetStringBuilder().ToString();
                resultado = cliente.AutorizaJugadaXml(param.Consorcio, param.Usuario, param.Password, Ticket_En_XML);
            }
           
            cliente.Close();
            LotteryVIP_Response response = DataAccess.UnitOfWork.GenericMethods.XmlMapper.DeserializeXML<LotteryVIP_Response>(resultado);
            return response;
        }

        public static Bingo_Response PagoGanadorBingo(LoteriaServicioParametros param, SalidaEn output, string pReferenciaBingo, double pMonto)
        {

            string resultado = "";

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            if (output == SalidaEn.XML)
            {
                resultado = cliente.PagoTicketBingo(param.Consorcio, param.Usuario, param.Password, pReferenciaBingo, pMonto.ToString(), "json");
            }
            else if (output == SalidaEn.JSON)
            {
                resultado = cliente.PagoTicketBingo(param.Consorcio, param.Usuario, param.Password, pReferenciaBingo, pMonto.ToString(), "json");

            }
            cliente.Close();
            Bingo_Response response = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON<Bingo_Response>(resultado);
            cliente.Close();

            return response;

        }
        public static Bingo_Response GanadorBingoPorTicket(LoteriaServicioParametros param, SalidaEn output, string pReferenciaBingo)
        {
            string resultado = "";
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            if (output == SalidaEn.XML)
            {
                resultado = cliente.GanadorBingoPorTicket(param.Consorcio, param.Usuario, param.Password, pReferenciaBingo, "json");
            }
            else if (output == SalidaEn.JSON)
            {
                resultado = cliente.GanadorBingoPorTicket(param.Consorcio, param.Usuario, param.Password, pReferenciaBingo, "json");
            }
            cliente.Close();
            Bingo_Response response = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON<Bingo_Response>(resultado);
            cliente.Close();

            return response;

        }//End ConsultaJugadasFecha()~

        public static async Task<string> AutorizaJugadaAsync(LoteriaServicioParametros param, SalidaEn output)
        {

            string resultado = "", Ticket_En_XML = "", Ticket_En_JSON = "";

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            if (output == SalidaEn.XML)
            {

                StringWriter sw = new StringWriter();
                XmlWriter xmlWriter = XmlWriter.Create(sw);
                XmlDocument doc = param.Ticket_En_XML();

                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();
                Ticket_En_XML = sw.GetStringBuilder().ToString();
                resultado = await cliente.AutorizaJugadaXmlAsync(param.Consorcio, param.Usuario, param.Password, Ticket_En_XML);

            }
            else if (output == SalidaEn.JSON)
            {                               /**CHECK THIS SECTION DON'T FORGET**/

                JObject obj = param.Ticket_En_JSON();
                Ticket_En_JSON = obj.ToString();
                resultado = await cliente.AutorizaJugadaJsonAsync(param.Consorcio, param.Usuario, param.Password, Ticket_En_JSON);

            }

            cliente.Close();

            return resultado;

        }//End AutorizaJugadaAsync()~


        /*****************************************************************************************************************************************************AnulaJugada|(Async)***/

        public static LotteryVIP_Response AnulaJugada(LoteriaServicioParametros param, SalidaEn output)
        {

            string resultado = "";

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            if (output == SalidaEn.XML)
            {
                resultado = cliente.AnulaJugadaXml(param.Consorcio, param.Usuario, param.Password, param.Autorizacion);
                LotteryVIP_Response response = DataAccess.UnitOfWork.GenericMethods.XmlMapper.DeserializeXML<LotteryVIP_Response>(resultado);
                return response;
            }
            else if (output == SalidaEn.JSON)
            {
                resultado = cliente.AnulaJugadaJson(param.Consorcio, param.Usuario, param.Password, param.Autorizacion);
            }

            cliente.Close();

            return null;

        }//End AnulaJugada()~

        public static async Task<string> AnulaJugadaAsync(LoteriaServicioParametros param, SalidaEn output)
        {

            string resultado = "";

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            if (output == SalidaEn.XML)
            {

                resultado = await cliente.AnulaJugadaXmlAsync(param.Consorcio, param.Usuario, param.Password, param.Autorizacion);
            }
            else if (output == SalidaEn.JSON)
            {

                resultado = await cliente.AnulaJugadaJsonAsync(param.Consorcio, param.Usuario, param.Password, param.Autorizacion);

            }

            cliente.Close();

            return resultado;

        }//End AnulaJugadaAsync()~

        /*****************************************************************************************************************************************************ConsultaJugadaFecha|(Async)***/

        public static string ConsultaJugadasFecha(LoteriaServicioParametros param, SalidaEn output)
        {

            string resultado = "";

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            if (output == SalidaEn.XML)
            {
                resultado = cliente.ConsultaJugadasFechaXml(param.Consorcio, param.Usuario, param.Password, param.FechaDeJugadas, param.PPagina);
            }
            else if (output == SalidaEn.JSON)
            {
                resultado = cliente.ConsultaJugadasFechaJson(param.Consorcio, param.Usuario, param.Password, param.FechaDeJugadas, param.PPagina);

            }

            cliente.Close();

            return resultado;

        }//End ConsultaJugadasFecha()~

        public static async Task<string> ConsultaJugadasFechaAsync(LoteriaServicioParametros param, SalidaEn output)
        {

            string resultado = "";

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            if (output == SalidaEn.XML)
            {
                resultado = await cliente.ConsultaJugadasFechaXmlAsync(param.Consorcio, param.Usuario, param.Password, param.FechaDeJugadas, param.PPagina);
            }
            else if (output == SalidaEn.JSON)
            {
                resultado = await cliente.ConsultaJugadasFechaJsonAsync(param.Consorcio, param.Usuario, param.Password, param.FechaDeJugadas, param.PPagina);
            }

            cliente.Close();

            return resultado;

        }//End ConsultaJugadasFechaAsync()~

        /*****************************************************************************************************************************************************ResultadoSorteos|(Async)***/

        public static LotteryVIP_Response ResultadoSorteos(LoteriaServicioParametros param)
        {
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

           LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);
            string resultado = "";
          //  LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(EndPoint);
            resultado = cliente.ResultadoSorteos(param.FechaDeResultadosDeSorteos, "YYYY-mm-dd");
            cliente.Close();
            LotteryVIP_Response response = DataAccess.UnitOfWork.GenericMethods.XmlMapper.DeserializeXML<LotteryVIP_Response>(resultado);
            return response;

        }//ResultadoSorteos()~

        public static async Task<string> ResultadoSorteosAsync(LoteriaServicioParametros param)
        {

            string resultado = "";

            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();
            Uri uri = new Uri(param.EndPointAddress);
            if (uri.Scheme == Uri.UriSchemeHttps)
            {
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.Transport;
                binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            }
            System.ServiceModel.EndpointAddress address = new System.ServiceModel.EndpointAddress(param.EndPointAddress);

            LotteryAuthServiceClient cliente = new LotteryAuthServiceClient(binding, address);

            resultado = await cliente.ResultadoSorteosAsync(param.FechaDeResultadosDeSorteos, "YYYY-mm-dd");

            cliente.Close();

            return resultado;

        }//ResultadoSorteosAsync()~
    }
}
