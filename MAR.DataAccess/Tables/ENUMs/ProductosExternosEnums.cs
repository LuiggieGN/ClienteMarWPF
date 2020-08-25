using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.Tables.Enums
{
    public class ProductosExternosEnums
    {
        public enum HttpMethod
        {
            GET, POST, PUT, PATCH, DELETE, COPY, HEAD, OPTIONS, LINK, UNLINK, PURGE
        }

        public enum ServiceMethod
        {
            //Billetes
            SellTicket,
            CancelTicket,
            QuicPick,
            EstadoTicket,
            PagoTicket,
            WinningNumber,
            Ping,
            SMS_GetToken,
            SMS_GetBalance,
            SMS_SendSms
        }

        public enum BilleteMarltonStatus
        {
            ErrorGeneral = 500,
            Exitoso = 200,
            TicketInvalido = 400,
            UsuarioInvalido = 402,
            TicketPagadoAnteriormente = 403,
            TicketExpiradoOVencido = 404
        }
        public enum JuegaMasMarltonStatus
        {
            ErrorGeneral = 500,
            Exitoso = 200,
            TicketInvalido = 400,
            UsuarioInvalido = 402,
            TicketPagadoAnteriormente = 403,
            TicketExpiradoOVencido = 404,
            SorteoCerrado = 410,
            EsperandoSorteo = 900,
            TieneTicketFreePorJugar = 901,
            TicketPagoadoNadaPendiente = 902,
            TicketNoGanador = 903,
            TicketNoEncontrado = 904
        }

        public enum PagaFacilMidasRedStatus
        {
            SolicitudCompletadaCorrectamente = 0,
            ErrorNIC = 1,
            NoHayTransacciones = 02,
            NoHayPolizasConEsaCriteria = 3,
            TransaccionNoExiste = 04,
            NoHayFacturasPendientes = 10,
            BalanceNoDisponible = 11,
            ReferenciaNoEncontrada = 12,
            MontoMayorADeuda = 13,
            ErrorDeAutentificacion = 14,
            PagoRegistradoImposibleDuplicar = 16,
            NoFacturasPendiente = 17,
            ProblemasProveedor = 19,
            ServicioNoHabilidado = 21,
            TiempoExcedidoParaAnularTransaccion = 33,
        }
        public enum PolizasMidasRedStatus
        {
            SolicitudCompletadaCorrectamente = 0,
            PerfilVehiculoIncorrecto = 1,
            NoHayTransacciones = 02,
            NoHayPolizasConEsaCriteria = 3,
            TransaccionNoExiste = 04,
            NoHayFacturasPendientes = 10,
            BalanceNoDisponible = 11,
            ReferenciaNoEncontrada = 12,
            MontoMayorADeuda = 13,
            ErrorDeAutentificacion = 14,
            PagoRegistradoImposibleDuplicar = 16,
            NoFacturasPendiente = 17,
            ProblemasProveedor = 19,
            ServicioNoHabilidado = 21,
            TiempoExcedidoParaAnularTransaccion = 33,
        }

        public enum JuegosNuevosStatus
        {
            OperacionSatisFactoria = 0,
            AccesoDenegado = 45,
            ParametroXmlNulo = 50,
            ErrorGeneralDelSistema = 51,
            AutorizacionOriginalNoPuedeSerNula = 52,
            NumeroDeConsorcioNulo = 53,
            TicketYaExiste = 60,
            ConsorcioNoExiste = 70,
            MontoDeTicketNoCoincideConDetalle = 75,
            JuegoNoExiste = 80,
            CodigoDeReferenciaNoExisteOAnulado = 85,
            UsuarioOPasswordIncorrectos = 90,
            ConsorcioInactivo = 95,
            IpIncorrecta = 100,
            ParametroDettalleJugadaNoPuedeSerNulo = 230,
            ErrorInternoProcesoAutorizacionDeJugada = 231,
            ParametroXmlNoPuedeSerNulo = 232,
            ParametroJsonNoPuedeSerNulo = 233,
            ErrorInternoProcesoAnulacionDeJugada = 236,
            ParametroFechaNoPuedeSerNulo_1 = 237,  ///Se repite
            ErrorInternoProcesoConsultaJugadas = 238,
            ParametroFechaNoPuedeSerNulo_2 = 239,  ///Se repite
            ParametroDeFechaInvalido = 240,
            ErrorConsultandoNumerosGanadores = 241,
            ErrorInternoDelSistema = 251,
            NumeroDeAutorizacionOriginalNoPuedeSerNulo = 252,
            NumeroDeConsorcioNoPuedeSerNulo = 253,
            CodigoDeConsorcioNoPuedeSerNulo = 254,
            CodigoDeUsusarioNoPuedeSerNulo = 255,
            PasswordNoPuedeSerNulo = 256,
            CodigoDeUsuarioExcedeLongitudMaxima = 257,
            PasswordExcedeLongitudMaxima = 258,
            ErrorConsultandoNumerosGanadoresDeSorteos = 296,
            ErrorInternoConsultadoInformacionDeJugadas = 254,
            FormatoDeJsonInvalido = 255,
            ErrorConvirtiendoJsonAXml = 256
        }

    }
}
