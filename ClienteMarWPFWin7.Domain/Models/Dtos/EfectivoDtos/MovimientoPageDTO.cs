using System;
using System.Globalization;

namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class MovimientoPageDTO: PageDTO
    {
        public int? BancaId { get; set; }
        public int CajaId { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
        public string ConsultaDesde => FechaDesde.ToString("yyyyMMdd", new CultureInfo("en-Us"));
        public string ConsultaHasta => FechaHasta.AddDays(1).ToString("yyyyMMdd", new CultureInfo("en-Us"));
        public string CategoriaOperacion { get; set; } = null;
    }
}
