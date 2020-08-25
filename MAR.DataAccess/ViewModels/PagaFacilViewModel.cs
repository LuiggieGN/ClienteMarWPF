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

namespace MAR.DataAccess.ViewModels
{
    public class PagaFacilViewModel
    {
        public static string TOKEN { get; set; }
        public static string User { get; set; }
        public static string Pass { get; set; }
        public static string Url { get; set; }

        public class  Factura
        {
            public int TransaccionID { get; set; }
            public decimal Ingresos { get; set; }
            public DateTime FechaIngreso { get; set; }
            public string Estado { get; set; }
            public string Referencia { get; set; }
            public string Pin { get; set; }
            public string Nic { get; set; }
            public bool Activo { get; set; }
            public Suplidor Suplidor { get; set; }
        }

        public class Suplidor
        {
            public string Nombre { get; set; }  
            public string Referencia { get; set; }  
        }

        public static IEnumerable<Factura>  SetFacturasFromTransacciones(IEnumerable<MAR.DataAccess.Tables.DTOs.VP_Transaccion> transacciones )
        {
            return from p in transacciones select new Factura
            {
                TransaccionID = p.TransaccionID,
                Ingresos = p.Ingresos,
                FechaIngreso = p.FechaIngreso,
                Estado = p.Estado,
                Activo = p.Activo,
                Referencia = p.Referencia,
                Nic = p.ReferenciaCliente,
                Pin = BaseViewModel.GeneraPinGanador(p.TransaccionID),
                Suplidor = new Suplidor { Nombre = p.VP_Suplidor.Nombre, Referencia = p.VP_Suplidor.Referencia }
            };
        }


        public class PagaFacilMidasParameters : BaseParameter
        {
                public string Moneda { get; set; }
                public string Servicio { get; set; }
                public string Fecha1 { get; set; }
                public string Fecha2 { get; set; }
                public string Xid { get; set; }
                public string Pag { get; set; }
                public string TOKEN { get; set; }
                public string Proveedor { get; set; }
                public string categoria { get; set; }
                public int TOKEN_LIVE { get; set; }
                public int NIC { get; set; }
                public double monto { get; set; }


             

                public static PagaFacilMidasParameters Map_GetToken(string pUser, string pPass, int pTokenLive)
                {
                    List<string> pList = new List<string>
                    {
                         Url,
                        "Auth_Access.api2",
                         pUser,
                         pPass,
                         pTokenLive.ToString()
                    };
                    return new PagaFacilMidasParameters
                    {
                        ServiceUrl = UriGenerator(pList),
                    };
                }

                public static PagaFacilMidasParameters Map_GetBalance(string pToken)
                {
                    List<string> pList = new List<string>
                    {
                        Url,
                        "Balance.api2/0Auth",
                         pToken.ToString(),
                         "DOP"
                    };
                    return new PagaFacilMidasParameters
                    {
                        ServiceUrl = UriGenerator(pList),
                    };
                }
                public static PagaFacilMidasParameters Map_GetProveedores(string pToken, string pCategoria)
                {
                    List<string> pList = new List<string>
                    {
                        Url,
                        "Serv_Servicios.api2/obt-proveedores/0Auth",
                         pToken,
                         pCategoria
                    };
                    return new PagaFacilMidasParameters
                    {
                        ServiceUrl = UriGenerator(pList),
                    };
                }
                public static PagaFacilMidasParameters Map_ConsultarFacturas(string pToken, string pNic, string pXid, string pProveedor, string pMoneda)
                {
                    List<string> pList = new List<string>
                    {
                         Url,
                        "PagaFacil.api2",
                         pProveedor,
                         pXid,
                         "0Auth",
                         pToken,
                         pNic,
                         "cons",
                         pMoneda
                    };
                    return new PagaFacilMidasParameters
                    {
                        ServiceUrl = UriGenerator(pList),
                    };
                }
                public static PagaFacilMidasParameters Map_PagarFactura(string pToken, string pProveedor, string pXid, string pNic, string pMoneda, decimal pMonto)
                {
                    List<string> pList = new List<string>
                    {
                        Url,
                        "PagaFacil.api2",
                         pProveedor,
                         pXid,
                         "0Auth",
                         pToken,
                         pNic.ToString(),
                         "pago",
                         pMoneda,
                         AppLogic.MARHelpers.NumbersHelpers.FormatNumero(pMonto, AppLogic.MARHelpers.NumbersHelpers.FormatoEnum.DecimalSinComa).ToString(),
                         "0"
                    };
                    return new PagaFacilMidasParameters
                    {
                        ServiceUrl = UriGenerator(pList),
                    };
                }
                public static PagaFacilMidasParameters Map_AnularPagoFactura(string pToken, string pProveedor, string pXid, string pNic)
                {
                    List<string> pList = new List<string>
                    {
                         Url,
                        "PagaFacil.api2",
                         pProveedor,
                         pXid,
                         "0Auth",
                         pToken,
                         pNic.ToString(),
                         "anular"
                    };
                    return new PagaFacilMidasParameters
                    {
                        ServiceUrl = UriGenerator(pList),
                    };
                }
                public static PagaFacilMidasParameters Map_VerificarTransacciones(string pToken, string pProveedor, string pXid, DateTime pFechaDesde, DateTime pFechaHasta, int? pPage)
                {
                    List<string> pList = new List<string>
                    {
                        Url,
                        "Serv_Servicios.api2/gettransac/0Auth",
                         pToken,
                         pProveedor,
                         pFechaDesde.ToString("s"),
                         pFechaHasta.ToString("s"),
                         pXid,
                         pPage.ToString()
                    };
                    return new PagaFacilMidasParameters
                    {
                        ServiceUrl = UriGenerator(pList),
                    };
                }
        }
      
