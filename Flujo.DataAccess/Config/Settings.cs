using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    static internal partial class Settings
    {
        internal static string DBConnString1
        { 
            get {
                return string.Format("data source={0};initial catalog={1};persist security info=true;MultipleActiveResultSets=true;Min Pool Size=10;user id={2};pwd={3}",
                    Settings.DBServerName, Settings.DataName, Settings.DBUser1, Settings.DBPwd1);
            }
        }

        internal static string DBConnString2
        {
            get
            {
                return string.Format("data source={0};initial catalog={1};persist security info=true;MultipleActiveResultSets=true;Min Pool Size=10;user id={2};pwd={3}",
                    Settings.DBServerName, Settings.DataName, Settings.DBUser2, Settings.DBPwd2);
            }
        }

        internal static string ServiceURL 
        {
            get
            {
                return string.Format("{0}://{1}:{2}/{3}{4}", 
                                        Settings.ServiceProtocol, 
                                        Settings.DNSHosts[0],
                                        Settings.ClientPorts[0],
                                        Settings.ServiceFolder,
                                        Settings.ServiceFile);
            }
        }
            
        internal static string ServiceLocalURL
        {
            get
            {
                return string.Format("{0}://localhost:{1}/{2}{3}",
                                        Settings.ServiceProtocol,
                                        ServiceBackupPort,
                                        Settings.ServiceFolder,
                                        Settings.ServiceFile);
            }
        }

        internal static string ServiceDefaultPort 
        {
            get
            {
                return Settings.ClientPorts[0];
            }
        }

        internal static string ServiceBackupPort { 
            get {
                return Settings.ClientPorts[Settings.ClientPorts.Length-1];
            } 
        }

        internal static string[] ServerAllowedHosts
        {
            get
            {
                List<string> result= new List<string>();
                result.AddRange((from h in Settings.DNSHosts
                                join p in Settings.ClientPorts on 1 equals 1
                                select string.Format("{0}:{1}",h.ToLower(),p)));
                return result.ToArray();
            }
        }

        internal static string DataNumber
        {
            get
            {
                return Settings.DataName.Substring(Settings.DataName.Length-3);
            }
        }
    }
}
