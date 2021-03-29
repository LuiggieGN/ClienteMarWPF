
using ClienteMarWPFWin7.Domain.Models.Base;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;


namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class CuentaDTO 
    {
        public UsuarioDTO UsuarioDTO { get; set; }
        public MAR_Setting2 MAR_Setting2 { get; set; }
    }
}
 