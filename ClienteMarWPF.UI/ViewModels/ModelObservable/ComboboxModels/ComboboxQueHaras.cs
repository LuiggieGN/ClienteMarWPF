using ClienteMarWPF.UI.ViewModels.Base;
 
namespace ClienteMarWPF.UI.ViewModels.ModelObservable.ComboboxModels
{
    public class ComboboxQueHaras: ComboboxBase
    {
        private string _kind;

        public string Kind
        {
            get
            {
                return _kind;
            }
            set
            {
                _kind = value; NotifyPropertyChanged(nameof(Kind));
            }
        }



    }
}
