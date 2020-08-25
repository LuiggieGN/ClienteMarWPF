using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.AppLogic.MARHelpers
{
    public static class FechaHelper
    {
        public enum FormatoEnum
        {
            FechaBasico = 1,
            FechaHoraBasico = 2,
            FechaCortaDOW = 3,
            FechaLargaDOW = 4,
            FechaCortaRegional = 5,
            FechaLargaRegional = 6,
            HoraCortaRegional = 7,
            HoraLargaRegional = 8,
            FechaCorta = 9
        }

        public static string FormatFecha(System.DateTime Fecha, FormatoEnum Formato)
        {
            string functionReturnValue = null;
            switch ((int)Formato)
            {
                case 1:
                    //Fecha corta universal 2002-12-31
                    functionReturnValue = Fecha.Year + "-" + Fecha.Month + "-" + Fecha.Day;
                    break;
                case 2:
                    //Fecha y hora universal 2002-12-31 11:30:00 pm
                    functionReturnValue = Fecha.Year + "-" + Fecha.Month + "-" + Fecha.Day + " " + Fecha.ToShortTimeString().Replace(".", "");
                    break;
                case 3:
                    //Fecha corta personalizada Lunes, 31-Dic-2002
                    functionReturnValue = FormatDia(Fecha) + ", " + Fecha.Day + "-" + FormatMes(Fecha, true) + "-" + Fecha.Year;
                    break;
                case 4:
                    //Fecha larga personalizada Lunes, 31 de Diciembre de 2002
                    functionReturnValue = FormatDia(Fecha) + ", " + Fecha.Day + "-" + FormatMes(Fecha) + "-" + Fecha.Year;
                    break;
                case 5:
                    //Fecha corta del sistema Regional Setting
                    functionReturnValue = Fecha.ToShortDateString().Replace(".", "");
                    break;
                case 6:
                    //Fecha larga del sistema Regional Setting
                    functionReturnValue = Fecha.ToLongDateString().Replace(".", "");
                    break;
                case 7:
                    //Hora corta del sistema
                    functionReturnValue = Fecha.ToShortTimeString().Replace(".", "");
                    break;
                case 8:
                    //Hora larga del sistema
                    functionReturnValue = Fecha.ToLongTimeString().Replace(".", "");
                    break;
                case 9:
                    //Fecha corta personalizada 31-Dic-2002
                    functionReturnValue = Fecha.Day + "-" + FormatMes(Fecha, true) + "-" + Fecha.Year;
                    break;
                default:
                    //Nada
                    functionReturnValue = "";
                    break;
            }
            return functionReturnValue;
        }

        public static string FormatDia(System.DateTime Fecha)
        {
            string functionReturnValue = null;
            functionReturnValue = string.Empty;
            switch (Fecha.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    functionReturnValue = "Lunes";
                    break;
                case DayOfWeek.Tuesday:
                    functionReturnValue = "Martes";
                    break;
                case DayOfWeek.Wednesday:
                    functionReturnValue = "Miercoles";
                    break;
                case DayOfWeek.Thursday:
                    functionReturnValue = "Jueves";
                    break;
                case DayOfWeek.Friday:
                    functionReturnValue = "Viernes";
                    break;
                case DayOfWeek.Saturday:
                    functionReturnValue = "Sabado";
                    break;
                case DayOfWeek.Sunday:
                    functionReturnValue = "Domingo";
                    break;
            }
            return functionReturnValue;
        }

        public static string FormatMes(System.DateTime Fecha, bool Abreviado = false)
        {
            string functionReturnValue = null;
            functionReturnValue = string.Empty;
            switch (Fecha.Month)
            {
                case 1:
                    functionReturnValue = "Enero";
                    break;
                case 2:
                    functionReturnValue = "Febrero";
                    break;
                case 3:
                    functionReturnValue = "Marzo";
                    break;
                case 4:
                    functionReturnValue = "Abril";
                    break;
                case 5:
                    functionReturnValue = "Mayo";
                    break;
                case 6:
                    functionReturnValue = "Junio";
                    break;
                case 7:
                    functionReturnValue = "Julio";
                    break;
                case 8:
                    functionReturnValue = "Agosto";
                    break;
                case 9:
                    functionReturnValue = "Septiembre";
                    break;
                case 10:
                    functionReturnValue = "Octubre";
                    break;
                case 11:
                    functionReturnValue = "Noviembre";
                    break;
                case 12:
                    functionReturnValue = "Diciembre";
                    break;
            }
            if (Abreviado)
                functionReturnValue = functionReturnValue.Substring(0, 3);
            return functionReturnValue;
        }
        public static DateTime ObtenerFecha(string formato, string fecha)
        {
            DateTime dt; string dtFormat = formato;

            if (!DateTime.TryParseExact(fecha, dtFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt))
            {
                return DateTime.MinValue;
            }
            else
            {
                return dt;
            }
        }
    }
}
