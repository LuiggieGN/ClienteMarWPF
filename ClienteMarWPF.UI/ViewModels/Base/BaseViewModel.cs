using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ClienteMarWPF.UI.ViewModels.Helpers;

namespace ClienteMarWPF.UI.ViewModels.Base
{

    public delegate TViewModel CreateViewModel<TViewModel>() where TViewModel : BaseViewModel;

    public class BaseViewModel : INotifyPropertyChanged
    {
        public bool No { get => Booleano.No; }
        public bool Si { get => Booleano.Si; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(params string[] propertyNames)
        {
            if (PropertyChanged != null)
            {
                foreach (string propertyName in propertyNames)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }
} 
