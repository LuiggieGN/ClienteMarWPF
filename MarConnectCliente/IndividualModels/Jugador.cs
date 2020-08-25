using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MarConnectCliente.IndividualModels

{
    public class Jugador
    {
        public string TipoDocumento { get; set; }
        public string NumeroDocumento { get; set; }

        public override string ToString()
        {
            return new {
                NumeroDocumento= NumeroDocumento ?? "",
                TipoDocumento =TipoDocumento??""
            }.ToString();
        }
    }
}