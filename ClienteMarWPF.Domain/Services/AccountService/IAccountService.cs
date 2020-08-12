
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ClienteMarWPF.Domain.Models.Base;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;



namespace ClienteMarWPF.Domain.Services.AccountService
{
    public interface IAccountService : IServiceBase<CuentaUsuario,int>
    {
        static List<CuentaUsuario> sampleDb;
        Task<CuentaUsuario> GetByUserName(string username);
    }
}
