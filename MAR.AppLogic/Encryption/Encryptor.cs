using System.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;

namespace MAR.AppLogic.Encryption
{

	public class Encryptor
	{

		private static EncryptionEngine _EncEngine;
		private const string _EncMainKey = "rt|\\fhg#";
	    private const string _EncConfigKey = "Aet:8Tf+";
		private const string _EncPasswordKey = "!fh$k93Q";
		private const string _EncSessionKey = "i12f$Lp\\";
		private static EncryptionEngine EncEngine {
			get {
				if (_EncEngine == null)
					_EncEngine = new EncryptionEngine();
				return _EncEngine;
			}
		}

		public static string EncryptSession(string TheSession)
		{
			try {
				dynamic TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheSession), _EncSessionKey);
				return Convert.ToBase64String(TheEncData);
			} catch (Exception ex) {
				return "**Encryption Failed - BAD DATA**";
			}
		}

		public static string EncryptPassword(string ThePassword)
		{
			try {
				dynamic TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(ThePassword), _EncPasswordKey);
				return Convert.ToBase64String(TheEncData);
			} catch (Exception ex) {
				return "**Encryption Failed - BAD DATA**";
			}
		}

		public static string EncryptData(string TheData, bool ToDecryptTodayOnly = false, bool EncodeForWeb = true)
		{
			string functionReturnValue = null;
			try {
				string theKey = _EncMainKey;
				if (ToDecryptTodayOnly)
					theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
				dynamic TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheData), theKey);
				functionReturnValue = Convert.ToBase64String(TheEncData);
				if (EncodeForWeb)
                    functionReturnValue = HttpContext.Current.Server.UrlEncode(functionReturnValue);
			} catch (Exception ex) {
				return "**Encryption Failed - BAD DATA**";
			}
			return functionReturnValue;
		}

		public static string DecryptSession(string TheSession)
		{
			try {
				dynamic DecryptedBytes = EncEngine.Decrypt(Convert.FromBase64String(TheSession), _EncSessionKey);
				return Encoding.UTF8.GetString(DecryptedBytes);
			} catch (Exception ex) {
				return "**Encryption Failed - BAD DATA**";
			}

		}

		public static string DecryptData(string TheData, bool EncryptedForTodayOnly = false, bool EncodedForWeb = true)
		{
			try {
                if (TheData == "") return TheData;
				string theKey = _EncMainKey;
				if (EncryptedForTodayOnly)
					theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
                if (EncodedForWeb) TheData = HttpContext.Current.Server.UrlDecode(TheData);
				var TheEncData = Convert.FromBase64String(TheData);
				return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey));
			} catch (Exception ex) {
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

}
