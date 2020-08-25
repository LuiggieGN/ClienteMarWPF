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
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace MAR.DataAccess.EFRepositories.CLRepositories
{

    public class CLReciboRepository
    {

        
        public static CL_Recibo AgregarCL_Recibo(CL_Recibo pClRecibo, string pIdentificadorJuego, bool pReciboActivo)
        {

            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {

                    DataTable transDt = DALHelper.ToDataTable(pClRecibo.RF_Transacciones.ToList());
                    DataTable reciboDetalleDt = DALHelper.ToDataTable(pClRecibo.CL_ReciboDetalle_Extra.ToList());

                    List<RF_TransaccionJugada> tJugadas = new List<RF_TransaccionJugada>();
                    List<CL_ReciboDetalle_Extra> tReciboDetalles = new List<CL_ReciboDetalle_Extra>();

                    foreach (var item in pClRecibo.RF_Transacciones.Select(x => x.RFTransaccionJugadas))
                    {
                        tJugadas.Add(item.FirstOrDefault());
                    }

                    foreach (var item in pClRecibo.CL_ReciboDetalle_Extra)
                    {
                        tReciboDetalles.Add(item);
                    }

                    DataTable transJugadaDt = DALHelper.ToDataTable(tJugadas);
                    DataTable recDetallesDt = DALHelper.ToDataTable(tReciboDetalles);

                    //AGREGA INDEX PARA RECORRER LAS TABLAS EN SQL*************************************
                    transDt.Columns.Add("IndexID", typeof(Int32));
                    transJugadaDt.Columns.Add("IndexID", typeof(Int32));
                    recDetallesDt.Columns.Add("IndexID", typeof(Int32));
                    int index = 0;
                    foreach (DataRow row in transDt.Rows)
                    {
                        row["IndexID"] = index;
                        index++;
                    }
                    index = 0;
                    foreach (DataRow row in transJugadaDt.Rows)
                    {
                        row["IndexID"] = index;
                        index++;
                    }
                    foreach (DataRow row in recDetallesDt.Rows)
                    {
                        row["IndexID"] = index;
                        index++;
                    }
                    //REMUEVE COLUMNAS INNECESARIAS*************************************
                    transDt.Columns.Remove("RFTransaccionJugadas");
                    transDt.Columns.Remove("TransaccionID");
                    transDt.Columns.Remove("RF_TransaccionDetalle");
                    transJugadaDt.Columns.Remove("SorteoTipoJugada");
                    transJugadaDt.Columns.Remove("TransaccionJugadaID");
                    recDetallesDt.Columns.Remove("CL_Recibo");
                    recDetallesDt.Columns.Remove("Descuento");
                    recDetallesDt.Columns.Remove("Ingreso");
                    recDetallesDt.Columns.Remove("ReciboDetalleID");
                    recDetallesDt.Columns.Remove("ReciboID");
                    recDetallesDt.Columns.Remove("ValorMonto");




                    var p = new DynamicParameters();
                    p.Add("@Serie", pClRecibo.Serie);
                    p.Add("@BancaID", pClRecibo.BancaID);
                    p.Add("@ReciboActivo", pReciboActivo);
                    p.Add("@UsuarioID", pClRecibo.UsuarioID);
                    p.Add("@ClienteID", pClRecibo.ClienteID);
                    p.Add("@Ingresos", pClRecibo.Ingresos);
                    p.Add("@SolicitudID", pClRecibo.SolicitudID);
                    p.Add("@IdentificadorJuego", pIdentificadorJuego);
                    p.Add("@RFTransacciones", transDt.AsTableValuedParameter());
                    p.Add("@RFTransaccionJugada", transJugadaDt.AsTableValuedParameter());
                    p.Add("@CLReciboDetallesExtras", recDetallesDt.AsTableValuedParameter());

                    //var recibo = con.QueryFirst<CL_Recibo>("RF_Recibo_Agrega", p, commandType: CommandType.StoredProcedure);

                    List<RF_Transaccion> transacciones = new List<RF_Transaccion>();
                    var recibo = con.Query<CL_Recibo, RF_Transaccion, RF_TransaccionJugada, CL_Recibo>(
                        "RF_Recibo_Agrega",
                        (rec, transa, tJug) =>
                        {
                            CL_Recibo reciboEntry;
                            reciboEntry = rec;

                            reciboEntry.RF_Transacciones = new List<RF_Transaccion>();

                            transa.RFTransaccionJugadas.Add(tJug);
                            reciboEntry.RF_Transacciones.Add(transa);
                            transacciones.Add(transa); //ESTA LINEA ES PARA LLENAR LAS TRANSACCIONES Y LUEGO AGREGARLA A UN SOLO RECIBO
                            return reciboEntry;
                        }, param: p,
                        splitOn: "ReciboID,TransaccionID,TransaccionJugadaID", commandType: CommandType.StoredProcedure)
                    .Distinct().First();

                    recibo.RF_Transacciones = transacciones;


                    return recibo;
                }

            }
            catch (Exception e)
            {
                return null;
            }




            //var db = new MARContext();
            //db.CL_Recibo.Add(pClRecibo);
            //db.SaveChanges();
            //return pClRecibo;
        }



        public static bool AutorizaCL_ReciboJuegosNuevos(List<CL_ReciboDetalle_Extra> pClReciboExtra, int pReciboId)
        {

            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    List<CL_ReciboDetalle_Extra> tReciboDetallesExtra = new List<CL_ReciboDetalle_Extra>();

                    foreach (var item in pClReciboExtra)
                    {
                        tReciboDetallesExtra.Add(item);
                    }
                    DataTable recDetallesDt = DALHelper.ToDataTable(tReciboDetallesExtra);
                    //AGREGA INDEX PARA RECORRER LAS TABLAS EN SQL*************************************
                    recDetallesDt.Columns.Add("IndexID", typeof(Int32));
                    int index = 0;
                    foreach (DataRow row in recDetallesDt.Rows)
                    {
                        row["IndexID"] = index;
                        index++;
                    }
                    //REMUEVE COLUMNAS INNECESARIAS*************************************
                    recDetallesDt.Columns.Remove("CL_Recibo");
                    recDetallesDt.Columns.Remove("Descuento");
                    recDetallesDt.Columns.Remove("Ingreso");
                    recDetallesDt.Columns.Remove("ReciboDetalleID");
                    recDetallesDt.Columns.Remove("ReciboID");
                    recDetallesDt.Columns.Remove("ValorMonto");

                    var p = new DynamicParameters();
                    p.Add("@ReciboID", pReciboId);
                    p.Add("@CLReciboDetallesExtras", recDetallesDt.AsTableValuedParameter());


                    var autoriza = con.Query<bool>("RF_Recibo_AutorizaJuegosNuevos", param: p,commandType: CommandType.StoredProcedure).First();

                    return autoriza;
                }

            }
            catch (Exception e)
            {
                return false;
            }

        }


        public static List<CL_Recibo> Get_Recibos(object pReciboID, int pBancaID) // ReciboID o Ticket#
        {
            using (var con = DALHelper.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@ReciboOticket", pReciboID);
                string query;


                if (pReciboID == null && pBancaID > 0)
                {
                    query = "SELECT * FROM CL_Recibo R JOIN RF_Transaccion T ON R.ReciboID = T.ReciboID JOIN RF_TransaccionJugada TJ ON T.TransaccionID = TJ.TransaccionID WHERE CONVERT(date, Fecha) = CONVERT(date, GetDate()) AND  BancaID = " + pBancaID + " ";
                }

                else
                {
                    var isNumeric = int.TryParse(pReciboID.ToString(), out var n);
                    if (isNumeric)
                    {
                        query = "SELECT * FROM CL_Recibo R JOIN RF_Transaccion T ON R.ReciboID = T.ReciboID JOIN RF_TransaccionJugada TJ ON T.TransaccionID = TJ.TransaccionID WHERE R.ReciboID = @ReciboOticket AND BancaID = " + pBancaID + " ";
                    }
                    else if (!isNumeric && pBancaID > 0)
                    {
                        query = "SELECT * FROM CL_Recibo R JOIN RF_Transaccion T ON R.ReciboID = T.ReciboID JOIN RF_TransaccionJugada TJ ON T.TransaccionID = TJ.TransaccionID WHERE  R.Referencia = @ReciboOticket AND BancaID = " + pBancaID + " ";
                    }
                    else
                    {
                        query = "SELECT * FROM CL_Recibo R JOIN RF_Transaccion T ON R.ReciboID = T.ReciboID JOIN RF_TransaccionJugada TJ ON T.TransaccionID = TJ.TransaccionID WHERE  R.Referencia = @ReciboOticket ";
                    }
                }


                List<RF_Transaccion> transacciones = new List<RF_Transaccion>();
                //List<CL_Recibo> recibos = new List<CL_Recibo>();
                var recibo = con.Query<CL_Recibo, RF_Transaccion, RF_TransaccionJugada, CL_Recibo>(
                    query,
                    (rec, transa, tJug) =>
                    {
                        CL_Recibo reciboEntry;
                        reciboEntry = rec;

                        reciboEntry.RF_Transacciones = new List<RF_Transaccion>();

                        transa.RFTransaccionJugadas.Add(tJug);
                        transacciones.Add(transa); //ESTA LINEA ES PARA LLENAR LAS TRANSACCIONES Y LUEGO AGREGARLA A UN SOLO RECIBO
                        //recibos.Add(reciboEntry);
                        return reciboEntry;
                    }, param: p,
                    splitOn: "ReciboID,TransaccionID,TransaccionJugadaID", commandType: CommandType.Text)
                .Distinct().GroupBy(x => x.ReciboID).Select(x => x.FirstOrDefault());

                foreach (var item in recibo)
                {
                    foreach (var tran in transacciones)
                    {
                        if (tran.ReciboID == item.ReciboID)
                        {
                            item.RF_Transacciones.Add(tran);
                        }
                    }
                }

                return recibo.ToList();

            }
        }

        public static List<CamposExtras> GetCamposExtras(string pCampos, int pReciboId)
        {
            List<CamposExtras> campos = new List<CamposExtras>();
            using (var con = DALHelper.GetSqlConnection())
            {
                var p = new DynamicParameters();
                p.Add("@Campos", pCampos);
                p.Add("@ReciboID", pReciboId);
                var result = con.Query<CamposExtras>("RF_Recibo_GetExtraCampos", p, commandType: CommandType.StoredProcedure).Distinct().ToList();
                return result;
            }
        }

        public class CamposExtras
        {
            public string Campo { get; set; }
            public string Referencia { get; set; }
            public string Valor { get; set; }
        }







        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public CLReciboRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
