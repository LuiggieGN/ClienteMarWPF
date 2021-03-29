using System;

using System.Threading.Tasks;

using System.Collections.Generic;

using ClienteMarWPFWin7.Domain.Models.Base;

namespace ClienteMarWPFWin7.Domain.Services
{   
    public interface IServiceBase<T, TId> where T : IDataKeyBase<TId> 
    { 
        Task<T> Get(TId id);
        Task<IEnumerable<T>> GetAll();
        Task<bool> AddRange(List<T> entities);
        Task<T> Create(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Update(T entity); 
    }
}
