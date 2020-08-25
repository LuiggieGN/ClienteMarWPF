using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {
        internal static string[] DNSHosts = { "barrieto.ddns.net" }; //El primero es el default
        internal static string[] ClientPorts = { "80", "6293" }; //El primero es el default, ultimo es el backup, los demas son backwards compatibility
        internal static string ClientEnableLog = @"1";  // 0 consorcios fuera de RD y 1 para consorcios dentro de RD    // 0 consorcios fuera de RD y 1 para consorcios dentro de RD  
        internal static string DBServerName = @"MAIN01";
        internal static string DataName = @"DATA293";
        internal static string DBPwd1 = @"sp0rpk0qt0x";
        internal static string DBPwd2 = @"st0rpk0qt0z";
        internal static string ServiceProtocol = @"http";
        internal static string ServiceFolder = @"mar-svr5/";
        internal static string ServiceFile = @"mar-ptovta.asmx";
        internal static string DBUser1 = @"sa";
        internal static string DBUser2 = @"sa2";
    }
}
