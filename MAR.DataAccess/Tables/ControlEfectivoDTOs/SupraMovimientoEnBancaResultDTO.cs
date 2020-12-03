using System;


namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
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

    }
}
