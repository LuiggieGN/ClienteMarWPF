using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;


using ClienteMarWPF.Domain.Enums;

using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Factories;


namespace ClienteMarWPF.UI.ViewModels.Commands.MainWindow
{
    public class LogoutCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IAuthenticator _authenticator;
        private readonly INavigator _navigator;
        private readonly IViewModelFactory _viewModelFactory;
        
        public LogoutCommand(IAuthenticator authenticator, INavigator navigator, IViewModelFactory viewModelFactory)
        {
            _authenticator = authenticator;
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_authenticator != null && _navigator != null && _viewModelFactory != null)
            {
                _authenticator.CerrarSesion();
                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(Modulos.Login);
            }
        }



    }
}
