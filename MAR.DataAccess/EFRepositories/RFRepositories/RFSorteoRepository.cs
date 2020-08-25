using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using System.Data;
using MAR.AppLogic.MARHelpers;
using Dapper;
using System.Data.SqlClient;

namespace MAR.DataAccess.EFRepositories.RFRepositories
{
    public class RFSorteoRepository
    {

        public static object GetRFSorteoTiposJugada()
        {

            string query = "SELECT [SorteoTipoJugadaID] ,stj.[SorteoTipoID] ,stj.[Nombre],  stj.Referencia, st.Nombre as SorteoTipoNombre,st.Referencia as SorteoTipoReferencia, [CamposNumero] ,[NumeroMinimo]  ," +
                "[NumeroMaximo]  ,[Activo] ,[Opciones] ,[Instrucciones] " +
                " FROM RF_SorteoTipoJugada stj JOIN RF_SorteoTipo st on stj.SorteoTipoID = st.SorteoTipoID WHERE stj.Activo = 1";
            var result = DALHelper.GetDataTable(query, null, CommandType.Text);
            return result.Result;


        }
        public static object GetRFSorteoTiposJugadaPega4()
        {
            try
            {
                string query = $@"SELECT [SorteoTipoJugadaID] ,stj.[SorteoTipoID] ,stj.[Nombre],  stj.Referencia, st.Nombre as SorteoTipoNombre,st.Referencia as SorteoTipoReferencia, [CamposNumero] ,[NumeroMinimo]  ," +
                "[NumeroMaximo]  ,[Activo] ,[Opciones] ,[Instrucciones] " +
                " FROM RF_SorteoTipoJugada stj JOIN RF_SorteoTipo st on stj.SorteoTipoID = st.SorteoTipoID WHERE stj.Activo = 1 and stj.SorteoTipoID in (1,2)";
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    return con.Query(query, p, commandType: CommandType.Text); ;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IEnumerable<RF_SorteoDia> GetRFSorteoDia(Expression<Func<RF_SorteoDia, bool>> filter = null, Func<IQueryable<RF_SorteoDia>, IOrderedQueryable<RF_SorteoDia>> orderBy = null,
          string includeProperties = "")
        {
            var db = new MARContext();
            IQueryable<RF_SorteoDia> query = db.RF_SorteoDia;

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

        public static DataAccess.Tables.DTOs.VHEstatusDia Get_VHEstatusDia(int pEstatusDiaID)
        {
            string sqlQuery = "SELECT * FROM  VHEstatusDias where EstatusDiaID = @EstatusDiaID";
            using (var con = DALHelper.GetSqlConnection())
            {
                var vHEstatusDia = con.QueryFirstOrDefault<VHEstatusDia>(sqlQuery, new { EstatusDiaID = pEstatusDiaID });
                return vHEstatusDia;
            }
        }
        public static bool ActualizaHResumenConsolidaPagos(DateTime pEditFecha, string pComentario, int? pBancaId = null)
        {
            try
            {
                string sqlQuery = "SELECT * FROM  VHEstatusDias where EstatusDiaID = @EstatusDiaID";
                using (var con = DALHelper.GetSqlConnection())
                {
                    var p = new DynamicParameters();
                    p.Add("@EdiFecha", pEditFecha);
                    p.Add("@Bancas", pBancaId);
                    p.Add("@Comentario", pComentario);
                    con.Query("ActualizaHResumenConsolidaPagos", p, commandType: CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public static HEstatusDia Get_HEstatusDia(int pEstatusDiaID)
        {
            try
            {
                string sqlQuery = "SELECT * FROM  HEstatusDias where EstatusDiaID = @EstatusDiaID";
                using (var con = DALHelper.GetSqlConnection())
                {
                    var vHEstatusDia = con.QueryFirstOrDefault<HEstatusDia>(sqlQuery, new { EstatusDiaID = pEstatusDiaID });
                    return vHEstatusDia;
                }
         
            }
            catch (Exception)
            {
                throw;
            }
       
        }


        public static object GetRFSorteosDia(List<Tables.Enums.DbEnums.SorteoReferencia> pSorteosRef, int pBancaId)
        {

            List<string> sorteosRef = new List<string>();
            for (int i = 0; i < pSorteosRef.Count(); i++)
            {
                sorteosRef.Add(pSorteosRef[i].ToString());
            }


            //string sqlQuery = "SELECT sd.SorteoDiaID, MAX(eb.EsquemaPagoID) EsquemaPagoID ,sd.SorteoID, MAX(sd.Referencia) Referencia, MAX(SorteoTipoID) SorteoTipoID from RF_EsquemaPagoBanca eb JOIN RF_EsquemaPagoPremio ep on eb.EsquemaPagoID = ep.EsquemaPagoID " +
            //                  " JOIN RF_SorteoDia sd ON ep.SorteoID = sd.SorteoID JOIN RF_Sorteo s on sd.SorteoID = s.SorteoID WHERE  eb.Activo = 1 AND sd.Referencia IN('" + String.Join("','", sorteosRef) + "')  and eb.FechaActivo <= GETDATE() and sd.VentasCerradas = 0 and sd.DiaCerrado = 0 and sd.HoraCierreVentas >= GETDATE() " +
            //                  " and sd.HoraInicioVentas <= GETDATE() AND (eb.BancaID = " + pBancaId + " OR eb.BancaID = 0) GROUP BY SorteoDiaID, sd.SorteoID ";


            string sqlQuery = "SELECT sd.SorteoDiaID, MAX(eb.EsquemaPagoID) EsquemaPagoID ,sd.SorteoID,  MAX(sd.Referencia) Referencia, MAX(SorteoTipoID) SorteoTipoID, MAX(s.Nombre) as Nombre, sc.SorteoCampoId, sc.Nombre, sc.Referencia,rc.ReciboCampoID, rc.Nombre, rc.Referencia from RF_EsquemaPagoBanca eb JOIN RF_EsquemaPagoPremio ep on eb.EsquemaPagoID = ep.EsquemaPagoID " +
                            " JOIN RF_SorteoDia sd ON ep.SorteoID = sd.SorteoID JOIN RF_Sorteo s on sd.SorteoID = s.SorteoID LEFT JOIN RF_SorteoCampo sc ON sd.SorteoID = sc.SorteoID LEFT JOIN CL_ReciboCampo rc ON rc.Activo = 1 WHERE  eb.Activo = 1 AND sd.Referencia IN('" + String.Join("','", sorteosRef) + "')  and eb.FechaActivo <= GETDATE() and sd.VentasCerradas = 0 and sd.DiaCerrado = 0 and sd.HoraCierreVentas >= GETDATE() " +
                            " and sd.HoraInicioVentas <= GETDATE() AND (eb.BancaID = " + pBancaId + " OR eb.BancaID = 0) GROUP BY SorteoDiaID, sd.SorteoID , sc.Nombre, sc.Referencia, sc.SorteoCampoId , sc.SorteoID, rc.ReciboCampoID, rc.Nombre, rc.Referencia ";

            List<SorteoViewModel> sortesolist = new List<SorteoViewModel>();
            List<SorteoCampo> sorteosCampos = new List<SorteoCampo>();
            List<ReciboCampo> reciboCampos = new List<ReciboCampo>();
            using (var con = DALHelper.GetSqlConnection())
            {
                var sorteos = con.Query<SorteoViewModel, SorteoCampo, ReciboCampo, SorteoViewModel>(
                     sqlQuery,
                     (sor, prodCam, recCampo) =>
                     {
                         SorteoViewModel sorEntry;
                         sorEntry = sor;
                         sorEntry.SorteosCampos = new List<SorteoCampo>();
                         sorEntry.RecibosCampos = new List<ReciboCampo>();
                         sorteosCampos.Add(prodCam);
                         reciboCampos.Add(recCampo);
                         return sorEntry;
                     },
                     splitOn: "SorteoID,SorteoCampoId,ReciboCampoID", commandType: CommandType.Text)
                 .Distinct().GroupBy(x => x.SorteoID).Select(x => x.FirstOrDefault());

                foreach (var item in sorteos)
                {
                    foreach (var ca in sorteosCampos)
                    {
                        item.SorteosCampos.Add(ca);
                    }
                }
                var result = new { Sorteos = sorteos, ReciboCampos = reciboCampos.Distinct().GroupBy(x => x.ReciboCampoID).Select(x => x.FirstOrDefault()) };
                return result;
            }
        }
        public class SorteoViewModel
        {
            public int SorteoDiaID { get; set; }
            public int EsquemaPagoID { get; set; }
            public int SorteoID { get; set; }
            public string Nombre { get; set; }
            public int SorteoTipoID { get; set; }
            public string Referencia { get; set; }
            public List<SorteoCampo> SorteosCampos { get; set; }
            public List<ReciboCampo> RecibosCampos { get; set; }

        }
        public class SorteoCampo
        {
            public string SorteoCampoID { get; set; }
            public string Nombre { get; set; }
            public string Referencia { get; set; }

        }
        public class ReciboCampo
        {
            public string ReciboCampoID { get; set; }
            public string Nombre { get; set; }
            public string Referencia { get; set; }

        }
        public static DataAccess.ViewModels.BaseViewModel.SqlDataResult EntrarPremiosTradicionales(int pEstatusDiaId, string pQP1, string pQP2, string pQP3)
        {
            try
            {
                List<SqlParameter> sqlParamList = new List<SqlParameter>();
                SqlParameter[] sqlParams = new SqlParameter[4];
                sqlParams[0] = new SqlParameter("@EstatusDiaID", pEstatusDiaId);
                sqlParams[1] = new SqlParameter("@QP1", pQP1);
                sqlParams[2] = new SqlParameter("@QP2", pQP2);
                sqlParams[3] = new SqlParameter("@QP3", pQP3);
                sqlParamList.AddRange(sqlParams);
                var result = DALHelper.PostDataTable("CierraPremiosTradicionales", sqlParamList, CommandType.StoredProcedure);
                return result;
            }
            catch (Exception e)
            {
                return new ViewModels.BaseViewModel.SqlDataResult { OK = false, Result = e.Message + ' '+ e.StackTrace };
            }
           
        }

        public static DataAccess.ViewModels.BaseViewModel.SqlDataResult CorrigePremiosTradicionales(int pLoteriaId, string pQP1, string pQP2, string pQP3, DateTime pFecha)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter[] sqlParams = new SqlParameter[5];
            sqlParams[0] = new SqlParameter("@Loteria", pLoteriaId);
            sqlParams[1] = new SqlParameter("@Primero", pQP1);
            sqlParams[2] = new SqlParameter("@Segundo", pQP2);
            sqlParams[3] = new SqlParameter("@Tercero", pQP3);
            sqlParams[4] = new SqlParameter("@Fecha", pFecha);
            sqlParamList.AddRange(sqlParams);
            var result = DALHelper.PostDataTable("CorrigePremiosOficial", sqlParamList, CommandType.StoredProcedure);
            return result;
        }

        public static IEnumerable<RF_SorteoDia> ValidarRFSorteosPorID(int[] pSorteosDiaIds)
        {
            var db = new MARContext();
            IQueryable<RF_SorteoDia> query = db.RF_SorteoDia.Where(x => pSorteosDiaIds.Contains(x.SorteoDiaID) && !x.VentasCerradas && !x.DiaCerrado && x.HoraCierreVentas >= DateTime.Now && x.HoraInicioVentas <= DateTime.Now);
            return query.ToList();
        }

        public static RF_SorteoDia CerrarRFSorteo(int pSorteosDiaId)
        {
            var db = new MARContext();
            var sorteoDia = db.RF_SorteoDia.Where(x => x.SorteoDiaID == pSorteosDiaId).FirstOrDefault();
            sorteoDia.VentasCerradas = true;
            sorteoDia.HoraCierreVentas = DateTime.Now;
            db.SaveChanges();
            return sorteoDia;
        }


        public static DataAccess.ViewModels.BaseViewModel.SqlDataResult CerrarDia(int pSorteoID, int pSorteoDiaID)
        {
            List<SqlParameter> sqlParamList = new List<SqlParameter>();
            SqlParameter[] sqlParams = new SqlParameter[2];
            sqlParams[0] = new SqlParameter("@SorteoID", pSorteoID);
            sqlParams[1] = new SqlParameter("@SorteoDiaID", pSorteoDiaID);
            sqlParamList.AddRange(sqlParams);
            var result = DALHelper.PostDataTable("CierraDiaAbreNuevo", sqlParamList, CommandType.StoredProcedure);
            return result;
        }



        public static CorrigePremiosConfig GetCorrigePremiosConfig() // Valida si puede hacer la venta y no excede el limite
        {
            try
            {
                using (var con = DALHelper.GetSqlConnection())
                {
                    string query = $@"select top 1 Consorcio = GruNombre, EmailFromServerConfig  = s.Valor, EmailTo = n.eMail from TGrupos 
                                                  join MNotificaciones n on n.Tipo = 4
                                                  join SWebProductoConfig s on s.Opcion = 'SERVIDOR_DE_CORREO'";
                    var p = new DynamicParameters();
                    var config = con.Query<CorrigePremiosConfig>(query, p, commandType: CommandType.Text).FirstOrDefault();
                    return config;
                }
            }
            catch (Exception e)
            {
                return null;
            }
           
        }

        public class CorrigePremiosConfig
        {
            public string EmailFromServerConfig { get; set; }
            public string EmailTo { get; set; }
            public string Consorcio { get; set; }
        }




        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public RFSorteoRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
