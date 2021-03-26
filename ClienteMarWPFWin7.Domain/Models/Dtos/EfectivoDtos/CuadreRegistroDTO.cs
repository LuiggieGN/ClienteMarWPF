
using ClienteMarWPFWin7.Domain.Enums;
 

namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class CuadreRegistroDTO
    {
        public CuadreDTO Cuadre { get; set; }
        public CuadreGestorAccion CuadreGestorAccion { get; set; }
        public RutaAsignacionDTO RutaAsignacion { get; set; } 

    }
}
