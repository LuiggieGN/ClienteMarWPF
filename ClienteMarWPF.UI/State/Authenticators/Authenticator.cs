using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Services.AuthenticationService;

using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.State.Configurators;
using ClienteMarWPF.Domain.Exceptions;

namespace ClienteMarWPF.UI.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {

        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;
        private readonly IConfiguratorStore _configuratorStore;
        private readonly IBancaService _bancaService;


        public Authenticator(
            IAuthenticationService authenticationService,
            IAccountStore accountStore,
            IConfiguratorStore configuratorStore,
            IBancaService bancaService
        )
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
            _configuratorStore = configuratorStore;
            _bancaService = bancaService;
        }

        public CuentaDTO CurrentAccount
        {
            get
            {
                return _accountStore.CurrentAccount;
            }
            private set
            {
                _accountStore.CurrentAccount = value;
                CurrentAccountStateChanged?.Invoke();
            }
        }
        public BancaConfiguracionDTO BancaConfiguracion
        {
            get
            {
                return _configuratorStore.CurrentBancaConfiguracion;
            }
            set
            {
                _configuratorStore.CurrentBancaConfiguracion = value;
                CurrentBancaConfiguracionStateChanged?.Invoke();
            }
        }



        public bool IsLoggedIn => CurrentAccount != null;





        public void IniciarSesion(string usuario, string clave, int bancaid, string ipaddress)
        {
            try
            {


                CurrentAccount = _authenticationService.Logon2(usuario, clave, bancaid, ipaddress);

                BancaConfiguracion = _bancaService.LeerBancaConfiguraciones(bancaid);

            } 
            catch (UserNotFoundException ex1)
            {
                CerrarSesion();
                throw ex1;
            }
            catch (InvalidPasswordException ex2)
            {
                CerrarSesion();
                throw ex2;
            }
            catch (BancaConfiguracionesException ex3)
            {
                CerrarSesion();
                throw ex3;
            }
            catch (Exception ex)
            {
                CerrarSesion();
                throw ex;
            }
        }



 


        public void CerrarSesion()
        {
            CurrentAccount = null;
            BancaConfiguracion = null;
        }






        public event Action CurrentAccountStateChanged;
        public event Action CurrentBancaConfiguracionStateChanged;


    }// fin de clase Authenticator
}// fin de namespace Authenticators

















