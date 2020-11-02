
using ClienteMarWPF.UI.ViewModels.Commands.Login;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.LocalClientSetting;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;


namespace ClienteMarWPF.UI.Modules.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private string _username;
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


        public MessageViewModel ErrorMessageViewModel { get; }

        public string ErrorMessage
        {
            set => ErrorMessageViewModel.Message = value;
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel(IAuthenticator autenticador, IRenavigator renavigator, ILocalClientSettingStore localclientsettings)
        {
            ErrorMessageViewModel = new MessageViewModel();  LoginCommand = new LoginCommand(this, autenticador, renavigator, localclientsettings);
        }



    }
}
