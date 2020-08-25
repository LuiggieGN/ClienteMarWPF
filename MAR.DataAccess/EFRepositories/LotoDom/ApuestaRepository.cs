using Dapper;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories.LotoDom
{
    public class ApuestaRepository
    {

        public static TicketModel GetDTicket(string pTicketId)
        {
            try
            {
                string where = "";
                if (pTicketId.Contains("-"))
                {
                    where = " t.TicNumero = @TicketID ";
                }
                else
                {
                    where = " t.TicketID = @TicketID  ";
                }
                //(d.TDeNumero = @TicketID OR t.TicketID = @TicketID) AND hs.Activo = 1
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT 
                                NoTicket = T.TicNumero
                                ,TicketID = t.TicketID
                                ,SorteoMontoAcumulado = Sum(sma.Monto)
							    ,tc.Autorizacion
								,tc.TransaccionID
                            
                                ,BancaID = T.BancaID
                                ,LoteriaID = T.LoteriaID
                                ,AutenticacionReferencia = tc.Autorizacion
                                ,CodigoOperacionReferencia = tc.TransaccionID
                                ,tc.NautCalculado
                        
                                 ,MontoOperacion = t.TicCosto
                                 ,Fecha  = TicFecha
								   ,DetalleID = d.TicketDetalleID
                              
								 ,Saco = d.TDePago
                                 ,Monto = d.TDeCosto 
                                 ,Jugada = d.TDeNumero
                                  FROM DTickets t
                                  inner join DTicketDetalle d on t.TicketID = d.TicketID
                                  left  join SorteoMontoAcumulado sma on sma.LoteriaID = 100 and convert(date,GetDate()) = convert(date,sma.Fecha)
								  LEFT join TransaccionClienteHttp tc on tc.Referencia = t.TicNumero  and tc.Activo = 1
                                    where  " + where + $@"
                                  group by TicketDetalleID, t.TicketID, TicFecha, TicNumero,   TDePago,TDeCosto, TicCosto, TDeNumero, tc.Autorizacion,tc.TransaccionID,t.BancaID,t.LoteriaID ,tc.NautCalculado
								    "
                                  ;

                    var p = new DynamicParameters();
                    p.Add("@TicketID", pTicketId);
                    p.Add("@Where", where);
                    List<TicketDetalle> detallesList = new List<TicketDetalle>();

                    var ticket = con.Query<TicketModel, TicketDetalle, TicketModel>(
                         query,
                         (tic, detalle) =>
                         {
                             TicketModel ticEntry;

                             ticEntry = tic;
                             ticEntry.TicketDetalles = new List<TicketDetalle>();
                             detallesList.Add(detalle);
                             return ticEntry;
                         },
                         p, splitOn: "NoTicket,DetalleID", commandType: CommandType.Text)
                     .Distinct().GroupBy(x => x.NoTicket).Select(x => x.FirstOrDefault()).FirstOrDefault();
                    var detallesClean = detallesList.Distinct().GroupBy(x => x.DetalleID).Select(x => x.FirstOrDefault());
                    ticket.TicketDetalles.AddRange(detallesClean);
                    con.Close();
                    return ticket;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;
            }
        }

        public static void AgregaAcumuladoLotoDom(double pMontoAcumulado, int pLoteriaId)
        {
          
                try
                {
                    string sqlQuery = $@"INSERT INTO SorteoMontoAcumulado
                                       (LoteriaID
                                       ,Monto
                                       ,Fecha)
                                 VALUES
                                       (100
                                       ,@Monto
                                       ,GetDate())";
                    var paramers = new List<System.Data.SqlClient.SqlParameter>();
                    SqlParameter acumuladoParam = new SqlParameter("@Monto", SqlDbType.Decimal);
                    acumuladoParam.Value = (object)pMontoAcumulado ?? DBNull.Value;
                    paramers.Add(acumuladoParam);
                    DALHelper.PostDataTable(sqlQuery, paramers, CommandType.Text);
                }
                catch (Exception e)
                {
                string t = e.Message;
                }
        }
     
        public class TicketModel
        {
            public string NoTicket { get; set; }
            public double? SorteoMontoAcumulado { get; set; }
            public int TicketID { get; set; }
            public string NautCalculado { get; set; }
            public DateTime Fecha { get; set; }
          
            public int BancaID { get; set; }
            public int LoteriaID { get; set; }
            public string AutenticacionReferencia { get; set; }
            public string CodigoOperacionReferencia { get; set; }
            public decimal MontoOperacion { get; set; }
            public List<TicketDetalle> TicketDetalles { get; set; }
        }
        public class TicketDetalle
        {
            public int TicketID { get; set; }
            public int DetalleID { get; set; }
            public int Monto { get; set; }
            public decimal Saco { get; set; }
            public string Jugada { get; set; }
        }
    }

}
