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
    public static class BancaRepository
    {
        public static BancaConfiguracionDTO LeerBancaConfiguraciones(int bancaid)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@bancaid", bancaid);

                var configuraciones = new BancaConfiguracionDTO();

                using (var db = DALHelper.GetSqlConnection())
                {
                    using (var queryMultiple = db.QueryMultiple(BancaHelper.SelectBancaConfiguraciones, p, commandType: CommandType.Text))
                    {
                        configuraciones.BancaDto = queryMultiple.Read<BancaDTO>().FirstOrDefault();
                        configuraciones.CajaEfectivoDto = queryMultiple.Read<CajaDTO>().FirstOrDefault();
                        configuraciones.ControlEfectivoConfigDto = queryMultiple.Read<ControlEfectivoDTO>().FirstOrDefault();
                    }
                }

                return configuraciones;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static int? LeerBancaLastCuadreId(int bancaid)
        {
            try
            {
                var p = new DynamicParameters();
                p.Add("@bancaid", bancaid);

                int? ultimocuadre = 0;

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<int> ids = db.Query<int>(BancaHelper.SelectBancaUltimoCuadreId, p, commandType: CommandType.Text).ToList();

                    if (!ids.Any())
                    {
                        ultimocuadre = null;
                    }
                    else
                    {
                        ultimocuadre = ids.FirstOrDefault();
                    }
                }

                return ultimocuadre;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public static List<BancaControlEfectivoTransaccionDTO> LeerBancaLastTransaccionesApartirDelUltimoCuadre(int bancaid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); List<BancaControlEfectivoTransaccionDTO> transacciones = null;

                p.Add("@BanID", bancaid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    transacciones = db.Query<BancaControlEfectivoTransaccionDTO>(
                        BancaHelper.Procedure_SP_GET_BANCA_TRANSACCIONES_A_PARTIR_DE_ULTIMO_CUADRE,
                        p,
                        commandType: CommandType.StoredProcedure).ToList();

                    if (!transacciones.Any())
                    {
                        transacciones = null;
                    }
                }

                return transacciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CuadreDTO LeerBancaCuadrePorCuadreId(int cuadreid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); CuadreDTO cuadre = null;

                p.Add("@cuadreid", cuadreid);

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    List<CuadreDTO> cuadres = db.Query<CuadreDTO>(BancaHelper.SelectBancaCuadrePorCuadreId, p, commandType: CommandType.Text).ToList();

                    if (cuadres.Any())
                    {
                        cuadre = cuadres.First();
                    }

                }

                return cuadre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }//fin de clase
}
