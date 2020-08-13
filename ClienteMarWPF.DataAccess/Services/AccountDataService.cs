 

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.Domain.Services.AccountService; 

namespace ClienteMarWPF.DataAccess.Services
{
    public class AccountDataService : IAccountService
    {
        public static List<CuentaUsuario> sampleDb = new List<CuentaUsuario>()
        {
           new CuentaUsuario(){ Id = 1, UsuarioHolder = new Usuario { Id = 1 , UserName = "10", Password = "10", PasswordHash="sdfafe" } }
        };

        public Task<CuentaUsuario> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CuentaUsuario>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<CuentaUsuario> GetByUserName(string username)
        {
            /// Flujo.Service Desde aqui se va acceder al Asmx
            /// Que el asmx este disponible para consumir desde esta capa 

            CuentaUsuario cuenta = await Task.Run(() =>
            {
                return sampleDb.Where(x => x.UsuarioHolder.UserName == username).FirstOrDefault();
            });


            return cuenta;
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

        public Task<bool> AddRange(List<CuentaUsuario> entities)
        {
            throw new NotImplementedException();
        }


    }
}
