using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flujo.Entities.WebClient.ViewModels
{
    [Serializable()]
    public class TarjetaViewModel
    {                   
        public int      TarjetaID { get; set; }
        public int      UsuarioID { get; set; }
        public string   Clave { get; set; }
        public string   Propietario { get; set; }
        public string   TipoDocumento { get; set; }
        public string   NoDocumento { get; set; }
        public DateTime FechaCreacion { get; set; }

        public string FechaEnFormato
        {
            get
            {
                return FechaCreacion.ToString(" dd MMM, yyyy  hh:mm tt", CultureInfo.CreateSpecificCulture("es"));
            }
        }
    }
}
