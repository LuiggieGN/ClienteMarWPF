using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.UI.Modules.Recargas.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.PinterConfig;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Recargas
{
    public class DialogConfirmTicketCommand: ActionCommand
    {
        private readonly DialogImprimirTicketViewModel _viewModel;
        private IRecargaService _recargaService;
        private IAuthenticator _autenticador;
        private RecargasIndexRecarga datosRecargas;
        public DialogConfirmTicketCommand(DialogImprimirTicketViewModel viewModel, IAuthenticator autenticador,  IRecargaService recargaService,RecargasIndexRecarga dataRecargas):base()
        {
            _viewModel = viewModel;
            _recargaService = recargaService;
            _autenticador = autenticador;
            datosRecargas = dataRecargas;

            Action<object> comando = new Action<object>(CrearTicket);
            base.SetAction(comando);
        }


        public void CrearTicket(object parametro)
        {
            try {
                _recargaService.ConfirmRecarga(_autenticador.CurrentAccount.MAR_Setting2.Sesion);
                List<string[]> impresionRecargas = PrintJobs.FromImprimirRecarga(datosRecargas, _autenticador);
                TicketTemplateHelper.PrintTicket(impresionRecargas);
                new CerrarDialogoImprimirTicketCommand(_viewModel);
               
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}
