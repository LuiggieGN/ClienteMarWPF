using System;
using System.Globalization;


namespace ClienteMarWPFWin7.Domain.Models.Dtos
{
    public class TicketDTO
    {
        public int Ticket { get; set; }
        public string TicketNo { get; set; }
        public DateTime Fecha { get; set; }
        public string StrHora
        {
            get
            {
                return Fecha.ToString("hh:mm tt", CultureInfo.InvariantCulture);
            }
        }

        public bool Nulo { get; set; }

        public double Costo { get; set; }
        public double Pago { get; set; }
        public int Loteria { get; set; }
        public string Err { get; set; }

        public double Solicitud { get; set; }
        public JugadasDTO[] Items { get; set; }
    }

    public class JugadasDTO
    {
        public int Loteria { get; set; }
        public double Cantidad { get; set; }
        public double Costo { get; set; }
        public double Pago { get; set; }

        public string Numero { get; set; }
        public string QP { get; set; }
    }
}
