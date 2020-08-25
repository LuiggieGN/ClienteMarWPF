
namespace Flujo.Entities.WebClient.Enums
{
    public enum TipoFlujo
    {
         Ingreso = 0,
         Egreso  = 1
    }

    public enum TipoFlujoSubCategorias
    {
        //Ingresos
        DepositoDeCuadre = 100001,
        OtrosAbonos = 100002,
        //NivelarBalance = 100003,


        //Egresos
        SalidaDeCuadre = 200001,
        Compra = 200002,
        GastoGeneral = 200003
    }

    public enum TipoUsuario
    {
         Cajero               = 701,
         Mensajero        = 702,
         Administrador = 703
    }

    public enum UsuarioTiposDocumento
    {
         Cedula = 1,
         Pasaporte = 2,
         Licencia = 3
    }

    public enum PeriodoTiempoDefaults
    {
        Hoy = 0,
        Periodo_30_dias_atras = 1,
        Periodo_60_dias_atras = 2,
        Rango_de_fecha = 3 
    }

}
