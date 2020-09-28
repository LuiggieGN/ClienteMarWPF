using System;
using System.Collections.Generic;
using System.Text;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class TicketModel
    {
        public string NoTicket { get; set; }
        public int TicketID { get; set; }
        public string NautCalculado { get; set; }
        public DateTime Fecha { get; set; }
        public int TerminalID { get; set; }
        public string AutenticacionReferencia { get; set; }
        public string CodigoOperacionReferencia { get; set; }

        public int LocalID { get; set; }
        public int MontoOperacion { get; set; }
        public List<TicketDetalle> TicketDetalles { get; set; }
    }
    public class TicketDetalle
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

    public class DetalleJugadas
    {
        public List<JuegoPago> Juego { get; set; }
    }

    public class JuegoPago
    {
        public int TipoJugadaID { get; set; }
        public string Codigo { get; set; }
        public int MontoApostado { get; set; }
        public int MontoPagado { get; set; }
        public string Jugada { get; set; }

    }

    public class Jugadas
    {
        public string TipoJugada { get; set; }
        public string Jugada { get; set; }
        public int Monto { get; set; }
    }

}
