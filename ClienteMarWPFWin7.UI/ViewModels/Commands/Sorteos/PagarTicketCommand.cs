﻿using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos
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
                    ViewModel.TicketNumero = null;
                    ViewModel.TicketPin = null;

                    LeerBalanceAsync();
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




        #region Refrescar Balance
        private bool IsBusy_LeerBalanceThread = false;
        public void LeerBalanceAsync()
        {
            if (Autenticador != null &&
                Autenticador.BancaConfiguracion != null &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto != null &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto.PuedeUsarControlEfectivo == true &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto.BancaYaInicioControlEfectivo == true
                )
            {
                if (!IsBusy_LeerBalanceThread)
                {
                    Task.Factory.StartNew(() =>
                    {
                        IsBusy_LeerBalanceThread = true;
                        try
                        {
                            Autenticador.RefrescarBancaBalance();
                        }
                        catch
                        {

                        }
                        IsBusy_LeerBalanceThread = false;
                    });
                }
            }
        }
        #endregion
    }
}
