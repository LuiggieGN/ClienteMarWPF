using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.Domain.Enums
{
    public class CincoMinutosEnum
    {
        public enum TicketEstado
        {
            NoValido = 0,
            JugadoNoSorteo = 1,
            JugadoNoGanador = 2,
            JugadoGanadorNoPago = 3,
            JugadoGanadorPagado = 4
        }
    }
}
