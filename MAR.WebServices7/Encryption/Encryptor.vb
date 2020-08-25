Imports System.Text

Namespace Encryption

    Friend Class Encryptor

        'ALERT: Don't compile this project yet, must encrypt the connection string and the passwords first
        Private Shared _EncEngine As EncryptionEngine
        Private Const _EncMainKey As String = "rt|\fhg#"
        Private Const _EncConfigKey As String = "Aet:8Tf+"
        Private Const _EncPasswordKey As String = "!fh$k93Q"
        Private Const _EncSessionKey As String = "i12f$Lp\"

        Private Shared ReadOnly Property EncEngine() As EncryptionEngine
            Get
                If _EncEngine Is Nothing Then _EncEngine = New EncryptionEngine
                Return _EncEngine
            End Get
        End Property

        Public Shared Function EncryptSession(ByVal TheSession As String) As String
            Try
                Dim TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheSession), _EncSessionKey)
                Return Convert.ToBase64String(TheEncData)
            Catch ex As Exception
                Return "**Encryption Failed - BAD DATA**"
            End Try
        End Function

        Public Shared Function EncryptPassword(ByVal ThePassword As String) As String
            Try
                Dim TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(ThePassword), _EncPasswordKey)
                Return Convert.ToBase64String(TheEncData)
            Catch ex As Exception
                Return "**Encryption Failed - BAD DATA**"
            End Try
        End Function

        Public Shared Function EncryptData(ByVal TheData As String, Optional ByVal ToDecryptTodayOnly As Boolean = False, Optional ByVal EncodeForWeb As Boolean = True) As String
            Try
                Dim theKey As String = _EncMainKey
                If ToDecryptTodayOnly Then theKey = (Today.DayOfYear.ToString & theKey)
                Dim TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheData), theKey)
                EncryptData = Convert.ToBase64String(TheEncData)
                If EncodeForWeb Then EncryptData = HttpContext.Current.Server.UrlEncode(EncryptData)
            Catch ex As Exception
                Return "**Encryption Failed - BAD DATA**"
            End Try
        End Function

        Public Shared Function EncryptConfig(ByVal TheData As String) As String
            Try
                Dim theKey As String = _EncConfigKey
                theKey = (Today.DayOfYear.ToString & theKey)
                Dim TheEncData = EncEngine.Encrypt(Encoding.UTF8.GetBytes(TheData), theKey)
                EncryptConfig = Convert.ToBase64String(TheEncData)
            Catch ex As Exception
                Return "**Encryption Failed - BAD DATA**"
            End Try
        End Function

        Public Shared Function DecryptSession(ByVal TheSession As String) As String
            Try
                Dim DecryptedBytes = EncEngine.Decrypt(Convert.FromBase64String(TheSession), _EncSessionKey)
                Return Encoding.UTF8.GetString(DecryptedBytes)
            Catch ex As Exception
                Return "**Encryption Failed - BAD DATA**"
            End Try

        End Function

        Public Shared Function DecryptData(ByVal TheData As String, Optional ByVal EncryptedForTodayOnly As Boolean = False, Optional ByVal EncodedForWeb As Boolean = True) As String
            Try
                Dim theKey As String = _EncMainKey
                If EncryptedForTodayOnly Then theKey = (Today.DayOfYear.ToString & theKey)
                'If EncodedForWeb Then TheData = HttpContext.Current.Server.UrlDecode(TheData)
                Dim TheEncData = Convert.FromBase64String(TheData)
                Return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey))
            Catch ex As Exception
                Return "**Encryption Failed - BAD DATA**"
            End Try
        End Function

        Public Shared Function DecryptConfig(ByVal TheData As String) As String
            Try
                Dim theKey As String = _EncConfigKey
                theKey = (Today.DayOfYear.ToString & theKey)
                Dim TheEncData = Convert.FromBase64String(TheData)
                Return Encoding.UTF8.GetString(EncEngine.Decrypt(TheEncData, theKey))
            Catch ex As Exception
                Return "**Encryption Failed - BAD DATA**"
            End Try
        End Function
    End Class

End Namespace
