using ClienteMarWPFWin7.UI.ViewModels.Base;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Packaging;
using System.Text;
using System.Windows;

namespace ClienteMarWPFWin7.UI.ViewModels.ModelObservable
{
   public class ReportesObservable:BaseViewModel
    {

        private int reporteId;
        private string nombre;
        private bool ischecked;

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; NotifyPropertyChanged(nameof(Nombre)); }
        }

        public int ReporteID
        {
            get { return reporteId; }
            set { reporteId = value; NotifyPropertyChanged(nameof(ReporteID)); }
        }

        public bool IsChecked
        {
            get { return ischecked; }
            set { ischecked = value; NotifyPropertyChanged(nameof(IsChecked)); }
        }
    }

    public class ReportesSumVentasObservable : BaseViewModel
    {

        private int saco;
        private int resultado;
        private int comision;
        private string concepto;
        private int balance;
        

        public string Concepto
        {
            get { return concepto; }
            set { concepto = value; NotifyPropertyChanged(nameof(Concepto)); }
        }

        public int Resultado
        {
            get { return resultado; }
            set { resultado = value; NotifyPropertyChanged(nameof(Resultado)); }
        }

        public int Comision
        {
            get { return comision; }
            set { comision = value; NotifyPropertyChanged(nameof(Comision)); }
        }
        public int Saco
        {
            get { return saco; }
            set { saco = value; NotifyPropertyChanged(nameof(Saco)); }
        }
        public int Balance
        {
            get { return balance; }
            set { balance = value; NotifyPropertyChanged(nameof(Balance)); }
        }

    }

    public class ReporteListaPremiosObservable : BaseViewModel
    {

        private string listaPremios;
       
        public string ListadoDePremios
        {
            get { return listaPremios; }
            set { listaPremios = value; NotifyPropertyChanged(nameof(ListadoDePremios)); }
        }

    }

    public class ReportesSumVentasFechaObservable : BaseViewModel
    {

        public string fecha;
        private int saco;
        private int resultado;
        private int comision;
        private string concepto;
        private int balance;

        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; NotifyPropertyChanged(nameof(Fecha)); }
        }

        public string Concepto
        {
            get { return concepto; }
            set { concepto = value; NotifyPropertyChanged(nameof(Concepto)); }
        }

        public int Resultado
        {
            get { return resultado; }
            set { resultado = value; NotifyPropertyChanged(nameof(Resultado)); }
        }

        public int Comision
        {
            get { return comision; }
            set { comision = value; NotifyPropertyChanged(nameof(Comision)); }
        }
        public int Saco
        {
            get { return saco; }
            set { saco = value; NotifyPropertyChanged(nameof(Saco)); }
        }
        public int Balance
        {
            get { return balance; }
            set { balance = value; NotifyPropertyChanged(nameof(Balance)); }
        }

    }

    public class ReportesGanadoresObservable : BaseViewModel
    {

        private string _fecha;
        private int _monto;
        private string _categoria;
        private string _Tickets;


        public string Fecha
        {
            get { return _fecha; }
            set { _fecha = value; NotifyPropertyChanged(nameof(Fecha)); }
        }

        public int Monto
        {
            get { return _monto; }
            set { _monto = value; NotifyPropertyChanged(nameof(Monto)); }
        }
        public string Categoria
        {
            get { return _categoria; }
            set { _categoria = value; NotifyPropertyChanged(nameof(Categoria)); }
        }
        public string Tickets
        {
            get { return _Tickets; }
            set { _Tickets = value; NotifyPropertyChanged(nameof(Tickets)); }
        }

    }

   

    public class EstadoDeTicketGanadores: BaseViewModel
    {
        private ObservableCollection<ReportesGanadoresObservable> pendientePagar;
        private ObservableCollection<ReportesGanadoresObservable> sinReclamar;
        private ObservableCollection<ReportesGanadoresObservable> pagados;
        private Visibility pendientePagarVisibility;
        private Visibility sinReclamarVisibility;
        private Visibility pagadosVisibility;
        private Visibility mostrarPremiosVisibility;
        private Visibility noMostrarPremiosVisibility;
        private Visibility mostrarNoTicketGanadores;
        private string totalganadores;
        private string primera;
        private string segunda;
        private string tercera;
        private int posicionTituloPagados;
        private int posicionTituloPendientesPagar;
        private int posicionTituloSinReclamar;
        private int posicionTablaPagados;
        private int posicionTablaPendientesPagar;
        private int posicionTablaSinReclamar;
        private int posicionBalance;
        private int heightMensajeNoTicketGanadores;

        public ObservableCollection<ReportesGanadoresObservable> PendientesPagar
        {
            get { return pendientePagar; }
            set { pendientePagar = value; NotifyPropertyChanged(nameof(PendientesPagar)); }
        }
        public ObservableCollection<ReportesGanadoresObservable> SinReclamar
        {
            get { return sinReclamar; }
            set { sinReclamar = value; NotifyPropertyChanged(nameof(SinReclamar)); }
        }

        public ObservableCollection<ReportesGanadoresObservable> Pagados
        {
            get { return pagados; }
            set { pagados = value; NotifyPropertyChanged(nameof(Pagados)); }
        }
        public Visibility PendientePagarVisibility
        {
            get { return pendientePagarVisibility; }
            set { pendientePagarVisibility = value;NotifyPropertyChanged(nameof(PendientePagarVisibility)); }
        }
        public Visibility PagadosVisibility
        {
            get { return pagadosVisibility; }
            set { pagadosVisibility = value; NotifyPropertyChanged(nameof(PagadosVisibility)); }
        }
        public Visibility SinReclamarVisibility
        {
            get { return sinReclamarVisibility; }
            set {sinReclamarVisibility = value; NotifyPropertyChanged(nameof(SinReclamarVisibility)); }
        }
        public Visibility MostrarPremiosVisibity
        {
            get { return mostrarPremiosVisibility; }
            set { mostrarPremiosVisibility = value; NotifyPropertyChanged(nameof(MostrarPremiosVisibity)); }
        }
        public Visibility NoMostrarPremiosVisibity
        {
            get { return noMostrarPremiosVisibility; }
            set { noMostrarPremiosVisibility = value; NotifyPropertyChanged(nameof(NoMostrarPremiosVisibity)); }
        }
        public Visibility MostrarNoHayGanadoresVisibity
        {
            get { return mostrarNoTicketGanadores; }
            set { mostrarNoTicketGanadores = value; NotifyPropertyChanged(nameof(MostrarNoHayGanadoresVisibity)); }
        }
        public string TotalGanadores
        {
            get { return totalganadores; }
            set { totalganadores = value;NotifyPropertyChanged(nameof(TotalGanadores)); }
        }
        public string Primera
        {
            get { return primera; }
            set { primera = value; NotifyPropertyChanged(nameof(Primera)); }
        }
        public string Segunda
        {
            get { return segunda; }
            set { segunda = value; NotifyPropertyChanged(nameof(Segunda)); }
        }
        public string Tercera
        {
            get { return tercera; }
            set { tercera = value; NotifyPropertyChanged(nameof(Tercera)); }
        }
        public int PosicionTituloPagados
        {
            get { return posicionTituloPagados; }
            set { posicionTituloPagados = value; NotifyPropertyChanged(nameof(PosicionTituloPagados)); }
        }
        public int PosicionTituloPendientesPagos
        {
            get { return posicionTituloPendientesPagar; }
            set { posicionTituloPendientesPagar = value; NotifyPropertyChanged(nameof(PosicionTituloPendientesPagos)); }
        }
        public int PosicionTituloSinReclamar
        {
            get { return posicionTituloSinReclamar; }
            set { posicionTituloSinReclamar = value; NotifyPropertyChanged(nameof(PosicionTituloSinReclamar)); }
        }
        public int PosicionTablaPagados
        {
            get { return posicionTablaPagados; }
            set { posicionTablaPagados = value; NotifyPropertyChanged(nameof(PosicionTablaPagados)); }
        }
        public int PosicionTablaPendientesPagos
        {
            get { return posicionTablaPendientesPagar; }
            set { posicionTablaPendientesPagar = value; NotifyPropertyChanged(nameof(PosicionTablaPendientesPagos)); }
        }
        public int PosicionTablaSinReclamar
        {
            get { return posicionTablaSinReclamar; }
            set { posicionTablaSinReclamar = value; NotifyPropertyChanged(nameof(PosicionTablaSinReclamar)); }
        }
        public int PosicionBalance
        {
            get { return posicionBalance; }
            set { posicionBalance = value; NotifyPropertyChanged(nameof(PosicionBalance)); }
        }
        public int HeightTicketNoGanadores
        {
            get { return heightMensajeNoTicketGanadores; }
            set { heightMensajeNoTicketGanadores = value; NotifyPropertyChanged(nameof(HeightTicketNoGanadores)); }
        }
    }

    public class ReportesListaTajetasObservable : BaseViewModel
    {

        private string _suplidor;
        private string _hora;
        private string _precio;
        private string _serie;


        public string Suplidor
        {
            get { return _suplidor; }
            set { _suplidor = value; NotifyPropertyChanged(nameof(Suplidor)); }
        }

        public string Hora
        {
            get { return _hora; }
            set { _hora = value; NotifyPropertyChanged(nameof(Hora)); }
        }
        public string Precio
        {
            get { return _precio; }
            set { _precio = value; NotifyPropertyChanged(nameof(Precio)); }
        }
        public string Serie
        {
            get { return _serie; }
            set { _serie = value; NotifyPropertyChanged(nameof(Serie)); }
        }

    }

    public class ModelSerializePremios : BaseViewModel
    {

        private List<string[]> _printData;
        private string _ok;
        private string _err;
        

        public List<string[]> PrintData
        {
            get { return _printData; }
            set { _printData = value; NotifyPropertyChanged(nameof(PrintData)); }
        }

        public string Ok
        {
            get { return _ok; }
            set { _ok = value; NotifyPropertyChanged(nameof(Ok)); }
        }
        public string Err
        {
            get { return _err; }
            set { _err = value; NotifyPropertyChanged(nameof(Err)); }
        }

    }

    

  
    public class ReporteListNumeroColumns : BaseViewModel
    {
        private ObservableCollection<ReportesListaNumerosObservable> quiniela;
        private ObservableCollection<ReportesListaNumerosObservable> pale;
        private ObservableCollection<ReportesListaNumerosObservable> tripleta;
        private string totalCantidadQuiniela;
        private string totalPagoQuiniela;
        private string totalCantidadPale;
        private string totalPagoPale;
        private string totalCantidadTripleta;
        private string totalPagoTripleta;
        private Visibility quinielaVisibility;
        private Visibility paleVisibility;
        private Visibility tripletaVisibility;
        private int posiciontituloquiniela;
        private int posiciontitulopale;
        private int posiciontitulotripleta;
        private int posiciontablaquiniela;
        private int posiciontablapale;
        private int posiciontablatripleta;
        private int posiciontotalesquiniela;
        private int posiciontotalespale;
        private int posiciontotalestripletas;

        public ObservableCollection<ReportesListaNumerosObservable> Quiniela
        {
            get { return quiniela; }
            set { quiniela = value;NotifyPropertyChanged(nameof(Quiniela)); }
        }

        public ObservableCollection<ReportesListaNumerosObservable> Pale
        {
            get { return pale; }
            set { pale = value; NotifyPropertyChanged(nameof(Pale)); }
        }

        public ObservableCollection<ReportesListaNumerosObservable> Tripleta
        {
            get { return tripleta; }
            set { tripleta = value; NotifyPropertyChanged(nameof(Tripleta)); }
        }
        public Visibility QuinielaVisibilty
        {
            get { return quinielaVisibility; }
            set { quinielaVisibility = value; NotifyPropertyChanged(nameof(QuinielaVisibilty)); }
        }

        public Visibility PaleVisibility
        {
            get { return paleVisibility; }
            set { paleVisibility = value; NotifyPropertyChanged(nameof(PaleVisibility)); }
        }

        public Visibility TripletaVisibility
        {
            get { return tripletaVisibility; }
            set { tripletaVisibility = value; NotifyPropertyChanged(nameof(TripletaVisibility)); }
        }

        public string TotalCantidaQuiniela 
        {
            get { return totalCantidadQuiniela; }
            set { totalCantidadQuiniela = value; NotifyPropertyChanged(nameof(TotalCantidaQuiniela)); }
        }
        public string TotalPagoQuiniela
        {
            get { return totalPagoQuiniela; }
            set { totalPagoQuiniela = value; NotifyPropertyChanged(nameof(TotalPagoQuiniela)); }
        }
        public string TotalCantidadPale
        {
            get { return totalCantidadPale; }
            set { totalCantidadPale = value; NotifyPropertyChanged(nameof(TotalCantidadPale)); }
        }
        public string TotalPagoPale
        {
            get { return totalPagoPale; }
            set { totalPagoPale = value; NotifyPropertyChanged(nameof(TotalPagoPale)); }
        }
        public string TotalCantidadTripleta
        {
            get { return totalCantidadTripleta; }
            set { totalCantidadTripleta = value; NotifyPropertyChanged(nameof(TotalCantidadTripleta)); }
        }
        public string TotalPagoTripleta
        {
            get { return totalPagoTripleta; }
            set { totalPagoTripleta = value; NotifyPropertyChanged(nameof(TotalPagoTripleta)); }
        }
        public int PosicionTituloQuiniela
        {
            get { return posiciontituloquiniela; }
            set { posiciontituloquiniela = value; NotifyPropertyChanged(nameof(PosicionTituloQuiniela)); }
        }
        public int PosicionTituloPale
        {
            get { return posiciontitulopale; }
            set { posiciontitulopale = value; NotifyPropertyChanged(nameof(PosicionTituloPale)); }
        }
        public int PosicionTituloTripleta
        {
            get { return posiciontitulotripleta; }
            set { posiciontitulotripleta = value; NotifyPropertyChanged(nameof(PosicionTituloTripleta)); }
        }
        public int PosicionTablaQuiniela
        {
            get { return posiciontablaquiniela; }
            set { posiciontablaquiniela = value; NotifyPropertyChanged(nameof(PosicionTablaQuiniela)); }
        }
        public int PosicionTablaPale
        {
            get { return posiciontablapale; }
            set { posiciontablapale = value; NotifyPropertyChanged(nameof(PosicionTablaPale)); }
        }
        public int PosicionTablaTripleta
        {
            get { return posiciontablatripleta; }
            set { posiciontablatripleta = value; NotifyPropertyChanged(nameof(PosicionTablaTripleta)); }
        }
        public int PosicionTotalesQuiniela
        {
            get { return posiciontotalesquiniela; }
            set { posiciontotalesquiniela = value; NotifyPropertyChanged(nameof(PosicionTotalesQuiniela)); }
        }
        public int PosicionTotalesPale
        {
            get { return posiciontotalespale; }
            set { posiciontotalespale = value; NotifyPropertyChanged(nameof(PosicionTotalesPale)); }
        }
        public int PosicionTotalesTripleta
        {
            get { return posiciontotalestripletas; }
            set { posiciontotalestripletas = value; NotifyPropertyChanged(nameof(PosicionTotalesTripleta)); }
        }
    }

    public class ReportesListaNumerosObservable : BaseViewModel
    {
        private string _numerosColumn1;
        private string _numerosColumn2;
        private string _numerosColumn3;
        private int _cantidadColumn1;
        private int _cantidadColumn2;
        private int _cantidadColumn3;
        

        public int CantidadColumn1
        {
            get { return _cantidadColumn1; }
            set { _cantidadColumn1 = value; NotifyPropertyChanged(nameof(CantidadColumn1)); }
        }

        public int CantidadColumn2
        {
            get { return _cantidadColumn2; }
            set { _cantidadColumn2 = value; NotifyPropertyChanged(nameof(CantidadColumn2)); }
        }
        public int CantidadColumn3
        {
            get { return _cantidadColumn3; }
            set { _cantidadColumn3 = value; NotifyPropertyChanged(nameof(CantidadColumn3)); }
        }
        public string NumeroColumn1
        {
            get { return _numerosColumn1; }
            set { _numerosColumn1 = value; NotifyPropertyChanged(nameof(NumeroColumn1)); }
        }

        public string NumeroColumn2
        {
            get { return _numerosColumn2; }
            set { _numerosColumn2 = value; NotifyPropertyChanged(nameof(NumeroColumn2)); }
        }
        public string NumeroColumn3
        {
            get { return _numerosColumn3; }
            set { _numerosColumn3 = value; NotifyPropertyChanged(nameof(NumeroColumn3)); }
        }
        
    }

    public class PremiosVentas : BaseViewModel
    {
        private string _1ra;
        private string _2da;
        private string _3ra;
        private string C1ra;
        private string C2da;
        private string C3ra;
        private string M1ra;
        private string M2da;
        private string M3ra;
        private string totalNumerosPremiados;
        private string totalPalesPremiados;
        private string totalTripletaPremiados;
        private string totalPremiados;
        private string ganancia;
        private Visibility mostrarPremios;
        private Visibility noMostrarPremios;


        public string Primera
        {
            get { return _1ra; }
            set { _1ra = value; NotifyPropertyChanged(nameof(Primera)); }
        }

        public string Segunda
        {
            get { return _2da; }
            set { _2da = value; NotifyPropertyChanged(nameof(Segunda)); }
        }
        public string Tercera
        {
            get { return _3ra; }
            set { _3ra = value; NotifyPropertyChanged(nameof(Tercera)); }
        }
        public string Cantidad1RA
        {
            get { return C1ra; }
            set { C1ra = value; NotifyPropertyChanged(nameof(Cantidad1RA)); }
        }

        public string Cantidad2DA
        {
            get { return C2da; }
            set { C2da = value; NotifyPropertyChanged(nameof(Cantidad2DA)); }
        }
        public string Cantidad3RA
        {
            get { return C3ra; }
            set { C3ra = value; NotifyPropertyChanged(nameof(Cantidad3RA)); }
        }

        public string Monto1RA
        {
            get { return M1ra; }
            set { M1ra = value; NotifyPropertyChanged(nameof(Monto1RA)); }
        }

        public string Monto2DA
        {
            get { return M2da; }
            set { M2da = value; NotifyPropertyChanged(nameof(Monto2DA)); }
        }
        public string Monto3RA
        {
            get { return M3ra; }
            set { M3ra = value; NotifyPropertyChanged(nameof(Monto3RA)); }
        }
        public string TotalNumerosPremiados
        {
            get { return totalNumerosPremiados; }
            set { totalNumerosPremiados = value; NotifyPropertyChanged(nameof(TotalNumerosPremiados)); }
        }

        public string TotalPalesPremiados
        {
            get { return totalPalesPremiados; }
            set { totalPalesPremiados = value; NotifyPropertyChanged(nameof(TotalPalesPremiados)); }
        }

        public string TotalTripletaPremiados
        {
            get { return totalTripletaPremiados; }
            set { totalTripletaPremiados = value; NotifyPropertyChanged(nameof(TotalTripletaPremiados)); }
        }
        public string TotalGanancia
        {
            get { return ganancia; }
            set { ganancia = value; NotifyPropertyChanged(nameof(TotalGanancia)); }
        }

        public string TotalPremiados
        {
            get { return totalPremiados; }
            set { totalPremiados = value; NotifyPropertyChanged(nameof(TotalPremiados)); }
        }

        public Visibility MostrarPremios
        {
            get { return mostrarPremios; }
            set { mostrarPremios = value; NotifyPropertyChanged(nameof(MostrarPremios)); }
        }
        public Visibility NoMostrarPremios
        {
            get { return noMostrarPremios; }
            set { noMostrarPremios = value; NotifyPropertyChanged(nameof(NoMostrarPremios)); }
        }

    }

    public class TotalListNumeros : BaseViewModel
    {
        private int _totalQuiniela;
        private int _totalPale;
        private int _totalTripletas;
       

        public int TotalQuiniela
        {
            get { return _totalQuiniela; }
            set { _totalQuiniela = value; NotifyPropertyChanged(nameof(TotalQuiniela)); }
        }

        public int TotalPale
        {
            get { return _totalPale; }
            set { _totalPale = value; NotifyPropertyChanged(nameof(TotalPale)); }
        }
        public int TotalTripleta
        {
            get { return _totalTripletas; }
            set { _totalTripletas = value; NotifyPropertyChanged(nameof(TotalTripleta)); }
        }
       
    }

    public class ReportesDeVentas:BaseViewModel
    {
        private int numeros;
        private int posicionticketnulos;
        private string numerosRD;
        private string pales;
        private string tripletas;
        private string totalVenta;
        private string comision;
        private string ventaNeta;
        private string gananciaoperdida;
        private Visibility premiosdisponibles;
        private Visibility ticketnulosNodisponibles;
        private Visibility ticketnulosdisponibles;
        private List<string> premios;
        private List<string> ticketsNulos;

        public int Numeros
        {
            get { return numeros; }
            set { numeros = value; NotifyPropertyChanged(nameof(Numeros)); }
        }
        public int PosicionTicketNulo
        {
            get { return posicionticketnulos; }
            set { posicionticketnulos = value; NotifyPropertyChanged(nameof(PosicionTicketNulo)); }
        }

        public string NumerosRD
        {
            get { return numerosRD; }
            set { numerosRD = value; NotifyPropertyChanged(nameof(NumerosRD)); }
        }
        public string Pales
        {
            get { return pales; }
            set { pales = value; NotifyPropertyChanged(nameof(Pales)); }
        }
        public string Tripletas
        {
            get { return tripletas; }
            set { tripletas = value; NotifyPropertyChanged(nameof(Tripletas)); }
        }
        public string TotalVentas
        {
            get { return totalVenta; }
            set { totalVenta = value; NotifyPropertyChanged(nameof(TotalVentas)); }
        }
        public string Comision
        {
            get { return comision; }
            set { comision = value; NotifyPropertyChanged(nameof(Comision)); }
        }
        public string GananciaOPerdida
        {
            get { return gananciaoperdida; }
            set { gananciaoperdida = value; NotifyPropertyChanged(nameof(GananciaOPerdida)); }
        }
        public string VentaNeta
        {
            get { return ventaNeta; }
            set { ventaNeta = value; NotifyPropertyChanged(nameof(VentaNeta)); }
        }
        public List<string> Premios
        {
            get { return premios; }
            set { premios = value; NotifyPropertyChanged(nameof(Premios)); }
        }
        public List<string> TicketNulo
        {
            get { return ticketsNulos; }
            set { ticketsNulos = value; NotifyPropertyChanged(nameof(TicketNulo)); }
        }
        public Visibility PremiosDisponibles
        {
            get { return premiosdisponibles; }
            set { premiosdisponibles = value; NotifyPropertyChanged(nameof(PremiosDisponibles)); }
        }
        public Visibility TicketNulosDisponibles
        {
            get { return ticketnulosdisponibles; }
            set { ticketnulosdisponibles = value; NotifyPropertyChanged(nameof(TicketNulosDisponibles)); }
        }
        public Visibility TicketNulosNoDisponibles
        {
            get { return ticketnulosNodisponibles; }
            set { ticketnulosNodisponibles = value; NotifyPropertyChanged(nameof(TicketNulosNoDisponibles)); }
        }
    }

    public class ReporteListaTicketsObservable : BaseViewModel
    {
        string ticket;
        string hora;
        int vendio;
        string saco;
        bool nulo;
        Visibility mostrarNulos;
       
        public string Ticket
        {
            get { return ticket; }
            set { ticket = value; NotifyPropertyChanged(nameof(Ticket)); }
        }
        public string Hora
        {
            get { return hora; }
            set { hora = value; NotifyPropertyChanged(nameof(Hora)); }
        }
        public int Vendio
        {
            get { return vendio; }
            set { vendio = value; NotifyPropertyChanged(nameof(Vendio)); }
        }
        public string Saco
        {
            get { return saco; }
            set { saco = value; NotifyPropertyChanged(nameof(Saco)); }
        }
        public bool Nulo
        {
            get { return nulo; }
            set { nulo = value; NotifyPropertyChanged(nameof(Nulo)); }
        }
        public Visibility MostrarNulos
        {
            get { return mostrarNulos; }
            set { mostrarNulos = value; NotifyPropertyChanged(nameof(MostrarNulos)); }
        }

    }

    public class TotalesListadoTicket:BaseViewModel
    {
        string totalVenta;
        string totalSaco;
        string cantidadValidos;
        string cantidadNulos;


        public string TotalVenta
        {
            get { return totalVenta; }
            set { totalVenta = value; NotifyPropertyChanged(nameof(TotalVenta)); }
        }
        public string TotalSaco
        {
            get { return totalSaco; }
            set { totalSaco = value; NotifyPropertyChanged(nameof(TotalSaco)); }
        }
        public string CantidadValidos
        {
            get { return cantidadValidos; }
            set { cantidadValidos = value; NotifyPropertyChanged(nameof(CantidadValidos)); }
        }
        public string CantidadNulos
        {
            get { return cantidadNulos; }
            set { cantidadNulos = value; NotifyPropertyChanged(nameof(CantidadNulos)); }
        }
    }
}
