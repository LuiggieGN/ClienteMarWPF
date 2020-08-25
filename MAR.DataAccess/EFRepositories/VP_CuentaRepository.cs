using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using MAR.AppLogic.MARHelpers;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.Tables.Enums;

namespace MAR.DataAccess.EFRepositories
{

    public class VpCuentaRepository
    {
        public static VpCuetaValues GetVpCuenta(int pRiferoId, DbEnums.Productos pProductoLogicaKey)
        {
            var db = new Tables.DTOs.MARContext();
            var values = new VpCuetaValues();

            var cuentas = db.VP_Producto.Include(x => x.VP_CuentaTipo).Where(x => x.LogicaKey == (int)pProductoLogicaKey);
            //.Include(x => x.VP_CuentaTipo.VP_Cuenta)
            //.Include(x => x.VP_CuentaTipo.VP_Cuenta).Include(x => x.VP_CuentaTipo.VP_Cuenta.Select(c => c.VP_CuentaConfig)).Include(x => x.VP_CuentaTipo.VP_Cuenta.Select(t => t.VP_Suplidor));

            //backup del codigo de arriba por si no funciona
            //var cuentas =
            //db.VP_Producto.Include(x => x.VP_CuentaTipo)
            //  .Include(x => x.VP_CuentaTipo.VP_Cuenta)
            //  .Where(x => x.LogicaKey == (int)pProductoLogicaKey).Include(x => x.VP_CuentaTipo.VP_Cuenta).Include(x => x.VP_CuentaTipo.VP_Cuenta.Select(c => c.VP_CuentaConfig)).Include(x => x.VP_CuentaTipo.VP_Cuenta.Select(t => t.VP_Suplidor));

            var cuentaConfigs = cuentas.SelectMany(x => x.VP_CuentaTipo.VP_Cuenta).SelectMany(x => x.VP_CuentaConfig).Where(x => x.ConfigKey == DbEnums.CuentaConfig.RiferoID.ToString() && x.ConfigValue == pRiferoId.ToString());
            
            if (!cuentaConfigs.Any())
            {
                cuentaConfigs = cuentas.SelectMany(x => x.VP_CuentaTipo.VP_Cuenta).SelectMany(x => x.VP_CuentaConfig).Where(t => t.ConfigKey == DbEnums.CuentaConfig.CuentaDefault.ToString() && t.ConfigValue == "True");
            }
            values.ProductoId = cuentas.Select(x => x.ProductoID).FirstOrDefault();
            var cuenta = cuentas.Select(x=>x).SelectMany(x => x.VP_CuentaTipo.VP_Cuenta).Include(x => x.VP_CuentaTipo.VP_Cuenta.Select(t => t.VP_Suplidor)).Include(x=>x.VP_CuentaConfig).FirstOrDefault(x => x.CuentaID == cuentaConfigs.FirstOrDefault().CuentaID);
            values.VpCuenta = cuenta;
            return values;
        }
        public static VP_Cuenta GetVpCuentaNew(int pRiferoId, DbEnums.Productos pProductoLogicaKey)
        {

            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT c.*  FROM VP_Cuenta C JOIN VP_CuentaTipo CT ON CT.CuentaTipoID = C.CuentaTipoID JOIN VP_CuentaConfig
                                  cf on cf.CuentaID = c.CuentaID and (cf.ConfigKey = 'CuentaDefault' AND CF.ConfigValue = 'TRUE' OR CF.ConfigKey = 'RiferoID' AND CF.ConfigValue = @RiferoID) WHERE CT.Nombre = @Nombre";

                    string riferoId = pRiferoId.ToString();
                    var p = new DynamicParameters();
                    p.Add("@Nombre", pProductoLogicaKey.ToString());
                    p.Add("@RiferoID", riferoId);
                    var cuenta = con.QueryFirst<VP_Cuenta>(query, param: p,commandType: CommandType.Text);

                    return cuenta;
                }

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static double GetBalanceCuentaMAR(int pRiferoId, DbEnums.Productos pProductoLogicaKey)
        {

            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"SELECT ConfigValue FROM VP_CuentaConfig cf  where  ConfigKey = 'BalanceMAR'";


                    double cuenta = con.QueryFirst<double>(query,commandType: CommandType.Text);

                    return cuenta;
                }

            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static double UpdateBalanceCuentaMAR(double pMonto)
        {

            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"UPDATE VP_CuentaConfig SET  ConfigValue = ConfigValue + " + pMonto +" WHERE ConfigKey = 'BalanceMAR'";


                    double cuenta = con.QueryFirst<double>(query,commandType: CommandType.Text);

                    return cuenta;
                }

            }
            catch (Exception e)
            {
                return 0;
            }
        }
        public class VpCuetaValues
        {
            public int  ProductoId { get; set; }
            public VP_Cuenta VpCuenta { get; set; }
        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public VpCuentaRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
