using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.UnitOfWork;

namespace MAR.DataAccess.EFRepositories
{
    public interface IDTicketRepository : IGenericRepository<DTicket>
    {
        void RealizarVenta_Billete(DTicket ticket);
        IEnumerable<DTicket> GetTickets_Vendidos(DateTime desde, DateTime hasta);
        decimal GetLastSolicitud();
    }
}
