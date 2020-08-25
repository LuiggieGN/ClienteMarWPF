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
    public class ApuestaFueraDeLinea
    {

        public static void Apuesta()
        {
            //Get DTicket para llenar modelo ApuestaReuqestModel
            var dTickets= DataAccess.EFRepositories.Hacienda.ApuestaRepository.GetDTicketsFueraDeLinea();
          
            foreach (var dTicket in dTickets)
            {
                try
                {
                    // inicializa transaccion ClienteHttp
                    var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
                    {
                        Activo = false,
                        Autorizacion = "",
                        Comentario = "Hacienda, Inicia Apuesta Fuera De Linea",
                        Estado = "Solicitud",
                        Monto = dTicket.MontoOperacion,
                        Referencia = dTicket.NoTicket.ToString(),
                        TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Apuesta,
                        Fecha = DateTime.Now,
                        FechaSolicitud = DateTime.Now,
                        TipoAutorizacion = 1,
                        NautCalculado = dTicket.NautCalculado,
                        BancaID = dTicket.TerminalID
                    };

                    //Busca cuenta y producto
                    var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", 0,2);

                    //Crea base parameters
                    var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, dTicket.CodigoOperacionReferencia);
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
                    p.NumeroAutentificacionCalculado = dTicket.NautCalculado;
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

                    try
                    {
                        //Consume servicio
                        var apuesta = ClienteHTTP.CallService<ApuestaResponseModel, ApuestaRequestModel>(MetodosEnum.MetodoServicio.Apuesta, p, MetodosEnum.HttpMethod.POST, 
                            autenticacion, true);

                        //NAUT Calculado

                       
                        //Actualiza TransaccionClienteHttp
                        if (apuesta != null)
                        {
                            if (apuesta.CodigoRespuesta == "100")
                            {
                                //Agrega TransaccionClienteHttp
                                transaccion.Activo = true;
                                transaccion.Estado = "Apuesta Exitosa";
                                transaccion.Respuesta = apuesta.ToString();
                                transaccion.FechaRespuesta = DateTime.Parse(apuesta.FechaHoraRespuesta);
                                transaccion.Autorizacion = apuesta.NumeroAutentificacion;
                                transaccion.Comentario = apuesta.MensajeRespuesta;
                                transaccion.Peticion = p.ApuestaAnonima().ToString();
                                transaccion.NautCalculado = "";
                                var transaccionApuesta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

                                //actualiza Transaccion original
                                transaccion.TransaccionID = int.Parse(dTicket.CodigoOperacionReferencia);
                                transaccion.Estado = "Reenviada";
                                transaccion.Comentario = dTicket.CodigoOperacionReferencia;
                                transaccion.Autorizacion = apuesta.NumeroAutentificacion;
                                var actualizaTransaccion = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccionFueraLinea(transaccion);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        string error = e.Message; 
                    }
                }
                catch (Exception e)
                {
                    string error = e.Message;
                }
            }
    

        }

    }
}
