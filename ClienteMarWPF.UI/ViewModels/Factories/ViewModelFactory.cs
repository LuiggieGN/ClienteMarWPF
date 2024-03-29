﻿
using System;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.UI.Modules.CincoMinutos;
using ClienteMarWPF.UI.Modules.Configuracion;
using ClienteMarWPF.UI.Modules.Home;
using ClienteMarWPF.UI.Modules.Login;
using ClienteMarWPF.UI.Modules.Mensajeria; 
using ClienteMarWPF.UI.Modules.PagoServicios;
using ClienteMarWPF.UI.Modules.Recargas;
using ClienteMarWPF.UI.Modules.Reporte;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Inicio;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento;
using ClienteMarWPF.UI.Modules.JuegaMas;

using ClienteMarWPF.UI.State.Navigators;


using ClienteMarWPF.UI.ViewModels.Base;

namespace ClienteMarWPF.UI.ViewModels.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {

        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<ReporteViewModel> _createReporteViewModel;
        private readonly CreateViewModel<SorteosViewModel> _createSorteosViewModel;
        private readonly CreateViewModel<CincoMinutosViewModel> _createCincoMinutosViewModel;
        private readonly CreateViewModel<RecargasViewModel> _createRecargasViewModel;
        private readonly CreateViewModel<MensajeriaViewModel> _createMensajeriaViewModel; 
        private readonly CreateViewModel<PagoServiciosViewModel> _createPagoServiciosViewModel; 
        private readonly CreateViewModel<InicioViewModel> _createInicioControlEfectivoViewModel;       
        private readonly CreateViewModel<MovimientoViewModel> _createMovimientoControlEfectivoViewModel;
        private readonly CreateViewModel<JuegaMasViewModel> _createJuegaMasViewModel;


        public ViewModelFactory(
           CreateViewModel<HomeViewModel> createHomeViewModel,
           CreateViewModel<LoginViewModel> createLoginViewModel,
           CreateViewModel<ReporteViewModel> createReporteViewModel,
           CreateViewModel<SorteosViewModel> createSorteosViewModel,
           CreateViewModel<CincoMinutosViewModel> createCincoMinutosViewModel,
           CreateViewModel<RecargasViewModel> createRecargasViewModel,
           CreateViewModel<MensajeriaViewModel> createMensajeriaViewModel,
           CreateViewModel<PagoServiciosViewModel> createPagoServiciosViewModel, 
           CreateViewModel<InicioViewModel> createInicioControlEfectivoViewModel,
           CreateViewModel<MovimientoViewModel> createMovimientoControlEfectivoViewModel,
           CreateViewModel<JuegaMasViewModel> createJuegaMasViewModel

        )
        {
            _createHomeViewModel = createHomeViewModel;
            _createLoginViewModel = createLoginViewModel;
            _createReporteViewModel = createReporteViewModel;
            _createSorteosViewModel = createSorteosViewModel;
            _createCincoMinutosViewModel = createCincoMinutosViewModel;
            _createRecargasViewModel = createRecargasViewModel;
            _createMensajeriaViewModel = createMensajeriaViewModel; 
            _createPagoServiciosViewModel = createPagoServiciosViewModel; 
            _createInicioControlEfectivoViewModel = createInicioControlEfectivoViewModel;
            _createMovimientoControlEfectivoViewModel = createMovimientoControlEfectivoViewModel;
            _createJuegaMasViewModel = createJuegaMasViewModel;

        }

        /// <summary>
        ///    Factoria de viewmodels
        /// </summary>
        public BaseViewModel CreateViewModel(Modulos view)
        {
            switch (view)
            {
                case Modulos.Home:
                    return _createHomeViewModel();
                case Modulos.Login:
                    return _createLoginViewModel(); 
                case Modulos.Reporte:
                    return _createReporteViewModel();                
                case Modulos.Sorteos:
                    return _createSorteosViewModel();
                case Modulos.JuegaMas:
                    return _createJuegaMasViewModel();
                case Modulos.CincoMinutos:
                    return _createCincoMinutosViewModel();                
                case Modulos.Recargas:
                    return _createRecargasViewModel();               
                case Modulos.Mensajeria:
                    return _createMensajeriaViewModel();
                case Modulos.PagoServicios:
                    return _createPagoServiciosViewModel();                 
                case Modulos.InicioControlEfectivo:
                    return _createInicioControlEfectivoViewModel();
                case Modulos.RegistrosDeMovimiento:
                    return _createMovimientoControlEfectivoViewModel();
                default:
                    throw new ArgumentException("El ViewType no tiene ViewModel.", "viewType");
            }
        }



    }
}
