#region Namespaces
using System.Windows.Input;
using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands;
#endregion


namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.Modal
{
    public class DialogoTokenViewModel : BaseViewModel
    {
        #region Fields
        private bool _muestro;
        private string _posicion;
        private string _token;
        private string _pinGeneral;
        #endregion

        #region Properties
        public bool MuestroDialogo
        {
            get
            {
                return _muestro;
            }
            private set
            {
                _muestro = value; NotifyPropertyChanged(nameof(MuestroDialogo));
            }
        }
        public string Posicion => _posicion;
        public string Token => _token;
        public string PinGeneral => _pinGeneral;
        #endregion

        #region ErrMensaje
        public MessageViewModel Error { get; }
        public string ErrMensaje
        {
            set => Error.Message = value;
        }
        #endregion
        
        #region Comandos
        public ICommand AceptarCommand { get; }
        public ICommand CancelarCommand { get; }
        #endregion

        public DialogoTokenViewModel(string posicion, string token, string pinGeneral, ActionCommand cancelar, ActionCommand aceptar)
        {
            Error = new MessageViewModel();
           
            CancelarCommand = cancelar;
            
            AceptarCommand = aceptar;

            _posicion = posicion;            
            _token = token;
            _pinGeneral = pinGeneral;
        }


        public void Mostrar()
        {
            MuestroDialogo = true;
        }

        public void Ocultar()
        {
            MuestroDialogo = false;
        }

    } // fin de clase
}
