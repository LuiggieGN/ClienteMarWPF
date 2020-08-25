using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;

using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class PagingRepository<T>
    {
        public static List<T> GetAllRecords(string pSourceQuery, DynamicParameters p = null)
        {
            try
            {
                List<T> LaColeccionDeRecords;

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    if (p == null)
                    {
                        LaColeccionDeRecords = con.Query<T>(pSourceQuery, commandType: CommandType.Text).ToList();
                    }
                    else
                    {
                        LaColeccionDeRecords = con.Query<T>(pSourceQuery, p, commandType: CommandType.Text).ToList();
                    }

                }//Fin stament<Using>

                return LaColeccionDeRecords;

            }
            catch (Exception ex)
            {
                return new List<T>();
            }

        }//Fin de Metodo : GetAllRecords( ) 

        public static List<T> GetAllRecordsFromProcedure(string pStoredProcedureName, DynamicParameters p = null)
        {
            try
            {
                List<T> LaColeccionDeRecords;

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    if (p == null)
                    {
                        LaColeccionDeRecords = con.Query<T>(pStoredProcedureName, commandType: CommandType.StoredProcedure).ToList();
                    }
                    else
                    {
                        LaColeccionDeRecords = con.Query<T>(pStoredProcedureName, p, commandType: CommandType.StoredProcedure).ToList();
                    }

                }//Fin stament<Using>

                return LaColeccionDeRecords;

            }
            catch (Exception ex)
            {
                return new List<T>();
            }

        }//Fin de Metodo : GetAllRecords( ) 



        public static List<T> GetAllRecordsFromProcedure(string pStoredProcedureName, Dictionary<string, object> pParams, out int pTotalRecordsAPaginar)
        {
            pTotalRecordsAPaginar = 0;

            try
            {

                List<T> LaColeccionDeRecords; DynamicParameters p = (pParams == null || pParams.Count == 0) ? null : new DynamicParameters();


                if (pParams != null && pParams.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in pParams)
                    {
                        p.Add($"{item.Key}", item.Value);
                    }
                }

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    if (p == null)
                    {

                        using (var multi = con.QueryMultiple(pStoredProcedureName, commandType: CommandType.StoredProcedure))
                        {
                            pTotalRecordsAPaginar = multi.Read<int>().First();
                            LaColeccionDeRecords = multi.Read<T>().ToList();
                        }

                    }
                    else
                    {
                        using (var multi = con.QueryMultiple(pStoredProcedureName, p, commandType: CommandType.StoredProcedure))
                        {
                            pTotalRecordsAPaginar = multi.Read<int>().First();
                            LaColeccionDeRecords = multi.Read<T>().ToList();
                        }
                    }

                }//Fin stament<Using>

                return LaColeccionDeRecords;

            }
            catch (Exception ex)
            {
                return new List<T>();
            }

        }//Fin de Metodo : GetAllRecordsFromProcedure( ) 


        public static List<T> GetAllRecordsFromQuery(string pQuery, Dictionary<string, object> pParams, out int pTotalRecordsAPaginar)
        {
            pTotalRecordsAPaginar = 0;

            try
            {

                List<T> LaColeccionDeRecords; DynamicParameters p = (pParams == null || pParams.Count == 0) ? null : new DynamicParameters();


                if (pParams != null && pParams.Count > 0)
                {
                    foreach (KeyValuePair<string, object> item in pParams)
                    {
                        p.Add($"{item.Key}", item.Value);
                    }
                }

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    if (p == null)
                    {

                        using (var multi = con.QueryMultiple(pQuery, commandType: CommandType.Text))
                        {
                            pTotalRecordsAPaginar = multi.Read<int>().First();
                            LaColeccionDeRecords = multi.Read<T>().ToList();
                        }

                    }
                    else
                    {
                        using (var multi = con.QueryMultiple(pQuery, p, commandType: CommandType.Text))
                        {
                            pTotalRecordsAPaginar = multi.Read<int>().First();
                            LaColeccionDeRecords = multi.Read<T>().ToList();
                        }
                    }

                }//Fin stament<Using>

                return LaColeccionDeRecords;

            }
            catch (Exception ex)
            {
                return new List<T>();
            }

        }//Fin de Metodo : GetAllRecordsFromProcedure( ) 






    }// Fin de (Clase) PagingRepository<T>

}//Fin de (Namespace) DapperRepositories 