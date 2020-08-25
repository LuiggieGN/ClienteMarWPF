using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using MAR.DataAccess.Tables.DTOs;
using System.Data.Entity.Migrations;
using MAR.AppLogic.MARHelpers;
using Dapper;

namespace MAR.DataAccess.EFRepositories
{
    public class BancasRepository
    {
       

        public static InicioCierreRecargasBanca GetInicioCierreRecarga(int pBancaId)
        {
            IEnumerable<MBancasConfig> config = new List<MBancasConfig>();
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@BancaID", pBancaId);
                    string query = $@"Select * from MBancasConfig where BancaID = @BancaID AND ConfigKey IN ('BANCA_HORA_RECARGAS_INICIO') 
                        and Activo = 1 ORDER BY CONFIGKEY DESC;

                Select * from MBancasConfig where BancaID = @BancaID AND ConfigKey IN ('BANCA_HORA_RECARGAS_CIERRE') 
                        and Activo = 1 ORDER BY CONFIGKEY DESC";

                    using (var multi = con.QueryMultiple(query, p))
                    {
                        var inicio = multi.Read<MBancasConfig>().FirstOrDefault();
                        var cierre = multi.Read<MBancasConfig>().FirstOrDefault();
                        if (inicio != null && cierre != null)
                        {
                            //            Dim horaInicioRecarga = DateTime.Parse(FormatFecha(CDate(DateTime.Now.ToShortDateString()), 1) & " " & inicioR)
                            //Dim horaCierreRecarga = DateTime.Parse(FormatFecha(CDate(DateTime.Now.ToShortDateString()), 1) & " " & cierreR)


                            var inicioRecarga = DateTime.Parse(FechaHelper.FormatFecha(DateTime.Now, FechaHelper.FormatoEnum.FechaBasico) + " "+ inicio.ConfigValue);
                            var cierreRecarga = DateTime.Parse(FechaHelper.FormatFecha(DateTime.Now, FechaHelper.FormatoEnum.FechaBasico) + " "+ cierre.ConfigValue);
                            return new InicioCierreRecargasBanca { Inicio = inicioRecarga, Cierre = cierreRecarga };
                        }
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                return null;
            }
          
        }
        public class InicioCierreRecargasBanca
        {
            public DateTime Inicio { get; set; }
            public DateTime Cierre { get; set; }
        }
        public static IEnumerable<MBancasConfig> GetBancaConfig()
        {
            IEnumerable<MBancasConfig> config = new List<MBancasConfig>();

            using (var con = DALHelper.GetSqlConnection())
            {

                string query = "select * from MBancasConfig";

                config = con.Query<MBancasConfig>(query).ToList();

            }

            return config;

        }
        public static IEnumerable<MBanca> GetBanca(Expression<Func<MBanca, bool>> filter = null, Func<IQueryable<MBanca>, IOrderedQueryable<MBanca>> orderBy = null,
           string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<MBanca> query = db.MBancas;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            query = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public static void AgragaOActualizaBacanConfigRecord(int pBancaId, string pConfigKey, string pConfigValue, bool pActivo)
        {
            var db = new MARContext();
            var bancaConfig = new MBancasConfig
            {
                BancaID = pBancaId,
                ConfigKey = pConfigKey,
                ConfigValue = pConfigValue,
                Activo = pActivo
            };
            db.MBancasConfigs.AddOrUpdate(p => new { p.BancaID, p.ConfigKey }, bancaConfig);
            db.SaveChanges();
        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public BancasRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
