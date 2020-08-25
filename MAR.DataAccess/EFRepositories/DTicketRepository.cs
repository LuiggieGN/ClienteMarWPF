using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.UnitOfWork;

namespace MAR.DataAccess.EFRepositories
{
    public class DTicketRepository : GenericRepository<DTicket>, IDTicketRepository
    {
        public DTicketRepository(MARContext context) 
            : base(context)
        {
        }
       
        public MARContext MarContext { get { return Context as MARContext; } }

        public void RealizarVenta_Billete(DTicket ticket)
        {
            MarContext.DTickets.Add(ticket);
        }

        public IEnumerable<DTicket> GetTickets_Vendidos(DateTime desde, DateTime hasta)
        {
            MARContext marContext = new MARContext();
            return marContext.DTickets.Where(x => x.TicFecha >= desde && x.TicFecha <= hasta);
        }

        public decimal GetLastSolicitud()
        {
            if (MarContext.DTickets.Any(x => x.LoteriaID == 13))
            {
                decimal tt =  Convert.ToDecimal(MarContext.DTickets.OrderByDescending(p => p.TicketID)
                    .Select(r => r.TicSolicitud)
                    .First().ToString("0000000000.##"));
                return tt;
            }
            else
            {
                return Convert.ToDecimal(string.Empty.PadLeft(18, '0'));
            }
        }
    }
}
