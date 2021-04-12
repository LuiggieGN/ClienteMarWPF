namespace MAR.DataAccess.ControlEfectivoRepositories.Helpers
{
    internal static class BancaHelper
    {
        internal static string SelectBancaConfiguraciones = @"
           select 
                  BancaID, BanNombre, BanContacto, BanDireccion, BanDireccionIP, BanTelefono, BanNumeroLinea, 
                  RiferoID, BanActivo, BanComentario, BanValidIP, BanSesionActual, BanDireccionActual,
                  BanUsuarioActual, BanRePrintTicketID, BanUltimoTicket, BanVersion, BanMaxQuiniela, BanMaxPale, 
                  BanMaxTriple, BanComisionQ, BanComisionP, BanComisionT, EsquemaID, BanAlive, BanFirstContact, 
                  BanLastContact, BanAnula, BanVLocal, BanGanadores, BanMaxQuinielaLoc, BanMaxPaleLoc, BanMaxTripleLoc, 
                  BanTarjeta, BanComisionTarj, BanTerminalTarj, BanSerieTarj, BanRenta, BanRegistra, BanRemoteCMD,
           	   BanMaxSupeLoc, BanMaxQuinielaTic, BanMaxPaleTic, BanMaxTripleTic, BanSesionWeb, BanDireccionWeb,
           	   BanUsuarioWeb, BanMaxSupe, BanImpuesto, BanPrintRecarga, BanPrintReportes, BanBillete,BanServicios,BanLocal
           from   
                 dbo.MBancas where BancaID = @bancaid;
           
           
           if exists (select top 1 1 from flujo.Caja where BancaID = @bancaid)
              begin
                    select top 1  CajaID, TipoCajaID, ZonaID, UsuarioID, BancaID, Ubicacion, BalanceActual, BalanceMinimo, FechaBalance, FechaCreacion, Activa, CajaDescripcion, Disponible
                    from 
                        flujo.Caja where BancaID = @bancaid and TipoCajaID =1; 
              end
           else
              begin
           
                    insert into flujo.Caja (TipoCajaID, ZonaID, UsuarioID,BancaID, Ubicacion, BalanceActual, BalanceMinimo, FechaBalance, FechaCreacion, Activa, CajaDescripcion, Disponible )
                    values
                          (1
                          ,0
                          ,0
                          ,@bancaid
                          ,'Pendiente por asignar'
                          ,0
                          ,500
                          ,getdate()
                          ,getdate()
                          ,1
                          ,null
                          ,1);
                    
                    declare @nuevoid int = scope_identity();
                    
                    select top 1  CajaID, TipoCajaID, ZonaID, UsuarioID, BancaID, Ubicacion, BalanceActual, BalanceMinimo, FechaBalance, FechaCreacion, Activa, CajaDescripcion, Disponible
                    from 
                        flujo.Caja where CajaID = @nuevoid and TipoCajaID =1; 
              end
           
           
           select 
             (select top 1  isnull((
           	                   select top 1 1 from dbo.MBancasConfig where BancaID = @bancaid and ConfigKey = 'BANCA_ACTIVA_FLUJO' and ConfigValue = 'TRUE'  and Activo = 1
           	                 ), 
           					 0)) As PuedeUsarControlEfectivo ,
           
             (select top 1 isnull((
           
               select top 1 1 from flujo.Cuadre where CajaID = 
               (
                 select top 1 CajaID from flujo.Caja where BancaID = @bancaid and TipoCajaID = 1
               )
             ),0)) As BancaYaInicioControlEfectivo;
        ";


        internal static string SelectBancaCuadrePorCuadreId = @"
          select *  from flujo.Cuadre c where c.CuadreID = @cuadreid
        ";


        internal static string SelectBancaUltimoCuadreId = @"
            select top 1 
              c.CuadreID 
            from flujo.Cuadre c
            where c.CajaID = (select top 1 c.CajaID from flujo.Caja c where c.BancaID = @bancaid)
            order by c.CuadreID desc;
        ";


        internal static string BancaUsaControlEfectivo = @"
            if(@incluyeconfig = 1)
            begin 
                  declare @PoseeConfig bit = (
            	                               select top 1  
            			                       isnull(
            				                            (
                       	                                  select top 1 1 from dbo.MBancasConfig where BancaID = @bancaid and ConfigKey = 'BANCA_ACTIVA_FLUJO' and ConfigValue = 'TRUE'  and Activo = 1
                       	                                ), 
                       		                			 0
            				                		 )
                                             );  
            
                  declare @UsaControlEfectivo1 bit = (select top 1 isnull((
                       
                           select top 1 1 from flujo.Cuadre where CajaID = 
                           (
                             select top 1 CajaID from flujo.Caja where BancaID = @bancaid and TipoCajaID = 1
                           )
                  ),0));
            
            
            	  if(@PoseeConfig = 1 and @UsaControlEfectivo1 = 1)
            	     begin
            
            		      select 1 as UsaControlEfectivo
            	     end
            	  else
            	     begin
            		      select 0 as UsaControlEfectivo
            		 end       
            end
            else
            begin
            
                  declare @UsaControlEfectivo2 bit = (select top 1 isnull((
                       
                           select top 1 1 from flujo.Cuadre where CajaID = 
                           (
                             select top 1 CajaID from flujo.Caja where BancaID = @bancaid and TipoCajaID = 1
                           )
                  ),0));
            
            
                  select @UsaControlEfectivo2 as UsaControlEfectivo;
            end
        ";


        internal static string Procedure_SP_GET_BANCA_TRANSACCIONES_A_PARTIR_DE_ULTIMO_CUADRE = "[flujo].[Sp_ConsultaBancaTransaccionesDesdeUltimoCuadre]";


        internal static string QueryParaLeerDeudaDeBanca = @"select top 1 isnull(flujo.GetBancaTotalTiketsPendienteDePago(@bancaid),0 )";



        internal static string SelectBancaInactividad = @"
            select top 1
            convert(int,  isnull(
               (select top 1 ConfigValue from MBancasConfig where BancaID = @bancaid and ConfigKey like 'BANCA_INTERVALO_INACTIVIDAD_MINUTOS%'), '0'
            )) as IntervaloInactividad;
         ";



        internal static string SelectMarOperaciones = @"

declare @fecha date = convert(date, @fcompleta);

 select Tipo, TipoMovimiento, KeyMovimiento, BancaID, SUM(Monto) Monto,FechaMovimiento
  from (    
 
  SELECT 
     Tipo = 'Ingreso',
     TipoMovimiento = 'ApuestaActiva',
	 KeyMovimiento = 1,
	 BancaID AS BancaID,
	 Monto =  TicCosto, 
	 CONVERT(date,TicFecha) as FechaMovimiento
  FROM  DTickets WHERE BancaID = @bancaid and convert(date,ticfecha) = @fecha
 
    --HTickets activos
  UNION ALL   SELECT 
      Tipo = 'Ingreso',
      TipoMovimiento = 'ApuestaActiva',
	  KeyMovimiento = 1,
	  BancaID,
      Monto =  TicCosto, 
	  CONVERT(date,TicFecha) as FechaMovimiento
 FROM  HTickets WHERE  BancaID = @bancaid and convert(date,ticfecha) = @fecha

  --DTickets Nulos
  UNION ALL    SELECT 
    Tipo = 'Egreso',
      TipoMovimiento = 'ApuestaNULA',
	  KeyMovimiento = 2,
	  BancaID,
	Monto = TicCosto,  
	   CONVERT(date,TicFecha) as FechaMovimiento
  FROM  DTickets WHERE TicNulo = 1 and BancaID = @bancaid and convert(date,ticfecha) = @fecha

    --HTickets Nulos
  UNION ALL   SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'ApuestaNULA',
	  KeyMovimiento = 2,
	  BancaID,
      Monto =  TicCosto, 
	  CONVERT(date,TicFecha) as FechaMovimiento
 
 FROM  HTickets WHERE TicNulo = 1  AND BancaID = @bancaid and convert(date,ticfecha) = @fecha

    --RF Transacciones activos
 UNION ALL  SELECT 
      Tipo = 'Ingreso',
      TipoMovimiento = 'ApuestaRFActiva',
	  KeyMovimiento = 3,
	  BancaID,
	  Monto = Ingresos, 
	  CONVERT(date,Fecha) as FechaMovimiento
 
  FROM  CL_Recibo WHERE BancaID = @bancaid and convert(date,Fecha) = @fecha

     --RF Transacciones Nulas
 UNION ALL  SELECT 
         Tipo = 'Egreso',
      TipoMovimiento = 'ApuestaRFNULA',
	  KeyMovimiento = 4,
	  BancaID,
	 Monto = Ingresos, 
	 	   CONVERT(date,Fecha) as FechaMovimiento
  
  FROM  CL_Recibo WHERE Activo = 0  AND BancaID = @bancaid and convert(date,Fecha) = @fecha

 --PDPines Recargas activas
 UNION ALL  SELECT  
      Tipo = 'Ingreso',
      TipoMovimiento = 'RecargaActiva',
	  KeyMovimiento = 5,
	  BancaID,
	  Monto =  PinCosto, 
	  CONVERT(date,PinFecha) as FechaMovimiento
     
  FROM  PDPines  where  BancaID = @bancaid and convert(date,PinFecha) = @fecha
     
	 --PhPines Recargas activas
 UNION ALL  SELECT  
      Tipo = 'Ingreso',
      TipoMovimiento = 'RecargaActiva',
	  KeyMovimiento = 5,
	  BancaID,
	  Monto = PinCosto, 
	  CONVERT(date,PinFecha) as FechaMovimiento
     
  FROM  PHPines where BancaID = @bancaid and convert(date,PinFecha) = @fecha
    
  --PDPines Recargas NULAS
 UNION ALL  SELECT  
      Tipo = 'Egreso',
      TipoMovimiento = 'RecargaNula',
	  KeyMovimiento = 6,
	  BancaID,
	  Monto = PinCosto, 
	  CONVERT(date,PinFecha) as FechaMovimiento
   
  FROM  PDPines  where PinNulo = convert(int,1) AND BancaID = @bancaid and convert(date,PinFecha) = @fecha
  
     --PhPines Recargas NULAS
 UNION ALL  SELECT  
      Tipo = 'Egreso',
      TipoMovimiento = 'RecargaNula',
	  KeyMovimiento = 6,
	  BancaID,
	   Monto = PinCosto, 
	   CONVERT(date,PinFecha) as FechaMovimiento
    
  FROM  PHPines  where PinNulo = convert(int,1) AND BancaID = @bancaid and convert(date,PinFecha) = @fecha
   

     --COMISION PAGO BANCA HTICKET
 UNION ALL
  SELECT 
	 Tipo = 'Egreso',
	 TipoMovimiento = 'ComisionApuestaBanca',
	 KeyMovimiento = 15,
	 BancaID = h.BancaID,
	 Monto =  Sum(Round(TicCosto*((BanComisionQ + BanComisionP + BanComisionT)/3)/100,2)), 
	 -- cuidado si no ponen las comisiones iguales por cada jugada --Sum(Round(TDeCosto*(case when TDeQp='Q' then BanComisionQ when TDeQp='P' then BanComisionP When TDeQP='T' then BanComisionT end)/100,2)) as TDeComision
	 CONVERT(date,TicFecha) as FechaMovimiento 
	 FROM  HTickets h join MBancas b on b.BancaID = h.BancaID
	 WHERE TicNulo = 0 AND H.BancaID = @bancaid and convert(date,TicFecha) = @fecha
	 GROUP BY CONVERT(date,TicFecha), H.BancaID


  --COMISION PAGO BANCA DTICKET
 UNION ALL
  SELECT 
	 Tipo = 'Egreso',
	 TipoMovimiento = 'ComisionApuestaBanca',
	 KeyMovimiento = 15,
	 BancaID = h.BancaID,
	 Monto =  Sum(Round(TicCosto*((BanComisionQ + BanComisionP + BanComisionT)/3)/100,2)), 
	 -- cuidado si no ponen las comisiones iguales por cada jugada --Sum(Round(TDeCosto*(case when TDeQp='Q' then BanComisionQ when TDeQp='P' then BanComisionP When TDeQP='T' then BanComisionT end)/100,2)) as TDeComision
	 CONVERT(date,TicFecha) as FechaMovimiento 
	 FROM  DTickets h join MBancas b on b.BancaID = h.BancaID
	 WHERE TicNulo = 0 AND H.BancaID = @bancaid and convert(date,TicFecha) = @fecha
	 GROUP BY CONVERT(date,TicFecha), H.BancaID



       --VP Transacciones activos
 UNION ALL  SELECT 
      Tipo = 'Ingreso',
      TipoMovimiento = 'VP_TransaccionActiva',
	  KeyMovimiento = 7,
	  BancaID,
	  Monto = Ingresos, 
	  CONVERT(date,FechaIngreso) as FechaMovimiento
     
  FROM  VP_Transaccion where  Ingresos > 0  AND BancaID = @bancaid and convert(date,FechaIngreso) = @fecha

 --VP Transacciones activos
 UNION ALL  SELECT 
      Tipo = 'Ingreso',
      TipoMovimiento = 'VP_TransaccionActiva',
	  KeyMovimiento = 7,
	  BancaID,
	  Monto = Ingresos, 
	 CONVERT(date,FechaIngreso) as FechaMovimiento
   
  FROM  VP_HTransaccion where  Ingresos > 0  and BancaID = @bancaid and convert(date,FechaIngreso) = @fecha
    
         --VP Transacciones NULAS
 UNION ALL  SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'VP_TransaccionNula',
	  KeyMovimiento = 8,
	  BancaID,
	  Monto = Ingresos, 
	  CONVERT(date,FechaIngreso) as FechaMovimiento
  
  FROM  VP_Transaccion where Activo = 0 and convert(date,FechaIngreso) = @fecha and BancaID = @bancaid
  
 --VP Transacciones NULAS
 UNION ALL  SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'VP_TransaccionNula',
	  KeyMovimiento = 8,
	  BancaID,
	  Monto = Ingresos, 
	  CONVERT(date,FechaIngreso) as FechaMovimiento
 
  FROM  VP_HTransaccion where Activo = 0  and BancaID = @bancaid and convert(date,FechaIngreso) = @fecha
 
         --VP Transacciones activos
 UNION ALL  SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'VP_TransaccionDescuento',
	  KeyMovimiento = 9,
	  BancaID,
	  Monto = Descuentos,  
	  CONVERT(date,FechaIngreso) as FechaMovimiento
     
  FROM  VP_Transaccion where Activo = 1  and Descuentos > 0  AND BancaID = @bancaid and convert(date,FechaIngreso) = @fecha

 --VP Transacciones activos
 UNION ALL  SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'VP_TransaccionDescuento',
	  KeyMovimiento = 9,
	  BancaID,
	  Monto = Descuentos, 
	  CONVERT(date,FechaIngreso) as FechaMovimiento
    
  FROM  VP_HTransaccion where Activo = 1 and Descuentos > 0  AND BancaID = @bancaid  and convert(date,FechaIngreso)  = @fecha

   --HPagos Pagos
    UNION ALL  SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'PagoGanador',
	  KeyMovimiento = 10,
	  p.BancaID,
	  Monto = PagMonto, 
	  CONVERT(date,PagFecha) as FechaMovimiento
      
  FROM  HPagos p inner join DTickets d on p.PagoID = d.PagoID AND p.BancaId = d.Bancaid
  WHERE  D.BancaID = @bancaid AND P.BancaID = @bancaid AND convert(date,PagFecha) = @fecha
       
 --HPagos Pagos
 UNION ALL  SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'PagoGanador',
	  KeyMovimiento = 10,
	  p.BancaID,
	   	   Monto = PagMonto, 
	   CONVERT(date,PagFecha) as FechaMovimiento
     
  FROM  HPagos p inner join HTickets d on p.PagoID = d.PagoID AND p.BancaId = d.Bancaid
  WHERE  D.BancaID = @bancaid AND P.BancaID = @bancaid AND convert(date,PagFecha) = @fecha

  
UNION ALL   SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'PagoGanadorRemoto',
	  KeyMovimiento = 11,
	  p.BancaID,
	  Monto = PagMonto, 
	  CONVERT(date,PagFecha) as FechaMovimiento
    
  FROM  HPagos p inner join HTickets h on p.PagoID = h.PagoID and P.BancaID != h.BancaID
  WHERE P.BancaID != h.BancaID  and P.BancaID = @bancaid and h.BancaID != @bancaid  and convert(date,PagFecha) = @fecha

  UNION ALL  SELECT 
      Tipo = 'Egreso',
      TipoMovimiento = 'PagoGanadorRemoto',
	  KeyMovimiento = 11,
	  p.BancaID,
      Monto = PagMonto, 
	  CONVERT(date,PagFecha) as FechaMovimiento
     
  FROM  HPagos p inner join DTickets d on p.PagoID = d.PagoID and P.BancaID != d.BancaID
  WHERE P.BancaID != D.BancaID  and P.BancaID = @bancaid and d.BancaID != @bancaid and convert(date,PagFecha) = @fecha
 

 UNION ALL  SELECT
      Tipo = 'Egreso',
      TipoMovimiento = 'CL_PagoGanador',
	  KeyMovimiento = 13,
	  p.BancaID,
	  Monto = Monto, 
	  CONVERT(date,p.Fecha) as FechaMovimiento
  FROM  CL_Pagos p inner join CL_Recibo r on r.ReciboID = p.ReciboID
  where p.BancaID = r.BancaID  AND P.BancaID = @bancaid  and convert(date,P.Fecha) = @fecha
  
      --CL Pagos Remoto
 UNION ALL  SELECT
      Tipo = 'Egreso',
      TipoMovimiento = 'CL_PagoGanadorRemoto',
	  KeyMovimiento = 14,
	  p.BancaID,
	  Monto = Monto, 
	  CONVERT(date,p.Fecha) as FechaMovimiento
  FROM  CL_Pagos p inner join CL_Recibo r on r.ReciboID = p.ReciboID
  where p.BancaID != r.BancaID  AND P.BancaID = @bancaid  and convert(date,P.Fecha) = @fecha



	  ) AS flujo

group by Tipo, TipoMovimiento, KeyMovimiento, BancaID, FechaMovimiento


        ";


        internal static string LeerBancaRemoteCmdCommand = "select top 1  BanRemoteCMD from dbo.MBancas where BancaID = @bancaid";



        internal static string LeerEstadoBancaEstaActiva = "select top 1 isnull( (  select top 1 BanActivo from dbo.MBancas where BancaID = @bancaid ), 0) as ActivoEstado";














    }
}
