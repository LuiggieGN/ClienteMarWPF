using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.Domain.Services.AuthenticationService;

using ClienteMarWPF.UI.State.Accounts;


namespace ClienteMarWPF.UI.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;


        public Authenticator(
            IAuthenticationService authenticationService,
            IAccountStore accountStore
        )
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
        }

        public CuentaUsuario CurrentAccount
        {
            get
            {
                return _accountStore.CurrentAccount;
            }
            private set
            {
                _accountStore.CurrentAccount = value;
                StateChanged?.Invoke();
            }
        }

        public bool IsLoggedIn => CurrentAccount != null;

        public event Action StateChanged;

        public async Task Login(string username, string password)
        {
            CurrentAccount = await _authenticationService.Login(username, password);
        }

        public void Logout()
        {
            CurrentAccount = null;
        }





    }// fin de clase Authenticator
}// fin de namespace Authenticators
