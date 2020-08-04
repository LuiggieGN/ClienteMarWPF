
using System;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;



namespace ClienteMarWPF.Domain.Services.AuthenticationService
{
    public interface IAuthenticationService
    {
        Task<CuentaUsuario> Login(string username, string password);
    }
}
