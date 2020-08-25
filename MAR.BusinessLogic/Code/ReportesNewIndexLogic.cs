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
using MAR.DataAccess.EFRepositories;
using MAR.DataAccess.ViewModels.Mappers;


namespace MAR.BusinessLogic.Code
{
    public class ReportesIndexLogic
    {
        public static object ReportePremios_PorFecha(int pBancaId, string pBanca, int pPrintWidth, DateTime pFecha)
        {
            try
            {
                var premiosLot = HEstatusDiaRepository.GetPremios(pFecha).ToList();



                var premios = (from p in premiosLot
                               select new ReportesViewModels.ReportePremios
                               {
                                   Loteria = p.Sorteo
                              ,
                                   Primera = p.Premios
                              ,
                                   Segunda = "",
                                   Tercera = ""
                               }).ToList();

                try
                {
                    var premiosCamion = BusinessLogic.Code.SorteosMar.PagoGanadorLogic.ConsultaPremiosCamionMillonario(pBancaId);
                    if (premiosCamion != null)
                    {
                        foreach (var item in premiosCamion)
                        {
                            premios.Add(new ReportesViewModels.ReportePremios
                            {
                                Loteria = "Camion",
                                Primera = item.Numeros.ToString(),
                                Segunda = "",
                                Tercera = ""
                            });
                        }
                    }
                }
                catch
                {

                }
                //try
                //{
                //    var ganadores = JuegaMasViewModel.MarltonResponse.Exec_GetGanadoresJuegaMas(pFecha, pBancaId).WinnerNumber;
                //    if (!string.IsNullOrEmpty(ganadores))
                //    {
                //        premios.Add(new ReportesViewModels.ReportePremios
                //        {
                //            Loteria = "JuegaMas",
                //            Primera = ganadores,
                //            Segunda = "",
                //            Tercera = ""
                //        });
                //    }
                //}
                //catch (Exception e)
                //{
                //    string t = e.Message;
                //    t = t;
                //}


                return new
                {
                    OK = true,
                    Err = string.Empty,
                    PrintData = ReportsePrintJob.ImprimirListaPremios(pBanca, pFecha, pPrintWidth, premios)
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.Message,
                };
            }
        }

        public static object ReportePagosServicios_PorFecha(string pBanca, int pBancaId, string pDireccion, int pPrintWidth, DateTime pFechaVenta, int pRiferoId)
        {
            try
            {
                DateTime hasta = pFechaVenta.AddDays(1).Date;
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.PagaFacil);
                IEnumerable<Object> trans;
                if (pFechaVenta < DateTime.Today)
                {
                    trans = VpTransacciones.GetHTransacciones(x => x.FechaIngreso >= pFechaVenta && x.FechaIngreso <= hasta && x.BancaID == pBancaId && x.Activo && x.ProductoID == cuenta.ProductoId, null, "VP_Suplidor, VP_Producto");
                }
                else
                {
                    trans = VpTransacciones.GetTransacciones(x => x.FechaIngreso >= pFechaVenta && x.FechaIngreso <= hasta && x.BancaID == pBancaId && x.Activo && x.ProductoID == cuenta.ProductoId, null, "VP_Suplidor, VP_Producto");
                }
                ReportesViewModels.ReportePagosServicios[] reporte = PagosServiciosMapper.MapFromTransaccionToReportePagosServicioses(trans);
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    PrintData = ReportsePrintJob.ImprimirReportePagosServicios(pPrintWidth, pBanca, pDireccion, pFechaVenta, reporte)
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.Message,
                };
            }
        }

        public static object ReporteJuegaMas_PorFecha(string pBanca, int pBancaId, string pDireccion, int pPrintWidth, DateTime pFechaVenta, int pRiferoId)
        {
            try
            {
                DateTime hasta = pFechaVenta.AddDays(1).Date;
                var cuenta = VpCuentaRepository.GetVpCuenta(pRiferoId, DbEnums.Productos.JuegaMas);
                IEnumerable<Object> trans;
                if (pFechaVenta < DateTime.Today)
                {
                    trans = VpTransacciones.GetHTransacciones(x => x.FechaIngreso >= pFechaVenta && x.FechaIngreso <= hasta && x.BancaID == pBancaId && x.Activo && x.ProductoID == cuenta.ProductoId, null, "VP_Suplidor, VP_Producto");
                }
                else
                {
                    trans = VpTransacciones.GetTransacciones(x => x.FechaIngreso >= pFechaVenta && x.FechaIngreso <= hasta && x.BancaID == pBancaId && x.Activo && x.ProductoID == cuenta.ProductoId, null, "VP_Suplidor, VP_Producto");
                }
                ReportesViewModels.ReporteJuegaMasCliente[] reporte = JuegaMasMappers.MapFromTransaccionToReporteJuegaMas(trans);
                return new
                {
                    OK = true,
                    Err = string.Empty,
                    PrintData = ReportsePrintJob.ImprimirReporteJuegaMas(pPrintWidth, pBanca, pDireccion, pFechaVenta, reporte)
                };
            }
            catch (Exception e)
            {
                return new
                {
                    OK = false,
                    Err = e.Message,
                };
            }
        }

    }
}
