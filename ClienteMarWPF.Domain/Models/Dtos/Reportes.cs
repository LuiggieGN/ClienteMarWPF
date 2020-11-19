using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class ReportesSumaVentas
    {
        public string Reglon { get; set; }
        public int Resultado { get; set; }
        public int Comision { get; set; }
        public int Saco { get; set; }

        public int Balance { get; set; }
    }
}
