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
