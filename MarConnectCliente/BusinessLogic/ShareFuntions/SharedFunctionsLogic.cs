using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.BusinessLogic.ShareFuntions
{
   public  class SharedFunctionsLogic
    {
          public static DateTime ConvertirATipoFechaDIAOPERACION(string _fecha)
        {
            string _formato = "yyyy-MM-dd";    return FechaHelper.ObtenerFecha(_formato, _fecha);
        }

        public static DateTime ConvertirATipoFechaFECHASOLICITUD(string _fecha)
        {
            string _formato = "yyyy-MM-dd HH:mm:ss";    return FechaHelper.ObtenerFecha(_formato, _fecha);
        }

        
    }
}
