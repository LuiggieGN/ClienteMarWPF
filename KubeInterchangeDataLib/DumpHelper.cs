using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InterchangeDataLib
{
    /// <summary>
    /// Helper for dump class as string.
    /// </summary>
    public class DumpHelper
    {
        public static string DumpField(string desc, string v)
        {
            string s = string.Empty;

            s += desc + "=";
            s += (v == null) ? "(null)" : v;
            return s;
        }

        public static string DumpField(string desc, DateTime v)
        {
            return DumpField(desc, v.Equals(DateTime.MinValue) ? null : v.ToShortDateString() + " " + v.ToShortTimeString());
        }

        public static string DumpField(string desc, decimal v)
        {
            return DumpField(desc, v.ToString("#,##0.00"));
        }

        public static string DumpField(string desc, int v)
        {
            return DumpField(desc, v.ToString("#,##0"));
        }
    }
}
