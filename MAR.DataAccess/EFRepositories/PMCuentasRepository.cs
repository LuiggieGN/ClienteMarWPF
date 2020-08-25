using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using System.Data;
using Dapper;
using MAR.AppLogic.MARHelpers;
using static MAR.DataAccess.EFRepositories.PMCuentasRepository.CuentaProducto;

namespace MAR.DataAccess.EFRepositories
{

    public class PMCuentasRepository
    {

        public static IEnumerable<PMCuenta> GetPMCuentas(Expression<Func<PMCuenta, bool>> filter = null, Func<IQueryable<PMCuenta>, IOrderedQueryable<PMCuenta>> orderBy = null,
       string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<PMCuenta> query = db.PMCuentas;

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

        public static void ActualizarConfiguracionCuentas(int CuentaID,bool pInicioOcierreVacio, string HoraInicio = "", string HoraCierre = "")
        {

            string query = "";
            var p = new DynamicParameters();

            try
            {
                if (pInicioOcierreVacio)
                {

                    p.Add("@CuentaId", CuentaID);

                    query = @"-- ESTO ES CUENDO EL HORARIO ES VACIO
		                        --SI RiferoID != NULL ES CUENTA DE RIFERO

		                        IF (SELECT CuentaID FROM PMCuenta WHERE CuentaID = @CuentaId and RiferoID is not null) is not null
		                        BEGIN
		  		                        DELETE w
			                        FROM MBancasConfig w
			                        inner join MBancas b
			                        on b.BancaID = w.BancaID
			                        inner join PMCuenta c
			                        on c.RiferoID = b.RiferoID
			                        where ConfigKey = 'BANCA_HORA_RECARGAS_INICIO'
			                        or ConfigKey = 'BANCA_HORA_RECARGAS_CIERRE'
			                        and c.CuentaID = @CuentaId
		                        END

		                        --SI RiferoID = NULL ES CUENTA PRINCIPAL

		                        ELSE IF (SELECT CuentaID FROM PMCuenta WHERE CuentaID = @CuentaId and RiferoID is not null) is null
		                        BEGIN
			                        DELETE w
			                        FROM MBancasConfig w
			                        inner join MBancas b
			                        on b.BancaID = w.BancaID
			                        where b.RiferoID not in (select p.RiferoID from PMCuenta p where p.RiferoID IS NOT NULL)
			                        and (ConfigKey = 'BANCA_HORA_RECARGAS_INICIO'
			                        or ConfigKey = 'BANCA_HORA_RECARGAS_CIERRE')
		                        END";
                }
                else
                {


                    p.Add("@CuentaId", CuentaID);
                    p.Add("@Inicio", HoraInicio);
                    p.Add("@Cierre", HoraCierre);


                    query = @" -- ESTO ES CUENDO EL HORARIO TIENE INFO

		                                --SI RiferoID != NULL ES CUENTA DE RIFERO

                    IF (SELECT CuentaID FROM PMCuenta WHERE CuentaID = @CuentaId and RiferoID is not null) is not null
		                                BEGIN
                                    DELETE w
			                        FROM MBancasConfig w
			                        inner join MBancas b
			                        on b.BancaID = w.BancaID
			                        inner join PMCuenta c
			                        on c.RiferoID = b.RiferoID
			                        where ConfigKey = 'BANCA_HORA_RECARGAS_INICIO'
			                        or ConfigKey = 'BANCA_HORA_RECARGAS_CIERRE'
			                        and c.CuentaID = @CuentaId;

			                                INSERT INTO MBancasConfig(BancaID, ConfigKey, ConfigValue,activo)
			                                SELECT b.BancaID, 'BANCA_HORA_RECARGAS_INICIO', @Inicio,1 FROM MBancas b join PMCuenta p
			                                on p.RiferoID = b.RiferoID
			                                WHERE p.CuentaID = @CuentaId
			                                INSERT INTO MBancasConfig(BancaID, ConfigKey, ConfigValue,activo)
			                                SELECT b.BancaID, 'BANCA_HORA_RECARGAS_CIERRE', @Cierre,1 FROM MBancas b join PMCuenta p
			                                on p.RiferoID = b.RiferoID
			                                WHERE p.CuentaID = @CuentaId
		                                END

		                                --SI RiferoID = NULL ES CUENTA PRINCIPAL

		                                ELSE IF (SELECT CuentaID FROM PMCuenta WHERE CuentaID = @CuentaId and RiferoID is not null) is null
		                                BEGIN
                                   
                                            DELETE w
			                                FROM MBancasConfig w
			                                inner join MBancas b
			                                on b.BancaID = w.BancaID
			                                where b.RiferoID not in (select p.RiferoID from PMCuenta p where p.RiferoID IS NOT NULL)
			                                and (ConfigKey = 'BANCA_HORA_RECARGAS_INICIO'
			                                or ConfigKey = 'BANCA_HORA_RECARGAS_CIERRE')

			                                INSERT INTO MBancasConfig(BancaID, ConfigKey, ConfigValue, activo)
			                                SELECT b.BancaID, 'BANCA_HORA_RECARGAS_INICIO',@Inicio , 1 FROM MBancas b
			                                where b.RiferoID not in (select p.RiferoID from PMCuenta p where p.RiferoID IS NOT NULL)
			                                INSERT INTO MBancasConfig(BancaID, ConfigKey, ConfigValue,activo)
			                                SELECT b.BancaID, 'BANCA_HORA_RECARGAS_CIERRE',@Cierre,1 FROM MBancas b
			                                where b.RiferoID not in (select p.RiferoID from PMCuenta p where p.RiferoID IS NOT NULL)
		                                END";
                }


                using (var con = DALHelper.GetSqlConnection())
                {
                    var resultado = con.Query<bool>(query, p);
                }


            }

            catch (Exception e)
            {
                throw;
            }




        }

        public static CuentaProducto GetPMCuenta(int pRiferoId, string pCuentaNombre, int pMarBancaID, int pTipo)
        {

            try
            {
                string query = $@"SELECT CuentaID,CueNombre,CueComercio,CueComentario,CueActiva ,CueServidor ,CuePuerto,RiferoID, RecargaID FROM PMCuenta 
                                      where CueNombre = @CuentaNombre   AND CueActiva = 1 AND (RiferoID = @RiferoId OR RiferoID = 100000); 
                                      SELECT Nombre,URL,SuplidorID FROM SWebProducto where Nombre = @CuentaNombre AND Activo = 1;
                                      SELECT HaciendaTerminalID as TerminalId
                                     ,HaciendaLocalID AS LocalId
                                      FROM HaciendaTerminal h join MBancas b on h.MARTerminallD = b.BancaID
                                      where MARTerminallD = @MarBancaID  and h.Activo = 1 and Tipo = @Tipo;
                                       declare @Autorizacion varchar(50) set @Autorizacion =  
                                      (SELECT TOP 1  isnull(Autorizacion,'')  FROM  TransaccionClienteHttp WHERE TipoTransaccionID = 8 and Activo = 1  ORDER BY TransaccionID DESC) 
                                      select isnull(@Autorizacion,'') as Autorizacion;";
                var con = DALHelper.GetSqlConnection();
                var p = new DynamicParameters();
                p.Add("@CuentaNombre", pCuentaNombre);
                p.Add("@RiferoId", pRiferoId);
                p.Add("@Tipo", pTipo);
                p.Add("@MarBancaID", pMarBancaID);
                using (var multi = con.QueryMultiple(query, p))
                {
                    var pmCuenta = multi.Read<PMCuenta>().FirstOrDefault();
                    var swProducto = multi.Read<SWebProducto>().FirstOrDefault();
                    var terminal = multi.Read<HaciendaTerminal>().FirstOrDefault();
                    var transaccionCliente = multi.Read<TransaccionClienteHttp>().FirstOrDefault();
                    string autorizacionFueraDeLinea = string.IsNullOrEmpty(transaccionCliente.Autorizacion) ? "" : transaccionCliente.Autorizacion;
                    return new CuentaProducto { PMCuenta = pmCuenta, SWProducto = swProducto, Terminal = terminal, AutorizacionFueraDeLinea = transaccionCliente.Autorizacion };
                }
            }
            catch (Exception e)
            {
                string t = e.Message;
                return new CuentaProducto();
            }
        }

        public class CuentaProducto
        {
            public PMCuenta PMCuenta { get; set; }
            public SWebProducto SWProducto { get; set; }
            public HaciendaTerminal Terminal { get; set; }
            public string AutorizacionFueraDeLinea { get; set; }

            public class HaciendaTerminal
            {
                public int TerminalId { get; set; }
                public int LocalId { get; set; }
            }

        }



        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public PMCuentasRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }

    }
}
