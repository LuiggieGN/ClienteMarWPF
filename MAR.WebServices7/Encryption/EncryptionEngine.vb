Imports System.IO
Imports System.Security.Cryptography

Namespace Encryption

    Friend Class EncryptionEngine

        Private key() As Byte = {}
        Private IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

        Public Function Decrypt(ByVal dataToDecrypt() As Byte, _
            ByVal sEncryptionKey As String) As Byte()
            'Dim inputByteArray(dataToDecrypt.Length) As Byte
            key = System.Text.Encoding.UTF8.GetBytes(Left(sEncryptionKey, 8))
            Dim des As New DESCryptoServiceProvider()
            'inputByteArray = Convert.FromBase64String(dataToDecrypt)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write)
            cs.Write(dataToDecrypt, 0, dataToDecrypt.Length)
            cs.FlushFinalBlock()
            Return ms.ToArray()
        End Function

        Public Function Encrypt(ByVal dataToEncrypt() As Byte, _
            ByVal sEncryptionKey As String) As Byte()
            key = System.Text.Encoding.UTF8.GetBytes(Left(sEncryptionKey, 8))
            Dim des As New DESCryptoServiceProvider()
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write)
            cs.Write(dataToEncrypt, 0, dataToEncrypt.Length)
            cs.FlushFinalBlock()
            Return ms.ToArray()
        End Function

    End Class

End Namespace
