using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos; 

namespace ClienteMarWPF.UI.State.Accounts
{
    public interface IGestorStore
    {
        GestorSesionDTO GestorSesion { get; set; }
    }
} 
