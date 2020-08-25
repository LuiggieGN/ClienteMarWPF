using System;
using System.Linq;
using Dapper;
using System.Data;
using System.Collections.Generic;
using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;

using Flujo.Entities.WebClient.POCO;
using Flujo.Entities.WebClient.RequestModels;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class CajaRepositorio
    {

        public static List<Caja> GetCajasPorFiltroDesdeHasta(ConsultaCajaRequestModel consulta)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); List<Caja> cajas = null;
                p.Add("@TipoCajaID", consulta.TipoCajaID);
                p.Add("@CajaIDExcepcionDesde", consulta.CajaIDExcepcionDesde);
                p.Add("@CajaIDExcepcionHasta", consulta.CajaIDExcepcionHasta);
                p.Add("@Propietario", consulta.Propietario);

                string QueryGetCajas = @"  
                        select top 30
                    	   *
                    	from(
                              select m.BanContacto As CajaPropietarioNombre, c.* from flujo.Caja c join dbo.MBancas m on  c.BancaID = m.BancaID and c.CajaID in (select distinct cu.CajaID from flujo.Cuadre cu where cu.BalanceAnterior is null) 							  
                              union all
                              select u.Nombre      As CajaPropietarioNombre, c.* from flujo.Caja c join flujo.Usuario u on c.UsuarioID = u.UsuarioID
                    	      union all
                    	      select isnull(c.CajaDescripcion, '') As CajaPropietarioNombre, c.* from flujo.Caja c where c.TipoCajaID = 3
                    	) as r
                      where
                            r.TipoCajaID = @TipoCajaID and
                           (	          
                    		   (@CajaIDExcepcionDesde is null and @CajaIDExcepcionHasta is null)
                    		 or(@CajaIDExcepcionDesde is not null and @CajaIDExcepcionHasta is not null and r.CajaID <> @CajaIDExcepcionDesde and r.CajaID <> @CajaIDExcepcionHasta)
                    		 or(@CajaIDExcepcionDesde is null and @CajaIDExcepcionHasta is not null and r.CajaID <> @CajaIDExcepcionHasta)
                    		 or(@CajaIDExcepcionHasta is null and @CajaIDExcepcionDesde is not null and r.CajaID <> @CajaIDExcepcionDesde) 
                    	   )
                    	   and r.CajaPropietarioNombre like ltrim(rtrim(@Propietario)) + '%'
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    cajas = con.Query<Caja>(QueryGetCajas, p, commandType: CommandType.Text).ToList();
                }

                return cajas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool VerificarDisponibilidadDeCaja(int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool CajaEstaDisponible = false;

                p.Add("@CajaID", pCajaID);

                string QueryConsultaCajaDisponibilidad = @"  
                    select   isnull((select top 1 c.Disponible from flujo.Caja c where c.CajaID = @CajaID), 0) As Disponibilidad
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    CajaEstaDisponible = con.Query<bool>(QueryConsultaCajaDisponibilidad, p, commandType: CommandType.Text).FirstOrDefault();
                }

                return CajaEstaDisponible;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static ConsultaBalanceCaja GetCajaBalance(int pCajaID, int pUsuarioLogueadoID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();

                ConsultaBalanceCaja consulta = null;

                p.Add("@CajaID", pCajaID);
                p.Add("@UserID", pUsuarioLogueadoID);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<ConsultaBalanceCaja> ColleccionCajas = con.Query<ConsultaBalanceCaja>(SelectView.SelectCajaEspecificaBalancePorCajaID, p, commandType: CommandType.Text).ToList();

                    if (ColleccionCajas.Any())
                    {
                        consulta = ColleccionCajas.First();
                    }
                }

                return consulta;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static decimal GetCajaBalanceActual(int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); decimal balanceActual = 0;

                p.Add("@CajaID", pCajaID);
                p.Add("@BalanceActual", dbType: DbType.Decimal, direction: ParameterDirection.Output);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    con.Execute("[flujo].[Sp_ConsultaCajaBalanceActual]", p, commandType: CommandType.StoredProcedure);

                    balanceActual = p.Get<decimal>("@BalanceActual");
                }

                return balanceActual;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static Caja GetCajaVirtual(int pUsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();

                Caja box;

                p.Add("@UsuarioID", pUsuarioID);

                string query = @"

                     select top 1 * from flujo.Caja c where c.UsuarioID = @UsuarioID;
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<Caja> _current = con.Query<Caja>(query, p, commandType: CommandType.Text).ToList();

                    if (_current.Any())
                    {
                        box = _current.First();
                    }
                    else
                    {
                        box = null;
                    }
                }

                return box;
            }
            catch (Exception)
            {
                return null;
            }



        }


        public static int GetCajaBalancePorUsuario(int usuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@usuarioID", usuarioID);
                List<int> resultados = new List<int>();
                string query = @"
                select top 1 c.BalanceMinimo from flujo.caja c where c.UsuarioID = @usuarioID
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    resultados = con.Query<int>(query, p, commandType: CommandType.Text).ToList();
                }

                return resultados.Any() ? resultados.FirstOrDefault() : 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }




        public static bool ActualizarBalanceMinimoCajaPorUsuario(int BalanceMinimoCaja, int UsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@BalanceMinimo", BalanceMinimoCaja);
                p.Add("@UsuarioID", UsuarioID);


                string query = @" UPDATE [flujo].[Caja] SET[BalanceMinimo] = @BalanceMinimo WHERE UsuarioID = @UsuarioID ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    con.Execute(query, p, commandType: CommandType.Text);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool AddCaja(Caja Caja)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@TipoCajaID", Caja.TipoCajaID);
                p.Add("@ZonaID", Caja.ZonaID);
                p.Add("@UsuarioID", Caja.UsuarioID);
                p.Add("@BancaID", Caja.BancaID);
                p.Add("@Ubicacion", Caja.Ubicacion);
                p.Add("@BalanceActual", Caja.BalanceActual);
                p.Add("@BalanceMinimo", Caja.BalanceMinimo);
                p.Add("@Activa", Caja.Activa);

                string query = @"
                 INSERT INTO
			    [flujo].[Caja]([TipoCajaID],[ZonaID],[UsuarioID],[BancaID],[Ubicacion],[BalanceActual],[BalanceMinimo],[FechaBalance],[FechaCreacion],[Activa])
                 VALUES 
			    (@TipoCajaID ,@ZonaID ,@UsuarioID ,@BancaID ,@Ubicacion ,@BalanceActual ,@BalanceMinimo ,GETDATE()  ,GETDATE() ,@Activa)
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    con.Execute(query, p, commandType: CommandType.Text);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}