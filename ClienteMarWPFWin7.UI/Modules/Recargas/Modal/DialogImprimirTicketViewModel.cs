using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.State.PinterConfig;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Recargas;
using ClienteMarWPFWin7.UI.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ClienteMarWPFWin7.UI.Modules.Recargas.Modal
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



        public DialogImprimirTicketViewModel(INavigator nav, IAuthenticator aut, IViewModelFactory vistas, IRecargaService recargaService,RecargasIndexRecarga recargas)
        {
            _nav = nav;
            _aut = aut;
            _vistas = vistas;
            _recargaService = recargaService;


            CerrarDialogoInicioCommand = new CerrarDialogoImprimirTicketCommand(this);
            GenerarTicketCommand = new DialogConfirmTicketCommand(this, aut, _recargaService,recargas);
           

        }

        public DialogImprimirTicketViewModel()
        {
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
