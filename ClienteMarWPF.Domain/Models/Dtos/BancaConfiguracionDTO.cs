
using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class BancaConfiguracionDTO : Data
    {
        public int BancaId { get; set; }
        public int? BancaCajaId { get; set; }
        public ControlEfectivoDTO ControlEfectivoConfig { get; set; }

    }


    public class ControlEfectivoDTO
    {
        public bool PuedeUsarControlEfectivo { get; set; } /// = False => No puede |Control Efectivo|    
                                                           ///   True  => Si puede |Control Efectivo|
        public bool BancaYaInicioControlEfectivo { get; set; } /// = False => No ha iniciado flujo efectivo  
                                                               ///   True  =>  Ha iniciado flujo efectivo

    }

}
