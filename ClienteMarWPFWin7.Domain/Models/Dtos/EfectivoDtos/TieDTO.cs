
using System.Collections.Generic;

namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class TieDTO
    {
        public List<TipoAnonimoDTO> TiposIngresosQueSonAnonimo { get; set; }
        public List<TipoAnonimoDTO> TiposEgresosQueSonAnonimo { get; set; }
    }
}
