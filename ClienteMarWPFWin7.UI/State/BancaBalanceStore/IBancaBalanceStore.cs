using System; 
using ClienteMarWPFWin7.UI.ViewModels;

namespace ClienteMarWPFWin7.UI.State.BancaBalanceStore
{
    public interface IBancaBalanceStore
    {
        BancaBalanceViewModel CurrentBancaBalance { get; set; }
        event Action StateChanged;
    }
}
