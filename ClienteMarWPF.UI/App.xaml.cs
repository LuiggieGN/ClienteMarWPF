using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Services;
using ClienteMarWPF.Domain.Services.AccountService;
using ClienteMarWPF.Domain.Services.AuthenticationService;
using ClienteMarWPF.DataAccess.Services;

using ClienteMarWPF.UI.State.Accounts;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;

using ClienteMarWPF.UI.Modules.Home;
using ClienteMarWPF.UI.Modules.Login;
using ClienteMarWPF.UI.Modules.Modulo;
using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Factories;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


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



            ///@@ Registrando Servicio de Factoria de ViewModel y de los ( ViewModels de los modulos)

            services.AddSingleton<IViewModelFactory, ViewModelFactory>();
            services.AddSingleton<HomeViewModel>(
                services => new HomeViewModel()
            );

            services.AddSingleton<ModuloViewModel>(
                services => new ModuloViewModel()
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


            services.AddSingleton<Renavigator<HomeViewModel>>();
            services.AddSingleton<CreateViewModel<LoginViewModel>>(services =>
            {
                return () => new LoginViewModel(
                   services.GetRequiredService<IAuthenticator>(),
                   services.GetRequiredService<Renavigator<HomeViewModel>>()
                );
            });

            services.AddSingleton<INavigator, Navigator>();
            services.AddSingleton<IAuthenticator, Authenticator>();
            services.AddSingleton<IAccountStore, AccountStore>();

            services.AddScoped<MainWindowViewModel>();
            services.AddScoped<MainWindow>(s => new MainWindow(s.GetRequiredService<MainWindowViewModel>()));

            return services.BuildServiceProvider();
        }



    }// fin de clase App
}// fin de namespace ClienteMarWPF.UI



//services.AddSingleton<IServiceBase<CuentaUsuario, int>, AccountDataService>(); // IAccountService