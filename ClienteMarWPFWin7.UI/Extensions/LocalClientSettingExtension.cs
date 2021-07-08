using Microsoft.VisualBasic;
using System;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text.RegularExpressions;
using DeviceId;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.UI.ViewModels.Helpers;
using Microsoft.Win32;


namespace ClienteMarWPFWin7.UI.Extensions
{
    public static class LocalClientSettingExtension
    {
        /// <summary>
        /// Implementacion de Domingo de Hardware Key
        /// </summary>
        /// <param name="setting">Configuracion local de terminal</param>
        /// <returns>String unico por hardware</returns>
        public static string GetHwKeyOldSchoold(this LocalClientSettingDTO setting) 
        {
            if (setting != null
                && setting.BancaId != 0
                && setting.Direccion != string.Empty
                && (!InputHelper.InputIsBlank(setting.Direccion)))
            {
                var hwkey = GetHwKeyInt().ToString();
                return hwkey;
            }
            return "0";
        }

        public static int GetHwKeyInt()
        {
            int valor = 0;

            try
            {
                var rg = Registry.LocalMachine;
                var rg2 = rg.OpenSubKey("SYSTEM\\CurrentControlSet\\Control\\DeviceClasses\\{53f56307-b6bf-11d0-94f2-00a0c91efb8b}");
                var miembros = rg2.GetSubKeyNames();

                string stUpperCase; int x = 1;

                foreach (var st in miembros)
                {
                    stUpperCase = Strings.UCase(st);

                    if (
                         Strings.InStr(stUpperCase, "DISK") > 0 && 
                         Strings.InStr(stUpperCase, "USB") == 0                        
                       )
                    {
                        x = 1;

                        for (; x <= st.Length; x++)
                        {
                            valor += Strings.Asc(Strings.Mid(st,x,1));
                        }
                        valor += st.Length;

                    }//fin de If

                }//fin de foreach
            }
            catch  
            {
                valor += 45158;
            }
            return valor;
        }

        /// <summary>
        /// Implementacion de Luiggie de Hardware Key
        /// </summary>
        /// <param name="setting">Configuracion local de terminal</param>
        /// <returns>String unico por hardware</returns>
        public static string GetHwKey(this LocalClientSettingDTO setting)
        {
            if (setting != null
                && setting.BancaId != 0
                && setting.Direccion != string.Empty
                && (!InputHelper.InputIsBlank(setting.Direccion)))
            {
                var hwkey = GetHwKey(bancaid: setting.BancaId, bancaip: setting.Direccion);
                return hwkey;
            }

            return "0";
        }

        public static string GetHwKey(int bancaid, string bancaip)
        {
            string hwkey;

            BigInteger pcUniqueId = (BigInteger)(((GetPCIdValue(GetPCId()) + 4321) * 111) % 1000000);

            BigInteger pcAddressValue = (BigInteger)(((GetLongFromIPv4(dirreccionIPv4: bancaip) + 4321) * 111) % 1000000);

            hwkey = $"{ (((bancaid + pcUniqueId + pcAddressValue) * 111) % 1000000)}";

            return hwkey;
        }

        public static string GetPCId()
        {
            var deviceid = new DeviceIdBuilder()
                               .AddProcessorId()
                               .AddMotherboardSerialNumber()
                               .AddSystemDriveSerialNumber()
                               .ToString();
            return deviceid;
        }

        public static decimal GetPCIdValue(string pcid)
        {
            if (pcid == null || pcid == string.Empty || Regex.Replace(pcid, @"\s+", "") == string.Empty)
            {
                return 0;
            }

            try
            {
                decimal sumASCII = 0;

                foreach (char caracter in pcid)
                {
                    sumASCII += Convert.ToInt32(caracter);
                }

                return sumASCII;
            }
            catch
            {
                return 0;
            }
        }

        public static decimal GetLongFromIPv4(string dirreccionIPv4)
        {
            try
            {
                uint intaddress = 0;

                if (ValidarIPv4(dirreccionIPv4))
                {
                    var objipaddress = IPAddress.Parse(dirreccionIPv4);

                    var bytes = objipaddress.GetAddressBytes();

                    Array.Reverse(bytes);

                    intaddress = BitConverter.ToUInt32(bytes, 0);
                }

                return intaddress;
            }
            catch
            {
                return 0;
            }
        }

        public static bool ValidarIPv4(string dirreccionIPv4)
        {
            if (String.IsNullOrWhiteSpace(dirreccionIPv4))
            {
                return false;
            }

            string[] splitValues = dirreccionIPv4.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;

            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }









    }
}
