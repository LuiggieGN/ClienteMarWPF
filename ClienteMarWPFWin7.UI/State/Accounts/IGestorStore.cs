using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos; 

namespace ClienteMarWPFWin7.UI.State.Accounts
{
    public interface IGestorStore
    {
        GestorSesionDTO GestorSesion { get; set; }
    }
} 
