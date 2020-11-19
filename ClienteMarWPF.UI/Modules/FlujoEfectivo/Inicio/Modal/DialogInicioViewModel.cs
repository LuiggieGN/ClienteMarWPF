
using System;
using System.Windows.Input;

using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Factories;
using ClienteMarWPF.Domain.Services.BancaService;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Inicio;

namespace ClienteMarWPF.UI.Modules.FlujoEfectivo.Inicio.Modal
{
    public class DialogInicioViewModel : BaseViewModel
    {
        private bool _muestroDialogo;
        private bool _muestroBotonAceptar;
        private readonly INavigator _nav;
        private readonly IAuthenticator _aut;
        private readonly IViewModelFactory _vistas;
        private readonly IBancaService _banService;
        private decimal _bancaBalance;


        public MessageViewModel MensajeBalanceViewModel { get; }
        public MessageViewModel MensajeErrorViewModel { get; }

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
        public string MensajeBalance
        {
            set => MensajeBalanceViewModel.Message = value;
        }
        public string MensajeError
        {
            set => MensajeErrorViewModel.Message = value;
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

        public DialogInicioViewModel(INavigator nav, IAuthenticator aut, IViewModelFactory vistas, IBancaService banService)
        {
            _nav = nav;
            _aut = aut;
            _vistas = vistas;
            _banService = banService;

            MensajeBalanceViewModel = new MessageViewModel();
            MensajeErrorViewModel = new MessageViewModel();
            CerrarDialogoInicioCommand = new CerrarDialogoInicioCommand(this);
            CalcularBancaBalanceCommand = new CalcularBancaBalanceCommand(this, _aut, _banService);
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
