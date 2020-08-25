using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Flujo.Entities.WebClient.POCO;

namespace Flujo.Entities.WebClient.RequestModels
{
    public class MovimientoRequestModel
    {
        public int CategoriaDesde { get; set; }
        public int CategoriaHasta { get; set; }
        public string JsonCajaDesde { get; set; }
        public string JsonCajaHasta { get; set; }
        public string strMonto { get; set; }

        public Caja CajaDesde { get; set; }
        public Caja CajaHasta { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }

    }
}
