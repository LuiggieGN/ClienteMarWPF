
using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class BancaConfiguracionDTO  
    {
        public BancaDTO BancaDto {  get; set; }
        public CajaDTO CajaEfectivoDto { get; set; }
        public ControlEfectivoDTO ControlEfectivoConfigDto { get; set; }

    }


    public class ControlEfectivoDTO  
    {
        public bool PuedeUsarControlEfectivo { get; set; } /// = False => No puede |Control Efectivo|    
                                                           ///   True  => Si puede |Control Efectivo|
        public bool BancaYaInicioControlEfectivo { get; set; } /// = False => No ha iniciado flujo efectivo  
                                                               ///   True  =>  Ha iniciado flujo efectivo

    }

}
