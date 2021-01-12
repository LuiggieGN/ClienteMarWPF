using System; 
using ClienteMarWPF.UI.ViewModels;

namespace ClienteMarWPF.UI.State.BancaBalanceStore
{
    public interface IBancaBalanceStore
    {
        BancaBalanceViewModel CurrentBancaBalance { get; set; }
        event Action StateChanged;
    }
}
