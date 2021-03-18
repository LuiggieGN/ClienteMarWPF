
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Services.AuthenticationService;
using ClienteMarWPF.Domain.Services.CajaService;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.State.Configurators;
using ClienteMarWPF.UI.State.BancaBalanceStore;

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Threading;
using System.Threading;
using System.Timers;

namespace ClienteMarWPF.UI.State.Authenticators
{
    public class Authenticator : IAuthenticator
    {
        #region Fields
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountStore _accountStore;
        private readonly IConfiguratorStore _configuratorStore;
        private readonly IPermisosStore _permisosStore;
        private readonly IBancaBalanceStore _bancaBalanceStore;
        private readonly IBancaService _bancaService;
        private readonly ICajaService _cajaService;
        private bool _isLoggedIn;
        #endregion

        #region Constructor
        public Authenticator(
            IAuthenticationService authenticationService,
            IAccountStore accountStore,
            IConfiguratorStore configuratorStore,
            IBancaBalanceStore bancaBalanceStore,
            IBancaService bancaService,
            ICajaService cajaService,
            IPermisosStore permisosStore
        )
        {
            _authenticationService = authenticationService;
            _accountStore = accountStore;
            _configuratorStore = configuratorStore;
            _bancaBalanceStore = bancaBalanceStore;
            _bancaService = bancaService;
            _cajaService = cajaService;
            _permisosStore = permisosStore;
        }
        #endregion

        #region Properties
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
        public BancaBalanceViewModel BancaBalance
        {
            get
            {
                return _bancaBalanceStore.CurrentBancaBalance;
            }
            set
            {
                _bancaBalanceStore.CurrentBancaBalance = value;
                CurrentBancaBalanceStateChanged?.Invoke();
            }
        }
        //public bool IsLoggedIn => CurrentAccount != null;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set
            {
                _isLoggedIn = value;
                IsLoggedInStateChanged?.Invoke();
            }
        }


        public PermisosDTO Permisos
        {
            get
            {
                return _permisosStore.Permisos;
            }
            set
            {
                _permisosStore.Permisos = value;
                CurrentPermisosStateChanged?.Invoke();
            }
        }

        #endregion

        public void IniciarSesion(string usuario, string clave, int bancaid, string ipaddress)
        {
            try
            {
                CurrentAccount = _authenticationService.Logon2(usuario, clave, bancaid, ipaddress);

                BancaConfiguracion = _bancaService.LeerBancaConfiguraciones(bancaid);

                SetearPuntoDeVentaPermisos();

                RefrescarBancaBalance();

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


        private void SetearPuntoDeVentaPermisos()
        {
            var permisos = new PermisosDTO();

            var MoreOptions = this.CurrentAccount.MAR_Setting2.MoreOptions.ToList();

            for (var i = 0; i < MoreOptions.Count; i++)
            {
                var ConfigValue = MoreOptions[i];
                var ArrayValue = ConfigValue.Split("|");
                if (ArrayValue[0] == "BANCA_VENDE_CINCOMINUTOS")
                {
                    permisos.CincoMinutos = true;

                }
                else if (ArrayValue[0] == "BANCA_PAGA_SERVICIOS")
                {
                    permisos.Servicios = true;
                }
                else if (ArrayValue[0] == "BANCA_VENDE_TARJETAS")
                {
                    permisos.PuedeVenderRecargas = true;
                }
                else if (ArrayValue[0] == "BANCA_VENDE_BINGO")
                {
                    permisos.PuedeVenderBingo = true;
                }
            }

            Permisos = permisos;
        }

        public void RefrescarBancaBalance()
        {
            var bancaBalance = new BancaBalanceViewModel();

            if (BancaConfiguracion != null &&
                BancaConfiguracion.ControlEfectivoConfigDto != null &&
                BancaConfiguracion.CajaEfectivoDto != null &&
                BancaConfiguracion.ControlEfectivoConfigDto.PuedeUsarControlEfectivo == true &&
                BancaConfiguracion.ControlEfectivoConfigDto.BancaYaInicioControlEfectivo == true)
            {
                try
                {
                    bancaBalance.Balance = _cajaService.LeerCajaBalance(BancaConfiguracion.CajaEfectivoDto.CajaID);
                    bancaBalance.StrBalance = $"Balance: {bancaBalance.Balance.ToString("$ #,##0.00", CultureInfo.CreateSpecificCulture("en-US"))}";
                }
                catch
                {
                    bancaBalance.Balance = 0;
                    bancaBalance.StrBalance = "..";
                }
                bancaBalance.TieneBalance = true;
            }
            else
            {

                bancaBalance.Balance = 0;
                bancaBalance.StrBalance = string.Empty;
                bancaBalance.TieneBalance = false;
            }

            BancaBalance = bancaBalance;
        }

        public void CerrarSesion()
        {
            CurrentAccount = null;
            BancaConfiguracion = null;
            IsLoggedIn = false;
        }






        public event Action CurrentAccountStateChanged;
        public event Action CurrentBancaConfiguracionStateChanged;
        public event Action CurrentBancaBalanceStateChanged;
        public event Action IsLoggedInStateChanged;
        public event Action CurrentPermisosStateChanged;
    }// fin de clase Authenticator
}// fin de namespace Authenticators

















