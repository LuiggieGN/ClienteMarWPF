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

namespace MAR.DataAccess.ViewModels
{
    public class PolizasSegurosViewModel
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
            public PolizasSegurosParameters.DatosCliente Cliente { get; set; }
            public PolizasSegurosParameters.DatosVehiculo Vehiculo { get; set; }

        }
        public class PolizasSegurosParameters : BaseParameter
        {
            public string Xid { get; set; }
            public string Doc { get; set; }
            public string TOKEN { get; set; }
            public int TOKEN_LIVE { get; set; }
            public string documento { get; set; }
            public string vig { get; set; }
            
            public class DatosCliente
            {
                public string nombre { get; set; }
                public string cedula { get; set; }
                public string telefono { get; set; }
                public string email { get; set; }
                public string ciudad { get; set; }
            }

            public class DatosVehiculo
            {
                public string marca { get; set; }
                public string modelo { get; set; }
                public string año { get; set; }
                public string tipo { get; set; }
                public string chasis { get; set; }
                public string placa { get; set; }
            }

            public static PolizasSegurosParameters Map_GetToken(string pUser, string pPass, int pTokenLive)
            {
                List<string> pList = new List<string>
                {
                    Url,
                    "Auth_Access.api2",
                     pUser,
                     pPass,
                     pTokenLive.ToString()
                };
                return new PolizasSegurosParameters
                {
                    ServiceUrl = UriGenerator(pList),
                };
            }
            public static PolizasSegurosParameters Map_GetPreciosYplanes(string pToken)
            {
                List<string> pList = new List<string>
                {
                     Url,
                     "Seguros.api2/0/0Auth",
                     pToken,
                     "0",
                     "precios",
                     "todos",
                     "todos"
                };
                return new PolizasSegurosParameters
                {
                    ServiceUrl = UriGenerator(pList)
                };
            }
            public static PolizasSegurosParameters Map_ComprarPoliza(string pToken, int pXid, string pDoc, DatosCliente pDatosCliente, DatosVehiculo pDatosVehiculo, string pVig)
            {
                List<string> pList = new List<string>
                {
                     Url,
                     "Seguros",
                     "compra.api",
                     pXid.ToString(),
                     "0Auth",
                     pToken,
                     pDoc,
                     pDatosCliente.telefono,
                     pDatosVehiculo.tipo,
                     pDatosVehiculo.chasis,
                     pVig
                };
                return new PolizasSegurosParameters
                {
                    ServiceUrl = UriGenerator(pList)
                };
            }
            public static PolizasSegurosParameters Map_ConsultarPoliza(string pToken,  string pDoc)
            {
                List<string> pList = new List<string>
                {
                     Url,
                    "Seguros.api2/00/0Auth",
                     pToken,
                     pDoc,
                     "cons"
                };
                return new PolizasSegurosParameters
                {
                    ServiceUrl = UriGenerator(pList)
                };
            }

            public static PolizasSegurosParameters Map_GetTipoVehiculos(string pToken)
            {
                List<string> pList = new List<string>
                {
                     Url,
                    "Seguros/tipoveh.api",
                     "1",
                     "0Auth",
                     pToken
                };
                return new PolizasSegurosParameters
                {
                    ServiceUrl = UriGenerator(pList)
                };
            }

            public static PolizasSegurosParameters Map_AnulaSeguro(string pToken, string pXid)
            {
                List<string> pList = new List<string>
                {
                     Url,
                    "Seguros/anular.api",
                     pXid,
                     "0Auth",
                     pToken
                };
                return new PolizasSegurosParameters
                {
                    ServiceUrl = UriGenerator(pList)
                };
            }

        }
        public static IEnumerable<Poliza> SetPolizasFromTransacciones(IEnumerable<MAR.DataAccess.Tables.DTOs.VP_Transaccion> transacciones)
        {
            var transaccion =  from p in transacciones
                select new Poliza
                {
                    TransaccionID = p.TransaccionID,
                    Ingresos = p.Ingresos,
                    FechaIngreso = p.FechaIngreso,
                    Estado = p.Estado,
                    Activo = p.Activo,
                    Referencia = p.Referencia,
                    PolizaNumero = p.ReferenciaCliente,
                    Vigencia = p.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia.ToUpper() == DataAccess.Tables.Enums.DbEnums.VP_TransaccionesDestallesReferenciasPolizas.Vigencia.ToString().ToUpper())?.ValorText,
                    Pin = BaseViewModel.GeneraPinGanador(p.TransaccionID),
                    Cliente = new PolizasSegurosParameters.DatosCliente
                    {
                        cedula = p.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia.ToUpper() == DataAccess.Tables.Enums.DbEnums.VP_TransaccionesDestallesReferenciasPolizas.Cedula.ToString().ToUpper())?.ValorText,
                        telefono = p.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia.ToUpper() == DataAccess.Tables.Enums.DbEnums.VP_TransaccionesDestallesReferenciasPolizas.Telefono.ToString().ToUpper())?.ValorText,
                        nombre = p.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia.ToUpper() == DataAccess.Tables.Enums.DbEnums.VP_TransaccionesDestallesReferenciasPolizas.Nombre.ToString().ToUpper())?.ValorText
                    },
                    Vehiculo = new PolizasSegurosParameters.DatosVehiculo
                    {
                        chasis = p.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia.ToUpper() == DataAccess.Tables.Enums.DbEnums.VP_TransaccionesDestallesReferenciasPolizas.Chasis.ToString().ToUpper())?.ValorText
                    }
                };

            return transaccion;

        }


        public class PolizasSegurosResponse : BaseResponse
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

            public static PolizasSegurosResponse Exec_GetToken(string pUser, string pPass, int pTokenLive)
            {
                var parameters = PolizasSegurosParameters.Map_GetToken(pUser, pPass, pTokenLive);
                return GenericMethods.CallServicePostAction<PolizasSegurosResponse, PolizasSegurosParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
            }
            public static PolizasSegurosResponse Exec_GetPreciosYplanes(string pToken)
            {
                var parameters = PolizasSegurosParameters.Map_GetPreciosYplanes(pToken);
                return GenericMethods.CallServicePostAction<PolizasSegurosResponse, PolizasSegurosParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
            }
            public static PolizasSegurosResponse Exec_ComprarPoliza(string pToken,int pXid, string pDoc, PolizasSegurosParameters.DatosCliente pDatosCliente, PolizasSegurosParameters.DatosVehiculo pDatosVehiculo, string pVig)
            {
                var parameters = PolizasSegurosParameters.Map_ComprarPoliza(pToken, pXid, pDoc, pDatosCliente, pDatosVehiculo, pVig);
                return GenericMethods.CallServicePostAction<PolizasSegurosResponse, PolizasSegurosParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
            }
            public static PolizasSegurosResponse Exec_ConsultarPoliza(string pToken,  string pDoc)
            {
                var parameters = PolizasSegurosParameters.Map_ConsultarPoliza(pToken, pDoc);
                return GenericMethods.CallServicePostAction<PolizasSegurosResponse, PolizasSegurosParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
            }
            public static PolizasSegurosResponse Exec_GetTipoVehiculos(string pToken)
            {
                var parameters = PolizasSegurosParameters.Map_GetTipoVehiculos(pToken);
                return GenericMethods.CallServicePostAction<PolizasSegurosResponse, PolizasSegurosParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
            }
            public static PolizasSegurosResponse Exec_AnularSeguro(string pToken, string pXid)
            {
                var parameters = PolizasSegurosParameters.Map_AnulaSeguro(pToken, pXid);
                return GenericMethods.CallServicePostAction<PolizasSegurosResponse, PolizasSegurosParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
            }
        }
        

        public static Uri UriGenerator(List<string> pList)
        {
            StringBuilder sB = new StringBuilder();
            foreach (var param in pList)
            {
                sB.Append(param + "/");
            }
            return new Uri(sB.ToString());
        }


    }
}
