using System;
using System.Linq;
using Dapper;
using System.Data;
using System.Collections.Generic;
using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;

using Flujo.Entities.WebClient.POCO;


namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class BancaRepositorio
    {

      

        public static void AgregaErrorPrueba(string pError)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); Banca banca = null;
                p.Add("@Error", pError);

                string query = $@"INSERT INTO ErrorPruebaCincoMinutos ([Error]) values (@Error)";
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                   con.Query(query, p, commandType: CommandType.Text);
                }
              
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static Banca GetBancaPorCajaID(int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); Banca banca = null;

                p.Add("@CajaID", pCajaID);

                string query = @"select top 1  b.BancaID, b.BanNombre, b.BanContacto, b.BanDireccion, b.BanTelefono from MBancas b join flujo.Caja c on b.BancaID= c.BancaID where c.CajaID = @CajaID";

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

        public static List<Banca> GetBancas(string pSearch, List<int> pListadoBancaANoBuscar)
        {
            try
            {
                List<Banca> LaListaDeBancasEncontradas;

                string query = @" 

					  if((select * from flujo.IsInt32(@search)) is not null )
                       begin
                     
                              select 
                                b.BancaID,	
                                b.BanNombre , 
                                b.BanContacto,
                                b.BanDireccion, 
                                b.BanTelefono,
                                b.BanNumeroLinea,
                                b.BanActivo
                              from dbo.MBancas b join flujo.Caja c on b.BancaID =  c.BancaID  join ( select * from flujo.Cuadre cu where cu.BalanceAnterior is null) As r on c.CajaID = r.CajaID
                              where 
                                   b.BancaID = ltrim(rtrim(@search)) and not exists ( select * from dbo.MBancas m where b.BancaID = m.BancaID and m.BancaID in @ListadoBancasNoABuscar) and b.BanActivo =1 ;
                       end
                     else
                       begin
                              select 
                                b.BancaID,	
                                b.BanNombre, 
                                b.BanContacto,
                                b.BanDireccion, 
                                b.BanTelefono,
                                b.BanNumeroLinea,
                                b.BanActivo
                              from dbo.MBancas b  join flujo.Caja c on b.BancaID =  c.BancaID  join ( select * from flujo.Cuadre cu where cu.BalanceAnterior is null) As r on c.CajaID = r.CajaID
                              where 
                                   b.BanContacto like (ltrim(rtrim(@search)))+'%' and not exists ( select * from dbo.MBancas m where b.BancaID = m.BancaID and m.BancaID in @ListadoBancasNoABuscar ) and b.BanActivo =1;
                       end 
                     
                ";


                DynamicParameters p = new DynamicParameters(); 

                p.Add("@search", pSearch);


                if (pListadoBancaANoBuscar != null && pListadoBancaANoBuscar.Count() > 0)
                {
                    p.Add("@ListadoBancasNoABuscar", pListadoBancaANoBuscar);
                }

                else
                {
                    p.Add("@ListadoBancasNoABuscar",  new List<int>() { }  );
                }

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    LaListaDeBancasEncontradas = con.Query<Banca>(query, p, commandType: CommandType.Text).ToList(); 
                }

                return LaListaDeBancasEncontradas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Banca> GetBancas(List<int> pListadoBancaIds)
        {
            try
            {
                List<Banca> LaListaDeBancasEncontradas;

                string query = @" 
                   select 
                     b.BancaID,	
                     b.BanNombre , 
                     b.BanContacto,
                     b.BanDireccion, 
                     b.BanTelefono,
                     b.BanNumeroLinea,
                     b.BanActivo
                   from dbo.MBancas b where b.BancaID in @ListadoBancaIds                      
                ";


                DynamicParameters p = new DynamicParameters();

                if (pListadoBancaIds != null && pListadoBancaIds.Count() > 0)
                {
                    p.Add("@ListadoBancaIds", pListadoBancaIds);
                }

                else
                {
                    p.Add("@ListadoBancaIds", new List<int>() { });
                }

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    LaListaDeBancasEncontradas = con.Query<Banca>(query, p, commandType: CommandType.Text).ToList();
                }

                return LaListaDeBancasEncontradas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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

        public static List<BancaRifero> GetBancasYSuRifero(string pSereach)
        {
            try
            {
                List<BancaRifero> LaColleccionDeBancasYSuRifero = null;

                DynamicParameters p = new DynamicParameters();

                p.Add("@Sereach", pSereach);

                string query = @"

                  select top 20 b.BancaID, b.BanContacto As Banca, m.RiferoID, m.RifNombre As Rifero
                  from dbo.MBancas b join flujo.Caja c on b.BancaID = c.BancaID join MRiferos m on b.RiferoID = m.RiferoID
                  where 
                       b.BancaID   like ltrim(rtrim(@Sereach))+'%'
                    or b.BanContacto like ltrim(rtrim(@Sereach))+'%' 
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    LaColleccionDeBancasYSuRifero = con.Query<BancaRifero>(query, p,commandType: CommandType.Text).ToList();
                }

                return LaColleccionDeBancasYSuRifero;
            }
            catch (Exception ex)
            {
                return new List<BancaRifero>();
            }

        }

        public static List<BancaRuta> GetBancasActivas()
        {
            try
            {
                List<BancaRuta> LaColleccionDeBancas;

                string query = @"

                        Select 
                             b.BancaID           As Posicion,
                             b.BanContacto     As BancaNombre, 
                        	 b.BanDireccion   As BancaDireccion,	 	 
                        	 b.BanTelefono    As BancaTelefono
                          From
                             dbo.MBancas b
                          Where b.BanActivo = 1 
                          Order By b.BanContacto asc;                      
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    LaColleccionDeBancas = con.Query<BancaRuta>(query, commandType: CommandType.Text).ToList();
                }

                return LaColleccionDeBancas;
            }
            catch (Exception ex)
            {
                return new List<BancaRuta>();
            }

        }

        public static List<BancaBalance> GetBancasBalancePorNombre(string pNombre)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@search", pNombre);

            try
            {
                List<BancaBalance>  BancaDataCollection;

                string query = @"

                     select top  40   
                       c.CajaID,
                       c.BancaID,
                       m.BanContacto As Banca,
                       c.BalanceActual,
                       m.BanActivo  As Activa
                     
                     from flujo.Caja c join MBancas m on c.BancaID = m.BancaID join ( 
                        
                    	select distinct cu.CajaID from flujo.Cuadre cu where cu.CuadreAnteriorID is null
                    
                     ) As r on c.CajaID = r.CajaID
                     
                     where m.BanActivo = 1 and c.Activa = 1 and m.BanContacto like  '%'+@search+ '%';

               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    BancaDataCollection = con.Query<BancaBalance>(query, p,commandType: CommandType.Text).ToList();
                }

                return BancaDataCollection;
            }
            catch (Exception ex)
            {
                return new List<BancaBalance>();
            }

        }
 
        public static bool BancaYCajaSonValidos(int pBancaID, int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool Banca_Y_Caja_Estan_Relacionados = false;

                p.Add("@BancaID", pBancaID);
                p.Add("@BancaCajaID", pCajaID);

                string query = @"

                   select top 1
                     c.CajaID from flujo.Caja c 
                   where
                         c.CajaID  = @BancaCajaID
                     And c.BancaID = @BancaID
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<int> Ids = con.Query<int>(query, p, commandType: CommandType.Text).ToList();

                    if (Ids.Any())
                    {
                        Banca_Y_Caja_Estan_Relacionados = true;
                    }

                }

                return Banca_Y_Caja_Estan_Relacionados;


            }
            catch (Exception ex)
            {

                return false;
            }
        }

        public static List<Banca> GetPorCobrarOPorLLevarBancas(List<int> pListadoBancaIds)
        {
            try
            {
                List<Banca> LaListaDeBancasEncontradas;

                string query = @" 
                    With Bancas_CTE(BancaID, BanNombre, BanContacto, BanDireccion, BanTelefono, BanNumeroLinea,BanActivo,PoseeCuadreInicial)
                    As(
                       select 
                         m.BancaID,	
                         m.BanNombre , 
                         m.BanContacto,
                         m.BanDireccion, 
                         m.BanTelefono,
                         m.BanNumeroLinea,
                         m.BanActivo,
                    	 isnull((select top 1  1 from flujo.Cuadre cu where  cu.CajaID = (select top 1 c.CajaID from flujo.Caja c where c.BancaID = m.BancaID )  and cu.BalanceAnterior is null), 0) As PoseeCuadreInicial
                       from dbo.MBancas m where m.BanActivo = 1  and m.BancaID in @ListadoBancaIds
                    ) 
                    select 
                      cte_bancas.BancaID,
                      cte_bancas.BanNombre,
                      cte_bancas.BanContacto,
                      cte_bancas.BanDireccion,
                      cte_bancas.BanTelefono,
                      cte_bancas.BanNumeroLinea,
                      cte_bancas.BanActivo,   
                      flujo.GetBancaMontoPorLLevar(cte_bancas.BancaID) As MontoPorLLevar,
                      flujo.GetBancaMontoPorCobrar(cte_bancas.BancaID) As MontoPorCobrar,
                      cte_bancas.PoseeCuadreInicial
                    from Bancas_CTE cte_bancas  
                    Order By MontoPorCobrar Desc, MontoPorLLevar Desc                  
                ";

                DynamicParameters p = new DynamicParameters();

                if (pListadoBancaIds != null && pListadoBancaIds.Count() > 0)
                {
                    p.Add("@ListadoBancaIds", pListadoBancaIds);
                }

                else
                {
                    p.Add("@ListadoBancaIds", new List<int>() { });
                }

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    LaListaDeBancasEncontradas = con.Query<Banca>(query, p, commandType: CommandType.Text).ToList();
                }

                return LaListaDeBancasEncontradas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
