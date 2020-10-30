
using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Entities;

using MarPuntoVentaServiceReference;


namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class CuentaDTO 
    {
        public UsuarioDTO UsuarioDTO { get; set; }
        public MAR_Setting2 MAR_Setting2 { get; set; }
    }
}
 