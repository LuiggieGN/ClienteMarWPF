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
        private string _image { get; set; }
        private bool _isSelected { get; set; }
        private bool _isSuper { get; set; }


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
        public string Image
        {
            get { return _image; }
            set { _image = value; NotifyPropertyChanged(nameof(Image)); }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged(nameof(IsSelected)); }
        }
        public bool IsSuper
        {
            get { return _isSuper; }
            set { _isSuper = value; NotifyPropertyChanged(nameof(IsSelected)); }
        }


    }
}
