using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    public class FechaHelper
    {

        /// <param name="fa">Fecha sobre la cual se quiere obtener el dia de la semana</param>
        /// <param name="startOfWeek">Retorna la fecha en la semana equivalente al dia especificado y la fecha suministrada</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(DateTime fa, DayOfWeek startOfWeek)
        {

            //int diff = fa.Date.DayOfWeek - startOfWeek;

            int diff = (7 + (fa.DayOfWeek - startOfWeek)) % 7;
            DateTime ff = fa.AddDays(-1 * diff).Date;
            return ff;
        }

        /// <param name="fa">Fecha la cual suple el mes</param>
        /// <returns> Retorna el 1er. dia del mes de la fecha enviada</returns>
        public static DateTime GetFirstDayOfMonth(DateTime fa)
        {
            DateTime ff = new DateTime(fa.Year, fa.Month, 1).Date;
            return ff;
        }


        public static DateTime GetLastDayOfMonthAndLastTime(DateTime fa)
        {
            if (fa == null)
            {
                return DateTime.MinValue;
            }
            DateTime fb = fa.AddMonths(1);
            DateTime ff = new DateTime(fb.Year, fb.Month, 1).Date.AddSeconds(-1);
            return ff;
        }





    }
}
