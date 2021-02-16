
using ClienteMarWPF.UI.ViewModels.Commands.Login;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.ViewModels.Helpers;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
 
using System.Windows.Input;


namespace ClienteMarWPF.UI.Modules.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
        private bool _cargando;

        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                NotifyPropertyChanged(nameof(Username));
            }
        }

        public bool Cargando
        {
            get
            {
                return _cargando;
            }
            set
            {
                _cargando = value;
                NotifyPropertyChanged(nameof(Cargando));
            }
        }

        public MessageViewModel ErrorMessageViewModel { get; }
            
        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthenticator autenticador, IRenavigator renavigator, ILocalClientSettingStore localclientsettings)
        {
            Cargando = Booleano.No;
            ErrorMessageViewModel = new MessageViewModel(); 
            LoginCommand = new LoginCommand(this, autenticador, renavigator, localclientsettings);
        }



    }
}
