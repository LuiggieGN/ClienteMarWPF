using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.UI.ViewModels;
using System;
 
namespace ClienteMarWPF.UI.State.BancaBalanceStore
{
    public class BancaBalanceStore : IBancaBalanceStore
    {
        private BancaBalanceViewModel _currentBancaBalance;   
        public BancaBalanceViewModel CurrentBancaBalance
        { 
            get => _currentBancaBalance; 
            set {
                _currentBancaBalance = value;
                StateChanged?.Invoke();
            }
        }

        public event Action StateChanged;
    }
}
