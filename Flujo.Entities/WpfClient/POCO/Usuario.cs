using System;
 

namespace Flujo.Entities.WpfClient.POCO
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public int TipoUsuarioID { get; set; }
        public int TipoDocumentoID { get; set; }
        public string Documento { get; set; }
        public int ZonaID { get; set; }
        public bool Activo { get; set; }
        public int? ToquenFallidos { get; set; }
        public int? LoginFallidos { get; set; }
    }

    public class MUsuario
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
        public string Email { get; set; }
        public int? TipoUsuarioID { get; set; }
        public int? LoginFallidos { get; set; }
        public int? ToquenFallidos { get; set; }
        public string UsuPin { get; set; }
        public int? UsuPuedeCuadrar { get; set; }
        public int TipoAutenticacion { get; set; }

    }
}
