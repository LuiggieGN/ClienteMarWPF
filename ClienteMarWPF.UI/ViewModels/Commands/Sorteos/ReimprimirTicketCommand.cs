using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    class ReimprimirTicketCommand : ActionCommand
    {

        private readonly ValidarPagoTicketViewModel ViewModel;
        private readonly ISorteosService SorteosService;
        private readonly IAuthenticator Autenticador;

        public ReimprimirTicketCommand(ValidarPagoTicketViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            SorteosService = sorteosService;

            Action<object> comando = new Action<object>(ConsultarReimpresion);
            base.SetAction(comando);
        }

        private void ConsultarReimpresion(object parametro)
        {
            ViewModel.SetMensajeToDefaultSate();

            var numero = ViewModel.TicketNumero;
            var pin = ViewModel.TicketPin;


            if (
                  (!InputHelper.InputIsBlank(ViewModel.TicketNumero))
                     &&
                  (!InputHelper.InputIsBlank(ViewModel.TicketPin))
             )
            {

                var ReimprimirResponse = SorteosService.ReimprimirTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, Convert.ToInt32(pin));

                    if (ReimprimirResponse.Err == null)
                    {
                        ViewModel.SetMensaje(mensaje: "La reimpresion del ticket fue completada exitosamente.",
                                           icono: "Check",
                                           background: "#28A745",
                                           puedeMostrarse: true);
                    }
                    else
                    {
                        ViewModel.SetMensaje(mensaje: ReimprimirResponse.Err,
                                             icono: "Error",
                                             background: "#DC3545",
                                             puedeMostrarse: true);
                    }
            }
                else
                {

                ViewModel.SetMensaje(mensaje: "No ha digitado el Ticket o Pin",
                                     icono: "Error",
                                     background: "#DC3545",
                                     puedeMostrarse: true);
                }

        }

    }

}
