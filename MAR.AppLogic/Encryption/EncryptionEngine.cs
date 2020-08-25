using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;

namespace MAR.AppLogic.Encryption
{

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
			if (string.IsNullOrEmpty(sEncryptionKey)) sEncryptionKey=string.Empty;
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

