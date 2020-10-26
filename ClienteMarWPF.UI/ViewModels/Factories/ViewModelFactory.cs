
using System;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.UI.Modules.CincoMinutos;
using ClienteMarWPF.UI.Modules.Home;
using ClienteMarWPF.UI.Modules.Login;
using ClienteMarWPF.UI.Modules.Mensajeria;
using ClienteMarWPF.UI.Modules.Modulo;
using ClienteMarWPF.UI.Modules.PagoServicios;
using ClienteMarWPF.UI.Modules.Recargas;
using ClienteMarWPF.UI.Modules.Reporte;
using ClienteMarWPF.UI.Modules.Sorteos;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.InicioControlEfectivo;
using ClienteMarWPF.UI.State.Navigators;


using ClienteMarWPF.UI.ViewModels.Base;

namespace ClienteMarWPF.UI.ViewModels.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {

        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;
        private readonly CreateViewModel<LoginViewModel> _createLoginViewModel;
        private readonly CreateViewModel<ModuloViewModel> _createModuloViewModel;
        private readonly CreateViewModel<ReporteViewModel> _createReporteViewModel;
        private readonly CreateViewModel<SorteosViewModel> _createSorteosViewModel;
        private readonly CreateViewModel<CincoMinutosViewModel> _createCincoMinutosViewModel;
        private readonly CreateViewModel<RecargasViewModel> _createRecargasViewModel;
        private readonly CreateViewModel<MensajeriaViewModel> _createMensajeriaViewModel; 
        private readonly CreateViewModel<PagoServiciosViewModel> _createPagoServiciosViewModel;
        private readonly CreateViewModel<InicioControlEfectivoViewModel> _createInicioControlEfectivoViewModel;       
 

        public ViewModelFactory(
           CreateViewModel<HomeViewModel> createHomeViewModel,
           CreateViewModel<LoginViewModel> createLoginViewModel,
           CreateViewModel<ModuloViewModel> createModuloViewModel,
           CreateViewModel<ReporteViewModel> createReporteViewModel,
           CreateViewModel<SorteosViewModel> createSorteosViewModel,
           CreateViewModel<CincoMinutosViewModel> createCincoMinutosViewModel,
           CreateViewModel<RecargasViewModel> createRecargasViewModel,
           CreateViewModel<MensajeriaViewModel> createMensajeriaViewModel, 
           CreateViewModel<PagoServiciosViewModel> createPagoServiciosViewModel,
           CreateViewModel<InicioControlEfectivoViewModel> createInicioControlEfectivoViewModel
 
        )
        {
            _createHomeViewModel = createHomeViewModel;
            _createLoginViewModel = createLoginViewModel;
            _createModuloViewModel = createModuloViewModel;
            _createReporteViewModel = createReporteViewModel;
            _createSorteosViewModel = createSorteosViewModel;
            _createCincoMinutosViewModel = createCincoMinutosViewModel;
            _createRecargasViewModel = createRecargasViewModel;
            _createMensajeriaViewModel = createMensajeriaViewModel; 
            _createPagoServiciosViewModel = createPagoServiciosViewModel;
            _createInicioControlEfectivoViewModel = createInicioControlEfectivoViewModel;
 
        }

        /// <summary>
        ///    Factoria de viewmodels
        /// </summary>
        public BaseViewModel CreateViewModel(ViewTypeEnum view)
        {
            switch (view)
            {
                case ViewTypeEnum.Home:
                    return _createHomeViewModel();
                case ViewTypeEnum.Login:
                    return _createLoginViewModel();
                case ViewTypeEnum.Modulo:
                    return _createModuloViewModel();
                case ViewTypeEnum.Reporte:
                    return _createReporteViewModel();                
                case ViewTypeEnum.Sorteos:
                    return _createSorteosViewModel();               
                case ViewTypeEnum.CincoMinutos:
                    return _createCincoMinutosViewModel();                
                case ViewTypeEnum.Recargas:
                    return _createRecargasViewModel();               
                case ViewTypeEnum.Mensajeria:
                    return _createMensajeriaViewModel();
                case ViewTypeEnum.PagoServicios:
                    return _createPagoServiciosViewModel();
                case ViewTypeEnum.InicioControlEfectivo:
                    return _createInicioControlEfectivoViewModel(); 
                default:
                    throw new ArgumentException("El ViewType no tiene ViewModel.", "viewType");
            }
        }



    }
}
