using ClienteMarWPFWin7.UI.ViewModels.Base;

namespace ClienteMarWPFWin7.UI.ViewModels.ModelObservable.ComboboxModels
{
    public class ComboboxBase : BaseViewModel
    {
        private int _key;
        private string _value;
        private string _color;

        public virtual int Key
        {
            get => _key;
            set
            {
                _key = value; NotifyPropertyChanged(nameof(Key));
            }
        }
        public virtual string Value
        {
            get => _value;
            set
            {
                _value = value; NotifyPropertyChanged(nameof(Value));
            }

        }

        public string Color
        {
            get => _color;
            set
            {
                _color = value; NotifyPropertyChanged(nameof(Color));
            }
        }



    }
}
