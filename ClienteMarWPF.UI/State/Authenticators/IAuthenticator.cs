using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;


namespace ClienteMarWPF.UI.State.Authenticators
{
    public interface IAuthenticator
    {
        CuentaUsuario CurrentAccount { get; }

        BancaConfiguracion BancaConfiguracion { get; }

        bool IsLoggedIn { get; }

        event Action CurrentAccountStateChanged;

        event Action CurrentBancaConfiguracionStateChanged;

        Task Login(string username, string password);

        void Logout();



    }
}
