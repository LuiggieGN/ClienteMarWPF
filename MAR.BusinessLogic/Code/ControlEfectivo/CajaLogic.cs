
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

using System;
using System.Collections.Generic;

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

        public static SupraMovimientoDesdeHastaResultDTO RegistrarTransferencia(string jsonTransferencia)
        {
            try
            {
                var transferencia = JSONHelper.CreateNewFromJSONNullValueIgnore<SupraMovimientoDesdeHastaDTO>(jsonTransferencia);

                return CajaRepository.RegistrarTransferencia(transferencia);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MultipleDTO<PagerResumenDTO, List<MovimientoDTO>> LeerMovimientos(string jsonPaginaRequest)
        {
            try
            {

                var paginaRequest = JSONHelper.CreateNewFromJSONNullValueIgnore<MovimientoPageDTO>(jsonPaginaRequest);

                return CajaRepository.LeerMovimientos(paginaRequest);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<MovimientoDTO> LeerMovimientosNoPaginados(string jsonPaginaRequest)
        {
            try
            {
                var paginaRequest = JSONHelper.CreateNewFromJSONNullValueIgnore<MovimientoPageDTO>(jsonPaginaRequest);

                return CajaRepository.LeerMovimientosNoPaginados(paginaRequest);

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

        public static decimal LeerCajaBalanceMinimo(int cajaid)
        {
            try
            {
                return CajaRepository.LeerCajaBalanceMinimo(cajaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool SetearCajaDisponibilidad(string jsonCajaDisponibilidad)
        {
            try
            {
                var cajadisponilidad = JSONHelper.CreateNewFromJSONNullValueIgnore<CajaDisponibilidadDTO>(jsonCajaDisponibilidad);

                return CajaRepository.SetearCajaDisponibilidad(cajadisponilidad);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool LeerCajaExiste(int cajaid)
        {
            try
            {
                return CajaRepository.LeerCajaExiste(cajaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }//fin de clase
}
