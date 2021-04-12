using ClienteMarWPFWin7.UI.Modules.Home;

using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;

using System.Windows.Threading;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Home
{
    public class CargarBalancesCommand : ActionCommand
    {
        private DateTime UltimaActualizacion;
        private readonly HomeViewModel ViewModel;


        public CargarBalancesCommand(HomeViewModel viewmodel) : base()
        {
            ViewModel = viewmodel;
            base.SetAction(new Action<object>(CargarBalances), new Predicate<object>(PuedeCargarBalances));
        }

        private bool PuedeCargarBalances(object parametro) => true;
        private void CargarBalances(object parametro)
        {
            if (!HomeViewModel.Worker.IsBusy)
            {
                HomeViewModel.Worker = new BackgroundWorker();
                HomeViewModel.Worker.WorkerSupportsCancellation = Si;
                HomeViewModel.Worker.DoWork += LlenarCards;
                HomeViewModel.Worker.RunWorkerCompleted += LlenarCardsCompletado;
                HomeViewModel.Worker.RunWorkerAsync();
            }
        }

        private void LlenarCards(object sender, DoWorkEventArgs e)
        {
            UltimaActualizacion = DateTime.Now;

            ViewModel.Cargando = Si;

            ViewModel.UpdateCardsValue("*", "*", "*", "*","*", "*", "*", null);

            bool bancaUsaControlEfectivo = No;

            try
            {
                bancaUsaControlEfectivo = ViewModel.ServicioBanca.BancaUsaControlEfectivo(ViewModel.ServicioAutenticacion.BancaConfiguracion.BancaDto.BancaID, incluyeConfig: true);
            }
            catch
            {
                ViewModel.UpdateCardsValue("*", "*", "*", "*", "*", "*", "*", UltimaActualizacion);
                ViewModel.Toast.ShowError("Ha occurrido un error al cargar el dashboard. Verificar conexión de internet.");
                ViewModel.Cargando = No;
                HomeViewModel.Worker.CancelAsync();
                return;
            }


            bool CALL_ASYNC = Si;
            bool BREAK_INFINITY = No;

            for (; ; )
            {
                if (CALL_ASYNC)
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (bancaUsaControlEfectivo)
                        {
                            ConControlEfectivo();
                        }
                        else
                        {
                            SinControlEfectivo();
                        }
                        BREAK_INFINITY = Si;
                    });
                }

                CALL_ASYNC = No;

                if (BREAK_INFINITY)
                {
                    break;
                }

                if (HomeViewModel.Worker.CancellationPending)
                {
                    ViewModel.UpdateCardsValue("*", "*", "*", "*", "*", "*", "*", null);
                    e.Cancel = true;
                    break;
                }
                Thread.Sleep(25);
            }

            ViewModel.Cargando = No;
            HomeViewModel.Worker.CancelAsync();
        }
        private void LlenarCardsCompletado(object sender, RunWorkerCompletedEventArgs e) => ViewModel.Cargando = No;


        #region Cards Banca Con Control Efectivo - ACTIVO
        private void ConControlEfectivo()
        {
            try
            {
                int idcaja = ViewModel.ServicioAutenticacion.BancaConfiguracion?.CajaEfectivoDto?.CajaID ?? -1;

                var bancaBalance = "*"; 
                
                DateTime fechaConsulta = ViewModel.FechaAConsultar;

                decimal totVentasLoterias = 0, 
                        totVentasProductos = 0, 
                        totComisiones = 0, 
                        totAnulaciones = 0, 
                        totPagos = 0, 
                        totDescuentoVentasProductos = 0;

                var consulta = new MovimientoPageDTO() {
                    CajaId = idcaja, 
                    FechaDesde = fechaConsulta.Date, 
                    FechaHasta = fechaConsulta.Date, 
                    CategoriaOperacion = null
                };

                bancaBalance = "$" + ViewModel.ServicioCaja.LeerCajaBalance(idcaja).ToString("#,##0.00");

                List<MovimientoDTO> movmientos = ViewModel.ServicioCaja.LeerMovimientosNoPaginados(consulta);


                if (movmientos == null || movmientos.Count == 0)
                {
                    ViewModel.UpdateCardsValue("$0.00", "$0.00", "$0.00", "$0.00", "$0.00", "$0.00", bancaBalance, UltimaActualizacion);
                    return;
                }


                totVentasLoterias = movmientos.Where(s => (
                                                             (s.Categoria == "Ingreso" && s.CategoriaSubTipoID == 3) ||   // ApuestaActiva
                                                             (s.Categoria == "Ingreso" && s.CategoriaSubTipoID == 4)      // ApuestaRFActiva 
                                                           )
                                                     ).Sum(x => x.EntradaOSalida);



                totVentasProductos = movmientos.Where(s => (
                                                             (s.Categoria == "Ingreso" && s.CategoriaSubTipoID == 5) ||   // RecargaActiva
                                                             (s.Categoria == "Ingreso" && s.CategoriaSubTipoID == 6)      // VP_TransaccionActiva 
                                                           )
                                                     ).Sum(x => x.EntradaOSalida);



                totComisiones = movmientos.Where(s => (
                                                             (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 15)     // ComisionApuestaBanca                                                            
                                                      )
                                                 ).Sum(x => x.EntradaOSalida);



                totAnulaciones = movmientos.Where(s => (
                                                             (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 4) ||   // ApuestaNULA
                                                             (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 5) ||   // ApuestaRFNULA 
                                                             (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 6) ||   // RecargaNula 
                                                             (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 7)      // VP_TransaccionNula 
                                                           )
                                                     ).Sum(x => x.EntradaOSalida);


                
                totPagos = movmientos.Where(s => (
                                                        (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 9) ||   // PagoGanador
                                                        (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 10) ||  // PagoGanadorRemoto                                      
                                                        (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 12) ||  // CL_PagoGanador                                      
                                                        (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 13)     // CL_PagoGanadorRemoto       

                                                        )
                                                 ).Sum(x => x.EntradaOSalida);



                totDescuentoVentasProductos = movmientos.Where(s => (
                                                                      (s.Categoria == "Egreso" && s.CategoriaSubTipoID == 8)     // VP_TransaccionDescuento
                                                              )
                                           ).Sum(x => x.EntradaOSalida);




                ViewModel.UpdateCardsValue("$" + totVentasLoterias.ToString("#,##0.00"),
                                           "$" + totVentasProductos.ToString("#,##0.00"),
                                           "$" + totComisiones.ToString("#,##0.00"),
                                           "$" + totAnulaciones.ToString("#,##0.00"),
                                           "$" + totPagos.ToString("#,##0.00"), 
                                           "$" + totDescuentoVentasProductos.ToString("#,##0.00"),
                                               bancaBalance,
                                               UltimaActualizacion);
            }
            catch
            {
                ViewModel.UpdateCardsValue("*", "*", "*", "*", "*", "*", "*", UltimaActualizacion);
                ViewModel.Toast.ShowError("Ha occurrido un error al cargar el dashboard. Verificar conexión de internet.");
            }
        }
        #endregion

        #region Cards Banca Sin Control Efectivo  
        private void SinControlEfectivo()
        {
            try
            {
                decimal totVentasLoterias = 0,
                        totVentasProductos = 0,
                        totComisiones = 0,
                        totAnulaciones = 0,
                        totPagos = 0,
                        totDescuentoVentasProductos = 0;


                var fecha = ViewModel.FechaAConsultar;

                var bancaid = ViewModel.ServicioAutenticacion?.BancaConfiguracion?.BancaDto?.BancaID ?? -1;

                var operaciones = ViewModel.ServicioBanca.LeerBancaMarOperacionesDia(bancaid, fecha.ToString("yyyyMMdd", CultureInfo.InvariantCulture));

                if (operaciones == null || operaciones.Count == 0)
                {
                    ViewModel.UpdateCardsValue("$0.00", "$0.00", "$0.00", "$0.00", "$0.00", "$0.00", "*", UltimaActualizacion);
                    return;
                }


                totVentasLoterias = operaciones.Where(s => (
                                                             (s.Tipo == "Ingreso" && s.KeyMovimiento == 1) ||   // ApuestaActiva
                                                             (s.Tipo == "Ingreso" && s.KeyMovimiento == 3)      // ApuestaRFActiva 
                                                           )
                                                     ).Sum(x => x.Monto);


                totVentasProductos = operaciones.Where(s => (
                                                             (s.Tipo == "Ingreso" && s.KeyMovimiento == 5) ||   // RecargaActiva
                                                             (s.Tipo == "Ingreso" && s.KeyMovimiento == 7)      // VP_TransaccionActiva 
                                                           )
                                                     ).Sum(x => x.Monto);


                totComisiones = operaciones.Where(s =>
                                                        s.Tipo == "Egreso" && s.KeyMovimiento == 15              // ComisionApuestaBanca

                                                     ).Sum(x => x.Monto);



                totAnulaciones = operaciones.Where(s => (
                                                           (s.Tipo == "Egreso" && s.KeyMovimiento == 2) ||   // ApuestaNULA
                                                           (s.Tipo == "Egreso" && s.KeyMovimiento == 4) ||   // ApuestaRFNULA
                                                           (s.Tipo == "Egreso" && s.KeyMovimiento == 6) ||   // RecargaNula
                                                           (s.Tipo == "Egreso" && s.KeyMovimiento == 8)      // VP_TransaccionNula
                                                         )
                                                     ).Sum(x => x.Monto);



                totPagos = operaciones.Where(s => (
                                                    (s.Tipo == "Egreso" && s.KeyMovimiento == 10) ||   // PagoGanador
                                                    (s.Tipo == "Egreso" && s.KeyMovimiento == 11) ||   // PagoGanadorRemoto
                                                    (s.Tipo == "Egreso" && s.KeyMovimiento == 13) ||   // CL_PagoGanador
                                                    (s.Tipo == "Egreso" && s.KeyMovimiento == 14)      // CL_PagoGanadorRemoto
                                                  ) 
                                            ).Sum(x => x.Monto);



                totDescuentoVentasProductos = operaciones.Where(s => (
                                                               (s.Tipo == "Egreso" && s.KeyMovimiento == 9)  // VP_TransaccionDescuento 
                                                                )
                                                     ).Sum(x => x.Monto);


                ViewModel.UpdateCardsValue("$" + totVentasLoterias.ToString("#,##0.00"),
                                           "$" + totVentasProductos.ToString("#,##0.00"),
                                           "$" + totComisiones.ToString("#,##0.00"),
                                           "$" + totAnulaciones.ToString("#,##0.00"),
                                           "$" + totPagos.ToString("#,##0.00"),
                                           "$" + totDescuentoVentasProductos.ToString("#,##0.00"),
                                           "*",
                                           UltimaActualizacion);
            }
            catch
            {
                ViewModel.UpdateCardsValue("*", "*", "*", "*", "*", "*", "*", UltimaActualizacion);
                ViewModel.Toast.ShowError("Ha occurrido un error al cargar el dashboard. Verificar conexión de internet.");
            }
        }
        #endregion














    }//fin de clase CargarBalancesCommand

}//fin de namespace
