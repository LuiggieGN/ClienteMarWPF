using Dapper;
using MAR.AppLogic.MARHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Flujo.DataAccess.Hacienda
{
    public class CertificadoRepository
    {
        public static Certificado GetCertificado()
        {
            try
            {
                string query = @"SELECT Nombre   ,Password   FROM CertificadoExterno";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    var cert = con.Query<Certificado>(query, commandType: CommandType.Text).FirstOrDefault();
                    return cert;
                }
            }
            catch (Exception )
            {
                return new Certificado { Nombre = "", Password = "" };
            }
        }
        public class Certificado
        {
            public string Nombre { get; set; }
            public string Password { get; set; }
        }
    }
}
