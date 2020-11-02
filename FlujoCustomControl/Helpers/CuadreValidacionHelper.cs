using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlujoCustomControl.Helpers
{
    public static class CuadreValidacionHelper
    {
        // True : Si se permite retirar la cantidad especificada
        // False: No se permite retirar la cantidad especificada
        public static bool SePermiteRetirarMonto(decimal pMontoContado, decimal pMontoARetirar)
        {
            decimal excedente = pMontoContado - pMontoARetirar;

            if (
                  excedente >= 0
               )
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
