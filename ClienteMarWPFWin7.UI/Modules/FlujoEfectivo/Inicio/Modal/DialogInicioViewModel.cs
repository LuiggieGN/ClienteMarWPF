
using System;
using System.Windows.Input;

using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Factories;

using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Services.CuadreService;

using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.FlujoEfectivo.Inicio;

namespace ClienteMarWPFWin7.UI.Modules.FlujoEfectivo.Inicio.Modal
{
    public class DialogInicioViewModel : BaseViewModel
    {
        private bool _muestroDialogo;
        private bool _muestroBotonAceptar;
        private decimal _bancaBalance;
        private readonly INavigator _nav;
        private readonly IAuthenticator _aut;
        private readonly IViewModelFactory _vistas;
        private readonly IBancaService _banService;
        private readonly ICuadreService _cuadreService;


        public MessageViewModel MensajeBalanceViewModel { get; }
        public string MensajeBalance
        {
            set => MensajeBalanceViewModel.Message = value;
        }
        public MessageViewModel MensajeErrorViewModel { get; }
        public string MensajeError
        {
            set => MensajeErrorViewModel.Message = value;
        }
        public bool MuestroDialogo
        {
            get
            {
                return _muestroDialogo;
            }
            private set
            {
                _muestroDialogo = value; NotifyPropertyChanged(nameof(MuestroDialogo));
            }
        }
        public bool MuestroBotonAceptar
        {
            get
            {
                return _muestroBotonAceptar;
            }
            set
            {
                _muestroBotonAceptar = value; NotifyPropertyChanged(nameof(MuestroBotonAceptar));
            }
        }
        public decimal BancaBalance
        {
            get
            {
                return _bancaBalance;
            }
            set
            {
                _bancaBalance = value;  
            }
        }


        public ICommand CerrarDialogoInicioCommand { get; }
        public ICommand CalcularBancaBalanceCommand { get; }
        public ICommand IniciarControlEfectivoCommand { get; }


        public DialogInicioViewModel(INavigator nav, IAuthenticator aut, IViewModelFactory vistas, IBancaService banService, ICuadreService cuadreService)
        {
            _nav = nav;
            _aut = aut;
            _vistas = vistas;
            _banService = banService;
            _cuadreService = cuadreService;

            MensajeBalanceViewModel = new MessageViewModel();
            MensajeErrorViewModel = new MessageViewModel();
            CerrarDialogoInicioCommand = new CerrarDialogoInicioCommand(this);
            CalcularBancaBalanceCommand = new CalcularBancaBalanceCommand(this, _aut, _banService);
            IniciarControlEfectivoCommand = new IniciarControlEfectivoCommand(this, _nav, _aut, _vistas, _cuadreService);
        }



        public void Mostrar()
        {
            MuestroDialogo = true;
            if (CalcularBancaBalanceCommand != null)
            {
                CalcularBancaBalanceCommand.Execute(null);
            }
        }

        public void Ocultar()
        {
            MuestroDialogo = false;
            if (MensajeBalanceViewModel != null) MensajeBalanceViewModel.Message = null;
            if (MensajeErrorViewModel != null) MensajeErrorViewModel.Message = null;
        }





    }//fin de clase
}
