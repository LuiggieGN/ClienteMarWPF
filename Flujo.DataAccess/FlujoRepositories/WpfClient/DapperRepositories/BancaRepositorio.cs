using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.POCO;
using MAR.AppLogic.MARHelpers;

namespace Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories
{
    public static class BancaRepositorio
    {
        public static Banca GetBanca(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); Banca banca = null;

                p.Add("@BancaID", pBancaID);

                string query = @"select b.BancaID, b.BanNombre, b.BanContacto, b.BanDireccion, b.BanTelefono from MBancas b where b.BancaID =  @BancaID;";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<Banca> TheListOfBanca = con.Query<Banca>(query, p, commandType: CommandType.Text).ToList();

                    if (TheListOfBanca.Any())
                    {
                        banca = TheListOfBanca.First();
                    }
                }

                return banca;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }// Fin BancaRepositorio ~
}
