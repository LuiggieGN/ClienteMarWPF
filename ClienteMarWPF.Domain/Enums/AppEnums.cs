 

namespace ClienteMarWPF.Domain.Enums
{

    public enum MarSettingExt
    {
        ini = 0,
        txt = 1,
        json = 2
    }

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
        InicioControlEfectivo = 100,
        RegistrosDeMovimiento = 101
    }   

    public enum MarPtovtaFunciones
    {

    }

    public enum HaciendaFunciones 
    { 
    
    
    }

    public enum EfectivoFunciones
    {        
        Caja_RegistrarMovimientoEnBanca = 1000,   //AAA
        Caja_RegistrarMovimientoDesdeHasta = 1001,//AAA
        Caja_LeerMovimientos = 1002,              //AAA
        Caja_LeerCajaBalance = 1003,//AAA
                                                         // Caja_SetearDisponibilidad = 1004,       //  Ojo esta operacion se va hacer en el mismo bach de query cuando se este cuadrando >> Disponibilidad = 0 luego se cuadra  y por ultimo Disponibilidad = 1
        Caja_LeerCajaDeUsuarioPorUsuarioId = 1004,//AAA   


        Banca_LeerBancaLastCuadreId = 2000,                               //AAA 
        Banca_LeerBancaLastTransaccionesApartirDelUltimoCuadre = 2001,    //AAA 
        Banca_LeerBancaCuadrePorCuadreId = 2002,                          //AAA
        Banca_LeerBancaTieneControlEfectivo = 2003,                                          //esto es igual a banca posee cuadre inicial 
        Banca_LeerDeuda = 2004,
        Banca_LeerBancaConfiguraciones = 2005,      //AAA 
               
        
        Cuadre_Registrar = 3000, //AAA      
        Cuadre_EnlazarCuadreConRuta = 3001, //AAA  


        Tie_LeerTiposAnonimos = 4000, //AAA   
               
        Multiple_LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario = 8000 //AAA
         
    }
 






    public enum CuadreGestorAccion 
    {
        Depositar,
        Retirar
    }

    public enum CuadreTipo
    {
       Inicial = 0,
       Manual = 1,
       Sistema = 2
    }

    public enum ArqueoDeCajaResultado
    {
        Faltante,
        Sobrante
    }



}


//GetBancaCajaId = 16,
//GetCajaBalanceActual = 30,