using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {
        internal static string[] DNSHosts = { "wrosario.ddns.net", "wrosario.dynip.com" }; //El primero es el default
        internal static string[] ClientPorts = { "80", "7500", "6000" }; //El primero es el default, ultimo es el backup, los demas son backwards compatibility
        internal static string ClientEnableLog = @"1";
        internal static string DBServerName = @"MAR-SERVER";
        internal static string DataName = @"DATA000";
        internal static string DBPwd1 = @"p0l9i8j7";
        internal static string DBPwd2 = @"p0l9i8j7";
        internal static string ServiceProtocol = @"http";
        internal static string ServiceFolder = @"mar-svr5/";
        internal static string ServiceFile = @"mar-ptovta.asmx";
        internal static string DBUser1 = @"sa";
        internal static string DBUser2 = @"sa2";
    }
}
