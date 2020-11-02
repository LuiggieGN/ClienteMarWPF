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
        CuentaDTO CurrentAccount { get; }

        BancaConfiguracionDTO BancaConfiguracion { get; }

        bool IsLoggedIn { get; }

        event Action CurrentAccountStateChanged;

        event Action CurrentBancaConfiguracionStateChanged;

        void IniciarSesion(string usuario, string clave, int bancaid, string ipaddress);

        void CerrarSesion();



    }
}
