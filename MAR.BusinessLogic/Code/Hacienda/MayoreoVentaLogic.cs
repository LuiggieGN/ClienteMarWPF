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
    public class MayoreoVentaLogic
    {
        public static string ApuestaMayoreo(string pTicketId)
        {
            //Get DTicket para llenar modelo ApuestaReuqestModel
            var dTicket = DataAccess.EFRepositories.Hacienda.ApuestaRepository.GetDTicket(pTicketId);

            // inicializa transaccion ClienteHttp
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "Hacienda, Inicia Venta Mayoreo",
                Estado = "Solicitud",
                Monto = dTicket.MontoOperacion,
                Referencia = dTicket.NoTicket.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.VentaMayoreo,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                BancaID = dTicket.TerminalID
            };

            //Agrega TransaccionClienteHttp
            var transaccionApuesta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", 0,2);

            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionApuesta.TransaccionID.ToString());
            VentaCompraMayoreoRequestModel pApuestaModel = new VentaCompraMayoreoRequestModel();

            var p = pApuestaModel;
            p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
            p.Usuario = cuenta.PMCuenta.RecargaID;
            p.Password = cuenta.PMCuenta.CueServidor;
            p.DiaOperacion = baseParams.DiaOperacion;
            p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
            p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
            p.CodigoOperacion = baseParams.CodigoOperacion;
            p.MontoOperacion = dTicket.MontoOperacion;
            p.TipoOperacion = MetodosEnum.MetodoServicio.Apuesta.ToString();
            //p.EstablecimientoReferencia = pEstablecimientoReferencia;

            var jugadas = new List<Juego>();
            foreach (var item in dTicket.TicketDetalles)
            {
                jugadas.Add(new Juego { Codigo = item.Codigo, Jugada = ApuestaHelper.SeparaJugadaConGuion(item.Jugada), Monto = (int)item.Monto, TipoJugadaID = item.TipoJugadaID, });
            }
            p.DesgloseOperacion = new Jugada {
                 Fecha = dTicket.Fecha.ToString("yyyy-MM-dd HH:mm:ss"), NoTicket = dTicket.NoTicket,
                  Detalle = new DetalleJugada {  Juego = jugadas }
            };
            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));


            //Consume servicio
            var apuesta = ClienteHTTP.CallService<VentaCompraMayoreoResponseModel, VentaCompraMayoreoRequestModel>(MetodosEnum.MetodoServicio.Apuesta, p, MetodosEnum.HttpMethod.POST, autenticacion);

            //Actualiza TransaccionClienteHttp
            if (apuesta.CodigoRespuesta == "100")
            {
                transaccion.Activo = true;
                transaccion.Estado = "Venta Mayoreo Exitosa";
            }
            transaccion.Respuesta = apuesta.ToString();
            transaccion.FechaRespuesta = DateTime.Parse(apuesta.FechaHoraRespuesta);
            transaccion.Autorizacion = apuesta.NumeroAutentificacion;
         

            transaccion.Comentario = apuesta.MensajeRespuesta;
            transaccion.Peticion = p.VentaAnonimaWithNAUTC().ToString();
            transaccion.TransaccionID = transaccionApuesta.TransaccionID;
            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);

            ////Agrega transaccionRefencia 
            ////*********************revisa esto para los parametros. el detallee***
            //DataAccess.EFRepositories.Hacienda.TransaccionReferenciaRepository.AgregaTransaccion(transaccion.TransaccionID, MetodosEnum.MetodoServicio.Apuesta, apuesta.ToString());
            if (apuesta.CodigoRespuesta == "100")
            {
                return apuesta.NumeroAutentificacion;
            }
            return "";
        }
        
    }
}
