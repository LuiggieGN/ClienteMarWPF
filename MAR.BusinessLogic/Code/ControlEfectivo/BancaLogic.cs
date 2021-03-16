
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.ControlEfectivo
{
    public static class BancaLogic
    {
        public static BancaConfiguracionDTO LeerBancaConfiguraciones(int bancaid)
        {
            try
            {
                return BancaRepository.LeerBancaConfiguraciones(bancaid);
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
                return BancaRepository.LeerBancaLastCuadreId(bancaid);
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
                return BancaRepository.LeerBancaLastTransaccionesApartirDelUltimoCuadre(bancaid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static CuadreDTO LeerBancaCuadrePorCuadreId(int cuadreid)
        {
            try
            {
                return BancaRepository.LeerBancaCuadrePorCuadreId(cuadreid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool BancaUsaControlEfectivo(int bancaid, bool incluyeConfig)
        {
            try
            {
                return BancaRepository.BancaUsaControlEfectivo(bancaid, incluyeConfig);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal LeerDeudaDeBanca(int bancaid)
        {
            try
            {
                return BancaRepository.LeerDeudaDeBanca(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static int LeerBancaInactividad(int bancaid)
        {
            try
            {
                return BancaRepository.LeerBancaInactividad(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }//fin de clase
}
