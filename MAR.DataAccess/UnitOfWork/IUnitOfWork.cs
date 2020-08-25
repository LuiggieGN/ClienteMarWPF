using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.EFRepositories;


namespace MAR.DataAccess.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDTicketRepository DTicket { get; }
        Task<int> CompleteAsync();
        int Complete();
    }

  
}
