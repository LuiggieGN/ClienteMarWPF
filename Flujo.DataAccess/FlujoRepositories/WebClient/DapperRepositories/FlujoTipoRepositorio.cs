using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using Dapper;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;


using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;
using Flujo.Entities.WebClient.ViewModels;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class FlujoTipoRepositorio
    {

 

        /// <summary>
        ///     Obtengo los |Tipos de Ingresos| definido en la tabla TipoIngreos  
        /// </summary>
        /// <param name="pUsuarioID">Id del usuario para buscar que tipo de usuario es </param>
        /// <returns></returns>

        public static  List<FlujoTipoCategoria> GetTiposDeIngreso( int pUsuarioID )
        {
            try
            {
                List<FlujoTipoCategoria> LaColeccionDeTiposIngreso;

                DynamicParameters p = new DynamicParameters();

                p.Add("@UsuarioID", pUsuarioID);

                string query = @"

                select ti.TipoIngresoID As Posicion, ti.TipoNombre As FlujoTipoNombre, ti.LogicaKey
                
                 from flujo.TipoIngreso ti 
                
                where ti.EsTipoSistema = 0 and ti.TipoIngresoID in (
                
                  select ai.TipoIngresoID from flujo.PermisoTipoUsuarioIngreso ai where ai.TipoUsuarioID = (select top 1 u.TipoUsuarioID from flujo.Usuario u Where u.UsuarioID = @UsuarioID)
                );
             ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<FlujoTipoCategoria> TResult = con.Query<FlujoTipoCategoria>(query, p, commandType: CommandType.Text).ToList();

                    if (TResult.Any())
                    {
                        LaColeccionDeTiposIngreso = TResult;
                    }
                    else
                    {
                        LaColeccionDeTiposIngreso = new List<FlujoTipoCategoria>();
                    }
                }

                return LaColeccionDeTiposIngreso;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        } // Fin GetTiposDeIngresos( ) ~

        public static List<FlujoTipoCategoria> GetTiposDeIngreso( )
        {

            try
            {
                List<FlujoTipoCategoria> LaColeccionDeTiposIngreso;

                string query = @"
               select ti.TipoIngresoID As Posicion, ti.TipoNombre As FlujoTipoNombre, ti.LogicaKey from flujo.TipoIngreso ti where ti.EsTipoSistema = 0;
           ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<FlujoTipoCategoria> TResult = con.Query<FlujoTipoCategoria>(query, commandType: CommandType.Text).ToList();

                    if (TResult.Any())
                    {
                        LaColeccionDeTiposIngreso = TResult;
                    }
                    else
                    {
                        LaColeccionDeTiposIngreso = new List<FlujoTipoCategoria>();
                    }
                }

                return LaColeccionDeTiposIngreso;
            }
            catch (Exception ex)
            {

                throw ex;
            }
 
        } // Fin GetTiposDeIngresos( ) ~

        /// <summary>
        ///     Obtengo lso |Tipos de Egresos | definido en la tabla TipoEgresos
        /// </summary>
        /// <param name="pUsuarioID">Id del usuario para buscar que tipo deusuario es</param>
        /// <returns></returns>
        public static  List<FlujoTipoCategoria> GetTiposDeEgresos(int pUsuarioID )
        {

            try
            {
                List<FlujoTipoCategoria> LaColeccionDeTiposIngreso;

                string query = @"
                      select te.TipoEgresoID As Posicion , te.TipoNombre As FlujoTipoNombre, te.LogicaKey  
                      from flujo.TipoEgreso te
                      where te.EsTipoSistema = 0 and te.TipoEgresoID in(
                      
                        select ae.TipoEgresoID from flujo.PermisoTipoUsuarioEgreso ae where ae.TipoUsuarioID = (select top 1 u.TipoUsuarioID from flujo.Usuario u Where u.UsuarioID = @UsuarioID)
                      )
                ";

                DynamicParameters p = new DynamicParameters();

                p.Add("@UsuarioID", pUsuarioID);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<FlujoTipoCategoria> TResult = con.Query<FlujoTipoCategoria>(query, p, commandType: CommandType.Text).ToList();

                    if (TResult.Any())
                    {
                        LaColeccionDeTiposIngreso = TResult;
                    }
                    else
                    {
                        LaColeccionDeTiposIngreso = new List<FlujoTipoCategoria>();
                    }
                }

                return LaColeccionDeTiposIngreso;
            }
            catch (Exception ex)
            {

                throw ex;
            } 
 
        }//Fin GetTiposDeEgresos( )~


        /// <summary>
        ///     Obtengo lso |Tipos de Egresos | definido en la tabla TipoEgresos
        /// </summary>
        /// <param name="pUsuarioID">Id del usuario para buscar que tipo deusuario es</param>
        /// <returns></returns>
        public static List<FlujoTipoCategoria> GetTiposDeEgresos( )
        {
            try
            {
                List<FlujoTipoCategoria> LaColeccionDeTiposIngreso;

                string query = @"
                    select te.TipoEgresoID As Posicion , te.TipoNombre As FlujoTipoNombre, te.LogicaKey from flujo.TipoEgreso te where te.EsTipoSistema = 0;
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<FlujoTipoCategoria> TResult = con.Query<FlujoTipoCategoria>(query, commandType: CommandType.Text).ToList();

                    if (TResult.Any())
                    {
                        LaColeccionDeTiposIngreso = TResult;
                    }
                    else
                    {
                        LaColeccionDeTiposIngreso = new List<FlujoTipoCategoria>();
                    }
                }

                return LaColeccionDeTiposIngreso;
            }
            catch (Exception ex)
            {

                throw ex;
            }
 
        }//Fin GetTiposDeEgresos( )~	



    }// Fin de Clase FlujoTipoRepositorio

}// Fin de Namespace 
