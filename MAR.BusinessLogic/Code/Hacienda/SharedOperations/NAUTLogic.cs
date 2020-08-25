using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.Hacienda.SharedOperations
{
   public  class NAUTLogic
    {
        public static string GeneraNautCalculado(string pEstablecimientoId, string pCodigoOperacion, string pCodigoFueraDeLinea, char pInicialTipoOperacion = 'A')
        {
            string combinado = pEstablecimientoId + pCodigoOperacion + pCodigoFueraDeLinea;
            string EncryptedData = EncryptingStringData.EncryptHMACSHA1(combinado, pTipo:pInicialTipoOperacion);
            return EncryptedData;
        }
    }
    public class EncryptingStringData
    {

        public static string EncryptHMACSHA1(string cadena, string key = "HaciendaEncrypting", char pTipo = 'A')
        {


            ASCIIEncoding encoding = new ASCIIEncoding();

            byte[] keyByte = encoding.GetBytes(key);

            HMACMD5 hmacmd5 = new HMACMD5(keyByte);
            //HMACSHA1 hmacsha1 = new HMACSHA1(keyByte);
            //HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);


            byte[] messageBytes = encoding.GetBytes(cadena);

            byte[] hashmessage = hmacmd5.ComputeHash(messageBytes);

            string hmac2 = ByteToString(hashmessage);


            return pTipo + hmac2.Remove(0, 18); //SUSTITUIR 18 CARACTERES POR LA LETRA DEL TIPO DE OPERACION

        }


        /*converts byte to encrypted string*/
        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}
