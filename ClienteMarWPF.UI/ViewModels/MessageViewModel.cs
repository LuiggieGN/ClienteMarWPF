using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPF.UI.ViewModels.Base;

namespace ClienteMarWPF.UI.ViewModels
{
    public class MessageViewModel : BaseViewModel
    {
        private string _message;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                NotifyPropertyChanged(nameof(Message), nameof(HasMessage));
            }
        }

        public bool HasMessage => !string.IsNullOrEmpty(Message);
    }
}
