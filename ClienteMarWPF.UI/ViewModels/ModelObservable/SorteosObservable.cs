using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.ModelObservable
{
    public class SorteosObservable: BaseViewModel
    {
        private int _loteriaID;
        private string _loteria;
        private bool _isSelected;
        private DateTime _date;

        public int LoteriaID
        {
            get { return _loteriaID; }
            set { _loteriaID = value; NotifyPropertyChanged(nameof(LoteriaID)); }
        }
        public string Loteria
        {
            get { return _loteria; }
            set { _loteria = value; NotifyPropertyChanged(nameof(Loteria)); }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged(nameof(IsSelected)); }
        }
        public DateTime Date
        {
            get { return _date; }
            set { _date = value; NotifyPropertyChanged(nameof(Date)); }
        }


    }

    public class SorteosTicketModels : BaseViewModel
    {   
        private string banNombre;
        private string banDireccion;
        private string telefono;
        private string fecha;
        private string loteria;
        private string ticketno;
        private List<LoteriaTicketPin> pin;
        private List<JugadasTicketModels> jugadas;
        private string firma;
        private string textReviseJugada;
        private int costo;
        private int ticket;
        private bool nulo;
        private string hora;
        private int pago;

        private int _loteriaID;
        
        
        
        public string BanNombre
        {
            get { return banNombre; }
            set { banNombre = value; NotifyPropertyChanged(nameof(BanNombre)); }
        }
        public string BanDireccion
        {
            get { return banDireccion; }
            set { banDireccion = value; NotifyPropertyChanged(nameof(BanDireccion)); }
        }
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; NotifyPropertyChanged(nameof(Telefono)); }
        }
        public string Fecha
        {
            get { return fecha; }
            set { fecha = value; NotifyPropertyChanged(nameof(Fecha)); }
        }
         public string Loteria
        {
            get { return loteria; }
            set { loteria = value; NotifyPropertyChanged(nameof(Loteria)); }
        }
        public string TicketNo
        {
            get { return ticketno; }
            set { ticketno = value; NotifyPropertyChanged(nameof(TicketNo)); }
        }
        public List<LoteriaTicketPin> Pin
        {
            get { return pin; }
            set { pin = value; NotifyPropertyChanged(nameof(Pin)); }
        }
        public List<JugadasTicketModels> Jugadas
        {
            get { return jugadas; }
            set { jugadas = value; NotifyPropertyChanged(nameof(Jugadas)); }
        }
        public string Firma
        {
            get { return firma; }
            set { firma = value; NotifyPropertyChanged(nameof(Firma)); }
        }
        public string TextReviseJugada
        {
            get { return textReviseJugada; }
            set { textReviseJugada = value; NotifyPropertyChanged(nameof(TextReviseJugada)); }
        }
        public int LoteriaID
        {
            get { return _loteriaID; }
            set { _loteriaID = value; NotifyPropertyChanged(nameof(LoteriaID)); }
        }
        public int Costo
        {
            get { return costo; }
            set { costo = value; NotifyPropertyChanged(nameof(Costo)); }
        }
        public int Ticket
        {
            get { return ticket; }
            set { ticket = value; NotifyPropertyChanged(nameof(Ticket)); }
        }
        public bool Nulo
        {
            get { return nulo; }
            set { nulo = value; NotifyPropertyChanged(nameof(Nulo)); }
        }
        public string Hora
        {
            get { return hora; }
            set { hora = value; NotifyPropertyChanged(nameof(Hora)); }
        }
        public int Pago
        {
            get { return pago; }
            set { pago = value; NotifyPropertyChanged(nameof(Pago)); }
        }
    }

    public class JugadasTicketModels : BaseViewModel
    {
        private string numero;
        private int costo;
        private string tipoJugada;
       

        public string Numero
        {
            get { return numero; }
            set { numero = value; NotifyPropertyChanged(nameof(Numero)); }
        }
        public int Costo
        {
            get { return costo; }
            set { costo = value; NotifyPropertyChanged(nameof(Costo)); }
        }
        public string TipoJugada
        {
            get { return tipoJugada; }
            set { tipoJugada = value; NotifyPropertyChanged(nameof(TipoJugada)); }
        }
       




    }
}
