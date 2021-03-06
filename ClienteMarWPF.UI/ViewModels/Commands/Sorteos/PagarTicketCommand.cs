﻿using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.PinterConfig;
using ClienteMarWPF.UI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
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

            ViewModel.SetMensajeToDefaultSate();

            var numero = ViewModel.TicketNumero;
            var pin = ViewModel.TicketPin;

            if (
                    (!InputHelper.InputIsBlank(ViewModel.TicketNumero))
                       &&
                    (!InputHelper.InputIsBlank(ViewModel.TicketPin))
               )
            {
                var WinnerResponse = SorteosService.ConsultarTicket(Autenticador.CurrentAccount.MAR_Setting2.Sesion, numero, pin, true);
                
                List<string[]> printValidacionTicket = PrintJobs.FromPagoGanador(WinnerResponse.Mensaje, WinnerResponse.Aprobado, Autenticador);
                TicketTemplateHelper.PrintTicket(printValidacionTicket);


                if (WinnerResponse.Aprobado < 0 )
                {
                    ViewModel.PudePagar = false;

                    ViewModel.SetMensaje(mensaje: WinnerResponse.Mensaje,
                                         icono: "Error",
                                         background: "#DC3545",
                                         puedeMostrarse: true);
                }
                else
                {
                    ViewModel.PudePagar = false;

                    ViewModel.SetMensaje(mensaje: WinnerResponse.Mensaje,
                                        icono: "Check",
                                        background: "#28A745",
                                        puedeMostrarse: true);
                }
           
            }
            else
            {
                ViewModel.PudePagar = false;

                ViewModel.SetMensaje(mensaje: "No ha digitado el Ticket o Pin",
                                     icono: "Error",
                                     background: "#DC3545",
                                     puedeMostrarse: true);
            }


        }
    }
}
