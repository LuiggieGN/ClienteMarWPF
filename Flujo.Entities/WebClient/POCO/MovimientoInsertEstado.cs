using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.POCO
{
    public class MovimientoInsertEstado
    {
        public bool MovimientoFueProcesado { get; set; }
        public int? MovimientoCajaOrigen { get; set; }
        public int? MovimientoCajaDestino { get; set; }
        public string MensajeError { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
