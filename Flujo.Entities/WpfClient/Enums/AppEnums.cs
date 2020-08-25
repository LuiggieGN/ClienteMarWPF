
namespace Flujo.Entities.WpfClient.Enums
{
    public enum TipoFlujo
    {
         Ingreso = 0,
         Egreso = 1
    }

    public enum TipoFlujoSubCategorias
    {
         DepositoDineroCaja = 100001,
         RetiroDineroCaja = 200001
    }

    public enum TipoUsuario
    {
         Cajero = 701,
         Mensajero = 702,
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
        Periodo_7_dias_atras = 1,
        Periodo_30_dias_atras = 2,
        Rango_de_fecha = 3 //,
        //Periodo_7_dias_atras = 4

    }


    public enum FaltanteSobrante_Enum
    {
        Faltante ,
        Sobrante  

    }

}
