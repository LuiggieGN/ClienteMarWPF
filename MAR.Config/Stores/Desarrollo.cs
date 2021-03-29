using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    internal static partial class Settings
    {

        internal static string[] DNSHosts = { "localhost", "localhost" }; //El primero es el default
        internal static string[] ClientPorts = { "80", "80" }; //El primero es el default, ultimo es el backup, los demas son backwards compatibility
        internal static string ClientEnableLog = @"1";
        internal static string DBServerName = @".\SQLSERVER217";
        internal static string DataName = @"DATA002";
        internal static string DBPwd1 = @"XXXX";
        internal static string DBPwd2 = @"XXXX";
        internal static string ServiceProtocol = @"http";
        internal static string ServiceFolder = @"MarWebServiceMarHacienda/";
        internal static string ServiceFile = @"mar-ptovta.asmx";
        internal static string DBUser1 = @"sa";
        internal static string DBUser2 = @"sa";





    }
}