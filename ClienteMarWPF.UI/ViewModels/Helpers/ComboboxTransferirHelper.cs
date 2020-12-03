

using System.Collections.ObjectModel;
using ClienteMarWPF.UI.ViewModels.ModelObservable.ComboboxModels;

namespace ClienteMarWPF.UI.ViewModels.Helpers
{
    public static class ComboboxTransferirHelper
    {
        public static ObservableCollection<ComboboxTransferir> LeerDefaults()
        {
            var comboox = new ObservableCollection<ComboboxTransferir>();
            comboox.Add(new ComboboxTransferir() { Key = 1, Value = "True", Color = "#28A745", KindDe = "User", De = "Gestor", KindA = "Home", A = "Banca" });
            comboox.Add(new ComboboxTransferir() { Key = 0, Value = "False", Color = "#DC3545", KindDe = "Home", De = "Banca", KindA = "User", A = "Gestor" });
            return comboox;
        }

    }// fin de clase
}
