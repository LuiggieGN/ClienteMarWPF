using ClienteMarWPF.UI.ViewModels.Base;
 
namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels
{
    public class InputCajeraViewModel:BaseViewModel
    {
        private bool _muestro;
        private string _cajera;

        public bool Muestro 
        {
            get
            {
                return _muestro;
            }
            set
            {
                _muestro = value; NotifyPropertyChanged(nameof(Muestro));
            }
        }
        public string Cajera
        {
            get
            {
                return _cajera;
            }
            set
            {
                _cajera = value; NotifyPropertyChanged(nameof(Cajera));
            }
        }


    }// fin de clase
}
