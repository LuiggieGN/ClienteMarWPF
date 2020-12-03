using ClienteMarWPF.UI.ViewModels.Base;
using MarPuntoVentaServiceReference;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace ClienteMarWPF.UI.ViewModels.ModelObservable
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
        private int  comision;
        private string  concepto;
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

    public class ReportesSumVentasFechaObservable : BaseViewModel
    {

        public string fecha;
        private string saco;
        private string resultado;
        private string comision;
        private string concepto;
        private string balance;

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

        public string Resultado
        {
            get { return resultado; }
            set { resultado = value; NotifyPropertyChanged(nameof(Resultado)); }
        }

        public string Comision
        {
            get { return comision; }
            set { comision = value; NotifyPropertyChanged(nameof(Comision)); }
        }
        public string Saco
        {
            get { return saco; }
            set { saco = value; NotifyPropertyChanged(nameof(Saco)); }
        }
        public string Balance
        {
            get { return balance; }
            set { balance = value; NotifyPropertyChanged(nameof(Balance)); }
        }

    }

    public class ReportesGanadoresObservable : BaseViewModel
    {

        private string _fecha;
        private double _monto;
        private string _categoria;
        private string _Tickets;


        public string Fecha
        {
            get { return _fecha; }
            set { _fecha = value; NotifyPropertyChanged(nameof(Fecha)); }
        }

        public double Monto
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

    public class ReporteListNumeroColumns : BaseViewModel
    {
        private ObservableCollection<ReportesListaNumerosObservable> quiniela;
        private ObservableCollection<ReportesListaNumerosObservable> pale;
        private ObservableCollection<ReportesListaNumerosObservable> tripleta;

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
    }

    public class ReportesListaNumerosObservable : BaseViewModel
    {
        private string _numerosColumn1;
        private string _numerosColumn2;
        private string _numerosColumn3;
        private string _cantidadColumn1;
        private string _cantidadColumn2;
        private string _cantidadColumn3;
        private string _tipoJugada;


        public string CantidadColumn1
        {
            get { return _cantidadColumn1; }
            set { _cantidadColumn1 = value; NotifyPropertyChanged(nameof(CantidadColumn1)); }
        }

        public string CantidadColumn2
        {
            get { return _cantidadColumn2; }
            set { _cantidadColumn2 = value; NotifyPropertyChanged(nameof(CantidadColumn2)); }
        }
        public string CantidadColumn3
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
        public string TipoJugada
        {
            get { return _tipoJugada; }
            set { _tipoJugada = value; NotifyPropertyChanged(nameof(TipoJugada)); }
        }
    }

    public class ReportesDeVentas:BaseViewModel
    {
        private int numeros;
        private string numerosRD;
        private string pales;
        private string tripletas;
        private string totalVenta;
        private string comision;
        private string ventaNeta;
        private Visibility premiosdisponibles;
        private Visibility ticketnulosdisponibles;
        private List<string> premios;
        private List<string> ticketsNulos;

        public int Numeros
        {
            get { return numeros; }
            set { numeros = value; NotifyPropertyChanged(nameof(Numeros)); }
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
    }

    public class ReporteListaTicketsObservable : BaseViewModel
    {
        string ticket;
        string hora;
        string vendio;
        string saco;
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
        public string Vendio
        {
            get { return vendio; }
            set { vendio = value; NotifyPropertyChanged(nameof(Vendio)); }
        }
        public string Saco
        {
            get { return saco; }
            set { saco = value; NotifyPropertyChanged(nameof(Saco)); }
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
