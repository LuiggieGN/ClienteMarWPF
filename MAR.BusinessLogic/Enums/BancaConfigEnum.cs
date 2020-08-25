using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Enums
{
    public class BancaConfigEnums
    {
        public enum BancaConfigKeyEnum
        {
            BANCA_PAGA_SERVICIOS = 1,
            BANCA_VENDE_JUEGAMAS = 2,
            BANCA_VENDE_BILLETES = 3,
            BANCA_VENDE_TARJETAS = 4,
            BANCA_VENDE_POLIZAS = 5,
            BANCA_VENDE_PEGA4 = 6,
            BANCA_VENDE_JUEGOSNUEVOS = 7,
            BANCA_PAGA_DE_OTRA = 8,
            BANCA_OTRA_LE_PUEDE_PAGAR = 9,
            BANCA_VENDE_LOTERIASNUEVAS = 10,
            BANCA_VENDE_BINGO = 11,
            //BANCA_BLOQUEA_FLUJO = 12,
            BANCA_ACTIVA_FLUJO = 12,
            BANCA_INCLUYE_COMISIONES_FLUJO = 13,
            BANCA_INTERVALO_INACTIVIDAD_MINUTOS = 14,
            BANCA_VENDE_CINCOMINUTOS = 15,
            BANCA_HORA_RECARGAS_INICIO = 16,
            BANCA_HORA_RECARGAS_CIERRE = 17,
        }
        public enum BancaConfigValueEnum
        {
            TRUE = 1,
            NULL = 2
        }

    }
}
