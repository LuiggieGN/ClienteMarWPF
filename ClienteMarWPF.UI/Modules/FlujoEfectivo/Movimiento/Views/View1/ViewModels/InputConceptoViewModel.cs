using ClienteMarWPF.UI.ViewModels.Base;
 
namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1.ViewModels
{
    public class InputConceptoViewModel:BaseViewModel
    {
        private bool _muestro;
        private string _texto;

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
        public string Texto
        {
            get
            {
                return _texto;
            }
            set
            {
                _texto = value; NotifyPropertyChanged(nameof(Texto));
            }
        }


    }// fin de clase
}
