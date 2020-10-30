
using System;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;



namespace ClienteMarWPF.Domain.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
         CuentaDTO Login(string usuario, string clave, int bancaid, string ipaddress);
    }
}
