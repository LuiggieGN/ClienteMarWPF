using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.UI.Modules.Recargas;
using ClienteMarWPF.UI.Modules.Recargas.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.PinterConfig;
using ClienteMarWPF.UI.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClienteMarWPF.UI.ViewModels.Commands.Recargas
{
    public class SendRecargaCommand : ActionCommand
    {
        private readonly RecargasViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IViewModelFactory _vistas;
        private readonly INavigator _nav;
        private readonly IRecargaService RecargaService;
        public Random r = new Random();
        public static int Solicitud;
        public SendRecargaCommand(RecargasViewModel viewModel, IAuthenticator autenticador, INavigator nav, IViewModelFactory vistas, IRecargaService recargaService) : base()
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            RecargaService = recargaService;
            _nav = nav;
            _vistas = vistas;
            Solicitud = r.Next(999,9989);
            Action<object> comando = new Action<object>(SendRecarga);
            base.SetAction(comando);

        }


        private void SendRecarga(object parametro)
        {
            ViewModel.ErrorMessage = string.Empty;
           

            

            if ((ViewModel.Provedor.OperadorID != 0) &&  (ViewModel.Telefono != null && ViewModel.Telefono != null) &&  Convert.ToDouble(ViewModel.Monto) != 0)
            {
                try
                {
                    Solicitud += 1;
                    var recarga = RecargaService.GetRecarga(Autenticador.CurrentAccount.MAR_Setting2.Sesion,
                                                            Autenticador.CurrentAccount.MAR_Setting2.Sesion.Usuario, Autenticador.CurrentAccount.UsuarioDTO.UsuClave,
                                                            ViewModel.Provedor.OperadorID, ViewModel.Telefono, Convert.ToDouble(ViewModel.Monto), Solicitud);

                    if(recarga.Err != null) 
                    {
                        ViewModel.ErrorMessage = recarga.Err;
                    }
                    else
                    {
                        RecargasIndexRecarga DATOSRecargas = new RecargasIndexRecarga() { UsuarioId = 0, Solicitud = Solicitud, Clave = "ok", Monto = recarga.Costo, Numero = recarga.Numero, Serie = recarga.Serie, Suplidor = "ok", SuplidorId = 0 };

                       //List<string[]> impresionRecargas = PrintJobs.FromImprimirRecarga(DATOSRecargas,Autenticador);
                       //TicketTemplateHelper.PrintTicket(impresionRecargas);

                        ViewModel.Dialog = new DialogImprimirTicketViewModel(_nav, Autenticador, _vistas, RecargaService, DATOSRecargas);
                        ViewModel.Dialog.Mostrar();
                        
                       
                       
                    }
                }
                catch (Exception e)
                {

                    ViewModel.ErrorMessage = e.Message;
                }
            }
            else
            {
                ViewModel.ErrorMessage = "Hay campos que no estan correctos.";
            }

         
     
        }
    }
}
