using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo.Entities.WebClient.POCO
{
    [Serializable()]
    public class BancaRuta : ICloneable
    {
        public int       Posicion             { get; set; }
        public string  BancaNombre   { get; set; }
        public string  BancaDireccion { get; set; }
        public string  BancaTelefono  { get; set; }
        public bool     BancaSeleccionada { get; set; } = false;
        public bool     Desabilitada     { get; set; } = false;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
