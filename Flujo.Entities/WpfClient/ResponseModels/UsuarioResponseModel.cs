using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.ResponseModels
{
    public class UsuarioResponseModel
    {
        public int       UsuarioID { get; set; }
        public string  Nombre { get; set; }
        public int       TipoUsuarioID { get; set; }
        public int       TipoDocumentoID { get; set; }
        public string  Documento  { get; set; }
        public int       ZonaID { get; set; }
        public bool    Activo { get; set; }
        public int      ToquenFallidos { get; set; }
        public int      LoginFallidos { get; set; }
    }
}
