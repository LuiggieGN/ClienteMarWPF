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

namespace MAR.BusinessLogic.Code
{
    public class JuegaMasIndexLogic
    {
        public static object GetTicket_VendidosHoy(int pRiferoId, int pBancaId)
        {
            DateTime desde = DateTime.Today;
            DateTime hasta = DateTime.Today.AddDays(1);
            try
            {
                var cuentaId = DataAccess.EFRepositories.VpCuentaRepository.GetVpCuenta(pRiferoId,
                    DbEnums.Productos.JuegaMas).VpCuenta.CuentaID;
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
            return  ganadores;
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
                    if (productoCampos.Any(x => x.Nombre.ToUpper() == c.Name.ToUpper() && x.Activo))
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
        private static VP_Transaccion AgregaMartonToVPTransaccion(VP_Transaccion pTransaccion, JuegaMasViewModel.MarltonResponse pTMarltonResponse)
        {
            pTransaccion.Referencia = pTMarltonResponse.Serial;
            pTransaccion.Estado = pTMarltonResponse.status.Message;
            pTransaccion.Activo = true;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString()).ValorText = pTMarltonResponse.TicketNo;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString()).ValorText = pTMarltonResponse.Hora_Sorteo;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString()).ValorText = pTMarltonResponse.Numero_Sorteo.ToString();
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString()).ValorText = pTMarltonResponse.Fecha_Sorteo;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString()).ValorText = pTMarltonResponse.Serial;
            pTransaccion.VP_TransaccionDetalle.FirstOrDefault(x => x.Referencia == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString()).ValorText = pTMarltonResponse.Ticket_Control;
            return pTransaccion;
        }
        public static object RealizarVenta_JuegaMasNueva(int pPrintWidth, DTicket pDTicket, int pBancaId, int pUsuarioId, int pLastSolicitud, int pRiferoId, string pBanca, string pDireccion)
        {
            var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.JuegaMas);

            if (pDTicket.DTicketDetalles.Any())
            {
                try
                {
                    //agregando transaccion de JuegaMas
                    var transDetalle = from p in pDTicket.DTicketDetalles
                                       select new JuegaMasViewModel.JugadaJuegaMAS()
                                       {
                                           Jugada = p.TDeNumero,
                                           Cantidad = p.TDeCantidad,
                                           Monto = double.Parse(p.TDeCosto.ToString())
                                       };
                    var productoCampos = ProductosRepository.GetVpProductoCampos(x => x.ProductoID == cuenta.ProductoId);
                    var transaccionDetalles = MapObjectsToVpTransaccionDetalles(transDetalle, cuenta.ProductoId, productoCampos);
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString()
                    });

                    var trans = VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                        pDTicket.TicCosto, 0, DbEnums.Productos.JuegaMas.ToString(), pUsuarioId, transaccionDetalles);

                    var ticketMarlton = JuegaMasViewModel.MarltonResponse.EnviarJugada_JuegaMas(trans);

                    if (JuegaMasIndexLogic.ErrorRespuestas(int.Parse(ticketMarlton.status.Code.ToString())))

                        return new
                        {
                            OK = false,
                            Err = ticketMarlton.status.Message,
                            Mensaje = ticketMarlton.status.Message,
                        };
                    if (ticketMarlton.Serial == "00000" && ticketMarlton.detalle == null)
                        return new
                        {
                            OK = false,
                            Err = "Fallo la transaccion intente mas tarde",
                            Mensaje = "Fallo la transaccion intente mas tarde",
                        };
                    if ((ticketMarlton.detalle == null || ticketMarlton.detalle.Count() < 1))
                    {
                        try
                        {
                            trans = AgregaMartonToVPTransaccion(trans, ticketMarlton);
                            var ticketViewModel = JuegaMasViewModel.MapFromTransaccion(VpTransacciones.ActualizaTransaccion(trans), true);
                            return new
                            {
                                OK = true,
                                Err = string.Empty,
                                Mensaje = ticketMarlton.status.Message,
                                Ticket = ticketViewModel,
                                PrintData = JuegaMasPrintJob.ImprimirVentaJuegaMas(pPrintWidth, ticketMarlton, ticketViewModel, pBanca, pDireccion)
                            };
                        }
                        catch (Exception e)
                        {
                            JuegaMasViewModel.MarltonResponse.Exec_CancelarJuegaMas(ticketMarlton.Serial, pBancaId);
                            return new
                            {
                                OK = false,
                                Err = e.ToString(),
                                Mensaje = "Fallo la transaccion intente mas tarde"
                            };
                        }
                    }
                    return new
                    {
                        OK = true,
                        Err = string.Empty,
                        Mensaje = ticketMarlton.status.Message,
                        JugadaMarlton = ticketMarlton
                    };
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
            return new
            {
                OK = false,
                Err = "No existen Jugadas"
            };
        }
        public static object RealizarVenta_JuegaMasNuevaFree(int pPrintWidth, DTicket pDTicket, int pBancaId, int pUsuarioId, int pLastSolicitud, int pRiferoId, string pBanca, string pDireccion, string pSerialTF)
        {

            var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.JuegaMas);

            if (pDTicket.DTicketDetalles.Any())
            {
                try
                {
                    //agregando transaccion de JuegaMas
                    var transDetalle = from p in pDTicket.DTicketDetalles
                                       select new JuegaMasViewModel.JugadaJuegaMAS()
                                       {
                                           Jugada = p.TDeNumero,
                                           Cantidad = p.TDeCantidad,
                                           Monto = 0
                                       };
                    var productoCampos = ProductosRepository.GetVpProductoCampos(x => x.ProductoID == cuenta.ProductoId);
                    var transaccionDetalles = MapObjectsToVpTransaccionDetalles(transDetalle, cuenta.ProductoId, productoCampos);
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString()
                    });

                    var trans = VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                        0, 0, DbEnums.Productos.JuegaMas.ToString(), pUsuarioId, transaccionDetalles);
                    trans.Referencia = pSerialTF;
                    var ticketMarlton = JuegaMasViewModel.MarltonResponse.EnviarJugada_JuegaMasFree(trans);

                    if (JuegaMasIndexLogic.ErrorRespuestas(int.Parse(ticketMarlton.status.Code.ToString())))

                        return new
                        {
                            OK = false,
                            Err = ticketMarlton.status.Message,
                            Mensaje = ticketMarlton.status.Message,
                        };
                    if (ticketMarlton.Serial == "00000" && ticketMarlton.detalle == null)
                        return new
                        {
                            OK = false,
                            Err = "Fallo la transaccion intente mas tarde",
                            Mensaje = "Fallo la transaccion intente mas tarde",
                        };
                    if ((ticketMarlton.detalle == null || ticketMarlton.detalle.Count() < 1))
                    {
                        try
                        {
                            trans = AgregaMartonToVPTransaccion(trans, ticketMarlton);
                            var ticketViewModel = JuegaMasViewModel.MapFromTransaccion(VpTransacciones.ActualizaTransaccion(trans), true);
                            return new
                            {
                                OK = true,
                                Err = string.Empty,
                                Mensaje = ticketMarlton.status.Message,
                                Ticket = ticketViewModel,
                                JugadaMarlton = ticketMarlton,
                                PrintData = JuegaMasPrintJob.ImprimirVentaJuegaMas(pPrintWidth, ticketMarlton, ticketViewModel, pBanca, pDireccion)
                            };
                        }
                        catch (Exception e)
                        {
                            JuegaMasViewModel.MarltonResponse.Exec_CancelarJuegaMas(ticketMarlton.Serial, pBancaId);
                            return new
                            {
                                OK = false,
                                Err = e.ToString(),
                                Mensaje = "Fallo la transaccion intente mas tarde"
                            };
                        }
                    }
                    return new
                    {
                        OK = true,
                        Err = string.Empty,
                        Mensaje = ticketMarlton.status.Message,
                        JugadaMarlton = ticketMarlton
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
            return new
            {
                OK = false,
                Err = "No existen Jugadas"
            };
        }
        public static object JugarQuickPick_Marlton(int pPrintWidth, int pBancaId, int pUsuarioId, int pRiferoId, string pBanca, string pDireccion)
        {
            try
            {
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.JuegaMas);

                var quickpick = JuegaMasViewModel.MarltonResponse.Exec_QuickPickMarlton(pBancaId);
                if (quickpick.Monto != null && quickpick.Serial != "00000")
                {
                    //agregando transaccion de JuegaMas
                    var transDetalle = new JuegaMasViewModel.JugadaJuegaMAS()
                    {
                        Jugada = quickpick.BilleteQP,
                        Cantidad = 1,
                        Monto = double.Parse(quickpick.Monto.ToString())
                    };
                    var productoCampos = ProductosRepository.GetVpProductoCampos(x => x.ProductoID == cuenta.ProductoId);
                    var transaccionDetalles = MapObjectsToVpTransaccionDetalles(new[] { transDetalle }, cuenta.ProductoId, productoCampos);

                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketMarlton.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.Serial.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.HoraSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.NumeroSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.FechaSorteo.ToString()
                    });
                    transaccionDetalles.Add(new VP_TransaccionDetalle
                    {
                        ProductoCampoID = (productoCampos.FirstOrDefault(x => x.Nombre == DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString()).ProductoCampoID),
                        Referencia = DbEnums.VP_TransaccionesDestallesReferenciasJuegaMas.TicketControl.ToString()
                    });

                    //agregando transaccion de billetes
                    var trans = VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                        decimal.Parse(quickpick.Monto.ToString()), 0, DbEnums.Productos.JuegaMas.ToString(), pUsuarioId, transaccionDetalles);


                    try
                    {
                        trans = AgregaMartonToVPTransaccion(trans, quickpick);
                        var ticketViewModel = JuegaMasViewModel.MapFromTransaccion(VpTransacciones.ActualizaTransaccion(trans), true);
                        return new
                        {
                            OK = true,
                            QuickPick = quickpick,
                            Ticket = ticketViewModel,
                            Err = string.Empty,
                            PrintData = JuegaMasPrintJob.ImprimirVentaJuegaMas(pPrintWidth, quickpick, ticketViewModel, pBanca, pDireccion)
                        };
                    }
                    catch (Exception e)
                    {
                        JuegaMasViewModel.MarltonResponse.Exec_CancelarJuegaMas(quickpick.Serial, pBancaId);
                        return new
                        {
                            OK = false,
                            Err = e.ToString(),
                            Mensaje = "Fallo la transaccion intente mas tarde"
                        };
                    }
                }
                return new
                {
                    OK = false,
                    Mensaje = "Fallo la transaccion intente mas tarde"
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
        public static object CancelarJuegaMas(int pTransaccionId, string pPagoPin, int pBancaId)
        {
            try
            {
                var trans = DataAccess.EFRepositories.VpTransacciones.GetTransacciones(x => x.TransaccionID == pTransaccionId).FirstOrDefault();
                if (pBancaId  > 0)
                {
                    var bancaAnula = DataAccess.EFRepositories.BancasRepository.GetBanca(x => x.BancaID == pBancaId).FirstOrDefault().BanAnula;
                    if (!bancaAnula)
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "Esta Banca NO tiene permisos para ANULAR"
                        };
                    }
                }
              
                string msj = "Transaccion sin respuesta";
                if (trans != null)
                {
                    string serial = trans.Referencia;
                    if (!BaseViewModel.ComparaPinGanador(trans.TransaccionID, pPagoPin))
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "El pin no corresponde con el ticket que usted desea anular."
                        };
                    }
                    var cancelaBilleteMarlton = JuegaMasViewModel.MarltonResponse.Exec_CancelarJuegaMas(serial, pBancaId);
                     msj = cancelaBilleteMarlton.status.Message;
                    if (JuegaMasIndexLogic.ErrorRespuestas(int.Parse(cancelaBilleteMarlton.status.Code.ToString())))
                    {
                        return new
                        {
                            OK = true,
                            Mensaje = msj
                        };
                    }
                    else
                    {
                        trans.Estado = cancelaBilleteMarlton.status.StatusMessage;
                        trans.Activo = false;
                        DataAccess.EFRepositories.VpTransacciones.ActualizaTransaccion(trans);
                        return new
                        {
                            OK = true,
                            Mensaje = msj
                        };
                    }
                }
                return new
                {
                    OK = false,
                    Mensaje = msj
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.ToString(),
                    Mensaje = "Ocurrio un error procesando la transaccion."
                };
            }
        }
    
        public static object ValidarJuegaMas(int pTransaccionId, int pBancaId, string pSerial)
        {
            try
            {
                if (pSerial != null)
                {
                    var juegaMas = JuegaMasViewModel.MarltonResponse.Exec_ValidarJuegaMas(pSerial, pBancaId);
                    if (JuegaMasIndexLogic.ErrorRespuestas(int.Parse(juegaMas.status.Code.ToString())))
                    {
                        return new
                        {
                            OK = false,
                            Err = juegaMas.status.Message,
                            Mensaje = juegaMas.status.Message,
                        };
                    }
                    decimal? montoSinMontoFree = juegaMas.Monto - juegaMas.monto_jugada_Free;
                    //probando DESCOMENTAR Y PONER EL SERIAL 299098552 o 571431517
                    //return new
                    //{
                    //    OK = true,
                    //    Err = string.Empty,
                    //    Aprobado = true,
                    //    Monto = juegaMasEstado.Monto,
                    //    Mensaje = juegaMasEstado.statusTicket + " " + juegaMasEstado.Monto,
                    //    TicketFree = juegaMasEstado.TicketFree
                    //};

                    //JuegaMas ganador NO PAGADO
                    if ((juegaMas.detalle.Any() && juegaMas.Estadopago == false && juegaMas.monpag <= 0) || (juegaMas.detalle.Any() && juegaMas.Estadopago && juegaMas.TicketFree && !juegaMas.Jugado_Free))
                    {
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = true,
                            Monto = montoSinMontoFree,
                            Mensaje = juegaMas.statusTicket + " " + montoSinMontoFree,
                            TicketFree = juegaMas.TicketFree,
                            Jugada = juegaMas,
                            Estado = ProductosExternosEnums.JuegaMasMarltonStatus.TieneTicketFreePorJugar
                        };
                    }

                    //JuegaMas ganador YA PAGADO
                    if (juegaMas.Estadopago)
                    {
                        string msgPagado = "Ya este ticket fue registrado como PAGADO en fecha " + juegaMas.fechaPagado + " con un monto de: " + juegaMas.Monto;
                        if (juegaMas.monto_jugada_Free > 0)
                        {
                            msgPagado = "Ya este ticket fue PAGADO en fecha " + juegaMas.fechaPagado + " con un monto efectivo de: " + juegaMas.monpag + " y " + juegaMas.Num_Jug_Free + " Jugadas Gratis que ya fueron usadas.";
                        }
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = false,
                            Monto = juegaMas.Monto,
                            Mensaje = msgPagado,
                            Estado = ProductosExternosEnums.JuegaMasMarltonStatus.TicketPagoadoNadaPendiente
                        };
                    }

                    //JuegaMas esperando sorteo
                    if (!juegaMas.Sorteo_efectuado)
                    {
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = false,
                            Mensaje = " El sorteo no ha pasado",
                            Estado = ProductosExternosEnums.JuegaMasMarltonStatus.EsperandoSorteo
                        };
                    }

                    //JuegaMas NO Ganador
                    if (!juegaMas.detalle.Any() && !juegaMas.Estadopago && juegaMas.monpag == 0 && juegaMas.Sorteo_efectuado)
                    {
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = false,
                            Mensaje = " NO resulto ganador en el sorteo correspondiente.",
                            Estado = ProductosExternosEnums.JuegaMasMarltonStatus.TicketNoGanador
                        };
                    }
                }
                return new
                {
                    OK = true,
                    Mensaje = "No se encuentra ese numero de ticket",
                    Aprobado = false,
                    Estado = ProductosExternosEnums.JuegaMasMarltonStatus.TicketNoEncontrado
                };
            }
            catch (Exception e)
            {

                return new
                {
                    OK = false,
                    Mensaje = "La consulta fallo en el servidor",
                    Err = e.ToString()
                };
            }
        }
        public static object RealizarPago_JuegaMas(int pBancaId, int pTicNumero, string pSerial, int pUsuarioId, decimal pMonto, int pPrintWidth, string pBanca, string pDireccion, int pRiferoId)
        {
            try
            {
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.JuegaMasGanador);
                if (pSerial != null)
                {
                    if (pMonto < 0)
                    {
                        pMonto = 0;
                    }
                    var pagoJuegaMas = JuegaMasViewModel.MarltonResponse.Exec_PagarJuegaMas(pSerial, pBancaId);
                    if (JuegaMasIndexLogic.ErrorRespuestas(int.Parse(pagoJuegaMas.status.Code.ToString())))
                    {
                        return new
                        {
                            OK = false,
                            Err = pagoJuegaMas.status.Message,
                            Mensaje = pagoJuegaMas.status.Message,
                        };
                    }
                    if (pagoJuegaMas.status.Code == (int)ProductosExternosEnums.JuegaMasMarltonStatus.Exitoso)
                    {
                        var transGanador = DataAccess.EFRepositories.VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                            0, pMonto, DbEnums.Productos.JuegaMasGanador.ToString(), pUsuarioId, null);
                        transGanador.Activo = true;
                        transGanador.Estado = "Exitoso";
                        transGanador.Referencia = pSerial;
                        VpTransacciones.ActualizaTransaccion(transGanador);
                        string mensaje = "El pago de $" + pMonto.ToString("N2") + " para el ticket ganador numero " + pTicNumero.ToString() + " ha sido aprobado.";
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
                            OK = true,
                            Mensaje = pagoJuegaMas.status.Message
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
        public static void RegistraBancaEnMarlton(int pBancaId)
        {
            try
            {
                var registraBanca = "desactivado JuegaMas"; //JuegaMasViewModel.MarltonResponse.Exec_RegistraBancaJuegaMas(pBancaId);
                //DataAccess.EFRepositories.BancasRepository.AgragaOActualizaBacanConfigRecord(pBancaId, DbEnums.BancaConfigKeyEnum.BANCA_JUEGAMAS_FECHA_REGISTRADA_EN_MARLTON.ToString()
                //    , DateTime.Now.ToString(), true);
            }
            catch (Exception e)
            {
                //ignore
            }
        }
        public static bool IsBancaLocationPendiente(int pBancaId)
        {
            return false;
            //var bancaRegistrada = DataAccess.EFRepositories.BancasRepository.GetBancaConfig(x => x.BancaID == pBancaId && x.ConfigKey.ToUpper() ==
            //DataAccess.Tables.Enums.DbEnums.BancaConfigKeyEnum.BANCA_JUEGAMAS_FECHA_REGISTRADA_EN_MARLTON.ToString());
            //if (bancaRegistrada.Any())
            //{
            //    var fechaRegistrada = DateTime.Parse(bancaRegistrada.FirstOrDefault().ConfigValue);
            //    if (fechaRegistrada >= DateTime.Today.Date)
            //    {
            //        return false;
            //    }
            //}
            //return true;
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
