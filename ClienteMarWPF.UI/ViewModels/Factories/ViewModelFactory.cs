
using System;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.UI.Modules.Home;
using ClienteMarWPF.UI.Modules.Login;
using ClienteMarWPF.UI.Modules.Modulo;
using ClienteMarWPF.UI.Modules.Reporte;

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

        public ViewModelFactory(
           CreateViewModel<HomeViewModel> createHomeViewModel,
           CreateViewModel<LoginViewModel> createLoginViewModel,
           CreateViewModel<ModuloViewModel> createModuloViewModel,
           CreateViewModel<ReporteViewModel> createReporteViewModel
        )
        {
            _createHomeViewModel = createHomeViewModel;
            _createLoginViewModel = createLoginViewModel;
            _createModuloViewModel = createModuloViewModel;
            _createReporteViewModel = createReporteViewModel;
        }

        /// <summary>
        ///    Factoria de viewmodels
        /// </summary>
        public BaseViewModel CreateViewModel(ViewTypeEnum viewType)
        {
            switch (viewType)
            {
                case ViewTypeEnum.Home:
                    return _createHomeViewModel();
                case ViewTypeEnum.Login:
                    return _createLoginViewModel();
                case ViewTypeEnum.Modulo:
                    return _createModuloViewModel();
                case ViewTypeEnum.Reporte:
                    return _createReporteViewModel();
                default:
                    throw new ArgumentException("El ViewType no tiene ViewModel.", "viewType");
            }
        }



    }
}
