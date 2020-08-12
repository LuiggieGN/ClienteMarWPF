
using System;
using ClienteMarWPF.UI.ViewModels.Base;
 

namespace ClienteMarWPF.UI.State.Navigators
{
    public class Navigator : INavigator
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel {
            get => _currentViewModel;
            set {
                _currentViewModel = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
