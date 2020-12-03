namespace MAR.DataAccess.ControlEfectivoRepositories.Helpers
{
    internal static class CuadreHelper
    {

        internal static string Procedure_SP_REGISTRAR_CUADRE = "[flujo].[Sp_CuadrarBanca]";


        internal static string QueryEnlazarRutaConCuadre = @"

             update flujo.Ruta set Estado = @rutaEstado, UltimaLocalidad = @rutaUltimaLocalidad, OrdenRecorrido = @rutaOrdenRecorrido where RutaID = @rutaId;
             update flujo.Cuadre set RutaID = 0 where CajaID = @bancaCajaId and  RutaID = @rutaId;
             update flujo.Cuadre set RutaID = @rutaId where CuadreID = @cuadreId;
             select 1;

        "; //Nota: cuando se agregue el historico de cuadre agregar la segunda linea de igual forma para la tabla de historico de cuadre 



    }
}
