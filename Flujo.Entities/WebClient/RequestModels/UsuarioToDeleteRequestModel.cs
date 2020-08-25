using System;
using System.Linq;
using System.Collections.Generic; 
using System.Text;

namespace Flujo.Entities.WebClient.RequestModels
{
    public class UsuarioToDeleteRequestModel
    {
        public int UsuarioQueElimina { get; set; }
        public int UsuarioAEliminar { get; set; }
        public bool TienePermisoDeEliminar { get; set; }
        public decimal Codigo { get; set; }

        public bool EsValidoElCodigoEnviado
        {
            get
            {
                string strcodigo = this.Codigo.ToString().Replace(",", "").Replace(".", "");

                string strValid = (
                                        (UsuarioQueElimina + UsuarioAEliminar) * -1 * 3.14

                                  ).ToString().Replace(",", "").Replace(".", "");


                bool isvalid = strcodigo.Equals(strValid);


                return isvalid;
            }
        }
    }
}
