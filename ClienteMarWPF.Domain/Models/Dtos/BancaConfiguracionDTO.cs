
using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class BancaConfiguracionDTO : Data
    {
        public int BancaId { get; set; }
        public int? BancaCajaId { get; set; }
        public BancaControlEfectivoConfigDTO BancaControlEfectivoConfig { get; set; }
         
    }


    public class BancaControlEfectivoConfigDTO 
    {
        public bool ControlEfectivoEstaActivo { get; set; } /// = False => Control Efectivo Inactivo      || True  =>  Control Efectivo Permitido
        public bool BancaInicioFlujoEfectivo  { get; set; } /// = False => No ha iniciado flujo efectivo  || True  =>  Ha iniciado flujo efectivo

    }

}
 