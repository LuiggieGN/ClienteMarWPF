
using System;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;



namespace ClienteMarWPF.Domain.Services.CuentaUsuarioService
{
    public interface ICuentaUsuarioService : IServiceBase<CuentaUsuario,int>
    {
        Task<CuentaUsuario> ConsultaCuentaUsuarioPorNombreUsuario(string username);
    }
}
