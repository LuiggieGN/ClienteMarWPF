Public Class MarSerializador

    '┼ separador de propiedades ASCII 197
    '╪ separador de sub-objetos ASCII 216
    '╫ separador de objetos ASCII 215
    '╬ separador de coleccion ASCII 206
    'MarSerializador version 1.0

    Public Shared Function Ejecuta(ByVal pCadena As String) As String
        If pCadena Is Nothing OrElse pCadena.Trim.Length = 0 Then Return String.Empty

        Dim TheCollection = pCadena.Split(Chr(206))
        Dim TheResponse = String.Empty
        Dim TheService As New PtoVta

        Select Case UCase(TheCollection(0))
            Case "PLACEBET"
                TheResponse = Encadena(TheService.PlaceBet(Desencadena(Of MAR_Session)(TheCollection(1)), _
                                                           Desencadena(Of MAR_Bet)(TheCollection(2)), _
                                                           CDbl(TheCollection(3)), _
                                                           CBool(TheCollection(4))))

            Case "PLACEMULTIBET"
                TheResponse = Encadena(TheService.PlaceMultiBet(Desencadena(Of MAR_Session)(TheCollection(1)), _
                                                                Desencadena(Of MAR_MultiBet)(TheCollection(2))))

            Case "PLACELOCALBET"
                TheResponse = TheService.PlaceLocalBet(Desencadena(Of MAR_Session)(TheCollection(1)), _
                                                       Desencadena(Of MAR_Bet)(TheCollection(2)), _
                                                       CDbl(TheCollection(3)))

        End Select

        Return TheResponse
    End Function

    Public Shared Function Encadena(ByVal pObjeto As Object) As String
        If pObjeto Is Nothing Then Return String.Empty
        Dim sb As New StringBuilder

        Select Case pObjeto.GetType.ToString
            Case (New MAR_Session).GetType.ToString
                Dim theObj = CType(pObjeto, MAR_Session)
                sb.Append(theObj.Banca).Append(Chr(197))
                sb.Append(theObj.Err).Append(Chr(197))
                sb.Append(theObj.LastPin).Append(Chr(197))
                sb.Append(theObj.Sesion).Append(Chr(197))
                sb.Append(theObj.Usuario)

            Case (New MAR_Bet).GetType.ToString
                Dim theObj = CType(pObjeto, MAR_Bet)
                sb.Append(theObj.Cedula).Append(Chr(215))
                sb.Append(theObj.Cliente).Append(Chr(215))
                sb.Append(theObj.Costo).Append(Chr(215))
                sb.Append(theObj.Err).Append(Chr(215))
                sb.Append(theObj.Grupo).Append(Chr(215))
                If theObj.Items IsNot Nothing Then
                    For n As Integer = 0 To theObj.Items.Count - 1
                        sb.Append(Encadena(theObj.Items(n)))
                        If n < theObj.Items.Count - 1 Then sb.Append(Chr(216)) 'Separa cada BetItem
                    Next
                End If
                sb.Append(Chr(215))
                sb.Append(theObj.Loteria).Append(Chr(215))
                sb.Append(theObj.Nulo).Append(Chr(215))
                sb.Append(theObj.Pago).Append(Chr(215))
                sb.Append(theObj.Solicitud).Append(Chr(215))
                sb.Append(theObj.StrFecha).Append(Chr(215))
                sb.Append(theObj.StrHora).Append(Chr(215))
                sb.Append(theObj.Ticket).Append(Chr(215))
                sb.Append(theObj.TicketNo)

            Case (New MAR_MultiBet).GetType.ToString
                Dim theObj = CType(pObjeto, MAR_MultiBet)
                sb.Append(theObj.Err).Append(Chr(215))
                If theObj.Headers IsNot Nothing Then
                    For n As Integer = 0 To theObj.Headers.Count - 1
                        sb.Append(Encadena(theObj.Headers(n)))
                        If n < theObj.Headers.Count - 1 Then sb.Append(Chr(216)) 'Separa cada BetHeader
                    Next
                End If
                sb.Append(Chr(215))
                If theObj.Items IsNot Nothing Then
                    For n As Integer = 0 To theObj.Items.Count - 1
                        sb.Append(Encadena(theObj.Items(n)))
                        If n < theObj.Items.Count - 1 Then sb.Append(Chr(216)) 'Separa cada BetItem
                    Next
                End If

            Case (New MAR_BetItem).GetType.ToString
                Dim theObj = CType(pObjeto, MAR_BetItem)
                sb.Append(theObj.Cantidad).Append(Chr(197))
                sb.Append(theObj.Costo).Append(Chr(197))
                sb.Append(theObj.Loteria).Append(Chr(197))
                sb.Append(theObj.Numero).Append(Chr(197))
                sb.Append(theObj.Pago).Append(Chr(197))
                sb.Append(theObj.QP)

            Case (New MAR_BetHeader).GetType.ToString
                Dim theObj = CType(pObjeto, MAR_BetHeader)
                sb.Append(theObj.Cedula).Append(Chr(197))
                sb.Append(theObj.Cliente).Append(Chr(197))
                sb.Append(theObj.Costo).Append(Chr(197))
                sb.Append(theObj.Grupo).Append(Chr(197))
                sb.Append(theObj.Loteria).Append(Chr(197))
                sb.Append(theObj.Nulo).Append(Chr(197))
                sb.Append(theObj.Pago).Append(Chr(197))
                sb.Append(theObj.Solicitud).Append(Chr(197))
                sb.Append(theObj.StrFecha).Append(Chr(197))
                sb.Append(theObj.StrHora).Append(Chr(197))
                sb.Append(theObj.Ticket).Append(Chr(197))
                sb.Append(theObj.TicketNo)

        End Select

        Return sb.ToString

    End Function

    Public Shared Function Desencadena(Of T)(ByVal pCadena As String) As T
        If pCadena Is Nothing OrElse pCadena.Trim.Length = 0 Then Return Nothing
        Dim sb As New StringBuilder

        Dim asmbl = System.Reflection.Assembly.GetAssembly(GetType(T))
        Dim pObjeto = asmbl.CreateInstance(GetType(T).ToString)

        Select Case pObjeto.GetType.ToString
            Case (New MAR_Session).GetType.ToString
                Dim TheValues = pCadena.Split(Chr(197))
                pObjeto.Banca = CInt(TheValues(0))
                pObjeto.Err = CStr(TheValues(1))
                pObjeto.LastPin = CInt(TheValues(2))
                pObjeto.Sesion = CInt(TheValues(3))
                pObjeto.Usuario = CInt(TheValues(4))

            Case (New MAR_Bet).GetType.ToString
                Dim TheValues = pCadena.Split(Chr(215))
                pObjeto.Cedula = CStr(TheValues(0))
                pObjeto.Cliente = CStr(TheValues(1))
                pObjeto.Costo = CDbl(TheValues(2))
                pObjeto.Err = CStr(TheValues(3))
                pObjeto.Grupo = CInt(TheValues(4))
                If CStr(TheValues(5)).Trim.Length > 0 Then
                    Dim TheItems As New List(Of MAR_BetItem)
                    Dim TheValItems = CStr(TheValues(5)).Split(Chr(216))
                    For n As Integer = 0 To TheValItems.Count - 1
                        TheItems.Add(Desencadena(Of MAR_BetItem)(TheValItems(n)))
                    Next
                    pObjeto.Items = TheItems.ToArray()
                End If
                pObjeto.Loteria = CInt(TheValues(6))
                pObjeto.Nulo = CBool(TheValues(7))
                pObjeto.Pago = CDbl(TheValues(8))
                pObjeto.Solicitud = CDbl(TheValues(9))
                pObjeto.StrFecha = CStr(TheValues(10))
                pObjeto.StrHora = CStr(TheValues(11))
                pObjeto.Ticket = CInt(TheValues(12))
                pObjeto.TicketNo = CStr(TheValues(13))

            Case (New MAR_MultiBet).GetType.ToString
                Dim TheValues = pCadena.Split(Chr(215))
                pObjeto.Err = CStr(TheValues(0))
                If CStr(TheValues(1)).Trim.Length > 0 Then
                    Dim TheHeaders As New List(Of MAR_BetHeader)
                    Dim TheValHeaders = CStr(TheValues(1)).Split(Chr(216))
                    For n As Integer = 0 To TheValHeaders.Count - 1
                        TheHeaders.Add(Desencadena(Of MAR_BetHeader)(TheValHeaders(n)))
                    Next
                    pObjeto.Headers = TheHeaders.ToArray()
                End If
                If CStr(TheValues(2)).Trim.Length > 0 Then
                    Dim TheItems As New List(Of MAR_BetItem)
                    Dim TheValItems = CStr(TheValues(2)).Split(Chr(216))
                    For n As Integer = 0 To TheValItems.Count - 1
                        TheItems.Add(Desencadena(Of MAR_BetItem)(TheValItems(n)))
                    Next
                    pObjeto.Items = TheItems.ToArray()
                End If


            Case (New MAR_BetItem).GetType.ToString
                Dim TheValues = pCadena.Split(Chr(197))
                pObjeto.Cantidad = CDbl(TheValues(0))
                pObjeto.Costo = CDbl(TheValues(1))
                pObjeto.Loteria = CInt(TheValues(2))
                pObjeto.Numero = CStr(TheValues(3))
                pObjeto.Pago = CDbl(TheValues(4))
                pObjeto.QP = CStr(TheValues(5))

            Case (New MAR_BetHeader).GetType.ToString
                Dim TheValues = pCadena.Split(Chr(197))
                pObjeto.Cedula = CStr(TheValues(0))
                pObjeto.Cliente = CStr(TheValues(1))
                pObjeto.Costo = CDbl(TheValues(2))
                pObjeto.Grupo = CInt(TheValues(3))
                pObjeto.Loteria = CInt(TheValues(4))
                pObjeto.Nulo = CBool(TheValues(5))
                pObjeto.Pago = CDbl(TheValues(6))
                pObjeto.Solicitud = CDbl(TheValues(7))
                pObjeto.StrFecha = CStr(TheValues(8))
                pObjeto.StrHora = CStr(TheValues(9))
                pObjeto.Ticket = CInt(TheValues(10))
                pObjeto.TicketNo = CStr(TheValues(11))

        End Select

        Return CType(pObjeto, T)

    End Function

End Class
