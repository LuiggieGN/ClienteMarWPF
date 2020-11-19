
using Accessibility;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Services.ReportesService;
using ClienteMarWPF.UI.State.Authenticators;
using ClienteMarWPF.UI.State.Navigators;

using ClienteMarWPF.UI.ViewModels;
using ClienteMarWPF.UI.ViewModels.Base;
using ClienteMarWPF.UI.ViewModels.Commands.Reporte;
using ClienteMarWPF.UI.ViewModels.ModelObservable;
using MarPuntoVentaServiceReference;
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

namespace ClienteMarWPF.UI.Modules.Reporte
{
    public class ReporteViewModel : BaseViewModel
    {
        public ICommand ObtenerReportes { get; }
        public ObservableCollection<ReportesObservable> ReporteBinding;
        public ObservableCollection<ReportesSumVentasObservable> ReporteMostrarBinding;
       
       
        public ReporteViewModel(IAuthenticator autenticador, IReportesServices reportesServices)
        {
            Fecha = DateTime.Now.ToString("yyyy/MM/dd");
            FechaInicio = DateTime.Now.ToString();
            FechaFin = Convert.ToDateTime(DateTime.Now).AddDays(-1).ToString();
            SoloTotales = true;
            ObtenerReportes = new GetReportesCommand(this, autenticador, reportesServices);
            RPTSumaVentas = Visibility.Hidden;
            RPTTicketGanadores = Visibility.Hidden;
            
        }

        #region PropertyOfView
        //###########################################################
        private string _fecha;
        private string _reporte;
        private int _loteriaID;
        private string _nombrereporte;
        private string _nombrebanca;
        private string _fechareporte;
        private string _fechaActualReport;
        private int _reporteID;
        // Reportes Ventas Por Fecha
        private string _fechainicio;
        private string _fechaFin;
        private bool _soloTotales;
        /////////////////////////////
        private int totalresultado;
        private int totalcomision;
        private int totalsaco;
        private int totalbalance;
        public string _nombreloteria;

        //////////// Reportes a mostrar /////////
        private Visibility _rptsumaventas;
        private Visibility _rptticketGanadores;
        ////////////////////////////////////////
        private ObservableCollection<ReportesSumVentasObservable> _informacionesreportes;
        private ObservableCollection<ReportesObservable> _reportes;
        private DataGrid GridAgrupado;
        private ObservableCollection<ReportesGanadoresObservable> _reporteganadoresbinding;

        //###########################################################

        public string Fecha
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

        public string FechaInicio
        {
            get { return _fechainicio; }
            set { _fechainicio = value; NotifyPropertyChanged(nameof(FechaInicio)); }
        }

        public string FechaFin
        {
            get { return _fechaFin; }
            set { _fechaFin = value; NotifyPropertyChanged(nameof(FechaFin)); }
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

        public string NombreLoteria
        {
            get { return _nombreloteria; }
            set { _nombreloteria = value; NotifyPropertyChanged(nameof(NombreLoteria)); }
        }

        public Visibility RPTSumaVentas
        {
            get { return _rptsumaventas; }
            set { _rptsumaventas = value; NotifyPropertyChanged(nameof(RPTSumaVentas)); }
        }

        public Visibility RPTTicketGanadores
        {
            get { return _rptticketGanadores; }
            set { _rptticketGanadores = value; NotifyPropertyChanged(nameof(RPTTicketGanadores)); }
        }

        public string FechaActualReport
        {
            get { return _fechaActualReport; }
            set { _fechaActualReport = value; NotifyPropertyChanged(nameof(FechaActualReport)); }
        }

        public ObservableCollection<ReportesGanadoresObservable> ReportesGanadores
        {
            get { return _reporteganadoresbinding; }
            set { _reporteganadoresbinding = value; NotifyPropertyChanged(nameof(ReportesGanadores)); }
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
                return _reportes= new ObservableCollection<ReportesObservable> {
                 new ReportesObservable  {Nombre="Reportes de Ventas",ReporteID=1,IsChecked=true},
                 new ReportesObservable  {Nombre="Lista de Numeros",ReporteID=2,IsChecked=false},
                 new ReportesObservable  {Nombre="Lista de Tickets",ReporteID=3,IsChecked=false},
                 new ReportesObservable  {Nombre="Reportes Ganadores",ReporteID=4,IsChecked=false},
                 new ReportesObservable  {Nombre="Lista de Tarjetas",ReporteID=5,IsChecked=false},
                 new ReportesObservable  {Nombre="Suma De Ventas",ReporteID=6,IsChecked=false},
                 new ReportesObservable  {Nombre="Ventas por Fecha",ReporteID=7,IsChecked=false},
                 new ReportesObservable  {Nombre="Pagos Remotos",ReporteID=8,IsChecked=false},
                 new ReportesObservable  {Nombre="Lista de Premios",ReporteID=9,IsChecked=false}
                };
            }
            set
            {
                _reportes = value;
                NotifyPropertyChanged(nameof(Reporte));
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

        public string NombreBanca {
            get { return _nombrebanca; }
            set { _nombrebanca = value; NotifyPropertyChanged(nameof(NombreBanca));}
        }

        public string FechaReporte
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


        public int TotalResultado
        {
            get { return totalresultado; }
            set { totalresultado = value; NotifyPropertyChanged(nameof(TotalResultado)); }
        }

        public int TotalComision
        {
            get { return totalcomision; }
            set { totalcomision = value; NotifyPropertyChanged(nameof(TotalComision)); }
        }
        public int TotalSaco
        {
            get { return totalsaco; }
            set { totalsaco = value; NotifyPropertyChanged(nameof(TotalSaco)); }
        }
        public int TotalBalance
        {
            get { return totalbalance; }
            set { totalbalance = value; NotifyPropertyChanged(nameof(TotalBalance)); }
        }


        //###########################################################
        #endregion


    }
}
