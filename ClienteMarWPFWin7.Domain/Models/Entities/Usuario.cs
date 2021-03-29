
using ClienteMarWPFWin7.Domain.Models.Base;

namespace ClienteMarWPFWin7.Domain.Models.Entities
{
    public class Usuario : Data
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }

    }
}
