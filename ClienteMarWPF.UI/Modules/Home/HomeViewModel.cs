using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Services.CajaService;

using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.DashboardCard;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Home;

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System;


namespace ClienteMarWPF.UI.Modules.Home
{
    public class HomeViewModel : BaseViewModel
    {
        #region Fields
        bool _cargando;
        bool _isFocus;
        DateTime _fechaAConsultar;
        #endregion

        #region Properties
        public static BackgroundWorker Worker = new BackgroundWorker();

        public IAuthenticator AuthenticatorService { get; }
        public IDashboardCard DashboardService { get; }
        public IBancaService BancaService { get; }
        public ICajaService CajaService { get; }
        public string Card_Ventas_Loterias { get => DashboardService?.Card_Ventas_Loterias ?? "*"; }
        public string Card_Ventas_Productos { get => DashboardService?.Card_Ventas_Productos ?? "*"; }
        public string Card_Comisiones { get => DashboardService?.Card_Comisiones ?? "*"; }
        public string Card_Anulaciones { get => DashboardService?.Card_Anulaciones ?? "*"; }
        public string Card_Balances { get => DashboardService?.Card_Balances ?? "*"; } 
        public string UltimaFechaDeActualizacion { get => DashboardService?.UltimaFechaDeActualizacionStr ?? ""; }
        public bool Cargando
        {
            get
            {
                return _cargando;
            }
            set
            {
                _cargando = value;
                NotifyPropertyChanged(nameof(Cargando));
            }
        }
        public bool IsFocus
        {
            get
            {
                return _isFocus;
            }
            set
            {
                _isFocus = value;
                NotifyPropertyChanged(nameof(IsFocus));
            }
        }
        public DateTime FechaAConsultar
        {
            get
            {
                return _fechaAConsultar;
            }
            set
            {
                _fechaAConsultar = value;

                DashboardService?.SetFechaAConsultar(_fechaAConsultar);

                NotifyPropertyChanged(nameof(FechaAConsultar));
            }
        }
        #endregion

        #region ICommands
        public ICommand CargarBalancesCommand { get; }
        #endregion

        public HomeViewModel(IAuthenticator authenticatorService ,IDashboardCard dashboardService, IBancaService bancaService, ICajaService cajaService)
        {
            AuthenticatorService = authenticatorService;
            DashboardService = dashboardService;
            BancaService = bancaService;
            CajaService = cajaService;

            FechaAConsultar = dashboardService.FechaAConsultar;

            Cargando = No;
            CargarBalancesCommand = new CargarBalancesCommand(this);

            if (dashboardService.IsLoadingForFirstTime)
            {
                CargarBalancesCommand.Execute(null);
                dashboardService.IsLoadingForFirstTime = No;
            }

        }







        public void UpdateCardsValue(string card_ventas_loterias,
                                     string card_ventas_productos,
                                     string card_comisiones,
                                     string card_anulaciones,
                                     string card_balances,
                                     DateTime? ultimaActualizacion)
        {
            DashboardService.SetCardsValue(card_ventas_loterias,
                                           card_ventas_productos,
                                           card_comisiones,
                                           card_anulaciones,
                                           card_balances,
                                           ultimaActualizacion);

            NotifyPropertyChanged(nameof(Card_Ventas_Loterias),
                                  nameof(Card_Ventas_Productos),
                                  nameof(Card_Comisiones),
                                  nameof(Card_Anulaciones),
                                  nameof(Card_Balances),
                                  nameof(UltimaFechaDeActualizacion));

        }//fin de metodo UpdateCardsValue







    }//fin de Clase

}//fin de Namespace
