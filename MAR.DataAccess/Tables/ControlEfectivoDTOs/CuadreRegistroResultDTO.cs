
using System;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{   
    public class CuadreRegistroResultDTO
    {
        public bool FueProcesado { get; set; }
        public string CuadreRef { get; set; }
        public int? CuadreID { get; set; }
        public decimal MontoReal { get; set; }
        public decimal? BalanceAnterior { get; set; }
        public int? CuadreAnteriorID { get; set; }
        public string Error { get; set; }
        public DateTime Fecha { get; set; }
        public decimal NuevoBalance { get; set; }

    }
}
