
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

    public class PermisosDTO
    {
        public bool PuedeVenderRecargas { get; set; }   
        public bool PuedeAnular { get; set; }
        public bool Servicios { get; set; }
        public bool PagoTicket { get; set; }
        public bool ReimprimirTicket { get; set; }
        public bool ImprimirTicketRegarga { get; set; }
        public bool PuedeVentasLocales { get; set; }
        public bool PuedeImprimirReportes { get; set; }
        public bool PuedePagarRemoto { get; set; }
        public int BancaID { get; set; }
        public bool CincoMinutos { get; set; }
        public bool MedirInactividad { get; set; }
        public int MinutosIncatividad { get; set; }
        public bool PuedeVenderBingo { get; set; }
    }

}
