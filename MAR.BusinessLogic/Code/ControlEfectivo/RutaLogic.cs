
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

using System;
using System.Collections.Generic;

namespace MAR.BusinessLogic.Code.ControlEfectivo
{
    public static class RutaLogic
    {

        public static RutaAsignacionDTO LeerGestorAsignacionPendiente(int gestorUsuarioId, int bancaId)
        {
            try
            {
                return RutaRepository.LeerGestorAsignacionPendiente(gestorUsuarioId, bancaId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }








    }//fin de clase
}
