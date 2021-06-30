using System;
using System.Globalization;


namespace MAR.DataAccess.Tables.ControlEfectivoDTOs
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

        public int Costo { get; set; }
    }
}
