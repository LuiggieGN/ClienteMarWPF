using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;

using Dapper;

using Flujo.Entities.WebClient.POCO;
using Flujo.Entities.WebClient.Enums;

using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;

using System.Text.RegularExpressions;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
   public static class RutaRepository
    {
        public static bool EstaRutaFueRegistradaHoy(string pNombreRuta)
        {
            bool FueRegistrada = true;

            DynamicParameters p = new DynamicParameters();   p.Add("@NombreRuta", pNombreRuta);

            string query = @"
                    
                     Select 
                       r.RutaID From flujo.Ruta r
                     Where
                           Convert(date,r.FechaCreacion) =  Convert(date,getdate())
                       And r.Nombre = @NombreRuta 
                       And r.Activa = 1;
            ";            

            try
            {
                using (var con  =  DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<int> IdsNombresRutasIgualesDeHoy = con.Query<int>(query, p, commandType: CommandType.Text).ToList();

                    if (! IdsNombresRutasIgualesDeHoy.Any())
                    {
                        FueRegistrada = false;
                    }
                }

                return FueRegistrada;                
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public static bool CrearNuevaRuta(FormularioRuta pFormulario)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();  bool RecordFueInsertado = false;

                string BancasEnRuta = pFormulario.BancasEnRutasJson.Replace("[", "").Replace("]", "").Replace("Posicion", "BancaID");
                string Comentario = Regex.Replace(pFormulario.Comentario ?? "", @"\s+", " ").Trim();

                p.Add("@NombreRuta", pFormulario.NombreRuta);
                p.Add("@BancasEnRuta", BancasEnRuta);
                p.Add("@Mensajero", pFormulario.MensajeroAsignado.Posicion);
                p.Add("@Comentario", Comentario);

                string query = @"

                       insert into flujo.Ruta
                                  (
                       		          ZonaID
                                     ,TipoRutaID
                                     ,Nombre
                                     ,OrdenRecorrido
                                     ,UsuarioID
                                     ,FechaInicio
                                     ,UltimaLocalidad
                                     ,Estado
                                     ,Nota
                                     ,FechaCreacion
                                     ,Activa
                       		   )
                            values
                                  (
                       	    	    -1
                                  , -1
                                  , @NombreRuta
                                  , @BancasEnRuta
                                  , @Mensajero
                                  , getdate()
                                  , null
                                  , 'Activa'
                                  , @Comentario
                                  , getdate()
                                  , 1
                       		   );
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    int affected = con.Execute(query, p,  commandType: CommandType.Text);

                    if (affected > 0)
                    {
                        RecordFueInsertado = true;
                    }

                }

                return RecordFueInsertado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}
