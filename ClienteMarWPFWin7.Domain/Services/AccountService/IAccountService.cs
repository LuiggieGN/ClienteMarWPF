
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPFWin7.Domain.Models.Base;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;



namespace ClienteMarWPFWin7.Domain.Services.AccountService
{
    public interface IAccountService 
    {
        CuentaDTO Logon2(string usuario, string clave, int bancaid, string ipaddress); 
    }
}
