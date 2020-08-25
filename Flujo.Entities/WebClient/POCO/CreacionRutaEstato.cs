using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo.Entities.WebClient.POCO
{
    public class CreacionRutaEstato
    {
        public bool RutaFueProcesada { get; set; }
        public List<string> Errores { get; set; }
    }
}
