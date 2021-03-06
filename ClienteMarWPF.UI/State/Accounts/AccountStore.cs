﻿using System;
using System.Collections.Generic;
using System.Text;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.UI.State.Accounts
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
