
using System;
using System.Collections.Generic;

using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.UI.Modules.CincoMinutos;
using ClienteMarWPFWin7.UI.Modules.Configuracion;
using ClienteMarWPFWin7.UI.Modules.Home;
using ClienteMarWPFWin7.UI.Modules.Login;
using ClienteMarWPFWin7.UI.Modules.Mensajeria; 
using ClienteMarWPFWin7.UI.Modules.PagoServicios;
using ClienteMarWPFWin7.UI.Modules.Recargas;
using ClienteMarWPFWin7.UI.Modules.Reporte;
using ClienteMarWPFWin7.UI.Modules.Sorteos;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Movimiento;
using ClienteMarWPFWin7.UI.Modules.PegaMas;

using ClienteMarWPFWin7.UI.State.Navigators;


using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.Modules.EnLinea;

namespace ClienteMarWPFWin7.UI.ViewModels.Factories
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
        private readonly CreateViewModel<PegaMasViewModel> _createPegaMasViewModel;
        private readonly CreateViewModel<EnLineaViewModel> _createEnLineaViewModel;


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
           CreateViewModel<PegaMasViewModel> createPegaMasViewModel,
           CreateViewModel<EnLineaViewModel> createEnLineaViewModel


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
            _createPegaMasViewModel = createPegaMasViewModel;
            _createEnLineaViewModel = createEnLineaViewModel;

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
                case Modulos.PegaMas:
                    return _createPegaMasViewModel();
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
                case Modulos.EnLinea:
                    return _createEnLineaViewModel();
                default:
                    throw new ArgumentException("El ViewType no tiene ViewModel.", "viewType");
            }
        }



    }
}
