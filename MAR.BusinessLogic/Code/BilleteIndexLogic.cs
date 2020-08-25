using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using MAR.BusinessLogic.Code.PrintJobs;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.DataAccess.Tables.ViewModels;
using MAR.DataAccess.UnitOfWork;
using Newtonsoft.Json;
using MAR.DataAccess.ViewModels;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;



namespace MAR.BusinessLogic.Code
{
    public class BilleteIndexLogic
    {
        public static object GetTicket_VendidosHoy()
        {
            DateTime desde = DateTime.Today;
            DateTime hasta = DateTime.Today.AddDays(1);
            UnitOfWork db = new UnitOfWork(new MARContext());
            int loteriaId =
                DataAccess.EFRepositories.LoteriaRepository.GetLoterias(
                    x => x.LotNombre == DbEnums.Productos.Billetes.ToString()).FirstOrDefault().LoteriaID;
            var tickets = db.DTicket.Get(x => x.TicFecha >= desde && x.TicNulo == false && x.LoteriaID == loteriaId, q => q.OrderByDescending(s => s.TicketID), includeProperties: "MBanca,DTicketDetalles,DTicketDetalles.DBilleteDetalles").ToList();

            if (tickets.Any())
            {
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    Tickets = tickets
                };
            }
            return new
            {
                OK = false,
                Err = "Fallo la transaccion intente mas tarde"
            };
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
                BilleteViewModel.SuplidorMarlton.Login = login;
                BilleteViewModel.SuplidorMarlton.PassWord = password;
                BilleteViewModel.SuplidorMarlton.RetailId = retailid;
                BilleteViewModel.SuplidorMarlton.Url = url;
                return new
                {
                    OK = true,
                    Err = string.Empty
                };
            }
            catch (Exception)
            {
                return new
                {
                    OK = false,
                    Err = "No hay suplidor configurado"
                };
            }
        }

        public static object GetTicketDetalle(string pTicketNo)
        {
            UnitOfWork db = new UnitOfWork(new MARContext());

            try
            {
                var theTicket = db.DTicket.Get(x => x.TicNumero == pTicketNo, q => q.OrderByDescending(s => s.TicketID), includeProperties: "DTicketDetalles,DTicketDetalles.DBilleteDetalles").FirstOrDefault();

                if (theTicket != null)
                {
                    var ticketViewModel = BilleteViewModel.MapFromBet(theTicket, true);

                    return new
                    {
                        OK = true,
                        //Err = theTicket.Err,
                        Ticket = ticketViewModel
                    };
                }
                else
                {
                    return new
                    {
                        OK = true,
                        Ticket = false,
                        Mensaje = "No se encuentra el numero de Ticket"
                    };
                }

            }
            catch
            {

                return new
                {
                    OK = false,
                    Mensaje = "",
                };
            }

        }

        public static object RealizarVenta_Billete(int pPrintWidth, DTicket pDTicket, int pBancaId, int pUsuarioId, int pLastSolicitud, int pRiferoId, string pBanca, string pDireccion)
        {
            UnitOfWork unitOfWork = new UnitOfWork(new MARContext());
            MARContext marContext = new MARContext();

            var cuenta = DataAccess.EFRepositories.VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.Billetes);
            int loteriaId =
              DataAccess.EFRepositories.LoteriaRepository.GetLoterias(
                  x => x.LotNombre == DbEnums.Productos.Billetes.ToString()).FirstOrDefault().LoteriaID;
            if (pDTicket.DTicketDetalles.Any())
            {
                try
                {
                    //agregando transaccion de billetes
                    var trans = DataAccess.EFRepositories.VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                        pDTicket.TicCosto, 0, DbEnums.Productos.Billetes.ToString(), pUsuarioId, null);

                    var horaRequest = DateTime.Now;
                    var ticketMarlton = BilleteViewModel.MarltonResponse.EnviarJugada_Billete(pDTicket);
                    if (ticketMarlton.status.Code == (int)ProductosExternosEnums.BilleteMarltonStatus.ErrorGeneral)
                        return new
                        {
                            OK = false,
                            Err = "Fallo la transaccion intente mas tarde",
                            Mensaje = "Fallo la transaccion intente mas tarde",
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
                            pDTicket = BilleteViewModel.Map_TicketFromDetalles(
                                  pDTicket.DTicketDetalles.Select(x => new BilleteViewModel.MarltonParameters.Detalle { Serial = ticketMarlton.Serial, Jugada = x.TDeNumero, Monto = x.TDeCosto, Loteria = loteriaId, HoraRequest = horaRequest, HoraResponse = DateTime.Now, NumeroSorteo = (int)ticketMarlton.Numero_Sorteo }).ToList(), pBancaId, pUsuarioId, pRiferoId);

                            marContext.DTickets.Add(pDTicket);
                            marContext.SaveChanges();

                            var ticketLocal =
                                unitOfWork.DTicket.Get(x => x.TicketID == pDTicket.TicketID, null, "DTicketDetalles")
                                    .FirstOrDefault();
                            string pin = GeneraPinGanador((int)ticketLocal.TicSolicitud);

                            //actualiza transaccion
                            trans.Referencia = ticketMarlton.Serial;
                            trans.Estado = ticketMarlton.status.Message;
                            trans.Activo = true;
                            DataAccess.EFRepositories.VpTransacciones.ActualizaTransaccion(trans);
                            return new
                            {
                                OK = true,
                                Err = string.Empty,
                                Mensaje = ticketMarlton.status.Message,
                                Ticket = ticketLocal,
                                JugadaMarlton = ticketMarlton,
                                PrintData = BilletePrintJob.ImprimirVentaBillete(pPrintWidth, ticketMarlton, ticketLocal, pBanca, pDireccion, pin)
                            };
                        }
                        catch (Exception)
                        {
                            BilleteViewModel.MarltonResponse.Exec_CancelarBillete(ticketMarlton.Serial, pBancaId);
                            return new
                            {
                                OK = false,
                                Err = "Fallo la transaccion intente mas tarde",
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
                catch (Exception)
                {
                    return new
                    {
                        OK = false,
                        Err = "Fallo la transaccion intente mas tarde"
                    };
                }
            }
            return new
            {
                OK = false,
                Err = "No existen Jugadas"
            };
        }

        public static object CancelarBillete(string pTicNumero, string pPagoPin, int pBancaId)
        {
            try
            {
                MARContext db = new MARContext();
                var dTicket = db.DTickets.FirstOrDefault(x => x.TicNumero == pTicNumero);
                if (dTicket != null)
                {
                    string serial = dTicket.TicSolicitud.ToString(CultureInfo.InvariantCulture);
                    if (!BaseViewModel.ComparaPinGanador(int.Parse(serial), pPagoPin))
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "El pin no corresponde con el ticket que usted desea anular."
                        };
                    }
                    dTicket.TicNulo = true;
                    db.SaveChanges();

                    var cancelaBilleteMarlton = BilleteViewModel.MarltonResponse.Exec_CancelarBillete(serial, pBancaId);
                    var trans = DataAccess.EFRepositories.VpTransacciones.GetTransacciones(x => x.Referencia == serial).FirstOrDefault();
                    trans.Estado = cancelaBilleteMarlton.status.StatusMessage;
                    trans.Activo = false;
                    DataAccess.EFRepositories.VpTransacciones.ActualizaTransaccion(trans);
                    return new
                    {
                        OK = true,
                        Mensaje = "Ticket anulado exitosamente"
                    };
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
                    Mensaje = e.Message
                };
            }
        }

        public static object ValidarBillete(string pTicNumero, int pBancaId, string pPin)
        {
            MARContext db = new MARContext();
            try
            {
                var dTicket = db.DTickets.FirstOrDefault(x => x.TicNumero == pTicNumero);
                if (dTicket != null)
                {
                    if (dTicket.TicNulo)
                    {
                        return new
                        {
                            OK = true,
                            Mensaje = "Este numero de ticket fue anulado",
                            Aprobado = false
                        };
                    }

                    string serial = dTicket.TicSolicitud.ToString(CultureInfo.InvariantCulture);
                    if (!BaseViewModel.ComparaPinGanador(int.Parse(serial), pPin))
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "El pin no corresponde con el ticket que usted desea consultar.",
                            Err = "El pin no corresponde con el ticket que usted desea consultar."
                        };
                    }

                    var billeteEstado = BilleteViewModel.MarltonResponse.Exec_ValidarBillete(serial, pBancaId);

                    //probando DESCOMENTAR Y PONER EL SERIAL 1539296443 o 2119766149
                    //return new
                    //{
                    //    OK = true,
                    //    Err = string.Empty,
                    //    Aprobado = true,
                    //    Ticket = dTicket,
                    //    Monto = billeteEstado.Monto,
                    //    Mensaje = billeteEstado.statusTicket + " " + billeteEstado.Monto
                    //};

                    //Billete ganador NO PAGADO
                    if (billeteEstado.detalle.Any() && billeteEstado.Estadopago == false && billeteEstado.monpag == 0)
                    {
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = true,
                            Ticket = dTicket,
                            Monto = billeteEstado.Monto,
                            Mensaje = billeteEstado.statusTicket + " " + billeteEstado.Monto
                        };
                    }

                    //Billete ganador YA PAGADO
                    if (billeteEstado.Estadopago && billeteEstado.monpag > 0)
                    {
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = false,
                            Ticket = dTicket,
                            Monto = billeteEstado.Monto,
                            Mensaje = "Ya este ticket fue registrado como PAGADO en fecha " + billeteEstado.fechaPagado + " con un monto de: " + billeteEstado.monpag
                        };
                    }

                    //Billete esperando sorteo
                    if (!billeteEstado.detalle.Any() && !billeteEstado.Estadopago && billeteEstado.monpag == 0)
                    {
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = false,
                            Mensaje = " El sorteo no ha pasado"
                        };
                    }

                    //Billete NO Ganador
                    if (!billeteEstado.detalle.Any() && !billeteEstado.Estadopago && billeteEstado.monpag == 0)
                    {
                        return new
                        {
                            OK = true,
                            Err = string.Empty,
                            Aprobado = false,
                            Mensaje = " NO resulto ganador en el sorteo correspondiente."
                        };
                    }
                }
                return new
                {
                    OK = true,
                    Mensaje = "No se encuentra ese numero de ticket",
                    Aprobado = false
                };
            }
            catch (Exception e)
            {

                return new
                {
                    OK = false,
                    Mensaje = e.Message
                };
            }
        }

        public static object RealizarPago_Billete(int pBancaId, string pTicNumero, string pPin, string pUsuario, decimal pMonto, int pPrintWidth, string pBanca, string pDireccion)
        {
            try
            {
                var db = new MARContext();
                var ticketEditar = (from d in db.DTickets
                                    join b in db.MBancas on d.BancaID equals b.BancaID
                                    join l in db.TLoterias on d.LoteriaID equals l.LoteriaID
                                    where l.LotNombre == DbEnums.Productos.JuegaMas.ToString() && !d.TicPagado && d.TicNumero == pTicNumero
                                    select new
                                    {
                                        d,
                                        l,
                                        b
                                    }).FirstOrDefault(); //agregar que al where "!= ticketpagado"
                if (ticketEditar != null)
                {
                    if (!BaseViewModel.ComparaPinGanador(int.Parse(ticketEditar.d.TicSolicitud.ToString(CultureInfo.InvariantCulture)), pPin))
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = "El pin no corresponde con el ticket que usted desea consultar.",
                            Err = "El pin no corresponde con el ticket que usted desea consultar."
                        };
                    }

                    var pagoBillete = BilleteViewModel.MarltonResponse.Exec_PagarBillete(ticketEditar.d.TicSolicitud.ToString(CultureInfo.InvariantCulture), pBancaId);
                    if (pagoBillete.status.Code == (int)ProductosExternosEnums.BilleteMarltonStatus.Exitoso)
                    {
                        ticketEditar.d.TicPagado = true;
                        var pagos = new HPago
                        {
                            PagFecha = DateTime.Now,
                            TicNumero = pTicNumero,
                            PagUsuario = "1",
                            BancaID = pBancaId,
                            BanContacto = ticketEditar.b.BanContacto,
                            PagMonto = pMonto,
                            EdiFecha = DateTime.Now,
                            LoteriaID = ticketEditar.l.LoteriaID
                        };

                        db.HPagos.Add(pagos);
                        db.SaveChanges();
                        string mensaje = "El pago de $" + pMonto.ToString("N2") + " para el ticket ganador numero " + pTicNumero + " ha sido aprobado.";
                        return new
                        {
                            OK = true,
                            Mensaje = "Ticket pagado exitosamente",
                            PrintData = BilletePrintJob.ImprimirPagoGanador(pBanca, pPrintWidth, mensaje, pagos.PagoID, pDireccion),
                            Err = string.Empty,
                        };
                    }
                    else if (pagoBillete.status.Code == (int)ProductosExternosEnums.BilleteMarltonStatus.TicketPagadoAnteriormente)
                    {
                        return new
                        {
                            OK = false,
                            Mensaje = pagoBillete.status.Message,
                            Err = pagoBillete.status.Message,
                        };
                    }
                    else
                    {
                        return new
                        {
                            OK = true,
                            Mensaje = pagoBillete.status.Message
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
                    Mensaje = e.Message
                };
            }
        }

        public static object JugarQuickPick_Marlton(int pPrintWidth, int pBancaId, int pUsuarioId, int pRiferoId, string pBanca, string pDireccion)
        {
            try
            {

                var detallesList = new List<BilleteViewModel.MarltonParameters.Detalle>();
                var horaRequest = DateTime.Now;
                var cuenta = DataAccess.EFRepositories.VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.Billetes);
                var loteria =
                    DataAccess.EFRepositories.LoteriaRepository.GetLoterias(
                        x => x.LotNombre == DbEnums.Productos.Billetes.ToString()).FirstOrDefault();
                if (loteria == null)
                {
                    return null;
                }

                var quickpick = BilleteViewModel.MarltonResponse.Exec_QuickPickMarlton(pBancaId);
                if (quickpick.Monto != null && quickpick.Serial != "00000")
                {
                    //agregando transaccion de billetes
                    var trans = DataAccess.EFRepositories.VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                       decimal.Parse(quickpick.Monto.ToString()),0, DbEnums.Productos.Billetes.ToString(), pUsuarioId, null);

                    detallesList.Add(new BilleteViewModel.MarltonParameters.Detalle
                    {
                        Jugada = quickpick.BilleteQP,
                        Monto = (decimal)quickpick.Monto,
                        Loteria = loteria.LoteriaID,
                        HoraRequest = horaRequest,
                        HoraResponse = DateTime.Now,
                        Serial = quickpick.Serial,
                        NumeroSorteo = (int)quickpick.Numero_Sorteo
                    });
                    try
                    {
                        UnitOfWork unitOfWork = new UnitOfWork(new MARContext());
                        var dTicket = BilleteViewModel.Map_TicketFromDetalles(detallesList, pBancaId, pUsuarioId, pRiferoId);
                        var db = new MARContext();
                        db.DTickets.Add(dTicket);
                        db.SaveChanges();

                        //A specified Include path is not valid. The EntityType 'DATA000Model.DTicket' does not declare a navigation property with the name 'DTicketDetalles'.

                        var ticketLocal = unitOfWork.DTicket.Get(x => x.TicketID == dTicket.TicketID, null, "DTicketDetalles").FirstOrDefault();
                        string pin = GeneraPinGanador((int)ticketLocal.TicSolicitud);

                        //actualiza transaccion
                        trans.Referencia = quickpick.Serial;
                        trans.Estado = quickpick.status.Message;
                        trans.Activo = true;
                        DataAccess.EFRepositories.VpTransacciones.ActualizaTransaccion(trans);
                        return new
                        {
                            OK = true,
                            QuickPick = quickpick,
                            Ticket = ticketLocal,
                            Err = string.Empty,
                            PrintData = BilletePrintJob.ImprimirVentaBillete(pPrintWidth, quickpick, ticketLocal, pBanca, pDireccion, pin)
                        };
                    }
                    catch (Exception)
                    {
                        BilleteViewModel.MarltonResponse.Exec_CancelarBillete(quickpick.Serial, pBancaId);
                        return null;
                    }
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static object JugarQuickPick_Mar(int pPrintWidth, int pBancaId, int pCantidad, int pFracciones, int pUsuarioId,
            int pRiferoId, string pBanca, string pDireccion)
        {
            try
            {
                var ticketMarlton = new BilleteViewModel.MarltonResponse();
                var detallesList = new List<BilleteViewModel.MarltonParameters.Detalle>();
                int cantidadQp = pCantidad;
                while (cantidadQp > 0)
                {
                    var horaRequest = DateTime.Now;
                    var parameters = BilleteViewModel.MarltonParameters.Map_QuickPick_Mar(pBancaId, pFracciones, cantidadQp);
                    detallesList.AddRange(parameters.detalle.Select(item => new BilleteViewModel.MarltonParameters.Detalle
                    {
                        Jugada = item.Jugada,
                        Monto = item.Monto,
                        Loteria = item.Loteria,
                        Serial = item.Serial,
                        HoraRequest = horaRequest,
                        HoraResponse = item.HoraResponse,
                        NumeroSorteo = item.NumeroSorteo
                    }));
                    parameters.detalle = detallesList.ToArray();
                    BilleteViewModel.MarltonResponse quickPick = GenericMethods.CallServicePostAction<BilleteViewModel.MarltonResponse, BilleteViewModel.MarltonParameters>(ProductosExternosEnums.ServiceMethod.SellTicket, parameters, ProductosExternosEnums.HttpMethod.PUT);
                    if (quickPick.Serial == "00000" && quickPick.detalle == null)
                    {
                        return null;
                    }
                    cantidadQp = quickPick.detalle != null ? quickPick.detalle.Count() : 0;
                    if (quickPick.detalle != null)
                        foreach (var item in quickPick.detalle)
                        {
                            var detalle = detallesList.FirstOrDefault(x => item.Jugada.Contains(x.Jugada));
                            detallesList.Remove(detalle);
                        }
                    ticketMarlton = quickPick;
                }

                var horaResponse = DateTime.Now;
                detallesList.ForEach(x =>
                {
                    x.HoraResponse = horaResponse;
                    x.Serial = ticketMarlton.Serial;
                    x.NumeroSorteo = (int)ticketMarlton.Numero_Sorteo;
                });

                try
                {
                    var db = new MARContext();
                    UnitOfWork unitOfWork = new UnitOfWork(new MARContext());
                    var dTicket = BilleteViewModel.Map_TicketFromDetalles(detallesList, pBancaId, pUsuarioId, pRiferoId);
                    var cuenta = DataAccess.EFRepositories.VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.Billetes);
                    //agregando transaccion de billetes
                    var trans = DataAccess.EFRepositories.VpTransacciones.AgregarTransaccion(pBancaId, cuenta.VpCuenta.VP_Suplidor.FirstOrDefault().SuplidorID, cuenta.ProductoId, cuenta.VpCuenta.CuentaID,
                        dTicket.TicCosto, 0, DbEnums.Productos.Billetes.ToString(), pUsuarioId, null);

                    db.DTickets.Add(dTicket);
                    db.SaveChanges();

                    //actualiza transaccion
                    trans.Referencia = ticketMarlton.Serial;
                    trans.Estado = ticketMarlton.status.Message;
                    trans.Activo = true;
                    DataAccess.EFRepositories.VpTransacciones.ActualizaTransaccion(trans);

                    var ticketLocal = unitOfWork.DTicket.Get(x => x.TicketID == dTicket.TicketID, null, "DTicketDetalles").FirstOrDefault();
                    string pin = GeneraPinGanador((int)ticketLocal.TicSolicitud);
                    return new
                    {
                        OK = true,
                        QuickPick = ticketMarlton,
                        Jugadas = detallesList,
                        Ticket = ticketLocal,
                        Err = string.Empty,
                        PrintData = BilletePrintJob.ImprimirVentaBillete(pPrintWidth, ticketMarlton, ticketLocal, pBanca, pDireccion, pin)
                    };
                }
                catch (Exception)
                {
                    BilleteViewModel.MarltonResponse.Exec_CancelarBillete(ticketMarlton.Serial, pBancaId);
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static object ReporteBillete_PorFecha(int pPrintWidth, int pBancaId, string pBanca, string pDireccion, DateTime pFechaVenta)
        {
            var db = new MARContext();
            // var billetes = from tickets in db.DTickets where tickets.TicFecha >= pFechaVenta && tickets.TicFecha <= pFechaVenta
            try
            {
                DateTime fechaHasta = pFechaVenta.AddDays(1);
                //var billetes =
                var billetes = db.DTickets.Where(x => x.TicFecha >= pFechaVenta && x.TicFecha <= fechaHasta && x.BancaID == pBancaId)
                    .Join(db.TLoterias, d => d.LoteriaID, l => l.LoteriaID, (d, l) => new { d, l }).Where(x => x.l.LotNombre == DbEnums.Productos.Billetes.ToString())
                      .Include(d => d.d.DTicketDetalles).Select(x => x.d).ToList();



                if (billetes.Any())
                {
                    return new
                    {
                        OK = true,
                        Err = string.Empty,
                        PrintData = BilletePrintJob.ImprimirReporteBilletesPorFecha(pPrintWidth, billetes, pBanca, pDireccion, pFechaVenta),
                    };
                }
                else
                {
                    return new
                    {
                        OK = false,
                        Err = "No existen billetes para esta fecha."
                    };
                }
            }
            catch
            {
                return new
                {
                    OK = false,
                    Err = "El requerimiento fallo intente mas tarde."
                };
            }
        }

        private static string GeneraPinGanador(int pSol)
        {
            var sb = new System.Text.StringBuilder();
            int ConfirmK = 0;
            var rdn = new Random(pSol);
            for (var n = 1; n < 4; n++)
            {
                var iK = rdn.Next(9);
                sb.Append(iK.ToString());
                ConfirmK += iK;
            }

            int seed = 1;
            var sb2 = new System.Text.StringBuilder();
            for (var x = 0; x < pSol.ToString().Length; x++)
            {
                seed = seed + (ConfirmK + Convert.ToInt32(pSol.ToString()[x].ToString()));
                var iK = seed % 10;
                sb2.Append(iK.ToString());
            }
            return sb.ToString() + sb2.ToString().Substring(sb2.Length - 5);
        }
    }
}
