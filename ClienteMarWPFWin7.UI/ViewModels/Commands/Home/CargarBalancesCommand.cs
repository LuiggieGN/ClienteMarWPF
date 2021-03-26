using ClienteMarWPFWin7.UI.Modules.Home;

using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;

using System.Windows.Threading;
using System.ComponentModel;
using System.Threading;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Home
{
    public class CargarBalancesCommand : ActionCommand
    {
        private readonly HomeViewModel ViewModel;
        private DateTime UltimaActualizacion;

        public CargarBalancesCommand(HomeViewModel viewmodel) : base()
        {
            ViewModel = viewmodel;

            var comando = new Action<object>(CargarBalances);
            var puede = new Predicate<object>(PuedeCargarBalances);

            base.SetAction(comando, puede);
        }


        private bool PuedeCargarBalances(object parametro)
        {
            return true;
        }


        private void CargarBalances(object parametro)
        {
            if (!HomeViewModel.Worker.IsBusy)
            {
                HomeViewModel.Worker = new BackgroundWorker();
                HomeViewModel.Worker.WorkerSupportsCancellation = Si;
                HomeViewModel.Worker.DoWork += worker_DoWork;
                HomeViewModel.Worker.RunWorkerCompleted += worker_RunWorkerCompleted;
                HomeViewModel.Worker.RunWorkerAsync();
            }
        }


        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {

            UltimaActualizacion = DateTime.Now;

            ViewModel.Cargando = Si;
            ViewModel.UpdateCardsValue("*", "*", "*", "*", "*", null);

            bool bancaUsaControlEfectivo = false;

            try
            {
                bancaUsaControlEfectivo = ViewModel.BancaService.BancaUsaControlEfectivo(ViewModel.AuthenticatorService.BancaConfiguracion.BancaDto.BancaID, incluyeConfig: true);
            }
            catch
            {
                bancaUsaControlEfectivo = false;
            }


            if (!bancaUsaControlEfectivo) // Si la BANCA no usa Control de Efectivo Ingresa aqui
            {
                ViewModel.UpdateCardsValue("*", "*", "*", "*", "*", null);
                return;
            }


            bool CALL_ASYNC = Si;
            bool BREAK_INFINITY = No;

            for (;;)
            {
                if (CALL_ASYNC)
                {
                    Task.Factory.StartNew(() =>
                    {
                        GetAndSetCardsData();
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
                    ViewModel.UpdateCardsValue("*", "*", "*", "*", "*",null);
                    e.Cancel = true;
                    break;
                }

                Thread.Sleep(25);
            }


            ViewModel.Cargando = No;
            HomeViewModel.Worker.CancelAsync();

        }



        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ViewModel.Cargando = No;
        }


        private void GetAndSetCardsData()
        {
            Thread.Sleep(2000);


            string bancaBalance = "*";
            decimal totVentasLoterias = 0,
                    totVentasProductos = 0,
                    totComisiones = 0,
                    totAnulaciones = 0;



            DateTime fechaConsulta = ViewModel.FechaAConsultar;

            int cajaid = ViewModel.AuthenticatorService.BancaConfiguracion?.CajaEfectivoDto?.CajaID ?? -1;

            var pideMovimientosDesde = new MovimientoPageDTO();
            pideMovimientosDesde.CajaId = cajaid;
            pideMovimientosDesde.FechaDesde = fechaConsulta.Date;
            pideMovimientosDesde.FechaHasta = fechaConsulta.Date;
            pideMovimientosDesde.CategoriaOperacion = null;

            try
            {
                bancaBalance = "$" + ViewModel.CajaService.LeerCajaBalance(cajaid).ToString("#,##0.00");
            }
            catch
            {
                bancaBalance = "*";
            }


            List<MovimientoDTO> movmientos;

            try
            {
                movmientos = ViewModel.CajaService.LeerMovimientosNoPaginados(pideMovimientosDesde);
            }
            catch
            {
                movmientos = new List<MovimientoDTO>(); ;
            }



            if (movmientos == null || movmientos.Count == 0)
            {
                ViewModel.UpdateCardsValue("$0.00", "$0.00", "$0.00", "$0.00", bancaBalance, UltimaActualizacion);
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



            ViewModel.UpdateCardsValue("$" + totVentasLoterias.ToString("#,##0.00"),
                                       "$" + totVentasProductos.ToString("#,##0.00"),
                                       "$" + totComisiones.ToString("#,##0.00"),
                                       "$" + totAnulaciones.ToString("#,##0.00"),
                                           bancaBalance,
                                           UltimaActualizacion);

        }











    }//fin de Clase

}//fin de Namespace
