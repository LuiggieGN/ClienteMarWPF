using Dapper;
using MAR.AppLogic.MARHelpers;
using MarConnectCliente.ResponseModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories.SorteosMar
{
    public class ApuestaRepository
    {
        //Este metodo valida si hacienda ha iniciado el dia anteriormente
        public static TicketModel GetDTicket(string pTicketId)
        {
            try
            {
                string where = "";
                if (pTicketId.Contains("-"))
                {
                    where = " t.TicNumero = @TicketID AND hs.Activo = 1 and ht.Activo = 1";
                }
                else
                {
                    where = " t.TicketID = @TicketID AND hs.Activo = 1  and ht.Activo = 1";
                }
                //(d.TDeNumero = @TicketID OR t.TicketID = @TicketID) AND hs.Activo = 1
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT 
                                NoTicket = T.TicNumero
							    ,tc.Autorizacion
								,tc.TransaccionID
                                ,TerminalID = ht.HaciendaTerminalID
                                ,AutenticacionReferencia = tc.Autorizacion
                                ,CodigoOperacionReferencia = tc.TransaccionID
                                ,tc.NautCalculado
                                ,LocalID = ht.HaciendaLocalID
                                 ,MontoOperacion = t.TicCosto
                                 ,Fecha  = TicFecha
								   ,DetalleID = d.TicketDetalleID
                                 ,TipoJugadaID = stj.TipoJugadaID
                                 ,hs.SorteoID
                                 ,hs.Codigo
								 ,Saco = d.TDePago
                                 ,Monto = d.TDeCosto 
                                 ,Jugada = d.TDeNumero
                                  FROM DTickets t
                                  inner join DTicketDetalle d on t.TicketID = d.TicketID
                                  inner join HaciendaSorteo hs on hs.MARSorteoID = t.LoteriaID
                                  inner join HaciendaSorteoTipoJugada stj on stj.SorteoID = hs.SorteoID AND d.TDeQP = stj.MARReferencia
                                  inner join HaciendaTerminal ht on ht.MARTerminallD = t.BancaID
								  LEFT join TransaccionClienteHttp tc on tc.Referencia = t.TicNumero  and tc.Activo = 1
                                    where  " + where + $@"
                                  group by TicketDetalleID, t.TicketID, TicFecha, TicNumero, TipoJugadaID, hs.SorteoID, Codigo,TDePago,TDeCosto, TicCosto, TDeNumero, tc.Autorizacion,tc.TransaccionID, ht.HaciendaTerminalID, ht.HaciendaLocalID  ,tc.NautCalculado
								    union
									SELECT 
                                NoTicket = T.TicNumero
								 ,tc.Autorizacion
                                
								,tc.TransaccionID
                                ,TerminalID = ht.HaciendaTerminalID
                                ,AutenticacionReferencia = tc.Autorizacion
                                ,CodigoOperacionReferencia = tc.TransaccionID
                                 ,tc.NautCalculado
                                ,LocalID = ht.HaciendaLocalID
                                 ,MontoOperacion = t.TicCosto
                                 ,Fecha  = TicFecha
								   ,DetalleID = d.TicketDetalleID
                                 ,TipoJugadaID = stj.TipoJugadaID
                                 ,hs.SorteoID
                                 ,hs.Codigo
								 ,Saco = d.TDePago
                                 ,Monto = d.TDeCosto 
                                 ,Jugada = d.TDeNumero
                                  FROM HTickets t
                                  inner join HTicketDetalle d on t.TicketID = d.TicketID
                                  inner join HaciendaSorteo hs on hs.MARSorteoID = t.LoteriaID
                                  inner join HaciendaSorteoTipoJugada stj on stj.SorteoID = hs.SorteoID AND d.TDeQP = stj.MARReferencia
                                   inner join HaciendaTerminal ht on ht.MARTerminallD = t.BancaID
								   LEFT join TransaccionClienteHttp tc on tc.Referencia = t.TicNumero and tc.Activo = 1
                                       where  " + where + $@"
                                   group by TicketDetalleID, t.TicketID, TicFecha, TicNumero, TipoJugadaID, hs.SorteoID, Codigo,TDePago,TDeCosto, TicCosto, TDeNumero, tc.Autorizacion,tc.TransaccionID,ht.HaciendaTerminalID, ht.HaciendaLocalID  ,tc.NautCalculado "
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

        public static void AgregaErrorPrueba(string pError)
        {
            using (var con = DALHelper.GetSqlConnection())
            {
                try
                {
                    string query = $@"INSERT INTO ErrorPruebaCincoMinutos ([Error]) values (@Error)";
                    var p = new DynamicParameters();
                    p.Add("@Error", pError);


                    var consultaPago = con.Query(query, p, commandType: CommandType.Text);

                }
                catch (Exception e)
                {

                }
            }
        }
        public static List<TicketModel> GetDTicketsFueraDeLinea()
        {
            try
            {
                string where = " len(tc.NautCalculado) > 5 and    hs.Activo = 1 and ht.Activo = 1 and tc.Estado != 'Reenviada'";

                //(d.TDeNumero = @TicketID OR t.TicketID = @TicketID) AND hs.Activo = 1
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT 
                                NoTicket = T.TicNumero
							    ,tc.Autorizacion
								,tc.TransaccionID
                                ,TerminalID = ht.HaciendaTerminalID
                                ,AutenticacionReferencia = tc.Autorizacion
                                ,CodigoOperacionReferencia = tc.TransaccionID
                                ,tc.NautCalculado
                                ,t.TicketID 
                                ,LocalID = ht.HaciendaLocalID
                                 ,MontoOperacion = t.TicCosto
                                 ,Fecha  = TicFecha
								   ,DetalleID = d.TicketDetalleID
                                , d.TicketID 
                                 ,TipoJugadaID = stj.TipoJugadaID
                                 ,hs.SorteoID
                                 ,hs.Codigo
								 ,Saco = d.TDePago
                                 ,Monto = d.TDeCosto 
                                 ,Jugada = d.TDeNumero
                                  FROM DTickets t
                                  inner join DTicketDetalle d on t.TicketID = d.TicketID
                                  inner join HaciendaSorteo hs on hs.MARSorteoID = t.LoteriaID
                                  inner join HaciendaSorteoTipoJugada stj on stj.SorteoID = hs.SorteoID AND d.TDeQP = stj.MARReferencia
                                  inner join HaciendaTerminal ht on ht.MARTerminallD = t.BancaID
								  LEFT join TransaccionClienteHttp tc on tc.Referencia = t.TicNumero
                                    where  " + where + $@"
                                  group by TicketDetalleID, t.TicketID,d.TicketID, TicFecha, TicNumero, TipoJugadaID, hs.SorteoID, Codigo,TDePago,TDeCosto, TicCosto, TDeNumero, tc.Autorizacion,tc.TransaccionID, ht.HaciendaTerminalID, ht.HaciendaLocalID  ,tc.NautCalculado
								    union
									SELECT 
                                NoTicket = T.TicNumero
								 ,tc.Autorizacion
                                
								,tc.TransaccionID
                                ,TerminalID = ht.HaciendaTerminalID
                                ,AutenticacionReferencia = tc.Autorizacion
                                ,CodigoOperacionReferencia = tc.TransaccionID
                                 ,tc.NautCalculado
                                ,t.TicketID 
                                ,LocalID = ht.HaciendaLocalID
                                 ,MontoOperacion = t.TicCosto
                                 ,Fecha  = TicFecha
								   ,DetalleID = d.TicketDetalleID
                                 ,TipoJugadaID = stj.TipoJugadaID
                                 ,d.TicketID 
                                 ,hs.SorteoID
                                 ,hs.Codigo
								 ,Saco = d.TDePago
                                 ,Monto = d.TDeCosto 
                                 ,Jugada = d.TDeNumero
                                  FROM HTickets t
                                  inner join HTicketDetalle d on t.TicketID = d.TicketID
                                  inner join HaciendaSorteo hs on hs.MARSorteoID = t.LoteriaID
                                  inner join HaciendaSorteoTipoJugada stj on stj.SorteoID = hs.SorteoID AND d.TDeQP = stj.MARReferencia
                                   inner join HaciendaTerminal ht on ht.MARTerminallD = t.BancaID
								   LEFT join TransaccionClienteHttp tc on tc.Referencia = t.TicNumero  
                                       where  " + where + $@"
                                   group by TicketDetalleID, t.TicketID, d.TicketID, TicFecha, TicNumero, TipoJugadaID, hs.SorteoID, Codigo,TDePago,TDeCosto, TicCosto, TDeNumero, tc.Autorizacion,tc.TransaccionID,ht.HaciendaTerminalID, ht.HaciendaLocalID  ,tc.NautCalculado "
                                  ;

                    var p = new DynamicParameters();
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
                     .Distinct().GroupBy(x => x.NoTicket).Select(x => x.FirstOrDefault());

                    var detallesClean = detallesList.Distinct().GroupBy(x => x.DetalleID).Select(x => x.FirstOrDefault());
                    foreach (var item in ticket)
                    {
                        item.TicketDetalles = new List<TicketDetalle>();
                        foreach (var d in detallesClean)
                        {
                            if (d.TicketID == item.TicketID)
                            {
                                item.TicketDetalles.Add(d);
                            }
                        }
                    }
                    con.Close();
                    return ticket.ToList();
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;
            }
        }


        public static void ActualizaSacoVPTrasnacciones(List<ConsultaSaco> pConsultaSaco)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    DataTable transDt = DALHelper.ToDataTable(pConsultaSaco.ToList());
                    //AGREGA INDEX PARA RECORRER LAS TABLAS EN SQL*************************************
                    transDt.Columns.Add("Id", typeof(Int32));
                    int index = 0;
                    foreach (DataRow row in transDt.Rows)
                    {
                        row["Id"] = index;
                        index++;
                    }
                    //REMUEVE COLUMNAS INNECESARIAS*************************************
                    transDt.Columns.Remove("Referencia");
                    transDt.Columns.Remove("MARBancaID");

                    var p = new DynamicParameters();
                    p.Add("@VPTransaccionSaco", transDt.AsTableValuedParameter());
                    var  desgloseJugadas = con.Query("VP_ActualizaSacoVPTransaccion", p, commandType: CommandType.StoredProcedure).Distinct();

                }
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public class TicketModel
        {
            public string NoTicket { get; set; }
            public int TicketID { get; set; }
            public string NautCalculado { get; set; }
            public DateTime Fecha { get; set; }
            public int TerminalID { get; set; }
            public string AutenticacionReferencia { get; set; }
            public string CodigoOperacionReferencia { get; set; }

            public int LocalID { get; set; }
            public decimal MontoOperacion { get; set; }
            public List<TicketDetalle> TicketDetalles { get; set; }
        }
        public class TicketDetalle
        {
            public int TicketID { get; set; }
            public int DetalleID { get; set; }
            public string Codigo { get; set; }
            public int SorteoID { get; set; }
            public int Monto { get; set; }
            public decimal Saco { get; set; }
            public string Jugada { get; set; }
            public int TipoJugadaID { get; set; }


        }
    }
}

