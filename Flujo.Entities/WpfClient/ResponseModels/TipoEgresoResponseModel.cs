using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.Entities.WpfClient.ResponseModels
{
    public class TipoEgresoResponseModel
    {
        public int             Id                     { get; set; }
        public string        Nombre           { get; set; }
        public string        Descripcion     { get; set; }
        public int?           LogicaKey        { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool          Activo               { get; set; }
    }
}
