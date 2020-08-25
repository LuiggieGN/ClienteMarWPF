using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.RequestModels
{
    [Serializable()]
    public class ConsultaMovimientoAdministrativoRequest
    {
        public int concepto { get; set; }
        public int referencia { get; set; }
        public int? referenciaSoloUna { get; set; }
        public int periodo { get; set; }
        public string periodoClientRangeStart { get; set; }
        public string periodoClientRangeEnd { get; set; }
    }
}