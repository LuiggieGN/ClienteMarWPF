using MAR.BusinessLogic.Code.Hacienda.SharedOperations;
using MAR.DataAccess.EFRepositories;
using MAR.DataAccess.EFRepositories.SorteosMar;
using MAR.DataAccess.Tables.DTOs;
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
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.SorteosMar
{
    public class ApuestaLogic
    {
        public static object Apuesta(ApuestaRepository.TicketModel pTicketModel, int pBancaId, int pUsuarioId, int pRiferoId, ProductosRepository.ProductoViewModel pProducto, string pDireccion
            ,int pPrintW, string pBancaNombre, string pPrintHead, string pPrintFoo)
        {
            //Get DTicket para llenar modelo ApuestaReuqestModel
            pTicketModel.TerminalID = pBancaId;
            //Busca cuenta y producto
         //   var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, "CincoMinutos", 0);
            var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, DbEnums.Productos.CincoMinutos.ToString(), pBancaId,1);
            
            
            // responder cuando la terminal no esta configurada para conectarse al servicio HaciendaTerminal tabla
            if (cuenta.Terminal == null)
            {
                return new { OK = false, Ticket = new List<string>(), PrintData = "", RespuestaApi = new ApuestaResponseModel { CodigoRespuesta = "0"
                , MensajeRespuesta = "Error. La Terminal no está configurada en la plataforma"}  };
            }

            var vpTransaccion = AgregaVPTransaccion(pBancaId, pUsuarioId, pRiferoId, pProducto, pTicketModel, cuenta);
            //var dTicket = DataAccess.EFRepositories.Hacienda.ApuestaRepository.GetDTicket(pTicketId);
            if (vpTransaccion == null)
            {
                return "Fallo al agregar la Transaccion en MAR";
            }
            pTicketModel.NoTicket = vpTransaccion.TransaccionID.ToString();

            // inicializa transaccion ClienteHttp
            var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
            {
                Activo = false,
                Autorizacion = "",
                Comentario = "MAR, Inicia Apuesta",
                Estado = "Solicitud",
                Monto = pTicketModel.MontoOperacion,
                Referencia = pTicketModel.NoTicket.ToString(),
                TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.Apuesta,
                Fecha = DateTime.Now,
                FechaSolicitud = DateTime.Now,
                TipoAutorizacion = 1,
                NautCalculado = "",
                FechaRespuesta = null,
                BancaID = pTicketModel.TerminalID
            };

            //Agrega TransaccionClienteHttp
            var transaccionApuesta = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

        

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
            p.MontoOperacion = pTicketModel.MontoOperacion;
            p.TipoOperacion = MetodosEnum.MetodoServicio.Apuesta.ToString();
            p.TerminalID = cuenta.Terminal.TerminalId;
            p.LocalID = cuenta.Terminal.LocalId;

            var jugadas = new List<Juego>();
            foreach (var item in pTicketModel.TicketDetalles)
            {
                jugadas.Add(new Juego { Codigo = "CM5", Jugada = item.Jugada, Monto = item.Monto, TipoJugadaID = item.TipoJugadaID, });
            }
            p.DesgloseOperacion = new Jugada
            {
                Fecha = pTicketModel.Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                NoTicket = pTicketModel.NoTicket,
                Detalle = new DetalleJugada { Juego = jugadas }
            };
            var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

            string codigoFueraLinea = "";
            try
            {
                //Consume servicio
                var apuestaResponse = ClienteHTTP.CallService<ApuestaResponseModel, ApuestaRequestModel>(MetodosEnum.MetodoServicio.Apuesta, p, MetodosEnum.HttpMethod.POST, autenticacion);

                //NAUT Calculado

                if (apuestaResponse == null || apuestaResponse.CodigoRespuesta != "100")
                {
                    codigoFueraLinea = NAUTLogic.GeneraNautCalculado(p.EstablecimientoID, p.CodigoOperacion, cuenta.AutorizacionFueraDeLinea, 'T');
                }

                //Actualiza TransaccionClienteHttp
                if (apuestaResponse != null)
                {
                    
                    if (apuestaResponse.CodigoRespuesta == "100")
                    {
                        transaccion.Activo = true;
                        vpTransaccion.Activo = true;
                        vpTransaccion.Estado = "Apuesta Exitosa";
                        vpTransaccion.Referencia = apuestaResponse.NumeroAutentificacion;
                        transaccion.Estado = "Apuesta Exitosa";
                    }
                    transaccion.Respuesta = apuestaResponse.ToString();
                    transaccion.FechaRespuesta = DateTime.Parse(apuestaResponse.FechaHoraRespuesta);
                    transaccion.Autorizacion = apuestaResponse.NumeroAutentificacion;
                    transaccion.Comentario = apuestaResponse.MensajeRespuesta;
                }
                else
                {
                    transaccion.Respuesta = "Fallo la Conexion al Servicio Hacienda";
                    transaccion.FechaRespuesta =ClienteHTTP.FechaRespuesta;
                }
                transaccion.Peticion = p.ApuestaAnonima().ToString();
                transaccion.TransaccionID = transaccionApuesta.TransaccionID;
                transaccion.NautCalculado = codigoFueraLinea;
                transaccion.FechaSolicitud = ClienteHTTP.FechaSolicitud;
                transaccion.FechaRespuesta = ClienteHTTP.FechaRespuesta;
       
                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);
            
                var vpTransaccionUpdated = VpTransacciones.ActualizaTransaccion(vpTransaccion);

                if (apuestaResponse != null && apuestaResponse.CodigoRespuesta == "100")
                {
                    List<ApuestaRepository.TicketModel> tckets = new List<ApuestaRepository.TicketModel>();
                    tckets.Add(pTicketModel);
                    string firma = GeneraFirma(pTicketModel.Fecha.ToShortDateString(), pTicketModel.Fecha.ToString("t"), pTicketModel.NoTicket, tckets);
                    var printData = PrintJobs.CincoMinutosPrintJob.ImprimirCincoMinutos(pPrintW, pBancaNombre, pDireccion, pTicketModel.NoTicket.ToString(),
                        BaseViewModel.GeneraPinGanador(vpTransaccion.TransaccionID), false, p, apuestaResponse, pPrintFoo, pPrintHead,pBancaId, firma);
                    return new { OK = true, Ticket = new List<string>(), PrintData = printData, RespuestaApi = apuestaResponse };
                    //return apuesta.NumeroAutentificacion;
                }
                if (apuestaResponse.CodigoRespuesta == "800")
                {
                    return new { OK = false, PrintData = "", RespuestaApi = "Error al procesar la transaccion. Comunique el administrador." };
                }
                return  new { OK = true, Ticket = new List<string>(), PrintData =  "", RespuestaApi = apuestaResponse };
            }
            catch (Exception e)
            {
                return new { CodigoFueraDeLinea = codigoFueraLinea, OK = false};
            }

        }

        private static string GeneraFirma(string pFecha, string pHora, string pTicket, List<ApuestaRepository.TicketModel> pJugadas)
        {
            try
            {
                var rdn = (new Random(DateTime.Now.Millisecond)).Next(31) + 1;
                var Cadena = String.Format("{0}{1}{2}{3}{4}21", pFecha, pHora, pTicket, pJugadas, rdn);
                long acum1 = 0;
                for (var i = 1; i <= Cadena.Length; i++)
                {
                    acum1 += (i * (int)Cadena[i - 1]);
                }

                var Source = (acum1 % 100000).ToString().PadLeft(5, '0');

                acum1 = 0;
                foreach (var item in pJugadas)
                {
                    Cadena = String.Format("{0}{1}{2}3", pJugadas,
                                            rdn,
                                            Convert.ToInt32(Math.Ceiling(item.MontoOperacion)));
                    for (var i = 0; i < Cadena.Length; i++)
                    {
                        acum1 += (int)Cadena[i];
                    }

                }


                Source = String.Format("{0}-{1}", Source, (acum1 % 10000000).ToString().PadLeft(7, '1'));

                var theCrosswalk = GetCrosswalk();
                var theResult = string.Empty;
                for (var cnt = 1; cnt <= 13; cnt++)
                {
                    var iSrcNo = 0;
                    var iSrc = Source[cnt - 1].ToString();
                    if (cnt == 6)
                    {
                        iSrcNo = rdn;
                    }
                    else
                    {
                        iSrcNo = theCrosswalk.Where(x => x.Value.Equals(iSrc)).Select(x => x.Key).FirstOrDefault();
                        iSrcNo = (iSrcNo + rdn + cnt) % 33;
                    }
                    theResult += theCrosswalk[iSrcNo];
                }
                return theResult;
            }
            catch
            {
                return "- no disponible -";
            }
        }
        private static Dictionary<int, string> GetCrosswalk()
        {
            return ("*0#Q*1#V*2#C*3#0*4#H*5#5*6#M*7#R*8#W*9#D*10#1*11#6*12#N*13#S*14#X*15#E*16#2*17#J*18#7*19#T*20#A*21#Y*22#F*23#3*24#K*25#8*26#P*27#U*28#B*29#L*30#G*31#4*32#9")
                       .Split('*').Where(x => x.Length > 1)
                       .Select(xx => new { key = Convert.ToInt32(xx.Split('#')[0]), value = xx.Split('#')[1] })
                       .ToDictionary(x => x.key, y => y.value);
        }
        public static VP_Transaccion AgregaVPTransaccion(int pBancaId, int pUsuarioId, int pRiferoId, ProductosRepository.ProductoViewModel pProducto, ApuestaRepository.TicketModel pTicketDetalle, PMCuentasRepository.CuentaProducto pCuenta)
        {
            try
            {
                var pmCuenta = pCuenta;
                if (pmCuenta == null)
                {
                    return null;
                }
               // var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.CincoMinutos);
              
                //agregando transaccion de CincoMinutos
                var transDetalle = from p in pTicketDetalle.TicketDetalles
                                   select new JuegaMasViewModel.JugadaJuegaMAS()
                                   {
                                       Jugada = p.Jugada,
                                       Cantidad = double.Parse(p.Monto.ToString()),
                                       Codigo = "CM5",
                                       Monto = double.Parse(p.Monto.ToString())
                                   };

                var transaccionDetalles = MapObjectsToVpTransaccionDetalles(transDetalle, pProducto.ProductoID, pProducto.ProductoCampos);
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasCincoMinutos.Codigo.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasCincoMinutos.Autorizacion.ToString()
                });

                var trans = VpTransacciones.AgregarTransaccion(pBancaId, pProducto.SuplidorID, pProducto.ProductoID, pProducto.Cuenta.CuentaID,
                        pTicketDetalle.MontoOperacion, 0, pProducto.Referencia, pUsuarioId, transaccionDetalles);

                return trans;
                //if (true)//(!ErrorRespuesta(int.Parse(auth.CodResp)))
                //{
                //    trans = AgregaLotteryVipToVPTransaccion(trans, auth);

                //    var ticketViewModel = VpTransacciones.ActualizaTransaccion(trans);


                //    return new
                //    {
                //        OK = true,
                //        Mensaje = "Exitosa",
                //        //  Bingo = ticketViewModel
                //    };
                //}



                //return new
                //{
                //    OK = false,
                //    Mensaje = auth.DescResp
                //};
            }
            catch (Exception e)
            {
                return null;
            }
        }
        private static VP_Transaccion AgregaLotteryVipToVPTransaccion(VP_Transaccion pTransaccion, MAR.DataAccess.ViewModels.Mappers.LotteryVIPMapper.LotteryVIP_Response pLoteryVipResponse)
        {
            pTransaccion.Referencia = pLoteryVipResponse.Referencia;
            pTransaccion.Estado = pLoteryVipResponse.DescResp;
            pTransaccion.Activo = true;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasBingo.Autorizacion.ToString()).ValorText = pLoteryVipResponse.Autorizacion;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasBingo.HoraSorteo.ToString()).ValorText = pLoteryVipResponse.CartaBingo.FechaSorteo;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasBingo.NumeroSorteo.ToString()).ValorText = pLoteryVipResponse.CartaBingo.NumeroSorteo.ToString();
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasBingo.FechaSorteo.ToString()).ValorText = pLoteryVipResponse.CartaBingo.FechaSorteo;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasBingo.Serial.ToString()).ValorText = pLoteryVipResponse.CartaBingo.NumeroSerie;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasBingo.CartonBingo.ToString()).ValorText = pLoteryVipResponse.CartaBingo.NumeroCarta;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasBingo.ReferenciaBingo.ToString()).ValorText = pLoteryVipResponse.CartaBingo.ReferenciaBingo;

            return pTransaccion;
        }
        private static List<VP_TransaccionDetalle> MapObjectsToVpTransaccionDetalles(IEnumerable<object> pObjects, int pProductoId, IEnumerable<VP_ProductoCampo> productoCampos)
        {
            List<VP_TransaccionDetalle> transaccionDetalles = new List<VP_TransaccionDetalle>();

            foreach (var item in pObjects)
            {
                Type type = item.GetType();
                PropertyInfo[] objInfo = type.GetProperties();
                foreach (PropertyInfo c in objInfo)
                {
                    if (productoCampos.Any(x => x.Nombre.ToUpper() == c.Name.ToUpper()))
                    {
                        var t = new VP_TransaccionDetalle
                        {
                            ProductoCampoID = productoCampos.FirstOrDefault(x => x.Nombre.ToUpper() == c.Name.ToUpper()).ProductoCampoID,
                            Referencia = productoCampos.FirstOrDefault(x => x.Nombre.ToUpper() == c.Name.ToUpper()).Nombre,
                            ValorText = c.GetValue(item, null).ToString()
                        };
                        transaccionDetalles.Add(t);
                    }
                }
            }
            return transaccionDetalles;
        }

    }
}
