using System;
using System.Data;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Flujo.Entities.WpfClient.RequestModel;
using Flujo.Entities.WpfClient.ResponseModels;
using Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers;
using MAR.AppLogic.MARHelpers;

namespace Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories
{
    public static class CuadreRepositorio
    {


        public static CuadreResponse Procesar(CuadreModel cuadre, bool esUnRetiro)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); CuadreResponse response = null;
                p.Add("@CuadreTipo", cuadre.Tipo);
                p.Add("@BanCajaID", cuadre.CajaID);
                p.Add("@BancaBalanceMin", cuadre.BalanceMinimo);
                p.Add("@BanDineroPorPagar", cuadre.MontoPorPagar);
                p.Add("@BanMontoContado", cuadre.MontoContado);
                p.Add("@Cajera", cuadre.UsuarioCaja);
                p.Add("@CajaOrigenID", cuadre.CajaOrigenID);
                p.Add("@UsuaOrigenID", cuadre.UsuarioOrigenID);
                p.Add("@MontoRetirado", cuadre.MontoRetirado);
                p.Add("@MontoDepositado", cuadre.MontoDepositado);
                p.Add("@EsUnDeposito", esUnRetiro);
                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();
                    response = con.Query<CuadreResponse>("[flujo].[Sp_CuadrarBanca]", p, commandType: CommandType.StoredProcedure).FirstOrDefault();
                }
                return response;
            }
            catch (Exception ex)
            {
                return new CuadreResponse() { FueProcesado = false, Error = ex.Message + ". " + ex.StackTrace };
            }
        }

        public static bool GuardarCuadre(CuadreModel cuadre)
        {
            if (cuadre == null) return false;

            try
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@CajaID", cuadre.CajaID);
                p.Add("@UsuarioCaja", cuadre.UsuarioCaja);
                p.Add("@BalanceAnterior", cuadre.BalanceAnterior);
                p.Add("@CajaOrigenID", cuadre.CajaOrigenID);
                p.Add("@UsuarioOrigenID", cuadre.UsuarioOrigenID);
                p.Add("@CuadreAnteriorID", cuadre.CuadreAnteriorID);
                p.Add("@BalanceMinimo", cuadre.BalanceMinimo);
                p.Add("@MontoPorPagar", cuadre.MontoPorPagar);
                p.Add("@MontoReal", cuadre.MontoReal);
                p.Add("@MontoContado", cuadre.MontoContado);
                p.Add("@MontoFaltante", cuadre.MontoFaltante);
                p.Add("@MontoRetirado", cuadre.MontoRetirado);
                p.Add("@MontoDepositado", cuadre.MontoDepositado);
                p.Add("@Balance", cuadre.Balance);


                string query = @"
                                  insert into flujo.Cuadre(CajaID, CajaOrigenID, CuadreAnteriorID, MontoContado, MontoReal, MontoRetirado, Balance, MontoFaltante, Fecha, MontoDepositado, BalanceAnterior, BalanceMinimo,MontoPorPagar, UsuarioCaja, UsuarioOrigenID)
                                  values(
                                     @CajaID,
                                     @CajaOrigenID,
                                     @CuadreAnteriorID, 
                                     @MontoContado, 
                                     @MontoReal,
                                     @MontoRetirado,
                                     @Balance, 
                                     @MontoFaltante,
                                     getdate(),
                                     @MontoDepositado,
                                     @BalanceAnterior,
                                     @BalanceMinimo,
                                     @MontoPorPagar,
                                     @UsuarioCaja,
                                     @UsuarioOrigenID                             
                                  );
                                ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    int affectedRows = con.Execute(query, p, commandType: CommandType.Text);

                    if (affectedRows > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool EstaBancaPoseeCuadreInical(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool bancaPoseeCuadreInicial = false;
                p.Add("@BanID", pBancaID);

                string query = @"
                                    select isnull(
                                        ( select top 1 1 from flujo.Cuadre cu where cu.CajaID = (select top 1 c.CajaID from flujo.Caja c where c.BancaID = @BanID) and cu.CuadreAnteriorID is null )                                    
                                   , 0) As PoseeCuadreInical
                               ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    bancaPoseeCuadreInicial = con.Query<bool>(query, p, commandType: CommandType.Text).First();
                }

                return bancaPoseeCuadreInicial;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static CuadreModel GetCuadre(int pCuadreID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); CuadreModel cuadre = null;

                p.Add("@CuadreID", pCuadreID);

                string query = @"select *  from flujo.Cuadre c where c.CuadreID = @CuadreID";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<CuadreModel> TheListOfCuadres = con.Query<CuadreModel>(query, p, commandType: CommandType.Text).ToList();

                    if (TheListOfCuadres.Any())
                    {
                        cuadre = TheListOfCuadres.First();
                    }

                }

                return cuadre;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int? GetBancaLastCuadre_ID(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); int? LastCuadreID = 0;

                p.Add("@BanID", pBancaID);

                string query = @"
                                  select top 1 
                                    c.CuadreID 
                                  from flujo.Cuadre c
                                  where c.CajaID = (select top 1 c.CajaID from flujo.Caja c where c.BancaID = @BanID)
                                  order by c.CuadreID desc;
                                ";


                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<int> TheListOfCuadreIDs = con.Query<int>(query, p, commandType: CommandType.Text).ToList();

                    if (!TheListOfCuadreIDs.Any())
                    {
                        LastCuadreID = null;
                    }
                    else
                    {
                        LastCuadreID = TheListOfCuadreIDs.First();
                    }
                }

                return LastCuadreID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BancaTransaccionesResponse> GetTransaccionesDesdeUltimoCuadre(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); List<BancaTransaccionesResponse> TheListOfTransacciones = null;

                p.Add("@BanID", pBancaID);

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    TheListOfTransacciones = con.Query<BancaTransaccionesResponse>("[flujo].[Sp_ConsultaBancaTransaccionesDesdeUltimoCuadre]", p, commandType: CommandType.StoredProcedure).ToList();

                    if (!TheListOfTransacciones.Any())
                    {
                        TheListOfTransacciones = null;
                    }
                }

                return TheListOfTransacciones;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AsentarTransaccionesDesdeUltimoCuadre(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); int rowAffected = 0;

                p.Add("@BanID", pBancaID);


                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    rowAffected = con.Execute("[flujo].[Sp_AsentarMovimientoBanca]", p, commandType: CommandType.StoredProcedure);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void RegistarMovimientoCuadre(int pCajaID, string TipoMovimiento, decimal pMonto)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); p.Add("@CajaID", pCajaID);

                string q1 = @" select  isnull( (  select top 1  1  from flujo.Caja c where c.CajaID = @CajaID  and c.TipoCajaID = ( select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL'  ) ), 0  )  As CajaEsTipoTerminal";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    bool CajaEsTipoTerminal = con.QueryFirst<bool>(q1, p, commandType: CommandType.Text);

                    p.Add("@Monto", pMonto);

                    if (CajaEsTipoTerminal)
                    {
                        // ## Banca (Caja de tipo terminal)
                        if (TipoMovimiento.Equals("Entrada"))
                        {
                            con.Execute(@"
						          insert into flujo.Movimiento(TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
					              values (
					                  -1,
					                  @CajaID,
					                  -1,
					                  getdate(),
					                  @Monto,
				   	                  'Depósito de cuadre',
					  	              'Procesado',
					  	              getdate(), 
					  	              1 
					               );	
				                 insert into [flujo].[Ingreso] (CajaID, EgresoID, TipoIngresoID, MovimientoID, Balance, FechaIngreso, Referencia)
						         values(						        	
						         	    @CajaID,             
						         	    null,                     
						         	    (select top 1  ti.TipoIngresoID from flujo.TipoIngreso ti where ti.LogicaKey = 100001),                        
						         	    scope_identity(),
						         	    0,                         
						         	    getdate(),
						         	    ''     
							     );	
                             ", p, commandType: CommandType.Text);
                        }
                        else
                        {
                            con.Execute(@"
						          insert into flujo.Movimiento(TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
					              values (
					                  -1,
					                  @CajaID,
					                  -1,
					                  getdate(),
					                  @Monto,
				   	                  'Salida de cuadre',
					  	              'Procesado',
					  	              getdate(), 
					  	              1 
					               );	
                                 insert into [flujo].[Egreso]  (CajaID, TipoEgresoID, IngresoID, MovimientoID, Balance, FechaEgreso, Referencia )
                                 values (
						             @CajaID                    
                                   ,(select top 1 te.TipoEgresoID from flujo.TipoEgreso te where te.LogicaKey = 200001)                                
                                   , null                          
                                   , scope_identity() 
                                   , 0                               
                                   , getdate()
                                   , ''
						         );	
                             ", p, commandType: CommandType.Text);
                        }

                    }
                    else
                    {
                        // ## Entidad X (Caja de tipo virtual)

                        if (TipoMovimiento.Equals("Entrada"))
                        {
                            con.Execute(@"

                                   declare @MontoActualizado money = 0;
                                   declare @MontoViejo money = 0; 
                                   
                                   select top 1  @MontoViejo = c.BalanceActual from flujo.Caja c where  c.CajaID = @CajaID;
                                   
                                   set @MontoActualizado = isnull(@MontoViejo, 0) + @Monto;
                                   
                                   update flujo.Caja set BalanceActual= @MontoActualizado where CajaID = @CajaID;

						           insert into flujo.Movimiento(TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
					               values (
					                   -1,
					                   @CajaID,
					                   -1,
					                   getdate(),
					                   @Monto,
				   	                   'Depósito de cuadre',
					  	               'Procesado',
					  	               getdate(), 
					  	               1 
					                );	

				                   insert into [flujo].[Ingreso] (CajaID, EgresoID, TipoIngresoID, MovimientoID, Balance, FechaIngreso, Referencia)
						           values(						        	
						          	    @CajaID,             
						          	    null,                     
						          	    (select top 1  ti.TipoIngresoID from flujo.TipoIngreso ti where ti.LogicaKey = 100001),                        
						          	    scope_identity(),
						          	    @MontoActualizado,                         
						          	    getdate(),
						          	    ''     
							       );	
                             ", p, commandType: CommandType.Text);
                        }
                        else
                        {
                            con.Execute(@"

                                declare @MontoActualizado money = 0;
                                declare @MontoViejo money = 0;                                 
                                select top 1  @MontoViejo = c.BalanceActual from flujo.Caja c where  c.CajaID = @CajaID;
                                
                                set @MontoActualizado = isnull(@MontoViejo, 0) - @Monto ;
                                
                                update flujo.Caja set BalanceActual= @MontoActualizado where CajaID = @CajaID;


						          insert into flujo.Movimiento(TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
					              values (
					                  -1,
					                  @CajaID,
					                  -1,
					                  getdate(),
					                  @Monto,
				   	                  'Salida de cuadre',
					  	              'Procesado',
					  	              getdate(), 
					  	              1 
					               );	
                                 insert into [flujo].[Egreso]  (CajaID, TipoEgresoID, IngresoID, MovimientoID, Balance, FechaEgreso, Referencia )
                                 values (
						             @CajaID                    
                                   ,(select top 1 te.TipoEgresoID from flujo.TipoEgreso te where te.LogicaKey = 200001)                                
                                   , null                          
                                   , scope_identity() 
                                   , @MontoActualizado                              
                                   , getdate()
                                   , ''
						         );	

                             ", p, commandType: CommandType.Text);
                        }
                    }

                }// Fin Using
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void RegistarMontoAfavorCuadre(int pCajaID, string TipoMovimiento, decimal pMonto)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); p.Add("@CajaID", pCajaID);

                string ConsultaSiCajaEstipoTerminal = @" select  isnull( (  select top 1  1  from flujo.Caja c where c.CajaID = @CajaID  and c.TipoCajaID = ( select top 1 tc.TipoCajaID from flujo.TipoCaja tc where tc.Nombre = 'CAJA_TERMINAL'  ) ), 0  )  As CajaEsTipoTerminal";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    bool CajaEsTipoTerminal = con.QueryFirst<bool>(ConsultaSiCajaEstipoTerminal, p, commandType: CommandType.Text);

                    p.Add("@Monto", pMonto);

                    if (CajaEsTipoTerminal)
                    {
                        // ## Banca (Caja de tipo terminal)
                        if (TipoMovimiento.Equals("Entrada"))
                        {
                            con.Execute(@"
						          insert into flujo.Movimiento(TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
					              values (
					                  -1,
					                  @CajaID,
					                  -1,
					                  getdate(),
					                  @Monto,
				   	                  'Sobrante en caja',
					  	              'Procesado',
					  	              getdate(), 
					  	              1 
					               );	
				                 insert into [flujo].[Ingreso] (CajaID, EgresoID, TipoIngresoID, MovimientoID, Balance, FechaIngreso, Referencia)
						         values(						        	
						         	    @CajaID,             
						         	    null,                     
						         	    (select top 1  ti.TipoIngresoID from flujo.TipoIngreso ti where ti.LogicaKey = 100004),                        
						         	    scope_identity(),
						         	    0,                         
						         	    getdate(),
						         	    ''     
							     );	
                             ", p, commandType: CommandType.Text);
                        }

                    }

                }// Fin Using
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal GetMontoTotalizadoTiketsPendientesDePagoSinReclamar(int pBancaID)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); decimal PagosPendienteSinReclamar = 0;
                p.Add("@BancaID", pBancaID);

                string query = @"
                                 select flujo.GetBancaTotalTiketsPendienteDePago(@BancaID) As PorPagar
                              ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<decimal> TheListOfPagos = con.Query<decimal>(query, p, commandType: CommandType.Text).ToList();

                    if (TheListOfPagos.Any())
                    {
                        PagosPendienteSinReclamar = TheListOfPagos.First();
                    }
                }

                return PagosPendienteSinReclamar;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public static bool EnlazaCuadreConAsignacion(string rutaEstado, int rutaUltimaLocalidad, string rutaOrdenRecorrido, int cuadreId, int bancaCajaId , int rutaId)
        {
            try
            {
                DynamicParameters p = new DynamicParameters(); bool fueEnlazado = false;
                p.Add("@rutaEstado", rutaEstado);
                p.Add("@rutaUltimaLocalidad", rutaUltimaLocalidad);
                p.Add("@rutaOrdenRecorrido", rutaOrdenRecorrido);
                p.Add("@cuadreId", cuadreId);
                p.Add("@bancaCajaId", bancaCajaId);
                p.Add("@rutaId", rutaId);

                string query = @"
                                  update flujo.Ruta set Estado = @rutaEstado, UltimaLocalidad = @rutaUltimaLocalidad, OrdenRecorrido = @rutaOrdenRecorrido where RutaID = @rutaId;
                                  update flujo.Cuadre set RutaID = 0 where CajaID = @bancaCajaId and  RutaID = @rutaId;
                                  update flujo.Cuadre set RutaID = @rutaId where CuadreID = @cuadreId;
                                  select 1;
                              ";

                using (var con = DapperDBHelper.GetSqlConnection())
                {
                    con.Open();

                    List<bool> estados = con.Query<bool>(query, p, commandType: CommandType.Text).ToList();

                    if (estados.Any())
                    {
                        fueEnlazado = estados.First();
                    }
                    con.Close();
                }

                return fueEnlazado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


 



    }
}