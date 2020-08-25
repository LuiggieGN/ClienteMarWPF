using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MAR.BusinessLogic.Code.PrintJobs;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;

using System.Reflection;
using MAR.DataAccess.EFRepositories;
using MAR.DataAccess.ViewModels;
using JuegosTemplate.DataAccess.Servicios.LoteriaServicio.Utilidades;
using MAR.BusinessLogic.Models.RequestModel;
using MAR.DataAccess.ViewModels.Mappers;

namespace MAR.BusinessLogic.Code
{
    public class BingoIndexLogic
    {

        public static object GetProductosDisponibles()
        {
            List<DataAccess.Tables.Enums.DbEnums.VP_ProductoReferencia> productoRef = new List<DataAccess.Tables.Enums.DbEnums.VP_ProductoReferencia>();
            List<DataAccess.Tables.Enums.DbEnums.ReciboCampoReferencia> reciboRef = new List<DataAccess.Tables.Enums.DbEnums.ReciboCampoReferencia>();
            productoRef.Add(DataAccess.Tables.Enums.DbEnums.VP_ProductoReferencia.Bingo_C);
            productoRef.Add(DataAccess.Tables.Enums.DbEnums.VP_ProductoReferencia.Bingo_M);
            productoRef.Add(DataAccess.Tables.Enums.DbEnums.VP_ProductoReferencia.Bingo_I);
            reciboRef.Add(DataAccess.Tables.Enums.DbEnums.ReciboCampoReferencia.Bingo);
            var result = DataAccess.EFRepositories.ProductosRepository.GetVpProductosYCampos(productoRef,reciboRef);
            return new
            {
                OK = true,
                Productos = result
            };
        }

        public static object GetTicket_VendidosHoy(int pRiferoId, int pBancaId)
        {
            DateTime desde = DateTime.Today;
            DateTime hasta = DateTime.Today.AddDays(1);
            try
            {
                var cuentaId = DataAccess.EFRepositories.VpCuentaRepository.GetVpCuentaNew(pRiferoId,DbEnums.Productos.Bingo).CuentaID;

                var transacciones = VpTransacciones.GetTransacciones(x => x.FechaIngreso >= desde.Date && x.FechaIngreso <= hasta.Date && x.CuentaID == cuentaId && x.BancaID == pBancaId, null, "VP_TransaccionDetalle");
                var tickets = JuegaMasViewModel.MapFromTransacciones(transacciones).OrderByDescending(x => x.TicketID);
                return new { OK = true, Err = string.Empty, Tickets = tickets };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Mensaje = "Fallo la transaccion intente mas tarde",
                    Err = e.ToString()
                };
            }
        }

        public static object GetPremios(DateTime pFecha, int pBancaId)
        {
            var ganadores = JuegaMasViewModel.MarltonResponse.Exec_GetGanadoresJuegaMas(pFecha, pBancaId).WinnerNumber;
            return ganadores;
        }


        public static object SetSuplidor(string suplidorNombre)
        {
            MARContext db = new MARContext();
            try
            {
                var configuration = (from sup in db.PMSuplidores
                                     join pro in db.SWebProductoes on sup.SuplidorID equals pro.SuplidorID
                                     join con in db.SWebProductoConfigs on pro.WebProductoID equals con.WebProductoID
                                     where sup.SupNombre == suplidorNombre
                                     select new { con }).ToList();
                string login = configuration.FirstOrDefault(x => x.con.Opcion.ToUpper().Contains("LOGIN")).con.Valor;
                string password = configuration.FirstOrDefault(x => x.con.Opcion.ToUpper().Contains("PASSWORD")).con.Valor;
                string url = configuration.FirstOrDefault(x => x.con.Opcion.ToUpper().Contains("URL")).con.Valor;
                string retailid = configuration.FirstOrDefault(x => x.con.Opcion.ToUpper().Contains("RETAILID")).con.Valor;
                JuegaMasViewModel.SuplidorMarlton.Login = login;
                JuegaMasViewModel.SuplidorMarlton.PassWord = password;
                JuegaMasViewModel.SuplidorMarlton.RetailId = retailid;
                JuegaMasViewModel.SuplidorMarlton.Url = url;
                return new
                {
                    OK = true,
                    Err = string.Empty
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.ToString(),
                    Mensaje = "No hay suplidor configurado."
                };
            }
        }

