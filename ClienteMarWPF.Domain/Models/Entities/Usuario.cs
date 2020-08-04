
using ClienteMarWPF.Domain.Models.Base;

namespace ClienteMarWPF.Domain.Models.Entities
{
    public class Usuario : Data
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }

    }
}
