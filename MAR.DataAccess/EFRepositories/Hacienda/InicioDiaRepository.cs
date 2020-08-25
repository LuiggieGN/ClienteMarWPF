using Dapper;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories.Hacienda
{
    public class InicioDiaRepository
    {
        //Este metodo valida si hacienda ha iniciado el dia anteriormente
        public static bool ValidaInicioDia()
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"select top 1 TransaccionID from TransaccionClienteHttp where  TipoTransaccionID  = 8 and  convert(date,Fecha) = convert(date,GetDate()) and Activo = 1";

                    var result = con.Query<string>(query, commandType: CommandType.Text).ToList();
                    if (result.Any())
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return false;
            }
        }

    }
}

