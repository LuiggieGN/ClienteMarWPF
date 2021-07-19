using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ClienteMarWPFWin7.Domain.Enums.CincoMinutosEnum;
using static ClienteMarWPFWin7.Domain.Models.Dtos.TicketResponseModel;

namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class ConsultaPagoResponseModel: BaseCincoMinutosDTO
    {
        public _RespuestaApi RespuestaApi { get; set; }
        public class _RespuestaApi
        {

            public string FechaHoraRespuesta { get; set; }
            public string CodigoRespuesta { get; set; }
            public string MensajeRespuesta { get; set; }
            public object Peticion { get; set; }
            public int Saco { get; set; }
            public TicketEstado TicketEstado { get; set; }
            public List<_TicketDetalle> TicketDetalle { get; set; }
            public class _TicketDetalle
            {
                public string Sorteo { get; set; }
                public string Codigo { get; set; }
                public string SorteoID { get; set; }
                public string SorteoNumero { get; set; }
                public int TipoJugadaID { get; set; }
                public int Saco { get; set; }
                public string Jugada { get; set; }
                public int Aposto { get; set; }
                public string FechaPago { get; set; }
                public int JugadaEstado { get; set; }
            }
        }
    }
}
