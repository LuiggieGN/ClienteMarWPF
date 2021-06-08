using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.UI.Modules.Mensajeria;
using ClienteMarWPFWin7.UI.State.Authenticators;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Threading;

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Mensajes
{
    public class GetMensajesCommand : ActionCommand
    {
        private readonly MensajeriaViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IMensajesService MensajesService;
        public static DispatcherTimer Timer { get; set; }
        public GetMensajesCommand(MensajeriaViewModel viewModel, IAuthenticator autenticador, IMensajesService mensajesService) : base()
        {
            ViewModel = viewModel;
            Autenticador = autenticador;
            MensajesService = mensajesService;

            Action<object> comando = new Action<object>(GetMensajes);
            base.SetAction(comando);

            if (Timer != null)
            {
                Timer.Stop();
            }
            //Timer que corre cada x segundos
            Timer = new DispatcherTimer();
            Timer.Tick += new EventHandler(ObtenerMensajes);
            Timer.Interval = TimeSpan.FromSeconds(5);
            Timer.Start();
        }

        private void GetMensajes(object parametro)
        {
            var cantidad = "";

            try
            {
                var mensajes = MensajesService.GetMessages(Autenticador.CurrentAccount.MAR_Setting2.Sesion).msj;
                if (mensajes != null)
                {
                    foreach (var item in mensajes)
                    {
                        ViewModel.Mensajes.Add(item);

                    }

                    for (var i = 0; i < mensajes.Length; i++)
                    {
                        cantidad = $"Tienes {i + 1} nuevos mensajes.";
                    }

                    (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(cantidad, "Info");
                }
                else
                {
                    return;
                }

            }
            catch (Exception)
            {

            }

        }

        public void ObtenerMensajes(object sender, EventArgs e)
        {
            GetMensajes(sender);
            
        }

    }
}
