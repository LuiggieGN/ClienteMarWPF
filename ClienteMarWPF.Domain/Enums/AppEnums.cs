using System;

namespace ClienteMarWPF.Domain.Enums
{

    /// <summary>
    ///  Cliente vistas activas
    /// </summary>
    public enum ViewTypeEnum
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
        Configuracion = 9
    }

    /// <summary>
    ///  Definicion global de funciones contenidas en el servicio de flujo efectibo
    /// </summary>
    public enum FlujoEfectivoRoutingFunctions
    {
        GetBancaCajaId = 16,
        GetCajaBalanceActual = 30
    }

    /// <summary>
    /// Definicion global de funciones contenidas en el servico del cliente de MAR
    /// </summary>
    public enum ClienteMarServicioEnum
    {

    }




}
