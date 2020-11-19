using ClienteMarWPF.Domain.Services.MensajesService;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Mensajes;
using MarPuntoVentaServiceReference;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Mensajeria
{
    public class MensajeriaViewModel : BaseViewModel
    {
        public ICommand SendMensajeCommand { get; }
        public ICommand GetMensajesCommand { get; }
        public ObservableCollection<MAR_Mensaje> MensajeriaBinding = new ObservableCollection<MAR_Mensaje>();

        public MensajeriaViewModel(IAuthenticator autenticador, IMensajesService mensajesService)
        {
            SendMensajeCommand = new SendMensajeCommand(this, autenticador, mensajesService);
            GetMensajesCommand = new GetMensajesCommand(this, autenticador, mensajesService);
            //MensajeriaBinding = new ObservableCollection<MAR_Mensaje>();
            //GetMensajesCommand.Execute(null);

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
