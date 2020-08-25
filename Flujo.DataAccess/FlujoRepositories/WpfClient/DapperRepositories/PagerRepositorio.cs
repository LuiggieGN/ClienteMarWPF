using System;
using System.Data;
using System.Linq;
using Dapper;

using System.Collections.ObjectModel;
using System.Collections.Generic;

using Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers;
using MAR.AppLogic.MARHelpers;

namespace Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories
{
    public static class PagerRepositorio
    {
        public static  ObservableCollection<T> GetQueryPagingResult<T>
                (

                   int         pStart,
                   int         pItemCount,
                   string    pSortColumn,
                   bool       pIsAscending,
                   out int   pTotalOfItems,
                   string    query, 
                   DynamicParameters parametros 

               ) where  T : class
        {

            try
            {
                List<T> ListSelectElementos;

                ObservableCollection<T> ColeccionElementosEnPagina = new ObservableCollection<T>();

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    if (parametros != null)
                    {
                        ListSelectElementos = con.Query<T>(query, parametros, commandType: CommandType.Text).ToList();
                    }
                    else
                    {
                        ListSelectElementos = con.Query<T>(query, commandType: CommandType.Text).ToList();
                    }

                    if (ListSelectElementos.Any())
                    {
                        List<T> ListOrdenado = ListSelectElementos.OrderBy(pSortColumn, pIsAscending).ToList();

                        IEnumerable<T> SkippedSequence = ListOrdenado.Skip(  pStart  ).Take(pItemCount);

                        foreach (var item in SkippedSequence)
                        {
                            ColeccionElementosEnPagina.Add(item);
                        }
                    }

                    pTotalOfItems = ListSelectElementos.Count;

                    con.Close();
                }

                return ColeccionElementosEnPagina;

            }
            catch (Exception ex)
            {
                pTotalOfItems = 0;

                return new ObservableCollection<T>();
            }

        }//Fin GetQueryPagingResult<T>( )

        public class PagingResult<T> where T:class
        {
            public int TotalPages { get; set; }
            public  ObservableCollection<T> Listado { get; set; }
        }
        public static PagingResult<T> GetProcedurePagingResult<T>
                (

                   int pStart,
                   int pItemCount,
                   string pSortColumn,
                   bool pIsAscending,
                   string storeProcedure,
                   Dictionary<string, object> parameters

               ) where T : class
        {
            PagingResult<T> pageResult = new PagingResult<T>();
            try
            {
                List<T> ListSelectElementos;

                ObservableCollection<T> ColeccionElementosEnPagina = new ObservableCollection<T>();
                DynamicParameters parametros = null;
                if (parameters != null && parameters.Count > 0)
                {
                    parametros= new DynamicParameters();
                    foreach (KeyValuePair<string,object> item in parameters)
                    {
                        parametros.Add(item.Key.Replace("{[", ""),item.Value.ToString().Replace("]}",""));
                    }
                }
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    if (parametros != null)
                    {
                        ListSelectElementos = con.Query<T>(storeProcedure, parametros, commandType: CommandType.StoredProcedure).ToList();
                    }
                    else
                    {
                        ListSelectElementos = con.Query<T>(storeProcedure, commandType: CommandType.StoredProcedure).ToList();
                    }

                    if (ListSelectElementos.Any())
                    {
                        //List<T> ListOrdenado = ListSelectElementos.OrderBy(pSortColumn, pIsAscending).ToList();

                        IEnumerable<T> SkippedSequence = ListSelectElementos.Skip(pStart).Take(pItemCount);

                        foreach (var item in SkippedSequence)
                        {
                            ColeccionElementosEnPagina.Add(item);
                        }
                    }

                    pageResult.Listado = ColeccionElementosEnPagina;
                    pageResult.TotalPages = ListSelectElementos.Count;

                    con.Close();
                }

                return pageResult;

            }
            catch (Exception ex)
            {
                pageResult.TotalPages = 0;
                pageResult.Listado = new ObservableCollection<T>();
                return pageResult;
            }

        }//Fin GetQueryPagingResult<T>( )

    }//Fin Class PagerRepositorio

}// ~
