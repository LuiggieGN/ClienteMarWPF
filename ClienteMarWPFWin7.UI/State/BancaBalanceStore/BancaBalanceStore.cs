using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.ViewModels;
using System;
 
namespace ClienteMarWPFWin7.UI.State.BancaBalanceStore
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
