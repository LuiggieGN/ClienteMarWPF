
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;
using System;
 

namespace MAR.BusinessLogic.Code.ControlEfectivo
{
    public static class MultipleLogic
    {
        public static MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(string pin)
        {
            try
            {
                return MultipleRepository.LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(pin);
            }
            catch (Exception e)
            {
                throw e;
            }
        }








    }//fin de clase
}
