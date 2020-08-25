using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WebClient.RequestModels
{
    [Serializable()]
    public class ArqueoConsultaRequest
    {
        public string TipoArqueo { get; set; }
        public string Banca { get; set; }
        public string Rifero { get; set; }
        public string CajeraResponsable { get; set; }
        public string PeriodoTipo { get; set; }
        public string DInicio { get; set; }
        public string DFin { get; set; }
    }
}
