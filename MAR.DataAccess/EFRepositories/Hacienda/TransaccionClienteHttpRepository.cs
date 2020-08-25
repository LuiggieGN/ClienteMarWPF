using Dapper;
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.Tables.DTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.EFRepositories.Hacienda
{
    public class TransaccionClienteHttpRepository
    {
        public static TransaccionClienteHttp AgregaTransaccion(TransaccionClienteHttp pTransaccion)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"INSERT INTO  TransaccionClienteHttp
           (TipoTransaccionID
           ,BancaID
           ,Fecha
           ,Monto
           ,Referencia
           ,FechaSolicitud
           ,Estado
           ,Activo
           ,TipoAutorizacion
           ,Peticion
           ,NautCalculado
           ,Comentario, Respuesta, Autorizacion, FechaRespuesta)
     VALUES
           (@TipoTransaccionID, @BancaID,@Fecha,@Monto, @Referencia, @FechaSolicitud, @Estado, @Activo, @TipoAutorizacion, @Peticion, @NautCalculado, @Comentario, @Respuesta,@Autorizacion, @FechaRespuesta );
                                     SELECT top 1 * from TransaccionClienteHttp where TransaccionID = SCOPE_IDENTITY();";
                    var p = new DynamicParameters();
                    p.Add("@TipoTransaccionID", pTransaccion.TipoTransaccionID);
                    p.Add("@BancaID", pTransaccion.BancaID);
                    p.Add("@Fecha", pTransaccion.Fecha);
                    p.Add("@Referencia", pTransaccion.Referencia);
                    p.Add("@Estado", pTransaccion.Estado);
                    p.Add("@Monto", pTransaccion.Monto);
                    p.Add("@Activo", pTransaccion.Activo);
                    p.Add("@FechaSolicitud", pTransaccion.FechaSolicitud);
                    p.Add("@TipoAutorizacion", pTransaccion.TipoAutorizacion);
                    p.Add("@Peticion", pTransaccion.Peticion);
                    p.Add("@Respuesta", pTransaccion.Respuesta);
                    p.Add("@FechaRespuesta", pTransaccion.FechaRespuesta);
                    p.Add("@Autorizacion", pTransaccion.Autorizacion);
                    p.Add("@NautCalculado", pTransaccion.NautCalculado);
                    p.Add("@Comentario", pTransaccion.Comentario);

                    var trasaccion = con.QueryFirst<TransaccionClienteHttp>(query, p, commandType: CommandType.Text);
                    return trasaccion;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;

            }
        }
        public static void AgregaError(string pError)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@Error", pError);

                string query = $@"INSERT INTO ErrorPruebaCincoMinutos ([Error]) values (@Error)";
                using (var con = DALHelper.GetSqlConnection())
                {
                    con.Open();

                    con.Query(query, p, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static TransaccionClienteHttp ActualizaTransaccion(TransaccionClienteHttp pTransaccion)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"UPDATE  TransaccionClienteHttp
                                           SET  
                                              Autorizacion = @Autorizacion
                                              ,FechaRespuesta = @FechaRespuesta
                                              ,Estado = @Estado
                                              ,Activo = @Activo
                                              ,Respuesta = @Respuesta
                                              ,NautCalculado = @NautCalculado
                                              ,Peticion = @Peticion
                                              ,Comentario = @Comentario
                                         WHERE transaccionID = @TransaccionId; 
                                  SELECT top 1 * from TransaccionClienteHttp where TransaccionID = @TransaccionId;";

                    var p = new DynamicParameters();
                    p.Add("@TransaccionId", pTransaccion.TransaccionID);
                    p.Add("@Autorizacion", pTransaccion.Autorizacion);
                    p.Add("@FechaRespuesta", pTransaccion.FechaRespuesta);
                    p.Add("@Estado", pTransaccion.Estado);
                    p.Add("@Activo", pTransaccion.Activo);
                    p.Add("@Respuesta", pTransaccion.Respuesta);
                    p.Add("@Peticion", pTransaccion.Peticion);
                    p.Add("@NautCalculado", pTransaccion.NautCalculado);
                    p.Add("@Comentario", pTransaccion.Comentario);

                    var trasaccion = con.QueryFirst<TransaccionClienteHttp>(query, p, commandType: CommandType.Text);
                    return trasaccion;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;

            }
        }

        public static TransaccionClienteHttp ActualizaTransaccionFueraLinea(TransaccionClienteHttp pTransaccion)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"UPDATE  TransaccionClienteHttp
                                           SET  
                                         
                                               Estado = @Estado
                                      ,  Autorizacion = @Autorizacion
                                          ,Comentario = @Comentario
                                         WHERE transaccionID = @TransaccionId; 
                                  SELECT top 1 * from TransaccionClienteHttp where TransaccionID = @TransaccionId;";

                    var p = new DynamicParameters();
                    p.Add("@TransaccionId", pTransaccion.TransaccionID);
                    p.Add("@Estado", pTransaccion.Estado);
                    p.Add("@Comentario", pTransaccion.Comentario);
                    p.Add("@Autorizacion", pTransaccion.Autorizacion);
                    var trasaccion = con.QueryFirst<TransaccionClienteHttp>(query, p, commandType: CommandType.Text);
                    return trasaccion;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;

            }
        }


        public static TransaccionClienteHttp GetTransaccionClienteHttp(string pReferencia)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT  TransaccionID
                            ,TipoTransaccionID
                            ,BancaID
                            ,Fecha
                            ,Monto
                            ,Referencia
                            ,Autorizacion
                            ,Estado
                            ,Activo
                        FROM TransaccionClienteHttp where Referencia = @Referencia and Activo = 1"
                                                 ;

                    var p = new DynamicParameters();
                    p.Add("@Referencia", pReferencia);
                    var transaccion = con.Query<TransaccionClienteHttp>(query, p, commandType: CommandType.Text).FirstOrDefault();
                    con.Close();
                    return transaccion;
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return null;
            }
        }
    }
}
