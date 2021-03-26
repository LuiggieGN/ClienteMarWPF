



using System;
using System.Windows.Input;
using System.Collections.Generic;

using ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio.Modal;

using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.Authenticators;

using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Factories;
using ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Inicio;

using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Services.CuadreService;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio
{
    public class InicioViewModel : BaseViewModel
    {

        private readonly INavigator _nav;
        private readonly IAuthenticator _aut;
        private readonly IViewModelFactory _vistas;
        private readonly IBancaService _banService;
        private readonly ICuadreService _cuadreService;
        private DialogInicioViewModel _dialog;
 

        public DialogInicioViewModel Dialog  
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
        public ICommand CreaNuevoDialogoInicioCommand { get; }



        public InicioViewModel(INavigator nav, IAuthenticator aut, IViewModelFactory vistas,  IBancaService banService, ICuadreService cuadreService)
        {
            _nav = nav;
            _aut = aut;
            _vistas = vistas;
            _banService = banService;
            _cuadreService = cuadreService;

            CreaNuevoDialogoInicioCommand = new CreaNuevoDialogoInicioCommand(this, _nav,_aut,_vistas, _banService,_cuadreService);
        }

 
    }//fin de clase
}



 