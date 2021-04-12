
using System;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class MarOperacionDTO  
    {
        public string Tipo { get; set; }
        public string TipoMovimiento { get; set; }
        public int KeyMovimiento { get; set; }
        public int BancaID { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaMovimiento { get; set; }
    }
}
