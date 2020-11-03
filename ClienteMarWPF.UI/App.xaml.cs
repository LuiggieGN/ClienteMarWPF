using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
 
using ClienteMarWPF.Domain.Services.AccountService;
using ClienteMarWPF.Domain.Services.AuthenticationService;
using ClienteMarWPF.DataAccess.Services;

using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.Configurators;
using ClienteMarWPF.UI.State.LocalClientSetting;

using ClienteMarWPF.UI.Modules.Home;
using ClienteMarWPF.UI.Modules.Login;
using ClienteMarWPF.UI.Modules.Modulo;
using ClienteMarWPF.UI.Modules.Reporte;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Factories;

using System;
using System.Windows;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.UI.Modules.CincoMinutos;
using ClienteMarWPF.UI.Modules.Recargas;
using ClienteMarWPF.UI.Modules.Mensajeria; 
using ClienteMarWPF.UI.Modules.PagoServicios;
using ClienteMarWPF.UI.Modules.Configuracion;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.InicioControlEfectivo;
using ClienteMarWPF.Domain.Services.MensajesService;

namespace ClienteMarWPF.UI
{

    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            IServiceProvider serviceProvider = CreateServiceProvider();

            Window window = serviceProvider.GetRequiredService<MainWindow>();
            window.Show();

            base.OnStartup(e);
        }


        private IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();


            //@@ Registrando Servicios del Dominio (ClienteMarWPF.Domain)            
            services.AddSingleton<IAccountService, AccountDataService>();
            services.AddSingleton<IPasswordHasher<Usuario>, PersonalizedPasswordHasher>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IBancaService, BancaDataService>();
            services.AddSingleton<IMensajesService, MensajesDataService>();
 


            ///@@ Registrando Servicio de Factoria de ViewModel y de los ( ViewModels de los modulos)
            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<HomeViewModel>(
                services => new HomeViewModel()
            );

            services.AddSingleton<ModuloViewModel>(
                services => new ModuloViewModel( services.GetRequiredService<IBancaService>())
            );

            services.AddSingleton<ReporteViewModel>(
                services => new ReporteViewModel()
            );

            services.AddSingleton<SorteosViewModel>(
                services => new SorteosViewModel()
            );

            services.AddSingleton<CincoMinutosViewModel>(
                services => new CincoMinutosViewModel()
            );

            services.AddSingleton<RecargasViewModel>(
                services => new RecargasViewModel()
            );

            services.AddSingleton<MensajeriaViewModel>(
                services => new MensajeriaViewModel(
                    services.GetRequiredService<IAuthenticator>(), 
                    services.GetRequiredService<IMensajesService>())
            );


            services.AddSingleton<PagoServiciosViewModel>(
                services => new PagoServiciosViewModel()
            );

            services.AddSingleton<ConfiguracionViewModel>(
                services => new ConfiguracionViewModel()
            );


            services.AddSingleton<InicioControlEfectivoViewModel>(
                services => new InicioControlEfectivoViewModel(
                )
            );

 

            ///@@ Habilta Navegacion entre modulos de la aplicacion



            services.AddSingleton<CreateViewModel<HomeViewModel>>(services =>
            {
                return () => services.GetRequiredService<HomeViewModel>(); 
            });

            services.AddSingleton<CreateViewModel<ModuloViewModel>>(services =>
            {
                return () => services.GetRequiredService<ModuloViewModel>();
            });

            services.AddSingleton<CreateViewModel<ReporteViewModel>>(services =>
            {
                return () => services.GetRequiredService<ReporteViewModel>();
            });

            services.AddSingleton<CreateViewModel<SorteosViewModel>>(services =>
            {
                return () => services.GetRequiredService<SorteosViewModel>();
            });

            services.AddSingleton<CreateViewModel<CincoMinutosViewModel>>(services =>
            {
                return () => services.GetRequiredService<CincoMinutosViewModel>();
            });

            services.AddSingleton<CreateViewModel<RecargasViewModel>>(services =>
            {
                return () => services.GetRequiredService<RecargasViewModel>();
            });

            services.AddSingleton<CreateViewModel<MensajeriaViewModel>>(services =>
            {
                return () => services.GetRequiredService<MensajeriaViewModel>();
            });
 
            services.AddSingleton<CreateViewModel<PagoServiciosViewModel>>(services =>
            {
                return () => services.GetRequiredService<PagoServiciosViewModel>();
            });

            services.AddSingleton<CreateViewModel<ConfiguracionViewModel>>(services =>
            {
                return () => services.GetRequiredService<ConfiguracionViewModel>();
            });

            
            services.AddSingleton<CreateViewModel<InicioControlEfectivoViewModel>>(services =>
            {
                return () => services.GetRequiredService<InicioControlEfectivoViewModel>();
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

            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();
            services.AddSingleton<IConfiguratorStore, ConfiguratorStore>();
            services.AddSingleton<ILocalClientSettingStore, LocalClientSettingStore>();

            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));

            return services.BuildServiceProvider();
        }



    }// fin de Clase App

}// fin de namespace ClienteMarWPF.UI



 