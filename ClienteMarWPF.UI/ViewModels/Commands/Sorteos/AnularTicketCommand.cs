using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class AnularTicketCommand: ActionCommand
    {
        private readonly ValidarPagoTicketViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        public AnularTicketCommand(ValidarPagoTicketViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(AnularTicket);
            base.SetAction(comando);
        }

        private void AnularTicket(object parametro)
        {
            var numero = ViewModel.TicketNumero;
            var pin = ViewModel.TicketPin;
            if ((numero != null && pin != null) && (numero != "" && pin != ""))
            {
                var AnularResponse = SorteosService.AnularTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, numero, pin);
                if (AnularResponse != string.Empty)
                {
                    ViewModel.PudePagar = false;
                    ViewModel.MostrarMensajes = true;
                    ViewModel.MensajeResponse = AnularResponse;
                }
                else
                {
                    ViewModel.PudePagar = false;
                    ViewModel.MostrarMensajes = true;
                    ViewModel.MensajeResponse = "No ha digitado el Ticket o Pin";
                }
            }
            else
            {
                ViewModel.PudePagar = false;
                ViewModel.MostrarMensajes = true;
                ViewModel.MensajeResponse = "No ha digitado el Ticket o Pin";
            }
        }
    }
}
