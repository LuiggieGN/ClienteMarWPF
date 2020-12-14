#region Namespace
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Modal;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View3;
using ClienteMarWPF.Domain.Services.TieService;
using ClienteMarWPF.Domain.Services.CajaService;
using ClienteMarWPF.Domain.Services.MultipleService;
#endregion

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento
{
    public class MovimientoViewModel : BaseViewModel
    {
        #region Fields
        private readonly IAuthenticator _aut;
        private readonly ITieService _tieService;
        private readonly ICajaService _cajaService;
        private readonly IMultipleService _multipleServcie;
        private readonly EnCajaViewModel _view1;
        private readonly DesdeHastaViewModel _view2;
        private readonly ConsultaMovimientoViewModel _view3;
        private DialogoTokenViewModel _dialog;
        #endregion

        #region Propiedades
        public EnCajaViewModel View1
        {
            get => _view1;
        }

        public DesdeHastaViewModel View2
        {
            get => _view2;
        }

        public ConsultaMovimientoViewModel View3
        {
            get => _view3;
        }

        public DialogoTokenViewModel Dialog
        {
            get
            {
                return _dialog;
            }
            set
            {
                _dialog = value; NotifyPropertyChanged(nameof(Dialog));
            }
        }
        #endregion
                          


        public MovimientoViewModel(IAuthenticator aut, 
                                   ITieService tieService,
                                   ICajaService cajaService,
                                   IMultipleService multipleServcie)
        {
            _aut = aut;
            _tieService = tieService;
            _cajaService = cajaService;
            _multipleServcie = multipleServcie;

            _view1 = new EnCajaViewModel(_aut, _tieService, _cajaService);
            _view2 = new DesdeHastaViewModel(this,_aut, _cajaService,_multipleServcie); 
            _view3 = new ConsultaMovimientoViewModel(_aut, _cajaService);
        }


 




    }
}
