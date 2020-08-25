using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers;
using MAR.AppLogic.MARHelpers;
using Flujo.Entities.WpfClient.POCO;

            
namespace Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories
{
    public static  class CajaRepositorio
    { 

        public static bool ConfigurarCajaDisponibilidad(int pBancaID, bool pDisponible)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();  bool QueryFueCompletadoExitosamente = false;
                p.Add("@BancaID", pBancaID);
                p.Add("@SetCajaDisponibilidad", pDisponible);

                string querySeteaCajaDisponibilidad = @"
                    update flujo.Caja SET Disponible = @SetCajaDisponibilidad where CajaID = (select top 1 c.CajaID from flujo.Caja c where c.BancaID =  @BancaID )
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    int FilasAfectadas = con.Execute(querySeteaCajaDisponibilidad, p, commandType: CommandType.Text);

                    if (FilasAfectadas > 0)
                    {
                        QueryFueCompletadoExitosamente = true;
                    }
                }

                return QueryFueCompletadoExitosamente;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CajaFueAsignadaABanca(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool FueAsignada = false;

                p.Add("@BanID", pBancaID);

                string query__ValidadBancaTieneCaja = @"

                    select isnull(
                       (select top 1  1 As Existe from flujo.Caja c where c.BancaID = @BanID),
                    0) As Existe
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<bool> existe = con.Query<bool>(query__ValidadBancaTieneCaja, p, commandType: CommandType.Text).ToList();

                    if (existe.Any())
                    {
                        FueAsignada = existe.First();
                    } 
                }

                return FueAsignada;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool CreaCajaABanca(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool FueCreada = false;

                p.Add("@BanID", pBancaID);

                string query__ParaCrearBanca = @"

                        insert into flujo.Caja (TipoCajaID, ZonaID, UsuarioID, BancaID, Ubicacion, BalanceActual,BalanceMinimo,FechaBalance,FechaCreacion, Activa)     
	                    values(
	                       (select tc.TipoCajaID from flujo.TipoCaja tc Where tc.Nombre ='CAJA_TERMINAL'),   --TipoCajaID
	                         0,                       --ZonaID
	                         0,                       --UsuarioID 
	                         @BanID,                  --BancaID
	                    	 'Pendiente por asignar', --Ubicacion
	                    	 0,                       --BalanceActual
	                         500,                     --BalanceMinimo
	                         getdate(),               --FechaBalance
	                    	 getdate(),               --FechaCreacion
	                    	 1                        --Activa
	                    );
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    int affected = con.Execute(query__ParaCrearBanca, p, commandType: CommandType.Text);

                    if ( affected > 0)
                    {
                        FueCreada = true;
                    }
                }

                return FueCreada;
            }
            catch (Exception ex)
            {
                throw ex;
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

                    con.Close();
                }

                return box;
            }
            catch (Exception ex)
            {
                throw ex;
            }



        }

        public static  int  GetBancaCajaID(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); int CajaID =  -1;

                p.Add("@BancaID", pBancaID);

                string query = @"
                
                     select  top  1  c.CajaID from flujo.Caja c Where c.TipoCajaID = 1 and c.BancaID = @BancaID ;
    
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<int> TheIdsList = con.Query<int>(query, p,  commandType: CommandType.Text).ToList();

                    if (TheIdsList.Any())
                    {
                        CajaID = TheIdsList[0];
                    }

                    con.Close();
                }

                return CajaID;

            }
            catch (Exception ex)
            {

                return -1;
            }

        }

        public static decimal GetCajaBalanceActual (int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();  decimal balanceActual = 0;

                p.Add("@CajaID", pCajaID);
                p.Add("@BalanceActual", dbType: DbType.Decimal, direction: ParameterDirection.Output);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    con.Execute("[flujo].[Sp_ConsultaCajaBalanceActual]", p, commandType: CommandType.StoredProcedure) ;

                    balanceActual = p.Get<decimal>("@BalanceActual");
                }

                return balanceActual;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static decimal GetCajaBalanceMinimo(int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); decimal balanceMinimo = 0;

                p.Add("@CajaID", pCajaID);

                string query = @"
                
                     select top 1 c.BalanceMinimo from flujo.Caja c where c.CajaID = @CajaID
    
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<decimal> TheList = con.Query<decimal>(query, p, commandType: CommandType.Text).ToList();

                    if (TheList.Any())
                    {
                        balanceMinimo = TheList[0];
                    }

                    con.Close();
                }

                return balanceMinimo;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

    }
}
