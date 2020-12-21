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
    public static class RutaRepository
    {

        public static RutaAsignacionDTO LeerGestorAsignacionPendiente(int gestorUsuarioId, int bancaId)
        {
            try
            {

                var p = new DynamicParameters();
                p.Add("@usuarioid", gestorUsuarioId);
                p.Add("@bancaId", bancaId);


                RutaAsignacionDTO RutaAsignacion = null;

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();

                    RutaAsignacion = db.Query<RutaAsignacionDTO>(RutaHelper.Procedure_SP_GET_GESTOR_ASIGNACION_PENDIENTE, p, commandType: CommandType.StoredProcedure).FirstOrDefault();

                    if (RutaAsignacion != null && RutaAsignacion.OrdenRecorrido != null && RutaAsignacion.OrdenRecorrido != string.Empty)
                    {
                        RutaAsignacion.RutaRecorridoDTO = JSONHelper.CreateNewFromJSONNullValueIgnore<RutaRecorridoDTO>(RutaAsignacion.OrdenRecorrido);
                    }

                    db.Close();
                }

                return RutaAsignacion;
            }
            catch (Exception e)
            {
                throw e;
            }
        }










    }//fin de clase
}
