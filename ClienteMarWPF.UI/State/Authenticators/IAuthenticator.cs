using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.UI.ViewModels;

namespace ClienteMarWPF.UI.State.Authenticators
{
    public interface IAuthenticator
    {
        CuentaDTO CurrentAccount { get; }

        BancaConfiguracionDTO BancaConfiguracion { get; set; }

        BancaBalanceViewModel BancaBalance { get; set; }

        bool IsLoggedIn { get; set; }

        event Action CurrentAccountStateChanged;

        event Action CurrentBancaConfiguracionStateChanged;

        event Action CurrentBancaBalanceStateChanged;

        event Action IsLoggedInStateChanged;

        void IniciarSesion(string usuario, string clave, int bancaid, string ipaddress);

        void RefrescarBancaBalance();

        void CerrarSesion();



    }
}
