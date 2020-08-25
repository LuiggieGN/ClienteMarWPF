using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.RequestModels
{
    public class ConsultaCajaRequestModel
    {
        public int    TipoCajaID { get; set; }
        public int?   CajaIDExcepcionDesde { get; set; }
        public int?   CajaIDExcepcionHasta { get; set; }
        public string Propietario { get; set; }
    }
}
