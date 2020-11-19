Imports System.Data
Imports System.Data.SqlClient
Public Class DAL
    Public Class ConfiguracionProductos
        Public Shared Function LeeProductoConfig(ByVal pProductoID As Integer, Optional ByVal pConfigOpcion As String = Nothing) As DataTable
            Dim TheKey = String.Format("TblProductoConfig_{0}_{1}", pProductoID, pConfigOpcion)
            Dim TheResult = ObjCache.ReadFromCache(Of DataTable)(TheKey)

            If TheResult Is Nothing Then
                Dim TheTable = GetTabla("Select * from SWebProductoConfig Where WebProductoID=@pro And Activo=1 And Opcion=ISNULL(@opc,Opcion)",
                                     ({New Pair With {.First = "pro", .Second = pProductoID},
                                       New Pair With {.First = "opc", .Second = pConfigOpcion}}).ToList)

                If TheTable IsNot Nothing AndAlso TheTable.Rows.Count > 0 Then
                    ObjCache.AddToCache(TheKey, TheTable)
                    TheResult = TheTable
                End If
            End If

            Return TheResult
        End Function
    End Class

    Public Shared Function GetTabla(ByVal pCommand As String, Optional ByVal pParams As List(Of Pair) = Nothing, Optional ByVal pCommandType As CommandType = CommandType.Text, Optional pCommandTimeout As Integer = 210) As DataTable
        Dim TheResult As New DataTable
        Using da As New SqlDataAdapter(pCommand, PtoVta.StrSQLCon)
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

    Public Class ObjCache
        Public Shared Sub AddToCache(ByVal pCacheKey As String, ByVal pObject As Object, Optional ByVal pExpiration As TimeSpan? = Nothing)
            If Not pExpiration.HasValue Then pExpiration = New TimeSpan(0, 30, 0)
            HttpContext.Current.Cache.Insert(pCacheKey, pObject, Nothing, Now.AddMinutes(pExpiration.Value.TotalMinutes), Cache.NoSlidingExpiration)
        End Sub

        Public Shared Function ReadFromCache(Of T)(ByVal pCacheKey As String) As T
            Return CType(HttpContext.Current.Cache(pCacheKey), T)
        End Function

    End Class

End Class
