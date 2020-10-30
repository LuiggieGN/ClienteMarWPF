
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
using MarPuntoVentaServiceReference;



namespace ClienteMarWPF.Domain.Services.AccountService
{
    public interface IAccountService 
    {
        CuentaDTO Logon2(string usuario, string clave, int bancaid, string ipaddress); 
    }
}
