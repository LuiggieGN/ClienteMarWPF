
using System;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPFWin7.Domain.Enums;

using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands;
using ClienteMarWPFWin7.UI.ViewModels.Commands.MainWindow;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.CuadreBuilders;
using ClienteMarWPFWin7.UI.State.LocalClientSetting;
using ClienteMarWPFWin7.UI.ViewModels.Factories;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.MultipleService;
using ClienteMarWPFWin7.Domain.Services.RutaService;
using ClienteMarWPFWin7.Domain.Services.CuadreService;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.CuadreLogin;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Cuadre.Windows.Cuadre;
using System.Windows.Threading;
using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.UI.Modules.Mensajeria;
using System.Linq;

namespace ClienteMarWPFWin7.UI
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
        private readonly InicioPCResultDTO _inicioPC;
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

        private readonly IMensajesService MensajesService;
        public InicioPCResultDTO InicioPC => _inicioPC;
        public Action ReIniciarApp { get; }
        public App Aplicacion { get; }
        public ToastViewModel Toast => _toast;
        #endregion

        #region Commands
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
                                   ILocalClientSettingStore localClientSetting,
                                   IMensajesService mensajesService,
                                   InicioPCResultDTO inicioPC,
                                   Action reInicioApp,
                                   App aplicativo

            )
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
            MensajesService = mensajesService;

            _inicioPC = inicioPC;

            ReIniciarApp = reInicioApp;

            Aplicacion = aplicativo;

            LogoutCommand = new LogoutCommand(_autenticador, _navegadordeModulos, _factoriaViewModel);

            IniciarCuadreCommand = new IniciarCuadreCommand(this);

            CambiarTerminalConfiguracionLocalCommand = new CambiarTerminalConfiguracionLocalCommand(this);

            UpdateCurrentViewModelCommand = new UpdateCurrentViewModelCommand(_navegadordeModulos, _factoriaViewModel);
            UpdateCurrentViewModelCommand.Execute(Modulos.Login);

            if (TimerConsultaMensaje != null)
            {
                TimerConsultaMensaje.Stop();
                TimerConsultaMensaje = null;
            }

            TimerConsultaMensaje = new DispatcherTimer();
            TimerConsultaMensaje.Tick += (sender, args) => CuentaMensaje();
            TimerConsultaMensaje.Interval = TimeSpan.FromMinutes(2);


        }


        #region metodos
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


        #endregion




        private void CuentaMensaje()
        {

            if (!CantidadMensajesEnviadoPorOtro.HasValue)
            {
                try
                {
                    if (_autenticador != null && _autenticador.CurrentAccount != null && _autenticador.CurrentAccount.MAR_Setting2 != null)
                    {
                        var mensajes = MensajesService.GetMessages(_autenticador.CurrentAccount.MAR_Setting2.Sesion).msj;
                        if (mensajes == null || mensajes.Length == 0)
                        {
                            CantidadMensajesEnviadoPorOtro = null;
                        }
                        else
                        {
                            int bancaid = _autenticador.BancaConfiguracion.BancaDto.BancaID;
                            long cuentaSegunCondicion = mensajes.LongCount(x => x.BancaID != bancaid);
                            CantidadMensajesEnviadoPorOtro = cuentaSegunCondicion == 0 ? null : (long?)cuentaSegunCondicion;
                        }
                    }

                }
                catch
                {
                    CantidadMensajesEnviadoPorOtro = null;
                }

            }
            else
            {

                try
                {
                    if (!ConsultaMensajeIsBusy)
                    {
                        if (_autenticador != null && _autenticador.CurrentAccount != null && _autenticador.CurrentAccount.MAR_Setting2 != null)
                        {

                            long nuevosMensajes;
                            ConsultaMensajeIsBusy = true;


                            var mensajes = MensajesService.GetMessages(_autenticador.CurrentAccount.MAR_Setting2.Sesion).msj;
                            if (mensajes == null || mensajes.Length == 0)
                            {
                                ConsultaMensajeIsBusy = false;
                                return;
                            }
                            else
                            {
                                int bancaid = _autenticador.BancaConfiguracion.BancaDto.BancaID;
                                nuevosMensajes = mensajes.LongCount(x => x.BancaID != bancaid);
                                
                            }

                            if (nuevosMensajes > CantidadMensajesEnviadoPorOtro.Value)
                            {
                                TimerConsultaMensaje?.Stop();
                                CantidadMensajesEnviadoPorOtro = nuevosMensajes;
                                int bancaid = _autenticador.BancaConfiguracion.BancaDto.BancaID;
                                var destino = mensajes.Where(x => x.BancaID != bancaid).Select(y => y.Origen);
                                var contenido = mensajes.Where(x => x.BancaID != bancaid).Select(y => y.Contenido);
                                NotificacionMensajeWindow modal = new NotificacionMensajeWindow(destino.ToArray()[0].ToString(),contenido.ToArray()[0].ToString());
                                modal.ShowDialog();
                                Console.WriteLine(modal);
                            }
                            ConsultaMensajeIsBusy = false;

                        }
                    }

                }
                catch
                {

                }
            }
        }

        public static DispatcherTimer TimerConsultaMensaje { get; set; }
        public static long? CantidadMensajesEnviadoPorOtro = null;
        public static bool ConsultaMensajeIsBusy = false;
        public static CuadreLoginView CuadreV1 = null;
        public static CuadreView CuadreV2 = null;

    }
}
