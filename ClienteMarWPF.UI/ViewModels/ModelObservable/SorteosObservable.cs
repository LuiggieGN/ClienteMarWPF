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



    }
}
