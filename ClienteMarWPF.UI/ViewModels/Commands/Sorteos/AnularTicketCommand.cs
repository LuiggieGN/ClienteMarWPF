using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Sorteos
{
    public class AnularTicketCommand : ActionCommand
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

            ViewModel.SetMensajeToDefaultSate();
            ViewModel.PudePagar = false;

            var numero = ViewModel.TicketNumero;
            var pin = ViewModel.TicketPin;



            if (
                    (!InputHelper.InputIsBlank(ViewModel.TicketNumero))
                       &&
                    (!InputHelper.InputIsBlank(ViewModel.TicketPin))
               )
            {
                var AnularResponse = SorteosService.AnularTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, numero, pin);
                if (AnularResponse != string.Empty)
                {
                    if (AnularResponse != null && AnularResponse.Equals("OK"))
                    {
                        ViewModel.SetMensaje(mensaje: "La anulaciòn del ticket fue completada exitosamente.",
                                             icono: "Check",
                                             background: "#28A745",
                                             puedeMostrarse: true);
                    }
                    else
                    {
                        ViewModel.SetMensaje(mensaje: AnularResponse ?? "Ha ocurrido un error al procesar la operaciòn",
                                             icono: "Error",
                                             background: "#DC3545",
                                             puedeMostrarse: true);
                    }
                }
                else
                {
                    ViewModel.SetMensaje(mensaje: "Ha ocurrido un error al procesar la operaciòn",
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
