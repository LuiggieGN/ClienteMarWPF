﻿using System;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.UI.ViewModels;

namespace ClienteMarWPFWin7.UI.State.Authenticators
{
    public interface IAuthenticator
    {
        CuentaDTO CurrentAccount { get; }

        BancaConfiguracionDTO BancaConfiguracion { get; set; }
        PermisosDTO Permisos { get; set; }

        BancaBalanceViewModel BancaBalance { get; set; }

        bool IsLoggedIn { get; set; }

        event Action CurrentAccountStateChanged;

        event Action CurrentBancaConfiguracionStateChanged;

        event Action CurrentBancaBalanceStateChanged;

        event Action CurrentPermisosStateChanged;

        event Action IsLoggedInStateChanged;

        void IniciarSesion(string usuario, string clave, int bancaid, string ipaddress);

        void RefrescarBancaBalance();

        void CerrarSesion();



    }
}
