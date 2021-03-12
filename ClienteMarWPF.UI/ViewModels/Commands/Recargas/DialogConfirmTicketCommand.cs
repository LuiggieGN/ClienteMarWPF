using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.UI.Modules.Recargas.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Recargas
{
    public class DialogConfirmTicketCommand : ActionCommand
    {
        private readonly DialogImprimirTicketViewModel _viewModel;
        private IRecargaService _recargaService;
        private IAuthenticator _autenticador;
        public DialogConfirmTicketCommand(DialogImprimirTicketViewModel viewModel, IAuthenticator autenticador, IRecargaService recargaService) : base()
        {
            _viewModel = viewModel;
            _recargaService = recargaService;

            var comando = new Action<object>(CrearTicket);
            base.SetAction(comando);
        }

        public void CrearTicket(object parametro)
        {
            _recargaService.ConfirmRecarga(_autenticador.CurrentAccount.MAR_Setting2.Sesion);
        }
    }
}
