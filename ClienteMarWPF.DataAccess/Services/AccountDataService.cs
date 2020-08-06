

using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.Domain.Services.AccountService; 

namespace ClienteMarWPF.DataAccess.Services
{
    public class AccountDataService : IAccountService
    {
        public Task<CuentaUsuario> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CuentaUsuario>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<CuentaUsuario> GetByUserName(string username)
        {
            throw new NotImplementedException();
        }


        public Task<CuentaUsuario> Create(CuentaUsuario entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(CuentaUsuario entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(CuentaUsuario entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CuentaUsuario>> AddRange(List<CuentaUsuario> entities)
        {
            throw new NotImplementedException();
        }


    }
}
