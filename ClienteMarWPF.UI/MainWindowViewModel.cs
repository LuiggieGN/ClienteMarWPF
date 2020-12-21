
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Enums;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands;
using ClienteMarWPF.UI.ViewModels.Commands.MainWindow;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels.Factories;

using ClienteMarWPF.UI.State.Configurators;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.MultipleService;
using ClienteMarWPF.Domain.Services.RutaService;


namespace ClienteMarWPF.UI
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private readonly IAuthenticator _autenticador;
        private readonly IViewModelFactory _factoriaViewModel;
        private readonly INavigator _navegadordeModulos;
        private readonly IMultipleService _multipleService;
        private readonly IRutaService _rutaService;
        private readonly ToastViewModel _toast;
        #endregion

        #region Properties
        public BaseViewModel CurrentViewModel => _navegadordeModulos.CurrentViewModel;
        public BancaConfiguracionDTO BancaConfiguracion => _autenticador?.BancaConfiguracion;
        public bool EstaLogueado => _autenticador?.IsLoggedIn ?? false;

        public IAuthenticator AutService => _autenticador;
        public IMultipleService MultipleService => _multipleService;
        public IRutaService RutaService => _rutaService;
        #endregion

        #region Commands
        public ToastViewModel Toast => _toast;
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand IniciarCuadreCommand { get; }
        #endregion

        public MainWindowViewModel(INavigator navegadordeModulos, 
                                   IViewModelFactory factoriaViewModel,
                                   IAuthenticator autenticador,
                                   IMultipleService multipleService,
                                   IRutaService rutaService)
        {

            _toast = new ToastViewModel();

            _autenticador = autenticador;
            _autenticador.CurrentAccountStateChanged += AccountStateChanged;
            _autenticador.CurrentBancaConfiguracionStateChanged += BanConfigStateChanged;
            
            _factoriaViewModel = factoriaViewModel;
            
            _navegadordeModulos = navegadordeModulos;
            _navegadordeModulos.StateChanged += NaviStateChanged;

            _multipleService = multipleService;
            _rutaService = rutaService;

            LogoutCommand = new LogoutCommand(_autenticador, _navegadordeModulos, _factoriaViewModel);

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navegadordeModulos, _factoriaViewModel);
            UpdateCurrentViewModelCommand.Execute(Modulos.Login);

            IniciarCuadreCommand = new IniciarCuadreCommand(this);
        }

        private void AccountStateChanged()
        {
            NotifyPropertyChanged(nameof(EstaLogueado));
        }
        private void BanConfigStateChanged()
        {
            NotifyPropertyChanged(nameof(BancaConfiguracion));
        }
        private void NaviStateChanged()
        {
            NotifyPropertyChanged(nameof(CurrentViewModel));
        }

      






    }
}
