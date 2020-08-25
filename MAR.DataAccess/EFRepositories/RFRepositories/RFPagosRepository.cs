using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using System.Data;
using MAR.AppLogic.MARHelpers;
using Dapper;
using System.Data.SqlClient;
using static MAR.DataAccess.ViewModels.Pega4ViewModel;

namespace MAR.DataAccess.EFRepositories.RFRepositories
{
    public class RFPagosRepository
    {
        public static List<PagoConsultado> ConsultarPago(int pReciboID)
        {
            using (var con = DALHelper.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@ReciboID", pReciboID);
                var consultaPago = con.Query<PagoConsultado>("RF_ConsultaPago", p, commandType: CommandType.StoredProcedure).Distinct().ToList();
                return consultaPago;
            }
        }

        public static List<PagoRealizado> RealizaPago(int pReciboID, int pBancaID)
        {
            using (var con = DALHelper.GetSqlConnection())
            {
                try
                {
                    var p = new DynamicParameters();
                    p.Add("@ReciboID", pReciboID);
                    p.Add("@BancaID",pBancaID);


                    var consultaPago = con.Query<PagoRealizado>("RF_AplicaPago", p, commandType: CommandType.StoredProcedure).Distinct().ToList();
                    return consultaPago;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

        public class PagoRealizado
        {
            public string ReferenciaTrans { get; set; }
        }



    }
}
