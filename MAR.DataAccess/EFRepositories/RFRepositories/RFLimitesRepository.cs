using Dapper;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MAR.DataAccess.Tables.Enums.DbEnums;

namespace MAR.DataAccess.EFRepositories.RFRepositories
{
    public class RFLimitesRepository
    {


        public static LimitesResult ValidaLimites(int pSorteoDiaId, decimal pMontoFactura) // Valida si puede hacer la venta y no excede el limite
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@SorteoDiaID", pSorteoDiaId);
                    p.Add("@MontoAfacturar", pMontoFactura);
                    var limite = con.QueryFirst<LimitesResult>("LimitesRF_Valida", p, commandType: CommandType.StoredProcedure);
                    return limite;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }


        public static DataAccess.ViewModels.BaseViewModel.SqlDataResult GrabarLimites(int pTipoLimite, DataTable pLimitesDt)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@Tipo", pTipoLimite);
                    p.Add("@LimitesGlobal", pLimitesDt.AsTableValuedParameter());
                    con.Query("LimitesRF_Agrega", p, commandType: CommandType.StoredProcedure);
                }
                return new DataAccess.ViewModels.BaseViewModel.SqlDataResult { OK = true, Result = "OK" };
            }
            catch (Exception e)
            {
                return new DataAccess.ViewModels.BaseViewModel.SqlDataResult { OK = false, Result = e.Message };
            }
        }


        public static decimal GetLimiteRF(string pSorteoID, LimitesRF pTipoLimite)
        {
            using (var con = DALHelper.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@SorteoID", pSorteoID);
                p.Add("@Tipo", pTipoLimite);
                decimal limite = con.QueryFirst<decimal>("LimitesRF_Get", p, commandType: CommandType.StoredProcedure);
                return Math.Round(limite);
            }

        }


        public class LimitesResult
        {
            public bool Valido { get; set; }
            public decimal PuedeVender { get; set; }
            public string Sorteo { get; set; }
            public string TipoLimite { get; set; }
        }

       





        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public RFLimitesRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}

