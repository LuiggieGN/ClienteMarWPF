using MAR.BusinessLogic.Code.Hacienda.SharedOperations;
using MAR.DataAccess.EFRepositories;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.ViewModels;
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

namespace MAR.BusinessLogic.Code.SorteosMar
{
    public class AnulacionLogic
    {
        public static object Anulacion(string pTicketId, int pBancaID, string pPin, int pRiferoId)
        {
            try
            {
                if (!BaseViewModel.ComparaPinGanador(int.Parse(pTicketId), pPin))
                {
                    return new { OK = false, Err = "Pin incorrecto", Mensaje = "Pin incorrecto" };
                }


                //Get DTicket para llenar modelo Anulacion
                var transaccionClienteHttp = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.GetTransaccionClienteHttp(pTicketId);

                // inicializa transaccion ClienteHttp
                if (transaccionClienteHttp == null)
                {
                    return new { OK = false, Error = "El Ticket no fue encontrado" };
                }


                
                    // inicializa transaccion ClienteHttp
                    var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
                    {
                        Activo = false,
                        Autorizacion = "",
                        Comentario = "MAR, Inicia Anulacion",
                        Estado = "Solicitud",
                        Referencia = pTicketId,
                        Monto = transaccionClienteHttp.Monto,
                        TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Anulacion,
                        Fecha = DateTime.Now,
                        FechaSolicitud = DateTime.Now,
                        TipoAutorizacion = 1,
                        NautCalculado = "",
                        FechaRespuesta = null,
                        BancaID = transaccionClienteHttp.BancaID
                    };

                    //Agrega TransaccionClienteHttp
                    var transaccionAnulacion = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

                    //Busca cuenta y producto
                    var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, DbEnums.Productos.CincoMinutos.ToString(), pBancaID, 1);


                    //Crea base parameters
                    var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionAnulacion.TransaccionID.ToString());
                    AnulacionRequestModel pAnulacionModel = new AnulacionRequestModel();

                    var p = pAnulacionModel;
                    p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                    p.Usuario = cuenta.PMCuenta.RecargaID;
                    p.Password = cuenta.PMCuenta.CueServidor;
                    p.CodigoOperacion = transaccionAnulacion.TransaccionID.ToString();
                    p.DiaOperacion = baseParams.DiaOperacion;
                    p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                    p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                    p.CodigoOperacion = baseParams.CodigoOperacion;
                    p.MontoOperacion = transaccionAnulacion.Monto;
                    p.CodigoOperacionReferencia = transaccionClienteHttp.TransaccionID.ToString();
                    p.TipoOperacion = MetodosEnum.MetodoServicio.Anulacion.ToString();
                    p.TerminalID = cuenta.Terminal.TerminalId;
                    p.NumeroAutenticacionReferencia = transaccionClienteHttp.Autorizacion;
                    p.LocalID = cuenta.Terminal.LocalId;
                    p.CodigoRazonAnulacion = "100";
                    var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

                    //Consume servicio
                    var anulacion = ClienteHTTP.CallService<AnulacionResponseModel, AnulacionRequestModel>(MetodosEnum.MetodoServicio.Anulacion, p, MetodosEnum.HttpMethod.POST, autenticacion, false);

                    //NAUT Calculado
                    string codigoFueraLinea = "";
                    if (anulacion == null || anulacion.CodigoRespuesta != "100")
                    {
                        codigoFueraLinea = NAUTLogic.GeneraNautCalculado(p.EstablecimientoID, p.CodigoOperacion, cuenta.AutorizacionFueraDeLinea, 'T');
                    }

                    if (anulacion != null)
                    {
                        //Actualiza TransaccionClienteHttp
                        if (anulacion.CodigoRespuesta == "100")
                        {

                            //Anula vp transaccion
                            VpTransacciones.AnulaVPTransaccion(int.Parse(pTicketId),"Anulado");

                            transaccion.Activo = true;
                            transaccion.Estado = "Anulacion Exitosa";
                            transaccion.Respuesta = anulacion.ToString();
                            transaccion.FechaRespuesta = DateTime.Parse(anulacion.FechaHoraRespuesta);
                            transaccion.Autorizacion = anulacion.NumeroAutentificacion;
                            transaccion.Comentario = anulacion.MensajeRespuesta;
                            transaccion.Peticion = p.Anulacion().ToString();
                            transaccion.TransaccionID = transaccionAnulacion.TransaccionID;
                            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                            return new { OK = true, PrintData = "", RespuestaApi = "Anulacion Exitosa" };
                        }
                        else
                        {
                            transaccion.Activo = false;
                            transaccion.Estado = "Rechazada";
                            transaccion.Respuesta = anulacion.ToString();
                            transaccion.FechaRespuesta = DateTime.Parse(anulacion.FechaHoraRespuesta);
                            transaccion.Autorizacion = anulacion.NumeroAutentificacion;
                            transaccion.Comentario = anulacion.MensajeRespuesta;
                            transaccion.Peticion = p.Anulacion().ToString();
                            transaccion.TransaccionID = transaccionAnulacion.TransaccionID;
                            transaccion.NautCalculado = codigoFueraLinea;
                            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                            return new { OK = false, PrintData = "", RespuestaApi = "Error. NO se pudo anular la apuesta" };
                        }
                    }
                    else
                    {
                        transaccion.Activo = false;
                        transaccion.Estado = "Solicitud";
                        transaccion.Respuesta = "El Servicio Hacienda  No Responde";
                        transaccion.FechaRespuesta = DateTime.Now;
                        transaccion.Comentario = anulacion.ToString();
                        transaccion.NautCalculado = codigoFueraLinea;
                        var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                        return new { OK = true, PrintData = "", RespuestaApi = "Error. NO se pudo anular la apuesta" };
                    }

                    ////Agrega transaccionRefencia 
                    ////*********************revisa esto para los parametros. el detallee NO RECUERDO SI LO NECESITO O NO***
                    //DataAccess.EFRepositories.Hacienda.TransaccionReferenciaRepository.AgregaTransaccion(transaccion.TransaccionID, MetodosEnum.MetodoServicio.Apuesta, apuesta.ToString());
            }
            catch (Exception e)
            {
                DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaError(e.Message + e.StackTrace);
                return new { OK = true, PrintData = "", RespuestaApi = "Error. NO se pudo anular la apuesta" };
            }
           
        }
    }

}
