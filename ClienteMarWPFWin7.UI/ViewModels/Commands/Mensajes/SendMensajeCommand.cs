using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.UI.Modules.Mensajeria;
using ClienteMarWPFWin7.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Mensajes
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
                if (ViewModel.Mensaje != null)
                {
                    MensajesService.SendMessage(Autenticador.CurrentAccount.MAR_Setting2.Sesion, ViewModel.Mensaje);
                    ViewModel.Mensaje = null;
                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta("Mensaje enviado correctamente.", "Excelente");
                }
                else
                {
                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta("Lo sentimos, no se puede enviar un mensaje vacio.", "Info");
                }
                
            }
            catch (Exception)
            {

            }
            
        }
    }
}
