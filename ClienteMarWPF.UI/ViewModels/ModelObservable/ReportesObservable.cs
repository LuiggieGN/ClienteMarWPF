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

    public class ReportesMostrarObservable : BaseViewModel
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
}
