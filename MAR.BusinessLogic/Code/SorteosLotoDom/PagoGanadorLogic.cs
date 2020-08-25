using MAR.BusinessLogic.Code.Hacienda.SharedOperations;
using MarConnectCliente;
using MarConnectCliente.BusinessLogic.ShareFuntions;
using MarConnectCliente.Enums;
using MarConnectCliente.Helpers;
using MarConnectCliente.RequestModels;
using MarConnectCliente.RequestModels.LotoDom;
using MarConnectCliente.ResponseModels;
using MarConnectCliente.ResponseModels.LotoDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.SorteosLotoDom
{
    public class PagoGanadorLogic
    {
        public static string PagoGanador(string pTicketId)
        {
            //Get DTicket para llenar modelo PagoRequestModel
            var dTicket = DataAccess.EFRepositories.LotoDom.ApuestaRepository.GetDTicket(pTicketId);

            // inicializa transaccion ClienteHttp
            if (dTicket == null)
            {
                return "";
            }
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "LotoDom, Inicia PagoGanador",
                Estado = "Solicitud",
                Monto = dTicket.TicketDetalles.Sum(x =>x.Saco),
                Referencia = dTicket.NoTicket.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.PagoGanador,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                FechaRespuesta = null,
                BancaID = dTicket.BancaID
            };

            //Agrega TransaccionClienteHttp
            var transaccionPago = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "LotoDom", dTicket.BancaID,0);

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
            p.TerminalID = dTicket.BancaID;
            p.NumeroAutenticacionReferencia = dTicket.AutenticacionReferencia;
            p.CodigoOperacionReferencia = dTicket.CodigoOperacionReferencia;
         
            p.Jugador = null;
            p.NumeroAutenticacionReferencia = dTicket.AutenticacionReferencia;
            p.CodigoOperacionReferencia = dTicket.CodigoOperacionReferencia;
           
            var jugadas = new List<JuegoPago>();
        
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


        public static ConsultaGanadorLotoDomResponseModel ConsultaTicketGanador(int pTicketId, int pBancaID)
        {
            try
            {
                //Get DTicket para llenar modelo ApuestaReuqestModel
             
                if (pTicketId != null)
                {
                    // inicializa transaccion ClienteHttp
                    var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
                    {
                        Activo = false,
                        Autorizacion = "",
                        Comentario = "LotoDom, Inicia ConsultaGanador",
                        Estado = "Solicitud",
                        Referencia = pTicketId.ToString(),
                        Monto = 0,
                        TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.ConsultaPagoGanador,
                        Fecha = DateTime.Now,
                        FechaSolicitud = DateTime.Now,
                        TipoAutorizacion = 1,
                        NautCalculado = "",
                        FechaRespuesta = null,
                        BancaID = pBancaID
                    };

                    //Agrega TransaccionClienteHttp
                    var transaccionConsultaGanador = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

                    //Busca cuenta y producto
                    var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "LotoDom", pBancaID, 0);


                    //Crea base parameters
                    var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionConsultaGanador.TransaccionID.ToString());
                    ConsultaGanadorLotoDomRequestModel pAnulacionModel = new ConsultaGanadorLotoDomRequestModel();

                    var p = pAnulacionModel;
                    p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                    p.Usuario = cuenta.PMCuenta.RecargaID;
                    p.Password = cuenta.PMCuenta.CueServidor;
                    p.CodigoOperacion = transaccionConsultaGanador.TransaccionID.ToString();
                    p.DiaOperacion = baseParams.DiaOperacion;
                    p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                    p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                    p.CodigoOperacion = baseParams.CodigoOperacion;
                    p.TipoOperacion = MetodosEnum.MetodoServicio.Anulacion.ToString();

                    var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));


                    string user = p.Usuario;
                    string pass = p.Password;

                    var trama = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                    "t=" + user,
                    ":",
                    pass,
                    "*",
                    "winner_query",
                    "*",
                    pTicketId,
                     "*");

                    //        t=user.ventas:user.ventas1234*validacion*2|11|15|N;2|12|10|N;*1002028*1*0001
                    //var t ="t=user.ventas:user.ventas1234*validation*2|11|15|N;2|12|10|N;*1002028*1*0001";
                    var param = new ConsultaGanadorLotoDomRequestModel
                    {
                        ServiceUrl = p.ServiceUrl,
                        CurlString = trama
                    };

                    //Consume servicio

                    var consultaPago = ClienteHTTP.CallService<ConsultaGanadorLotoDomResponseModel, ConsultaGanadorLotoDomRequestModel>(null, param, MetodosEnum.HttpMethod.POST, null, false, true);


                    if (consultaPago != null)
                    {
                        //Actualiza TransaccionClienteHttp
                        if (consultaPago.Cod == "00")
                        {
                            transaccion.Activo = true;
                            transaccion.Estado = "ConsultaGanador Exitosa";
                            transaccion.Respuesta = consultaPago.ToString();
                            transaccion.FechaRespuesta = DateTime.Parse(ClienteHTTP.FechaRespuesta.ToString());

                            transaccion.Comentario = consultaPago.Msg;
                            transaccion.Peticion = p.ConsultaGanadorAnonima().ToString();
                            transaccion.TransaccionID = transaccionConsultaGanador.TransaccionID;
                            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                        }
                        else
                        {

                            transaccion.Activo = false;
                            transaccion.Estado = "Rechazada";
                            transaccion.Respuesta = consultaPago.ToString();
                            transaccion.FechaRespuesta = DateTime.Parse(ClienteHTTP.FechaRespuesta.ToString());
                            transaccion.Autorizacion = consultaPago.Msg;
                            transaccion.Comentario = consultaPago.Msg;
                            transaccion.Peticion = p.ConsultaGanadorAnonima().ToString();
                            transaccion.TransaccionID = transaccionConsultaGanador.TransaccionID;
                            //transaccion.NautCalculado = codigoFueraLinea;
                            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                        }
                    }
                    else
                    {
                        transaccion.Activo = false;
                        transaccion.Estado = "Solicitud";
                        transaccion.Respuesta = "El Servicio LotoDom  No Responde";
                        transaccion.FechaRespuesta = DateTime.Now;
                        transaccion.Comentario = consultaPago.ToString();
                        //transaccion.NautCalculado = codigoFueraLinea;
                        var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                    }
                    return consultaPago;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaError(e.Message + e.StackTrace);
                return null;
            }

        }

        
    }
}
