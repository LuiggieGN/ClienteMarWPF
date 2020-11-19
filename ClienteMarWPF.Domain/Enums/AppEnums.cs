 

namespace ClienteMarWPF.Domain.Enums
{ 
    public enum Modulos
    {
        Home = 0,
        Login = 1,
        Modulo = 2,
        Reporte = 3,
        Sorteos = 4,
        CincoMinutos = 5,
        Recargas = 6,
        Mensajeria = 7,
        PagoServicios = 8,
        Configuracion = 9,
        InicioControlEfectivo = 100        
    }   

    public enum MarFunciones
    {

     }

    public enum EfectivoFunciones
    {
        
        Caja_RegistrarMovimientoEnCaja = 1000,
        Caja_RegistrarMovimientoDesdeHasta = 1001,
        Caja_ConsultarMovimientos = 1002,
        Caja_LeerCajaBalance = 1003,
        Caja_SetearDisponibilidad = 1004,
        



        Banca_LeerBancaLastCuadreId = 2000,                               //AAA 
        Banca_LeerBancaLastTransaccionesApartirDelUltimoCuadre = 2001,    //AAA 
        Banca_LeerBancaCuadrePorCuadreId = 2002,                          //AAA
                                                                           
        

        Banca_LeerBancaTieneControlEfectivo = 2003, //esto es igual a banca posee cuadre inicial 
        Banca_LeerDeuda = 2004,
        Banca_LeerBancaConfiguraciones = 2005,      //AAA 
               
        
        Cuadre_Registrar = 3000,        



        TipoES_LeerTipoIngresoByLogicaKey = 4000, 
        TipoES_LeerTipoEgresoByLogicaKey = 4001,



        Tarjeta_LeerTarjetaDeUsuario = 7000   



    }
 



    public enum MarSettingExt 
    {
      ini =0,
      txt = 1,
      json = 2
    }


    public enum CuadreGestorAccion 
    {
        Depositar,
        Retirar
    }


}


//GetBancaCajaId = 16,
//GetCajaBalanceActual = 30,