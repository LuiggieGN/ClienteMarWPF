using System;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class MovimientoPageDTO: PageDTO
    {
        public int? BancaId { get; set; }
        public int CajaId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string ConsultaDesde { get; set; }
        public string ConsultaHasta { get; set; }
        public string CategoriaOperacion { get; set; }
    }
}
