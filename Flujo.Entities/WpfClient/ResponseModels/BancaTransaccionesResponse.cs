using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.ResponseModels
{
    public class BancaTransaccionesResponse
    {
        public int     LogicaKey { get; set; }
        public int     TipoMovimientoSistemaID { get; set; }
        public string  IngresoOEgreso { get; set; }
        public string  Tipo { get; set; }
        public string  TipoNombre { get; set; }
        public decimal MontoAcumulado { get; set; }
        public bool    EsTipoSistema { get; set; }
    }
}
