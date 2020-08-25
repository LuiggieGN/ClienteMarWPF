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

namespace MAR.DataAccess.EFRepositories
{
    public class GrupoRepository
    {


        public static object GrupoConectadoConHacienda()
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = "SELECT TOP(1) GrupEnHacienda FROM TGrupos";
                    var result = con.QueryFirst<bool>(query, commandType: CommandType.Text);
                    return result;
                }
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}

