namespace MAR.DataAccess.ControlEfectivoRepositories.Helpers
{
    internal static class TieHelper
    { 
        internal static string SelectTiposIngresosAnonimos = @"

            select 
                    1             as Clave, -- Esto dice si es un ingreso ( 1 ) o egreso ( 0 )
            		TipoIngresoID as Id,
            		Tipo,
            		TipoNombre,
            		Descripcion,
            		LogicaKey,
            		EsTipoSistema,
            		EsTipoAnonimo,
            		FechaCreacion,
            		Activo,
            		Orden	  
            from flujo.TipoIngreso where EsTipoSistema = 0 and EsTipoAnonimo = 1 and Activo = 1;  

        ";

        internal static string SelectTiposEgresosAnonimos = @"
            
            select   
                    0             as Clave, -- Esto dice si es un ingreso ( 1 ) o egreso ( 0 )
            		TipoEgresoID as Id,
            		Tipo,
            		TipoNombre,
            		Descripcion,
            		LogicaKey,
            		EsTipoSistema,
            		EsTipoAnonimo,
            		FechaCreacion,
            		Activo,
            		Orden	  
            from flujo.TipoEgreso  where EsTipoSistema = 0 and EsTipoAnonimo = 1 and Activo= 1;

        ";

    }//fin de clase
}
