using System;
using System.Collections.Generic;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class TarjetaDTO
    {
        public int TarjetaID { get; set; }
        public int UsuarioID { get; set; }
        public string Serial { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public string JsonTokens { get; set; }
        public bool Activo { get; set; }
        public List<List<string>> Tokens { get; set; } = null;
        public List<string> InlineTokens { get; set; } = null;

    }
}
