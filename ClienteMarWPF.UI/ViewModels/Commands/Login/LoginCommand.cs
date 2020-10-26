
using ClienteMarWPF.Domain.Exceptions;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.Modules.Login;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ClienteMarWPF.UI.ViewModels.Commands.Login
{
    public class LoginCommand : AsyncCommandBase
    {
        private readonly LoginViewModel _loginViewModel;
        private readonly IAuthenticator _autenticador;
        private readonly IRenavigator _renavigator;

        public LoginCommand(LoginViewModel loginViewModel, IAuthenticator autenticador, IRenavigator renavigator)
        {
            _loginViewModel = loginViewModel;
            _autenticador = autenticador;
            _renavigator = renavigator;
        }
        

        public override async Task ExecuteAsync(object parameter)
        {

            _loginViewModel.ErrorMessage = string.Empty;


            try
            {
                await _autenticador.Login(_loginViewModel.Username, parameter.ToString());
              
                _renavigator.Renavigate();

            }
            catch (UserNotFoundException)
            {
                _loginViewModel.ErrorMessage = "Credenciales inválidas";
            }
            catch (InvalidPasswordException) 
            {
                _loginViewModel.ErrorMessage = "Credenciales inválidas";
            }
            catch (Exception)
            {
                _loginViewModel.ErrorMessage = "Verificar conexión de internet";
            }
        }




    }// fin de Clase LoginCommand

}// fin de namespace 
