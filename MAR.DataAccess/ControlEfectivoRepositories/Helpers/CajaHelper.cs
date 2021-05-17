namespace MAR.DataAccess.ControlEfectivoRepositories.Helpers
{
    internal static class CajaHelper
    {
        internal static string Procedure_SP_CAJA_BALANCE_ACTUAL = @"[flujo].[Sp_ConsultaCajaBalanceActual]";
        internal static string Procedure_SP_CAJA_LEER_MOVIMIENTOS = @"[flujo].[Sp_ConsultaCajaMovimientosPaginados]";
        internal static string Procedure_SP_CAJA_LEER_MOVIMIENTOS_NO_PAGINADOS = @"[flujo].[Sp_ConsultaCajaMovimientos]";

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


		internal static string QueryParaRegistrarTransferencia = @"


   begin try 
	set nocount on;	
	begin transaction SupraMovimientoDesdeHasta		
	declare @today datetime = getdate(); 

	declare @affectedmovorigen  int;
    declare @movorigenid        int = null;
    declare @affectedmovdestino int;
	declare @movdestinoid       int = null;
 
	insert into [flujo].[Movimiento](TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
    values ( 0, @CajaOrigenID, @UsuarioID, @today, @Monto, @Descripcion,'Procesado', @today, 1  );
	
	set @affectedmovorigen = @@rowcount;
	set @movorigenid = scope_identity()

	insert into [flujo].[Movimiento](TipoMovimientoID, CajaID, UsuarioID, Fecha, Monto, Descripcion, Estado, FechaUltimaActualizacion, Activo)
    values ( 0, @CajaDestino, @UsuarioID, @today, @Monto, @Descripcion, 'Procesado', @today, 1);
	 
	 
	 set @affectedmovdestino = @@rowcount;
	 set @movdestinoid = scope_identity();
	 	 
	 if(
	      ( @affectedmovorigen > 0 and @movorigenid is not null ) and (  @affectedmovdestino > 0 and @movdestinoid  is not null ) 
	   )
	   begin	   	   
		   declare @NumMovOrigen  int = isnull ((select count(*) from flujo.Egreso  e where e.CajaID = @CajaOrigenID and year(@today) = year(e.FechaEgreso) ), 0 ) + 1;	    
		   declare @refOrigen     varchar(50);

		   declare @NumMovDestino int = isnull ((select count(*) from flujo.Ingreso i where i.CajaID = @CajaDestino and year(@today) = year(i.FechaIngreso) ), 0 ) + 1;	
		   declare @refDestino    varchar(50); 
		   	  
	  	   -- @@ Caja Origen ID     
	       if(@TipoCajaOrigen = 1) -- (Banca de Loteria )
	         begin
 	            set @refOrigen = 'ET'+ convert(varchar(4),year(@today))+ right('00000' + convert(varchar(5), abs(@CajaOrigenID)), 5 )+right('0000000' + convert(varchar(7), abs(@NumMovOrigen)), 7)
                insert into [flujo].[Egreso]( CajaID, IngresoID, TipoEgresoID, MovimientoID, Balance, FechaEgreso, Referencia)
                values (@CajaOrigenID, null, 1, @movorigenid, 0, @today, @refOrigen); 
	         end
	       else if(@TipoCajaOrigen = 2 or @TipoCajaOrigen = 3) -- (Usuario || Banco)  
	         begin 	            
				if(@TipoCajaOrigen = 2)
				  begin
				    set @refOrigen = 'EV'+ convert(varchar(4),year(@today))+ right('00000' + convert(varchar(5), abs(@CajaOrigenID)), 5 )+right('0000000' + convert(varchar(7), abs(@NumMovOrigen)), 7)
				  end 

				if(@TipoCajaOrigen = 3)
				  begin
				    set @refOrigen = 'EB'+ convert(varchar(4),year(@today))+ right('00000' + convert(varchar(5), abs(@CajaOrigenID)), 5 )+right('0000000' + convert(varchar(7), abs(@NumMovOrigen)), 7)
				  end

				  declare @CajaOrigenBalanceActual decimal = (select top 1 isnull(c.BalanceActual, 0) As Balance from flujo.Caja c where c.CajaID = @CajaOrigenID);	         
				  update flujo.Caja set BalanceActual = (@CajaOrigenBalanceActual - @Monto) where  CajaID = @CajaOrigenID;

                  insert into [flujo].[Egreso]( CajaID, IngresoID, TipoEgresoID, MovimientoID, Balance, FechaEgreso, Referencia)
                  values (@CajaOrigenID, null, 1, @movorigenid, (@CajaOrigenBalanceActual - @Monto), @today, @refOrigen);

	         end
           else
	         begin
	            raiserror('Error. No se pudo completar la operación', 16, 1);
	         end	 			 		
		    
		   declare @affectedLastOrigen int = @@rowcount;
		   declare @egresoid int = scope_identity();

		   if(@affectedLastOrigen < 1  or @egresoid is null)
		     begin
              raiserror('Error. No se pudo completar la operación', 16, 1);
			 end


		   -- @@ Caja Destino ID	 
	       if(@TipoCajaDestino = 1)  -- (Banca de Loteria )
	         begin
	            set @refDestino = 'IT'+ convert(varchar(4),year(@today))+ right('00000' + convert(varchar(5), abs(@CajaDestino)), 5 )+ right('0000000' + convert(varchar(7), abs(@NumMovDestino)), 7);
				insert into [flujo].[Ingreso](CajaID, EgresoID, TipoIngresoID, MovimientoID, Balance, FechaIngreso, Referencia )
                values (@CajaDestino, null, 1, @movdestinoid, 0, @today, @refDestino);

	         end
	       else if(@TipoCajaDestino = 2 or @TipoCajaDestino = 3)  -- ( Usuario || Banco )    
	         begin
				if(@TipoCajaDestino = 2)
				  begin
				    set @refDestino = 'IV'+ convert(varchar(4),year(@today))+ right('00000' + convert(varchar(5), abs(@CajaDestino)), 5 )+ right('0000000' + convert(varchar(7), abs(@NumMovDestino)), 7);
				  end

				if(@TipoCajaDestino = 3)
				  begin
				    set @refDestino = 'IB'+ convert(varchar(4),year(@today))+ right('00000' + convert(varchar(5), abs(@CajaDestino)), 5 )+ right('0000000' + convert(varchar(7), abs(@NumMovDestino)), 7);
				  end

				  declare @CajaDestinoBalanceActual decimal = (select top 1 isnull(c.BalanceActual, 0) As Balance from flujo.Caja c where c.CajaID = @CajaDestino);	
				  update flujo.Caja set BalanceActual = (@CajaDestinoBalanceActual + @Monto) where  CajaID = @CajaDestino;

                  insert into [flujo].[Ingreso](CajaID, EgresoID, TipoIngresoID, MovimientoID, Balance, FechaIngreso, Referencia)
                  values ( @CajaDestino, null, 1, @movdestinoid, (@CajaDestinoBalanceActual + @Monto), @today, @refDestino );
	          
	         end
	       else
	        begin
 	           raiserror('Error. No se pudo completar la operación', 16, 1);
	        end			      	       

		   declare @affectedLastDestino int = @@rowcount;
		   declare @ingresoid int = scope_identity();

		   if(@affectedLastDestino < 1  or @ingresoid is null)
		     begin
              raiserror('Error. No se pudo completar la operación', 16, 1);
			 end

		   commit transaction SupraMovimientoDesdeHasta;

		   select            1  As  FueProcesado, 
		          @movorigenid  As  OrigenMovId ,
				 @movdestinoid  As  DestinoMovId, 
				    @refOrigen  As  RefOrigen,
				   @refDestino  As  RefDestino,
				             '' As  Error, 
					     @today As  FechaTransferencia;
	   end
	 else
	  begin
	     raiserror('Error. No se pudo completar la operación', 16, 1);
	  end
end try
begin catch
  if(@@trancount > 0)
   begin
    rollback transaction SupraMovimientoDesdeHasta;
   end

   select                0 As FueProcesado,  
                      null As OrigenMovId , 
		              null As DestinoMovId,
				        '' As RefOrigen,
				        '' As RefDestino,
           error_message() As Error,
		         getdate() As FechaTransferencia;
end catch

        ";

		internal static string QueryParaLeerBalanceMinimoDeCaja = @"select top 1 isnull((select top 1 BalanceMinimo from flujo.Caja where CajaID =	@cajaid),0)";

		internal static string QuerySetCajaDisponibilidad = @"

           if(@bancaid is not null)
           begin
               update flujo.Caja set Disponible = @disponibilidad where BancaID = @bancaid and TipoCajaID =1;
           end
           
           if(@cajaid is not null)
           begin
               update flujo.Caja set Disponible = @disponibilidad where CajaID = @cajaid;
           end

           select 1 As Completado;
        ";


		internal static string QueryCajaExiste = @"select top 1 isnull((select top 1 1 from flujo.Caja where CajaID = @cajaid),0) As Existe;";











	}
}
