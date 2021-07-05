
#region Namespaces
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.UI.Modules.Mensajeria;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Windows.Threading;
#endregion

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.Mensajes
{
    public class GetMensajesCommand : ActionCommand
    {
        private int _idbanca;
        public static DispatcherTimer Timer { get; set; }

        private readonly MensajeriaViewModel ViewModel;
        private readonly IAuthenticator Autenticador;
        private readonly IMensajesService MensajesService;

        public GetMensajesCommand(MensajeriaViewModel viewModel,
                                  IAuthenticator autenticador,
                                  IMensajesService mensajesService) : base()
        {
            _idbanca = autenticador?.BancaConfiguracion?.BancaDto?.BancaID ?? -1;

            ViewModel = viewModel;
            Autenticador = autenticador;
            MensajesService = mensajesService;

            base.SetAction(new Action<object>(BuscaMensajes));

            if (Timer != null)
            {
                Timer.Stop();
            }

            Timer = new DispatcherTimer();
            Timer.Tick += (sender, args) => BuscaMensajes(sender);
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Start();
        }


        private void BuscaMensajes(object parametro)
        {
            try
            {
                if (ViewModel.ScrollDownPendiente == Si)
                {
                    ViewModel.EscrolearHaciaAbajo?.Invoke();
                    ViewModel.ScrollDownPendiente = No;
                }

                var mensajes = MensajesService.GetMessages(Autenticador.CurrentAccount.MAR_Setting2.Sesion).msj;

                if (mensajes != null)
                {
                    #region Logica Revertiendo Mensajes

                    var pilaMensajes = new Stack<MAR_Mensaje2>();

                    for (int indiceMensaje = 0; indiceMensaje < mensajes.Length; indiceMensaje++)
                    {
                        pilaMensajes.Push(mensajes[indiceMensaje]);
                    }

                    ViewModel.Mensajes.Clear();

                    foreach (var item in pilaMensajes)
                    {
                        ViewModel.Mensajes.Add(
                            new ChatMensajeObservable
                            {
                                BancaID = item.BancaID,
                                MensajeID = item.MensajeID,
                                Tipo = item.Tipo,
                                Asunto = item.Asunto,
                                Contenido = item.Contenido,
                                Fecha = item.Fecha,
                                Hora = item.Hora,
                                Origen = item.Origen,
                                Destino = item.Destino,
                                Leido = item.Leido,
                                SinLeerTotal = item.SinLeerTotal,
                                EsMiMensaje = item.BancaID == _idbanca ? Si : No
                            });
                    }


                    ViewModel.RegistrarCambiosEnColeccionDeMensajes();

                    #endregion
                }
                else
                {
                    return;
                }

            }
            catch
            {

            }

        }//fin de metodo BuscaMensajes( )






    }
}











































//public void Notificacion(object parametro)
//{
//    var cantidad = "";

//    try
//    {
//        var mensajes = MensajesService.GetMessagesNotificacion(Autenticador.CurrentAccount.MAR_Setting2.Sesion).msj;
//        if (mensajes != null)
//        {
//            foreach (var item in mensajes)
//            {
//                ViewModel.Mensajes2.Add(item);

//            }

//            for (var i = 0; i < mensajes.Length; i++)
//            {
//                cantidad = $"Tienes {i + 1} nuevos mensajes.";
//            }

//            (Application.Current.MainWindow as ClienteMarWPFWin7.UI.MainWindow).MensajesAlerta(cantidad, "Info");
//        }
//        else
//        {
//            return;
//        }

//    }
//    catch (Exception)
//    {

//    }
//}