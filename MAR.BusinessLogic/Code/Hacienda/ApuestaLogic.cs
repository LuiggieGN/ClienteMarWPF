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
    public class ApuestaLogic
    {
        public static object Apuesta(string pTicketId)
        {
            //Get DTicket para llenar modelo ApuestaReuqestModel
            var dTicket = DataAccess.EFRepositories.Hacienda.ApuestaRepository.GetDTicket(pTicketId);

            if (dTicket == null)
            {
                return new { OK = true, Mensaje = "", Respuesta = "", Err = "" };
            }
            // inicializa transaccion ClienteHttp
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "Hacienda, Inicia Apuesta",
                Estado = "Solicitud",
                Monto = dTicket.MontoOperacion,
                Referencia = dTicket.NoTicket.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Apuesta,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                FechaRespuesta = null,
                BancaID = dTicket.TerminalID
            };

            //Agrega TransaccionClienteHttp
            var transaccionApuesta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", dTicket.BancaID,2);

            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionApuesta.TransaccionID.ToString());
            ApuestaRequestModel pApuestaModel = new ApuestaRequestModel();

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
            p.TerminalID = dTicket.TerminalID;
            p.LocalID = dTicket.LocalID;

            var jugadas = new List<Juego>();
            foreach (var item in dTicket.TicketDetalles)
            {
                jugadas.Add(new Juego { Codigo = item.Codigo, Jugada = ApuestaHelper.SeparaJugadaConGuion(item.Jugada), Monto = (int)item.Monto, TipoJugadaID = item.TipoJugadaID, });
            }
            p.DesgloseOperacion = new Jugada
            {
                Fecha = dTicket.Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                NoTicket = dTicket.NoTicket,
                Detalle = new DetalleJugada { Juego = jugadas }
            };
            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

            string codigoFueraLinea = "";
            try
            {
                //Consume servicio
                var apuesta = ClienteHTTP.CallService<ApuestaResponseModel, ApuestaRequestModel>(MetodosEnum.MetodoServicio.Apuesta, p, MetodosEnum.HttpMethod.POST, autenticacion, true);

                //NAUT Calculado

                if (apuesta == null || apuesta.CodigoRespuesta != "100")
                {
                    codigoFueraLinea = NAUTLogic.GeneraNautCalculado(p.EstablecimientoID, p.CodigoOperacion, cuenta.AutorizacionFueraDeLinea, 'T');
                }

                //Actualiza TransaccionClienteHttp
                if (apuesta != null)
                {
                    
                    if (apuesta.CodigoRespuesta == "100")
                    {
                        transaccion.Activo = true;
                        transaccion.Estado = "Apuesta Exitosa";
                    }
                    transaccion.Respuesta = apuesta.ToString();
                    transaccion.FechaRespuesta = DateTime.Parse(apuesta.FechaHoraRespuesta);
                    transaccion.Autorizacion = apuesta.NumeroAutentificacion;
                    transaccion.Comentario = apuesta.MensajeRespuesta;
                }
                else
                {
                    transaccion.Respuesta = "Fallo la Conexion al Servicio Hacienda";
                    transaccion.FechaRespuesta =DateTime.Now;
                }
              
                transaccion.Peticion = p.ApuestaAnonima().ToString();
                transaccion.TransaccionID = transaccionApuesta.TransaccionID;
                transaccion.NautCalculado = codigoFueraLinea;

                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
             
                if (apuesta != null && apuesta.CodigoRespuesta == "100")
                {
                    return new { OK = true, Mensaje = apuesta.MensajeRespuesta, Respuesta = apuesta.NumeroAutentificacion, Err = ""};
                }
                return new { OK = true, Mensaje = apuesta.MensajeRespuesta, Respuesta = codigoFueraLinea, Err = "" };
            }
            catch (Exception e)
            {
                return   new { OK = true, Mensaje = e.Message, Respuesta = codigoFueraLinea, Err = "" };  
            }

        }

    }
}
