
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.ControlEfectivo
{
    public static class TieLogic
    {
        public static TieDTO LeerTiposAnonimos()
        {
            try
            {
                return TieRepository.LeerTiposAnonimos();
            }
            catch (Exception e)
            {
                throw e;
            }

        }//fin de metodo LeerTiposAnonimos();








    }//fin de clase
}
