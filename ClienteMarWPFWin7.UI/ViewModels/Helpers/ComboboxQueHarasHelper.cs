

using System.Collections.ObjectModel;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable.ComboboxModels;

namespace ClienteMarWPFWin7.UI.ViewModels.Helpers
{
    public static class ComboboxQueHarasHelper
    {
        public static ObservableCollection<ComboboxQueHaras> LeerOpcionesDefaultDeCombobox()
        {
            var comboox = new ObservableCollection<ComboboxQueHaras>();
            comboox.Add(new ComboboxQueHaras() { Key = 1, Value = "Entrada de dinero en caja", Color = "#28A745", Kind = "Add" });
            comboox.Add(new ComboboxQueHaras() { Key = 0, Value = "Salida de dinero en caja", Color = "#DC3545", Kind = "Minus" });
            return comboox;
        }

    }// fin de clase
}
