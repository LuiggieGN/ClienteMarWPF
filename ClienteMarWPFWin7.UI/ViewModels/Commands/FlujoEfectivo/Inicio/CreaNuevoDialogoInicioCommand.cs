
using System;

using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Services.CuadreService;

using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Factories;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio;
using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio.Modal;
 

namespace ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Inicio
{    
     
    public class CreaNuevoDialogoInicioCommand : ActionCommand
    {
        private readonly InicioViewModel _viewmodel;
        private readonly INavigator _nav;
        private readonly IAuthenticator _aut;
        private readonly IViewModelFactory _vistas;
        private readonly IBancaService _banService;
        private readonly ICuadreService _cuadreService;

        public CreaNuevoDialogoInicioCommand(InicioViewModel viewmodel, INavigator nav, IAuthenticator aut, IViewModelFactory vistas, IBancaService banService, ICuadreService cuadreService) :base()
        {
            _viewmodel = viewmodel;
            _nav = nav;
            _aut = aut;
            _vistas = vistas;
            _banService = banService;
            _cuadreService = cuadreService;

            SetAction(new Action<object>(CrearNuevo)); 
        }



        public void CrearNuevo( object parametro ) 
        {
            _viewmodel.Dialog = new DialogInicioViewModel(_nav,_aut,_vistas,_banService,_cuadreService);
            _viewmodel.Dialog.Mostrar();
        }




    }
}
