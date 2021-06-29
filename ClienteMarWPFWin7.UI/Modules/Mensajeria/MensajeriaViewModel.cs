
#region Namespaces
using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Mensajes;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
#endregion

namespace ClienteMarWPFWin7.UI.Modules.Mensajeria
{
    public class MensajeriaViewModel : BaseViewModel
    {
        #region Campos
        private string _mensaje;
        public ObservableCollection<MAR_Mensaje2> Mensajes = new ObservableCollection<MAR_Mensaje2>();
        #endregion

        #region Comandos
        public ICommand SendMensajeCommand { get; }
        public ICommand GetMensajesCommand { get; }
        #endregion

        #region Acciones
        public Action EscrolearHaciaAbajo { get; set; }
        #endregion

        public MensajeriaViewModel(IAuthenticator autenticador,
                                   IMensajesService mensajesService)
        {
            SendMensajeCommand = new SendMensajeCommand(this, autenticador, mensajesService);
            GetMensajesCommand = new GetMensajesCommand(this, autenticador, mensajesService);
            ScrollDownPendiente = Si;
        }



        #region Propiedades

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

        public ObservableCollection<MAR_Mensaje2> MensajeriaBinding
        {
            get { return Mensajes; }
        }

        public bool  ScrollDownPendiente { get; set; }

        #endregion

        #region Actualiza Propiedades
        public void MensajeFueronActualizado()
        {
            NotifyPropertyChanged(nameof(MensajeriaBinding));
        }
        #endregion

    }
}
