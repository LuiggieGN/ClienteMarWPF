using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class TicketDetalles
    {
        public int TicketID { get; set; }
        public int DetalleID { get; set; }
        public string Codigo { get; set; }
        public int SorteoID { get; set; }
        public int Monto { get; set; }
        public int Saco { get; set; }
        public string Jugada { get; set; }
        public int TipoJugadaID { get; set; }
    }
}
