using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Timers;
using System.Windows.Input;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;

namespace ClienteMarWPFWin7.UI.Modules.Sorteos
{
    public class SorteosViewModel : BaseViewModel
    {
        public ICommand RealizarApuestaCommand { get; }
        public ICommand GetListadoTicketsCommand { get; }
        public ICommand GetGanadoresCommand { get; }
        public ICommand ValidarPagoTicketCommand { get; }
        //public ICommand CopiarTicketCommand { get; }

        public  IAuthenticator Autenticador { get; }

        public ObservableCollection<MAR_Bet> listaTicketsJugados;
        public ObservableCollection<UltimosSorteos> ganadores;
        public ObservableCollection<MAR_Bet> listadoTicketPrecargada;
        private ValidarPagoTicketViewModel _dialog;
        private string _ticketSeleccionado;
        private List<Jugada> _listJugadas;
        public SorteosView SorteoViewClass { get; set;}

        public List<int> loteriasMultiples = new List<int>() { };

        public SorteosViewModel(IAuthenticator autenticador, ISorteosService sorteosService)
        {
            Autenticador = autenticador;
            RealizarApuestaCommand = new RealizarApuestaCommand(this, autenticador, sorteosService);
            //GetListadoTicketsCommand = new GetListadoTicketsCommand(this, autenticador, sorteosService,null);
            GetGanadoresCommand = new GetGanadoresCommand(this, autenticador, sorteosService);
            ValidarPagoTicketCommand = new ValidarPagoTicketCommand(this, autenticador, sorteosService);
            
            
            listaTicketsJugados = new ObservableCollection<MAR_Bet>();
            ganadores = new ObservableCollection<UltimosSorteos>();
            listadoTicketPrecargada = new ObservableCollection<MAR_Bet>();
            //CopiarTicketCommand = new CopiarTicketCommand(this, autenticador, sorteosService);
            //GetListadoTicketsCommand.Execute(null);

            //!!@@ Ojo => Bug
            //GetGanadoresCommand.Execute(null); //@Bug!! hace que el modulo cargue demasiado lento debido a que se hace llamdas a metodo de servicio dentro de u foreach
                                                      //    !! Si se descomenta favor notificar att: Jaasiel y Luiggie 
           
        }

        public ObservableCollection<MAR_Bet> ListaTickets
        {
            get
            {
                return listaTicketsJugados;
            }
          

        }


        public ObservableCollection<MAR_Bet> ListadoTicketsPrecargados
        {
            get
            {
                return listadoTicketPrecargada;
            }
            set
            {
                listadoTicketPrecargada = value;
                NotifyPropertyChanged(nameof(listadoTicketPrecargada));
            }

        }

        public ObservableCollection<UltimosSorteos> UltimosSorteos
        {
            get
            {
                return ganadores;
            }

        }
        public ValidarPagoTicketViewModel Dialog
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

        public List<Jugada> ListadoJugada
        {
            get
            {
                return _listJugadas;
            }
            set
            {
               _listJugadas= value; NotifyPropertyChanged(nameof(ListadoJugada));
            }
        }

     

        #region PropertyOfView
        //###########################################################
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
        //###########################################################
        public string TicketSeleccionado
        {
            get
            {
                return _ticketSeleccionado;
            }
            set
            {
                _ticketSeleccionado = value;
                NotifyPropertyChanged(nameof(TicketSeleccionado));
            }
        }

        public List<int> LoteriasMultiples
        {
            get { return loteriasMultiples; }
            set { loteriasMultiples = value; NotifyPropertyChanged(nameof(LoteriasMultiples)); }
        }
        #endregion

    }
}
