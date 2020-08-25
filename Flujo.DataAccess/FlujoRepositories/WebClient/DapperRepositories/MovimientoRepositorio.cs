
using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using Dapper;
using Flujo.Entities.WebClient.POCO;
using Flujo.Entities.WebClient.Enums;
using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;
using MAR.AppLogic.MARHelpers;
using Flujo.Entities.WebClient.RequestModels;

namespace Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories
{
    public static class MovimientoRepositorio
    {

        public static MovimientoInsertEstado CrearMovimiento(MovimientoRequestModel peticion, Usuario usuarioQueRealizoMovimiento)
        {
            try
            {
                MovimientoInsertEstado estadoInsert = null;

                DynamicParameters p = new DynamicParameters();

                p.Add("@TipoCajaOrigen", peticion.CategoriaDesde);
                p.Add("@TipoCajaDestino", peticion.CategoriaHasta);
                p.Add("@CajaOrigenID", peticion.CajaDesde.CajaID);
                p.Add("@CajaDestino", peticion.CajaHasta.CajaID);
                p.Add("@UsuarioID", usuarioQueRealizoMovimiento.UsuarioID);
                p.Add("@Descripcion", peticion.Descripcion);
                p.Add("@Monto", peticion.Monto);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    estadoInsert = con.Query<MovimientoInsertEstado>("[flujo].[Sp_RegistrarMovimientoDesdeCajaOrigenACajaDestino]", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                return estadoInsert;
            }
            catch (Exception ex)
            {
                MovimientoInsertEstado estadoError = new MovimientoInsertEstado() { FechaCreacion = DateTime.Now, MensajeError = $"{ex.Message}. {ex.StackTrace}", MovimientoCajaDestino = (int?)null, MovimientoCajaOrigen = (int?)null, MovimientoFueProcesado = false };
                return estadoError;
            }
        }


        public static bool AplicarMovimientoACaja
            (

                Caja pCajaDondeSeRealizaMovimiento,
                string pEntradaOSalida,
                FlujoTipoCategoria pTipoIngresoOTipoEgreso,
                decimal pMonto,
                string pDescripcion,
                Caja pCajaQuerealizaMovimiento,
                int? pCajaRefID

            )
        {

            try
            {

                bool MovimientoFueAplicado = false;

                DynamicParameters p = new DynamicParameters();

                int NewMovimientoID = -1;

                p.Add("@CajaIDDondeSeRealizoMovimiento", pCajaDondeSeRealizaMovimiento.CajaID);
                p.Add("@UsuarioQueRealizoMovimiento", pCajaQuerealizaMovimiento.UsuarioID);
                p.Add("@Monto", pMonto);
                p.Add("@Descripcion", pDescripcion);


                string query = @"

                   insert into flujo.Movimiento
                              (
                   		          TipoMovimientoID
                                  ,CajaID
                                  ,UsuarioID
                                  ,Fecha
                                  ,Monto
                                  ,Descripcion
                                  ,Estado
                                  ,FechaUltimaActualizacion
                                  ,Activo
                   		   )
                        values
                              (
                   		        -1
                              , @CajaIDDondeSeRealizoMovimiento
                              , @UsuarioQueRealizoMovimiento
                              , getdate()
                              , @Monto
                              , @Descripcion
                              , 'Procesado'
                              , getdate()
                              , 1
                   		   );
                   
                    select scope_identity();
                   
            ";


                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<int> ListaIds = con.Query<int>(query, p, commandType: CommandType.Text).ToList();

                    if (ListaIds.Any())
                    {
                        NewMovimientoID = ListaIds.First();
                    }

                    if (NewMovimientoID.Equals(-1))
                    {
                        return false;
                    }

                    //Apartir de aqui voy a tener el ID de mi movimiento {NewMovimientoID}

                    if (pEntradaOSalida.Equals("Entrada"))
                    {

                        DynamicParameters p1 = new DynamicParameters();

                        p1.Add("@CajaDondeSeRealizaMovimiento", pCajaDondeSeRealizaMovimiento.CajaID);
                        p1.Add("@TipoIngresoID", pTipoIngresoOTipoEgreso.Posicion);
                        p1.Add("@MovimientoID", NewMovimientoID);
                        p1.Add("@MontoEnviado", pMonto);
                        p1.Add("@Referencia", GetUniqueKey("I"));
                        p1.Add("@CajaRefID", pCajaRefID);

                        string queryIngreso = @"

                                if exists (

                                   select top 1
                                           * 
                                   from 
                                         flujo.Caja c 
                                   where 
                                           c.CajaID = @CajaDondeSeRealizaMovimiento and c.TipoCajaID =  ( select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL' )
                                )
                                begin
 
				                        insert into flujo.Ingreso (CajaID, EgresoID, TipoIngresoID, MovimientoID, Balance, FechaIngreso, Referencia, CajaIDReferencia)
						                values(						        	
						                	    @CajaDondeSeRealizaMovimiento,             
						                	    null,                     
						                	    @TipoIngresoID,                        
						                	    @MovimientoID,
						                	    0,                         
						                	    getdate(),
						                	    @Referencia,
                                                @CajaRefID
							            );	

                                end
                                else 
                                   begin

                                        declare @MontoActualizado money = 0;
                                        declare @MontoViejo money = 0;    

                                        select top 1  @MontoViejo = c.BalanceActual from flujo.Caja c where  c.CajaID = @CajaDondeSeRealizaMovimiento;                                   
                                        set @MontoActualizado = isnull(@MontoViejo, 0) + @MontoEnviado;                                   
                                       
                                        update flujo.Caja set BalanceActual= @MontoActualizado where CajaID = @CajaDondeSeRealizaMovimiento;             

                                        insert into flujo.Ingreso
                                                   (
                                        		    CajaID
                                                   ,EgresoID
                                                   ,TipoIngresoID
                                                   ,MovimientoID
                                                   ,Balance
                                                   ,FechaIngreso
                                                   ,Referencia
                                                   ,CajaIDReferencia
                                        		   )
                                             values
                                                   (
                                        		       @CajaDondeSeRealizaMovimiento
                                                     , null
                                                     , @TipoIngresoID
                                                     , @MovimientoID
                                                     , @MontoActualizado
                                                     , getdate()
                                                     , @Referencia
                                                     , @CajaRefID
                                        		   );
                                   end        

                        ";

                        int affectedIngreso = con.Execute(queryIngreso, p1, commandType: CommandType.Text);

                        if (affectedIngreso > 0)
                        {
                            MovimientoFueAplicado = true;
                        }
                        else
                        {
                            MovimientoFueAplicado = false;
                        }
                    }
                    else
                    {
                        //$$ Bloque de Codigo de Salida

                        DynamicParameters p2 = new DynamicParameters();

                        p2.Add("@CajaDondeSeRealizaMovimiento", pCajaDondeSeRealizaMovimiento.CajaID);
                        p2.Add("@TipoEgresoID", pTipoIngresoOTipoEgreso.Posicion);
                        p2.Add("@MovimientoID", NewMovimientoID);
                        p2.Add("@MontoEnviado", pMonto);
                        p2.Add("@Referencia", GetUniqueKey("E"));
                        p2.Add("@CajaRefID", pCajaRefID);


                        string queryEgreso = @"

                                if exists (
                                   select top 1 
                                      * from flujo.Caja c 
                                   where 
                                     c.CajaID = @CajaDondeSeRealizaMovimiento and c.TipoCajaID =  ( select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL' )
                                )
                                begin
                                         insert into flujo.Egreso  (CajaID, TipoEgresoID, IngresoID, MovimientoID, Balance, FechaEgreso, Referencia, CajaIDReferencia )
                                         values (
						                     @CajaDondeSeRealizaMovimiento                    
                                           , @TipoEgresoID                              
                                           , null                          
                                           , @MovimientoID
                                           , 0                               
                                           , getdate()
                                           , @Referencia
                                           , @CajaRefID
						                 );	
                                end
                                else 
                                begin

                                         declare @MontoActualizado money = 0;
                                         declare @MontoViejo money = 0; 
                                         
                                         select top 1  @MontoViejo = c.BalanceActual from flujo.Caja c where  c.CajaID = @CajaDondeSeRealizaMovimiento;                                         
                                         set @MontoActualizado = isnull(@MontoViejo, 0) - @MontoEnviado;                                         
                                         update flujo.Caja set BalanceActual= @MontoActualizado where CajaID = @CajaDondeSeRealizaMovimiento;
                                         
                                         insert into flujo.Egreso
                                                    (
                                         		     CajaID
                                                    ,TipoEgresoID
                                                    ,IngresoID
                                                    ,MovimientoID
                                                    ,Balance
                                                    ,FechaEgreso
                                                    ,Referencia
                                                    ,CajaIDReferencia
                                         		   )
                                              values
                                                    (
                                         		     @CajaDondeSeRealizaMovimiento
                                                    ,@TipoEgresoID
                                                    ,null
                                                    ,@MovimientoID
                                                    ,@MontoActualizado
                                                    ,getdate()
                                                    ,@Referencia
                                                    ,@CajaRefID
                                         		   );
                                end

                        ";

                        int affectedEgreso = con.Execute(queryEgreso, p2, commandType: CommandType.Text);

                        if (affectedEgreso > 0)
                        {
                            MovimientoFueAplicado = true;
                        }
                        else
                        {
                            MovimientoFueAplicado = false;
                        }

                        MovimientoFueAplicado = true;
                    }

                }//Fin Using                  

                return MovimientoFueAplicado;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static string GetUniqueKey(string Code)
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return "UD" + Code + string.Format("{0:x}", i - DateTime.Now.Ticks).ToUpper();
        }


        /***  Metodos de Consulta*/
        public static List<MovimientoDatos> ConsultarLastXRecords(int pCajaID, int pSeleccionaXRecords)
        {
            try
            {
                List<MovimientoDatos> coleccionMovimientos = new List<MovimientoDatos>();

                DynamicParameters p = new DynamicParameters();

                p.Add("@CajaID", pCajaID);
                p.Add("@SelectXRecord", pSeleccionaXRecords);

                string query = @"

		                  select * from (
         select           
           R1.CajaID,
           R1.Categoria, 
           R1.CategoriaSubTipoID, 
    		R1.CategoriaConcepto,
           row_number() over(partition by R1.CajaID order by R1.Fecha  DESC ) As Orden,
           m.MovimientoID, 
           R1.Fecha,
           R1.Referencia,
           m.Descripcion, 
           m.Monto As EntradaOSalida, 
           R1.Balance,
           R1.CajaReferencia,
		   CajaRef.CategoriaRef,
		   CajaRef.CategoriaRefNombre
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
    		        	  i.Balance,
    		        	  i.CajaIDReferencia  As  CajaReferencia       
                    from flujo.Ingreso i         
                    Where
                        i.CajaID =  @CajaID            
                      union all     
                    select           
                         e.CajaID,
    		        	'Egreso'  As Categoria,								
    		        	 e.TipoEgresoID  As CategoriaSubTipoID,
    		        	 (select top 1 te.TipoNombre from flujo.TipoEgreso te Where te.TipoEgresoID = e.TipoEgresoID) As CategoriaConcepto,
    		        	 e.MovimientoID,
    		        	 e.FechaEgreso  As Fecha, 
    		        	 e.Referencia,
    		        	 e.Balance,
    		        	 e.CajaIDReferencia As CajaReferencia
                    
                    from flujo.Egreso e
                    
                    Where
                        e.CajaID =  @CajaID
         
           ) R1 join flujo.Movimiento m on m.MovimientoID = R1.MovimientoID left join (  	     
		       select c.CajaID, m.BanContacto      As CategoriaRefNombre , 'Banca'   As CategoriaRef from flujo.Caja c join MBancas m on c.BancaID = m.BancaID where c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre ='CAJA_TERMINAL')	
		        union all
		       select c.CajaID, u.Nombre           As CategoriaRefNombre , 'Usuario' As CategoriaRef from flujo.Caja c join flujo.Usuario u on c.UsuarioID = u.UsuarioID where c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre ='CAJA_VIRTUAL')	
		        union all
		       select c.CajaID, c.CajaDescripcion  As CategoriaRefNombre , 'Entidad Financiera' As CategoriaRef from flujo.Caja c where c.TipoCajaID = (select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre ='CAJA_BANCO')			
			) As CajaRef on R1.CajaReferencia = CajaRef.CajaID 
                          
                            ) R2 Where R2.Orden <= @SelectXRecord

               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {

                    con.Open();

                    List<MovimientoDatos> rr = con.Query<MovimientoDatos>(query, p, commandType: CommandType.Text).ToList();

                    if (rr.Any())
                    {
                        coleccionMovimientos.AddRange(rr);
                    }

                    con.Close();
                }
                return coleccionMovimientos;

            }
            catch (Exception e)
            {
                return new List<MovimientoDatos>();
            }
        }
    }
}