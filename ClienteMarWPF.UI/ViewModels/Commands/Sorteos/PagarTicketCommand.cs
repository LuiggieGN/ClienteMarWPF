using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class PagarTicketCommand: ActionCommand
    {
        private readonly ValidarPagoTicketViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;
        public PagarTicketCommand(ValidarPagoTicketViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(PagarTicket);
            base.SetAction(comando);
        }

        private void PagarTicket(object parametro)
        {
            var numero = ViewModel.TicketNumero;
            var pin = ViewModel.TicketPin;
            if ((numero != null && pin != null) && (numero != "" && pin != ""))
            {
                var WinnerResponse = SorteosService.ConsultarTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, numero, pin, true);
                if (WinnerResponse.Aprobado < 0)
                {
                    ViewModel.PudePagar = false;
                    ViewModel.MostrarMensajes = true;
                    ViewModel.MensajeResponse = WinnerResponse.Mensaje;
                    //MessageBox.Show(WinnerResponse.Mensaje, "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    ViewModel.PudePagar = false;
                    ViewModel.MostrarMensajes = true;
                    ViewModel.MensajeResponse = WinnerResponse.Mensaje;
                }
            }
            else
            {
                ViewModel.PudePagar = false;
                ViewModel.MostrarMensajes = true;
                ViewModel.MensajeResponse = "No ha digitado el Ticket o Pin";
               // MessageBox.Show("No ha digitado el Ticket o Pin", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }
    }
}
