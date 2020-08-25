using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;

namespace MAR.DataAccess.EFRepositories
{
    public class VpSuplidores
    {
        public static IEnumerable<VP_Suplidor> GetSuplidores(Expression<Func<VP_Suplidor, bool>> filter = null, Func<IQueryable<VP_Suplidor>, IOrderedQueryable<VP_Suplidor>> orderBy = null,
        string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<VP_Suplidor> query = db.VP_Suplidor;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }


        public static IQueryable<SWebProductoConfig> GetSuplidorProductoConfiguration(string pSuplidorNombre)
        {
            var db = new MARContext();
            var configuration = (from sup in db.PMSuplidores
                                 join pro in db.SWebProductoes on sup.SuplidorID equals pro.SuplidorID
                                 join con in db.SWebProductoConfigs on pro.WebProductoID equals con.WebProductoID
                                 where sup.SupNombre == pSuplidorNombre
                                 select con);
            return configuration;
        }

        public static VP_Suplidor BuscarSuplidoresPorId(int pSuplidorId)
        {
            var db = new MARContext();
            var suplidor = db.VP_Suplidor.FirstOrDefault(x => x.SuplidorID == pSuplidorId);
            return suplidor;
        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public VpSuplidores()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
