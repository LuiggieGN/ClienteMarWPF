using System;
using System.Collections.Generic;


namespace MAR.DataAccess.Tables.DTOs
{
    public partial class DTicket
    {
        //New Properties and Navegations
        public MBanca MBanca { get; set; }
        public ICollection<DTicketDetalle> DTicketDetalles { get; set; }

        public DTicket()
        {
            DTicketDetalles = new HashSet<DTicketDetalle>();
        }
    }
}
