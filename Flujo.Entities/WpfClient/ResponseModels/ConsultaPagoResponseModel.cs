using Flujo.Entities.WpfClient.PublicModels;
using Flujo.Entities.WpfClient.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Flujo.Entities.WpfClient.Enums.CincoMinutosEnum;
using static Flujo.Entities.WpfClient.PublicModels.TicketResponseModel;

namespace Flujo.Entities.WpfClient.RequestModel
{
    public class ConsultaPagoResponseModel : BaseCincoMinutosResponseModel
    {
       
        public _RespuestaApi RespuestaApi { get; set; }
        public Ticket Ticket { get; set; }
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
