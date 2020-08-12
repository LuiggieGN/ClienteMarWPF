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
        
        bool IsLoggedIn { get; }

        event Action StateChanged;

        Task Login(string username, string password);

        void Logout();



    }
}
