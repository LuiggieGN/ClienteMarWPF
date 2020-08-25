using System;
using System.Collections.Generic;

namespace Flujo.Entities.WebClient.RequestModels
{
    public class ConsultaBancaRequestModel
    {
        public string    Search { get; set; }
        public List<int> BancasAExclurirEnConsulta { get; set; }
    }
}
