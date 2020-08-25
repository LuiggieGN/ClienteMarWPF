using System;
using System.Collections.Generic;

namespace Flujo.Entities.WebClient.POCO
{
    public class FormularioRuta
    {
        public ConsultaUsuarioBalance MensajeroAsignado { get; set; }
        public string                                MensajeroAsignadoJson { get; set; }

        public List<BancaRuta>            Bancas { get; set; }
        public string                               BancasEnRutasJson { get; set; }

        public string                               NombreRuta { get; set; }
        public string                               Comentario { get; set; }
    }
}
