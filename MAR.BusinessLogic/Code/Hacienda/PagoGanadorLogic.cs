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
    public class PagoGanadorLogic
    {
        public static string PagoGanador(string pTicketId)
        {
            //Get DTicket para llenar modelo PagoRequestModel
            var dTicket = DataAccess.EFRepositories.Hacienda.ApuestaRepository.GetDTicket(pTicketId);

            // inicializa transaccion ClienteHttp
            if (dTicket == null)
            {
                return "";
            }
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "Hacienda, Inicia PagoGanador",
                Estado = "Solicitud",
                Monto = dTicket.TicketDetalles.Sum(x =>x.Saco),
                Referencia = dTicket.NoTicket.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.PagoGanador,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                FechaRespuesta = null,
                BancaID = dTicket.TerminalID
            };

            //Agrega TransaccionClienteHttp
            var transaccionPago = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", 0,2);

            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionPago.TransaccionID.ToString());
            PagoGanadorRequestModel pagoGanadorRequestModel = new PagoGanadorRequestModel();

            var p = pagoGanadorRequestModel;
            p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
            p.Usuario = cuenta.PMCuenta.RecargaID;
            p.Password = cuenta.PMCuenta.CueServidor;
            p.DiaOperacion = baseParams.DiaOperacion;
            p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
            p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
            p.CodigoOperacion = baseParams.CodigoOperacion;
            p.MontoOperacion = dTicket.TicketDetalles.Sum(x => x.Saco);
            p.TipoOperacion = MetodosEnum.MetodoServicio.PagoGanador.ToString();
            p.TerminalID = dTicket.TerminalID;
            p.NumeroAutenticacionReferencia = dTicket.AutenticacionReferencia;
            p.CodigoOperacionReferencia = dTicket.CodigoOperacionReferencia;
            p.LocalID = dTicket.LocalID;
            p.Jugador = null;
            p.NumeroAutenticacionReferencia = dTicket.AutenticacionReferencia;
            p.CodigoOperacionReferencia = dTicket.CodigoOperacionReferencia;
           
            var jugadas = new List<JuegoPago>();
            foreach (var item in dTicket.TicketDetalles)
            {
                jugadas.Add(new JuegoPago { Codigo = item.Codigo, Jugada = ApuestaHelper.SeparaJugadaConGuion(item.Jugada), MontoPagado = item.Saco, MontoApostado = item.Monto, TipoJugadaID = item.TipoJugadaID, });
            }
            p.DesglosePago = new  DesglosePago {
                 Fecha = dTicket.Fecha.ToString("yyyy-MM-dd HH:mm:ss"), NoTicket = dTicket.NoTicket,
                  Detalle = new  DetalleJugadasPago {  Juego = jugadas }
            };
            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));


            //Consume servicio
            var pago = ClienteHTTP.CallService<PagoGanadoresResponseModel, PagoGanadorRequestModel>(MetodosEnum.MetodoServicio.PagoGanador, p, MetodosEnum.HttpMethod.POST, 
                autenticacion, true);

            //Actualiza TransaccionClienteHttp
            if (pago.CodigoRespuesta == "100")
            {
                transaccion.Activo = true;
                transaccion.Estado = "Pago Exitoso";
            }
            transaccion.Respuesta = pago.MensajeRespuesta;
            transaccion.FechaRespuesta = DateTime.Parse(pago.FechaHoraRespuesta);
            transaccion.Autorizacion = pago.NumeroAutentificacion;
         

            transaccion.Comentario = pago.MensajeRespuesta;
            transaccion.Peticion = p.ToString();
            transaccion.TransaccionID = transaccionPago.TransaccionID;
            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);

       
            if (pago.CodigoRespuesta == "100")
            {
                return pago.NumeroAutentificacion;
            }
            return "";
        }
        
    }
}
