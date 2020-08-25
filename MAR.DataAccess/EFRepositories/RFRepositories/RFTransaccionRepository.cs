using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using MAR.AppLogic.MARHelpers;
using Dapper;
using System.Data;

namespace MAR.DataAccess.EFRepositories.RFRepositories
{

    public class RFTransaccionRepository
    {
        public static IEnumerable<RF_Transaccion> GetRFTransaccion(Expression<Func<RF_Transaccion, bool>> filter = null, Func<IQueryable<RF_Transaccion>, IOrderedQueryable<RF_Transaccion>> orderBy = null,
          string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<RF_Transaccion> query = db.RF_Transaccion;

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

        public static string Anula_RfTransaccion(int pReciboId, string pEstado, string pAutorizacionAnulacion)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@ReciboID", pReciboId);
                    p.Add("@Estado", pEstado);
                    p.Add("@AnulacionAutorizacion", pAutorizacionAnulacion);
                    var anulacion = con.Query<bool>("RF_Recibo_Anula", p, commandType: CommandType.StoredProcedure).First();
                    if (anulacion)
                    {
                        return "Ticket anulado correctamente.";
                    }
                    return "El ticket no existe";
                }
            
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static IEnumerable<RF_Transaccion> AgregarRF_Transacciones(IEnumerable<RF_Transaccion> pRfTransacciones)
        {
            var db = new MARContext();
            db.RF_Transaccion.AddRange(pRfTransacciones);
            db.SaveChanges();
            return pRfTransacciones;
        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public RFTransaccionRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
