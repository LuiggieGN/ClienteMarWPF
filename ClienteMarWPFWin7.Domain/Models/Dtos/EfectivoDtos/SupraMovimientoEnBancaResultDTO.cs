using System;
using System.Globalization;

namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class SupraMovimientoEnBancaResultDTO
    {
        public int? MovimientoId { get; set; }
        public int? IngresoID { get; set; }
        public int? EgresoID { get; set; } 
        public DateTime FechaRegistro { get; set; }
        public bool FueProcesado { get; set; } 
        public string MensajeError { get; set; }
        public string Referencia { get; set; }

        public string FechaRegistro_dd_MMM_yyyy_hh_mm_tt
        {
            get
            {
                return FechaRegistro.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));
            }
        }

    }
}
