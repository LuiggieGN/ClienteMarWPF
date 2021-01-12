using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPF.UI.ViewModels.Base;

namespace ClienteMarWPF.UI.ViewModels
{
    public class BancaBalanceViewModel : BaseViewModel
    {
        private decimal _balance;
        private string _strBalance;
        private bool _tieneBalance;

        public decimal Balance
        {
            get => _balance;
            set { _balance = value; NotifyPropertyChanged(nameof(Balance)); }
        }
        public string StrBalance
        {
            get => _strBalance;
            set { _strBalance = value; NotifyPropertyChanged(nameof(StrBalance)); }
        }
        public bool TieneBalance
        {
            get => _tieneBalance;
            set { _tieneBalance = value; NotifyPropertyChanged(nameof(TieneBalance)); }
        }




    }
}
