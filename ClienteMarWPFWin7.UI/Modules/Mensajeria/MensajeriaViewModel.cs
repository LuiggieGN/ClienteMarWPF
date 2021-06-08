using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Mensajes;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ClienteMarWPFWin7.UI.Modules.Mensajeria
{
    public class MensajeriaViewModel : BaseViewModel
    {
        public ICommand SendMensajeCommand { get; }
        public ICommand GetMensajesCommand { get; }
        public ObservableCollection<MAR_Mensaje2> Mensajes = new ObservableCollection<MAR_Mensaje2>();
        //public ObservableCollection<MAR_Mensaje> Mensajes2 = new ObservableCollection<MAR_Mensaje>();
        public MensajeriaViewModel(IAuthenticator autenticador, IMensajesService mensajesService)
        {
            SendMensajeCommand = new SendMensajeCommand(this, autenticador, mensajesService);
            GetMensajesCommand = new GetMensajesCommand(this, autenticador, mensajesService);
            //MensajeriaBinding = new ObservableCollection<MAR_Mensaje>();
            //GetMensajesCommand.Execute(null);

        }

        public ObservableCollection<MAR_Mensaje2> MensajeriaBinding { 
        get { return Mensajes; }
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
