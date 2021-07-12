using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class TicketResponseModel
    {
        public Ticket[] Tickets { get; set; }
        public class Ticket
        {
            public string TicNumero { get; set; }
            public string TicFecha { get; set; }
            public string Hora { get; set; }
            public int TicketID { get; set; }
            public int Sorteo { get; set; }
            public decimal TicCosto { get; set; }
            public decimal TicPago { get; set; }
            public string TicketMartlon { get; set; }
            public string TicketControl { get; set; }

            public bool TicNulo { get; set; }
            public string Pin { get; set; }
            public string SorteoNombre { get; set; }
            public JugadaDetalle[] Jugadas { get; set; }
            public string Firma { get; set; }
            public string Estado { get; set; }
            public string NumeroSorteo { get; set; }
            public string FechaSorteo { get; set; }
            public string HoraSorteo { get; set; }
            public string Serial { get; set; }
            public static string Url { get; set; }

            public class JugadaDetalle
            {
                public double Cantidad { get; set; }
                public string Jugada { get; set; }
                public double Monto { get; set; }
                public string Codigo { get; set; }
            }
        }
    }
}
