using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;


using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.UI.Modules.Home;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Factories;


namespace ClienteMarWPF.UI.ViewModels.Commands.MainWindow
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

            if (parameter is Modulos)
            {
                Modulos viewType = (Modulos)parameter;

                _navigator.CurrentViewModel = _viewModelFactory.CreateViewModel(viewType);
            }
        }



    }
}
