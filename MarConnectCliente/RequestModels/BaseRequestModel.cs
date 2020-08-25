using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.RequestModels
{
    public class BaseRequestModel
    {
        public string FechaHoraSolicitud { get; set; }
        public string DiaOperacion { get; set; }
        public string TipoOperacion { get; set; }
        public string CodigoOperacion { get; set; }//referencia
        public string EstablecimientoID { get; set; }//consorio
        public string Usuario { get; set; }
        public string Password { get; set; }
        public Uri ServiceUrl { get; set; }
        public string NumeroAutentificacionCalculado { get; set; }
        public string CurlString { get; set; }
        public Dictionary<string, string> HeardersDictionary { get; set; }
    }
}
