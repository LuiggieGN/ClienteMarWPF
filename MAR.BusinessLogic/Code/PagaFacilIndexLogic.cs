using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.UI.WebControls;
using MAR.DataAccess.EFRepositories;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.ViewModels;

namespace MAR.BusinessLogic.Code
{
    public class PagaFacilIndexLogic
    {

        public static object GetToken(int pRiferoId)
        {
            try
            {
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.PagaFacil);
                if (cuenta != null)
                {
                    var c = cuenta.VpCuenta.VP_CuentaConfig;
                    PagaFacilViewModel.Pass = c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("PASS")).ConfigValue;
                    PagaFacilViewModel.User = c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("USER")).ConfigValue;
                    PagaFacilViewModel.Url = c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("URL")).ConfigValue;
                    int tokenLive = int.Parse(c.FirstOrDefault(x => x.ConfigKey.ToUpper().Contains("TOKEN")).ConfigValue);

                    var response = PagaFacilViewModel.PagaFacilMidasResponse.Exec_GetToken(PagaFacilViewModel.User,
                        PagaFacilViewModel.Pass, tokenLive);
                    PagaFacilViewModel.TOKEN = response.token;
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

        public static object GetProveedores()
        {
            try
            {
                var response = PagaFacilViewModel.PagaFacilMidasResponse.Exec_GetProveedores(PagaFacilViewModel.TOKEN, DbEnums.Productos.PagaFacil.ToString().ToLower());
                if (ErrorRespuestas(response.cod) == true)
                {
                    return new { OK = false, Err = response.msg };
                }
                var proveedores = response.proveedores.OrderBy(x => x.nombre);
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    Proveedores = proveedores
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

        public static object GetBalance(int pRiferoId)
        {
            try
            {
                GetToken(pRiferoId);
                var response = PagaFacilViewModel.PagaFacilMidasResponse.Exec_GetBalance(PagaFacilViewModel.TOKEN);
                if (ErrorRespuestas(response.cod) == true)
                {
                    return new { OK = false, Err = response.msg };
                }
                return response.balance;
            }
            catch (Exception e)
            {
                return "El suplidor de pagos de servicios no esta disponible en estos momentos.";
            }
        }

        public static object GetProveedoresMar(int pRiferoId)
        {
            try
            {
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.PagaFacil);
                if (cuenta != null)
                {
                    int cuentaId = cuenta.VpCuenta.CuentaID;
                    var suplidores = VpSuplidores.GetSuplidores(x => x.Activo && x.CuentaID == cuentaId, q => q.OrderBy(s => s.Nombre));
                    return new { OK = true, Err = string.Empty, Proveedores = suplidores };
                }
                return new { OK = false, Err = "No existe cuenta configurada. Contacte un administrador", Proveedores = "" };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }

        public static object ConsultarFacturas(string pNic, string pProveedor)
        {
            try
            {
                const string xid = "10000";
                var response = PagaFacilViewModel.PagaFacilMidasResponse.Exec_ConsultarFacturas(PagaFacilViewModel.TOKEN, pNic, xid, pProveedor, "DOP");

                if (ErrorRespuestas(response.cod) == true)
                {
                    return new { OK = false, Err = response.msg };
                }
                return new { OK = true, Err = string.Empty, Consulta = response };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }

        public static object BuscarTransacciones(string pProveedor, DateTime pFechaDesde, DateTime pFechaHasta, int? pPage)
        {
            try
            {
                const string xid = "12345679";
                var response = PagaFacilViewModel.PagaFacilMidasResponse.Exec_VerificarTransacciones(PagaFacilViewModel.TOKEN, "0", xid
                    , pFechaDesde, pFechaHasta, pPage);

                if (ErrorRespuestas(response.cod))
                {
                    return new { OK = false, Err = response.msg };
                }
                return new { OK = true, Err = string.Empty, Transacciones = response.transac };
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
                    DbEnums.Productos.PagaFacil);
                var transacciones = VpTransacciones.GetTransacciones(x => x.FechaIngreso >= pFechaDesde.Date && x.FechaIngreso <= pFechaHasta.Date && x.CuentaID == cuenta.VpCuenta.CuentaID && x.BancaID == pBancaId && x.ProductoID == cuenta.ProductoId, null, "VP_Suplidor");
                var facturas = PagaFacilViewModel.SetFacturasFromTransacciones(transacciones);
                return new { OK = true, Err = string.Empty, Transacciones = facturas };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }

        public static object ReimprimeFactura(int pPrintWidth,
            string pBancaNombre, string pDireccion, PagaFacilViewModel.Factura pFactura)
        {
            try
            {
                return new
                {
                    OK = true,
                    PrintData = PrintJobs.PagaFacilPrintJob.ImprimirPagoServicio(pPrintWidth, pBancaNombre, pDireccion, pFactura.Referencia, pFactura.Ingresos, pFactura.Nic, pFactura.TransaccionID.ToString(),
                                                              BaseViewModel.GeneraPinGanador(pFactura.TransaccionID), pFactura.Suplidor.Nombre, true)
                };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }

        public static object PagarFactura(string pNic, string pProveedor, decimal pMonto, int pBancaId, int pPrintWidth, string pBancaNombre, int pUsuarioId, string pDireccion, string pProveedorNombre, int pRiferoId)
        {
            var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.PagaFacil);
            var trans = VpTransacciones.AgregarTransaccion(pBancaId, int.Parse(pProveedor.Split('-')[1]), cuenta.ProductoId, cuenta.VpCuenta.CuentaID, pMonto, 0, pNic, pUsuarioId, null);
            try
            {
                var response = PagaFacilViewModel.PagaFacilMidasResponse.Exec_PagarFacturas(PagaFacilViewModel.TOKEN, pProveedor.Split('-')[0], trans.TransaccionID.ToString(), pNic, "DOP", pMonto);
                trans.Estado = response.msg;
                if (ErrorRespuestas(response.cod))
                {
                    VpTransacciones.ActualizaTransaccion(trans);
                    return new { OK = false, Err = response.msg };
                }
                trans.Referencia = response.aut_cod;
                trans.Activo = true;
                VpTransacciones.ActualizaTransaccion(trans);
                return new { OK = true, Err = string.Empty, Mensaje = response.msg, PrintData = PrintJobs.PagaFacilPrintJob.ImprimirPagoServicio(pPrintWidth, pBancaNombre, pDireccion, response.aut_cod, pMonto, pNic, trans.TransaccionID.ToString(), BaseViewModel.GeneraPinGanador(trans.TransaccionID), pProveedorNombre, false) };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message };
            }
        }
        public static object AnularPagoFactura(string pProveedor, string pNic, int pTransaccionId, string pPin)
        {
            try
            {
                if (!BaseViewModel.ComparaPinGanador(pTransaccionId, pPin))
                {
                    return new { OK = false, Err = "Pin incorrecto", Mensaje = "Pin incorrecto" };
                }

                var response = PagaFacilViewModel.PagaFacilMidasResponse.Exec_AnularPagoFactura(PagaFacilViewModel.TOKEN, pProveedor, pTransaccionId.ToString(), pNic);
                if (ErrorRespuestas(response.cod))
                {
                    return new { OK = false, Err = response.msg };
                }

                var trans = VpTransacciones.BuscarTransaccionPorId(pTransaccionId);
                trans.Estado = response.msg;
                trans.Activo = false;
                VpTransacciones.ActualizaTransaccion(trans);
                return new { OK = true, Err = string.Empty, Mensaje = response.msg };
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
            if (pCod == (int)ProductosExternosEnums.PagaFacilMidasRedStatus.SolicitudCompletadaCorrectamente)
            {
                return false;
            }
            return Enum.GetValues(typeof(ProductosExternosEnums.PagaFacilMidasRedStatus)).Cast<ProductosExternosEnums.PagaFacilMidasRedStatus>().Any(item => pCod == (int)item);
        }
    }
}
