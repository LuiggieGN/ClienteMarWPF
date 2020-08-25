using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;
using MAR.AppLogic.MARHelpers;
using Dapper;

namespace MAR.DataAccess.EFRepositories
{
    public class LoteriaRepository
    {
        public static IEnumerable<TLoteria> GetLoterias(Expression<Func<TLoteria, bool>> filter = null, Func<IQueryable<TLoteria>, IOrderedQueryable<TLoteria>> orderBy = null,
        string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<TLoteria> query = db.TLoterias;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public static List<int> GetLoteriasVentasAbiertas(int[] pLoteriasId)
        {

            string sqlQuery = "SELECT   LoteriaID  FROM HEstatusDias   WHERE EDiCierreVentaFecha > GetDate() AND LoteriaID IN (" + string.Join(",", pLoteriasId) + ") " +
                           " and EDiVentaCerrada = 0 and(EDiVentaIniciada = 1 OR EDiInicioVentaFecha < GetDate())  Group by LoteriaID, EdiFecha ORDER BY EdiFecha DESC";
            using (var con = DALHelper.GetSqlConnection())
            {
                var loteriasIdDisponibles = con.Query<int>(sqlQuery);

                List<int> loteriasId = new List<int>();
                foreach (var item in loteriasIdDisponibles)
                {
                    loteriasId.Add(item);
                }
                return loteriasId;
            }


        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public LoteriaRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
