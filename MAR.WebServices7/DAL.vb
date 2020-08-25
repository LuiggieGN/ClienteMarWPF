Imports System.Data
Imports System.Data.SqlClient
Public Class DAL
    Public Class ConfiguracionProductos

    End Class
    Friend Shared StrSQLCon As String = ConfigReader.ReadString(MAR.Config.ConfigEnums.DBConnection2)
    Public Shared Function GetTabla(ByVal pCommand As String, Optional ByVal pParams As List(Of Pair) = Nothing, Optional ByVal pCommandType As CommandType = CommandType.Text, Optional pCommandTimeout As Integer = 210) As DataTable
        Dim TheResult As New DataTable
        Using da As New SqlDataAdapter(pCommand, StrSQLCon)
            da.SelectCommand.CommandType = pCommandType
            da.SelectCommand.CommandTimeout = pCommandTimeout
            If pParams IsNot Nothing Then
                For Each p In pParams
                    da.SelectCommand.Parameters.AddWithValue(p.First, IIf(p.Second Is Nothing, DBNull.Value, p.Second))
                Next
            End If
            da.Fill(TheResult)
        End Using
        Return TheResult
    End Function



End Class
