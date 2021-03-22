#region Namespaces
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Enums;

using ClienteMarWPF.DataAccess.Services;

using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.Configurators;
using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.State.CuadreBuilders;
using ClienteMarWPF.UI.State.BancaBalanceStore;
using ClienteMarWPF.UI.State.DashboardCard;

using ClienteMarWPF.UI.Modules.Home;
using ClienteMarWPF.UI.Modules.Login;
using ClienteMarWPF.UI.Modules.Reporte;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Factories;

using ClienteMarWPF.UI.Modules.Sorteos;

using ClienteMarWPF.UI.Modules.CincoMinutos;
using ClienteMarWPF.UI.Modules.Recargas;
using ClienteMarWPF.UI.Modules.Mensajeria;
using ClienteMarWPF.UI.Modules.PagoServicios;
using ClienteMarWPF.UI.Modules.Configuracion;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Inicio;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento;
using ClienteMarWPF.UI.Modules.RegistrarPC;
using ClienteMarWPF.UI.Modules.JuegaMas;

using ClienteMarWPF.Domain.Services.AccountService;
using ClienteMarWPF.Domain.Services.AuthenticationService;
using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Services.MensajesService;
using ClienteMarWPF.Domain.Services.ReportesService;
using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.Domain.Services.CuadreService;
using ClienteMarWPF.Domain.Services.TieService;
using ClienteMarWPF.Domain.Services.CajaService;
using ClienteMarWPF.Domain.Services.MultipleService;
using ClienteMarWPF.Domain.Services.RutaService;
using ClienteMarWPF.Domain.Services.PuntoVentaService;
using ClienteMarWPF.Domain.Services.JuegaMasService;

using System;
using System.Windows;
using MarPuntoVentaServiceReference;
using ClienteMarWPF.UI.Views.Controls;

#endregion


namespace ClienteMarWPF.UI.Extensions
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

        private static IServiceProvider AddServicesProvider(IServiceCollection services, InicioPCResultDTO inicioDto, App aplicativo)
        {
            Action reInicioApp = null;

            #region DB Servicios ... 
            services.AddSingleton<IAccountService, AccountDataService>();
            services.AddSingleton<IPasswordHasher<Usuario>, PersonalizedPasswordHasher>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IBancaService, BancaDataService>();
            services.AddSingleton<ICuadreService, CuadreDataService>();
            services.AddSingleton<IMensajesService, MensajesDataService>();
            services.AddSingleton<IReportesServices, ReportesDataService>();
            services.AddSingleton<IRecargaService, RecargaDataService>();
            services.AddSingleton<ISorteosService, SorteosDataService>();
            services.AddSingleton<ITieService, TieDataService>();
            services.AddSingleton<ICajaService, CajaDataService>();
            services.AddSingleton<IMultipleService, MultipleDataService>();
            services.AddSingleton<IRutaService, RutaDataService>();
            services.AddSingleton<IJuegaMasService, JuegaMasDataService>();
            #endregion

            #region Crea Modulos ...
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            #endregion

            #region Modulos Servicios ... 
            services.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
            {
                return () => new HomeViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IDashboardCard>(),
                    services.GetRequiredService<IBancaService>(),
                    services.GetRequiredService<ICajaService>()
               );
            });

            services.AddSingleton<CreateViewModel<ReporteViewModel>>(services =>
            {
                return () => new ReporteViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IReportesServices>(),
                    services.GetRequiredService<IJuegaMasService>()
                );
            });

            services.AddSingleton<CreateViewModel<SorteosViewModel>>(services =>
            {
                return () => new SorteosViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ISorteosService>()
                );
            });

            services.AddSingleton<CreateViewModel<JuegaMasViewModel>>(services =>
            {
                return () => new JuegaMasViewModel( );
            });

            services.AddSingleton<CreateViewModel<CincoMinutosViewModel>>(services =>
            {
                return () => new CincoMinutosViewModel();
            });

            services.AddSingleton<CreateViewModel<RecargasViewModel>>(services =>
            {
                return () => new RecargasViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IRecargaService>(),
                    services.GetRequiredService<INavigator>(),
                    services.GetRequiredService<IViewModelFactory>()
                );
            });

            services.AddSingleton<CreateViewModel<MensajeriaViewModel>>(services =>
            {
                return () => new MensajeriaViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IMensajesService>()
                );
            });

            services.AddSingleton<CreateViewModel<PagoServiciosViewModel>>(services =>
            {
                return () => new PagoServiciosViewModel();
            });

            services.AddSingleton<CreateViewModel<InicioViewModel>>(services =>
            {
                return () => new InicioViewModel(
                   services.GetRequiredService<INavigator>(),
                   services.GetRequiredService<IAuthenticator>(),
                   services.GetRequiredService<IViewModelFactory>(),
                   services.GetRequiredService<IBancaService>(),
                   services.GetRequiredService<ICuadreService>()
                );
            });

            services.AddSingleton<Renavigator<HomeViewModel>>();

            services.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
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

            services.AddSingleton<CreateViewModel<MovimientoViewModel>>(services =>
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
            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<IPermisosStore, PermisosStore>();
            services.AddSingleton<IConfiguratorStore, ConfiguratorStore>();
            services.AddSingleton<ILocalClientSettingStore, LocalClientSettingStore>();
            services.AddSingleton<IBancaBalanceStore, BancaBalanceStore>();
            services.AddSingleton<ICuadreBuilder, CuadreBuilder>();
            services.AddSingleton<IDashboardCard, DashboardCard>();
            #endregion

            services.AddScoped<MainWindowViewModel>(
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
            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));

            var providers = services.BuildServiceProvider();

            return providers;

        }//Fin de metodo AddServicesProvider( )











    }//Fin de clase AppExtension
}//Fin de Namespace
