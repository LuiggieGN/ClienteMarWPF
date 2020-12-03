
using System;

using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Services.CuadreService;

using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Factories;

using ClienteMarWPF.UI.Modules.FlujoEfectivo.Inicio;
using ClienteMarWPF.UI.Modules.FlujoEfectivo.Inicio.Modal;
 

namespace ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Inicio
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
