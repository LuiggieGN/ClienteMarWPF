

using ClienteMarWPF.UI.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace ClienteMarWPF.UI.Modules.RegistrarPC
{
    public class RegistrarPCWindowViewModel : BaseViewModel
    {

        RegistrarPCControlViewModel _control;


        public RegistrarPCControlViewModel Control
        {
            get => _control;
            set 
            {
                _control = value;
                NotifyPropertyChanged(nameof(Control));
            }
        }



        public RegistrarPCWindowViewModel(RegistrarPCControlViewModel control)
        {
            _control = control;
        }










    }
}
