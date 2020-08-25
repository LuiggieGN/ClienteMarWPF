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
    public class TransaccionReferenciaRepository
    {
        public static bool AgregaTransaccion(int pTransaccionId, MarConnectCliente.Enums.MetodosEnum.MetodoServicio pReferenciaKey, string pDetalle)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"INSERT INTO TransaccionReferencia   (ReferenciaKey, TransaccionID , Fecha, ReferenciaValue, Detalle)
                       VALUES
                         (@ReferenciaKey, @TransaccionID, GetDate(),  @ReferenciaValue, @Detalle)";
                    var p = new DynamicParameters();
                    p.Add("@TransaccionID", pTransaccionId.ToString());
                    p.Add("@ReferenciaValue", "Modelo");
                    p.Add("@ReferenciaKey", pReferenciaKey);
                    p.Add("@Detalle", pDetalle);
                    con.Query(query, p, commandType: CommandType.Text);
                    return true;
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
