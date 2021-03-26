
using System;
using System.Threading.Tasks;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;



namespace ClienteMarWPFWin7.Domain.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
         CuentaDTO Logon2(string usuario, string clave, int bancaid, string ipaddress);
    }
}
