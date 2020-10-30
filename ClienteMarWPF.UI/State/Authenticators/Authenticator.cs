using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.Domain.Services.AuthenticationService;

using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.State.Configurators;

namespace ClienteMarWPF.UI.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;
        private readonly IConfiguratorStore _configuratorStore;

        public Authenticator(
            IAuthenticationService authenticationService,
            IAccountStore accountStore,
            IConfiguratorStore configuratorStore
        )
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
            _configuratorStore = configuratorStore;
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


        public event Action CurrentAccountStateChanged;
        public event Action CurrentBancaConfiguracionStateChanged;


        public void IniciarSesion(string usuario, string clave, int bancaid, string ipaddress)
        {
            CurrentAccount = _authenticationService.Login(usuario, clave, bancaid, ipaddress);

            definirObtenerConfiguracionesDeBanca();
        }


        public void definirObtenerConfiguracionesDeBanca()
        {
            //!! pendiente Si la banca no tiene caja hay que crearsela desde aqui

            BancaConfiguracion = new BancaConfiguracionDTO()
            {
                BancaId = 6,
                BancaCajaId = 42,
                BancaControlEfectivoConfig = new BancaControlEfectivoConfigDTO()
                {
                    ControlEfectivoEstaActivo = true,
                    BancaInicioFlujoEfectivo = false         //-- equivale a posee cuadre inicial
                }
            };

        }


        public void CerrarSesion()
        {
            CurrentAccount = null;
        }





    }// fin de clase Authenticator
}// fin de namespace Authenticators
