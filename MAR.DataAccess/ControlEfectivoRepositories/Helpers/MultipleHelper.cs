namespace MAR.DataAccess.ControlEfectivoRepositories.Helpers
{
    internal static class MultipleHelper
    { 
        internal static string LeerUsuarioSuCajaYSuTarjeta = @"

          declare @tablausuario table(
          	UsuarioID int not null,
          	UsuNombre varchar(50) not null,
          	UsuApellido varchar(50) not null,
          	UsuCedula varchar(25) not null,
          	UsuFechaNac date not null,
          	UsuUserName varchar(20) not null,
          	UsuClave varchar(50) not null,
          	UsuVenceClave datetime  not null,
          	UsuActivo bit  not null,
          	UsuNivel int  not null,
          	UsuComentario text  not null,
          	UsuTema varchar(250) null,
          	Email varchar (50) null,
          	TipoUsuarioID int null,
          	LoginFallidos int null,
          	ToquenFallidos int null,
          	UsuPin varchar (10) null,
          	UsuPuedeCuadrar int null,
          	TipoAutenticacion int  not null 
          );
          
          insert into @tablausuario( UsuarioID, UsuNombre, UsuApellido, UsuCedula, UsuFechaNac, UsuUserName, UsuClave, UsuVenceClave, UsuActivo, UsuNivel, UsuComentario,	UsuTema, Email, TipoUsuarioID, LoginFallidos, ToquenFallidos, UsuPin, UsuPuedeCuadrar, TipoAutenticacion )
          select 
               top 1 
                 UsuarioID,
                 UsuNombre,
                 UsuApellido,
                 UsuCedula,
                 UsuFechaNac,
                 UsuUserName,
                 UsuClave,
                 UsuVenceClave,
                 UsuActivo,
                 UsuNivel,
                 UsuComentario,
                 UsuTema,
                 Email,
                 TipoUsuarioID,
                 LoginFallidos,
                 ToquenFallidos,
                 UsuPin,
                 UsuPuedeCuadrar,
                 TipoAutenticacion
          from dbo.MUsuarios where UsuPin = @pin;    
          
          
          select 
               top 1 
                 UsuarioID,
                 UsuNombre,
                 UsuApellido,
                 UsuCedula,
                 UsuFechaNac,
                 UsuUserName,
                 UsuClave,
                 UsuVenceClave,
                 UsuActivo,
                 UsuNivel,
                 UsuComentario,
                 UsuTema,
                 Email,
                 TipoUsuarioID,
                 LoginFallidos,
                 ToquenFallidos,
                 UsuPin,
                 UsuPuedeCuadrar,
                 TipoAutenticacion
          from @tablausuario;
          
          select 
               top 1
                  CajaID,
                  TipoCajaID,
                  ZonaID,
                  UsuarioID,
                  BancaID,
                  Ubicacion,
                  BalanceActual,
                  BalanceMinimo,
                  FechaBalance,
                  FechaCreacion,
                  Activa,
                  CajaDescripcion,
                  Disponible
           from flujo.Caja where TipoCajaID = 2 and UsuarioID = (select top 1 UsuarioID from @tablausuario);
           
          select
               top 1 
                  TarjetaID,
                  UsuarioID,
                  Serial,
                  FechaCreacion,
                  Comentario,
                  Tokens as JsonTokens,
                  Activo 
          from flujo.UsuarioTarjetaClaves where UsuarioID = (select top 1 UsuarioID from @tablausuario); 

        ";

 

    } // fin de clase
}
