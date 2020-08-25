using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.DataAccess.UnitOfWork
{
    public class BaseResponse
    {
        public string RetailId { get; set; }
        public int? TerminalId { get; set; }
        public int? SolicitudId { get; set; }
        public DateTime? Fecha { get; set; }
        public DateTime? Hora { get; set; }
    
    }

}
