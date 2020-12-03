
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

using System;
 

namespace MAR.BusinessLogic.Code.ControlEfectivo
{
    public static class CajaLogic
    {

        public static SupraMovimientoEnBancaResultDTO RegistrarMovimientoEnBanca(string jsonMovimiento)
        {
            try
            {
                var movimiento = JSONHelper.CreateNewFromJSONNullValueIgnore<SupraMovimientoEnBancaDTO>(jsonMovimiento);
                
                return CajaRepository.RegistrarMovimientoEnBanca(movimiento);
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
                return CajaRepository.LeerCajaBalance(cajaid);
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
                return CajaRepository.LeerCajaDeUsuarioPorUsuarioId(usuarioid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
 





    }//fin de clase
}
