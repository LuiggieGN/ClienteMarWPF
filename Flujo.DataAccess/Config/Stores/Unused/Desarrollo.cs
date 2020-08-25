using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {
        internal static string[] DNSHosts = { "localhost", "test.matecomsa.com" }; //El primero es el default
        internal static string[] ClientPorts = { "14217", "14217" }; //El primero es el default, ultimo es el backup, los demas son backwards compatibility
        internal static string ClientEnableLog = @"1";
        internal static string DBServerName = @"(local)";
        internal static string DataName = @"DATA000";
        internal static string DBPwd1 = @"sp0rpk0qt0x";
        internal static string DBPwd2 = @"sp0rpk0qt0x";
        internal static string ServiceProtocol = @"http";
        internal static string ServiceFolder = @"";
        internal static string ServiceFile = @"mar-ptovta.asmx";
        internal static string DBUser1 = @"sa";
        internal static string DBUser2 = @"sa";
    }
}
