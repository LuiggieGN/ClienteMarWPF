using ClienteMarWPFWin7.Domain.Services.RecargaService;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos;
using ClienteMarWPFWin7.UI.ViewModels.Factories;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Linq;
using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Models.Dtos;

namespace ClienteMarWPFWin7.UI.Modules.Sorteos.Modal
{
    public class ValidarPagoTicketViewModel: BaseViewModel
    {

        private ObservableCollection<TicketDTO> listadotipojugadas;
        public ICommand CerrarValidarPagoTicketCommand { get; }
        public ICommand ConsultarTicketCommand { get; }
        public ICommand PagarTicketCommand { get; }
        public ICommand AnularTicketCommand { get; }
        public ICommand ReimprimirTicketCommand { get; }
        public ICommand GetListadoTicketsCommand { get; }
        public ICommand CopiarTicketCommand { get; }
        public ObservableCollection<TicketDTO> listaTicketsJugados { get => listadotipojugadas; 
            set {
                listadotipojugadas = value;
                NotifyPropertyChanged(nameof(listadotipojugadas));
                }}
        
        public SorteosView claseSorteo;
        public SorteosViewModel SorteoVM;

        public Action SeleccionarInputVacios;
        public Action SeleccionarTxtTicket;

        public ValidarPagoTicketViewModel(SorteosViewModel viewModel, IAuthenticator autenticador, ISorteosService sorteosService,IBancaService bancaService,Action seleccionarInputsVacios,Action seleccionarTxtTicket)
        {

            SetMensajeToDefaultSate();

            listaTicketsJugados = new ObservableCollection<TicketDTO>();

            CerrarValidarPagoTicketCommand = new CerrarValidarPagoTicketCommand(this);
            GetListadoTicketsCommand = new GetListadoTicketsCommand(viewModel, autenticador, sorteosService,bancaService,this);
            ConsultarTicketCommand = new ConsultarTicketCommand(this, autenticador, sorteosService);
            PagarTicketCommand = new PagarTicketCommand(this, autenticador, sorteosService);
            AnularTicketCommand = new AnularTicketCommand(this, autenticador, sorteosService,viewModel);
            ReimprimirTicketCommand = new ReimprimirTicketCommand(this, autenticador, sorteosService,viewModel);
            CopiarTicketCommand = new CopiarTicketCommand(viewModel, autenticador, sorteosService,false,this);
            SorteoVM = viewModel;
            SeleccionarInputVacios = seleccionarInputsVacios;
            SeleccionarTxtTicket = seleccionarTxtTicket;
            //this.TicketNumero = new SorteosView().GetTicketNumeroPrecargar();
            GetListadoTicketsCommand.Execute(null);

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
            SeleccionarTxtTicket?.Invoke();
        }

        public void Ocultar()
        {
            MuestroDialogo = false;
            SeleccionarInputVacios?.Invoke();
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

        public ObservableCollection<TicketDTO> ListaTickets
        {
            get
            {
                return listaTicketsJugados;
            }


        }

        public SorteosView ClaseSorteo
        {
            get { return claseSorteo; }
            set { claseSorteo = value; NotifyPropertyChanged(nameof(ClaseSorteo)); }
        }

        private string _totalVentas;
        //###########################################################
        public string TotalVentas
        {
            get
            {
                return _totalVentas;
            }
            set
            {
                _totalVentas = value;
                NotifyPropertyChanged(nameof(TotalVentas));
            }
        }

      
    }
}
