using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.POCO
{
    public class Banca
    {
        public int    BancaID { get; set; }
        public string BanNombre { get; set; }
        public string BanContacto { get; set; }
        public string BanDireccion { get; set; }
        public string BanTelefono { get; set; }
    }
}
