using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Admin.SharedFuntions
{
    public class Autorizaciones
    {
        public static bool AutorizaCorreccionPremios(string pInput)
        {
            int anio = DateTime.Now.Year;
            int mes = DateTime.Now.Month;
            int dia = DateTime.Now.Day;

            string codigo = (((anio + mes + dia) * 29) * 321).ToString();
            codigo = codigo.Substring(codigo.Length - 5, 5);
            if (pInput == codigo)
            {
                return true;
            }

            return false;
        }
    }
}
