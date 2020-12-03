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
    public static class CuadreRepository
    {
        public static CuadreRegistroResultDTO Registrar(CuadreDTO cuadre, bool esUnRetiro)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); CuadreRegistroResultDTO registroEstado = null;
                p.Add("@CuadreTipo", cuadre.Tipo);
                p.Add("@BanCajaID", cuadre.CajaID);
                p.Add("@BancaBalanceMin", cuadre.BalanceMinimo);
                p.Add("@BanDineroPorPagar", cuadre.MontoPorPagar);
                p.Add("@BanMontoContado", cuadre.MontoContado);
                p.Add("@Cajera", cuadre.UsuarioCaja);
                p.Add("@CajaOrigenID", cuadre.CajaOrigenID);
                p.Add("@UsuaOrigenID", cuadre.UsuarioOrigenID);
                p.Add("@MontoRetirado", cuadre.MontoRetirado);
                p.Add("@MontoDepositado", cuadre.MontoDepositado);
                p.Add("@EsUnDeposito", esUnRetiro);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    registroEstado = db.Query<CuadreRegistroResultDTO>(CuadreHelper.Procedure_SP_REGISTRAR_CUADRE, p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (registroEstado == null)
                    {
                        throw new Exception("La operacion no pudo ser completada");
                    }
                    db.Close();
                }

                return registroEstado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static bool EnlazarRutaConCuadre(string rutaEstado, int rutaUltimaLocalidad, string rutaOrdenRecorrido, int cuadreId, int bancaCajaId, int rutaId)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool fueEnlazado = false;
                p.Add("@rutaEstado", rutaEstado);
                p.Add("@rutaUltimaLocalidad", rutaUltimaLocalidad);
                p.Add("@rutaOrdenRecorrido", rutaOrdenRecorrido);
                p.Add("@cuadreId", cuadreId);
                p.Add("@bancaCajaId", bancaCajaId);
                p.Add("@rutaId", rutaId);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<bool> estados = db.Query<bool>(CuadreHelper.QueryEnlazarRutaConCuadre, p, commandType: CommandType.Text).ToList();

                    if (estados.Any())
                    {
                        fueEnlazado = estados.First();
                    }
                    db.Close();
                }

                return fueEnlazado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }









    }//fin de clase
}
