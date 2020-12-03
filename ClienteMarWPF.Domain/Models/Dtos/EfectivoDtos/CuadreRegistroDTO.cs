
using ClienteMarWPF.Domain.Enums;
 

namespace ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos
{
    public class CuadreRegistroDTO
    {
        public CuadreDTO Cuadre { get; set; }
        public CuadreGestorAccion CuadreGestorAccion { get; set; }
        public RutaAsignacionDTO RutaAsignacion { get; set; } 

    }
}
