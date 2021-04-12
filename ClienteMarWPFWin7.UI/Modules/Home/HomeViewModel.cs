

using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Services.CajaService;
using ClienteMarWPFWin7.Domain.Services.ReportesService;

using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.DashboardCard;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Home;
using ClienteMarWPFWin7.UI.ViewModels;

using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System;
using System.Text;
using ClienteMarWPFWin7.Domain.Helpers;


namespace ClienteMarWPFWin7.UI.Modules.Home
{
    public class HomeViewModel : BaseViewModel
    {
        #region Fields
        bool _cargando;
        bool _isFocus;
        DateTime _fechaAConsultar;
        private readonly ToastViewModel _toast;
        #endregion

        #region Properties
        public ToastViewModel Toast => _toast;
        public static BackgroundWorker Worker = new BackgroundWorker();

        public IAuthenticator ServicioAutenticacion { get; }
        public IDashboardCard ServicioDashboard { get; }
        public IBancaService ServicioBanca { get; }
        public ICajaService ServicioCaja { get; }

        public string Card_Ventas_Loterias { get => ServicioDashboard?.Card_Ventas_Loterias ?? "*"; }
        public string Card_Ventas_Productos { get => ServicioDashboard?.Card_Ventas_Productos ?? "*"; }
        public string Card_Comisiones { get => ServicioDashboard?.Card_Comisiones ?? "*"; }
        public string Card_Anulaciones { get => ServicioDashboard?.Card_Anulaciones ?? "*"; }
        public string Card_Pagos { get => ServicioDashboard?.Card_Pagos ?? "*"; }
        public string Card_Descuentos_Productos { get => ServicioDashboard?.Card_Descuentos_Productos ?? "*"; }
        public string Card_Balances { get => ServicioDashboard?.Card_Balances ?? "*"; }
        public string UltimaFechaDeActualizacion { get => ServicioDashboard?.UltimaFechaDeActualizacionStr ?? ""; }
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

                ServicioDashboard?.SetFechaAConsultar(_fechaAConsultar);

                NotifyPropertyChanged(nameof(FechaAConsultar));
            }
        }
        #endregion

        #region ICommands
        public ICommand CargarBalancesCommand { get; }
        #endregion

        public HomeViewModel(IAuthenticator servicioAutenticacion,
                             IDashboardCard servicioDashboard,
                             IBancaService servicoBanca,
                             ICajaService servicioCaja)
        {

            _toast = new ToastViewModel();

            ServicioAutenticacion = servicioAutenticacion;
            ServicioDashboard = servicioDashboard;
            ServicioBanca = servicoBanca;
            ServicioCaja = servicioCaja;

            FechaAConsultar = servicioDashboard.FechaAConsultar;

            Cargando = No;

            CargarBalancesCommand = new CargarBalancesCommand(this);

            if (servicioDashboard.IsLoadingForFirstTime)
            {
                servicioDashboard.IsLoadingForFirstTime = No;

                CargarBalancesCommand.Execute(null);
            }
        }







        public void UpdateCardsValue(string card_ventas_loterias,
                                     string card_ventas_productos,
                                     string card_comisiones,
                                     string card_anulaciones,
                                     string card_pagos,
                                     string card_descuentos_productos,
                                     string card_balances,
                                     DateTime? ultimaActualizacion)
        {

            ServicioDashboard.SetCardsValue(card_ventas_loterias,
                                            card_ventas_productos,
                                            card_comisiones,
                                            card_anulaciones,
                                            card_pagos,
                                            card_descuentos_productos,
                                            card_balances,
                                            ultimaActualizacion);

            NotifyPropertyChanged(nameof(Card_Ventas_Loterias),
                                  nameof(Card_Ventas_Productos),
                                  nameof(Card_Comisiones),
                                  nameof(Card_Anulaciones),
                                  nameof(Card_Pagos),
                                  nameof(Card_Descuentos_Productos),
                                  nameof(Card_Balances),
                                  nameof(UltimaFechaDeActualizacion));
        }//fin de metodo UpdateCardsValue







    }//fin de clase

}//fin de namespace
