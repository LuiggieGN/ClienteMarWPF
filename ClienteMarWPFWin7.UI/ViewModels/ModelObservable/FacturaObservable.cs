using ClienteMarWPFWin7.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPFWin7.UI.ViewModels.ModelObservable
{
    public class FacturaObservable : BaseViewModel
    {

            private int _facturaID;
            private string _fechaFactura;
            private string _fechaVence;
            private double _total;
            private bool _seleccion;

            public int FacturaID
            {
                get { return _facturaID; }
                set { _facturaID = value; NotifyPropertyChanged(nameof(FacturaID));}
            }

            public string FechaFactura
            {
                get { return _fechaFactura; }
                set { _fechaFactura = value; NotifyPropertyChanged(nameof(FechaFactura)); }
            }

            public string FechaVence
            {
                get { return _fechaVence; }
                set { _fechaVence = value; NotifyPropertyChanged(nameof(FechaVence)); }
            }

            public double Total
            {
                get { return _total; }
                set { _total = value; NotifyPropertyChanged(nameof(Total)); }
            }

            public bool Seleccion
            {
                get { return _seleccion; }
                set { _seleccion = value; NotifyPropertyChanged(nameof(Seleccion)); }
            }


    }
}
