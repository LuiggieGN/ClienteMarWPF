using System;
 
namespace Flujo.Entities.WpfClient.ResponseModels
{
    public class TokenDeSeguridadResponseModel
    {
        public int       TarjetaID { get; set; }
        public int       Posicion  { get; set; }
        public string    Toquen    { get; set; }
    }

    public class UsuarioTarjetaClave
    {
        public int TarjetaID { get; set; }
        public int UsuarioID { get; set; }
        public string Serial { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Comentario { get; set; }
        public string Tokens { get; set; }
        public bool Activo { get; set; }
    }


}
 