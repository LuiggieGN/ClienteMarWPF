using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.SorteosService;
using ClienteMarWPF.UI.Modules.Sorteos.Modal;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Sorteos;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Timers;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.Sorteos
{
    public class SorteosViewModel: BaseViewModel
    {
        public ICommand RealizarApuestaCommand { get; }
        public ICommand GetListadoTicketsCommand { get; }
        public ICommand GetUltimosSorteosCommand { get; }
        public ICommand ValidarPagoTicketCommand { get; }

        public ObservableCollection<MAR_Bet> listaTicketsJugados;
        public ObservableCollection<UltimosSorteos> ganadores;
        private ValidarPagoTicketViewModel _dialog;

        public SorteosViewModel(IAuthenticator autenticador, ISorteosService sorteosService)
        {
            RealizarApuestaCommand = new RealizarApuestaCommand(this, autenticador, sorteosService);
            GetListadoTicketsCommand = new GetListadoTicketsCommand(this, autenticador, sorteosService);
            GetUltimosSorteosCommand = new GetUltimosSorteosCommand(this, autenticador, sorteosService);
            ValidarPagoTicketCommand = new ValidarPagoTicketCommand(this, autenticador, sorteosService);

            listaTicketsJugados = new ObservableCollection<MAR_Bet>();
            ganadores = new ObservableCollection<UltimosSorteos>();
            GetListadoTicketsCommand.Execute(null);
            GetUltimosSorteosCommand.Execute(null);
        }

        public ObservableCollection<MAR_Bet> ListaTickets
        {
            get
            {
                return listaTicketsJugados;
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
        #endregion


    }
}
