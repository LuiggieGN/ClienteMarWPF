﻿using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
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

                        #region Actualiza lo Vendido Hoy

                        var ticket = ViewModel.ListaTickets.Where(t => t.TicketNo.Equals(numero)).FirstOrDefault();

                        if (ticket != null && ViewModel.SorteoVM != null)
                        {
                            try
                            {
                                decimal ticketCosto = Convert.ToDecimal(ticket.Costo);

                                if (ViewModel.SorteoVM.TotalesCargados.Value == true)
                                {
                                    ViewModel.SorteoVM.AgregarAlTotalVendidoHoy((-1 * ticketCosto));
                                }
                                else
                                {
                                    ViewModel.SorteoVM.AgregarTransaccionPendiente((-1 * ticketCosto));
                                }

                            }
                            catch { }
                        }

                        #endregion

                        ViewModel.SetMensaje(mensaje: "La anulaciòn del ticket fue completada exitosamente.",
                                             icono: "Check",
                                             background: "#28A745",
                                             puedeMostrarse: true);
                    }
                    else
                    {
                        ViewModel.SetMensaje(mensaje: AnularResponse ?? "No esta autorizado para anular",
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
