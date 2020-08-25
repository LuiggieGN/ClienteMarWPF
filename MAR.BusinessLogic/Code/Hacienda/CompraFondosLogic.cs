using MAR.BusinessLogic.Code.Hacienda.SharedOperations;
using MarConnectCliente;
using MarConnectCliente.BusinessLogic.ShareFuntions;
using MarConnectCliente.Enums;
using MarConnectCliente.Helpers;
using MarConnectCliente.RequestModels;
using MarConnectCliente.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.Hacienda
{
    public class CompraFondosLogic
    {

        public static object CompraFondo(int pBancaID, string pDoc, string pTipoDoc, decimal pMonto)
        {
            //Get jugador para llenar modelo
            // var jugador = **Busco el jugador aqui** DataAccess.EFRepositories.Hacienda.CierreDiaRepository.GetCierreDiaRequestValues(DateTime.Now);

            // inicializa transaccion ClienteHttp

            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "Hacienda, Inicia CompraFondo",
                Estado = "Solicitud",
                Monto = pMonto,
                Referencia = pDoc,
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.CompraFondo,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                FechaRespuesta = null,
                NautCalculado = "",
                BancaID = pBancaID
            };


            //Agrega TransaccionClienteHttp
            var trasaccionCierreVenta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", pBancaID,2);

            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, trasaccionCierreVenta.TransaccionID.ToString());
            CierreSorteoRequestModel cierreSorteoRequestModel = new CierreSorteoRequestModel();

            var compraFondoModel = new CompraFondosRequestModel();
            var p = compraFondoModel;
            p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
            p.Usuario = cuenta.PMCuenta.RecargaID;
            p.Password = cuenta.PMCuenta.CueServidor;
            p.DiaOperacion = baseParams.DiaOperacion;
            p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
            p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
            p.CodigoOperacion = baseParams.CodigoOperacion;
            p.TerminalID = cuenta.Terminal.TerminalId;
            p.LocalID = cuenta.Terminal.LocalId;
            p.MontoOperacion = pMonto;
            p.TipoOperacion = MetodosEnum.MetodoServicio.CompraFondo.ToString();
            p.Jugador = new MarConnectCliente.IndividualModels.Jugador { NumeroDocumento = pDoc, TipoDocumento = pTipoDoc };

            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

            //Consume servicio
            var fondo = ClienteHTTP.CallService<CompraFondosResponseModel, CompraFondosRequestModel>(MetodosEnum.MetodoServicio.CompraFondo, p, MetodosEnum.HttpMethod.POST,
                autenticacion, true);
            if (fondo.NumeroAutentificacion != null)
            {
                //Actualiza TransaccionClienteHttp
                if (fondo.CodigoRespuesta == "100")
                {
                    transaccion.Activo = true;
                    transaccion.Estado = "CompraFondo Exitoso";
                }
                transaccion.Respuesta = fondo.ToString();
                transaccion.FechaRespuesta = DateTime.Parse(fondo.FechaHoraRespuesta);
                transaccion.Autorizacion = fondo.NumeroAutentificacion;

                transaccion.Comentario = fondo.MensajeRespuesta;
                transaccion.Peticion = p.ToString();
                transaccion.TransaccionID = trasaccionCierreVenta.TransaccionID;
                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                if (fondo.CodigoRespuesta == "100")
                {
                    return new { OK = true, Mensaje = fondo.MensajeRespuesta + " Balance Actual: " + fondo.BalanceActual,  CompraFondo = fondo};
                }
                else
                {
                    return new { OK = true, Mensaje = fondo.MensajeRespuesta , CompraFondo = fondo };
                }
            }
            else
            {
                transaccion.Estado = "Fallo la conexion al servicio";
                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                return new { OK = false, Mensaje = transaccion.Estado };
            }
          


        }
    }
}
