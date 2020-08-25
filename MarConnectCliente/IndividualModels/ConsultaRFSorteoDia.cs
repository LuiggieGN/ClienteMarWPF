using System;
using System.Collections.Generic;

namespace MarConnectCliente.IndividualModels
{

    public class RFSorteoDiaConsultaValidacion
    {
        public bool EsValida { get; set; }

        public List<ConsultaRFSorteoDia>  TheConsultaOf_RF_Sorteo_Dia { get; set; }

    }

    public class ConsultaRFSorteoDia
    {
        public int       SorteoDiaID { get; set; }
        public int       SorteoID { get; set; }
        public DateTime  Dia { get; set; }
        public DateTime  HoraInicioVentas { get; set; }
        public DateTime  HoraCierreVentas { get; set; }
        public bool      VentasCerradas { get; set; }
        public string    Premios { get; set; }
        public DateTime? HoraPremios { get; set; }
        public bool      DiaCerrado { get; set; }
        public DateTime? HoraCierreDia { get; set; }
        public string    Estado { get; set; }
        public string    Referencia { get; set; }
        public string    PuedeReportar { get; set; }
    }
}
