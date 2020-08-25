using Dapper;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories.LotoDom
{
    public class AnulacionRepository
    {

        public static AnulacionRequestValues GetAnulacionRequestDBValues(string pTicketNumero)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"select distinct t.TransaccionID AS CodigoOperacionReferencia, TicketID = d.TicketID,
                                               t.Autorizacion as AutenticacionReferencia  , t.Monto AS MontoOperacion 
                                               from DTickets d 
                                               join TransaccionClienteHttp t on t.Referencia = d.TicNumero  where 
                                                TicNumero = @TicketNumero and d.TicNulo = 1  "
                                             ;

                    var p = new DynamicParameters();
                    p.Add("@TicketNumero", pTicketNumero);
                    var resultsAnulcion = con.QueryFirst<AnulacionRequestValues>(query, p, commandType: CommandType.Text);
                    con.Close();
                    return resultsAnulcion;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;
            }
        }


        public class AnulacionRequestValues
        {
            public int TicketID { get; set; }
            public decimal MontoOperacion { get; set; }
            public string AutenticacionReferencia { get; set; }
            public string CodigoOperacionReferencia { get; set; }
        }




    }
}
