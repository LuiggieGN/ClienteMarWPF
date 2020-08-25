using FluentScheduler;
using Flujo.Entities.WpfClient.Enums;
using MAR.BusinessLogic.Code.Hacienda;
using MAR.BusinessLogic.Code.Hacienda.SharedOperations;
using MAR.DataAccess.EFRepositories;
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
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.SorteosMar
{
    public class PagoGanadorLogic
    {
        public static object PagoGanador(int pBancaId, string pDireccion, string pBancaNombre, int pPrintW, int pRiferoId, string pTicketId, string pPin, DetalleJugadasPago pJugadas, int pUsuarioId)
        {
            try
            {
                if (!BaseViewModel.ComparaPinGanador(int.Parse(pTicketId), pPin))
                {
                    return new { OK = false, Err = "Pin incorrecto", Mensaje = "Pin incorrecto" };
                }


                //Get DTicket para llenar modelo PagoRequestModel
                var transaccionClienteHttp = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.GetTransaccionClienteHttp(pTicketId);

                // inicializa transaccion ClienteHttp
                if (transaccionClienteHttp == null)
                {
                    return new { OK = false, Error = "El Ticket no fue encontrado" };
                }
                var transaccion = new DataAccess.Tables.DTOs.TransaccionClienteHttp()
                {
                    Activo = false,
                    Autorizacion = "",
                    Comentario = "MAR, Inicia PagoGanador",
                    Estado = "Solicitud",
                    Monto = pJugadas.Juego.Sum(x => x.MontoPagado),
                    Referencia = pTicketId,
                    TipoTransaccionID = (int)MarConnectCliente.Enums.MetodosEnum.MetodoServicio.PagoGanador,
                    Fecha = DateTime.Now,
                    FechaSolicitud = DateTime.Now,
                    TipoAutorizacion = 1,
                    NautCalculado = "",
                    FechaRespuesta = null,
                    BancaID = pBancaId
                };

                //Agrega TransaccionClienteHttp
                var transaccionPago = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.AgregaTransaccion(transaccion);

                //Busca cuenta y producto
                var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, DbEnums.Productos.CincoMinutos.ToString(), pBancaId, 1);

                //Crea base parameters
                var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, transaccionPago.TransaccionID.ToString());


                var p = new PagoGanadorRequestModel();
                p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                p.Usuario = cuenta.PMCuenta.RecargaID;
                p.Password = cuenta.PMCuenta.CueServidor;
                p.DiaOperacion = baseParams.DiaOperacion;
                p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                p.CodigoOperacion = baseParams.CodigoOperacion;
                p.MontoOperacion = pJugadas.Juego.Sum(x => x.MontoPagado);
                p.TipoOperacion = MetodosEnum.MetodoServicio.PagoGanador.ToString();
                p.TerminalID = cuenta.Terminal.TerminalId;
                p.NumeroAutenticacionReferencia = transaccionClienteHttp.Autorizacion;
                p.CodigoOperacionReferencia = transaccionClienteHttp.TransaccionID.ToString();
                p.LocalID = cuenta.Terminal.LocalId;
                p.Jugador = null;

                //int vpTransaccionId = int.Parse(pTicketId);
                //var vpTransacciones = VpTransacciones.GetTransacciones(x => x.TransaccionID == vpTransaccionId && x.BancaID == pBancaId, null, "VP_TransaccionDetalle");
                //var vpTransaccionesJugadas = JuegaMasViewModel.MapFromTransacciones(vpTransacciones).FirstOrDefault().Jugadas;





                var jugadas = new List<JuegoPago>();

                foreach (var item in pJugadas.Juego)
                {
                    jugadas.Add(new JuegoPago { Codigo = "CM5", Jugada = item.Jugada, MontoApostado = item.MontoApostado, MontoPagado = item.MontoPagado, TipoJugadaID = item.TipoJugadaID });
                }
                p.DesglosePago = new DesglosePago
                {
                    Fecha = transaccionClienteHttp.Fecha.ToString("yyyy-MM-dd HH:mm:ss"),
                    NoTicket = pTicketId,
                    Detalle = new DetalleJugadasPago { Juego = jugadas }
                };
                var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

                //agrega pago vp transaccion
                var vpCuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.CincoMinutosGanador);
                var vpTransGanador = DataAccess.EFRepositories.VpTransacciones.AgregarTransaccion(pBancaId, vpCuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, vpCuenta.ProductoId, vpCuenta.VpCuenta.CuentaID,
                            0, transaccion.Monto, DbEnums.Productos.CincoMinutosGanador.ToString(), pUsuarioId, null);



                //Consume servicio
                var pago = ClienteHTTP.CallService<PagoGanadoresResponseModel, PagoGanadorRequestModel>(MetodosEnum.MetodoServicio.PagoGanador, p, MetodosEnum.HttpMethod.POST, autenticacion);

                if (pago == null)
                {
                    return new { OK = false, Error = "Fallo la Transaccion", Mensaje = "Fallo la Transaccion", PrintData = "" };
                }
                //Actualiza TransaccionClienteHttp
                if (pago.CodigoRespuesta == "100")
                {
                    transaccion.Activo = true;
                    transaccion.Estado = "Pago Exitoso";

                    //actualiza pago vpTransaccion
                    vpTransGanador.Activo = true;
                    vpTransGanador.FechaDescuento = DateTime.Now;
                    vpTransGanador.Referencia = pago.NumeroAutentificacion;
                }
                else
                {
                    return new { OK = true, Error = "Fallo la Transaccion", Mensaje = "Fallo la Transaccion", PrintData = "", RespuestaApi = pago };
                }
                transaccion.Respuesta = pago.MensajeRespuesta;
                transaccion.FechaRespuesta = DateTime.Parse(pago.FechaHoraRespuesta);
                transaccion.Autorizacion = pago.NumeroAutentificacion;

                transaccion.Comentario = pago.MensajeRespuesta;
                transaccion.Peticion = p.ToString();
                transaccion.TransaccionID = transaccionPago.TransaccionID;
                var transaccionActulizada = DataAccess.EFRepositories.Hacienda.TransaccionClienteHttpRepository.ActualizaTransaccion(transaccion);

                vpTransGanador.Estado = pago.MensajeRespuesta;
                VpTransacciones.ActualizaTransaccion(vpTransGanador);

                if (pago.CodigoRespuesta == "100")
                {
                    return new
                    {
                        OK = true,
                        Mensaje = "Pago realizado",
                        PrintData = PrintJobs.CincoMinutosPrintJob.ImprimirReciboPago(pDireccion, pBancaNombre, pPrintW, pTicketId,
                        int.Parse(p.MontoOperacion.ToString()), pago.NumeroAutentificacion),
                        RespuestaApi = pago
                    };
                }
                return new { OK = true, Error = "Fallo la Transaccion", Mensaje = "Fallo la Transaccion", PrintData = "", RespuestaApi = pago };
            }
            catch (Exception e)
            {
                return new { OK = false, Err = e.Message, Mensaje = e.Message };
            }


        }


        public static object ConsultaPagoGanador(int pBancaId, int pRiferoId, string pTicketId, string pPin)
        {
            try
            {


                if (!BaseViewModel.ComparaPinGanador(int.Parse(pTicketId), pPin))
                {
                    return new { OK = false, Error = "Pin o Ticket incorrecto" };
                }

                var ticket = GetTicketDetalle(int.Parse(pTicketId));
                if (ticket == null || !ticket.Jugadas.Any())
                {
                    return new { OK = false, Error = "No se encuentra el Numero de Ticket" };
                }

                //Busca cuenta y producto
                var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, DbEnums.Productos.CincoMinutos.ToString(), pBancaId, 1);

                //Crea base parameters
                var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, "0");
                var p = new ConsultaPagoGanadorRequestModel();
                p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                p.Usuario = cuenta.PMCuenta.RecargaID;
                p.Password = cuenta.PMCuenta.CueServidor;
                p.DiaOperacion = baseParams.DiaOperacion;
                p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                p.CodigoOperacion = baseParams.CodigoOperacion;
                p.NoTicket = pTicketId;
                p.TipoOperacion = MetodosEnum.MetodoServicio.ConsultaPagoGanador.ToString();

                var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));
                //Consume servicio
                var Consultapago = ClienteHTTP.CallService<ConsultaPagoGanadorResponseModel, ConsultaPagoGanadorRequestModel>(MetodosEnum.MetodoServicio.ConsultaPagoGanador, p, MetodosEnum.HttpMethod.POST, autenticacion);
                if (Consultapago != null)
                {

                    Consultapago.TicketEstado = SetTicketEstado(Consultapago.TicketDetalle);
                    return new { RespuestaApi = Consultapago, OK = true, Ticket = ticket };
                }
                else
                {
                    return new { RespuestaApi = Consultapago, OK = false, Error = "Fallo la transaccion" };
                }

            }
            catch (Exception)
            {

                return new { OK = false, Error = "Fallo la Transaccion" };
            }

        }


        public static decimal ConsultaSacoCincoMinutos(int pBancaId, string pDesde, string pHasta, string pDia)
        {
            try
            {
                string desde = null;
                string hasta = null;
                if (pDesde == null)
                {
                    desde = pDia;
                    hasta = pDia;
                }
                else
                {
                    desde = pDesde;
                    hasta = pHasta;
                }


                //Busca cuenta y producto
                var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(10000, DbEnums.Productos.CincoMinutos.ToString(), pBancaId, 1);

                //Crea base parameters
                var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, "0");
                var p = new ConsultaSacoRequestModel();
                p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                p.Usuario = cuenta.PMCuenta.RecargaID;
                p.Password = cuenta.PMCuenta.CueServidor;
                p.DiaOperacion = baseParams.DiaOperacion;
                p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                p.CodigoOperacion = baseParams.CodigoOperacion;
                p.FechaDesde = DateTime.Parse(desde).ToString("yyyy-MM-dd");
                p.FechaHasta = DateTime.Parse(hasta).ToString("yyyy-MM-dd");
                p.TerminalID = pBancaId == 0 ? pBancaId : cuenta.Terminal.TerminalId;
                p.TipoOperacion = MetodosEnum.MetodoServicio.ConsultaSaco.ToString();

                var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

                //Consume servicio
                var consultaSaco = ClienteHTTP.CallService<ConsultaSacoResponseModel, ConsultaSacoRequestModel>(MetodosEnum.MetodoServicio.ConsultaSaco, p, MetodosEnum.HttpMethod.POST, autenticacion);
                if (consultaSaco != null)
                {
                    if (consultaSaco.CodigoRespuesta == "100" && consultaSaco.Consulta.Any())
                    {
                        var consultaSacoClean = consultaSaco.Consulta.Where(x => x.Saco > 0).ToList();
                        foreach (var item in consultaSacoClean)
                        {
                            if (item.FechaPago < DateTime.Now.AddYears(-1))
                            {
                                item.FechaPago = null;
                            }
                        }
                        //me quede aqui y ahora voy a automatizar la busqueda del saco en el api
                        MAR.DataAccess.EFRepositories.SorteosMar.ApuestaRepository.ActualizaSacoVPTrasnacciones(consultaSacoClean);
                        //JobManager.Initialize(new Scheduller.MyRegistry());
                        //foreach (var item in consultaSaco.Consulta)
                        //{

                        //}

                        decimal saco = decimal.Parse(consultaSaco.Consulta.Sum(x => x.Saco).ToString());
                        return saco;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }

            }
            catch (Exception)
            {

                return 0;
            }

        }





        public static List<ConsultaPremios> ConsultaPremiosCamionMillonario(int pBancaId)
        {
            try
            {
                //Busca cuenta y producto
                var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(10000, DbEnums.Productos.CincoMinutos.ToString(), pBancaId, 1);

                //Crea base parameters
                var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, "0");
                var p = new ConsultaSacoRequestModel();
                p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                p.Usuario = cuenta.PMCuenta.RecargaID;
                p.Password = cuenta.PMCuenta.CueServidor;
                p.DiaOperacion = baseParams.DiaOperacion;
                p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                p.CodigoOperacion = baseParams.CodigoOperacion;
           
                p.TerminalID = pBancaId == 0 ? pBancaId : cuenta.Terminal.TerminalId;
                p.TipoOperacion = MetodosEnum.MetodoServicio.ConsultaPremios.ToString();

                var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

                //Consume servicio
                var premios = ClienteHTTP.CallService<ConsultaPremiosResponseModel,
                    ConsultaSacoRequestModel>(MetodosEnum.MetodoServicio.ConsultaPremios, p, MetodosEnum.HttpMethod.POST, autenticacion);
                if (premios != null)
                {
                    return premios.Premios; 
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }







        public static decimal CargaGanadoresCamionMillonario(int pBancaId, string pDesde, string pHasta, string pDia)
        {
            try
            {
                string desde = null;
                string hasta = null;
                if (pDesde == null)
                {
                    desde = pDia;
                    hasta = pDia;
                }
                else
                {
                    desde = pDesde;
                    hasta = pHasta;
                }


                //Busca cuenta y producto
                var cuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(10000, DbEnums.Productos.CincoMinutos.ToString(), pBancaId, 1);

                //Crea base parameters
                var baseParams = BaseRequestLogic.CreaBaseRequest(cuenta, "0");
                var p = new ConsultaSacoRequestModel();
                p.EstablecimientoID = cuenta.PMCuenta.CueComercio;
                p.Usuario = cuenta.PMCuenta.RecargaID;
                p.Password = cuenta.PMCuenta.CueServidor;
                p.DiaOperacion = baseParams.DiaOperacion;
                p.FechaHoraSolicitud = baseParams.FechaHoraSolicitud;
                p.ServiceUrl = new Uri(cuenta.SWProducto.URL);
                p.CodigoOperacion = baseParams.CodigoOperacion;
                p.FechaDesde = DateTime.Parse(desde).ToString("yyyy-MM-dd");
                p.FechaHasta = DateTime.Parse(hasta).ToString("yyyy-MM-dd");
                p.TerminalID = pBancaId == 0 ? pBancaId : cuenta.Terminal.TerminalId;
                p.TipoOperacion = MetodosEnum.MetodoServicio.ConsultaSaco.ToString();

                var autenticacion = new AuthenticationHeaderValue("Basic", BaseRequestLogic.Base64Encode($"{p.Usuario}:{p.Password}"));

                //Consume servicio
                var consultaSaco = ClienteHTTP.CallService<ConsultaSacoResponseModel, ConsultaSacoRequestModel>(MetodosEnum.MetodoServicio.ConsultaSaco, p, MetodosEnum.HttpMethod.POST, autenticacion);
                if (consultaSaco != null)
                {
                    if (consultaSaco.CodigoRespuesta == "100")
                    {

                        MAR.DataAccess.EFRepositories.SorteosMar.ApuestaRepository.ActualizaSacoVPTrasnacciones(consultaSaco.Consulta);
                        //foreach (var item in consultaSaco.Consulta)
                        //{

                        //}

                        return decimal.Parse(consultaSaco.Consulta.Sum(x => x.Saco).ToString());
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return 0;
                }

            }
            catch (Exception)
            {

                return 0;
            }

        }


        private static int SetTicketEstado(List<TicketResponseModel.TicketDetalle> pTicketDetalle)
        {
            //     public enum TicketEstado
            //{
            //    NoValido = 0,
            //    JugadoNoSorteo = 1,
            //    JugadoNoGanador = 2,
            //    JugadoGanadorNoPago = 3,
            //    JugadoGanadorPagado = 4
            //}


            //Ticket No Valido
            //if (pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.NoValido)
            //   )
            //{
            //    return (int)CincoMinutosEnum.TicketEstado.NoValido;
            //}


            //Hay sorteos sin salir
            if (pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoNoSorteo)
             )
            {
                return (int)CincoMinutosEnum.TicketEstado.JugadoNoSorteo;
            }

            //Hay sorteos sin salir y si ha ganado
            else if (pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoNoSorteo) &&
                pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoGanadorNoPago)
                )
            {
                return (int)CincoMinutosEnum.TicketEstado.JugadoNoSorteo;
            }

            //Todos los Sorteos Salieron y NO saco
            else if (pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoNoSorteo) == false &&
                pTicketDetalle.All(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoNoGanador)
                )
            {
                return (int)CincoMinutosEnum.TicketEstado.JugadoNoGanador;
            }

            //Todos los Sorteos Salieron y saco no pagado aun
            else if (pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoNoSorteo) == false &&
                pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoGanadorNoPago)
                )
            {
                return (int)CincoMinutosEnum.TicketEstado.JugadoGanadorNoPago;
            }

            //Todos los Sorteos Salieron y saco YA PAGADO
            else if (pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoNoSorteo) == false &&
                pTicketDetalle.Any(x => x.JugadaEstado == (int)CincoMinutosEnum.TicketEstado.JugadoGanadorPagado))
            {
                return (int)CincoMinutosEnum.TicketEstado.JugadoGanadorPagado;
            }
            else
            {
                return (int)CincoMinutosEnum.TicketEstado.NoValido;
            }
        }

        public static JuegaMasViewModel GetTicketDetalle(int pTransaccionId)
        {
            try
            {
                var trans = DataAccess.EFRepositories.VpTransacciones.GetTransacciones(
                    x => x.TransaccionID == pTransaccionId, includeProperties: "VP_TransaccionDetalle").FirstOrDefault();
                JuegaMasViewModel ticketViewModel = new JuegaMasViewModel();
                VP_HTransaccion transH = new VP_HTransaccion();
                if (trans != null)
                {
                    ticketViewModel = JuegaMasViewModel.MapFromTransaccion(trans, true);
                }
                else
                {
                    transH = DataAccess.EFRepositories.VpTransacciones.GetHTransacciones(
                  x => x.TransaccionID == pTransaccionId, includeProperties: "VP_HTransaccionDetalle").FirstOrDefault();
                    if (transH != null)
                    {
                        ticketViewModel = JuegaMasViewModel.MapFromHTransaccion(transH, true);
                    }
                }
                if (trans != null || transH != null)
                {
                    return ticketViewModel;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception e)
            {
                return null;
            }

        }
    }
}
