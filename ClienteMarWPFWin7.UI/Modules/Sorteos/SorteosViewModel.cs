using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.SorteosService;
using ClienteMarWPFWin7.UI.Modules.Sorteos.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Sorteos;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.Services.BancaService;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Globalization;

namespace ClienteMarWPFWin7.UI.Modules.Sorteos
{
    public class SorteosViewModel : BaseViewModel
    {
        #region Fields
        private ValidarPagoTicketViewModel _dialog;
        private string _ticketSeleccionado;
        private List<Jugada> _listJugadas;
        public bool buscarTicketsInService;//Variable para buscar en listado de ticket
        public ObservableCollection<TicketDTO> listaTicketsJugados;
        public ObservableCollection<UltimosSorteos> ganadores;
        public ObservableCollection<TicketDTO> listadoTicketPrecargada;
        private string _totalVentas;
        private string _totalVendidoHoy;
        public List<int> loteriasMultiples = new List<int>() { };
        private bool? _totalesCargados = false;
        private decimal _totalVendidoLoteria;
        private decimal _totalVentidoProducto;
        #endregion

        #region Services
        public IAuthenticator Autenticador { get; }
        public ISorteosService SorteoServicio { get; }
        public IBancaService BancaServicio { get; }
        #endregion

        #region Commands
        public ICommand RealizarApuestaCommand { get; }
        public ICommand GetListadoTicketsCommand { get; }
        public ICommand GetGanadoresCommand { get; }
        public ICommand ValidarPagoTicketCommand { get; }
        public ICommand LeerBancaTotalVendidoHoyCommand { get; }
        #endregion


        public SorteosViewModel(IAuthenticator autenticador,
                                ISorteosService sorteosService,
                                IBancaService bancaService)
        {
            Autenticador = autenticador;
            SorteoServicio = sorteosService;
            BancaServicio = bancaService;


            RealizarApuestaCommand = new RealizarApuestaCommand(this, autenticador, sorteosService);
            //GetListadoTicketsCommand = new GetListadoTicketsCommand(this, autenticador, sorteosService,null);
            GetGanadoresCommand = new GetGanadoresCommand(this, autenticador, sorteosService);
            ValidarPagoTicketCommand = new ValidarPagoTicketCommand(this, autenticador, sorteosService,bancaService);
            LeerBancaTotalVendidoHoyCommand = new LeerBancaTotalVendidoHoyCommand(this);



            listaTicketsJugados = new ObservableCollection<TicketDTO>();
            ganadores = new ObservableCollection<UltimosSorteos>();
            listadoTicketPrecargada = new ObservableCollection<TicketDTO>();
            //CopiarTicketCommand = new CopiarTicketCommand(this, autenticador, sorteosService);
            //GetListadoTicketsCommand.Execute(null);

            //!!@@ Ojo => Bug
            //GetGanadoresCommand.Execute(null); //@Bug!! hace que el modulo cargue demasiado lento debido a que se hace llamdas a metodo de servicio dentro de u foreach
            //    !! Si se descomenta favor notificar att: Jaasiel y Luiggie 


            TotalesCargados = No;
            TransaccionesPendientes = new List<decimal>();
            TotalVendidoHoy = $"Ventas de Sorteos | ..Cargando!!";

            Task.Factory.StartNew(() =>
            {
                SubProceso001();
            });
        }


        private void SubProceso001()
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(
            DispatcherPriority.Background,
            new Action(() =>
            {
                IsBusy_LeerVendidoHoy = true;
                LeerBancaTotalVendidoHoyCommand?.Execute(null);
                IsBusy_LeerVendidoHoy = false;
            }));
        }


        private bool IsBusy_LeerVendidoHoy = false;
        public void LeerVendidoHoyAsync()
        {
            if (!IsBusy_LeerVendidoHoy)
            {
                Task.Factory.StartNew(() =>
                {
                    IsBusy_LeerVendidoHoy = true;
                    LeerBancaTotalVendidoHoyCommand?.Execute(null);
                    IsBusy_LeerVendidoHoy = false;

                });
            }
        }


        #region PropertyOfView 

        public bool? TotalesCargados
        {
            get
            {
                return _totalesCargados;
            }
            set
            {
                _totalesCargados = value;
                NotifyPropertyChanged(nameof(TotalesCargados));
            }

        }

        public decimal TotalVendidoLoteria
        {
            get
            {
                return _totalVendidoLoteria;
            }
            set
            {
                _totalVendidoLoteria = value;
                NotifyPropertyChanged(nameof(TotalVendidoLoteria));
            }
        }

        public decimal TotalVendidoProductos
        {
            get
            {
                return _totalVentidoProducto;
            }
            set
            {
                _totalVentidoProducto = value;
                NotifyPropertyChanged(nameof(TotalVendidoProductos));
            }
        }

        public List<decimal> TransaccionesPendientes { get; set; }



        public ObservableCollection<TicketDTO> ListaTickets
        {
            get
            {
                return listaTicketsJugados;
            }

        }


        public ObservableCollection<TicketDTO> ListadoTicketsPrecargados
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

        public bool BuscarTicketsInService
        {
            get
            {
                return buscarTicketsInService;
            }
            set
            {
                buscarTicketsInService = value;
                NotifyPropertyChanged(nameof(BuscarTicketsInService));
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
                _listJugadas = value; NotifyPropertyChanged(nameof(ListadoJugada));
            }
        }

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
        public string TotalVendidoHoy
        {
            get
            {
                return _totalVendidoHoy;
            }
            set
            {
                _totalVendidoHoy = value;
                NotifyPropertyChanged(nameof(TotalVendidoHoy));
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

        public SorteosView SorteoViewClass { get; set; }

        public List<int> LoteriasMultiples
        {
            get { return loteriasMultiples; }
            set { loteriasMultiples = value; NotifyPropertyChanged(nameof(LoteriasMultiples)); }
        }
        #endregion

        #region Logica Total Vendido Hoy
        public void AgregarTransaccionPendiente(decimal monto)
        {
            if (TransaccionesPendientes == null)
            {
                TransaccionesPendientes = new List<decimal>();
            }
            TransaccionesPendientes.Add(monto);
        }

        public void AgregarAlTotalVendidoHoy(decimal monto)
        {
            if (TotalesCargados.HasValue && TotalesCargados.Value == Si)
            {
                TotalVendidoLoteria += monto;
                TotalVendidoHoy = $"Vendido Hoy | Sorteo :  {TotalVendidoLoteria.ToString("C", new CultureInfo("en-US"))} | Productos : {TotalVendidoProductos.ToString("C", new CultureInfo("en-US"))}";
            }
        }
        #endregion

        #region Refrescar Balance
        private bool IsBusy_LeerBalanceThread = false;
        public void LeerBalanceAsync()
        {
            if (Autenticador != null &&
                Autenticador.BancaConfiguracion != null &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto != null &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto.PuedeUsarControlEfectivo == true &&
                Autenticador.BancaConfiguracion.ControlEfectivoConfigDto.BancaYaInicioControlEfectivo == true
                )
            {
                if (!IsBusy_LeerBalanceThread)
                {
                    Task.Factory.StartNew(() =>
                    {
                        IsBusy_LeerBalanceThread = true;
                        try
                        {
                            Autenticador.RefrescarBancaBalance();
                        }
                        catch
                        {

                        }
                        IsBusy_LeerBalanceThread = false;
                    });
                }
            }
        }
        #endregion

    }
}
