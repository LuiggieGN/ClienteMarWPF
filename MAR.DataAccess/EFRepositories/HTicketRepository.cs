using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;
using MAR.DataAccess.UnitOfWork;

namespace MAR.DataAccess.EFRepositories
{
    public class HTicketRepository : GenericRepository<HTicket>, IHTicketRepository
    {
        public HTicketRepository(MARContext context) 
            : base(context)
        {
        }
       
        public MARContext MarContext { get { return Context as MARContext; } }


        
    }
}
