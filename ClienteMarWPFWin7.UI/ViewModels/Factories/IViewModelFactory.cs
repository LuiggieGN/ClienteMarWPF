
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.UI.ViewModels.Base;

namespace ClienteMarWPFWin7.UI.ViewModels.Factories
{
    public interface IViewModelFactory
    {
        BaseViewModel CreateViewModel(Modulos viewType);
    }
}
