
using System.Collections.Generic;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class TieDTO
    {
        public List<TipoAnonimoDTO> TiposIngresosQueSonAnonimo { get; set; }
        public List<TipoAnonimoDTO> TiposEgresosQueSonAnonimo { get; set; }
    }
}
