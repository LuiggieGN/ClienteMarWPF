
using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class CuentaUsuario : Data
    {
        public Usuario UsuarioHolder { get; set; }
    }
}
 