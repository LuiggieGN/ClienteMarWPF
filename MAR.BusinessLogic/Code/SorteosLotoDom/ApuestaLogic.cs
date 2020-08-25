using MAR.BusinessLogic.Code.SorteosLotoDom.SharedOperations;
using MAR.DataAccess.ViewModels;
using MarConnectCliente;
using MarConnectCliente.BusinessLogic.ShareFuntions;
using MarConnectCliente.Enums;
using MarConnectCliente.Helpers;
using MarConnectCliente.RequestModels.LotoDom;
using MarConnectCliente.ResponseModels.LotoDom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.SorteosLotoDom
{
    public class ApuestaLogic
    {
     
      
        public static object Apuesta(string pTicketId, int pRiferoId)
        {
            //Get DTicket para llenar modelo ApuestaReuqestModel
            var dTicket = DataAccess.EFRepositories.LotoDom.ApuestaRepository.GetDTicket(pTicketId);
            if (dTicket == null)
            {
                return new { OK = false, Mensaje = "Error creando la confirmacion del ticket", Respuesta = "", Err = "Error creando la confirmacion del ticket" };
            }
            
            // inicializa transaccion ClienteHttp
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "LotoDom, Inicia Apuesta",
                Estado = "Solicitud",
                Monto = dTicket.MontoOperacion,
                Referencia = dTicket.NoTicket.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Apuesta,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                FechaRespuesta = null,
                BancaID = dTicket.BancaID
            };

            //Agrega TransaccionClienteHttp
            var transaccionApuesta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, "LotoDom", dTicket.BancaID, 0);

            //Si el acumulado no esta, entonces busco en LOTODOM. le mando la pedirla en MAR DB otra vez
            if (dTicket.SorteoMontoAcumulado == null || dTicket.SorteoMontoAcumulado < 1)
            {
                dTicket.SorteoMontoAcumulado = ConsultaAcumuladoEnLotoDom(dTicket.BancaID, pRiferoId, cuenta, dTicket.LoteriaID);
            }
            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionApuesta.TransaccionID.ToString());
            ApuestaLotoDomRequestModel pApuestaModel = new ApuestaLotoDomRequestModel();

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
            p.TerminalID = dTicket.BancaID;
           

            var jugadas = new List<Juego>();
            StringBuilder jugadasBuilder = new StringBuilder();
            foreach (var item in dTicket.TicketDetalles)
            {
                jugadasBuilder.Append(2 + "|" + item.Jugada + "|" + item.Monto.ToString("N0") + "|N;");
                jugadas.Add(new Juego { Jugada = ApuestaHelper.SeparaJugadaConGuion(item.Jugada), Monto = item.Monto, });
            }
            
            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

            string codigoFueraLinea = "";
            try
            {
                string user = p.Usuario;
                string pass = p.Password;
                string jugadasString = jugadasBuilder.ToString();//"2|11|15|N;2|12|10|N;";
                string secuencia = dTicket.TicketID.ToString();
                string grupo = pRiferoId.ToString();
                string banc = dTicket.BancaID.ToString();

                //var response = LotoDomViewModel.ApuestaResponse.Exec_Apuesta(url, user, pass, jugadasString, secuencia, grupo, banc);

                var trama = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                "t=" + user,
                ":",
                pass,
                "*",
                "validation",
                "*",
                jugadasString,
                 "*",
                secuencia,
                 "*",
                grupo,
                 "*",
                banc);

                //        t=user.ventas:user.ventas1234*validacion*2|11|15|N;2|12|10|N;*1002028*1*0001
                //var t ="t=user.ventas:user.ventas1234*validation*2|11|15|N;2|12|10|N;*1002028*1*0001";
                var param = new ApuestaLotoDomRequestModel
                {
                    ServiceUrl = p.ServiceUrl,
                    CurlString = trama
                };

                //Consume servicio
                var apuesta = ClienteHTTP.CallService<ApuestaLotoDomResponseModel, ApuestaLotoDomRequestModel>(null, param, MetodosEnum.HttpMethod.POST, null, false, true);


                if (apuesta == null || apuesta.Cod != "00")
                {
                    //codigoFueraLinea = NAUTLogic.GeneraNautCalculado(p.EstablecimientoID, p.CodigoOperacion, cuenta.AutorizacionFueraDeLinea, 'T');
                    transaccion.Respuesta = "Fallo la Conexion al Servicio LotoDom";
                }

                //Actualiza TransaccionClienteHttp
                if (apuesta != null)
                {
                    if (apuesta.Cod  == "00")
                    {
                        transaccion.Activo = true;
                        transaccion.Estado = "Apuesta Exitosa";
                    }
                    transaccion.Respuesta = apuesta.ToString();
                    transaccion.FechaRespuesta = DateTime.Parse(ClienteHTTP.FechaRespuesta.ToString());
                    transaccion.Autorizacion = apuesta.no_auth;
                    transaccion.Comentario = apuesta.Msg??"";
                }
                else
                {
                    transaccion.Respuesta = "Fallo la Conexion al Servicio LotoDom";
                    transaccion.FechaRespuesta = DateTime.Now;
                }

                transaccion.Peticion = p.ApuestaAnonima().ToString();
                transaccion.TransaccionID = transaccionApuesta.TransaccionID;
                transaccion.NautCalculado = "";//codigoFueraLinea;

                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);

                if (apuesta != null && apuesta.Cod == "00")
                {
                    return new { OK = true, Mensaje = apuesta.Msg, Respuesta = apuesta.no_auth, MontoAcumulado = dTicket.SorteoMontoAcumulado??0, Err = "" };
                }
                return new { OK = true, Mensaje = apuesta.Msg, Respuesta = codigoFueraLinea, Err = "" };
            }
            catch (Exception e)
            {
                return new { OK = true, Mensaje = e.Message, Respuesta = codigoFueraLinea, Err = "" };
            }

        }



        public static double ConsultaAcumuladoEnLotoDom(int pBancaId, int pRiferoId, DataAccess.EFRepositories.PMCuentasRepository.CuentaProducto pCuenta, int pLoteriaId)
        {
            // inicializa transaccion ClienteHttp
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "LotoDom, Inicia GetAcumulado",
                Estado = "Solicitud",
                Monto = 0,
                Referencia = "",
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Apuesta,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                FechaRespuesta = null,
                BancaID = pBancaId
            };

            //Agrega TransaccionClienteHttp
            var transaccionApuesta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

            //Busca cuenta y producto
            var cuenta = pCuenta;// DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, "LotoDom", 0, 0);

            //Crea base parameters
            var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionApuesta.TransaccionID.ToString());
            AcumuladoLotoDomRequestModel pAcumuladoModel = new AcumuladoLotoDomRequestModel();

            var p = pAcumuladoModel;
            p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
            p.Usuario = cuenta.PMCuenta.RecargaID;
            p.Password = cuenta.PMCuenta.CueServidor;
            p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
            p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
            p.TipoOperacion = MetodosEnum.MetodoServicio.ConsultaAcumulado.ToString();

            //jugadasBuilder.Append(2 + "|" + item.Jugada + "|" + item.Monto.ToString("N0") + "|N;");

            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

        
            try
            {
                string user = p.Usuario;
                string pass = p.Password;

                var trama = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                "t=" + user,
                ":",
                pass,
                "*",
                "accumulated",
                "*",
                DateTime.Now,
                 "*");

                //        t=user.ventas:user.ventas1234*validacion*2|11|15|N;2|12|10|N;*1002028*1*0001
                //var t ="t=user.ventas:user.ventas1234*validation*2|11|15|N;2|12|10|N;*1002028*1*0001";
                var param = new AcumuladoLotoDomRequestModel
                {
                    ServiceUrl = p.ServiceUrl,
                    CurlString = trama
                };

                //Consume servicio
                var acumulado = ClienteHTTP.CallService<AcumuladoLotoDomResponseModel, AcumuladoLotoDomRequestModel>(null, param, MetodosEnum.HttpMethod.POST, null, false, true);


                if (acumulado == null || acumulado.Cod != "00")
                {
                    //codigoFueraLinea = NAUTLogic.GeneraNautCalculado(p.EstablecimientoID, p.CodigoOperacion, cuenta.AutorizacionFueraDeLinea, 'T');
                    transaccion.Respuesta = "Fallo la Conexion al Servicio LotoDom";
                }

                //Actualiza TransaccionClienteHttp
                if (acumulado != null)
                {
                    if (acumulado.Cod == "00")
                    {
                        transaccion.Activo = true;
                        transaccion.Estado = "ConsultaAcumulado Exitosa";
                    }
                    DataAccess.EFRepositories.LotoDom.ApuestaRepository.AgregaAcumuladoLotoDom(acumulado.Amount, pLoteriaId);
                    transaccion.Respuesta = acumulado.ToString();
                    transaccion.FechaRespuesta = DateTime.Parse(ClienteHTTP.FechaRespuesta.ToString());
                    transaccion.Autorizacion = "APPROVED";
                    transaccion.Comentario = acumulado.Msg;
                }
                else
                {
                    transaccion.Respuesta = "Fallo la Conexion al Servicio LotoDom";
                    transaccion.FechaRespuesta = DateTime.Now;
                }

                transaccion.Peticion = p.AcumuladoAnonima().ToString();
                transaccion.TransaccionID = transaccionApuesta.TransaccionID;
               

                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);

                if (acumulado != null && acumulado.Cod == "00")
                {
                    return acumulado.Amount;
                }
                return 0;
            }
            catch (Exception e)
            {
                return 0;
            }

        }

    }
}
