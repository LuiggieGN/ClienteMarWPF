

using System.Collections.ObjectModel;
using ClienteMarWPF.UI.ViewModels.ModelObservable.ComboboxModels;

namespace ClienteMarWPF.UI.ViewModels.Helpers
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
