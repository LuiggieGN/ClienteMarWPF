using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using MAR.AppLogic.MARHelpers;
using Dapper;
using System.Data;
using System.Data.Entity.Migrations;

namespace MAR.DataAccess.EFRepositories
{

    public class ProductosConfigRepository
    {
        public static void SetLimiteMinutosReImpresion(int pLimiteMinutos)
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x => x.Opcion == Tables.Enums.DbEnums.ProductosConfig.LIMITE_MINUTOS_REIMPRESION.ToString());
            if (query.Any())
            {
                query.FirstOrDefault().Valor = pLimiteMinutos.ToString();
            }
            else
            {
                var maxId = (from p in db.SWebProductoConfigs select p.WebProductoConfigID).Max();
                var prodConfig = new SWebProductoConfig()
                {
                    WebProductoConfigID = maxId + 1,
                    WebProductoID = 0,
                    Opcion = Tables.Enums.DbEnums.ProductosConfig.LIMITE_MINUTOS_REIMPRESION.ToString(),
                    Modo = 0,
                    Activo = true,
                    Valor = "5"
                };
                db.SWebProductoConfigs.Add(prodConfig);
            }
            db.SaveChanges();
        }

        public static int GetLimiteMinutosReImpresion()
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x => x.Opcion == Tables.Enums.DbEnums.ProductosConfig.LIMITE_MINUTOS_REIMPRESION.ToString());
            if (query.Any())
            {
                return int.Parse(query.FirstOrDefault().Valor);
            }
            else
            {
                var maxId = (from p in db.SWebProductoConfigs select p.WebProductoConfigID).Max();
                var prodConfig = new SWebProductoConfig()
                {
                    WebProductoConfigID = maxId + 1,
                    WebProductoID = 0,
                    Opcion = Tables.Enums.DbEnums.ProductosConfig.LIMITE_MINUTOS_REIMPRESION.ToString(),
                    Modo = 0,
                    Activo = true,
                    Valor = "5"
                };
                db.SWebProductoConfigs.Add(prodConfig);
                db.SaveChanges();
                return int.Parse(prodConfig.Valor);
            }
        }

        public static bool GetRecargasEncendidas()
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x => x.Opcion == Tables.Enums.DbEnums.ProductosConfig.VENTA_RECARGAS_ENCENDIDA.ToString());
            if (query.Any())
            {
                return query.FirstOrDefault().Activo;
            }
            else
            {
                SetRecargasEncendidas(true);
                return true;
            }
        }
        public static void SetRecargasEncendidas(bool Encendido)
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x => x.Opcion == Tables.Enums.DbEnums.ProductosConfig.VENTA_RECARGAS_ENCENDIDA.ToString());
            if (query.Any())
            {
                query.FirstOrDefault().Activo = Encendido;
            }
            else
            {
                var maxId = (from p in db.SWebProductoConfigs select p.WebProductoConfigID).Max();
                var prodConfig = new SWebProductoConfig()
                {
                    WebProductoConfigID = maxId + 1,
                    WebProductoID = 0,
                    Opcion = Tables.Enums.DbEnums.ProductosConfig.VENTA_RECARGAS_ENCENDIDA.ToString(),
                    Modo = 0,
                    Activo = true,
                    Valor = "Recargas.Activo"
                };
                db.SWebProductoConfigs.Add(prodConfig);
            }
            db.SaveChanges();
        }
        public static void SetHoraInicioCierreRegargas(string pInicio, string pCierre)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {

                    string query = $@"delete from SWebProductoConfig where Opcion = 'HORA_RECARGAS_INICIO' or Opcion = 'HORA_RECARGAS_CIERRE'

                                        IF (len(@Cierre) > 0 and len(@Inicio) > 0)
                                        BEGIN
                                        INSERT INTO SWebProductoConfig
                                                   (WebProductoConfigID
                                                   ,WebProductoID
                                                   ,Opcion
                                                   ,Valor
                                                   ,Modo
                                                   ,Activo)
                                             VALUES
                                                   ((select top 1 max(WebProductoConfigID) + 1 from SWebProductoConfig )
                                                   ,0
                                                   ,'HORA_RECARGAS_INICIO'
                                                   ,@Inicio
                                                   ,0
                                                   ,1)
                                        INSERT INTO SWebProductoConfig
                                                   (WebProductoConfigID
                                                   ,WebProductoID
                                                   ,Opcion
                                                   ,Valor
                                                   ,Modo
                                                   ,Activo)
                                             VALUES
                                                   ((select top 1 max(WebProductoConfigID) + 1 from SWebProductoConfig )
                                                   ,0
                                                   ,'HORA_RECARGAS_CIERRE'
                                                   ,@Cierre
                                                   ,0
                                                   ,1)
                                        END";

                    var p = new DynamicParameters();
                    p.Add("@Inicio", pInicio);
                    p.Add("@Cierre", pCierre);
                    con.Query(query, p, commandType: CommandType.Text);
                    
                }
            }
            catch (Exception e)
            {
                string t = e.Message ;
            }
        }
        public static bool PuedePagarRemoto(int pBancaId, int pBancaIdApagar)
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@BancaIdPaga", pBancaId);
                    p.Add("@BancaIdRecibePago", pBancaIdApagar);
                    var pagoRemoto = con.QueryFirst<bool>("ValidaPagoRemoto", p, commandType: CommandType.StoredProcedure);
                    return pagoRemoto;
                }
            }
            catch (Exception)
            {
                return true;
            }

        }
        public static bool RecargaEstaEnHorarioDeVentas()
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"declare @inicio DATETIME, @cierre DATETIME
                        SET @inicio = (Select top 1   Valor from SWebProductoConfig Where 
	                         Opcion =  'HORA_RECARGAS_INICIO'  AND Activo=1 AND WebProductoID=0)
	                         SET @cierre = (Select top 1  Valor from SWebProductoConfig Where 
	                         Opcion =  'HORA_RECARGAS_CIERRE'  AND Activo=1 AND WebProductoID=0)
	                        SET @inicio =  DATEADD(day, DATEDIFF(day, 0, GETDATE()), @inicio)
	                         SET @cierre = DATEADD(day, DATEDIFF(day, 0, GETDATE()), @cierre)
	                        SELECT @inicio as Inicio, @cierre AS Cierre";

                    var horario = con.QueryFirst<HoracioVentasRecargas>(query, commandType: CommandType.Text);
                    if (horario.Cierre != null && horario.Inicio != null)
                    {
                        if (DateTime.Now > DateTime.Parse(horario.Inicio) && DateTime.Now < DateTime.Parse(horario.Cierre))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    else if (horario.Cierre == null || horario.Inicio == null)
                    {
                        return true;
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                return true;
            }

        }

        public static HoracioVentasRecargas GetInicioCierreRecargasConsorcio()
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"declare @inicio DATETIME, @cierre DATETIME
                        SET @inicio = (Select top 1   Valor from SWebProductoConfig Where 
	                         Opcion =  'HORA_RECARGAS_INICIO'  AND Activo=1 AND WebProductoID=0)
	                         SET @cierre = (Select top 1  Valor from SWebProductoConfig Where 
	                         Opcion =  'HORA_RECARGAS_CIERRE'  AND Activo=1 AND WebProductoID=0)
	                        SET @inicio =  DATEADD(day, DATEDIFF(day, 0, GETDATE()), @inicio)
	                         SET @cierre = DATEADD(day, DATEDIFF(day, 0, GETDATE()), @cierre)
	                        SELECT @inicio as Inicio, @cierre AS Cierre";

                    var horario = con.QueryFirst<HoracioVentasRecargas>(query, commandType: CommandType.Text);
                    if (horario.Cierre != null && horario.Inicio != null)
                    {
                        return horario;
                    }
                   
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }


        public class HoracioVentasRecargas
        {
            public string Inicio { get; set; }
            public string Cierre { get; set; }
        }
        public static string GetLoteriaCierreRecargas()
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x => x.Activo && x.Opcion == Tables.Enums.DbEnums.ProductosConfig.LOTERIA_CIERRE_RECARGAS.ToString());
            if (query.Any())
            {
                return query.FirstOrDefault().Valor;
            }
            else
            {
                SetLoteriaCierreRecargas("2");
                return "2";
            }
        }
        public static void SetLoteriaCierreRecargas(string LoteriaID)
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x => x.Activo && x.Opcion == Tables.Enums.DbEnums.ProductosConfig.LOTERIA_CIERRE_RECARGAS.ToString());
            if (query.Any())
            {
                query.FirstOrDefault().Valor = LoteriaID;
            }
            else
            {
                var maxId = (from p in db.SWebProductoConfigs select p.WebProductoConfigID).Max();
                var prodConfig = new SWebProductoConfig()
                {
                    WebProductoConfigID = maxId + 1,
                    WebProductoID = 0,
                    Opcion = Tables.Enums.DbEnums.ProductosConfig.LOTERIA_CIERRE_RECARGAS.ToString(),
                    Modo = 0,
                    Activo = true,
                    Valor = LoteriaID
                };
                db.SWebProductoConfigs.Add(prodConfig);
            }
            db.SaveChanges();
        }
        public static List<string> GetPermisosUsuarioRifero(int UsuarioID)
        {
            var esEspecial = GetUsuarioRiferoEspecial(UsuarioID);
            var db = new MARContext();
            if (esEspecial)
            {
                var permisosEspeciales = db.RFuncionAdminUsuarios
                            .Where(x => x.UsuarioID == UsuarioID)
                            .Select(x => x.FuncionAdminID.ToString())
                            .ToList();
                return permisosEspeciales;
            }
            else
            {
                var query = db.SWebProductoConfigs.Where(x => x.Activo && x.Opcion == Tables.Enums.DbEnums.ProductosConfig.PERMISOS_RIFERO_USUARIOS.ToString());
                if (query.Any())
                {
                    return query.FirstOrDefault().Valor.Split(new char[] { ',' })
                        .Select(x => x.Trim())
                        .ToList();
                }
                else
                {
                    return new List<string>();
                }
            }
        }
        public static bool GetUsuarioRiferoEspecial(int UsuarioID)
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x =>
                x.Activo
                && x.Valor == UsuarioID.ToString()
                && x.Opcion == Tables.Enums.DbEnums.ProductosConfig.PERMISOS_RIFERO_ESPECIAL.ToString());
            return query.Any();
        }
        public static void SetUsuarioRiferoEspecial(int UsuarioID, bool EsEspecial)
        {
            var db = new MARContext();
            var query = db.SWebProductoConfigs.Where(x =>
                x.Valor == UsuarioID.ToString()
                && x.Opcion == Tables.Enums.DbEnums.ProductosConfig.PERMISOS_RIFERO_ESPECIAL.ToString());

            SWebProductoConfig prodConfig = query.FirstOrDefault();
            if (prodConfig != null)
            {
                prodConfig.Activo = EsEspecial;
            }
            else
            {
                var maxId = (from p in db.SWebProductoConfigs select p.WebProductoConfigID).Max();
                prodConfig = new SWebProductoConfig()
                {
                    WebProductoConfigID = maxId + 1,
                    WebProductoID = 0,
                    Opcion = Tables.Enums.DbEnums.ProductosConfig.PERMISOS_RIFERO_ESPECIAL.ToString(),
                    Modo = 0,
                    Activo = EsEspecial,
                    Valor = UsuarioID.ToString()
                };
                db.SWebProductoConfigs.Add(prodConfig);
            }
            if (!EsEspecial)
            {
                db.RFuncionAdminUsuarios.RemoveRange(db.RFuncionAdminUsuarios.Where(x => x.UsuarioID == UsuarioID).AsEnumerable());
            }
            db.SaveChanges();
        }
        public static List<string> GetModulosParaRiferos()
        {
            var db = new MARContext();
            var result = new List<string>();
            var query = db.SWebProductoConfigs.Where(x => x.Activo && x.Opcion == Tables.Enums.DbEnums.ProductosConfig.PERMISOS_RIFERO_USUARIOS.ToString());
            if (query.Any())
                result.AddRange(query.FirstOrDefault().Valor.Split(new char[] { ',' })
                        .Select(x => x.Trim()));

            query = db.SWebProductoConfigs.Where(x => x.Activo && x.Opcion == Tables.Enums.DbEnums.ProductosConfig.PERMISOS_RIFERO_OPCIONALES.ToString());
            if (query.Any())
                result.AddRange(query.FirstOrDefault().Valor.Split(new char[] { ',' })
                        .Select(x => x.Trim()));

            return result.Distinct().ToList();
        }

        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public ProductosConfigRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
