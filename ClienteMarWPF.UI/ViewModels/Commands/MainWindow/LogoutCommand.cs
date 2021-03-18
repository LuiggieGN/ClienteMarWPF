using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.UI.Extensions;
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
                
                if (MainWindowViewModel.CuadreV1 != null  && IsWindowOpen("CuadreLoginWindow"))
                {
                    try
                    {
                        MainWindowViewModel.CuadreV1.Close();
                    }
                    catch  
                    { 
                    }
                }

                if (MainWindowViewModel.CuadreV2 != null && IsWindowOpen("CuadreVista"))
                {
                    try
                    {
                        MainWindowViewModel.CuadreV2.Close();
                    }
                    catch  
                    {
                    }
                }


                _authenticator.CerrarSesion();

                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(Modulos.Login);

                InactividadExtension.RemoveInactividad();
            }
        }




        public static bool IsWindowOpen (string name)  
        {
            return Application.Current.Windows.OfType<Window>().Any(w => w.Name.Equals(name));
        }



    }
}
