
using System;

namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
{
    public class MUsuarioDTO
    {
        public int UsuarioID { get; set; }
        public string UsuNombre { get; set; }
        public string UsuApellido { get; set; }
        public string UsuCedula { get; set; }
        public DateTime UsuFechaNac { get; set; }
        public string UsuUserName { get; set; }
        public string UsuClave { get; set; }
        public DateTime UsuVenceClave { get; set; }
        public bool UsuActivo { get; set; }
        public int UsuNivel { get; set; }
        public string UsuComentario { get; set; }
        public string UsuTema { get; set; }
        public string Email { get; set; }
        public int? TipoUsuarioID { get; set; }
        public int? LoginFallidos { get; set; }
        public int? ToquenFallidos { get; set; }
        public string UsuPin { get; set; }
        public int? UsuPuedeCuadrar { get; set; }
        public int TipoAutenticacion { get; set; }
    }
}
