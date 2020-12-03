namespace MAR.DataAccess.ControlEfectivoRepositories.Helpers
{
    internal static class CajaHelper
    {
        internal static string Procedure_SP_CAJA_BALANCE_ACTUAL = @"[flujo].[Sp_ConsultaCajaBalanceActual]";


        internal static string SelectCajaDeUsuarioPorUsuarioId = @"
           select top 1
                CajaID
               ,TipoCajaID
               ,ZonaID
               ,CajaDescripcion
               ,UsuarioID
               ,BancaID
               ,Ubicacion
               ,BalanceActual
               ,BalanceMinimo
               ,FechaBalance
               ,FechaCreacion
               ,Disponible
               ,Activa
           from flujo.Caja  where UsuarioID = @usuarioid
        ";


        internal static string QueryParaRegistrarMovimientoEnBanca = @"


begin try

	begin transaction SupraMovimientoEnBanca
	declare @movimientoid     int = null;
	declare @fecha  datetime = getdate();
 
     
	if(
	  @cajaid is not null
	)
	  begin	  
	  	  	  	
      insert into flujo.Movimiento(TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
           VALUES
                 (
      		      0
                 ,@cajaid
                 ,@usuarioid
                 ,@fecha
                 ,@monto
                 ,@comentario
                 ,'Procesado'
                 ,@fecha
                 , 1
      		   );    

	  declare @filaFueInsertada int = @@rowcount;	  
	  set @movimientoid = scope_identity();

	       if(		   
		      @filaFueInsertada > 0 and @movimientoid is not null
		   )
            begin

			    if(				
				   @ie = 1      --Ingreso
				)  
				  begin
				   
				     declare @ingresoId int = null;				     
					 declare @numerosI int = 0;
					 declare @Ref__Ingresos varchar(30);

					 set @numerosI =  isnull ((select count(*) from flujo.Ingreso i where i.CajaID = @cajaid ), 0 ) + 1;
					 set @Ref__Ingresos = 'I'+ right('0000' + convert(varchar(4), abs(@bancaid)), 4 )+ right('0000000000' + convert(varchar(10), abs(@numerosI)), 10);
				      
				     insert into flujo.Ingreso(CajaID, EgresoID, TipoIngresoID, MovimientoID, Balance, FechaIngreso, Referencia)
                     VALUES
                           (
                             @cajaid
                           , null
                           , 1
                           , @movimientoid
                           , 0
                           , @fecha
                           , @Ref__Ingresos
                     );
					
					declare @filaFueInsertadaIngreso int = @@rowcount;
					set @ingresoId = scope_identity()

					if(
					    @filaFueInsertadaIngreso > 0 and @ingresoId is not null					  
					)
					   begin
					       commit transaction SupraMovimientoEnBanca; 
						   select @movimientoid As MovimientoId, @ingresoId As IngresoID, null As EgresoID,  @fecha As FechaRegistro, 1 As FueProcesado, '' As MensajeError, @Ref__Ingresos As Referencia
					   end
					else
					   begin
					      raiserror('La transacción no pudo ser completada. Intentar más tarde.', 16, 1);
					   end			  
				  end
				else if(
				   @ie = 0      --Egreso
				)
				  begin
            
				     declare @egresoId int = null;				     
					 declare @numerosE int = 0;
					 declare @Ref__Egresos varchar(30);

					 set @numerosE = isnull ((select count(*) from flujo.Egreso  e where e.CajaID = @cajaid ), 0 ) + 1;
					 set @Ref__Egresos  = 'E'+ right('0000' + convert(varchar(4), abs(@bancaid)), 4 )+right('0000000000' + convert(varchar(10), abs(@numerosE)), 10)

                     insert into [flujo].[Egreso]( CajaID, IngresoID, TipoEgresoID, MovimientoID, Balance, FechaEgreso, Referencia)
                          VALUES
                                (
                     		      @cajaid
                                , null
                                , 1
                                , @movimientoid
                                , 0
                                , @fecha
                                , @Ref__Egresos
                     		   );

					 declare @filaFueInsertadaEgreso int = @@rowcount;
					 set @egresoId = scope_identity()
					 
					 if(
					     @filaFueInsertadaEgreso > 0 and @egresoId is not null					  
					 )
					    begin
					       commit transaction SupraMovimientoEnBanca; 
					 	   select  @movimientoid As MovimientoId, null As IngresoID, @egresoId As EgresoID,  @fecha As FechaRegistro, 1 As FueProcesado, '' As MensajeError, @Ref__Egresos As Referencia
					    end
					 else
					    begin
					       raiserror('La transacción no pudo ser completada. Intentar más tarde.', 16, 1);
					    end	
				  end
				else
				  begin
				     raiserror('Tipo de flujo invalido. Debe seleccionarse (Ingreso o Egreso).', 16, 1);
				  end
		    end
	      else
		    begin
			    raiserror('El movimiento no fue insertado.', 16, 1);
			end
	  end
	else
	   begin
	      raiserror('La banca no posee caja asignada en flujo.', 16, 1);
	   end	
end try
begin catch  
   if(@@TRANCOUNT > 0)
      begin
	      rollback transaction SupraMovimientoEnBanca;
	  end

	  declare @hoy datetime = getdate();

      select  null As MovimientoId, null As IngresoID,  null As EgresoID,  @hoy As FechaRegistro, 0 As FueProcesado, ERROR_MESSAGE() As MensajeError, '' As Referencia;
	  
	 
end catch
 
        ";





    }
}
