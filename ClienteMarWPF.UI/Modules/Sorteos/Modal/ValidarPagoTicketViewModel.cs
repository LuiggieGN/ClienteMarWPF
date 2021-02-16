using ClienteMarWPF.Domain.Services.RecargaService;
using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Sorteos;
using ClienteMarWPF.UI.ViewModels.Factories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Sorteos.Modal
{
    public class ValidarPagoTicketViewModel: BaseViewModel
    {

        public ICommand CerrarValidarPagoTicketCommand { get; }
        public ICommand ConsultarTicketCommand { get; }
        public ICommand PagarTicketCommand { get; }
        public ICommand AnularTicketCommand { get; }

        public ValidarPagoTicketViewModel(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService)
        {

            SetMensajeToDefaultSate();


            CerrarValidarPagoTicketCommand = new CerrarValidarPagoTicketCommand(this);
            ConsultarTicketCommand = new ConsultarTicketCommand(this, autenticador, sorteosService);
            PagarTicketCommand = new PagarTicketCommand(this, autenticador, sorteosService);
            AnularTicketCommand = new AnularTicketCommand(this, autenticador, sorteosService);
        }

        #region PropertyOfView
        //###########################################################
        private string _ticketNumero;
        private string _ticketPin;
        private string _montoPorPagar;
        private string _mensajeResponse;
        private string _mensajeBackground;
        private string _mensajeIcono;
        private bool _pudePagar;
        private bool _mostrarMensajes;
        //###########################################################
        public string TicketNumero
        {
            get
            {
                return _ticketNumero;
            }
            set
            {
                _ticketNumero = value;
                NotifyPropertyChanged(nameof(TicketNumero));
            }
        }        
        public string TicketPin
        {
            get
            {
                return _ticketPin;
            }
            set
            {
                _ticketPin = value;
                NotifyPropertyChanged(nameof(TicketPin));
            }
        }        
        public string MontoPorPagar
        {
            get
            {
                return _montoPorPagar;
            }
            set
            {
                _montoPorPagar = value;
                NotifyPropertyChanged(nameof(MontoPorPagar));
            }
        }
        public bool PudePagar
        {
            get
            {
                return _pudePagar;
            }
            set
            {
                _pudePagar = value;
                NotifyPropertyChanged(nameof(PudePagar));
            }
        }        
        public string MensajeResponse
        {
            get
            {
                return _mensajeResponse;
            }
            set
            {
                _mensajeResponse = value;
                NotifyPropertyChanged(nameof(MensajeResponse));
            }
        }

        public string MensajeBackground
        {
            get
            {
                return _mensajeBackground;
            }
            set
            {
                _mensajeBackground = value;
                NotifyPropertyChanged(nameof(MensajeBackground));
            }
        }

        public string MensajeIcono
        {
            get
            {
                return _mensajeIcono;
            }
            set
            {
                _mensajeIcono = value;
                NotifyPropertyChanged(nameof(MensajeIcono));
            }
        }

        public bool MostrarMensajes
        {
            get
            {
                return _mostrarMensajes;
            }
            set
            {
                _mostrarMensajes = value;
                NotifyPropertyChanged(nameof(MostrarMensajes));
            }
        }
        //###########################################################
        #endregion

        private bool _muestroDialogo;
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

        public void Mostrar()
        {
            MuestroDialogo = true;

        }

        public void Ocultar()
        {
            MuestroDialogo = false;

        }


        public void SetMensajeToDefaultSate() 
        {
            SetMensaje(string.Empty, "Asterisk", "#007BFF", false);
        }
        public void SetMensaje(string mensaje, string icono, string background, bool puedeMostrarse) 
        {
            MensajeResponse = mensaje;
            MensajeIcono = icono;
            MensajeBackground = background;
            MostrarMensajes = puedeMostrarse;        
        }




    }
}
