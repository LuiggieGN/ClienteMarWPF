Imports System.Net.Sockets
Imports System.Net
Imports System.IO

Namespace MARRecargaLibs

    Public Class MidasCard
        '----------------------------
        'Usuario: MAT2291628
        'Clave: MG321100003662
        'IP Serv. Socket: 190.167.161.93 
        'Puerto: 3031
        '----------------------------
        Private Shared _Server = "190.167.161.93"
        Private Shared _Port = 3031
        Private Shared _SocketTimeoutSeconds = 75


        Public Shared Function Balance(ByVal pCuenta As String, ByVal pUser As String, ByVal pPwd As String) As String

            Dim TraEnvio As String

            TraEnvio = String.Format("@11#{0}#{1}%",
                                     pUser,
                                     pPwd)

            Return SocketEnvia(pCuenta, TraEnvio)

        End Function

        Public Shared Function Recharge(ByVal pCuenta As String, ByVal pUser As String, ByVal pPwd As String,
                                        ByVal pNumero As String, ByVal pMonto As Double, ByVal pBanca As Integer,
                                        ByVal pPinId As Integer, ByVal pSuplidor As Integer) As String

            Dim TheSuplidor As String
            Select Case pSuplidor
                Case 1 'Claro
                    TheSuplidor = "01"
                Case 2 'Orange
                    TheSuplidor = "30"
                Case 3 'Tricom
                    TheSuplidor = "40"
                Case 4 'Viva
                    TheSuplidor = "50"
                Case 5 'Digicel
                    TheSuplidor = "55"
                Case 6 'Mount
                    TheSuplidor = "56"
                Case 999 'Confirmar
                    TheSuplidor = "10"
                Case Else
                    Throw New Exception("Suplidor de servicio celular desconocido.")
            End Select

            Dim TraEnvio As String

            If TheSuplidor = "10" Then 'Confirmar
                TraEnvio = String.Format("@{0}#{1}#{2}#{3}#{4}#{5}#{6}#{7}%",
                                         TheSuplidor,
                                         pNumero,
                                         CInt(pMonto).ToString.PadLeft(4, "0"c),
                                         pUser,
                                         pPwd,
                                         pBanca.ToString.PadLeft(8, "0"c),
                                         pPinId.ToString.PadLeft(12, "0"c),
                                         String.Format("{0}:{1}:{2}", Now.Hour, Now.Minute.ToString.PadLeft(2, "0"c), Now.Second.ToString.PadLeft(2, "0"c)))
            Else 'Primer intento
                TraEnvio = String.Format("@{0}#{1}#{2}#{3}#{4}#{5}#{6}#confirm%",
                                         TheSuplidor,
                                         pNumero,
                                         CInt(pMonto).ToString.PadLeft(4, "0"c),
                                         pUser,
                                         pPwd,
                                         pBanca.ToString.PadLeft(8, "0"c),
                                         pPinId.ToString.PadLeft(12, "0"c))
            End If

            Return SocketEnvia(pCuenta, TraEnvio)
        End Function

        Private Shared Function SocketEnvia(ByVal pCuenta As String, ByVal pEnvia As String) As String
            Dim pTimeoutMillisecs As Integer = _SocketTimeoutSeconds * 1000
            Dim pCueValues = pCuenta.Split("|")
            Dim pServer = _Server
            Dim pPort = _Port
            If pCueValues.Count > 2 Then
                pServer = pCueValues(1)
                pPort = CInt(pCueValues(2))
            End If

            Dim TheConnect As Socket, handshake(200) As Char, leyo As Integer
            Try
                Dim TheConnBuilder As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                TheConnBuilder.Blocking = True

                Dim TheIP As IPAddress = Nothing
                If pServer.ToString.Split("."c).Count = 4 Then
                    TheIP = IPAddress.Parse(pServer)
                End If
                If TheIP Is Nothing Then
                    Dim ResolvedServerIPs As IPHostEntry = Dns.GetHostEntry(pServer)
                    TheIP = ResolvedServerIPs.AddressList.FirstOrDefault
                End If

                TheConnBuilder.Connect(New IPEndPoint(TheIP, pPort))
                TheConnBuilder.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, (pTimeoutMillisecs))
                TheConnBuilder.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, (pTimeoutMillisecs))
                TheConnBuilder.SendTimeout = (pTimeoutMillisecs)
                TheConnBuilder.ReceiveTimeout = (pTimeoutMillisecs)
                TheConnect = TheConnBuilder
            Catch ex As Exception
                Throw New Exception("El servidor de la banca no pudo conectarse al servidor del suplidor de tarjetas.", ex)
            End Try
            Dim Stream As NetworkStream = New NetworkStream(TheConnect)
            Dim reader As StreamReader = New StreamReader(Stream)
            Dim writer As StreamWriter = New StreamWriter(Stream)
            'Conectado


            writer.WriteLine(pEnvia)
            writer.Flush()

            SocketEnvia = reader.ReadLine()
            leyo = SocketEnvia.Length
            If leyo <= 0 Then
                Throw New Exception("Suplidor de tarjeta no responde")
            End If

            TheConnect.Close()

        End Function

        Public Shared Function Reversar(ByVal pUser As String, ByVal pPWD As String, ByVal pBanca As Integer, ByVal pPin As Integer, ByVal pCuenta As String) As String

            Dim TraEnvio = String.Format("@{0}#{1}#{2}#{3}#{4}%",
                             "99",
                             pUser,
                             pPWD,
                             pBanca.ToString.PadLeft(8, "0"c),
                             pPin.ToString.PadLeft(12, "0"c))

            Return SocketEnvia(pCuenta, TraEnvio)

        End Function

    End Class

    Public Class MAXRecargas

        Public Shared Function Recharge(ByVal pUser As String, ByVal pPwd As String,
                                         ByVal pNumero As String, ByVal pMonto As Double, ByVal pBanca As Integer,
                                         ByVal pPinId As Integer, ByVal pSuplidor As Integer) As String

            Select Case pSuplidor
                Case 1 'Claro
                    Recharge = RecargasLibrary.Recargas.RechargeClaroPhone(pUser, pPwd, pNumero, pMonto, pBanca, pPinId)
                Case 2 'Orange
                    Recharge = RecargasLibrary.Recargas.RechargeOrangePhone(pUser, pPwd, pNumero, pMonto, pBanca, pPinId)
                Case 3 'Tricom
                    Recharge = RecargasLibrary.Recargas.RechargeTricomPhone(pUser, pPwd, pNumero, pMonto, pBanca, pPinId)
                Case 4 'Viva
                    Recharge = RecargasLibrary.Recargas.RechargeVivaPhone(pUser, pPwd, pNumero, pMonto, pBanca, pPinId)
                Case 5 'Digicel
                    Recharge = RecargasLibrary.Recargas.RechargeDigicelPhone(pUser, pPwd, pNumero, pMonto, pBanca, pPinId)
                Case 6 'Moun
                    Recharge = RecargasLibrary.Recargas.RechargeMounPhone(pUser, pPwd, pNumero, pMonto, pBanca, pPinId)
                Case Else
                    Throw New Exception("Suplidor de servicio celular desconocido.")
            End Select

        End Function

        Public Shared Function Reversar(ByVal pUser As String, ByVal pPWD As String, ByVal pBanca As Integer, ByVal pPin As Integer, ByVal pSuplidor As Integer) As String

            Select Case pSuplidor
                Case 1 'Claro
                    Reversar = RecargasLibrary.Recargas.ReverseClaroRecharge(pUser, pPWD, pBanca, pPin)
                Case 2 'Orange
                    Reversar = RecargasLibrary.Recargas.ReverseOrangeRecharge(pUser, pPWD, pBanca, pPin)
                Case 3 'Tricom
                    Reversar = RecargasLibrary.Recargas.ReverseTricomRecharge(pUser, pPWD, pBanca, pPin)
                Case 4 'Viva
                    Return "Las recargas de VIVA no se pueden anular."
                Case 5 'Digicel
                    Return "Las recargas DIGICEL no se pueden anular"
                Case 6 'Moun
                    Return "Las recargas MOUN no se pueden anular"
                Case Else
                    Return "Suplidor de servicio celular desconocido."
            End Select

        End Function

    End Class

    Public Class UnionTelecard

        Public Shared Function Balance(ByVal pCuenta As String) As String
            Return "0" '@@@ Este codigo hay que removerlo

            '@@@
            '@@@ El codigo de abajo hay que descomentarlo 


            'Dim ws = New WSUnionUtil.ServiceSoapClient
            'Dim TheBalance = ws.GetCurrentBalance(pCuenta)
            'Return String.Format(" # # # # # #Balance disponible para recargas: {0:C2}", TheBalance)

        End Function

        Public Shared Function Recharge(ByVal pCuenta As String, ByVal pUser As String,
                                        ByVal pNumero As String, ByVal pMonto As Double,
                                        ByVal pPinId As Integer, ByVal pSuplidor As Integer) As List(Of String)


            Dim TheSuplidor As String, Result As New List(Of String)

            '@@@
            '@@@ El codigo de abajo hay que descomentarlo 

            'Select Case pSuplidor
            '    Case 1, 1001 'Claro
            '        TheSuplidor = "0001"
            '        If pSuplidor < 1000 Then
            '            Dim ws = New WSUnionClaro.ServiceSoapClient
            '            Dim resp = ws.Execute_Recharge_Service(pCuenta,
            '                                                    (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                    pUser, TheSuplidor, pNumero, CInt(pMonto).ToString)
            '            Result.Add(resp.Item(0))
            '            Result.Add(resp.Item(1))
            '            Result.Add(resp.Item(2))
            '        End If

            '    Case 2, 1002 'Orange
            '        TheSuplidor = "0002"
            '        If pSuplidor < 1000 Then
            '            Dim ws = New WSUnionOrange.ServiceSoapClient
            '            Dim resp = ws.Execute_Recharge_Service(pCuenta,
            '                                                    (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                    pUser, TheSuplidor, pNumero, CInt(pMonto).ToString)
            '            Result.Add(resp.Item(0))
            '            Result.Add(resp.Item(1))
            '            Result.Add(resp.Item(2))
            '        End If
            '    Case 3, 1003 'Tricom
            '        TheSuplidor = "0003"
            '        If pSuplidor < 1000 Then
            '            Dim ws = New WSUnionTricom.ServiceSoapClient
            '            Dim resp = ws.Execute_Recharge_Service(pCuenta,
            '                                                    (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                    pUser, TheSuplidor, pNumero, CInt(pMonto).ToString)
            '            Result.Add(resp.Item(0))
            '            Result.Add(resp.Item(1))
            '            Result.Add(resp.Item(2))
            '        End If
            '    Case 4, 1004 'Viva
            '        TheSuplidor = "0004"
            '        If pSuplidor < 1000 Then
            '            Dim ws = New WSUnionViva.ServiceSoapClient
            '            Dim resp = ws.Execute_Recharge_Service(pCuenta,
            '                                                    (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                    pUser, TheSuplidor, pNumero, CInt(pMonto).ToString)
            '            Result.Add(resp.Item(0))
            '            Result.Add(resp.Item(1))
            '            Result.Add(resp.Item(2))
            '        End If
            '    Case 5, 1005 'Digicel
            '        TheSuplidor = "0011"
            '        If pSuplidor < 1000 Then
            '            Dim ws = New WSUnionDigicel.ServiceSoapClient
            '            Dim resp = ws.Execute_Recharge_Service(pCuenta,
            '                                                    (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                    pUser, TheSuplidor, pNumero, CInt(pMonto).ToString)
            '            Result.Add(resp.Item(0))
            '            Result.Add(resp.Item(1))
            '            Result.Add(resp.Item(2))
            '        End If
            '    Case 6, 1006 'Mount
            '        TheSuplidor = "0007"
            '        If pSuplidor < 1000 Then
            '            Dim ws = New WSUnionMoun.ServiceSoapClient
            '            Dim resp = ws.Execute_Recharge_Service(pCuenta,
            '                                                    (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                    pUser, TheSuplidor, pNumero, CInt(pMonto).ToString)
            '            Result.Add(resp.Item(0))
            '            Result.Add(resp.Item(1))
            '            Result.Add(resp.Item(2))
            '        End If

            '    Case Else
            '        Throw New Exception("Suplidor de servicio celular desconocido.")
            'End Select

            'If pSuplidor >= 1000 Then
            '    Dim ws = New WSUnionUtil.ServiceSoapClient
            '    Dim resp = ws.QuerySaleTransaction(pCuenta,
            '                                       (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                       pUser, TheSuplidor, pNumero, CInt(pMonto).ToString)
            '    Result.Add(resp.Item(0))
            '    Result.Add(resp.Item(1))
            '    Result.Add(resp.Item(2))
            'End If

            Return Result
        End Function

        Public Shared Function Reversar(ByVal pCuenta As String, ByVal pUser As String,
                                        ByVal pNumero As String, ByVal pSerial As String,
                                        ByVal pPinId As Integer, ByVal pSuplidor As Integer) As List(Of String)


            Dim TheSuplidor As String, Result As New List(Of String)

            '@@@
            '@@@ El codigo de abajo hay que descomentarlo 

            'Select Case pSuplidor
            '    Case 1 'Claro
            '        TheSuplidor = "0001"
            '        Dim ws = New WSUnionClaro.ServiceSoapClient
            '        Dim resp = ws.Execute_Void_Recharge_Service(pCuenta,
            '                                                (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                pUser, TheSuplidor, pNumero, pSerial)
            '        Result.Add(resp.Item(0))
            '        Result.Add(resp.Item(1))
            '        Result.Add(resp.Item(2))
            '    Case 2 'Orange
            '        TheSuplidor = "0002"
            '        Dim ws = New WSUnionOrange.ServiceSoapClient
            '        Dim resp = ws.Execute_Void_Recharge_Service(pCuenta,
            '                                                (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                pUser, TheSuplidor, pNumero, pSerial)
            '        Result.Add(resp.Item(0))
            '        Result.Add(resp.Item(1))
            '        Result.Add(resp.Item(2))
            '    Case 3 'Tricom
            '        TheSuplidor = "0003"
            '        Dim ws = New WSUnionTricom.ServiceSoapClient
            '        Dim resp = ws.Execute_Void_Recharge_Service(pCuenta,
            '                                                (pPinId Mod 1000000).ToString.PadLeft(6, "0"c),
            '                                                pUser, TheSuplidor, pNumero, pSerial)
            '        Result.Add(resp.Item(0))
            '        Result.Add(resp.Item(1))
            '        Result.Add(resp.Item(2))
            '    Case 4 'Viva
            '    Case 5 'Digicel
            '    Case 6 'Mount
            '    Case Else
            '        Throw New Exception("Suplidor de servicio celular desconocido.")
            'End Select

            Return Result

        End Function

    End Class


End Namespace
