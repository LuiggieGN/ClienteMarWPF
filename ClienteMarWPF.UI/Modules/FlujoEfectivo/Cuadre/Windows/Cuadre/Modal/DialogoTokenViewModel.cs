#region Namespaces
using System.Windows.Input;
using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands;
#endregion


namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre.Modal
{
    public class DialogoTokenViewModel : BaseViewModel
    {
        #region Fields
        private bool _muestro;
        private string _posicion;
        private string _token;
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

        public DialogoTokenViewModel(string posicion, string token, ActionCommand cancelar, ActionCommand aceptar)
        {
            Error = new MessageViewModel();
           
            CancelarCommand = cancelar;
            
            AceptarCommand = aceptar;

            _posicion = posicion;
            
            _token = token;
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
