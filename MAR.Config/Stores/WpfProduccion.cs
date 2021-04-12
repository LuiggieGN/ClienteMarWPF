using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {

        internal static string[] DNSHosts = { "pruebasmar.ddns.net", "pruebasmar.ddns.net" }; //El primero es el default
        internal static string[] ClientPorts = { "80", "6999" }; //El primero es el default, ultimo es el backup, los demas son backwards compatibility
        internal static string ClientEnableLog = @"1";
        internal static string DBServerName = @"xx";
        internal static string DataName = @"xxx";
        internal static string DBPwd1 = @"xx";
        internal static string DBPwd2 = @"xx";
        internal static string ServiceProtocol = @"http";
        internal static string ServiceFolder = @"mar-svr5/";
        internal static string ServiceFile = @"mar-ptovta.asmx";
        internal static string DBUser1 = @"xx";
        internal static string DBUser2 = @"xx";





    }
}