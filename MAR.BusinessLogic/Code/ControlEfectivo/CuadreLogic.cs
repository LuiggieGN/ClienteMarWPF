
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.ControlEfectivo
{
    public static class CuadreLogic
    {
        public static CuadreRegistroResultDTO Registrar(string jsonCuadre, bool esUnRetiro)
        {
            try
            {
                CuadreDTO cuadre = JSONHelper.CreateNewFromJSONNullValueIgnore<CuadreDTO>(jsonCuadre);

                return CuadreRepository.Registrar(cuadre, esUnRetiro);
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
                return CuadreRepository.EnlazarRutaConCuadre(rutaEstado,rutaUltimaLocalidad,rutaOrdenRecorrido,cuadreId, bancaCajaId, rutaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




    }//fin de clase
}
