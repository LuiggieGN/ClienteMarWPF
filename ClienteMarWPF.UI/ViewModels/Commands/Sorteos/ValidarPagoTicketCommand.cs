using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class ValidarPagoTicketCommand:ActionCommand
    {
        private readonly SorteosViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        public ValidarPagoTicketCommand(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(ValidarPagoTicket);
            base.SetAction(comando);
        }

        private void ValidarPagoTicket(object parametro)
        {
            ViewModel.Dialog = new ValidarPagoTicketViewModel(ViewModel, Autenticador,  SorteosService);
            ViewModel.Dialog.Mostrar();
        }
    }
}
