using ClienteMarWPF.Domain.Services.MensajesService;
using ClienteMarWPF.UI.Modules.Mensajeria;
using ClienteMarWPF.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.Commands.Mensajes
{
    public class SendMensajeCommand: ActionCommand
    {
        private readonly MensajeriaViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IMensajesService MensajesService;

        public SendMensajeCommand(MensajeriaViewModel viewModel, IAuthenticator autenticador, IMensajesService mensajesService) : base ()
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            MensajesService = mensajesService;

            Action<object> comando = new Action<object>(sendMensajes);
            base.SetAction(comando);
        }

        private void sendMensajes(object parametro)
        {
            try
            {
                MensajesService.SendMessage(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Mensaje);
            }
            catch (Exception)
            {

            }
            
        }
    }
}
