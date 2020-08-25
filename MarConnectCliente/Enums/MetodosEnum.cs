using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.Enums
{
    public class MetodosEnum
    {
        public enum MetodoServicio
        {
            CompraFondo = 2,
            Apuesta = 3,
            Anulacion = 4,
            PagoGanador = 5,
            Inicio = 8,
            VentaMayoreo = 12,
            CompraMayoreo = 14,
            PagoMayoreo = 11,
            CobroMayoreo = 13,
            CierreSorteo = 7,
            CierreDia = 6,
            Ping  = 9,
            ConsultaPagoGanador = 10,
            ConsultaSaco = 16,
            ConsultaAcumulado = 17,
            ConsultaPremios = 18,

        }
        public enum HttpMethod
        {
            GET, POST, PUT, PATCH, DELETE, COPY, HEAD, OPTIONS, LINK, UNLINK, PURGE
        }
    }
}
