
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View1;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento.Views.View2;

using ClienteMarWPF.Domain.Services.TieService;
using ClienteMarWPF.Domain.Services.CajaService;





namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Movimiento
{
    public class MovimientoViewModel : BaseViewModel
    {
        private readonly EnCajaViewModel _view1;
        private readonly DesdeHastaViewModel _view2;

        private readonly IAuthenticator _aut;
        private readonly ITieService _tieService;
        private readonly ICajaService _cajaService;
                     

        public EnCajaViewModel View1
        {
            get => _view1; 
        }

        public DesdeHastaViewModel View2
        {
            get => _view2; 
        }

        public MovimientoViewModel(IAuthenticator aut, ITieService tieService, ICajaService cajaService)
        {
            _aut = aut;
            _tieService = tieService;
            _cajaService = cajaService;

            _view1 = new EnCajaViewModel(_aut, _tieService, _cajaService);
            _view2 = new DesdeHastaViewModel(_aut, _cajaService); 
        }


 




    }
}
