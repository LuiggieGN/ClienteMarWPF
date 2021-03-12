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




    }
}
