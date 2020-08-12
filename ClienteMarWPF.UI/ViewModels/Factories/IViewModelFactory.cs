
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.UI.ViewModels.Base;

namespace ClienteMarWPF.UI.ViewModels.Factories
{
    public interface IViewModelFactory
    {
        BaseViewModel CreateViewModel(ViewTypeEnum viewType);
    }
}
