using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace MAR.Config.Encryption
{

    internal class Encryptor
    {
        private static EncryptionEngine _EncEngine;
        //private const string _EncMainKey = @"rt|\fhg#";
        private const string _EncConfigKey = @"Aet:8Tf+";
        //private const string _EncPasswordKey = @"!fh$k93Q";
        //private const string _EncSessionKey = @"i12f$Lp\";

        private static EncryptionEngine EncEngine
        {
            get
            {
                if (_EncEngine == null) _EncEngine = new EncryptionEngine();
                return _EncEngine;
            }
        }

        public static string EncryptConfig(string TheData)
        {
            try
            {
                var theKey = _EncConfigKey;
                theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
                var TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheData), theKey);
                var result = Convert.ToBase64String(TheEncData);
                return result;
            }
            catch (Exception ex)
            {
                return @"**Encryption Failed - BAD DATA**";
            }
        }

        public static string DecryptConfig(string TheData)
        {
            try
            {
                var theKey = _EncConfigKey;
                theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
                var TheEncData = Convert.FromBase64String(TheData);
                return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey));
            }
            catch (Exception ex)
            {
                return @"**Encryption Failed - BAD DATA**";
            }
        }

        //public static string EncryptSession(string pSession) {
        //    try {
        //        var TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(pSession), _EncSessionKey);
        //        return Convert.ToBase64String(TheEncData);
        //    }
        //    catch(Exception ex) {
        //        return @"**Encryption Failed - BAD DATA**";
        //    }
        //}

        //public static string EncryptPassword(string pPassword) {
        //    try {
        //        var TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(pPassword), _EncPasswordKey);
        //        return Convert.ToBase64String(TheEncData);
        //    }
        //    catch(Exception ex) {
        //        return @"**Encryption Failed - BAD DATA**";
        //    }
        //}

        //public static string EncryptData(string TheData, bool ToDecryptTodayOnly = false, bool EncodeForWeb = true) {
        //    try
        //    {
        //        var theKey = _EncMainKey;
        //        if (ToDecryptTodayOnly) theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
        //        var TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheData), theKey);
        //        var result = Convert.ToBase64String(TheEncData);
        //        if (EncodeForWeb) result = HttpContext.Current.Server.UrlEncode(result);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        return @"**Encryption Failed - BAD DATA**";
        //    }
        //}

        //public static string DecryptSession(string TheSession) {
        //    try {
        //        var DecryptedBytes = EncEngine.Decrypt(Convert.FromBase64String(TheSession), _EncSessionKey);
        //        return Encoding.UTF8.GetString(DecryptedBytes);
        //    }
        //    catch(Exception ex) {
        //        return @"**Encryption Failed - BAD DATA**";
        //    }
        //}

        //public  static string DecryptData(string TheData, bool EncryptedForTodayOnly = false, bool EncodedForWeb = true) {
        //    try {
        //        var theKey = _EncMainKey;
        //        if (EncryptedForTodayOnly) theKey = (DateTime.Today.DayOfYear.ToString() + theKey);
        //        if (EncodedForWeb) TheData = HttpContext.Current.Server.UrlDecode(TheData);
        //        var TheEncData = Convert.FromBase64String(TheData);
        //        return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey));
        //    } catch(Exception ex) {
        //        return @"**Encryption Failed - BAD DATA**";
        //    }
        //}


    }
}