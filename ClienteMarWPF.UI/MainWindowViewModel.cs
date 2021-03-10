
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
using ClienteMarWPF.UI.State.CuadreBuilders;
using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.ViewModels.Factories;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.MultipleService;
using ClienteMarWPF.Domain.Services.RutaService;
using ClienteMarWPF.Domain.Services.CuadreService;


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
        private readonly ICuadreBuilder _cuadreBuilder;
        private readonly ILocalClientSettingStore _localClientSetting;
        private readonly ToastViewModel _toast;
        #endregion

        #region Properties
        public BaseViewModel CurrentViewModel => _navegadordeModulos.CurrentViewModel;
        public BancaConfiguracionDTO BancaConfiguracion => _autenticador?.BancaConfiguracion;
        public string GlobalTerminalId => _autenticador?.BancaConfiguracion?.BancaDto?.BancaID.ToString() ?? "- -";
        public string GlobalTerminalNombre => _autenticador?.BancaConfiguracion?.BancaDto?.BanContacto ?? "- -";

        public PermisosDTO Permisos => _autenticador?.Permisos;

        #region BancaBalance
        public string StrBancaBalance => _autenticador?.BancaBalance?.StrBalance ?? string.Empty;
        public bool VerBancaBalanceEnCaja => _autenticador?.BancaBalance?.TieneBalance ?? false;
        #endregion

        public bool EstaLogueado => _autenticador?.IsLoggedIn ?? false;
        public IAuthenticator AutService => _autenticador;
        public IMultipleService MultipleService => _multipleService;
        public IRutaService RutaService => _rutaService;
        public ICuadreBuilder CuadreBuilder => _cuadreBuilder;
        public ILocalClientSettingStore LocalClientSetting => _localClientSetting;


        #endregion

        #region Commands
        public ToastViewModel Toast => _toast;
        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand IniciarCuadreCommand { get; }
        public ICommand CambiarTerminalConfiguracionLocalCommand { get; }
        #endregion

        public MainWindowViewModel(INavigator navegadordeModulos,
                                   IViewModelFactory factoriaViewModel,
                                   IAuthenticator autenticador,
                                   IMultipleService multipleService,
                                   IRutaService rutaService,
                                   ICuadreBuilder cuadreBuilder,
                                   ILocalClientSettingStore localClientSetting)
        {

            _toast = new ToastViewModel();

            _autenticador = autenticador;
            _autenticador.CurrentAccountStateChanged += AccountStateChanged;
            _autenticador.CurrentBancaConfiguracionStateChanged += BanConfigStateChanged;
            _autenticador.CurrentBancaBalanceStateChanged += BanBalanceStateChanged;
            _autenticador.IsLoggedInStateChanged += LoggedInStateChanged;
            _autenticador.CurrentPermisosStateChanged += PermisosStateChanged;

            _factoriaViewModel = factoriaViewModel;


            _navegadordeModulos = navegadordeModulos;
            _navegadordeModulos.StateChanged += NaviStateChanged;


            _multipleService = multipleService;
            _rutaService = rutaService;
            _cuadreBuilder = cuadreBuilder;
            _localClientSetting = localClientSetting;


            LogoutCommand = new LogoutCommand(_autenticador, _navegadordeModulos, _factoriaViewModel);

            IniciarCuadreCommand = new IniciarCuadreCommand(this);

            CambiarTerminalConfiguracionLocalCommand = new CambiarTerminalConfiguracionLocalCommand(this);

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navegadordeModulos, _factoriaViewModel);
            UpdateCurrentViewModelCommand.Execute(Modulos.Login);



        }



        private void AccountStateChanged()
        {
            NotifyPropertyChanged(nameof(EstaLogueado),
                                  nameof(GlobalTerminalId),
                                  nameof(GlobalTerminalNombre));
        }
        private void BanConfigStateChanged()
        {
            NotifyPropertyChanged(nameof(BancaConfiguracion));
        }
        private void BanBalanceStateChanged()
        {
            NotifyPropertyChanged(nameof(VerBancaBalanceEnCaja),
                                  nameof(StrBancaBalance));
        }
        private void NaviStateChanged()
        {
            NotifyPropertyChanged(nameof(CurrentViewModel));
        }
        private void LoggedInStateChanged()
        {
            NotifyPropertyChanged(nameof(EstaLogueado));
        }

        private void PermisosStateChanged()
        {
            NotifyPropertyChanged(nameof(Permisos));
        }



    }
}
