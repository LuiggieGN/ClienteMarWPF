
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Enums;

using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels.Factories;
 



namespace ClienteMarWPF.UI
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IViewModelFactory _viewModelFactory;
        private readonly INavigator _navigator;
        private readonly IAuthenticator _authenticator;

        public bool IsLoggedIn => _authenticator?.IsLoggedIn??false;
        public BaseViewModel CurrentViewModel => _navigator.CurrentViewModel;
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand LogoutCommand { get; }


        public MainWindowViewModel(INavigator navigator, IViewModelFactory viewModelFactory, IAuthenticator authenticator)
        {
            LogoutCommand = new LogoutCommand(authenticator, navigator, viewModelFactory);


            _viewModelFactory = viewModelFactory;
            _navigator = navigator;
            _authenticator = authenticator;

            _navigator.StateChanged += Navigator_StateChanged;
            _authenticator.StateChanged += Authenticator_StateChanged;

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navigator, _viewModelFactory);
            UpdateCurrentViewModelCommand.Execute(ViewTypeEnum.Login);
        }


        private void Authenticator_StateChanged()
        {
            NotifyPropertyChanged(nameof(IsLoggedIn));
        }

        private void Navigator_StateChanged()
        {
            NotifyPropertyChanged(nameof(CurrentViewModel));
        }



    }
}
