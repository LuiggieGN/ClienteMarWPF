using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Recargas;
using ClienteMarWPF.UI.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Recargas.Modal
{
   public class DialogImprimirTicketViewModel : BaseViewModel
    {
        private bool _muestroDialogo;
        private readonly INavigator _nav;
        private readonly IAuthenticator _aut;
        private readonly IRecargaService _recargaService;
        private readonly IViewModelFactory _vistas;
     



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


        public ICommand CerrarDialogoInicioCommand { get; }

        public ICommand GenerarTicketCommand { get; }



        public DialogImprimirTicketViewModel(INavigator nav, IAuthenticator aut, IViewModelFactory vistas, IRecargaService recargaService)
        {
            _nav = nav;
            _aut = aut;
            _vistas = vistas;
            _recargaService = recargaService;


            CerrarDialogoInicioCommand = new CerrarDialogoImprimirTicketCommand(this);
            GenerarTicketCommand = new DialogConfirmTicketCommand(this, aut, _recargaService);

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