        public static object GetTicketDetalle(int pTransaccionId)
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
                    return new
                    {
                        OK = true,
                        Ticket = ticketViewModel
                    };
                }
                else
                {
                    return new
                    {
                        OK = false,
                        Ticket = false,
                        Mensaje = "No se encuentra el numero de Ticket"
                    };
                }

            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Mensaje = "Fallo la transaccion intente mas tarde",
                    Err = e.ToString()
                };
            }

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

        private static string GeneraFirma(string pFecha, string pHora, string pTicket, List<DataAccess.Tables.DTOs.VP_Transaccion> pJugadas)
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
                                            Convert.ToInt32(Math.Ceiling(item.Ingresos)));
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



        public static object RealizaVenta(int pPrintWidth, int pBancaId, int pUsuarioId, int pRiferoId, string pBanca, string pDireccion, ProductosRepository.ProductoViewModel pProducto, string pFood, string pHead)
        {
            try
            {
                var pmCuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, DbEnums.Productos.Bingo.ToString(), pBancaId,3);
                if (pmCuenta == null)
                {
                    return new
                    {
                        OK = false,
                        Mensaje = "Cuenta no configurada",
                        Err = "La Cuenta no esta configurada para este Rifero"
                    };
                }
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.Bingo);
                var balanceCliente = DataAccess.EFRepositories.VpCuentaRepository.GetBalanceCuentaMAR(pRiferoId, DbEnums.Productos.DepositoProductoMar);
                if (pProducto.Monto > balanceCliente)
                {
                    return new
                    {
                        OK = false,
                        Mensaje = "Su Cuenta ProductosMAR NO cuenta con suficiente balance",
                        Err = "Su Cuenta ProductosMAR NO cuenta con suficiente balance",
                    };
                }



                //agregando transaccion de Bingo

                var detalle =  new JuegaMasViewModel.JugadaJuegaMAS { Monto = pProducto.Monto, Cantidad = 1, Jugada = "" };
                var transDetalle = new List<JuegaMasViewModel.JugadaJuegaMAS>();
                transDetalle.Add(detalle);


                var transaccionDetalles = MapObjectsToVpTransaccionDetalles(transDetalle, pProducto.ProductoID, pProducto.ProductoCampos);
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasBingo.ReferenciaBingo.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasBingo.ReferenciaBingo.ToString()
                });
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasBingo.Serial.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasBingo.Serial.ToString()
                });
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasBingo.HoraSorteo.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasBingo.HoraSorteo.ToString()
                });
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasBingo.CartonBingo.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasBingo.CartonBingo.ToString()
                });
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasBingo.FechaSorteo.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasBingo.FechaSorteo.ToString()
                });
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasBingo.NumeroSorteo.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasBingo.NumeroSorteo.ToString()
                });
                transaccionDetalles.Add(new VP_TransaccionDetalle
                {
                    ProductoCampoID = (pProducto.ProductoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasBingo.Autorizacion.ToString()).ProductoCampoID),
                    Referencia = DbEnums.VP_TransaccionesDestallesReferenciasBingo.Autorizacion.ToString()
                });

                var trans = VpTransacciones.AgregarTransaccion(pBancaId, pProducto.SuplidorID, pProducto.ProductoID, pProducto.Cuenta.CuentaID,
                        decimal.Parse(pProducto.Monto.ToString()), 0, pProducto.Referencia , pUsuarioId, transaccionDetalles);




                var jugadas = new List<JuegosNuevosRequestModel.ClienteJugadas>();

                jugadas.Add(new JuegosNuevosRequestModel.ClienteJugadas { Nombre = pProducto.Nombre, CodigoJuego = pProducto.Referencia, Jugada = "", Monto = int.Parse(pProducto.Monto.ToString()) });


                var loteriaParams = new LoteriaServicioParametros
                {
                    Ticket = new LoteriaServicioParametros.LoteriaTicket
                    {
                        CedulaDelCajero = "123",
                        CedulaDelSupervisor = "123",
                        CodigoDeAgencia = pBancaId.ToString(),
                        CodigoDeCaja = pBancaId.ToString(),
                        CodigoDeConsorcio = pmCuenta.PMCuenta.CueComercio,
                        Fecha = DateTime.Now,
                        NombreDeAgencia = pBanca,
                        NombreDelSupervisor = "XXX",
                        NombreDelCajero = pUsuarioId.ToString(),
                        NumeroDeTicket = trans.TransaccionID.ToString(),
                        MontoJugada =  Convert.ToInt32(pProducto.Monto).ToString()
                    },
                    EndPointAddress = pmCuenta.SWProducto.URL
                };
                var auth = AutorizaJugada(new JuegosNuevosRequestModel.ClienteDatosSuplidos { Consorcio = pmCuenta.PMCuenta.CueComercio, Password = pmCuenta.PMCuenta.CueServidor, Usuario = pmCuenta.PMCuenta.RecargaID, Jugadas = jugadas }, loteriaParams);

                if (!ErrorRespuesta(int.Parse(auth.CodResp)))
                {
                    trans = AgregaLotteryVipToVPTransaccion(trans, auth);

                    var ticketViewModel = VpTransacciones.ActualizaTransaccion(trans);

                    MAR.DataAccess.EFRepositories.VpCuentaRepository.UpdateBalanceCuentaMAR(-pProducto.Monto);

                    var transacciones = new List<VP_Transaccion>();
                    transacciones.Add(trans);
                    string firma = GeneraFirma(trans.FechaIngreso.ToShortDateString(), trans.FechaIngreso.ToString("t"), trans.Referencia, transacciones);
                    var printData = PrintJobs.JuegosNuevosPrintJob.ImprimirBingo(pPrintWidth, pBanca, pDireccion,  trans.Referencia, BaseViewModel.GeneraPinGanador(trans.TransaccionID), false, ticketViewModel, pFood, pHead, pBancaId, firma,
                            auth.Autorizacion, auth.CartaBingo, pProducto.Nombre);







                    return new
                    {
                        OK = true,
                        PrintData = printData,
                        Mensaje = "Exitosa",
                      //  Bingo = ticketViewModel
                    };
                }



                return new
                {
                    OK = false,
                    Mensaje = auth.DescResp
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.ToString(),
                    Mensaje = "Fallo la transaccion intente mas tarde"
                };
            }
        }


        private static bool ErrorRespuesta(int pCod)
        {
            if (pCod == (int)ProductosExternosEnums.JuegosNuevosStatus.OperacionSatisFactoria)
            {
                return false;
            }
            return true;
        }

        private static List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego> ListameDetalleDeJuegos(List<JuegosNuevosRequestModel.ClienteJugadas> j)
        {

            List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego> d = new List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego>();

            foreach (var x in j)
            {

                d.Add(

                    new LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego()
                    {

                        Codigo = x.CodigoJuego,
                        Monto = Convert.ToString(x.Monto),
                        Jugada = x.Jugada
                    }
                );

            }
            return d;

        }//End ListameDetalleDeJuegos()~


        public static DataAccess.ViewModels.Mappers.LotteryVIPMapper.LotteryVIP_Response AutorizaJugada(JuegosNuevosRequestModel.ClienteDatosSuplidos pClienteDatos, LoteriaServicioParametros pLoteriaServicio)                  /////////////////////La salida puede ser solo en XML| chequiar => JSON
        {
            List<LoteriaServicioParametros.LoteriaTicket.DetalleDeJuego> detalleJugadas = ListameDetalleDeJuegos(pClienteDatos.Jugadas);
            pLoteriaServicio.Consorcio = pClienteDatos.Consorcio;
            pLoteriaServicio.Usuario = pClienteDatos.Usuario;
            pLoteriaServicio.Password = pClienteDatos.Password;
            pLoteriaServicio.Ticket.Juegos = detalleJugadas;
            return LotteryVIPMapper.AutorizaJugada(pLoteriaServicio, LotteryVIPMapper.SalidaEn.XML);
        }//End AutorizaJugada()~

        public static object ResultadoSorteos(string pReferenciaBingo, int pRiferoId)                                                  /////////////////////La salida es en XML
        {

            try
            {
                var pagado = DataAccess.EFRepositories.VpTransacciones.PremioYaPagado(pReferenciaBingo, DbEnums.Productos.BingoGanador);
                if (pagado)
                {
                    return new
                    {
                        OK = false,
                        Err = "NO PAGUE, Premio ya pagado anteriormente.",
                        Mensaje = "NO PAGUE, Premio ya pagado anteriormente.",
                    };
                }

                var pmCuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, DbEnums.Productos.Bingo.ToString(), 0,3);


                var authPago = ConsultaGanadorBingoPorTicket(new LoteriaServicioParametros
                {
                    Consorcio = pmCuenta.PMCuenta.CueComercio,
                    Password = pmCuenta.PMCuenta.CueServidor,
                    Usuario = pmCuenta.PMCuenta.RecargaID,
                    EndPointAddress = pmCuenta.SWProducto.URL
                },pReferenciaBingo);

                // dependiendo el resultado voy a marcar el ticket con Saco
                if (!ErrorRespuesta(int.Parse(authPago.CodResp)))
                {
                    return new
                    {
                        OK = true,
                        ConsultaPago = authPago.Ganador.FirstOrDefault()
                    };
                }
                else
                {
                    return new { OK = false, Mensaje = authPago.DescResp, Err = authPago.DescResp };
                }
            }
            catch (Exception e)
            {

                return new
                {
                    OK = false,
                    Err = e.Message,
                    Mensaje = e.Message
                };
            }





        }//End ResultadoSorteos()~

        private static LotteryVIPMapper.Bingo_Response ConsultaGanadorBingoPorTicket(LoteriaServicioParametros pLoteriaServicio, string pReferenciaBingo)                                 /////////////////////La salida puede  ser en XML|JSON
        {
            return LotteryVIPMapper.GanadorBingoPorTicket(pLoteriaServicio, LotteryVIPMapper.SalidaEn.XML, pReferenciaBingo);

        }//End AnulaJugada()~

        public static LotteryVIPMapper.Bingo_Response PagoGanadorBingo(LoteriaServicioParametros pLoteriaServicio, string pReferenciaBingo, double pMonto)                                 /////////////////////La salida puede  ser en XML|JSON
        {
            return LotteryVIPMapper.PagoGanadorBingo(pLoteriaServicio, LotteryVIPMapper.SalidaEn.XML, pReferenciaBingo, pMonto);

        }
        public static object RealizarPago_Bingo(int pBancaId, string pReferenciaBingo,  int pUsuarioId, double pMonto, int pPrintWidth, string pBanca, string pDireccion, int pRiferoId)
        {
            try
            {
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.BingoGanador);
                if (pReferenciaBingo != null)
                {
                    if (pMonto < 0)
                    {
                        pMonto = 0;
                    }



                    var pmCuenta = DataAccess.EFRepositories.PMCuentasRepository.GetPMCuenta(pRiferoId, DbEnums.Productos.Bingo.ToString(), 0,3);

                    var authPago = PagoGanadorBingo(new LoteriaServicioParametros
                    {
                        Consorcio = pmCuenta.PMCuenta.CueComercio,
                        Password = pmCuenta.PMCuenta.CueServidor,
                        Usuario = pmCuenta.PMCuenta.RecargaID,
                        EndPointAddress = pmCuenta.SWProducto.URL
                    },pReferenciaBingo, pMonto);


                    if (!ErrorRespuesta(int.Parse(authPago.CodResp)))

                    {
                        var transGanador = DataAccess.EFRepositories.VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                            0, decimal.Parse(pMonto.ToString()), DbEnums.Productos.BingoGanador.ToString(), pUsuarioId, null);
                        transGanador.Activo = true;
                        transGanador.Estado = authPago.DescResp;
                        transGanador.Referencia = pReferenciaBingo;
                        VpTransacciones.ActualizaTransaccion(transGanador);
                        string mensaje = "El pago de $" + pMonto.ToString("N2") + " para el ticket ganador numero " + pReferenciaBingo + " ha sido aprobado.";
                        return new
                        {
                            OK = true,
                            Mensaje = "Ticket pagado exitosamente",
                            PrintData = JuegaMasPrintJob.ImprimirPagoGanador(pBanca, pPrintWidth, mensaje, transGanador.TransaccionID, pDireccion),
                            Err = string.Empty,
                        };
                    }
                    else
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = authPago.DescResp,
                            Err = authPago.DescResp
                        };
                    }
                }
                return new
                {
                    OK = false,
                    Mensaje = "No se encuentra el numero de Ticket"
                };

            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.ToString(),
                    Mensaje = "Ocurrio un error procesando la transaccion"
                };
            }
        }
        public static object ReimprimirTicket(int pTransaccionId, int pPrintWidth, int pBancaId, int pUsuarioId, int pRiferoId, string pBanca, string pDireccion)
        {
            if (VpTransacciones.PuedeReimprimir(pTransaccionId))
            {

                return new
                {
                    OK = true,
                    PrintData = new List<string[]>(), // JuegaMasPrintJob.ImprimirVentaJuegaMas(pPrintWidth, ticketMarlton, ticketViewModel, pBanca, pDireccion, pin),
                    Err = string.Empty
                };
            }
            else
            {
                return new
                {
                    OK = false,
                    Mensaje = "No puede Reimprimir este Ticket",
                    Err = "No puede Reimprimir este Ticket"
                };
            }

        }

        private static bool ErrorRespuestas(int pCod)
        {
            if (pCod == (int)ProductosExternosEnums.JuegaMasMarltonStatus.Exitoso)
            {
                return false;
            }
            return Enum.GetValues(typeof(ProductosExternosEnums.JuegaMasMarltonStatus)).Cast<ProductosExternosEnums.JuegaMasMarltonStatus>().Any(item => pCod == (int)item);
        }

    }
}
