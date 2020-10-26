 namespace FlujoCustomControl.Code.AppEnums
{
    public enum RoutingFunctions : int
    {
        BancaTieneCuadreInicial = 1,
        GetBancaUltimoCuadreId  = 2,
        GetTransaccionesBancaDesdeUltimoCuadre = 3,
        GetCuadreById = 4,
        GetMontoTotalizadoTicketsPendienteDePagoByBancaId = 6,
        GetBancaByBancaId = 7,
        SubmitNuevoCuadre = 8,


        GetBancaCajaId = 16,
        GetFirstSurperUsuario = 17,

        SetCajaDisponibilidad = 27,
        GetCajaBalanceActual = 30,
        GetCajaVirtualByUsuarioId = 31,
        GetCajaBalanceMinimoByCajaId = 32,
        BancaTieneCajaAsignada = 33,
        CrearCajaABanca = 34,

        GetMUsuarioByPin = 40,
        GetGestorAsignacionPendiente = 41,
        GetUsuarioTarjeta = 42,
        EnlazaCuadreConAsignacion = 43

    }
}

