namespace ClienteMarWPFWin7.Domain.Enums
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
        PegaMas = 10,
        EnLinea = 11,
        InicioControlEfectivo = 100,
        RegistrosDeMovimiento = 101
    }   
    

    public enum EfectivoFunciones
    { 
        Caja_RegistrarMovimientoEnBanca = 1000,     
        Caja_RegistrarMovimientoDesdeHasta = 1001,  
        Caja_LeerMovimientos = 1002,                
        Caja_LeerCajaBalance = 1003,                
        Caja_LeerCajaDeUsuarioPorUsuarioId = 1004,        
        Caja_SetearCajaDisponibilidad = 1005,       
        Caja_LeerCajaDisponibilidad = 1006,  // -- NOT IMPLEMENTED YET    Por CajaID
        Caja_LeerCajaBalanceMinimo = 1007,
        Caja_LeerMovimientosNoPaginados = 1008,
        Caja_LeerCajaExiste = 1009,


        Banca_Rel = 1999,
        Banca_LeerBancaLastCuadreId = 2000,                                 
        Banca_LeerBancaLastTransaccionesApartirDelUltimoCuadre = 2001,      
        Banca_LeerBancaCuadrePorCuadreId = 2002,                           
        Banca_UsaControlEfectivo = 2003, 
        Banca_LeerDeudaDeBanca = 2004, 
        Banca_LeerBancaConfiguraciones = 2005,       
        Banca_LeerInactividad = 2006,
        Banca_LeerBancaMarOperacionesDia = 2007,
        Banca_LeerBancaRemoteCmdCommand = 2008,
        Banca_LeerEstadoBancaEstaActiva = 2009,
        Banca_LeerVentaDeHoyDeLoterias = 2010,
        Banca_LeerVentaDeHoyDeProductos = 2011,
        Banca_LeerTicketFueAnulado = 2012,
        Banca_LeerTicketsHoy = 2013,


        Cuadre_Registrar = 3000,  
        Cuadre_EnlazarCuadreConRuta = 3001,   
        

        Tie_LeerTiposAnonimos = 4000,     
        
        
        Multiple_LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario = 8000,         


        Ruta_LeerGestorAsignacionPendiente = 9000  
    }

    public enum JuegaMasFunciones
    {
        ReporteListadoPremio = 12       
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

    public enum ServicioMarConexion
    {
        NoConectado = 0,
        Conectado = 1
    }




}

 