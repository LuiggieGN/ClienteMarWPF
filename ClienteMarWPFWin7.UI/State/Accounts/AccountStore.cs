using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;

namespace ClienteMarWPFWin7.UI.State.Accounts
{
    public class AccountStore : IAccountStore
    {
        private CuentaDTO _currentAccount;
            
        public CuentaDTO CurrentAccount { 
            get => _currentAccount;
            set {
                _currentAccount = value;
                StateChanged?.Invoke();
            } 
        }

        public event Action StateChanged;



    }
}
