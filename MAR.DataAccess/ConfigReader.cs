using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace MAR.DataAccess
{
    static internal class ConfigReader
    {

        static internal string ReadString(MAR.Config.ConfigEnums pConfigId)
        {
            return Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(pConfigId));
        }

        static internal string[] ReadStringArray(MAR.Config.ConfigEnums pConfigId)
        {
            dynamic theArray = MAR.Config.Reader.ReadStringArray(pConfigId);
            for (int n = 0; n <= theArray.Length - 1; n++)
            {
                theArray[n] = Encryptor.DecryptConfig(theArray(n));
            }
            return theArray;
        }

        private class Encryptor
        {
            private static EncryptionEngine _EncEngine;
            private const string _EncConfigKey = "Aet:8Tf+";
            private static EncryptionEngine EncEngine
            {
                get
                {
                    if (_EncEngine == null)
                        _EncEngine = new EncryptionEngine();
                    return _EncEngine;
                }
            }
            public static string DecryptConfig(string TheData)
            {
                try
                {
                    string theKey = _EncConfigKey;
                    theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
                    dynamic TheEncData = Convert.FromBase64String(TheData);
                    return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey));
                }
                catch (Exception ex)
                {
                    //Log exception somewhere
                    return "**Encryption Failed - BAD DATA**";
                }
            }
        }

        private class EncryptionEngine
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
                //Dim inputByteArray(dataToDecrypt.Length) As Byte
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