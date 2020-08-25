using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace MAR.Config.Encryption {
    internal class EncryptionEngine {
        private Byte[] key = {};
        private Byte[] IV = {0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF};

        internal Byte[] Decrypt(Byte[] dataInput,string encKey) {
            key = System.Text.Encoding.UTF8.GetBytes(encKey.Substring(0,8));
            var des = new DESCryptoServiceProvider();
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(dataInput, 0, dataInput.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

        internal Byte[] Encrypt(Byte[] dataInput,string encKey) {
            key = System.Text.Encoding.UTF8.GetBytes(encKey.Substring(0,8));
            var des = new DESCryptoServiceProvider();
            var ms = new MemoryStream();
            var cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
            cs.Write(dataInput, 0, dataInput.Length);
            cs.FlushFinalBlock();
            return ms.ToArray();
        }

    }
}