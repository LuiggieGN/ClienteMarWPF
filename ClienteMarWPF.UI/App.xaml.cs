#region Namespaces
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.DataAccess.Services;

using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.Configurators;
using ClienteMarWPF.UI.State.LocalClientSetting;
using ClienteMarWPF.UI.State.CuadreBuilders;
using ClienteMarWPF.UI.State.BancaBalanceStore;

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

using System;
using System.Windows;
using System.Threading;
using System.Windows.Markup;
using System.Globalization;
#endregion

namespace ClienteMarWPF.UI
{

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            // ** Overrinding default Application |Culture Info to spanish|
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-DO");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("es-DO");
            FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(
                        XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));

            
            // ** Stablish default Application |Services providers|
            IServiceProvider serviceProvider = CreateServiceProvider();
            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();            


            base.OnStartup(e);
        }

        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            #region Aqui Se Registran Todos Los Servicios Que Acceden a la Base de Datos los cuales son implementados por los modulos
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
            #endregion

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();    //Este Servicio Contiene la factoria de ViewModels Disponibles

            #region Habilita Navegacion Entre Modulos (Aqui estan definidos los modulos disponibles de la aplicacion y los servicios que requieren cada modulo)

            services.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
            {
                return () => new HomeViewModel();
            }); 

            services.AddSingleton<CreateViewModel<ReporteViewModel>>(services =>
            {
                return () => new ReporteViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<IReportesServices>()
                );
            });

            services.AddSingleton<CreateViewModel<SorteosViewModel>>(services =>
            {
                return () => new SorteosViewModel(
                    services.GetRequiredService<IAuthenticator>(),
                    services.GetRequiredService<ISorteosService>()
                );
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

            services.AddSingleton<CreateViewModel<ConfiguracionViewModel>>(services =>
            {
                return () => new ConfiguracionViewModel(services.GetRequiredService<ILocalClientSettingStore>());
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
                return () => new LoginViewModel(
                   services.GetRequiredService<IAuthenticator>(),
                   services.GetRequiredService<Renavigator<HomeViewModel>>(),
                   services.GetRequiredService<ILocalClientSettingStore>()
                );
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

            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<IConfiguratorStore, ConfiguratorStore>();
            services.AddSingleton<ILocalClientSettingStore, LocalClientSettingStore>();
            services.AddSingleton<IBancaBalanceStore, BancaBalanceStore>();
            services.AddSingleton<ICuadreBuilder, CuadreBuilder>();

            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));

            return services.BuildServiceProvider(); 
        }


    }// fin de Clase App

}// fin de namespace ClienteMarWPF.UI



