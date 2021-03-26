
using System;
using ClienteMarWPFWin7.UI.ViewModels.Base;
 

namespace ClienteMarWPFWin7.UI.State.Navigators
{
    public class Renavigator<TViewModel> : IRenavigator where TViewModel : BaseViewModel     
    {
        private readonly INavigator _navigator;
        private readonly CreateViewModel<TViewModel> _createViewModel;

        public Renavigator(INavigator navigator, CreateViewModel<TViewModel> createViewModel)
        {
            _navigator = navigator;
            _createViewModel = createViewModel;
        }

        public void Renavigate()
        {
            _navigator.CurrentViewModel = _createViewModel();
        }
    }
}
