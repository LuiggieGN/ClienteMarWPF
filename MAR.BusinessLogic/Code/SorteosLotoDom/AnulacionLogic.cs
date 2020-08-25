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
    public class AnulacionLogic
    {
        public static void Anulacion(string pTicketNumero, int pBancaID)
        {
            try
            {
                //Get DTicket para llenar modelo ApuestaReuqestModel
                var anulacionValues = DataAccess.EFRepositories.LotoDom.AnulacionRepository.GetAnulacionRequestDBValues(pTicketNumero);
                if (anulacionValues != null)
                {
                    // inicializa transaccion ClienteHttp
                    var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
                    {
                        Activo = false,
                        Autorizacion = "",
                        Comentario = "LotoDom, Inicia Anulacion",
                        Estado = "Solicitud",
                        Referencia = pTicketNumero,
                        Monto = anulacionValues.MontoOperacion,
                        TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Anulacion,
                        Fecha = DateTime.Now,
                        FechaSolicitud = DateTime.Now,
                        TipoAutorizacion = 1,
                        NautCalculado = "",
                        FechaRespuesta = null,
                        BancaID = pBancaID
                    };

                    //Agrega TransaccionClienteHttp
                    var transaccionAnulacion = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

                    //Busca cuenta y producto
                    var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(100000, "LotoDom", pBancaID, 0);


                    //Crea base parameters
                    var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionAnulacion.TransaccionID.ToString());
                    AnulacionLotoDomRequestModel pAnulacionModel = new AnulacionLotoDomRequestModel();

                    var p = pAnulacionModel;
                    p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                    p.Usuario = cuenta.PMCuenta.RecargaID;
                    p.Password = cuenta.PMCuenta.CueServidor;
                    p.CodigoOperacion = transaccionAnulacion.TransaccionID.ToString();
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
                    "cancel",
                    "*",
                    anulacionValues.TicketID,
                     "*");

                    //        t=user.ventas:user.ventas1234*validacion*2|11|15|N;2|12|10|N;*1002028*1*0001
                    //var t ="t=user.ventas:user.ventas1234*validation*2|11|15|N;2|12|10|N;*1002028*1*0001";
                    var param = new AnulacionLotoDomRequestModel
                    {
                        ServiceUrl = p.ServiceUrl,
                        CurlString = trama
                    };

                    //Consume servicio
                   
                    var anulacion = ClienteHTTP.CallService<AnulacionLotoDomResponseModel, AnulacionLotoDomRequestModel>(null, param, MetodosEnum.HttpMethod.POST, null, false, true);


                    if (anulacion != null)
                    {
                        //Actualiza TransaccionClienteHttp
                        if (anulacion.Cod == "00")
                        {
                            transaccion.Activo = true;
                            transaccion.Estado = "Anulacion Exitosa";
                            transaccion.Respuesta = anulacion.ToString();
                            transaccion.FechaRespuesta = DateTime.Parse(ClienteHTTP.FechaRespuesta.ToString());

                            transaccion.Comentario = anulacion.Msg;
                            transaccion.Peticion = p.AnulacionAnonima().ToString();
                            transaccion.TransaccionID = transaccionAnulacion.TransaccionID;
                            var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                        }
                        else
                        {
                          
                            transaccion.Activo = false;
                            transaccion.Estado = "Rechazada";
                            transaccion.Respuesta = anulacion.ToString();
                            transaccion.FechaRespuesta = DateTime.Parse(ClienteHTTP.FechaRespuesta.ToString());
                            transaccion.Autorizacion = anulacion.Msg;
                            transaccion.Comentario = anulacion.Msg;
                            transaccion.Peticion = p.AnulacionAnonima().ToString();
                            transaccion.TransaccionID = transaccionAnulacion.TransaccionID;
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
                        transaccion.Comentario = anulacion.ToString();
                        //transaccion.NautCalculado = codigoFueraLinea;
                        var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
                    }

                }
            }
            catch (Exception e)
            {
                DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaError(e.Message + e.StackTrace);
            }
           
        }
    }

}
