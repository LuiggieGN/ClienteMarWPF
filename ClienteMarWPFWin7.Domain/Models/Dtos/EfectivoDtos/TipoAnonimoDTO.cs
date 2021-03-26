using System;

namespace ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos
{
    public class TipoAnonimoDTO
    {
        public int Clave { get; set; }
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string TipoNombre { get; set; }
        public string Descripcion { get; set; }
        public int? LogicaKey { get; set; }
        public bool EsTipoSistema { get; set; }
        public bool EsTipoAnonimo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
        public int Orden { get; set; }

    }//fin de clase
}
