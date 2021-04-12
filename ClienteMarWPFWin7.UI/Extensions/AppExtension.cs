#region Namespaces
using Microsoft.Extensions.DependencyInjection;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.Enums;

using ClienteMarWPFWin7.Data.Services;

using ClienteMarWPFWin7.UI.State.Accounts;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.Configurators;
using ClienteMarWPFWin7.UI.State.LocalClientSetting;
using ClienteMarWPFWin7.UI.State.CuadreBuilders;
using ClienteMarWPFWin7.UI.State.BancaBalanceStore;
using ClienteMarWPFWin7.UI.State.DashboardCard;

using ClienteMarWPFWin7.UI.Modules.Home;
using ClienteMarWPFWin7.UI.Modules.Login;
using ClienteMarWPFWin7.UI.Modules.Reporte;

using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Factories;

using ClienteMarWPFWin7.UI.Modules.Sorteos;

using ClienteMarWPFWin7.UI.Modules.CincoMinutos;
using ClienteMarWPFWin7.UI.Modules.Recargas;
using ClienteMarWPFWin7.UI.Modules.Mensajeria;
using ClienteMarWPFWin7.UI.Modules.PagoServicios;
using ClienteMarWPFWin7.UI.Modules.Configuracion;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento;
using ClienteMarWPFWin7.UI.Modules.RegistrarPC;
using ClienteMarWPFWin7.UI.Modules.JuegaMas;

using ClienteMarWPFWin7.Domain.Services.AccountService;
using ClienteMarWPFWin7.Domain.Services.AuthenticationService;
using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Services.MensajesService;
using ClienteMarWPFWin7.Domain.Services.ReportesService;
using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.Domain.Services.CuadreService;
using ClienteMarWPFWin7.Domain.Services.TieService;
using ClienteMarWPFWin7.Domain.Services.CajaService;
using ClienteMarWPFWin7.Domain.Services.MultipleService;
using ClienteMarWPFWin7.Domain.Services.RutaService;
using ClienteMarWPFWin7.Domain.Services.PuntoVentaService;
using ClienteMarWPFWin7.Domain.Services.JuegaMasService;

using System;
using System.Windows;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.UI.Views.Controls;

#endregion


