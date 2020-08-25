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

namespace MAR.DataAccess.EFRepositories
{

    public class ReportesRepository
    {
        public static bool EstaCerrandoDia(int pBancaId)
        {
            //me quede verificando si esta cerrando el dia
            //string sqlQuery = "SELECT TOP (1) eb.EsquemaPagoID FROM RF_SorteoDia sd JOIN RF_EsquemaPagoPremio ep ON sd.SorteoID = ep.SorteoID JOIN RF_EsquemaPagoBanca eb ON ep.EsquemaPagoID = eb.EsquemaPagoID " +
            //                 " WHERE eb.FechaActivo <= GETDATE()  AND sd.SorteoDiaID = " + pSorteoDiaId + " and eb.BancaID = " + pBancaId + " ORDER BY FechaActivo DESC";
            //return DataAccess.Repositories.SqlDataAccess.GetDataTable(sqlQuery, null).Rows[0][0];
            return true;
        }

        public static List<Ganadores> GetGanadores(int pBancaId, DateTime pFecha)
        {
            try
            {
                DateTime fecha = pFecha.Date;
                string sqlQuery = "SELECT * FROM VTicketsRFGanadores where BancaID = " + 32 + " AND CONVERT(date, TicFecha) = '" + fecha + "'";
                using (var con = DALHelper.GetSqlConnection())
                {
                    var ganadoresRF = con.Query<VTicketsGanadore>(sqlQuery).Distinct().ToList();
                    List<Ganadores> ganadores = new List<Ganadores>();
                    foreach (var item in ganadoresRF)
                    {
                        ganadores.Add(new Ganadores { Loteria = item.LotNombre, Saco = item.Saco, Premio = item.PremioQ1, Ticket = item.TicNumero, TicPagado = item.TicPagado });
                    }
                    return ganadores;
                }
            }
            catch (Exception e)
            {

                return null;
            }

        }
        public class Ganadores
        {
            public string Loteria { get; set; }
            public string Premio { get; set; }
            public string Ticket { get; set; }
            public decimal? Saco { get; set; }
            public bool TicPagado { get; set; }
        }


        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public ReportesRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
