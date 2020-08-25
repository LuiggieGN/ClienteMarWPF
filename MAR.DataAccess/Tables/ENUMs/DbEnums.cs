using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.DataAccess.Tables.Enums
{
    public class DbEnums
    {
        public enum CuentaConfig
        {
            RiferoID,
            Producto,
            USER,
            PASS,
            TokenLive,
            Url,
            CuentaDefault
        }
        public enum ReciboCampoReferencia
        {
            Bingo,
            CincoMinutos
        }
        public enum VP_ProductoReferencia
        {
            Bingo_M,
            Bingo_C,
            Bingo_I,
            CincoMinutos
        }
        //Productos with LogicaKey
        public enum Productos
        {
            PagaFacil = 1,
            Polizas = 2,
            Billetes = 3,
            JuegaMas = 4,
            JuegaMasGanador = 5,
            Pega3Pega4 = 6,
            JuegosNuevos = 7,
            LoteriasNuevas = 8,
            Bingo = 9,
            BingoGanador = 10,
            DepositoProductoMar = 11,
            CincoMinutos = 12,
            CincoMinutosGanador = 13,
            Remesas


        }
        public enum VP_TransaccionesDestallesReferenciasPolizas
        {
            Nombre = 1,
            Cedula = 2,
            Telefono = 3,
            Placa = 4,
            Chasis = 5,
            Vigencia = 6
        }
        public enum VP_SuplidorReferencia
        {
            SegurosMidas
        }
        public enum VP_TransaccionesDestallesReferenciasJuegaMas
        {
            Cantidad = 1,
            Jugada = 2,
            Monto = 3,
            TicketMarlton = 4,
            NumeroSorteo = 5,
            HoraSorteo = 6,
            FechaSorteo = 7,
            Serial = 8,
            TicketControl = 9,
            SerialTF = 10
        }
        public enum VP_TransaccionesDestallesReferenciasBingo
        {
            Monto = 1,
            HoraSorteo = 2,
            FechaSorteo = 3,
            Serial = 4,
            Autorizacion = 5,
            ReferenciaBingo = 6,
            CartonBingo = 7,
            NumeroSorteo
        }
        public enum VP_TransaccionesDestallesReferenciasCincoMinutos
        {
            Monto,
            Codigo,
            Jugada,
            Autorizacion
        }
        public enum BancaConfigKeyEnum
        {
            BANCA_PAGA_SERVICIOS = 1,
            BANCA_VENDE_JUEGAMAS = 2,
            BANCA_VENDE_BILLETES = 3,
            BANCA_VENDE_TARJETAS = 4,
            BANCA_VENDE_POLIZAS = 5,
            BANCA_JUEGAMAS_FECHA_REGISTRADA_EN_MARLTON = 6
        }
        public enum ProductosConfig
        {
            LIMITE_MINUTOS_REIMPRESION = 1,
            VENTA_RECARGAS_ENCENDIDA = 2,
            LOTERIA_CIERRE_RECARGAS = 3,
            PERMISOS_RIFERO_USUARIOS = 4,
            PERMISOS_RIFERO_ESPECIAL = 5,
            PERMISOS_RIFERO_OPCIONALES = 6,
            HORA_RECARGAS_INICIO = 7,
            HORA_RECARGAS_CIERRE = 8
        }
        public enum BancaConfigValueEnum
        {
            TRUE = 1,
        }

        public enum RFTransaccionEstado
        {
            Solicitud,
            Cancelada,
            Exitosa,
        }

        public enum RFTiposJugadas
        {
            Exacta,
            Combinada,
            Aproximada
        }
        public enum SorteoConfig
        {
            HoraInicioVentas,
            HoraCierreVentas
        }
        public enum SorteoReferencia
        {
            Pega3Dia,
            Pega4Dia,
            Pega3Noche,
            Pega4Noche,
            ZodiacoDia,
            MascotaNoche,
            LaFechaDia,
            LaFechaNoche,
            QuinielitaSignoMes,
            QuinielitaMascotaMes,
            DobletazoZodiaco,
            DobletazoMascota,
            Pega4Real
        }

        public enum LimitesRF
        {
            GlobalPega3Pega4 = 1,
            PorDinero = 2,
            PorBanca = 3,
            PorRifro = 4
        }
    }
}
