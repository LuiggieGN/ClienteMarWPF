using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.Config
{
    public static class Reader
    {
        public static string ReadString(ConfigEnums pConfigId)
        {
            string result = null;
            switch (pConfigId)
            {
                case ConfigEnums.DBConnection1:
                    {
                        result = Settings.DBConnString1;
                        break;
                    }
                case ConfigEnums.DBConnection2:
                    {
                        result = Settings.DBConnString2;
                        break;
                    }
                case ConfigEnums.ServiceURL:
                    {
                        result = Settings.ServiceURL;
                        break;
                    }
                case ConfigEnums.ServiceLocalURL:
                    {
                        result = Settings.ServiceLocalURL;
                        break;
                    }
                case ConfigEnums.ServicePort:
                    {
                        result = Settings.ServiceDefaultPort;
                        break;
                    }
                case ConfigEnums.ServiceBackupPort:
                    {
                        result = Settings.ServiceBackupPort;
                        break;
                    }
                case ConfigEnums.AdminVersion:
                    {
                        result = Settings.AdminVersion;
                        break;
                    }
                case ConfigEnums.ClientWinVersion:
                    {
                        result = Settings.ClientWinVersion;
                        break;
                    }
                case ConfigEnums.ClientEnableLog:
                    {
                        result = Settings.ClientEnableLog;
                        break;
                    }
                case ConfigEnums.ServiceHostName:
                    {
                        result = Settings.DNSHosts[0];
                        break;
                    }
                case ConfigEnums.ClientWebVersion:
                    {
                        result = Settings.ClientWebVersion;
                        break;
                    }
                case ConfigEnums.AdminReleaseNotes:
                    {
                        result = String.Join("|", Settings.AdminCambios
                                         .Where(rn => rn.Mostrar)
                                         .Take(15)
                                         .Select(rn => rn.Descripcion)
                                         .ToArray());
                        break;
                    }
                case ConfigEnums.ClienteWinReleaseNotes:
                    {
                        result = String.Join("|", Settings.ClienteWinCambios
                                         .Where(rn => rn.Mostrar)
                                         .Take(15)
                                         .Select(rn => rn.Descripcion)
                                         .ToArray());
                        break;
                    }
                case ConfigEnums.ClienteWebReleaseNotes:
                    {
                        result = String.Join("|", Settings.ClienteWebCambios
                                         .Where(rn => rn.Mostrar)
                                         .Take(15)
                                         .Select(rn => rn.Descripcion)
                                         .ToArray());
                        break;
                    }

                default:
                    {
                        throw new Exception("Missing MAR-Config Entry");
                        //result = @"";
                        //break;
                    }
            };
            return Encryption.Encryptor.EncryptConfig(result);
        }

        public static string[] ReadStringArray(ConfigEnums pConfigId)
        {
            string[] result = null;
            switch (pConfigId)
            {
                case ConfigEnums.AllowedWebHosts:
                    {
                        result = Settings.ServerAllowedHosts;
                        break;
                    }
                default:
                    {
                        result = new string[] {ReadString(pConfigId)};
                        break;
                    }
            };
            for (int s = 0; s < result.Length;s++ )
            {
                result[s] = Encryption.Encryptor.EncryptConfig(result[s]);
            };
            return result;
        }

    }
}
