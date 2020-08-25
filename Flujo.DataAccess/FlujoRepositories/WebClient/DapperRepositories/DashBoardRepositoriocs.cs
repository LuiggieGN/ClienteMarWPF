using System;
using System.Linq;
using Dapper;
using System.Data;
using System.Collections.Generic;
using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;

using Flujo.Entities.WebClient.POCO;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public class DashBoardRepositoriocs
    {
        public static EstadoConsorcio GetConsorcioEstado(int? RiferoID)
        {
            try
            {

                EstadoConsorcio estado = null;
                DynamicParameters p = new DynamicParameters();
                p.Add("@RifereroID", RiferoID);


                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<EstadoConsorcio> EstadoSelect = con.Query<EstadoConsorcio>("flujo.Sp_GetConsorcioEstado", p, commandType: CommandType.StoredProcedure).ToList();

                    if (EstadoSelect.Any())
                    {
                        estado = EstadoSelect.First();
                    }
                }

                return estado;

            }
            catch (Exception ex)
            {
                return null;
            }
        }




    }
}