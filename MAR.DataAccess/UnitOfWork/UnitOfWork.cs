using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.EFRepositories;
using MAR.DataAccess.Tables.DTOs;
using Newtonsoft.Json;

namespace MAR.DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MARContext _context;

        public UnitOfWork(MARContext context)
        {
            _context = context;
            DTicket = new DTicketRepository(_context);
        }

        public IDTicketRepository DTicket { get; private set; }

        public  virtual Task<int> CompleteAsync()
        {
            return  _context.SaveChangesAsync();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }

    
    }
}
