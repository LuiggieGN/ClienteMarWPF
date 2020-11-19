using ClienteMarWPF.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
        private string renglon;
        private int balance;
        

        public string Renglon
        {
            get { return renglon; }
            set { renglon = value; NotifyPropertyChanged(nameof(Renglon)); }
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
}
