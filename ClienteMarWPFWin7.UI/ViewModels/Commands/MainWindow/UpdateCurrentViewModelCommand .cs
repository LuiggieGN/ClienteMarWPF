using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;


using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.UI.Modules.Home;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Factories;


namespace ClienteMarWPFWin7.UI.ViewModels.Commands.MainWindow
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly INavigator _navigator;
        private readonly IViewModelFactory _viewModelFactory;

        public UpdateCurrentViewModelCommand(INavigator navigator, IViewModelFactory viewModelFactory)
        {
            _navigator = navigator;
            _viewModelFactory = viewModelFactory;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {

            if (HomeViewModel.Worker != null && HomeViewModel.Worker.WorkerSupportsCancellation)
            {
                HomeViewModel.Worker.CancelAsync();
            }

            if (SorteosView.Timer != null )
            {
                SorteosView.Timer.Stop();
                SorteosView.Timer = null;
            }

            if (parameter is Modulos)
            {
                Modulos viewType = (Modulos)parameter;

                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }



    }
}
