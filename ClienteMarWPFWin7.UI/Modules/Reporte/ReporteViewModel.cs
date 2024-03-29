﻿
using Accessibility;
using ClienteMarWPFWin7.Data;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Services.JuegaMasService;
using ClienteMarWPFWin7.Domain.Services.ReportesService;
using ClienteMarWPFWin7.UI.Modules.Reporte.Modal;
using ClienteMarWPFWin7.UI.State.Authenticators;
using ClienteMarWPFWin7.UI.State.Navigators;

using ClienteMarWPFWin7.UI.ViewModels;
using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Reporte;
using ClienteMarWPFWin7.UI.ViewModels.Commands.Reportes;
using ClienteMarWPFWin7.UI.ViewModels.ModelObservable;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ClienteMarWPFWin7.UI.Modules.Reporte
{
    public class ReporteViewModel : BaseViewModel
    {
        #region "Campos"
        public List<MAR_Loteria2> _sorteos;
        //###########################################################
        private DateTime _fecha;
        private string _reporte;
        private int _loteriaID;
        private string _nombrereporte;
        private string _nombrebanca;
        private DateTime _fechareporte;
        private string _fechaActualReport;
        private string _totalVendidoListaTarjeta;
        private int _reporteID;
        private string fechaReporteLabel;
        // Reportes Ventas Por Fecha
        private DateTime _fechainicio;
        private DateTime _fechaFin;
        private string _labeldesde;
        private string _labelhasta;
        private bool _soloTotales;
        private string _totalComision;
        private string _totalSaco;
        private string _totalBalance;
        private string _totalVenta;
        /////////////////////////////
        private string totalresultado;
        private string totalcomision;
        private string totalsaco;
        private string totalbalance;
        public string _nombreloteria;
        public string totalesPagosRemotos;
        public TotalListNumeros totalesListNumeros;

        //////////// Reportes a mostrar /////////
        private Visibility _rptsumaventas;
        private Visibility _rptticketGanadores;
        private Visibility _rptSumVentasFecha;
        private Visibility _rptlisttarjetas;
        private Visibility _rptVentas;
        private Visibility _rptlistticket;
        private Visibility _rptpagosremotos;
        private Visibility _rptlistnumeros;
        private Visibility _rptlistpremios;
        ////////////////////////////////////////
        private ObservableCollection<ReportesSumVentasObservable> _informacionesreportes;
        private ObservableCollection<ReportesObservable> _reportes;
        private DataGrid GridAgrupado;
        private EstadoDeTicketGanadores _reporteganadoresbinding = new EstadoDeTicketGanadores();
        private ObservableCollection<ReportesSumVentasFechaObservable> _reporteSumVentasPorFecha;
        private ObservableCollection<ReportesListaTajetasObservable> _reporteListaTarjetas;
        private ObservableCollection<ReporteListaTicketsObservable> _reporteListaTicket;
        private ObservableCollection<ReporteListaTicketsObservable> _reporteAllDataListaTicket;
        private ObservableCollection<ReporteListaTicketsObservable> _reportepagoRemoto;
        private ReporteListNumeroColumns _reporteListaNumeros = new ReporteListNumeroColumns() { };
        private string _reporteListadopremioObservable = null;
       
        private DataGrid _repoteSumFech;
        private ReportesDeVentas _reporteventas = new ReportesDeVentas();
        private ObservableCollection<ReporteListaTicketsObservable> _ticketsNulosReportVentas = new ObservableCollection<ReporteListaTicketsObservable>();
        private TotalesListadoTicket _totalesListTicket = new TotalesListadoTicket();
        private PremiosVentas _premiosventas = new PremiosVentas();
        private bool canChangeOptionListTicket;
        private DialogoReporteViewModel _dialog;

        /// para loadings y iconos de reportes
        Visibility loadingRPTVentas = Visibility.Collapsed;
        Visibility loadingRPTListNumero = Visibility.Collapsed;
        Visibility loadingRPTGanadores = Visibility.Collapsed;
        Visibility loadingRPTListTarjeta = Visibility.Collapsed;
        Visibility loadingRPTSumaVentas = Visibility.Collapsed;
        Visibility loadingRPTVentasPorFechaResumen = Visibility.Collapsed;
        Visibility loadingImpresion = Visibility.Collapsed;
        Visibility loadingRPTVentasPagosRemotos = Visibility.Collapsed;
        Visibility loadingRPTVentasNumerosGanadores = Visibility.Collapsed;
        Visibility loadingRPTListTicket = Visibility.Collapsed;

        Visibility iconRPTVentas = Visibility.Visible;
        Visibility iconRPTListNumero = Visibility.Visible;
        Visibility iconRPTGanadores = Visibility.Visible;
        Visibility iconRPTListTarjeta = Visibility.Visible;
        Visibility iconRPTSumaVentas = Visibility.Visible;
        Visibility iconRPTVentasPorFechaResumen = Visibility.Visible;
        Visibility iconRPTVentasPorFechaNoResumen = Visibility.Visible;
        Visibility iconRPTVentasPagosRemotos = Visibility.Visible;
        Visibility iconRPTVentasNumerosGanadores = Visibility.Visible;
        Visibility iconRPTListTicket = Visibility.Visible;
        Visibility iconImpresion = Visibility.Collapsed;
        #endregion

        #region "Propiedades"
        //###########################################################

        public DateTime Fecha
        {
            get
            {
                return _fecha;
            }
            set
            {
                _fecha = value;
                NotifyPropertyChanged(nameof(Fecha));
            }
        }

        public DataGrid ReporteSumFecha
        {
            get { return _repoteSumFech; }
            set { _repoteSumFech = value; NotifyPropertyChanged(nameof(FechaInicio)); }
        }

        public DateTime FechaInicio
        {
            get { return _fechainicio; }
            set { _fechainicio = value; NotifyPropertyChanged(nameof(FechaInicio)); }
        }

        public DateTime FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; NotifyPropertyChanged(nameof(FechaFin)); }
        }
        public string FechaReporteLabel
        {
            get { return fechaReporteLabel; }
            set { fechaReporteLabel = value; NotifyPropertyChanged(nameof(FechaReporteLabel)); }
        }
       

        public string TotalVentaListTarjeta
        {
            get { return _totalVendidoListaTarjeta; }
            set { _totalVendidoListaTarjeta = value; NotifyPropertyChanged(nameof(TotalVentaListTarjeta)); }
        }

        public string LabelDesde
        {
            get { return _labeldesde; }
            set { _labeldesde = value; NotifyPropertyChanged(nameof(LabelDesde)); }
        }

        public string LabelHasta
        {
            get { return _labelhasta; }
            set { _labelhasta = value; NotifyPropertyChanged(nameof(LabelHasta)); }
        }

        public string ObservableListadoPremios
        {
            get { return _reporteListadopremioObservable; }
            set { _reporteListadopremioObservable = value; NotifyPropertyChanged(nameof(ObservableListadoPremios)); }
        }

        public TotalesListadoTicket TotalesListTicket
        {
            get { return _totalesListTicket; }
            set { _totalesListTicket = value; NotifyPropertyChanged(nameof(TotalesListTicket)); }
        }

        public bool SoloTotales
        {
            get { return _soloTotales; }
            set { _soloTotales = value; NotifyPropertyChanged(nameof(SoloTotales)); }
        }

        public string NombreReporte
        {
            get { return _nombrereporte; }
            set { _nombrereporte = value; NotifyPropertyChanged(nameof(NombreReporte)); }
        }

        public string TotalVentasSumVenFecha
        {
            get { return _totalVenta; }
            set { _totalVenta = value; NotifyPropertyChanged(nameof(TotalVentasSumVenFecha)); }
        }

        public string TotalComisSumVenFecha
        {
            get { return _totalComision; }
            set { _totalComision = value; NotifyPropertyChanged(nameof(TotalComisSumVenFecha)); }
        }
        public string TotalSacoSumVenFecha
        {
            get { return _totalSaco; }
            set { _totalSaco = value; NotifyPropertyChanged(nameof(TotalSacoSumVenFecha)); }
        }
        public string TotalBalanSumVenFecha
        {
            get { return _totalBalance; }
            set { _totalBalance = value; NotifyPropertyChanged(nameof(TotalBalanSumVenFecha)); }
        }
        public string NombreLoteria
        {
            get { return _nombreloteria; }
            set { _nombreloteria = value; NotifyPropertyChanged(nameof(NombreLoteria)); }
        }

        public ReportesDeVentas ReportesDeVentas
        {
            get { return _reporteventas; }
            set { _reporteventas = value; NotifyPropertyChanged(nameof(ReportesDeVentas)); }

        }
        public ObservableCollection<ReporteListaTicketsObservable> TicketNulosReporteVentas
        {
            get { return _ticketsNulosReportVentas; }
            set { _ticketsNulosReportVentas = value; NotifyPropertyChanged(nameof(TicketNulosReporteVentas)); }
        }

        public Visibility RPTSumaVentasVisibility
        {
            get { return _rptsumaventas; }
            set { _rptsumaventas = value; NotifyPropertyChanged(nameof(RPTSumaVentasVisibility)); }
        }

        public Visibility RPTTicketGanadoresVisibility
        {
            get { return _rptticketGanadores; }
            set { _rptticketGanadores = value; NotifyPropertyChanged(nameof(RPTTicketGanadoresVisibility)); }
        }

        public Visibility RPTSumVentaFechaVisibility
        {
            get { return _rptSumVentasFecha; }
            set { _rptSumVentasFecha = value; NotifyPropertyChanged(nameof(RPTSumVentaFechaVisibility)); }
        }

        public Visibility RPTListTarjetasVisibility
        {
            get { return _rptlisttarjetas; }
            set { _rptlisttarjetas = value; NotifyPropertyChanged(nameof(RPTListTarjetasVisibility)); }
        }

        public Visibility RPTVentasVisibily
        {
            get { return _rptVentas; }
            set { _rptVentas = value; NotifyPropertyChanged(nameof(RPTVentasVisibily)); }
        }

        public Visibility RPTListNumerosVisibility
        {
            get { return _rptlistnumeros; }
            set { _rptlistnumeros = value; NotifyPropertyChanged(nameof(RPTListNumerosVisibility)); }
        }

        public Visibility RPTLitTicketVisibility
        {
            get { return _rptlistticket; }
            set { _rptlistticket = value; NotifyPropertyChanged(nameof(RPTLitTicketVisibility)); }
        }

        public Visibility RPTPagosRemotosVisibility
        {
            get { return _rptpagosremotos; }
            set { _rptpagosremotos = value; NotifyPropertyChanged(nameof(RPTPagosRemotosVisibility)); }
        }

        public Visibility RPTListPremioVisibility
        {
            get { return _rptlistpremios; }
            set { _rptlistpremios = value; NotifyPropertyChanged(nameof(RPTListPremioVisibility)); }
        }

        public string FechaActualReport
        {
            get { return _fechaActualReport; }
            set { _fechaActualReport = value; NotifyPropertyChanged(nameof(FechaActualReport)); }
        }

        public EstadoDeTicketGanadores ReportesGanadores
        {
            get { return _reporteganadoresbinding; }
            set { _reporteganadoresbinding = value; NotifyPropertyChanged(nameof(ReportesGanadores)); }
        }

        public ObservableCollection<ReporteListaTicketsObservable> ReporteListTicket
        {
            get { return _reporteListaTicket; }
            set { _reporteListaTicket = value; NotifyPropertyChanged(nameof(ReporteListTicket)); }
        }
        public ObservableCollection<ReporteListaTicketsObservable> ReporteAllDataListTicket
        {
            get { return _reporteAllDataListaTicket; }
            set { _reporteAllDataListaTicket = value; NotifyPropertyChanged(nameof(ReporteAllDataListTicket)); }
        }
        public ObservableCollection<ReporteListaTicketsObservable> ReportePagosRemotos
        {
            get { return _reportepagoRemoto; }
            set { _reportepagoRemoto = value; NotifyPropertyChanged(nameof(ReportePagosRemotos)); }
        }

        public ObservableCollection<ReportesSumVentasFechaObservable> ReportesSumVentasPorFecha
        {
            get { return _reporteSumVentasPorFecha; }
            set { _reporteSumVentasPorFecha = value; NotifyPropertyChanged(nameof(ReportesSumVentasPorFecha)); }
        }

        public ObservableCollection<ReportesListaTajetasObservable> ReportesListaTarjetas
        {
            get { return _reporteListaTarjetas; }
            set { _reporteListaTarjetas = value; NotifyPropertyChanged(nameof(ReportesListaTarjetas)); }
        }
        public ReporteListNumeroColumns ReportesListaNumeros
        {
            get { return _reporteListaNumeros; }
            set { _reporteListaNumeros = value; NotifyPropertyChanged(nameof(ReportesListaNumeros)); }
        }

        public PremiosVentas PremiosVentas
        {
            get { return _premiosventas; }
            set { _premiosventas = value; NotifyPropertyChanged(nameof(PremiosVentas)); }
        }

        public string TotalesPagosRemotos
        {
            get { return totalesPagosRemotos; }
            set { totalesPagosRemotos = value; NotifyPropertyChanged(nameof(TotalesPagosRemotos)); }
        }

        public string Reporte
        {
            get
            {
                return _reporte;
            }
            set
            {
                _reporte = value;
                Console.WriteLine(_reporte);
                NotifyPropertyChanged(nameof(Reporte));
            }
        }

        public ObservableCollection<ReportesObservable> Reportes
        {
            get
            {
                return _reportes = new ObservableCollection<ReportesObservable> {
                 new ReportesObservable  {Nombre="Reportes De Ventas",ReporteID=1,IsChecked=true},
                 new ReportesObservable  {Nombre="Lista De Numeros",ReporteID=2,IsChecked=false},
                 new ReportesObservable  {Nombre="Lista De Tickets",ReporteID=3,IsChecked=false},
                 new ReportesObservable  {Nombre="Reportes Ganadores",ReporteID=4,IsChecked=false},
                 new ReportesObservable  {Nombre="Lista De Tarjetas",ReporteID=5,IsChecked=false},
                 new ReportesObservable  {Nombre="Suma De Ventas",ReporteID=6,IsChecked=false},
                 new ReportesObservable  {Nombre="Ventas por Fecha",ReporteID=7,IsChecked=false},
                 new ReportesObservable  {Nombre="Pagos Remotos",ReporteID=8,IsChecked=false},
                 new ReportesObservable  {Nombre="Lista De Premios",ReporteID=9,IsChecked=false}
                };
            }
            set
            {
                _reportes = value;
                NotifyPropertyChanged(nameof(Reporte));
            }
        }

        public List<MAR_Loteria2> Sorteos
        {
            get
            {
                return _sorteos;
            }
            set
            {
                _sorteos = value;
                NotifyPropertyChanged(nameof(Sorteos));
            }
        }

        public int LoteriaID
        {
            get
            {
                return _loteriaID;
            }
            set
            {
                _loteriaID = value;
                NotifyPropertyChanged(nameof(LoteriaID));
            }
        }

        public string NombreBanca
        {
            get { return _nombrebanca; }
            set { _nombrebanca = value; NotifyPropertyChanged(nameof(NombreBanca)); }
        }

        public DateTime FechaReporte
        {
            get { return _fechareporte; }
            set { _fechareporte = value; NotifyPropertyChanged(nameof(FechaReporte)); }
        }
        public int ReporteID
        {
            get { return _reporteID; }
            set { _reporteID = value; NotifyPropertyChanged(nameof(ReporteID)); }
        }

        public ObservableCollection<ReportesSumVentasObservable> InformacionesReportes
        {
            get { return _informacionesreportes; }
            set { _informacionesreportes = value; NotifyPropertyChanged(nameof(InformacionesReportes)); }
        }

        public DataGrid GridAgrupadoMode
        {
            get { return GridAgrupado; }
            set { GridAgrupado = value; NotifyPropertyChanged(nameof(GridAgrupadoMode)); }
        }


        public string TotalResultado
        {
            get { return totalresultado; }
            set { totalresultado = value; NotifyPropertyChanged(nameof(TotalResultado)); }
        }

        public string TotalComision
        {
            get { return totalcomision; }
            set { totalcomision = value; NotifyPropertyChanged(nameof(TotalComision)); }
        }
        public string TotalSaco
        {
            get { return totalsaco; }
            set { totalsaco = value; NotifyPropertyChanged(nameof(TotalSaco)); }
        }
        public string TotalBalance
        {
            get { return totalbalance; }
            set { totalbalance = value; NotifyPropertyChanged(nameof(TotalBalance)); }
        }

        public TotalListNumeros TotalListNumeros
        {
            get { return totalesListNumeros; }
            set { totalesListNumeros = value; NotifyPropertyChanged(nameof(TotalListNumeros)); }
        }
        public bool CanChangeOptionListTicket
        {
            get { return canChangeOptionListTicket; }
            set { canChangeOptionListTicket = value; NotifyPropertyChanged(nameof(CanChangeOptionListTicket)); }
        }

        public DialogoReporteViewModel Dialogo
        {
            get { return _dialog; }
            set { _dialog = value; NotifyPropertyChanged(nameof(Dialogo)); }
        }

        public ObservableCollection<ReportesObservable> ReporteBinding;
        public ObservableCollection<ReportesSumVentasObservable> ReporteMostrarBinding;
        //###########################################################

        #endregion

        #region "Comandos"
        public ICommand ObtenerReportes { get; }
        public ICommand PrintReportes { get; }
        public ICommand ChangeOptionListTicket { get; }
        public ICommand AbrirModalRangoFechaCommand { get; }
        #endregion


        public ReporteViewModel(IAuthenticator autenticador, IReportesServices reportesServices,IJuegaMasService servicioJuegaMas)
        {
            Fecha = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"), CultureInfo.InvariantCulture);
            FechaInicio = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"), CultureInfo.InvariantCulture).AddDays(-7);
            FechaFin = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"), CultureInfo.InvariantCulture).AddDays(-1);
            SoloTotales = true;
            ObtenerReportes = new GetReportesCommand(this, autenticador, reportesServices,servicioJuegaMas);
            PrintReportes = new PrintReports(this, autenticador, reportesServices,servicioJuegaMas);
            ChangeOptionListTicket = new ChangeOpcionListTicket(this, autenticador, reportesServices);
            AbrirModalRangoFechaCommand = new AbrirModalRangoFechaCommand(this);

          
            //Ocultando vistas de todos los reportes inicialmente
            RPTSumaVentasVisibility = Visibility.Hidden;
            RPTTicketGanadoresVisibility = Visibility.Hidden;
            RPTSumVentaFechaVisibility = Visibility.Hidden;
            RPTListTarjetasVisibility = Visibility.Hidden;
            RPTVentasVisibily = Visibility.Hidden;
            RPTLitTicketVisibility = Visibility.Hidden;
            RPTPagosRemotosVisibility = Visibility.Hidden;
            RPTListNumerosVisibility = Visibility.Hidden;
            ReportesListaNumeros.QuinielaVisibilty = Visibility.Hidden;
            ReportesListaNumeros.PaleVisibility = Visibility.Hidden;
            ReportesListaNumeros.TripletaVisibility = Visibility.Hidden;
            ReportesGanadores.MostrarNoHayGanadoresVisibity = Visibility.Hidden;
            RPTListPremioVisibility = Visibility.Hidden;
            /////////////////////////////////////////////////////
            ///
            /// Inicializando posiciones de componentes de listadoNumero//
            /// 
            ReportesListaNumeros.PosicionTituloQuiniela = 1;
            ReportesListaNumeros.PosicionTablaQuiniela = 2;
            ReportesListaNumeros.PosicionTotalesQuiniela = 3;
            ReportesListaNumeros.PosicionTituloPale = 4;
            ReportesListaNumeros.PosicionTablaPale = 5;
            ReportesListaNumeros.PosicionTotalesPale = 6;
            ReportesListaNumeros.PosicionTituloTripleta = 7;
            ReportesListaNumeros.PosicionTablaTripleta = 8;
            ReportesListaNumeros.PosicionTotalesTripleta = 9;

            //////////////////////////////////////////////////////////////
            ///// Inicializando posiciones de componentes de ganadores /////
            ReportesGanadores.PosicionTituloPagados = 2;
            ReportesGanadores.PosicionTablaPagados = 3;
            ReportesGanadores.PosicionTituloPendientesPagos = 4;
            ReportesGanadores.PosicionTablaPendientesPagos = 5;
            ReportesGanadores.PosicionTituloSinReclamar = 6;
            ReportesGanadores.PosicionTablaSinReclamar = 7;
            ReportesGanadores.PosicionBalance = 8;

            ////////////////////////////////////////////////////////////////

            ReportesListaNumeros.Quiniela = new ObservableCollection<ReportesListaNumerosObservable>() { };
            ReportesListaNumeros.Pale = new ObservableCollection<ReportesListaNumerosObservable>() { };
            ReportesListaNumeros.Tripleta = new ObservableCollection<ReportesListaNumerosObservable>() { };
            MAR_Loteria2 opcionTodas = new MAR_Loteria2() { Nombre = "Todas", LoteriaKey = 0 };
            Sorteos = new List<MAR_Loteria2>() { };
            Sorteos.Add(opcionTodas);
            foreach (var loteria in SessionGlobals.LoteriasTodas) { Sorteos.Add(loteria); }

            PremiosVentas.MostrarPremios = Visibility.Hidden;
            PremiosVentas.NoMostrarPremios = Visibility.Hidden;
        }

        #region
        public Visibility LoadingRPTVentas
        {
            get { return loadingRPTVentas; }
            set { loadingRPTVentas = value; NotifyPropertyChanged(nameof(loadingRPTVentas)); }
        }
        public Visibility LoadingRPTListNumero
        {
            get { return loadingRPTListNumero; }
            set { loadingRPTListNumero = value; NotifyPropertyChanged(nameof(loadingRPTListNumero)); }
        }
        public Visibility LoadingRPTGanadores
        {
            get { return loadingRPTGanadores; }
            set { loadingRPTGanadores = value; NotifyPropertyChanged(nameof(loadingRPTGanadores)); }
        }
        public Visibility LoadingRPTListTarjeta
        {
            get { return loadingRPTListTarjeta; }
            set { loadingRPTListTarjeta = value; NotifyPropertyChanged(nameof(loadingRPTListTarjeta)); }
        }
        public Visibility LoadingRPTSumaVentas
        {
            get { return loadingRPTSumaVentas; }
            set { loadingRPTSumaVentas = value; NotifyPropertyChanged(nameof(loadingRPTSumaVentas)); }
        }
        public Visibility LoadingRPTVentasPorFecha
        {
            get { return loadingRPTVentasPorFechaResumen; }
            set { loadingRPTVentasPorFechaResumen = value; NotifyPropertyChanged(nameof(loadingRPTVentasPorFechaResumen)); }
        }

        public Visibility LoadingRPTPagosRemotos
        {
            get { return loadingRPTVentasPagosRemotos; }
            set { loadingRPTVentasPagosRemotos = value; NotifyPropertyChanged(nameof(loadingRPTVentasPagosRemotos)); }
        }
        public Visibility LoadingRPTListPremiosGanadores
        {
            get { return loadingRPTVentasNumerosGanadores; }
            set { loadingRPTVentasNumerosGanadores = value; NotifyPropertyChanged(nameof(loadingRPTVentasNumerosGanadores)); }
        }
        public Visibility LoadingRPTListTicket
        {
            get { return loadingRPTListTicket; }
            set { loadingRPTListTicket = value; NotifyPropertyChanged(nameof(loadingRPTListTicket)); }
        }

        public Visibility LoadingImprimir
        {
            get { return loadingImpresion; }
            set { loadingImpresion = value; NotifyPropertyChanged(nameof(loadingImpresion)); }
        }

        public Visibility IconRPTVentas
        {
            get { return iconRPTVentas; }
            set { iconRPTVentas = value; NotifyPropertyChanged(nameof(iconRPTVentas)); }
        }
        public Visibility IconRPTListNumero
        {
            get { return iconRPTListNumero; }
            set { iconRPTListNumero = value; NotifyPropertyChanged(nameof(iconRPTListNumero)); }
        }
        public Visibility IconRPTGanadores
        {
            get { return iconRPTGanadores; }
            set { iconRPTGanadores = value; NotifyPropertyChanged(nameof(iconRPTGanadores)); }
        }
        public Visibility IconRPTListTarjeta
        {
            get { return iconRPTListTarjeta; }
            set { iconRPTListTarjeta = value; NotifyPropertyChanged(nameof(iconRPTListTarjeta)); }
        }
        public Visibility IconRPTSumaVentas
        {
            get { return iconRPTSumaVentas; }
            set { iconRPTSumaVentas = value; NotifyPropertyChanged(nameof(iconRPTSumaVentas)); }
        }
        public Visibility IconRPTVentasPorFecha
        {
            get { return iconRPTVentasPorFechaResumen; }
            set { iconRPTVentasPorFechaResumen = value; NotifyPropertyChanged(nameof(iconRPTVentasPorFechaResumen)); }
        }
        public Visibility IconRPTVentasPorFechaNoResumen
        {
            get { return iconRPTVentasPorFechaNoResumen; }
            set { iconRPTVentasPorFechaNoResumen = value; NotifyPropertyChanged(nameof(iconRPTVentasPorFechaNoResumen)); }
        }
        public Visibility IconRPTPagosRemotos
        {
            get { return iconRPTVentasPagosRemotos; }
            set { iconRPTVentasPagosRemotos = value; NotifyPropertyChanged(nameof(iconRPTVentasPagosRemotos)); }
        }
        public Visibility IconRPTListPremiosGanadores
        {
            get { return iconRPTVentasNumerosGanadores; }
            set { iconRPTVentasNumerosGanadores = value; NotifyPropertyChanged(nameof(iconRPTVentasNumerosGanadores)); }
        }

        public Visibility IconRPTListTicket
        {
            get { return iconRPTListTicket; }
            set { iconRPTListTicket = value; NotifyPropertyChanged(nameof(iconRPTListTicket)); }
        }
        public Visibility IconImprimir
        {
            get { return iconImpresion; }
            set { iconImpresion = value; NotifyPropertyChanged(nameof(iconImpresion)); }
        }
        #endregion




    }
}
