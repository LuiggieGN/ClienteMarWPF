using System;
using Dapper;
using System.Data;
using System.Collections.Generic;
using System.Linq;

using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;

using Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers;
using MAR.AppLogic.MARHelpers;
using Flujo.Entities.WpfClient.POCO;

namespace Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories
{
    public static  partial class MovimientoRepositorio
    {
        public static MovimientoInsertEstado InsertarMovimientoSinUsuarioAutorizado(int pBancaID, int pBancaUsuarioID, decimal pMonto, string pDescripcion, int pIngresoOEgreso, int pTipoIngresoOTipoEgreso)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@BancaID", pBancaID);
                p.Add("@BancaUsuarioID", pBancaUsuarioID);
                p.Add("@Monto", pMonto);
                p.Add("@Descripcion", pDescripcion);
                p.Add("@IngresoOEgreso", pIngresoOEgreso);
                p.Add("@TipoIngresoOTipoEgreso", pTipoIngresoOTipoEgreso);
                MovimientoInsertEstado insertestado = null;

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    insertestado = con.Query<MovimientoInsertEstado>("[flujo].[Sp_InsertMovimientoBanca]", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (insertestado == null)
                    {
                        insertestado = new MovimientoInsertEstado() { FueProcesado = false, MensajeError = "Ha ocurrido un error al procesar la trasancción." };
                    }
                }

                return insertestado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MovimientoInsertEstado InsertaMovimientoConUsuarioAutorizado(int pBancaID, int pBancaUsuarioID, int pUsuarioExternoAutorizadoID, decimal pMonto, string pDescripcion, int pIngresoOEgreso, int pTipoIngresoOTipoEgreso)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@BancaID", pBancaID);
                p.Add("@BancaUsuarioID", pBancaUsuarioID);
                p.Add("@UsuarioExternoAutorizadoID", pUsuarioExternoAutorizadoID);
                p.Add("@Monto", pMonto);
                p.Add("@Descripcion", pDescripcion);
                p.Add("@IngresoOEgreso", pIngresoOEgreso);
                p.Add("@TipoIngresoOTipoEgreso", pTipoIngresoOTipoEgreso);
                MovimientoInsertEstado insertestado = null;

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    insertestado = con.Query<MovimientoInsertEstado>("[flujo].[Sp_InsertMovimientoBancaConUsuarioExternoAutorizado]", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                    if (insertestado == null)
                    {
                        insertestado = new MovimientoInsertEstado() { FueProcesado = false, MensajeError = "Ha ocurrido un error al procesar la trasancción." };
                    }
                }

                return insertestado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /*** Metodos para insertar records*/
      

        public static List<TipoIngresoResponseModel> ConsultarCategoriaTiposIngresos()
        {
            try
            {
                List<TipoIngresoResponseModel> theTipoIngresoList = new List<TipoIngresoResponseModel>();

                string query = @"
                                   select   ti.TipoIngresoID  As Id,  ti.TipoNombre  As Nombre,  ti.Descripcion, ti.LogicaKey,  ti.FechaCreacion,  ti.Activo  from flujo.TipoIngreso ti where ti.EsTipoSistema = 0 and ti.EsTipoAnonimo = 1;
                                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<TipoIngresoResponseModel> aux = con.Query<TipoIngresoResponseModel>(query, commandType: CommandType.Text).ToList();

                    if (aux.Any())
                    {
                        theTipoIngresoList.AddRange(aux);
                    }

                    con.Close();
                }
                return theTipoIngresoList;
            }
            catch (Exception ex)
            {
                return new List<TipoIngresoResponseModel>();
            }
        }
        public static List<TipoEgresoResponseModel> ConsultarCategoriasTiposEgresos()
        {
            try
            {
                List<TipoEgresoResponseModel> theTipoEgresoList = new List<TipoEgresoResponseModel>();

                string query = @"
                                   select  te.TipoEgresoID  As Id,  te.TipoNombre  As Nombre,  te.Descripcion, te.LogicaKey,  te.FechaCreacion,  te.Activo  from flujo.TipoEgreso te where te.EsTipoSistema = 0 and te.EsTipoAnonimo = 1;
                                ";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<TipoEgresoResponseModel> aux = con.Query<TipoEgresoResponseModel>(query, commandType: CommandType.Text).ToList();

                    if (aux.Any())
                    {
                        theTipoEgresoList.AddRange(aux);
                    }

                    con.Close();
                }

                return theTipoEgresoList;
            }
            catch (Exception ex )
            {
                return new List<TipoEgresoResponseModel>();                
            }
        }

        /*** Genrando Id de forma aleatoria*/
        private static string GenerateId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks).ToUpper();
        }

    }// Fin Clase Parcial  MovimientoRepositorio

}





















































