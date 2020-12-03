
namespace ClienteMarWPF.UI.ViewModels.ModelObservable.ComboboxModels
{
    public class ComboboxTransferir : ComboboxBase
    {
        private readonly string _deTexto = "De: ";
        private string _kindDe;
        private string _de;

        private readonly string _aTexto = "A: ";
        private string _kindA;
        private string _a;

        #region De
        public string DeTexto
        {
            get => _deTexto;
        }
        public string KindDe
        {
            get => _kindDe;
            set
            {
                _kindDe = value; NotifyPropertyChanged(nameof(KindDe));
            }
        }
        public string De
        {
            get => _de;
            set
            {
                _de = value; NotifyPropertyChanged(nameof(De));
            }
        }
        #endregion

        #region A
        public string ATexto
        {
            get => _aTexto;
        }
        public string KindA
        {
            get => _kindA;
            set
            {
                _kindA = value; NotifyPropertyChanged(nameof(KindA));
            }
        }
        public string A
        {
            get => _a;
            set
            {
                _a = value; NotifyPropertyChanged(nameof(A));
            }
        }
        #endregion


    }
}
