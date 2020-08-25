using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {
        internal static string[] DNSHosts = { "lafamosa.ddns.net", "lafamosa.dynip.com" }; //El primero es el default
        internal static string[] ClientPorts = {"80", "6005"}; //El primero es el default, ultimo es el backup, los demas son backwards compatibility
        
        
        internal static string ClientEnableLog = @"1";
        internal static string DBServerName = @"MIGUEL-SPORT";
        internal static string DataName = @"DATA005";
        internal static string DBPwd1 = @"sp0rpk0qt5x";
        internal static string DBPwd2 = @"st0rpk0qt5z";
        internal static string ServiceProtocol = @"http";
        internal static string ServiceFolder = @"mar-svr5/";
        internal static string ServiceFile = @"mar-ptovta.asmx";
        internal static string DBUser1 = @"sa";
        internal static string DBUser2 = @"sa2";
    }
}
