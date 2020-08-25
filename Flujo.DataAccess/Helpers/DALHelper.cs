using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
namespace MAR.AppLogic.MARHelpers
{
    public class DapperDBHelper
    {
        public static class ConfigReader
        {
            public static string ReadString(MAR.Config.ConfigEnums pConfigId)
            {
                return  Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(pConfigId));
            }
        }
        public static System.Data.SqlClient.SqlConnection GetSqlConnection()
        {
            //string server = DapperDBHelper.ConfigReader.ReadString(MAR.Config.ConfigEnums.DBConnection2);
            string server = "data source=192.168.10.133;initial catalog=DATA001;persist security info=true;MultipleActiveResultSets=true;Min Pool Size=30;Max Pool Size=200;user id=sa;pwd=desarrollo"; // Trabajando Remoto
            return new System.Data.SqlClient.SqlConnection(server);
        }

        public class Encryptor
        {

            private static EncryptionEngine _EncEngine;
            private const string _EncMainKey = "rt|\\fhg#";
            private const string _EncConfigKey = "Aet:8Tf+";
            private const string _EncPasswordKey = "!fh$k93Q";
            private const string _EncSessionKey = "i12f$Lp\\";
            private static EncryptionEngine EncEngine
            {
                get
                {
                    if (_EncEngine == null)
                        _EncEngine = new EncryptionEngine();
                    return _EncEngine;
                }
            }

            public static string EncryptSession(string TheSession)
            {
                try
                {
                    dynamic TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheSession), _EncSessionKey);
                    return Convert.ToBase64String(TheEncData);
                }
                catch (Exception ex)
                {
                    return "**Encryption Failed - BAD DATA**";
                }
            }

            public static string EncryptPassword(string ThePassword)
            {
                try
                {
                    dynamic TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(ThePassword), _EncPasswordKey);
                    return Convert.ToBase64String(TheEncData);
                }
                catch (Exception ex)
                {
                    return "**Encryption Failed - BAD DATA**";
                }
            }

            public static string EncryptData(string TheData, bool ToDecryptTodayOnly = false, bool EncodeForWeb = false)
            {
                string functionReturnValue = null;
                try
                {
                    string theKey = _EncMainKey;
                    if (ToDecryptTodayOnly)
                        theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
                    dynamic TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheData), theKey);
                    functionReturnValue = Convert.ToBase64String(TheEncData);
                }
                catch (Exception ex)
                {
                    return "**Encryption Failed - BAD DATA**";
                }
                return functionReturnValue;
            }

            public static string DecryptSession(string TheSession)
            {
                try
                {
                    dynamic DecryptedBytes = EncEngine.Decrypt(Convert.FromBase64String(TheSession), _EncSessionKey);
                    return Encoding.UTF8.GetString(DecryptedBytes);
                }
                catch (Exception ex)
                {
                    return "**Encryption Failed - BAD DATA**";
                }

            }

            public static string DecryptData(string TheData, bool EncryptedForTodayOnly = false, bool EncodedForWeb = false)
            {
                try
                {
                    if (TheData == "") return TheData;
                    string theKey = _EncMainKey;
                    if (EncryptedForTodayOnly)
                        theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
                    var TheEncData = Convert.FromBase64String(TheData);
                    return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey));
                }
                catch (Exception ex)
                {
                    return "**Encryption Failed - BAD DATA**";
                }
            }

            public static string DecryptConfig(string TheData)
            {
                try
                {
                    if (TheData == "") return TheData;
                    string theKey = _EncConfigKey;
                    theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
                    var TheEncData = Convert.FromBase64String(TheData);





                    return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey));
                }
                catch (Exception ex)
                {
                    return "**Encryption Failed - BAD DATA**";
                }
            }

        }

        internal class EncryptionEngine
        {

            private byte[] key = {

        };
            private byte[] IV = {
            0x12,
            0x34,
            0x56,
            0x78,
            0x90,
            0xab,
            0xcd,
            0xef

        };
            public byte[] Decrypt(byte[] dataToDecrypt, string sEncryptionKey)
            {
                if (string.IsNullOrEmpty(sEncryptionKey)) sEncryptionKey = string.Empty;
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                //inputByteArray = Convert.FromBase64String(dataToDecrypt)
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(dataToDecrypt, 0, dataToDecrypt.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }

            public byte[] Encrypt(byte[] dataToEncrypt, string sEncryptionKey)
            {
                if (string.IsNullOrEmpty(sEncryptionKey)) sEncryptionKey = string.Empty;
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(dataToEncrypt, 0, dataToEncrypt.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }

        }

    }

}