namespace ClienteMarWPFWin7.UI.Extensions
{
    public static class AppExtension
    {
        private static bool PideConfiguracion(ILocalClientSettingStore localClientSettingStoreService)
        {
            try
            {
                var contexto = new ConfiguracionViewModel(localClientSettingStoreService);
                contexto.CargandoDesdelogin = false;

                var ventana = new ConfiguracionView(null, contexto);
                ventana.ShowDialog();

                return contexto.ConfiguracionFueCambiada;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        private static bool PideDialogoRegistrar(ILocalClientSettingStore localClientSettingStoreService, IPtoVaService ptova, LocalClientSettingDTO settings) 
        {
            var controlContexto = new RegistrarPCControlViewModel(localClientSettingStoreService, ptova, settings);

            var windowContexto = new RegistrarPCWindowViewModel(controlContexto);

            var ventana = new RegistrarPCWindow(windowContexto);
            ventana.ShowDialog();

            return controlContexto.RegistroDePCFueExitoso;

        }

        public static IServiceProvider CreateAppServicesProvider(this IServiceCollection services, App aplicativo, WindowAppLoding ventanaCargando)
        {
            InicioPCResultDTO inicio;

            try
            {
                var sc = new ServiceCollection();
                sc.AddSingleton<ILocalClientSettingStore, LocalClientSettingStore>();
                sc.AddSingleton<IPtoVaService, PtoVaService>();
                var sp = sc.BuildServiceProvider();


                var ReaderAndWriterIniFile = sp.GetService<ILocalClientSettingStore>();
                var Ptova = sp.GetService<IPtoVaService>();


                ReaderAndWriterIniFile.ReadDektopLocalSetting();


                var Sys = ReaderAndWriterIniFile.LocalClientSettings;


                if (Sys.BancaId != 0)
                {

                    inicio = Ptova.IniciarPC(bancaid: Sys.BancaId, bancaip_And_Hwkey: Sys.Direccion + ";" + Sys.GetHwKey()); // ONLINE                   

                    ventanaCargando.Close();


                    if (inicio.InicioPCResponse.Err != string.Empty)
                    {
                        //Hubo un |Error| en el Inicio

                        MessageBox.Show(inicio.InicioPCResponse.Err, "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        if (inicio.InicioPCResponse.Err.ToUpper().Contains("TERMINAL MAL INSTALADA") & PideConfiguracion(ReaderAndWriterIniFile))
                        {
                            return null; // @@ Re-Start es requerido para aplicar nueva configuracion -- 
                        }
                    }
                    else
                    {
                        //Inicio |Exitoso|

                        if (Sys.Identidad == inicio.InicioPCResponse.Llave[0] && inicio.InicioPCResponse.Llave[0] != string.Empty)
                        {
                            ReaderAndWriterIniFile.WriteDesktopLocalSetting(Sys);
                            inicio.EstaPCTienePermisoDeConexionAServicioDeMAR = true;
                        }
                        else
                        {
                            Sys.Identidad = Sys.GetHwKey();

                            RegistroPCResultDTO registroPC = Ptova.RegistraCambioPC(bancaid: Sys.BancaId, hwkey: Sys.Identidad);


                            if (registroPC.FueExitoso && Convert.ToDecimal(registroPC.CertificadoNumero) > 0)
                            {
                                Sys.Identidad = registroPC.CertificadoNumero;


                                ReaderAndWriterIniFile.WriteDesktopLocalSetting(Sys);
                                inicio.EstaPCTienePermisoDeConexionAServicioDeMAR = true;
                            }
                            else
                            {
                                
                                MessageBox.Show("Su computador ha sido cambiado o no esta registrado para esta banca en la central. Solicite autorizacion para registrarla.", "Su PC no esta registrada!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);


                                inicio.EstaPCTienePermisoDeConexionAServicioDeMAR = PideDialogoRegistrar(ReaderAndWriterIniFile, Ptova, Sys);


                                if (!inicio.EstaPCTienePermisoDeConexionAServicioDeMAR)
                                {
                                    bool TERMINAL_CONFIGURACION_LOCAL_FUE_CAMBIADA = PideConfiguracion(ReaderAndWriterIniFile);

                                    if (!TERMINAL_CONFIGURACION_LOCAL_FUE_CAMBIADA) 
                                    {
                                        MessageBox.Show("Su computador NO puede ser utilizado hasta tanto sea registrado exitosamente en el sistema", "Su PC no esta registrada!!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                                    }

                                    return null;
                                }






                            }

                        }//fin else |Registrar PC|


                    }//fin else |Cuando Inicio no da error| 






                }
                else
                {
                    ventanaCargando.Close();

                    // @@ Pendiente pedirle a Jaasiel esta logica ( Cuando el BancaID = 0  )
                    inicio = new InicioPCResultDTO();
                    inicio.InicioPCResponse = new MAR_Array();
                    inicio.InicioPCResponse.Err = "El Banca Id no puede ser 0";
                    inicio.SevidorConexion = ServicioMarConexion.NoConectado;
                }




            }
            catch
            {
                ventanaCargando.Close();

                inicio = new InicioPCResultDTO();
                inicio.InicioPCResponse = new MAR_Array();
                inicio.InicioPCResponse.Err = "Ha ocurrido un error";
                inicio.SevidorConexion = ServicioMarConexion.NoConectado;
            }


            return AddServicesProvider(services, inicio, aplicativo);
        }

        private static IServiceProvider AddServicesProvider(IServiceCollection coleccionDeServicios, InicioPCResultDTO inicioDto, App aplicativo)
        {
            Action reInicioApp = null;

            #region DB Servicios ... 
            coleccionDeServicios.AddSingleton<IAccountService, AccountDataService>(); 
            coleccionDeServicios.AddSingleton<IAuthenticationService, AuthenticationService>();
            coleccionDeServicios.AddSingleton<IBancaService, BancaDataService>();
            coleccionDeServicios.AddSingleton<ICuadreService, CuadreDataService>();
            coleccionDeServicios.AddSingleton<IMensajesService, MensajesDataService>();
            coleccionDeServicios.AddSingleton<IReportesServices, ReportesDataService>();
            coleccionDeServicios.AddSingleton<IRecargaService, RecargaDataService>();
            coleccionDeServicios.AddSingleton<ISorteosService, SorteosDataService>();
            coleccionDeServicios.AddSingleton<ITieService, TieDataService>();
            coleccionDeServicios.AddSingleton<ICajaService, CajaDataService>();
            coleccionDeServicios.AddSingleton<IMultipleService, MultipleDataService>();
            coleccionDeServicios.AddSingleton<IRutaService, RutaDataService>();
            coleccionDeServicios.AddSingleton<IJuegaMasService, JuegaMasDataService>();
            #endregion

            #region Crea Modulos ...
            coleccionDeServicios.AddSingleton<IViewModelFactory, ViewModelFactory>();
            #endregion

            #region Modulos Servicios ... 
            coleccionDeServicios.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
            {
                return () => new HomeViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IDashboardCard>(),
                    services.GetRequiredService<IBancaService>(),
                    services.GetRequiredService<ICajaService>() 
               );
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<ReporteViewModel>>(services =>
            {
                return () => new ReporteViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IReportesServices>(),
                    services.GetRequiredService<IJuegaMasService>()
                );
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<SorteosViewModel>>(services =>
            {
                return () => new SorteosViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ISorteosService>()
                );
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<JuegaMasViewModel>>(services =>
            {
                return () => new JuegaMasViewModel( );
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<CincoMinutosViewModel>>(services =>
            {
                return () => new CincoMinutosViewModel();
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<RecargasViewModel>>(services =>
            {
                return () => new RecargasViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IRecargaService>(),
                    services.GetRequiredService<INavigator>(),
                    services.GetRequiredService<IViewModelFactory>()
                );
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<MensajeriaViewModel>>(services =>
            {
                return () => new MensajeriaViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IMensajesService>()
                );
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<PagoServiciosViewModel>>(services =>
            {
                return () => new PagoServiciosViewModel();
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<InicioViewModel>>(services =>
            {
                return () => new InicioViewModel(
                   services.GetRequiredService<INavigator>(),
                   services.GetRequiredService<IAuthenticator>(),
                   services.GetRequiredService<IViewModelFactory>(),
                   services.GetRequiredService<IBancaService>(),
                   services.GetRequiredService<ICuadreService>()
                );
            });

            coleccionDeServicios.AddSingleton<Renavigator<HomeViewModel>>();

            coleccionDeServicios.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
            {
                var vm = new LoginViewModel(
                   services.GetRequiredService<IAuthenticator>(),
                   services.GetRequiredService<Renavigator<HomeViewModel>>(),
                   services.GetRequiredService<ILocalClientSettingStore>(),
                   services.GetRequiredService<IBancaService>(),
                   inicioDto
                );


                reInicioApp = new Action(vm.ReIniciarApp);

                return () => vm;
            });

            coleccionDeServicios.AddSingleton<CreateViewModel<MovimientoViewModel>>(services =>
            {
                return () => new MovimientoViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ITieService>(),
                    services.GetRequiredService<ICajaService>(),
                    services.GetRequiredService<IMultipleService>()
                );
            });
            #endregion

            #region State Servicios ...
            coleccionDeServicios.AddSingleton<INavigator, Navigator>();
            coleccionDeServicios.AddSingleton<IAuthenticator, Authenticator>();
            coleccionDeServicios.AddSingleton<IAccountStore, AccountStore>();
            coleccionDeServicios.AddSingleton<IPermisosStore, PermisosStore>();
            coleccionDeServicios.AddSingleton<IConfiguratorStore, ConfiguratorStore>();
            coleccionDeServicios.AddSingleton<ILocalClientSettingStore, LocalClientSettingStore>();
            coleccionDeServicios.AddSingleton<IBancaBalanceStore, BancaBalanceStore>();
            coleccionDeServicios.AddSingleton<ICuadreBuilder, CuadreBuilder>();
            coleccionDeServicios.AddSingleton<IDashboardCard, DashboardCard>();
            #endregion

            coleccionDeServicios.AddScoped<MainWindowViewModel>(
                s => new MainWindowViewModel(
                s.GetRequiredService<INavigator>(),
                s.GetRequiredService<IViewModelFactory>(),
                s.GetRequiredService<IAuthenticator>(),
                s.GetRequiredService<IMultipleService>(),
                s.GetRequiredService<IRutaService>(),
                s.GetRequiredService<ICuadreBuilder>(),
                s.GetRequiredService<ILocalClientSettingStore>(),
                inicioDto,
                reInicioApp,
                aplicativo
                )
            );
            coleccionDeServicios.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));

            var providers = coleccionDeServicios.BuildServiceProvider();

            return providers;

        }//Fin de metodo AddServicesProvider( )











    }//Fin de clase AppExtension
}//Fin de Namespace
