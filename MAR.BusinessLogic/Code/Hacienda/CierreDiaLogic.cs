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
    public class CierreDiaLogic
    {
       
        public static void  CierreDia(DateTime? pFecha)
        {
            //Get DTicket para llenar modelo PagoRequestModel

            bool sorteosAbiertos = DataAccess.EFRepositories.Hacienda.CierreDiaRepository.SorteosAbiertos(pFecha);
            if (sorteosAbiertos)
            {
                return;
            }
            var cierreData = DataAccess.EFRepositories.Hacienda.CierreDiaRepository.GetCierreDiaRequestValues(pFecha);
            var cierreDiaModel = new CierreDiaRequestModel();
            cierreDiaModel.CantidadTotalOperacionesExitosas = cierreData.Sum(x => x.CantidadOperaciones);
            cierreDiaModel.MontoTotalOperaciones = cierreData.Sum(x => x.MontoOperaciones);
            cierreDiaModel.DesgloseTotalesCierreDia = new List<DegloseCierreDia>();
            foreach (var item in cierreData)
            {
                cierreDiaModel.DesgloseTotalesCierreDia.Add(new DegloseCierreDia { MontoOperaciones = item.MontoOperaciones, CantidadOperaciones = item.CantidadOperaciones, Tipo = item.Tipo });
            }
 


            

            // inicializa transaccion ClienteHttp
            if (!cierreDiaModel.DesgloseTotalesCierreDia.Any())
            {
                return;
            }
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "Hacienda, Inicia CierreDia",
                Estado = "Solicitud",
                Monto = cierreDiaModel.DesgloseTotalesCierreDia.Sum(x => x.MontoOperaciones),
                Referencia = pFecha.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.CierreDia,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                FechaRespuesta = null,
                BancaID = 0
            };


            string t = "";
            //Agrega TransaccionClienteHttp
            var trasaccionCierreVenta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

           //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", 0,2);

            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, trasaccionCierreVenta.TransaccionID.ToString());
            CierreSorteoRequestModel cierreSorteoRequestModel = new CierreSorteoRequestModel();

            var p = cierreDiaModel;
            p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
            p.Usuario = cuenta.PMCuenta.RecargaID;
            p.Password = cuenta.PMCuenta.CueServidor;
            p.DiaOperacion = DateTime.Parse(pFecha.ToString()).ToString("yyyy-MM-dd");
            p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
            p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
            p.CodigoOperacion = baseParams.CodigoOperacion;
            p.MontoTotalOperaciones = cierreDiaModel.DesgloseTotalesCierreDia.Sum(x => x.MontoOperaciones);
            p.TipoOperacion = MetodosEnum.MetodoServicio.CierreSorteo.ToString();
            p.CantidadTotalOperacionesExitosas = cierreDiaModel.DesgloseTotalesCierreDia.Sum(x => x.CantidadOperaciones);
            p.DesgloseTotalesCierreDia = cierreDiaModel.DesgloseTotalesCierreDia;

            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

            //Consume servicio
            var cierre = ClienteHTTP.CallService<CierreDiaResponseModel, CierreDiaRequestModel>(MetodosEnum.MetodoServicio.CierreDia, p, MetodosEnum.HttpMethod.POST, 
                autenticacion, true);
            if (autenticacion != null)
            {
                //Actualiza TransaccionClienteHttp
                if (cierre.CodigoRespuesta == "100" || cierre.CodigoRespuesta == "906")
                {
                    transaccion.Activo = true;
                    transaccion.Estado = "CierreDia Exitoso";
                }

                transaccion.Respuesta = cierre.ToString();
                transaccion.FechaRespuesta = DateTime.Parse(cierre.FechaHoraRespuesta);
                transaccion.Autorizacion = cierre.NumeroAutentificacion;

                transaccion.Comentario = cierre.MensajeRespuesta;
                transaccion.Peticion = p.ToString();
                transaccion.TransaccionID = trasaccionCierreVenta.TransaccionID;
                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
            }
            else
            {
                transaccion.Estado = "Fallo la conexion al servicio";
                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
            }

        }
    }
}
