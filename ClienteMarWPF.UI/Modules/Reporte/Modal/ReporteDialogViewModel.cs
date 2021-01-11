
using System;
using System.Windows.Input;

using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Factories;

using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Services.CuadreService;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.FlujoEfectivo.Inicio;

namespace ClienteMarWPF.UI.Modules.Reporte.Modal
{
    public class ReporteDialogViewModel : BaseViewModel
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


 


        public ReporteDialogViewModel(INavigator nav, IAuthenticator aut, IViewModelFactory vistas, IBancaService banService, ICuadreService cuadreService)
        {
            _nav = nav;
            _aut = aut;
            _vistas = vistas;
            _banService = banService;
            _cuadreService = cuadreService;

 
        }



        public void Mostrar()
        {
            MuestroDialogo = true; 
        }

        public void Ocultar()
        {
            MuestroDialogo = false; 
        }





    }//fin de clase
}
