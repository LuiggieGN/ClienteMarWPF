using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
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
            var acciones = parametro as Tuple<Action,Action>;

            ViewModel.Dialog = new ValidarPagoTicketViewModel(ViewModel, Autenticador,  SorteosService, acciones.Item1,acciones.Item2);
            ViewModel.Dialog.Mostrar();
        }
    }
}