/***  Metodos de Consulta*/
// public static List<MovimientoResponseModel> ConsultarLastXRecords(int pBancaID, int pSeleccionaXRecords)
// {
//     try
//     {
//         List<MovimientoResponseModel> coleccionMovimientos = new List<MovimientoResponseModel>();

//         DynamicParameters p = new DynamicParameters();

//         p.Add("@BancaID", pBancaID);
//         p.Add("@SelectXRecord", pSeleccionaXRecords);

//         string query = @"

//             select * from (
//                   select

//                     R1.CajaID,
//                     R1.Categoria, 
//                     R1.CategoriaSubTipoID, 
//R1.CategoriaConcepto,
//                     row_number() over(partition by R1.CajaID order by R1.Fecha  DESC ) As Orden,
//                     m.MovimientoID, 
//                     R1.Fecha,
//                     R1.Referencia,
//                     m.Descripcion, 
//                     m.Monto As EntradaOSalida, 
//                     R1.Balance

//                   from
//                     (

//                      select

//                           i.CajaID, 
//	  'Ingreso'       As Categoria, 
//	  i.TipoIngresoID As CategoriaSubTipoID, 
//	  (select top 1 ti.TipoNombre from flujo.TipoIngreso ti Where ti.TipoIngresoID = i.TipoIngresoID) As CategoriaConcepto,
//	  i.MovimientoID, 
//	  i.FechaIngreso  As Fecha,
//	  i.Referencia,
//	  i.Balance 

//                      from flujo.Ingreso i 

//                      Where
//                          i.CajaID =  ( select c.CajaID from  flujo.Caja c Where  c.BancaID = @BancaID and c.TipoCajaID = (  select tc.TipoCajaID from  flujo.TipoCaja tc Where tc.Nombre = 'CAJA_TERMINAL' ) )

//                        union all

//                      select

//                          e.CajaID,
//	'Egreso'  As Categoria,								
//	 e.TipoEgresoID  As CategoriaSubTipoID,
//	 (select top 1 te.TipoNombre from flujo.TipoEgreso te Where te.TipoEgresoID = e.TipoEgresoID) As CategoriaConcepto,
//	 e.MovimientoID,
//	 e.FechaEgreso  As Fecha, 
//	 e.Referencia,
//	 e.Balance

//                      from flujo.Egreso e

//                      Where
//                          e.CajaID =  ( select c.CajaID from  flujo.Caja c Where  c.BancaID = @BancaID and c.TipoCajaID = (  select tc.TipoCajaID from  flujo.TipoCaja tc Where tc.Nombre = 'CAJA_TERMINAL' ) )

//                     ) R1 join flujo.Movimiento m on m.MovimientoID = R1.MovimientoID 

//                     ) R2 Where R2.Orden <= @SelectXRecord

//        ";

//         using (var con = DapperDBHelper.GetSqlConnection())
//         {

//             con.Open();

//             List<MovimientoResponseModel> rr = con.Query<MovimientoResponseModel>( query,  p,  commandType: CommandType.Text).ToList();

//             if (rr.Any())
//             {
//                 coleccionMovimientos.AddRange(rr);
//             }

//             con.Close();
//         }
//         return coleccionMovimientos;

//     }
//     catch (Exception e)
//     {
//         return new List<MovimientoResponseModel>();
//     }
// }