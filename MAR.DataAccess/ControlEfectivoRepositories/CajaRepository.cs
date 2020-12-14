using Dapper;

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories.Helpers;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

namespace MAR.DataAccess.ControlEfectivoRepositories
{
    public static class CajaRepository
    {
        public static SupraMovimientoEnBancaResultDTO RegistrarMovimientoEnBanca(SupraMovimientoEnBancaDTO movimiento)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@cajaid", movimiento.CajaId);
                p.Add("@bancaid", movimiento.BancaId);
                p.Add("@usuarioid", movimiento.UsuarioId);
                p.Add("@monto", movimiento.Monto);
                p.Add("@comentario", movimiento.Comentario);
                p.Add("@ie", movimiento.KeyIE);

                SupraMovimientoEnBancaResultDTO result = null;

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();
                    result = db.Query<SupraMovimientoEnBancaResultDTO>(CajaHelper.QueryParaRegistrarMovimientoEnBanca, p, commandType: CommandType.Text).FirstOrDefault();
                    db.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static SupraMovimientoDesdeHastaResultDTO RegistrarTransferencia(SupraMovimientoDesdeHastaDTO transferencia)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@TipoCajaOrigen", transferencia.TipoCajaOrigen);
                p.Add("@TipoCajaDestino", transferencia.TipoCajaDestino);
                p.Add("@CajaOrigenID", transferencia.CajaOrigenId);
                p.Add("@CajaDestino", transferencia.CajaDestinoId);
                p.Add("@UsuarioID", transferencia.UsuarioId);
                p.Add("@Descripcion", transferencia.Comentario);
                p.Add("@Monto", transferencia.Monto);


                SupraMovimientoDesdeHastaResultDTO result = null;

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();
                    result = db.Query<SupraMovimientoDesdeHastaResultDTO>(CajaHelper.QueryParaRegistrarTransferencia, p, commandType: CommandType.Text).FirstOrDefault();
                    db.Close();
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MultipleDTO<PagerResumenDTO, List<MovimientoDTO>> LeerMovimientos(MovimientoPageDTO paginaRequest)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@PaginaNo", paginaRequest.PaginaNo);
                p.Add("@PaginaSize", paginaRequest.PaginaSize);
                p.Add("@OrdenAsc", paginaRequest.OrdenAsc);
                p.Add("@OrdenColumna", paginaRequest.OrdenColumna);
                p.Add("@bancaId", paginaRequest.BancaId);
                p.Add("@cajaID", paginaRequest.CajaId);
                p.Add("@fechaDesde", paginaRequest.ConsultaDesde);
                p.Add("@fechaHasta", paginaRequest.ConsultaHasta);
                p.Add("@categoriaOperacion", paginaRequest.CategoriaOperacion);

                var multi = new MultipleDTO<PagerResumenDTO, List<MovimientoDTO>>();

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    using (var queryMultiple = db.QueryMultiple(CajaHelper.Procedure_SP_CAJA_LEER_MOVIMIENTOS, p, commandType: CommandType.StoredProcedure))

                    {
                        multi.PrimerDTO = queryMultiple.Read<PagerResumenDTO>().First();
                        
                        multi.SegundoDTO = queryMultiple.Read<MovimientoDTO>().ToList();

                    }// fin de using

                    db.Close();
                }

                return multi;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static decimal LeerCajaBalance(int cajaid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); decimal balanceActual = 0;

                p.Add("@CajaID", cajaid);
                p.Add("@BalanceActual", dbType: DbType.Decimal, direction: ParameterDirection.Output);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    db.Execute(CajaHelper.Procedure_SP_CAJA_BALANCE_ACTUAL, p, commandType: CommandType.StoredProcedure);

                    balanceActual = p.Get<decimal>("@BalanceActual");

                    db.Close();
                }

                return balanceActual;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CajaDTO LeerCajaDeUsuarioPorUsuarioId(int usuarioid)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@usuarioid", usuarioid);

                CajaDTO caja = null;           

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();
                    caja = db.Query<CajaDTO>(CajaHelper.SelectCajaDeUsuarioPorUsuarioId, p, commandType: CommandType.Text).FirstOrDefault();
                    db.Close();
                }

                return caja;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


 




    }//fin de clase
}
