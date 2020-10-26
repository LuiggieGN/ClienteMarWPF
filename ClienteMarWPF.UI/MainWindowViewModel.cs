
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

using ClienteMarWPF.UI.State.Configurators;
using ClienteMarWPF.Domain.Models.Dtos;

namespace ClienteMarWPF.UI
{
    public class MainWindowViewModel : BaseViewModel
    {
        public static FlujoServices.MAR_Session MarSession = new FlujoServices.MAR_Session();
        private readonly IViewModelFactory factoriaViewModel;
        private readonly INavigator navegadordeModulos;
        private readonly IAuthenticator autenticador;

        public bool EstaLogueado => autenticador?.IsLoggedIn??false;
        


        public BaseViewModel CurrentViewModel => navegadordeModulos.CurrentViewModel;
        public BancaConfiguracion BancaConfiguracion => autenticador?.BancaConfiguracion;



        public ICommand UpdateCurrentViewModelCommand { get; }
        public ICommand LogoutCommand { get; }


        public MainWindowViewModel(INavigator navegadordeModulos, IViewModelFactory factoriaViewModel, IAuthenticator autenticador)
        {
            LogoutCommand = new LogoutCommand(autenticador, navegadordeModulos, factoriaViewModel);

            this.factoriaViewModel = factoriaViewModel;       
            
            this.autenticador = autenticador;
            this.autenticador.CurrentAccountStateChanged += Authenticator_AccountStateChanged;
            this.autenticador.CurrentBancaConfiguracionStateChanged += Authenticator_BancaConfiguracionStateChanged;

            this.navegadordeModulos = navegadordeModulos;
            this.navegadordeModulos.StateChanged += Navigator_StateChanged;


            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(navegadordeModulos, factoriaViewModel);
            UpdateCurrentViewModelCommand.Execute(ViewTypeEnum.Login);
        }


        private void Authenticator_AccountStateChanged()
        {
            NotifyPropertyChanged(nameof(EstaLogueado));
        }

        private void Authenticator_BancaConfiguracionStateChanged()
        {
            NotifyPropertyChanged(nameof(BancaConfiguracion));
        }

        private void Navigator_StateChanged()
        {
            NotifyPropertyChanged(nameof(CurrentViewModel));
        }

      






    }
}
