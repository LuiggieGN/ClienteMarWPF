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
    public class CierreSorteoLogic
    {
       
        public static void  CierreSorteo(int pSorteoDiaId)
        {
            //Get DTicket para llenar modelo PagoRequestModel
            var cierreData = DataAccess.EFRepositories.Hacienda.CierreSorteoRepository.GetCierreSorteoRequestValues(pSorteoDiaId);
            var desgloseList = new List<Juego>();
            foreach (var item in cierreData)
            {
                var desglose = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON<List<Juego>>(item.Peticion);
                desgloseList.AddRange(desglose);
            }

            var desgloseClean = desgloseList.Distinct().GroupBy(x => x.Jugada).Select(x => x.FirstOrDefault()).ToList();

            var cierreRequest = new MarConnectCliente.RequestModels.CierreSorteoRequestModel();
            cierreRequest.DesglosesTotales = new List<DesgloseCierreSorteo>();
            foreach (var item in desgloseClean)
            {
                var jugadas = from d in desgloseList where d.Jugada == item.Jugada && d.Codigo == cierreData.FirstOrDefault().IdentificadorJuegoDeAzar select d;
                cierreRequest.DesglosesTotales.Add(new DesgloseCierreSorteo { Jugada = item.Jugada, CantidadApuestas = jugadas.Count(), Monto = jugadas.Sum(x => x.Monto) });
            }

            // inicializa transaccion ClienteHttp
            if (!cierreRequest.DesglosesTotales.Any())
            {
                return;
            }
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "Hacienda, Inicia CierreSorteo",
                Estado = "Solicitud",
                Monto = cierreRequest.DesglosesTotales.Sum(x => x.Monto),
                Referencia = pSorteoDiaId.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.CierreSorteo,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                BancaID = 0
            };

            //Agrega TransaccionClienteHttp
            var trasaccionCierreVenta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", 0,2);

            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, trasaccionCierreVenta.TransaccionID.ToString());
            CierreSorteoRequestModel cierreSorteoRequestModel = new CierreSorteoRequestModel();

            var p = cierreSorteoRequestModel;
            p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
            p.Usuario = cuenta.PMCuenta.RecargaID;
            p.Password = cuenta.PMCuenta.CueServidor;
            p.DiaOperacion = baseParams.DiaOperacion;
            p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
            p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
            p.CodigoOperacion = baseParams.CodigoOperacion;
            p.MontoTotalApuestas = cierreRequest.DesglosesTotales.Sum(x => x.Monto);
            p.TipoOperacion = MetodosEnum.MetodoServicio.CierreSorteo.ToString();
            p.IdentificadorJuegoDeAzar = cierreData.FirstOrDefault().IdentificadorJuegoDeAzar;
            p.FechaCierre =  cierreData.FirstOrDefault().FechaCierre.ToString("yyyy-MM-dd");
            p.CantidadTotalOperacionesExitosas = cierreRequest.DesglosesTotales.Sum(x => x.CantidadApuestas);
            p.DesglosesTotales = cierreRequest.DesglosesTotales;
         
            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

            //Consume servicio
            var cierre = ClienteHTTP.CallService<CierreSorteoResponseModel, CierreSorteoRequestModel>(MetodosEnum.MetodoServicio.CierreSorteo, p, 
                MetodosEnum.HttpMethod.POST, autenticacion, true);

            //Actualiza TransaccionClienteHttp
            if (cierre.CodigoRespuesta == "100" || cierre.CodigoRespuesta == "905" )
            {
                transaccion.Activo = true;
                transaccion.Estado = "CierreSorteo Exitoso";
            }

            transaccion.Respuesta = cierre.ToString();
            transaccion.FechaRespuesta = DateTime.Parse(cierre.FechaHoraRespuesta);
            transaccion.Autorizacion = cierre.NumeroAutentificacion;

            transaccion.Comentario = cierre.MensajeRespuesta;
            transaccion.Peticion = p.ToString();
            transaccion.TransaccionID = trasaccionCierreVenta.TransaccionID;
            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
         
        }
    }
}