        public class PagaFacilMidasResponse : BaseResponse
        {
            public int cod { get; set; }
            public string x_id { get; set; }
            public string msg { get; set; }
            public string moneda { get; set; }
            public string token { get; set; }
            public double balance { get; set; }
            public double total_pagar { get; set; }
            public int fact_pendientes { get; set; }
            public DateTime expira_en { get; set; }
            public IDictionary<string, Transac> transac { get; set; }
            public cliente Cliente { get; set; }
            public Factura[] Facturas { get; set; }
            public Proveedor[] proveedores { get; set; }
            public string aut_cod { get; set; }

                public class Transac
                {
                    public string aut_cod { get; set; }
                    public string x_id { get; set; }
                    public int estado { get; set; }
                    public int id { get; set; }
                    public double monto { get; set; }
                    public DateTime fecha { get; set; }
                }
                public class cliente
                {
                    public string nombre { get; set; }
                    public string apellidos { get; set; }
                    public string direccion { get; set; }
                    public string provincia { get; set; }
                    public string localidad { get; set; }
                }
                public class Factura
                {
                    public string fecha_fact { get; set; }
                    public string fecha_venc { get; set; }
                    public double? pago_min { get; set; }
                    public double? pago_total { get; set; }
                    public double? pago_reconex { get; set; }
                    public double? pago_atraso { get; set; }
                    public double? total { get; set; }
                    public string @ref {get; set; }
                }

                public class Proveedor
                {
                    public int id { get; set; }
                    public string nombre { get; set; }
                    public string servicio { get; set; }
                    public string prefijos { get; set; }
                    public string montos { get; set; }
                    public string prov_api { get; set; }
                    public string label_color { get; set; }
                }

                public static PagaFacilMidasResponse Exec_GetToken(string pUser, string pPass, int pTokenLive)
                {
                    var parameters = PagaFacilMidasParameters.Map_GetToken(pUser, pPass, pTokenLive);
                    return GenericMethods.CallServicePostAction<PagaFacilMidasResponse, PagaFacilMidasParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
                }
                public static PagaFacilMidasResponse Exec_GetProveedores(string pToken, string pCategoria)
                {
                    var parameters = PagaFacilMidasParameters.Map_GetProveedores(pToken, pCategoria);
                    return GenericMethods.CallServicePostAction<PagaFacilMidasResponse, PagaFacilMidasParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
                }
                public static PagaFacilMidasResponse Exec_GetBalance(string pToken)
                {
                    var parameters = PagaFacilMidasParameters.Map_GetBalance(pToken);
                    return GenericMethods.CallServicePostAction<PagaFacilMidasResponse, PagaFacilMidasParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
                }
                public static PagaFacilMidasResponse Exec_ConsultarFacturas(string pToken, string pNic, string pXid, string pProveedor, string pMoneda)
                {
                    var parameters = PagaFacilMidasParameters.Map_ConsultarFacturas(pToken, pNic, pXid,pProveedor,pMoneda);
                    return GenericMethods.CallServicePostAction<PagaFacilMidasResponse, PagaFacilMidasParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
                }
                public static PagaFacilMidasResponse Exec_PagarFacturas(string pToken, string pProveedor, string pXid, string pNic, string pMoneda, decimal pMonto)
                {
                    var parameters = PagaFacilMidasParameters.Map_PagarFactura(pToken, pProveedor, pXid, pNic, pMoneda, pMonto);
                    return GenericMethods.CallServicePostAction<PagaFacilMidasResponse, PagaFacilMidasParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
                }
                public static PagaFacilMidasResponse Exec_AnularPagoFactura(string pToken, string pProveedor, string pXid, string pNic)
                {
                    var parameters = PagaFacilMidasParameters.Map_AnularPagoFactura(pToken, pProveedor, pXid, pNic);
                    return GenericMethods.CallServicePostAction<PagaFacilMidasResponse, PagaFacilMidasParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
                }
                public static PagaFacilMidasResponse Exec_VerificarTransacciones(string pToken, string pProveedor, string pXid, DateTime pFechaDesde, DateTime pFechaHasta, int? pPage)
                {
                    var parameters = PagaFacilMidasParameters.Map_VerificarTransacciones(pToken, pProveedor, pXid, pFechaDesde, pFechaHasta, pPage);
                    return GenericMethods.CallServicePostAction<PagaFacilMidasResponse, PagaFacilMidasParameters>(null, parameters, ProductosExternosEnums.HttpMethod.GET);
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
