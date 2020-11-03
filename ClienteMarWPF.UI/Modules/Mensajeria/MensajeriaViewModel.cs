using ClienteMarWPF.Domain.Services.MensajesService;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Mensajes;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Mensajeria
{
    public class MensajeriaViewModel: BaseViewModel
    {
        public ICommand SendMensajeCommand { get; }
        public ICommand GetMensajesCommand { get; }
        public ObservableCollection<MensajesObservable> MensajeriaBinding;

        public MensajeriaViewModel(IAuthenticator autenticador, IMensajesService mensajesService)
        {
            SendMensajeCommand = new SendMensajeCommand(this, autenticador, mensajesService);
            GetMensajesCommand = new GetMensajesCommand(this, autenticador, mensajesService);
            MensajeriaBinding = new ObservableCollection<MensajesObservable>();
            GetMensajesCommand.Execute(null);

        }

        #region PropertyOfView
        //###########################################################
        private string _mensaje;
        //###########################################################
        public string Mensaje
        {
            get
            {
                return _mensaje;
            }
            set
            {
                _mensaje = value;
                NotifyPropertyChanged(nameof(Mensaje));
            }
        }
        //###########################################################
        #endregion

    }
}
