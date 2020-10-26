using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Flujo.Entities.WpfClient.Enums;
using Flujo.Entities.WpfClient.POCO;



namespace FlujoCustomControl.Helpers
{
    public static class FechaHelper
    {
        public static string  FechaDeHoy(string pIdiomaShortHand = "es" )
        {
            return DateTime.Now.ToString(" dd MMM  hh:mm tt, yyyy ", CultureInfo.CreateSpecificCulture(pIdiomaShortHand));
        }

        public static  List<PeriodoTiempo> DefaultPeriodosDeTiempo()
        {
            List<PeriodoTiempo> laLista = new List<PeriodoTiempo>();

           List<PeriodoTiempoDefaults> p =  EnumHelper.ObtenerEnumElementos<PeriodoTiempoDefaults>().ToList();

           laLista.AddRange(
               
                 p.Select( 
                     x => new PeriodoTiempo {
                           PeriodoDescripcion  =     (  Regex.Replace(  x.ToString() , @"^Periodo|[_]+",  "  ")   ).Trim()  
                         , PeriodoEnum = x
                     }                     
               )   // Resmplazo underscore [ _ ]  por string.Empty y luego trin
                               
           ) ;

            return laLista;

        }
       


    }
}
