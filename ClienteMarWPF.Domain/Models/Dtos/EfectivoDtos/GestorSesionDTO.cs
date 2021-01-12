
namespace ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos
{
    public class GestorSesionDTO
    {
       public MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> Gestor { get; set; }
       public RutaAsignacionDTO Asignacion { get; set; }
    }
}
