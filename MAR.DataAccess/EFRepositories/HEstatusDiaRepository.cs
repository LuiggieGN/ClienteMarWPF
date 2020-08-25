using Dapper;
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.Tables.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories
{
    public class HEstatusDiaRepository
    {
        public static IEnumerable<HEstatusDia> GetHEstatusDiaConfig(Expression<Func<HEstatusDia, bool>> filter = null, Func<IQueryable<HEstatusDia>, IOrderedQueryable<HEstatusDia>> orderBy = null,
         string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<HEstatusDia> query = db.HEstatusDias;

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

        public static IEnumerable<Premio> GetPremios(DateTime pFecha)
        {
            using (var con = DALHelper.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@Fecha", pFecha);
                string query = $@"    SELECT  t.LotNombre Sorteo , Premios =  (PremioQ1 + '-' + PremioQ2 + '-' + PremioQ3) FROM HEstatusDias
                                            h join tloterias t on h.LoteriaID = t.LoteriaID  where EDiFecha = @Fecha
                                            UNION
                                            SELECT  t.Nombre Sorteo , Premios =  h.Premios FROM RF_SorteoDia
                                            h join RF_Sorteo t on h.SorteoID = t.SorteoID  where h.Dia = @Fecha";

                var premios = con.Query<Premio>(query, p , commandType:CommandType.Text);
                return premios;
            }



        }
        public static void EntraPremiosSuperPale(int pLoteriaId, DateTime pFecha)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@Fecha", pFecha);
                    p.Add("@LoteriaId", pLoteriaId);
                    con.Query("EntraPrmiosSuperPale", p, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception e)
            {
                string test = e.Message;
            }
            
        }
        public static void ReAbrirDiaActul(int pLoteriaID)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@LoteriaID", pLoteriaID);
                    string query = $@"delete from HEstatusDias where LoteriaID = @LoteriaID AND EDiFecha > CONVERT(date,getdate())
                                UPDATE HEstatusDias
                                   SET EDiEntradaPremiosFecha = NULL
                                      ,EDiPremiosDentro = 0
                                      ,EDiCierreDiaFecha = NULL
                                      ,EDiDiaCerrado = 0
                                      ,PremioQ1 = ''
                                      ,PremioQ2 = ''
                                      ,PremioQ3 = ''
                                 WHERE LoteriaID = @LoteriaID  AND EDiFecha = CONVERT(date,getdate())";

                    con.Query(query, p, commandType: CommandType.Text);

                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                throw;
            }
        }

        public class Premio
        {
            public string Sorteo { get; set; }
            public string Premios { get; set; }
        }
    }
}
