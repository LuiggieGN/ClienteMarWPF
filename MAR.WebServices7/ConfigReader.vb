Friend Class ConfigReader

    Friend Shared Function ReadString(pConfigId As MAR.Config.ConfigEnums) As String
        Return Encryption.Encryptor.DecryptConfig(MAR.Config.Reader.ReadString(pConfigId))
    End Function

    Friend Shared Function ReadStringArray(pConfigId As MAR.Config.ConfigEnums) As String()
        Dim theArray = MAR.Config.Reader.ReadStringArray(pConfigId)
        For n As Integer = 0 To theArray.Length - 1
            theArray(n) = Encryption.Encryptor.DecryptConfig(theArray(n))
        Next
        Return theArray
    End Function

End Class
