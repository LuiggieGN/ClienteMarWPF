using ClienteMarWPF.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.UI.ViewModels.ModelObservable
{
    public class ProveedorRecargas: BaseViewModel
    {

        private int _operadorID;
        private string _operador;
        private string _pais;
        private string _url { get; set; }
        private bool _isSelected { get; set; }

        public int OperadorID
        {
            get { return _operadorID; }
            set { _operadorID = value; NotifyPropertyChanged(nameof(OperadorID)); }
        }


        public string Operador
        {
            get { return _operador; }
            set { _operador = value; NotifyPropertyChanged(nameof(Operador)); }
        }

        public string Pais
        {
            get { return _pais; }
            set { _pais = value; NotifyPropertyChanged(nameof(Pais)); }
        }

        public string Url
        {
            get { return _url; }
            set { _url = value; NotifyPropertyChanged(nameof(Url)); }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged(nameof(IsSelected)); }
        }


        
    }
}
