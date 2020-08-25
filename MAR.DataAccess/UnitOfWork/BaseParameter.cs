using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.DataAccess.UnitOfWork
{
    public class BaseParameter
    {
        ///Marlton Parameters
        public string Login { get; set; }
        public string Password { get; set; }
        public string RetailId { get; set; }
        public int TerminalId { get; set; }
        public int SolicitudId { get; set; }
        public string CurlString { get; set; }
        public Uri ServiceUrl { get; set; }
        public Dictionary<string, string> HeardersDictionary { get; set; }
        
        ///Midas Parameters
        public string User { get; set; }
        public string Pass { get; set; }
       

        public class Header
        {
            public class BilleteHeader
            {
                public string Login { get; set; }
                public string Password { get; set; }
                public string RetailId { get; set; }
                public int TerminalId { get; set; }
            }
        }
    }
}
