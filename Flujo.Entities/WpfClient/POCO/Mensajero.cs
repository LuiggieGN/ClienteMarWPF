using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.POCO
{
    public class Mensajero
    {
        public int UsuarioID  { get; set; }
        public int CajaID { get; set; }
        public string Usuario { get; set; }
        public string Password   { get; set; }
    }

}
