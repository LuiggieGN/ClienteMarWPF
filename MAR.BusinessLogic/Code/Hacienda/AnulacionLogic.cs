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
    public class AnulacionLogic
    {
        public static void Anulacion(string pTicketNumero, int pBancaID)
        {
            try
            {
                //Get DTicket para llenar modelo ApuestaReuqestModel
                var anulacionValues = DataAccess.EFRepositories.Hacienda.AnulacionRepository.GetAnulacionRequestDBValues(pTicketNumero);
                if (anulacionValues != null)
                {
                    // inicializa transaccion ClienteHttp
                    var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
                    {
                        Activo = false,
                        Autorizacion = "",
                        Comentario = "Hacienda, Inicia Anulacion",
                        Estado = "Solicitud",
                        Referencia = pTicketNumero,
                        Monto = anulacionValues.MontoOperacion,
                        TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Anulacion,
                        Fecha = DateTime.Now,
                        FechaSolicitud = DateTime.Now,
                        TipoAutorizacion = 1,
                        NautCalculado = "",
                        FechaRespuesta = null,
                        BancaID = anulacionValues.TerminalID
                    };

                    //Agrega TransaccionClienteHttp
                    var transaccionAnulacion = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

                    //Busca cuenta y producto
                    var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "Hacienda", pBancaID, 2);


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
                    p.MontoOperacion = anulacionValues.MontoOperacion;
                    p.CodigoOperacionReferencia = anulacionValues.CodigoOperacionReferencia;
                    p.TipoOperacion = MetodosEnum.MetodoServicio.Anulacion.ToString();
                    p.TerminalID = anulacionValues.TerminalID;
                    p.NumeroAutenticacionReferencia = anulacionValues.AutenticacionReferencia;
                    p.LocalID = anulacionValues.LocalID;
                    p.CodigoRazonAnulacion = "100";
                    var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

                    //Consume servicio
                    var anulacion = ClienteHTTP.CallService<AnulacionResponseModel, AnulacionRequestModel>(MetodosEnum.MetodoServicio.Anulacion, p, MetodosEnum.HttpMethod.POST, autenticacion, true);

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
                            transaccion.Activo = true;
                            transaccion.Estado = "Anulacion Exitosa";
                            transaccion.Respuesta = anulacion.ToString();
                            transaccion.FechaRespuesta = DateTime.Parse(anulacion.FechaHoraRespuesta);
                            transaccion.Autorizacion = anulacion.NumeroAutentificacion;
                            transaccion.Comentario = anulacion.MensajeRespuesta;
                            transaccion.Peticion = p.Anulacion().ToString();
                            transaccion.TransaccionID = transaccionAnulacion.TransaccionID;
                            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
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
                    }

                    ////Agrega transaccionRefencia 
                    ////*********************revisa esto para los parametros. el detallee NO RECUERDO SI LO NECESITO O NO***
                    //DataAccess.EFRepositories.Hacienda.TransaccionReferenciaRepository.AgregaTransaccion(transaccion.TransaccionID, MetodosEnum.MetodoServicio.Apuesta, apuesta.ToString());
                }
            }
            catch (Exception e)
            {
                DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaError(e.Message + e.StackTrace);
            }
           
        }
    }

}
