using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.UnitOfWork
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        //Metodos para obtener
        TEntity Get(int id);
        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        //Metodos para agregar
        void Add(TEntity entity);
        void AddRange(IEnumerable<TEntity> entities);

        //Metodos para anular
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);

    }
    
}
