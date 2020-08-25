using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {
        internal static string[] DNSHosts = { "consorcioMJ.ddns.net"}; //El primero es el default
        internal static string[] ClientPorts = {"80", "6211"}; //El primero es el default, ultimo es el backup, los demas son backwards compatibility
        
        
        internal static string ClientEnableLog = @"1";
        internal static string DBServerName = @"MAR-MJ";
        internal static string DataName = @"DATA211";
        internal static string DBPwd1 = @"sp2rpk1qt1x";
        internal static string DBPwd2 = @"st2rpk1qt1z";
        internal static string ServiceProtocol = @"http";
        internal static string ServiceFolder = @"mar-svr5/";
        internal static string ServiceFile = @"mar-ptovta.asmx";
        internal static string DBUser1 = @"sa";
        internal static string DBUser2 = @"sa2";
    }
}
