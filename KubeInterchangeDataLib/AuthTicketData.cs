using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterchangeDataLib
{
    public class AuthTicketData
    {
        /// <summary>
        /// Firstname
        /// </summary>
        public string Firstname = null;
        /// <summary>
        /// Lastname
        /// </summary>
        public string Lastname = null;
        /// <summary>
        /// ID Number
        /// </summary>
        public string ID = null;
        /// <summary>
        /// Date of authorization
        /// </summary>
        public DateTime Date = DateTime.MinValue;

        /// <summary>
        /// Override for ToString().
        /// </summary>
        /// <returns>Dump string</returns>
        public override string ToString()
        {
            string s = string.Empty;

            s += DumpHelper.DumpField("Firstname", Firstname);
            s += " ";
            s += DumpHelper.DumpField("Lastname", Lastname);
            s += " ";
            s += DumpHelper.DumpField("ID", ID);
            s += " ";
            s += DumpHelper.DumpField("Date", Date);
            return s;
        }
    }
}
