
using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class BancaConfiguracion : Data
    {
        public int BancaId { get; set; }
        public int? BancaCajaId { get; set; }
        public BancaControlEfectivoConfig BancaControlEfectivoConfig { get; set; }
         
    }


    public class BancaControlEfectivoConfig 
    {
        public bool ControlEfectivoEstaActivo { get; set; } /// = False => Control Efectivo Inactivo      || True  =>  Control Efectivo Permitido
        public bool BancaInicioFlujoEfectivo  { get; set; } /// = False => No ha iniciado flujo efectivo  || True  =>  Ha iniciado flujo efectivo

    }

}
 