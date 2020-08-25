using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.EFRepositories;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.ViewModels;
using System.Data.Entity;
using System.Reflection;

namespace MAR.BusinessLogic.Code
{
    public class PolizasSegurosIndexLogic
    {

        public static object GetMarcasVehiculos()
        {
            try
            {
                var db = new MARContext();
                var marcas = db.VP_MarcaVehiculo.Distinct();
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    Marcas = marcas
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.Message
                };
            }
        }

        public static object GetModelosVehiculos(int pMarcaId)
        {
            try
            {
                var db = new MARContext();
                var modelos = db.VP_ModeloVehiculo.Where(x => x.MarcaID == pMarcaId).Distinct();
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    Modelos = modelos
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.Message
                };
            }
        }


        public static object GetToken(int pRiferoId)
        {
            try
            {
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.Polizas);
                if (cuenta != null)
                {
                    var c = cuenta.VpCuenta.VP_CuentaConfig;
                    PolizasSegurosViewModel.Pass = c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("PASS")).ConfigValue;
                    PolizasSegurosViewModel.User = c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("USER")).ConfigValue;
                    PolizasSegurosViewModel.Url = c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("URL")).ConfigValue;
                    int tokenLive = int.Parse(c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("TOKEN")).ConfigValue);

                    var response = PolizasSegurosViewModel.PolizasSegurosResponse.Exec_GetToken(PolizasSegurosViewModel.User,
                        PolizasSegurosViewModel.Pass, tokenLive);
                    PolizasSegurosViewModel.TOKEN = response.token;
                }
                return new
                {
                    OK = true,
                    Err = string.Empty
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.Message
                };
            }
        }
     
        public static object GetPreciosYplanesDeSeguros(int pRiferoId)
        {
            try
            {
                GetToken(pRiferoId);
                var response = PolizasSegurosViewModel.PolizasSegurosResponse.Exec_GetPreciosYplanes(PolizasSegurosViewModel.TOKEN);

                if (ErrorRespuestas(response.cod))
                {
                    return new { OK = false, Err = response.msj };
                }
                var precios = response.precios.Values.ToArray();
                return new { OK = true, Err = string.Empty, PreciosYplanes = precios };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }

        private static object MyDictionaryToJson(IDictionary<string, PolizasSegurosViewModel.PolizasSegurosResponse.Precio> dict)
        {
            var entries = dict.Select(d =>
                 string.Format("\"{0}\": [{1}]", d.Key, string.Join(",", d.Value)));
            return "{" + string.Join(",", entries) + "}";
        }

        private static List<VP_TransaccionDetalle> MapObjectsToVpTransaccionDetalles(IEnumerable<object> pObjects, int pProductoId, IEnumerable<VP_ProductoCampo> productoCampos)
        {
            List<VP_TransaccionDetalle> transaccionDetalles = new List<VP_TransaccionDetalle>();

            foreach (var item in pObjects)
            {
                Type type = item.GetType();
                PropertyInfo[] objInfo = type.GetProperties();
                foreach (PropertyInfo c in objInfo)
                {
                    if (productoCampos.Any(x => x.Nombre.ToUpper() == c.Name.ToUpper() && x.Activo))
                    {
                        var t = new VP_TransaccionDetalle
                        {
                            ProductoCampoID = productoCampos.FirstOrDefault(x => x.Nombre.ToUpper() == c.Name.ToUpper()).ProductoCampoID,
                            Referencia = productoCampos.FirstOrDefault(x => x.Nombre.ToUpper() == c.Name.ToUpper()).Nombre,
                            ValorText = c.GetValue(item, null).ToString()
                        };
                        transaccionDetalles.Add(t);
                    }
                }
            }
            return transaccionDetalles;
        }

        public static object ComprarPoliza(string pDoc, PolizasSegurosViewModel.PolizasSegurosParameters.DatosCliente pDatosCliente, PolizasSegurosViewModel.PolizasSegurosParameters.DatosVehiculo pDatosVehiculo,
            string pVig, int pPrintWidth, int pRiferoId, int pBancaId, string pBancaNombre, string pDireccion, int pUsuarioId, decimal pPrecio)
        {
            try
            {
                //agregando transaccion de JuegaMas
                GetToken(pRiferoId);
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.Polizas);
                var productoCampos = ProductosRepository.GetVpProductoCampos(x => x.ProductoID == cuenta.ProductoId);
                var transaccionDetalles = MapObjectsToVpTransaccionDetalles(new object[] { pDatosCliente, pDatosVehiculo, new { Vigencia = pVig } }, cuenta.ProductoId, productoCampos);

                var trans = VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault(x => x.Referencia == DbEnums.VP_SuplidorReferencia.SegurosMidas.ToString()).SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                    pPrecio, 0, "", pUsuarioId, transaccionDetalles);

                var response = PolizasSegurosViewModel.PolizasSegurosResponse.Exec_ComprarPoliza(PolizasSegurosViewModel.TOKEN, trans.TransaccionID, pDoc, pDatosCliente, pDatosVehiculo, pVig);
                if (ErrorRespuestas(response.cod))
                {
                    return new { OK = false, Err = response.msj };
                }
                trans.Referencia = response.aut_cod;
                trans.Activo = true;
                trans.ReferenciaCliente = response.pol_no;
                trans.Estado = "Exitoso";
                VpTransacciones.ActualizaTransaccion(trans);

                var printData = PrintJobs.PolizasPrintJob.ImprimirPolizas(pPrintWidth, pBancaNombre, pDireccion,
                        response.aut_cod, trans.Ingresos, response.pol_no, trans.TransaccionID.ToString(),
                        BaseViewModel.GeneraPinGanador(trans.TransaccionID), "General de Seguros", false, pDatosVehiculo.chasis, pVig, pDatosCliente.cedula);
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    PreciosYplanes = response,
                    PrintData = printData
                };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }
        public static object ConsultarPoliza(string pDoc)
        {
            try
            {
                var response = PolizasSegurosViewModel.PolizasSegurosResponse.Exec_ConsultarPoliza(PolizasSegurosViewModel.TOKEN, pDoc);
                if (ErrorRespuestas(response.cod))
                {
                    return new { OK = false, Err = response.msj };
                }
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    Polizas = response
                };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }

        public static object GetTipoVehiculos()
        {
            try
            {
                var response = PolizasSegurosViewModel.PolizasSegurosResponse.Exec_GetTipoVehiculos(PolizasSegurosViewModel.TOKEN);
                if (ErrorRespuestas(response.cod))
                {
                    return new { OK = false, Err = response.msj };
                }
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    TiposVehiculos = response.tipos
                };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }

        public static object BuscarTransaccionesMar(DateTime pFechaDesde, DateTime pFechaHasta, int pRiferoId, int pBancaId, string pProveedor = null)
        {
            try
            {
                var cuenta = DataAccess.EFRepositories.VpCuentaRepository.GetVpCuenta(pRiferoId,
                    DbEnums.Productos.Polizas);
                var transacciones = VpTransacciones.GetTransacciones(x => x.FechaIngreso >= pFechaDesde.Date && x.FechaIngreso <= pFechaHasta.Date && x.CuentaID == cuenta.VpCuenta.CuentaID && x.BancaID == pBancaId && x.ProductoID == cuenta.ProductoId, null, "VP_Suplidor, VP_TransaccionDetalle");
                var polizas = PolizasSegurosViewModel.SetPolizasFromTransacciones(transacciones).OrderByDescending(x => x.TransaccionID);
                return new { OK = true, Err = string.Empty, Transacciones = polizas };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }




        public static object ReImprimir_Seguro(Models.RequestModel.SegurosRequestModel.Poliza pPoliza, int pPrintWidth, string pBancaNombre, string pDireccion, int pUsuarioId)
        {
            try
            {
                var printData = PrintJobs.PolizasPrintJob.ImprimirPolizas(pPrintWidth, pBancaNombre, pDireccion,
                pPoliza.CodigoAutorizacion, pPoliza.Precio, pPoliza.PolizaNumero, pPoliza.TransaccionID.ToString(),
                BaseViewModel.GeneraPinGanador(pPoliza.TransaccionID), "General de Seguros S.A.", true, pPoliza.Vehiculo.Chasis, pPoliza.Vigencia, pPoliza.Cliente.Cedula);
                return new
                {
                    OK = true,
                    PrintData = printData
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = "Ocurrio un error, intente mas tarde."
                };
            }

        }

        public static object AnularSeguro(int pTransaccionId, string pPin, int pRiferoId)
        {
            try
            {
                GetToken(pRiferoId);
                if (!BaseViewModel.ComparaPinGanador(pTransaccionId, pPin))
                {
                    return new { OK = false, Err = "Pin incorrecto", Mensaje = "Pin incorrecto" };
                }

                var response = PolizasSegurosViewModel.PolizasSegurosResponse.Exec_AnularSeguro(PolizasSegurosViewModel.TOKEN, pTransaccionId.ToString());
                if (ErrorRespuestas(response.cod))
                {
                    return new { OK = false, Err = response.msj };
                }

                var trans = VpTransacciones.BuscarTransaccionPorId(pTransaccionId);
                trans.Estado = response.msj;
                trans.Activo = false;
                VpTransacciones.ActualizaTransaccion(trans);
                return new { OK = true, Err = string.Empty, Mensaje = response.msj };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.Message
                };
            }
        }

        private static bool ErrorRespuestas(int pCod)
        {
            if (pCod == (int)ProductosExternosEnums.PolizasMidasRedStatus.SolicitudCompletadaCorrectamente)
            {
                return false;
            }
            return Enum.GetValues(typeof(ProductosExternosEnums.PolizasMidasRedStatus)).Cast<ProductosExternosEnums.PolizasMidasRedStatus>().Any(item => pCod == (int)item);
        }
    }
}
