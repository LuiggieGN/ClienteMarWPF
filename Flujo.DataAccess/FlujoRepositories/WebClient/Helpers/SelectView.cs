using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.Helpers
{
    public static class SelectView
    {
        public static string SelectRangoFechaMovimientos
        {
            get
            {
                string query = @"

                                  		                  select * from (
                                                                     select
                                                                       
                                                                       R1.CajaID,
                                                                       R1.Categoria, 
                                                                       R1.CategoriaSubTipoID, 
                                  							         R1.CategoriaConcepto,
                                                                       row_number() over(partition by R1.CajaID order by R1.Fecha  DESC) As Orden,
                                                                       m.MovimientoID, 
                                                                       R1.Fecha,
                                                                       R1.Referencia,
                                                                       m.Descripcion, 
                                                                       m.Monto As EntradaOSalida, 
                                                                       R1.Balance
                                                                       
                                                                     from
                                                                       (
                                                                     
                                                                        select
                                                                        
                                                                         i.CajaID, 
                                  							              'Ingreso'       As Categoria, 
                                  							         	  i.TipoIngresoID As CategoriaSubTipoID, 
                                  							         	  (select top 1 ti.TipoNombre from flujo.TipoIngreso ti Where ti.TipoIngresoID = i.TipoIngresoID) As CategoriaConcepto,
                                  							         	  i.MovimientoID, 
                                  							         	  i.FechaIngreso  As Fecha,
                                  							         	  i.Referencia,
                                  							         	  i.Balance 
                                                                        
                                                                        from flujo.Ingreso i 
                                                                        
                                                                        Where
                                                                            i.CajaID =  ( select c.CajaID from  flujo.Caja c Where  c.BancaID = @BancaID and c.TipoCajaID = (  select tc.TipoCajaID from  flujo.TipoCaja tc Where tc.Nombre = 'CAJA_TERMINAL' ) )
                                                                         and
                                    	                                   (i.FechaIngreso >= @FechaInicio and i.FechaIngreso < @FechaFin)
                                                                            
                                                                          union all
                                                                     
                                                                        select
                                                                           
                                                                        e.CajaID,
                                  							         	'Egreso'  As Categoria,								
                                  							         	 e.TipoEgresoID  As CategoriaSubTipoID,
                                  							         	 (select top 1 te.TipoNombre from flujo.TipoEgreso te Where te.TipoEgresoID = e.TipoEgresoID) As CategoriaConcepto,
                                  							         	 e.MovimientoID,
                                  							         	 e.FechaEgreso  As Fecha, 
                                  							         	 e.Referencia,
                                  							         	 e.Balance
                                                                        
                                                                        from flujo.Egreso e
                                                                        
                                                                        Where
                                                                            e.CajaID =  ( select c.CajaID from  flujo.Caja c Where  c.BancaID = @BancaID and c.TipoCajaID = (  select tc.TipoCajaID from  flujo.TipoCaja tc Where tc.Nombre = 'CAJA_TERMINAL' ) )
                                                                            and
                                    	                                       (e.FechaEgreso >= @FechaInicio and e.FechaEgreso < @FechaFin)
                                                                     
                                                                       ) R1 join flujo.Movimiento m on m.MovimientoID = R1.MovimientoID 
                                                            
                                                              ) R2
                                  
                                          ";

                return query;
            }
        }

        public static string SelectBancasActivas
        {
            get
            {
                string query = @"

                        Select 
                             b.BancaID       As Posicion,
                             b.BanNombre     As BancaNombre, 
                        	 b.BanDireccion  As BancaDireccion,	 	 
                        	 b.BanTelefono   As BancaTelefono
                          From
                             dbo.MBancas b
                          Where b.BanActivo = 1 
                          Order By b.BancaID asc;
               ";


                return query;
            }

        }

        public static string SelectCajasBalance
        {
            get
            {

                string query = @"

                    	  select 
                           c.CajaID,
                           'Mis Movimientos' As CategoriaTipo,  
                           u.Nombre As BancaNombreOTipoUsuarioNombre,
                           c.FechaBalance As FechaUltimaActualizacion,
                    	   '--' As BancaID 
                         from flujo.Caja c join flujo.Usuario u on c.UsuarioID = u.UsuarioID and u.UsuarioID = @UserID
                    	 union all
                           select
                            c.CajaID, 
                           'Entidad Financiera' as CategoriaTipo,
                            isnull(c.CajaDescripcion, '') as BancaNombreOTipoUsuarioNombre,
                            c.FechaBalance as FechaUltimaActualizacion,
                    		'--' As BancaID  
                           from flujo.Caja c 
                           where
                            c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_BANCO')
                         union all
                          select
                             c.CajaID,
                            'Banca' As CategoriaTipo,   
                             m.BanContacto As BancaNombreOTipoUsuarioNombre,
                         	 c.FechaBalance As FechaUltimaActualizacion,
                    		 convert(varchar(30),m.BancaID) As BancaID
                          from
                               flujo.Caja c  join MBancas m on c.BancaID = m.BancaID  and (@RiferoID is null or m.RiferoID = @RiferoID)
                          where 
                               c.CajaID in (select distinct cu.CajaID from flujo.Cuadre cu )
                               and m.BanActivo  = 1   --  Donde la Banca este activa	
                           and c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL')                         
                          union all                                                    
                          select 
                        	   c.CajaID,              --obtiene las cajas de un Mensajero
                        	   'Mensajero' As CategoriaTipo,
                        	   u.Nombre  As BancaNombreOTipoUsuarioNombre,
                        	   c.FechaBalance  As FechaUltimaActualizacion,
                    		   '--' As BancaID 
                          from 
                               flujo.Caja c  join flujo.Usuario u on c.UsuarioID <> @UserID and c.UsuarioID = u.UsuarioID and u.TipoUsuarioID = (select top 1 tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.LogicaKey = 702)
                          where
                        	    u.Activo = 1
                ";

                //..........................................Parametro
                //@UserID

                // string query = @"

                //          select 
                //            c.CajaID,
                //            'Mis Movimientos' As CategoriaTipo,  
                //            u.Nombre          As BancaNombreOTipoUsuarioNombre,
                //            c.BalanceActual   As Balance,
                //            c.FechaBalance    As FechaUltimaActualizacion
                //          from flujo.Caja c join flujo.Usuario u on c.UsuarioID = u.UsuarioID and u.UsuarioID = @UserID
                //          union all
                //           select
                //              c.CajaID,
                //             'Banca'                    As CategoriaTipo,   
                //              m.BanNombre       As BancaNombreOTipoUsuarioNombre,
                //              c.BalanceActual     As Balance,
                //          	 c.FechaBalance      As FechaUltimaActualizacion
                //           from
                //                flujo.Caja c  join MBancas m on c.BancaID = m.BancaID  
                //           where 
                //                   m.BanActivo  = 1   --  Donde la Banca este activa 
                //            and c.Activa          = 1   -- Y Donde la caja de la banca este activa
                //                                                  -- Y Donde el tipo de caja de la banca sea igual a Terminal
                //            and c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL')                         
                //           union all                                                    
                //           select 
                //         	   c.CajaID,              --obtiene las cajas de un Mensajero
                //         	   'Mensajero'          As CategoriaTipo,
                //         	   u.Nombre             As BancaNombreOTipoUsuarioNombre,
                //         	   c.BalanceActual   As Balance,
                //         	   c.FechaBalance    As FechaUltimaActualizacion
                //           from 
                //                flujo.Caja c  join flujo.Usuario u on c.UsuarioID <> @UserID and c.UsuarioID = u.UsuarioID and u.TipoUsuarioID = (select top 1 tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.LogicaKey = 702)
                //           where
                //         	    u.Activo = 1
                //            and  c.Activa = 1
                //";

                return query;
            }
        }

        public static string SelectCajaEspecificaBalancePorCajaID
        {
            get
            {
                //..........................................Parametro
                //@CajaID
                //@UserID

                string query = @"


						   if (@CajaID  = (select top 1 c.CajaID from flujo.Caja c where c.UsuarioID = @UserID ))
						    begin
                                select 
                                 c.CajaID,
                                 'Mis Movimientos' As CategoriaTipo,  
                                 u.Nombre As BancaNombreOTipoUsuarioNombre,
                                 c.FechaBalance As FechaUltimaActualizacion
                                from flujo.Caja c join flujo.Usuario u on c.UsuarioID = u.UsuarioID and u.UsuarioID = @UserID;
							end
						   else
						    begin
                                select
                                   c.CajaID,
                                  'Banca' As CategoriaTipo,   
                                   m.BanContacto As BancaNombreOTipoUsuarioNombre,
                                   c.FechaBalance As FechaUltimaActualizacion
                                from
                                     flujo.Caja c  join MBancas m on c.BancaID = m.BancaID  
                                where 
                                     m.BanActivo  = 1   
                                 and c.Activa = 1                                                                
                                 and c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL')      
					             and c.CajaID = @CajaID  
					             and c.BancaID is not null                 
                                 union all      
                                      select
                                         c.CajaID,
                                        'Entidad Financiera' As CategoriaTipo,   
                                         isnull(c.CajaDescripcion, '') As BancaNombreOTipoUsuarioNombre,
                                         c.FechaBalance As FechaUltimaActualizacion
                                      from
                                           flujo.Caja c    
                                      where 
                                       
                                           c.Activa = 1                                                                
                                       and c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_BANCO')      
                                       and c.CajaID = @CajaID                   
                                 union all 								                                               
                                 select 
                               	   c.CajaID,        
                               	   'Mensajero' As CategoriaTipo,
                               	   u.Nombre  As BancaNombreOTipoUsuarioNombre,
                               	   c.FechaBalance  As FechaUltimaActualizacion
                                 from 
                                      flujo.Caja c join flujo.Usuario u on c.UsuarioID = u.UsuarioID and c.CajaID = @CajaID  and u.TipoUsuarioID = (select top 1 tu.TipoUsuarioID from flujo.TipoUsuario tu where tu.LogicaKey = 702)
                                 where
                               	        u.Activo = 1
                                  and   c.Activa = 1
							end
                ";
                return query;
            }
        }

        public static string SelectMovimientosDeCajaEspecifica
        {
            get
            {
                //............................................Parametros    
                //@CajaID
                //@FechaInicio
                //@FechaFin

                string query = @"

                       select * from (
                                  select
                                    
                                    R1.CajaID,
                                    R1.Categoria, 
                                    R1.CategoriaSubTipoID, 
                                    R1.CategoriaConcepto,
                                    row_number() over(partition by R1.CajaID order by R1.Fecha  ASC) As Orden,
                                    m.MovimientoID, 
                                    R1.Fecha,
                                    R1.Referencia,
                                    m.Descripcion, 
                                    m.Monto As EntradaOSalida, 
                                    R1.Balance
                                    
                                  from
                                    (
                                  
                                     select
                                     
                                       i.CajaID, 
                                       'Ingreso'       As Categoria, 
                                	    i.TipoIngresoID As CategoriaSubTipoID, 
                                	    (select top 1 ti.TipoNombre from flujo.TipoIngreso ti Where ti.TipoIngresoID = i.TipoIngresoID) As CategoriaConcepto,
                                	    i.MovimientoID, 
                                	    i.FechaIngreso  As Fecha,
                                	    i.Referencia,
                                	    i.Balance 
                                     
                                     from flujo.Ingreso i 
                                     
                                     Where
                                         i.CajaID = @CajaID
                                      and
                                        (i.FechaIngreso >= @FechaInicio and i.FechaIngreso < @FechaFin)
                                          
                                       union all
                                  
                                     select
                                        
                                       e.CajaID,
                                	    'Egreso'  As Categoria,								
                                	    e.TipoEgresoID  As CategoriaSubTipoID,
                                	    (select top 1 te.TipoNombre from flujo.TipoEgreso te Where te.TipoEgresoID = e.TipoEgresoID) As CategoriaConcepto,
                                	    e.MovimientoID,
                                	    e.FechaEgreso  As Fecha, 
                                	    e.Referencia,
                                	    e.Balance
                                     
                                     from flujo.Egreso e
                                     
                                     Where
                                         e.CajaID =  @CajaID
                                         and
                                            (e.FechaEgreso >= @FechaInicio and e.FechaEgreso < @FechaFin)
                                  
                                    ) R1 join flujo.Movimiento m on m.MovimientoID = R1.MovimientoID 
                         
                       ) R2
                ";

                return query;
            }
        }

        public static string SelectTarjetas
        {
            get
            {
                string query = @"

                    select 
                    
                      ut.TarjetaID,
                      ut.UsuarioID,
                      ut.Serial         As Clave,
                      u.Nombre          As Propietario,
                      td.Tipo           As TipoDocumento,
                      u.Documento       As NoDocumento,
                      ut.FechaCreacion  As FechaCreacion
                      
                    
                    from        flujo.UsuarioTarjeta ut
                     inner join flujo.Usuario        u  on ut.UsuarioID = u.UsuarioID
                     inner join flujo.TipoDocumento  td on u.TipoDocumentoID = td.TipoDocumentoID
                    where
                         ut.Activo = 1
                ";

                return query;

            }
        }

        public static string VRUTAS
        {
            get
            {
                string query = @"

 		           select  
		              count(*) As TotalRecords 
		           from VRutas;		           	 
		           	         
                   select 
		             *
		           from (
		             select  
		               row_number() over (order by vr.RutaID desc) As RowRank,
		               vr.RutaID,
		               vr.TipoDeRuta,
		               vr.RutaNombre,
		               vr.MensajeroAsignado,
		               vr.Nota,
		               vr.Estado,
		               vr.OrdenRecorrido,
		               vr.UltimaLocalidad,
		               vr.FechaCreacion,
		               vr.RutaEstaActiva
		             from VRutas vr
		           )As rutas where  RowRank > @StartRowIndex and  RowRank <= (@StartRowIndex + @MaximumRows   );

               ";
                return query;
            }

        }

        public static string VMOVIMIENTOS__ADMINISTRATIVOS
        {
            get
            {
                string query = @"

                    select 
                     count(*) As TotalRecords 
                    from flujo.VMovimientos vm
                    where 
                                 vm.CajaID = @CajaID
                      and(
                            vm.LogicaKey = 100005 or vm.LogicaKey = 200004
                      )
                      and (	   
                                --Entradas & Salidas
                    	    	(@CategoriaConcepto = 1 and vm.CategoriaConcepto like '%')
                    		Or 
                    		    --Entrada 
                    		    (@CategoriaConcepto = 2 and vm.CategoriaConcepto = 'Entrada Administrativa')
                    		Or 		      
                    		    --Salida
                    			(@CategoriaConcepto = 3 and vm.CategoriaConcepto = 'Salida Administrativa')	 
                       )
                      and (	   
                                --Todas las Categorias
                    	    	(@CategoriaRef = 0 and vm.CategoriaRef like '%')
                    		Or 
                    		    --Banca de Loteria 
                    		    (@CategoriaRef = 1 and vm.CategoriaRef = 'Banca')
                    		Or 		      
                    		    --Usuario
                    			(@CategoriaRef = 2 and vm.CategoriaRef = 'Usuario')	 
                    		Or 		      
                    		    --Salida
                    			(@CategoriaRef = 3 and vm.CategoriaRef = 'Entidad Financiera')	 
                       )
                       and(
                                (@CajaRefID is null)
                    		Or
                    		    (@CajaRefID is not null and vm.CajaReferencia = @CajaRefID)
                       )  
                       and
                       (
                           vm.Fecha >= @desde and vm.Fecha <= @hasta
                       ) ;
                    
                    
                    select 
                      *
                    from (
                       select  
                          row_number() over (order by vm.Fecha desc) As RowRank,
                          vm.CajaID,   
                    	  vm.Categoria, 
                    	  vm.CategoriaSubTipoID, 
                    	  vm.CategoriaConcepto, 
                    	  vm.MovimientoID, 
                    	  vm.Fecha,
                    	  vm.Referencia,
                          vm.Descripcion, 
                          vm.EntradaOSalida, 
                    	  vm.Balance,
                    	  vm.CajaReferencia,
                    	  vm.CategoriaRef,
                    	  vm.CategoriaRefNombre
                       from flujo.VMovimientos vm
                         
                        where 
                                    vm.CajaID = @CajaID
                         and(
                               vm.LogicaKey = 100005 or vm.LogicaKey = 200004
                         )
                         and (	   
                                   --Entradas & Salidas
                       	    	(@CategoriaConcepto = 1 and vm.CategoriaConcepto like '%')
                       		Or 
                       		    --Entrada 
                       		    (@CategoriaConcepto = 2 and vm.CategoriaConcepto = 'Entrada Administrativa')
                       		Or 		      
                       		    --Salida
                       			(@CategoriaConcepto = 3 and vm.CategoriaConcepto = 'Salida Administrativa')	 
                          )
                         and (	   
                                   --Todas las Categorias
                       	    	(@CategoriaRef = 0 and vm.CategoriaRef like '%')
                       		Or 
                       		    --Banca de Loteria 
                       		    (@CategoriaRef = 1 and vm.CategoriaRef = 'Banca')
                       		Or 		      
                       		    --Usuario
                       			(@CategoriaRef = 2 and vm.CategoriaRef = 'Usuario')	 
                       		Or 		      
                       		    --Salida
                       			(@CategoriaRef = 3 and vm.CategoriaRef = 'Entidad Financiera')	 
                          )
                          and(
                                   (@CajaRefID is null)
                       		Or
                       		    (@CajaRefID is not null and vm.CajaReferencia = @CajaRefID)
                          )  
                          and
                          (
                              vm.Fecha >= @desde and vm.Fecha <= @hasta
                          ) 
                    )As VMov where  RowRank > @StartRowIndex and  RowRank <= (@StartRowIndex + @MaximumRows  );
                    
               ";
                return query;
            }




        }

    }
}