using Dapper;

using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.ControlEfectivoRepositories.Helpers;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

namespace MAR.DataAccess.ControlEfectivoRepositories
{
    public static class TieRepository
    {
        public static TieDTO LeerTiposAnonimos()
        {
            try
            {    
                var anonimos = new TieDTO();

                using (var db = DALHelper.GetSqlConnection())
                {
                    db.Open();
                    anonimos.TiposIngresosQueSonAnonimo = db.Query<TipoAnonimoDTO>(TieHelper.SelectTiposIngresosAnonimos, commandType: CommandType.Text).ToList();
                    anonimos.TiposEgresosQueSonAnonimo = db.Query<TipoAnonimoDTO>(TieHelper.SelectTiposEgresosAnonimos, commandType: CommandType.Text).ToList();
                    db.Close();
                }

                return anonimos;
            }
            catch (Exception e)
            {
                throw e;
            }

        }// fin de metodo LeerTiposAnonimos( )







    }//fin de clase
}
