using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using Newtonsoft.Json;
using System.Linq;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;

namespace MAR.BusinessLogic.Flujo.WebClient.Code.Validations
{
    public class RutasValidacion
    {
        public List<string> ValidarFormularioRuta(FormularioRuta pFormulario)
        {
            List<string> ColeccionErrores = new List<string>();

            try
            {
                pFormulario.MensajeroAsignado = JsonConvert.DeserializeObject<ConsultaUsuarioBalance>(pFormulario.MensajeroAsignadoJson);           
            }
            catch (Exception e)
            {
                pFormulario.MensajeroAsignado = null;
            }

            pFormulario.NombreRuta = pFormulario.NombreRuta == null ? "" : Regex.Replace(pFormulario.NombreRuta, @"\s+", " ").Trim();

            /** Validaciones de Rutas**/

            /*( 1 )  Mensajero Asignado*/
            if (pFormulario.MensajeroAsignado == null)
            {
                ColeccionErrores.Add(" Error :  asignar mensajero a la ruta.");
            }

            /*( 2 )  Nombre de Ruta*/
            if (pFormulario.NombreRuta.Equals(string.Empty) || pFormulario.NombreRuta.Equals( " " )  )
            {
                ColeccionErrores.Add("Error: el  campo nombre de ruta es requerido.");
            }
            else
            {
                /* ( 2.1 )  Nombre de Ruta fue Registrado Hoy*/
                if ( RutaRepository.EstaRutaFueRegistradaHoy(pFormulario.NombreRuta))
                {
                    ColeccionErrores.Add("Error: el nombre de esta ruta ya fue registrado en el dia hoy. Cambiar nombre");
                }
            }

            try
            {
                pFormulario.Bancas = JsonConvert.DeserializeObject<List<BancaRuta>>(pFormulario.BancasEnRutasJson);
            }
            catch (Exception e)
            {
                pFormulario.Bancas = null;
            }

            /*( 3 ) Bancas en Ruta*/
            if (pFormulario.Bancas == null)
            {
                ColeccionErrores.Add("Error: debe asignar al menos una banca en ruta");
            }

            return ColeccionErrores;
        }

    }
}
