using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.UI.Modules.Mensajeria;
using ClienteMarWPFWin7.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Mensajes
{
    public class GetMensajesCommand: ActionCommand
    {
        private readonly MensajeriaViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IMensajesService MensajesService;

        public GetMensajesCommand(MensajeriaViewModel viewModel, IAuthenticator autenticador, IMensajesService mensajesService) : base()
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            MensajesService = mensajesService;

            Action<object> comando = new Action<object>(GetMensajes);
            base.SetAction(comando);
        }

        private void GetMensajes(object parametro)
        {
            try
            {
              var mensajes = MensajesService.GetMessages(Autenticador.CurrentAccount.MAR_Setting2.Sesion).msj;
                if (mensajes != null)
                {
                    foreach (var item in mensajes)
                    {
                        ViewModel.Mensajes.Add(item);
                    }
                }

                
            }
            catch (Exception)
            {

            }

        }

    }
}
