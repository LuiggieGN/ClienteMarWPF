using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Dapper;

using Flujo.Entities.WebClient.POCO;

using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;
using Flujo.Entities.WebClient.ViewModels;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class UsuarioRepositorio
    {
        public static Usuario GetUsuarioPorCajaIDDeTipoVirtual(int pCajaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); Usuario user = null;

                p.Add("@CajaID", pCajaID);

                string query = @"
                    select u.* from flujo.Usuario u join flujo.Caja c on u.UsuarioID = c.UsuarioID and c.TipoCajaID = ( select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_VIRTUAL') where c.CajaID = @CajaID;
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<Usuario> theListOfUsers = con.Query<Usuario>(query, p, commandType: CommandType.Text).ToList();

                    if (theListOfUsers.Any())
                    {
                        user = theListOfUsers.FirstOrDefault();
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool PuedeEliminarUsuario(int pUsuarioQueIntentaEliminar, int pUsuarioAEliminar)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool PuedeEliminar = false;

                p.Add("@UsuarioQueModica", pUsuarioQueIntentaEliminar);
                p.Add("@UsuarioModificado", pUsuarioAEliminar);

                string query = @"  

                    select isnull
                    (
                      (
                         select top 1 
                           1
                         from 
                           flujo.PermisoModificarTipoUsuario pu
                         where
                                pu.RolModificadorID = (select top 1 u.TipoUsuarioID from flujo.Usuario u where u.UsuarioID = @UsuarioQueModica  and u.Activo = 1  )
                        	and  pu.RolModificadoID   = (select top 1 u.TipoUsuarioID from flujo.Usuario u where u.UsuarioID = @UsuarioModificado and u.Activo = 1  )
                        	and  pu.PuedeEliminar     = 1
                        	and  pu.Activo            = 1    
                      ),
                       
                   	0
                    );

                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<bool> TheList = con.Query<bool>(query, p, commandType: CommandType.Text).ToList();

                    if (TheList.Any())
                    {
                        PuedeEliminar = TheList.First();
                    }
                }

                return PuedeEliminar;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public static void DeleteUsuario(int usuarioid)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@UsuarioID", usuarioid);

                string query = @" delete from [flujo].[Usuario]  where UsuarioID = @usuarioid  ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    con.Execute(query, p, commandType: CommandType.Text);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DesactivarUsuario(int pUsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool desactivado = false;
                p.Add("@UsuarioID", pUsuarioID);

                string query1 = @"
 update flujo.Usuario set Activo = 0 where UsuarioID = @UsuarioID;
                ";

                string query2 = @"
delete from flujo.UsuarioTarjeta where UsuarioID  = @UsuarioID;
delete from flujo.ToquenUsuario  where TarjetaID = (select top 1 ut.TarjetaID from flujo.UsuarioTarjeta ut where UsuarioID  = @UsuarioID) ;
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    int affectedQuery1 = con.Execute(query1, p, commandType: CommandType.Text);

                    if(affectedQuery1 > 0)
                    {
                        con.Execute(query2, p, commandType: CommandType.Text);
                        desactivado = true;
                    }
                }

                return desactivado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Registro un usuario 
        /// </summary>
        /// <param name="pUsuario"></param>
        /// <returns></returns>
        public static int? AddUsuario(Usuario pUsuario)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); int? UsuarioIDRegistrado = null;

                p.Add("@UsuarioID", pUsuario.UsuarioID);
                p.Add("@Nombre", pUsuario.Nombre);
                p.Add("@TipoUsuarioID", pUsuario.TipoUsuarioID);
                p.Add("@TipoDocumentoID", pUsuario.TipoDocumentoID);
                p.Add("@Documento", pUsuario.Documento);
                p.Add("@Activo", pUsuario.Activo);
                p.Add("@ZonaID", pUsuario.ZonaID);


                string query = @"
                 INSERT INTO [flujo].[Usuario]([Nombre] ,[TipoUsuarioID],[TipoDocumentoID],[Documento],[ZonaID],[Activo])
					 VALUES( @Nombre,@TipoUsuarioID,@TipoDocumentoID,@Documento,@ZonaID,@Activo);

                     select scope_identity();
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<int?> Result = con.Query<int?>(query, p, commandType: CommandType.Text).ToList();

                    if (Result.Any())
                    {
                        UsuarioIDRegistrado = Result.First();
                    }
                }

                return UsuarioIDRegistrado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool ActualizaUsuario(Usuario pUsuario)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();

                p.Add("@UsuarioID", pUsuario.UsuarioID);
                p.Add("@Nombre", pUsuario.Nombre);
                p.Add("@TipoUsuarioID", pUsuario.TipoUsuarioID);
                p.Add("@TipoDocumentoID", pUsuario.TipoDocumentoID);
                p.Add("@Documento", pUsuario.Documento);
                p.Add("@Activo", pUsuario.Activo);
                p.Add("@ZonaID", pUsuario.ZonaID);

                string query = @"
                 UPDATE [flujo].[Usuario] set
                      [Nombre] = @Nombre
                      ,[TipoUsuarioID] = @TipoUsuarioID
                      ,[TipoDocumentoID] =@TipoDocumentoID
                      ,[Documento] = @Documento
                      ,[ZonaID] = @ZonaID
                      ,[Activo] = @Activo
                 WHERE  [UsuarioID] = @UsuarioID
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



        public static List<ComboBoxModel> GetUsuariosTipo(int pTipoUsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters();

                p.Add("@TipoUsuarioID", pTipoUsuarioID);

                List<ComboBoxModel> resultados = new List<ComboBoxModel>();
                string query = @"
                    -----------------------------------------------------------
                    -- Busca los roles que puede registrar un tipo de usuario
                    select
                       tu.TipoUsuarioID As [Value],
                       tu.Tipo As [Text]	
                    from flujo.TipoUsuario tu join  (
                        select 
                          pmu.RolModificadoID
                       from flujo.PermisoModificarTipoUsuario pmu      
                       where
                          pmu.RolModificadorID =(	 
                            select tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.TipoUsuarioID = @TipoUsuarioID
                  	    )    
                           and pmu.PuedeRegistrar = 1 and pmu.Activo = 1 
                   ) As permitido on tu.TipoUsuarioID = permitido.RolModificadoID
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    resultados = con.Query<ComboBoxModel>(query, p, commandType: CommandType.Text).ToList();
                }

                return resultados.Any() ? resultados : new List<ComboBoxModel>();
            }
            catch (Exception ex)
            {
                return new List<ComboBoxModel>();
            }
        }

        public static List<ComboBoxModel> GetDocumentosTipo()
        {
            try
            {
                List<ComboBoxModel> resultados = new List<ComboBoxModel>();
                string query = @"
          SELECT f.TipoDocumentoID as Value, f.tipo as Text from flujo.TipoDocumento f
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    resultados = con.Query<ComboBoxModel>(query, commandType: CommandType.Text).ToList();
                }

                return resultados.Any() ? resultados : new List<ComboBoxModel>();
            }
            catch (Exception ex)
            {
                return new List<ComboBoxModel>();
            }
        }

        /// <summary>
        ///   Obtengo el documento de un mensajero segun el parametro de busqueda
        /// </summary>
        /// <param name="pSereach">El nombre del rifero o el id del mensajero</param>
        /// <returns></returns>
        public static List<Rifero> GetRifero(string pSereach)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); List<Rifero> LaListaDeRifero = null;

                p.Add("@Sereach", pSereach);

                string query = @"

                     select top 20 * from (

                         select 
                            TResult.RiferoID,
                            TResult.RifNombre
                         from (
                         
                            select 
                              r.RiferoID, r.RifNombre
                            from
                               MRiferos r join MBancas mb on r.RiferoID = mb.RiferoID join flujo.Caja c on mb.BancaID = c.BancaID 
                         
                         ) As TResult
                         
                         where 
                                TResult.RiferoID like rtrim(ltrim(@Sereach)) + '%' 
                         	Or TResult.RifNombre like rtrim(ltrim(@Sereach)) + '%' 
                         
                         Group By TResult.RiferoID, TResult.RifNombre

                    ) As Consulta
                     
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    LaListaDeRifero = con.Query<Rifero>(query, p, commandType: CommandType.Text).ToList();

                }

                return LaListaDeRifero;
            }
            catch (Exception ex)
            {
                return new List<Rifero>();
            }
        }

        /// <summary>
        ///   Obtengo el documento de un mensajero segun el parametro de busqueda
        /// </summary>
        /// <param name="pSereach">El nombre del mensajero o numero de documento</param>
        /// <returns></returns>
        public static List<MensajeroDocumento> GetMensajeroDocumento(string pSereach)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); List<MensajeroDocumento> LaListaDeMensajeros = null;

                p.Add("@Sereach", pSereach);

                string query = @"

                    select 
                      u.UsuarioID,
                      u.Nombre     As Mensajero,
                      u.Documento   
                    from 
                    	  flujo.Usuario u join flujo.Caja c on u.UsuarioID = c.UsuarioID and c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc Where tc.Nombre = 'CAJA_VIRTUAL')
                                      join flujo.TipoDocumento td on u.TipoDocumentoID = td.TipoDocumentoID
                    where 
                          
                        (
                            u.TipoUsuarioID = (select top 1 tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.Tipo='Mensajero')
                            and (                    	
                    	         u.Nombre    like ltrim(rtrim(@Sereach))+'%' 
                    	      or u.Documento like ltrim(rtrim(@Sereach))+'%'    
                    	    ) and u.Activo = 1  
                        )
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    LaListaDeMensajeros = con.Query<MensajeroDocumento>(query, p, commandType: CommandType.Text).ToList();

                }

                return LaListaDeMensajeros;
            }
            catch (Exception ex)
            {
                return new List<MensajeroDocumento>();
            }
        }


        /// <summary>
        ///   Obtengo el Usuario mediante la especificacion de un UsuarioID en especifico 
        /// </summary>
        /// <param name="pUsuarioID">El Id del Usuario que se quiere  registrqar</param>
        /// <returns></returns>
        public static int GetUsuarioTarjetaID(int pUsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); int TarjetaID = -1;

                p.Add("@UsuarioID", pUsuarioID);

                string query = @"

                  select top 1 ut.TarjetaID from flujo.UsuarioTarjeta ut where ut.UsuarioID =  @UsuarioID;
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<int> theListOfTarjetaIDs = con.Query<int>(query, p, commandType: CommandType.Text).ToList();

                    if (theListOfTarjetaIDs.Any())
                    {
                        TarjetaID = theListOfTarjetaIDs.First();
                    }
                }

                return TarjetaID;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        /// <summary>
        ///   Obtengo el Usuario mediante la especificacion de un UsuarioID en especifico 
        /// </summary>
        /// <param name="pUsuarioID">El Id del Usuario que se quiere  registrqar</param>
        /// <returns></returns>
        public static Usuario GetFlujoUsuarioEquivalent(int pMARUsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); Usuario user = null;

                p.Add("@UsuarioMARID", pMARUsuarioID);

                string query = @"
                    select * from flujo.Usuario u where u.UsuarioMARID = @UsuarioMARID; 
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<Usuario> theListOfUsers = con.Query<Usuario>(query, p, commandType: CommandType.Text).ToList();

                    if (theListOfUsers.Any())
                    {
                        user = theListOfUsers.FirstOrDefault();
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        ///   Obtengo el Usuario mediante la especificacion de un UsuarioID en especifico 
        /// </summary>
        /// <param name="pUsuarioID">El Id del Usuario que se quiere  registrqar</param>
        /// <returns></returns>
        public static Usuario GetUsuario(int pUsuarioID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); Usuario user = null;

                p.Add("@UsuarioID", pUsuarioID);

                string query = @"
                    select * from flujo.Usuario u where u.UsuarioID = @UsuarioID; 
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<Usuario> theListOfUsers = con.Query<Usuario>(query, p, commandType: CommandType.Text).ToList();

                    if (theListOfUsers.Any())
                    {
                        user = theListOfUsers.FirstOrDefault();
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static ConsultaUsuarioBalance GetUsuarioBalance(int pUsuarioID)
        {
            try
            {
                ConsultaUsuarioBalance Result = null;

                List<ConsultaUsuarioBalance> RecordsConsulta;

                DynamicParameters p = new DynamicParameters();

                p.Add("@UsuarioID", pUsuarioID);

                string query = @"


                 select top 1 UsuarioXCajaVirtual.CajaID, UsuariosX.UsuarioID As Posicion, UsuariosX.Nombre, UsuariosX.Documento, UsuarioXCajaVirtual.BalanceActual , UsuariosX.Activo from (
                 
                    select * from (
                    
                       select top 1 * from flujo.Usuario u where u.UsuarioID = @UsuarioID
                    
                    ) 
                	As uf   
                 
                 )As UsuariosX 
                 
                       join flujo.Caja UsuarioXCajaVirtual on UsuariosX.UsuarioID = UsuarioXCajaVirtual.UsuarioID and UsuarioXCajaVirtual.TipoCajaID = (select tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_VIRTUAL')


               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    RecordsConsulta = con.Query<ConsultaUsuarioBalance>(query, p, commandType: CommandType.Text).ToList();

                    if (RecordsConsulta.Any())
                    {
                        Result = RecordsConsulta.First();
                    }
                }

                return Result;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        ///   Consulta datos de  usuarios referente a su caja mediante la especificacion de su nombre o numero de documento
        /// </summary>
        /// <param name="pSearch">Niombre del  usuario que se desea consultar o No. de Documento</param>
        /// <returns></returns>
        public static List<ConsultaUsuarioBalance> GetUsuariosBalances(string pSearch)
        {
            try
            {
                List<ConsultaUsuarioBalance> RecordsConsulta;

                DynamicParameters p = new DynamicParameters();

                p.Add("@Sereach", pSearch);

                string query = @"

                     select UsuarioXCajaVirtual.CajaID, UsuariosX.UsuarioID As Posicion, UsuariosX.Nombre, UsuariosX.Documento, UsuarioXCajaVirtual.BalanceActual , UsuariosX.Activo from (
                     
                        select * from (
                        
                           select top 40 * from flujo.Usuario u where u.Nombre like  '%'+@Sereach+'%' or  u.Documento like '%'+@Sereach+'%'
                        
                        ) 
                    	As uf   
                     
                     )As UsuariosX 
                     
                           join flujo.Caja UsuarioXCajaVirtual on UsuariosX.UsuarioID = UsuarioXCajaVirtual.UsuarioID and UsuarioXCajaVirtual.TipoCajaID = (select tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_VIRTUAL')

               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    RecordsConsulta = con.Query<ConsultaUsuarioBalance>(query, p, commandType: CommandType.Text).ToList();

                }

                return RecordsConsulta;

            }
            catch (Exception ex)
            {
                return new List<ConsultaUsuarioBalance>();
            }
        }



        /// <summary>
        ///   Consulta datos de  usuarios de tipo mensajero referente a su caja mediante la especificacion de su nombre o numero de documento
        /// </summary>
        /// <param name="pSearch">Nombre del  usuario que se desea consultar o No. de Documento</param>
        /// <returns></returns>
        public static List<ConsultaUsuarioBalance> GetUsuariosTipoMensajeroBalance(string pSearch)
        {
            try
            {
                List<ConsultaUsuarioBalance> RecordsConsulta;

                DynamicParameters p = new DynamicParameters();

                p.Add("@Sereach", pSearch);

                string query = @"

                     select UsuarioXCajaVirtual.CajaID, UsuariosX.UsuarioID As Posicion, UsuariosX.Nombre, UsuariosX.Documento, UsuarioXCajaVirtual.BalanceActual , UsuariosX.Activo from (
                     
                        select * from (
                        
                           select top 40 * 
                             from flujo.Usuario u 
                           where 
                                (
                                        u.TipoUsuarioID = (select top 1 tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.LogicaKey =  702) 
                                    and u.Nombre like  '%'+@Sereach+'%' or  u.Documento like '%'+@Sereach+'%'  
                                )
                                  and u.Activo = 1
                        ) 
                    	As uf   
                     
                     )As UsuariosX 
                     
                           join flujo.Caja UsuarioXCajaVirtual on UsuariosX.UsuarioID = UsuarioXCajaVirtual.UsuarioID and UsuarioXCajaVirtual.TipoCajaID = (select tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_VIRTUAL') 

               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    RecordsConsulta = con.Query<ConsultaUsuarioBalance>(query, p, commandType: CommandType.Text).ToList();

                }

                return RecordsConsulta;

            }
            catch (Exception ex)
            {
                return new List<ConsultaUsuarioBalance>();
            }
        }


        /// <summary>
        ///      Consulta los balances de un usuario ya sea por nombre o documento
        /// </summary>
        /// <param name="pSearch">Posible nombre o No. de Documento</param>
        /// <param name="pLogicaKeyTipoUsuario">Indica el Tipo de Usuario segun su LogicaKey tipo de usuario</param>
        /// <returns>
        ///       Retorna una consulta con los posibles usuarios cuyo nombre o No. de Documento comienzen segun el parametro de busqueda
        /// </returns>
        /// 
        public static List<ConsultaUsuarioBalance> GetUsuariosBalances(string pSearch, int pLogicaKeyTipoUsuario)
        {
            try
            {
                List<ConsultaUsuarioBalance> ColleccionUsuariosBalances;

                string search = pSearch.Replace("[", "[[]").Replace("%", "[%]");

                DynamicParameters p = new DynamicParameters();

                p.Add("@Sereach", search);
                p.Add("@LogicaKeyTipoUsuario", pLogicaKeyTipoUsuario);

                string query = @"

                       select  UsuarioXCajaVirtual.CajaID, UsuariosX.UsuarioID As Posicion, UsuariosX.Nombre, UsuariosX.Documento, UsuarioXCajaVirtual.BalanceActual , UsuariosX.Activo from (
                       
                          select * from (
                          
                             select top 27 * from flujo.Usuario u where u.Nombre like  concat( @Sereach,'%') or  u.Documento like concat(@Sereach,'%')
                          
                          ) As uf where uf.TipoUsuarioID = (select tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.LogicaKey = @LogicaKeyTipoUsuario )     
                       
                       )As UsuariosX 
                       
                             join flujo.Caja UsuarioXCajaVirtual on UsuariosX.UsuarioID = UsuarioXCajaVirtual.UsuarioID and UsuarioXCajaVirtual.TipoCajaID = (select tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_VIRTUAL')
                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    ColleccionUsuariosBalances = con.Query<ConsultaUsuarioBalance>(query, p, commandType: CommandType.Text).ToList();
                }

                return ColleccionUsuariosBalances ?? new List<ConsultaUsuarioBalance>();
            }
            catch (Exception ex)
            {
                return new List<ConsultaUsuarioBalance>();
            }

        }
    }
}
