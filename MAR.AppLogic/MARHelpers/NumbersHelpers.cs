using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.AppLogic.MARHelpers
{
    public static class NumbersHelpers
    {
        public enum FormatoEnum
        {
            DecimalSinComa = 1,
        }
        public static decimal FormatNumero(System.Decimal pDecimal, FormatoEnum pFormato)
        {
            decimal functionReturnValue;
            switch ((int)pFormato)
            {
                case 1:
                    //Decimal sin comma 1500.45
                    functionReturnValue = Convert.ToDecimal(pDecimal.ToString("#,##0.00"));
                    break;
                default:
                    //Nada
                    functionReturnValue = pDecimal;
                    break;
            }
            return functionReturnValue;
        }
    }
}
