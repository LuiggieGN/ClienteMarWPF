using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.ResponseModels
{
    public class CuadreResponse
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
