
using System;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;



namespace ClienteMarWPF.Domain.Services.AccountService
{
    public interface IAccountService : IServiceBase<CuentaUsuario,int>
    {
        Task<CuentaUsuario> GetByUserName(string username);
    }
}
