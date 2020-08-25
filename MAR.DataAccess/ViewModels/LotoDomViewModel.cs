using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MAR.DataAccess.Tables.ViewModels;
using MAR.DataAccess.UnitOfWork;
using MAR.DataAccess.Tables.Enums;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using MarConnectCliente;
using MarConnectCliente.RequestModels;
using static MarConnectCliente.Enums.MetodosEnum;
using MarConnectCliente.ResponseModels;

namespace MAR.DataAccess.ViewModels
{
    public class LotoDomViewModel
    {
        public static string TOKEN { get; set; }
        public static string User { get; set; }
        public static string Pass { get; set; }
        public static string Url { get; set; }

        public class Poliza
        {
            public int TransaccionID { get; set; }
            public decimal Ingresos { get; set; }
            public DateTime FechaIngreso { get; set; }
            public string Estado { get; set; }
            public string Vigencia { get; set; }
            public string Referencia { get; set; }
            public string Pin { get; set; }
            public string PolizaNumero { get; set; }
            public bool Activo { get; set; }

        }



        public class LotoDomParameters : BaseParameter
        {
            public string Xid { get; set; }
            public string Doc { get; set; }
            public string TOKEN { get; set; }
            public int TOKEN_LIVE { get; set; }
            public string documento { get; set; }
            public string vig { get; set; }
            
       

          
         
           

          
        }
    


        public class ApuestaResponse : BaseResponseModel
        {
            public int cod { get; set; }
            public string msj { get; set; }
            public string moneda { get; set; }
            public string token { get; set; }
            public double balance { get; set; }
            public DateTime expira_en { get; set; }
            public string aut_cod { get; set; }
            public string pol_no { get; set; }
            public string ticket { get; set; }
            public cliente Cliente { get; set; }
             

            public IDictionary<string, Precio> precios { get; set; }
            public TipoVehiculo[] tipos { get; set; }

            public IDictionary<string, poliza> polizas { get; set; }

            public class poliza
            {
                public string pol_no { get; set; }
                public string modelo { get; set; }
                public string Marca { get; set; }
                public string tipo_veh { get; set; }
                public string anio { get; set; }
                public string chasis { get; set; }
                public string fecha { get; set; }
                public string x_id { get; set; }
                public string estado { get; set; }
            }
            public class cliente
            {
                public string nombres { get; set; } 
                public string telefono { get; set; } 
                public string ciudad { get; set; } 
            }

            public class Precio
            {
                public int? prec_id { get; set; }
                public string veh_tipo { get; set; }
                public string vigencia { get; set; }
                public double? precio { get; set; }
                public string nombre { get; set; }
            }

            public class TipoVehiculo
            {
                public int? id { get; set; }
                public string tipo_veh { get; set; }
                public string nombre { get; set; }
            }

            public class LotoDomApuestaParameters : BaseRequestModel
            {
           
            }

            public static LotoDomApuestaParameters Map_Apuesta(string pUrl, string pUser, string pPass, string pJugadasEnString, string pSecuencia,
                string pGrupo, string pBank)
            {

                List<string> pList = new List<string>
                {
                   pUrl
                };

                var trama = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                "t=" + pUser,
                ":",
                pPass,
                "*",
                "validation",
                "*",
                pJugadasEnString,
                 "*",
                pSecuencia,
                 "*",
                pGrupo,
                 "*",
                pBank);

                //        t=user.ventas:user.ventas1234*validacion*2|11|15|N;2|12|10|N;*1002028*1*0001
                //var t ="t=user.ventas:user.ventas1234*validation*2|11|15|N;2|12|10|N;*1002028*1*0001";
                return new LotoDomApuestaParameters
                {
                    ServiceUrl = UriGenerator(pList)
                    ,CurlString = trama
                };

                //ServiceUrl = new Uri("http://gameone2.midas.software/api/play"),// UriGenerator(pList)
                //    HeardersDictionary = new Dictionary<string, string> { { "user", "user.ventas" }, { "pass", "user.ventas1234" },
                //    { "validation", "validation" }, { "PLAYS", "2|11|15|N;2|12|10|N;" }, { "SEQUENCE", "1002028" }, { "GROUP", "1" }, { "BANK", "0001" } }
            }

            

            public static ApuestaResponse Exec_Apuesta(string pUrl, string pUser, string pPass, string pJugadasEnString, string pSecuencia,
               string pGrupo, string pBank)
            {
                var parameters = Map_Apuesta(pUrl, pUser, pPass, pJugadasEnString, pSecuencia,  pGrupo, pBank);
               // return GenericMethods.CallServicePostAction<ApuestaResponse, LotoDomApuestaParameters>(null, parameters, ProductosExternosEnums.HttpMethod.POST);
                return ClienteHTTP.CallService<ApuestaResponse, LotoDomApuestaParameters>(null, parameters,HttpMethod.POST,null,false,true);
            }
        }
        

        public static Uri UriGenerator(List<string> pList)
        {
            StringBuilder sB = new StringBuilder();
            foreach (var param in pList)
            {
                sB.Append(param + "/");
            }
            string stringToReturn = sB.ToString().Substring(0, sB.ToString().Length - 1);
            return new Uri(stringToReturn);
        }


    }
}
