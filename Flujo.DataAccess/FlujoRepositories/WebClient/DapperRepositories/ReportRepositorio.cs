
using System;
using System.Linq;
using System.Data;
using Dapper;
using System.Collections.Generic;

using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;

using Flujo.Entities.WebClient.POCO;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class ReportRepositorio
    {


        public static List<SobranteFaltanteRecord> GetSumatoriaSobranteYFaltante(string pBanca, string pRifero, string pCajeraResponsable, string pTipoArqueo, DateTime pFInico, DateTime pFFin)
        {
            try
            {
                List<SobranteFaltanteRecord> records = null;

                DynamicParameters p = new DynamicParameters();
                p.Add("@SearchBanca", pBanca);
                p.Add("@SearchRifero", pRifero);
                p.Add("@CajeraResponsable", pCajeraResponsable);
                p.Add("@TipoArqueo", pTipoArqueo);
                p.Add("@Desde", pFInico);
                p.Add("@Hasta", pFFin);

                string query = @"
                            		if((select * from flujo.IsInt32(@SearchBanca)) is not null )
                            		   begin		     
                            			  --@BancaID ( int )
                            			  if((select * from flujo.IsInt32(@SearchRifero)) is not null)
                            			    begin
                            				   
                            				   --@RiferoID      ( int )
                            				                    
                             					          select 
                            					            sum(w.Monto) As Monto, w.TipoDeArqueo 
                            					          from VFaltanteSobrante w where w.BancaID =  ltrim (rtrim( @SearchBanca )) and w.RiferoID =  rtrim(ltrim( @SearchRifero )) and (						
                            					        	       w.CajeraResponsable like ltrim(rtrim(@CajeraResponsable)) + '%'  
                            					        	   and w.TipoDeArqueo      like ltrim(rtrim( @TipoArqueo ))  + '%'
                            					        	   and w.Fecha >= @Desde and w.Fecha <= @Hasta							   			
                            		         		      )                      
                                                          group by w.TipoDeArqueo
                            				end
                            			  else 
                            			    begin								   
                            				   --@RiferoNombre ( varchar )           
                                                 
                             					          select 
                            					              sum(w.Monto) As Monto, w.TipoDeArqueo 
                            					          from VFaltanteSobrante w where w.BancaID =  ltrim (rtrim( @SearchBanca )) and w.RifNombre like  rtrim(ltrim( @SearchRifero )) + '%' and (						
                            					        	       w.CajeraResponsable like ltrim(rtrim(@CajeraResponsable)) + '%'  
                            					        	   and w.TipoDeArqueo      like ltrim(rtrim( @TipoArqueo ))  + '%'
                            					        	   and w.Fecha >= @Desde and w.Fecha <= @Hasta							   			
                            		         		      )     
                            							  group by w.TipoDeArqueo                                                 
                            				end
                            		   end
                            		else
                            		   begin		     
                            			  --@BancaNombre( varchar )
                            			  if((select * from flujo.IsInt32(@SearchRifero)) is not null)
                            			     begin
                            				    --@RiferoID      ( int )								    
                            			                      
                             					          select 
                                                             sum(w.Monto) As Monto, w.TipoDeArqueo 
                            					          from VFaltanteSobrante w where w.BanContacto like ltrim (rtrim( @SearchBanca )) + '%' and w.RiferoID =  rtrim(ltrim( @SearchRifero )) and (						
                            					        	       w.CajeraResponsable like ltrim(rtrim(@CajeraResponsable)) + '%'  
                            					        	   and w.TipoDeArqueo      like ltrim(rtrim( @TipoArqueo ))  + '%'
                            					        	   and w.Fecha >= @Desde and w.Fecha <= @Hasta							   			
                            		         		      )                                    
                            							  group by w.TipoDeArqueo 
                            				 end
                            			  else
                            			     begin
                            				     --@RiferoNombre      ( varchar )
                                                   
                             					          select 
                                                              sum(w.Monto) As Monto, w.TipoDeArqueo 
                            					          from VFaltanteSobrante w where w.BanContacto like ltrim (rtrim( @SearchBanca )) + '%' and w.RifNombre like  rtrim(ltrim( @SearchRifero )) + '%' and (						
                            					        	       w.CajeraResponsable like ltrim(rtrim(@CajeraResponsable)) + '%'  
                            					        	   and w.TipoDeArqueo      like ltrim(rtrim( @TipoArqueo ))  + '%'
                            					        	   and w.Fecha >= @Desde and w.Fecha <= @Hasta							   			
                            		         		      )                                   
                             		                       group by w.TipoDeArqueo
                            
                            				 end
                            		   end
                            
               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    records = con.Query<SobranteFaltanteRecord>(query, p, commandType: CommandType.Text).ToList();
                }

                return records;
            }
            catch (Exception ex)
            {
                return new List<SobranteFaltanteRecord>();
            }
        }


        public static List<string> GetCajeraReponsableFaltanteSobrante(string pSearch)
        {
            try
            {
                List<string> records = null;

                DynamicParameters p = new DynamicParameters();

                p.Add("@Search", pSearch);

                string query = @"

                  select top 30 c.UsuarioCaja from flujo.Cuadre c where c.UsuarioCaja like ltrim(rtrim(@Search))+'%' group by c.UsuarioCaja

               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    records = con.Query<string>(query, p, commandType: CommandType.Text).ToList();
                }

                return records;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }


        public static List<CategoriaBalance> GetBalancesTotalesPorCategoria()
        {
            try
            {
                List<CategoriaBalance> ColeccionCategorias;

                string query = @"

                       select
                                  T.CategoriaTipo As Categoria,
  	                     sum( T.Balance)          As BalanceTotal
                          from
                          (
                            select
                               c.CajaID,
                               'Banca'                As CategoriaTipo,   
                               m.BanNombre    As BancaNombreOTipoUsuarioNombre,
                               c.BalanceActual  As Balance,
                           	  c.FechaBalance    As FechaUltimaActualizacion
                            from
                                 flujo.Caja c  join MBancas m on c.BancaID = m.BancaID  
                            where 
                                 m.BanActivo  = 1   --   Donde la Banca este activa 
                             and c.Activa       = 1   --Y Donde la caja de la banca este activa
                                                                --Y Donde el tipo de caja de la banca sea igual a Terminal
                             and c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL')      

                            union all

                            select 
                          	    c.CajaID,              --obtiene las cajas de un Mensajero
                          	   'Mensajero'           As CategoriaTipo,
                                 u.Nombre            As BancaNombreOTipoUsuarioNombre,
                          	    c.BalanceActual   As Balance,
                          	    c.FechaBalance    As FechaUltimaActualizacion
                            from 
                                 flujo.Caja c  join flujo.Usuario u on c.UsuarioID = u.UsuarioID and u.TipoUsuarioID = (select top 1 tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.LogicaKey = 702)
                            where
                          	    u.Activo = 1
                            and  c.Activa = 1	 		 	
                      
                        ) As T 
                        
                        group by T.CategoriaTipo;                    
                 ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    ColeccionCategorias = con.Query<CategoriaBalance>(query, commandType: CommandType.Text).ToList();

                }

                return ColeccionCategorias;
            }
            catch (Exception ex)
            {
                return new List<CategoriaBalance>();
            }

        }
    }
}