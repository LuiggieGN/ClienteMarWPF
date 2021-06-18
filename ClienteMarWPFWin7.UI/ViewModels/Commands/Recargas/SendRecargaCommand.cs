using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.UI.Modules.Recargas;
using ClienteMarWPFWin7.UI.Modules.Recargas.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.Factories;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Recargas
{
    public class SendRecargaCommand : ActionCommand
    {
        private readonly RecargasViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IViewModelFactory _vistas;
        private readonly INavigator _nav;
        private readonly IRecargaService RecargaService;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        public int Solicitud = 0;
        private bool RecargaSucess;
        private string RecargaMensajeError = string.Empty;
        private ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference.MAR_Pin Recarga;
        public SendRecargaCommand(RecargasViewModel viewModel, IAuthenticator autenticador, INavigator nav, IViewModelFactory vistas, IRecargaService recargaService) : base()
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            RecargaService = recargaService;
            _nav = nav;
            _vistas = vistas;

            worker.DoWork += worker_DoWork;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            Action<object> comando = new Action<object>(Recargar);
            Predicate<object> puede = new Predicate<object>(PuedeRecargar);
            base.SetAction(comando, puede);

        }




        public bool PuedeRecargar(object parametro)
        {
            return Si;                      
        }



        public void Recargar(object password)
        {
            if (!worker.IsBusy) { worker.RunWorkerAsync(argument: password); }            
        }



        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
           
            ViewModel.ErrorMessage = string.Empty;
            ViewModel.Cargando = Si;

            if ((ViewModel.Provedor.OperadorID != 0) && (ViewModel.Telefono != null && ViewModel.Telefono != null) && Convert.ToDouble(ViewModel.Monto) != 0)
            {
                try
                {
                    // Solicitud += 1;
                    SessionGlobals.GenerateNewSolicitudID(Autenticador.CurrentAccount.MAR_Setting2.Sesion.Sesion, true);
                    Solicitud = (int)SessionGlobals.SolicitudID;
                    Recarga = RecargaService.GetRecarga(Autenticador.CurrentAccount.MAR_Setting2.Sesion,
                                                            Autenticador.CurrentAccount.MAR_Setting2.Sesion.Usuario, Autenticador.CurrentAccount.UsuarioDTO.UsuClave,
                                                            ViewModel.Provedor.OperadorID, ViewModel.Telefono, Convert.ToDouble(ViewModel.Monto), Solicitud);

                    if (Recarga.Err != null)
                    {
                        RecargaSucess = No;
                    }
                    else
                    {

                        RecargaSucess = Si;
                    }
                }
                catch (Exception ex)
                {
                  
                    Recarga = new Domain.MarPuntoVentaServiceReference.MAR_Pin();
                    RecargaSucess = No;
                    Recarga.Err = "Ha ocurrido un error al momento de procesar la recarga.";

                    
                }
            }
            else
            {
               
                Recarga = new Domain.MarPuntoVentaServiceReference.MAR_Pin();
                RecargaSucess = No;
                Recarga.Err = "hay campos que estan vacios.";
            }


            ViewModel.Cargando = No;

        }



        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ViewModel.Cargando = Booleano.No;

            if (!RecargaSucess)
            {

                ViewModel.ErrorMessage = Recarga.Err;
                

            }
            else
            {
               var  DATOSRecargas = new RecargasIndexRecarga() { UsuarioId = 0, Solicitud = Solicitud, Clave = "ok", Monto = Recarga.Costo, Numero = Recarga.Numero, Serie = Recarga.Serie, Suplidor = "ok", SuplidorId = 0 };
                ViewModel.Dialog = new DialogImprimirTicketViewModel(_nav, Autenticador, _vistas, RecargaService, DATOSRecargas);
                ViewModel.Dialog.Mostrar();
                ViewModel.Telefono = null;
                ViewModel.Monto = null;
                LeerBalanceAsync();

            }
        }


        #region Refrescar Balance
        private bool IsBusy_LeerBalanceThread = false;
        public void LeerBalanceAsync()
        {
            if (Autenticador != null &&
                Autenticador.BancaConfiguracion != null &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto != null &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto.PuedeUsarControlEfectivo == true &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto.BancaYaInicioControlEfectivo == true
                )
            {
                if (!IsBusy_LeerBalanceThread)
                {
                    Task.Factory.StartNew(() =>
                    {
                        IsBusy_LeerBalanceThread = true;
                        try
                        {
                            Autenticador.RefrescarBancaBalance();
                        }
                        catch
                        {

                        }
                        IsBusy_LeerBalanceThread = false;
                    });
                }
            }
        }
        #endregion
    }
}
