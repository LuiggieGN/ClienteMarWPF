Imports System
Imports System.IO
Imports System.Text
Imports System.Threading
Imports System.Net
Imports System.Net.Sockets
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web
Imports System.Collections.Generic

<System.Web.Services.WebService(Namespace:="mar.do")> Public Class PtoVta
    Inherits System.Web.Services.WebService

#Region "Private Members and Conexion"

    Friend Shared StrSQLCon As String = ConfigReader.ReadString(MAR.Config.ConfigEnums.DBConnection2)
    Private AllowedHosts As String() = ConfigReader.ReadStringArray(MAR.Config.ConfigEnums.AllowedWebHosts)
    Private _BackupPort As Integer = CInt(ConfigReader.ReadString(MAR.Config.ConfigEnums.ServiceBackupPort))
    Private gCommandTimeOut As Integer = 210
    Private _RemoteIP As String
#End Region

#Region "Public WS Loteria"

    <WebMethod()> Public Function StrCmd(ByVal cmd As String) As String
        Try
            Return MarSerializador.Ejecuta(cmd)
        Catch ex As Exception
            Return String.Format("CMDERROR: {0}", ex.Message)
        End Try
    End Function

    <WebMethod()> Public Function LogonWinBrowser(ByVal Banca As Integer, Sesion As Integer, Llave As String) As MAR_ExistingSession
        Dim seed1 = Llave.Substring(0, 5)
        Dim seed2 = (((Integer.Parse(seed1) + Sesion) * Banca) Mod 100000).ToString().PadLeft(5, "0"c)
        If Llave <> seed1 & seed2 Then Return Nothing

        Dim result = New MAR_ExistingSession()
        Dim bancaTbl = DAL.GetTabla("SELECT * FROM MBANCAS WHERE BANCAID=@ban AND BANSESIONACTUAL=@sesion",
                            ({New Pair("ban", Banca), New Pair("sesion", Sesion)}).ToList(),
                            CommandType.Text, gCommandTimeOut)

        If bancaTbl Is Nothing OrElse bancaTbl.Rows.Count = 0 Then
            result.Err = "La sesion no es valida para esta banca."
            Return result
        End If

        If Not ValidHostName() Then
            result.Err = "Solicitud no autorizada."
            Return result
        End If

        result.SesionInfo = New MAR_Setting2 With {.Sesion = New MAR_Session}

        Try
            Dim bancaRow = bancaTbl.Rows(0)
            Dim TheExtraOptions As New List(Of String)

            Dim EsquemaID As Integer = bancaRow("EsquemaID")
            TheExtraOptions.Add(String.Format("PRNRPTS|{0}", If(bancaRow("BanPrintReportes"), 1, 0)))

            result.SesionInfo.Sesion.Banca = Banca
            result.SesionInfo.Sesion.Sesion = Sesion
            result.SesionInfo.Sesion.LastTck = 0

            Dim resultLoterias = New List(Of MAR_Loteria2)
            Dim loteriasTbl = DAL.GetTabla("Select A.LotNombre, ISNULL(A.NombreResumido,'') NombreResumido, ISNULL(A.Imagen,'') Imagen ,B.*,A.LotOculta,CASE WHEN sp.LoteriaIdDestino IS NULL THEN 0 ELSE 1 END AS EsSuper From Tloterias A INNER JOIN MPrecios B ON A.LoteriaID=B.LoteriaID" &
                                            " LEFT JOIN MPremioSuperPale sp ON A.LoteriaID=sp.LoteriaIdDestino AND sp.Activo=1" &
                                            " Where LotActivo='1' And EsquemaID=0" & EsquemaID)
            For n As Integer = 0 To loteriasTbl.Rows.Count - 1
                Dim iLoteria = New MAR_Loteria2
                Dim iLotRow = loteriasTbl.Rows(n)

                iLoteria.Numero = iLotRow("LoteriaID")
                iLoteria.Nombre = iLotRow("LotNombre")
                iLoteria.NombreResumido = iLotRow("NombreResumido")
                iLoteria.Imagen = iLotRow("Imagen")
                iLoteria.Oculta = iLotRow("LotOculta")
                iLoteria.PrecioQ = If(iLotRow("EsSuper"), 0, iLotRow("PreCostoQ"))
                iLoteria.PrecioP = iLotRow("PreCostoP")
                iLoteria.PrecioT = If(iLotRow("EsSuper"), 0, iLotRow("PreCostoT"))
                iLoteria.PagoQ1 = If(iLotRow("EsSuper"), 0, iLotRow("PrePagoQ1"))
                iLoteria.PagoQ2 = If(iLotRow("EsSuper"), 0, iLotRow("PrePagoQ2"))
                iLoteria.PagoQ3 = If(iLotRow("EsSuper"), 0, iLotRow("PrePagoQ3"))
                iLoteria.PagoP1 = iLotRow("PrePagoP1")
                iLoteria.PagoP2 = iLotRow("PrePagoP2")
                iLoteria.PagoP3 = iLotRow("PrePagoP3")
                iLoteria.PagoT1 = If(iLotRow("EsSuper"), 0, iLotRow("PrePagoT1"))
                iLoteria.PagoT2 = If(iLotRow("EsSuper"), 0, iLotRow("PrePagoT2"))
                iLoteria.PagoT3 = If(iLotRow("EsSuper"), -5, iLotRow("PrePagoT3")) 'Identifica SuperPale en el cliente (-5)

                Dim horaLoteria = DAL.GetTabla("Select DDeHoraFin-GruDelayCierre as Cierre From MDiasDefecto a INNER JOIN TGrupos b ON a.GrupoID=b.GrupoID  Where EsquemaID=0 And LoteriaID=0" & iLoteria.Numero &
                                                " Order By DDeDia")
                iLoteria.CieDom = horaLoteria.Rows(0)("Cierre")
                iLoteria.CieLun = horaLoteria.Rows(1)("Cierre")
                iLoteria.CieMar = horaLoteria.Rows(2)("Cierre")
                iLoteria.CieMie = horaLoteria.Rows(3)("Cierre")
                iLoteria.CieJue = horaLoteria.Rows(4)("Cierre")
                iLoteria.CieVie = horaLoteria.Rows(5)("Cierre")
                iLoteria.CieSab = horaLoteria.Rows(6)("Cierre")
                resultLoterias.Add(iLoteria)
            Next

            result.SesionInfo.Loterias = resultLoterias.ToArray()
            result.SesionInfo.MoreOptions = TheExtraOptions.ToArray()

            Dim posTbl = DAL.GetTabla("Select B.RifNombre, A.GruClientFooter, A.GruPrintHeader, A.GruPrintFooter, C.BanNombre, C.BanDireccion, C.BanTelefono, C.BanMaxQuinielaLoc, C.BanMaxPaleLoc, C.BanMaxTripleLoc, " &
                                        "C.BanComisionQ, C.BanComisionP, C.BanComisionT, C.BanRePrintTicketID, C.BanAnula, C.BanVLocal,C.BanTarjeta,0 AS BanGanadores, c.BanRemoteCMD, c.BanMaxSupeLoc, a.GruDosLoterias, c.BanPrintRecarga, B.RiferoID From TGrupos A INNER JOIN MRiferos B ON A.GrupoID=B.GrupoID INNER JOIN MBancas C ON B.RiferoID=C.RiferoID Where C.BancaID=0" & Banca)
            'BanGanadores siempre FALSE en el cliente para activar panel de Pago de Tickets
            Dim resultText As New List(Of String)
            If posTbl.Rows.Count > 0 Then 'AndAlso dr("BanNombre") <> "" Then
                resultText.Add(FormatFecha(Today, 1))
                For i = 1 To posTbl.Columns.Count
                    resultText.Add(posTbl.Rows(0)(i - 1))
                Next
                For i = (posTbl.Columns.Count + 1) To 24
                    resultText.Add("")
                Next
                resultText.Add("") 'Ejecucion de comando en el cliente: FTP|Put Archivo ; CALL|Archivo ; REN|Nombres Archivo ; COPY|Parametros
                resultText.Add(FormatFecha(Now, 7))
            End If
            result.POSInfo = New MAR_Array With {.Text = resultText.ToArray(), .Llave = (New List(Of String)).ToArray()}
        Catch ex As Exception
            result.Err = ex.ToString()
        End Try
        Return result
    End Function

    <WebMethod()> Public Function Logon2(ByVal Usuario As String, ByVal Clave As String, ByVal Banca As Integer, ByVal Direccion As String) As MAR_Setting2
        Logon2 = New MAR_Setting2
        If Banca = -1 Then
            Dim AdminSesion As New MAR_Session
            AdminSesion.Banca = Banca
            AdminSesion.Sesion = Direccion
            If ValidSesion(AdminSesion) Then
                Using cn As New SqlConnection(StrSQLCon)
                    cn.Open()
                    Dim cmd As SqlCommand = New SqlCommand("Select * From MUsuarios Where UsuUserName=@usu" &
                                                            " AND UsuClave=@clave" &
                                                            " AND UsuActivo=1 and UsuNivel>=100", cn)
                    cmd.Parameters.AddWithValue("usu", Usuario.Trim)
                    cmd.Parameters.AddWithValue("clave", Clave.Trim)
                    cmd.CommandTimeout = gCommandTimeOut
                    Dim dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read Then 'Usuario y Clave son validos
                        Logon2.Sesion.Err = String.Empty
                    Else
                        Logon2.Sesion.Err = "El usuario o password especificados son invalidos. Entre usuario y clave de administrador central."
                    End If
                End Using
            Else
                Logon2.Sesion.Err = "Terminal mal instalada..."
            End If
            Exit Function
        End If

        If Not ValidHostName() Then
            Logon2.Sesion.Err = "Solicitud no autorizada."
            Exit Function
        End If

        Dim TheExtraOptions As New List(Of String)
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Banca, "Entrada de Clave", "Inicio de Intento, User:" & Usuario)
            Dim cmd As SqlCommand = New SqlCommand("Select * From MUsuarios Where UsuUserName=@usu" &
                                                    " AND UsuClave=@clave" &
                                                    " AND UsuActivo=1", cn)
            cmd.Parameters.AddWithValue("usu", Usuario.Trim)
            cmd.Parameters.AddWithValue("clave", Clave.Trim)
            cmd.CommandTimeout = gCommandTimeOut

            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read Then 'Usuario y Clave son validos
                If dr("UsuActivo") Then 'Usuario esta activo
                    If (Not dr("UsuVenceClave") < Today) Or True Then 'Su clave no ha vencido
                        Logon2.Sesion.Usuario = dr("UsuarioID")
                        Dim Vence As Integer = DateDiff(DateInterval.Day, Today, dr("UsuVenceClave"))
                        'If Vence <= 15 Then Logon.Sesion.Err = "ADVERTENCIA!! Su clave vencerá en " & Vence & " dia(s), debe cambiarla inmediatamente, de lo contrario no podrá utilizar el sistema."
                        dr.Close()
                        cmd.Parameters.Clear()
                        cmd.CommandText = "Select A.*,B.GrupoID From MBancas A INNER JOIN MRiferos B ON A.RiferoID=B.RiferoID " &
                            "INNER JOIN RRiferosUsuarios C ON B.RiferoID=C.RiferoID Where C.UsuarioID=" & Logon2.Sesion.Usuario &
                            " AND A.BancaID=@ban AND BanDireccionIP=@direccion"
                        cmd.Parameters.AddWithValue("ban", Banca)
                        cmd.Parameters.AddWithValue("direccion", Direccion.Trim)
                        dr = cmd.ExecuteReader
                        If dr.Read Then 'La banca esta registrada y del usuario tiene permiso a usarla

                            If Usuario.ToUpper = "MAR-MOBILE" AndAlso Not dr("BanSerieTarj").ToString.ToUpper.StartsWith("MOBILE") Then
                                Logon2.Sesion.Err = "Esta banca solo tiene licencia para conectarse desde una computadora normal, no desde una portatil."
                            Else

                                Dim EsquemaID As Integer
                                EsquemaID = dr("EsquemaID")
                                If dr("BanActivo") Then 'La banca esta activa
                                    TheExtraOptions.Add(String.Format("PRNRPTS|{0}", If(dr("BanPrintReportes"), 1, 0)))

                                    Dim ActualDir As String = GetRemoteIP()
                                    If dr("BanValidIP") And Direccion <> ActualDir Then
                                        'Se exige que la conexion sea desde un IP especifico y no es así
                                        Logon2.Sesion.Err = "Esta PC no ha sido configurada apropiadamente en la red. Contacte al administrador del sistema."
                                    Else
                                        Dim GrupoID As Integer = dr("GrupoID")
                                        Logon2.Sesion.Banca = dr("BancaID")
                                        Logon2.Sesion.Sesion = DateDiff(DateInterval.Second, Today, Now)
                                        Logon2.Sesion.LastTck = 0
                                        dr.Close()
                                        cmd.Parameters.Clear()
                                        cmd.CommandText = "Select Count(*) as Total From Tloterias A INNER JOIN MPrecios B ON A.LoteriaID=B.LoteriaID Where LotActivo='1' And B.GrupoID=0" & GrupoID & " And EsquemaID=0" & EsquemaID
                                        dr = cmd.ExecuteReader
                                        Dim Total As Integer = 100
                                        If dr.Read Then
                                            Total = (dr("total") - 1)
                                        End If
                                        ReDim Logon2.Loterias(Total)
                                        cmd.CommandText = "Select A.LotNombre, ISNULL(A.NombreResumido,'') NombreResumido, ISNULL(A.Imagen,'') Imagen,B.*,A.LotOculta,CASE WHEN sp.LoteriaIdDestino IS NULL THEN 0 ELSE 1 END AS EsSuper From Tloterias A INNER JOIN MPrecios B ON A.LoteriaID=B.LoteriaID" &
                                                            " LEFT JOIN MPremioSuperPale sp ON A.LoteriaID=sp.LoteriaIdDestino AND sp.Activo=1" &
                                                            " Where LotActivo='1' And B.GrupoID=0" & GrupoID & " And EsquemaID=0" & EsquemaID
                                        dr.Close()
                                        dr = cmd.ExecuteReader
                                        Dim n As Integer = 0
                                        Do While dr.Read
                                            Logon2.Loterias(n) = New MAR_Loteria2
                                            Logon2.Loterias(n).Numero = dr("LoteriaID")
                                            Logon2.Loterias(n).Nombre = dr("LotNombre")
                                            Logon2.Loterias(n).NombreResumido = dr("NombreResumido")
                                            Logon2.Loterias(n).Imagen = dr("Imagen")
                                            Logon2.Loterias(n).Oculta = dr("LotOculta")
                                            Logon2.Loterias(n).PrecioQ = If(dr("EsSuper"), 0, dr("PreCostoQ"))
                                            Logon2.Loterias(n).PrecioP = dr("PreCostoP")
                                            Logon2.Loterias(n).PrecioT = If(dr("EsSuper"), 0, dr("PreCostoT"))
                                            Logon2.Loterias(n).PagoQ1 = If(dr("EsSuper"), 0, dr("PrePagoQ1"))
                                            Logon2.Loterias(n).PagoQ2 = If(dr("EsSuper"), 0, dr("PrePagoQ2"))
                                            Logon2.Loterias(n).PagoQ3 = If(dr("EsSuper"), 0, dr("PrePagoQ3"))
                                            Logon2.Loterias(n).PagoP1 = dr("PrePagoP1")
                                            Logon2.Loterias(n).PagoP2 = dr("PrePagoP2")
                                            Logon2.Loterias(n).PagoP3 = dr("PrePagoP3")
                                            Logon2.Loterias(n).PagoT1 = If(dr("EsSuper"), 0, dr("PrePagoT1"))
                                            Logon2.Loterias(n).PagoT2 = If(dr("EsSuper"), 0, dr("PrePagoT2"))
                                            Logon2.Loterias(n).PagoT3 = If(dr("EsSuper"), -5, dr("PrePagoT3")) 'Identifica SuperPale en el cliente (-5)
                                            n += 1
                                        Loop
                                        dr.Close()

                                        For n = 0 To Logon2.Loterias.Length - 1
                                            cmd.CommandText = "Select DDeHoraFin-GruDelayCierre as Cierre From MDiasDefecto a INNER JOIN TGrupos b ON a.GrupoID=b.GrupoID and A.GrupoID=0" & GrupoID & " Where EsquemaID=0 And LoteriaID=0" & Logon2.Loterias(n).Numero &
                                                " Order By DDeDia"
                                            dr = cmd.ExecuteReader
                                            dr.Read()
                                            Logon2.Loterias(n).CieDom = dr("Cierre")
                                            dr.Read()
                                            Logon2.Loterias(n).CieLun = dr("Cierre")
                                            dr.Read()
                                            Logon2.Loterias(n).CieMar = dr("Cierre")
                                            dr.Read()
                                            Logon2.Loterias(n).CieMie = dr("Cierre")
                                            dr.Read()
                                            Logon2.Loterias(n).CieJue = dr("Cierre")
                                            dr.Read()
                                            Logon2.Loterias(n).CieVie = dr("Cierre")
                                            dr.Read()
                                            Logon2.Loterias(n).CieSab = dr("Cierre")
                                            dr.Close()
                                        Next

                                        Dim banConfig = DAL.GetTabla("SELECT ConfigKey AS cKey,ISNULL(ConfigValue,'') AS cValue FROM MBancasConfig WHERE Activo=1 AND BancaID=@ban",
                                                                 ({New Pair("ban", Logon2.Sesion.Banca)}).ToList())
                                        For Each iRow As DataRow In banConfig.Rows
                                            If iRow("cKey").ToString() = "BANCA_JUEGAMAS_FECHA_REGISTRADA_EN_MARLTON" Then
                                                If IsDate(iRow("cValue").ToString()) AndAlso CDate(iRow("cValue").ToString()) >= Today Then TheExtraOptions.Add("BANCA_JUEGAMAS_FECHA_REGISTRADA_EN_MARLTON|1")
                                            Else
                                                TheExtraOptions.Add(String.Format("{0}|{1}", iRow("cKey").ToString(), iRow("cValue").ToString()))
                                            End If
                                        Next

                                        cmd.CommandText = "Update MBancas SET BanUsuarioActual=" & Logon2.Sesion.Usuario &
                                            ", BanDireccionActual='" & ActualDir & "', BanSesionActual=" & Logon2.Sesion.Sesion &
                                            " Where BancaID=" & Logon2.Sesion.Banca
                                        cmd.ExecuteNonQuery()

                                        Try
                                            Log(0, "Configura Banca " & Logon2.Sesion.Banca, GetJsonString(Logon2))
                                        Catch ex As Exception
                                            Dim a = "sa" 'nevermind
                                        End Try

                                    End If

                                Else
                                    Logon2.Sesion.Err = "Esta PC ha sido desactivada. Contacte al administrador del sistema."
                                End If
                            End If

                        Else
                            Logon2.Sesion.Err = "Usted no tiene permiso para utilizar este sistema en esta PC."
                        End If
                    Else
                        Logon2.Sesion.Err = "Su clave a expirado. Contacte al administrador del sistema."
                    End If
                Else
                    Logon2.Sesion.Err = "Su usuario ha sido desactivado. Contacte al administrador del sistema."
                End If
            Else
                Logon2.Sesion.Err = "Su nombre de usuario o su clave son incorrectos."
            End If
            dr.Close()
            If Logon2.Sesion.Err <> "" Then Log(Banca, "Entrada de Clave", "Intento fallido, User:" & Usuario & ", Error:" & Logon2.Sesion.Err)

            Logon2.MoreOptions = TheExtraOptions.ToArray


        End Using


    End Function

    <WebMethod()> Public Function Logon(ByVal Usuario As String, ByVal Clave As String, ByVal Banca As Integer, ByVal Direccion As String) As MAR_Setting
        'Para cliente version 8.1 en adelante ==> mirar Logon2()
        Logon = New MAR_Setting
        If Banca = -1 Then
            Dim AdminSesion As New MAR_Session
            AdminSesion.Banca = Banca
            AdminSesion.Sesion = Direccion
            If ValidSesion(AdminSesion) Then
                Using cn As New SqlConnection(StrSQLCon)
                    cn.Open()
                    Dim cmd As SqlCommand = New SqlCommand("Select * From MUsuarios Where UsuUserName=@usu" &
                                                            " AND UsuClave=@clave" &
                                                            " AND UsuActivo=1 and UsuNivel>=100", cn)
                    cmd.Parameters.AddWithValue("usu", Usuario.Trim)
                    cmd.Parameters.AddWithValue("clave", Clave.Trim)
                    cmd.CommandTimeout = gCommandTimeOut
                    Dim dr As SqlDataReader = cmd.ExecuteReader
                    If dr.Read Then 'Usuario y Clave son validos
                        Logon.Sesion.Err = String.Empty
                    Else
                        Logon.Sesion.Err = "El usuario o password especificados son invalidos. Entre usuario y clave de administrador central."
                    End If
                End Using
            Else
                Logon.Sesion.Err = "Terminal mal instalada..."
            End If
            Exit Function
        End If

        If Not ValidHostName() Then
            Logon.Sesion.Err = "Solicitud no autorizada."
            Exit Function
        End If

        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Banca, "Entrada de Clave", "Inicio de Intento, User:" & Usuario)
            Dim cmd As SqlCommand = New SqlCommand("Select * From MUsuarios Where UsuUserName=@usu" &
                                                    " AND UsuClave=@clave" &
                                                    " AND UsuActivo=1", cn)
            cmd.Parameters.AddWithValue("usu", Usuario.Trim)
            cmd.Parameters.AddWithValue("clave", Clave.Trim)
            cmd.CommandTimeout = gCommandTimeOut

            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read Then 'Usuario y Clave son validos
                If dr("UsuActivo") Then 'Usuario esta activo
                    If (Not dr("UsuVenceClave") < Today) Or True Then 'Su clave no ha vencido
                        Logon.Sesion.Usuario = dr("UsuarioID")
                        Dim Vence As Integer = DateDiff(DateInterval.Day, Today, dr("UsuVenceClave"))
                        'If Vence <= 15 Then Logon.Sesion.Err = "ADVERTENCIA!! Su clave vencerá en " & Vence & " dia(s), debe cambiarla inmediatamente, de lo contrario no podrá utilizar el sistema."
                        dr.Close()
                        cmd.Parameters.Clear()
                        cmd.CommandText = "Select A.*,B.GrupoID From MBancas A INNER JOIN MRiferos B ON A.RiferoID=B.RiferoID " &
                            "INNER JOIN RRiferosUsuarios C ON B.RiferoID=C.RiferoID Where C.UsuarioID=" & Logon.Sesion.Usuario &
                            " AND A.BancaID=@ban AND BanDireccionIP=@direccion"
                        cmd.Parameters.AddWithValue("ban", Banca)
                        cmd.Parameters.AddWithValue("direccion", Direccion.Trim)
                        dr = cmd.ExecuteReader
                        If dr.Read Then 'La banca esta registrada y del usuario tiene permiso a usarla

                            If Usuario.ToUpper = "MAR-MOBILE" AndAlso Not dr("BanSerieTarj").ToString.ToUpper.StartsWith("MOBILE") Then
                                Logon.Sesion.Err = "Esta banca solo tiene licencia para conectarse desde una computadora normal, no desde una portatil."
                            Else

                                Dim EsquemaID As Integer
                                EsquemaID = dr("EsquemaID")
                                If dr("BanActivo") Then 'La banca esta activa
                                    Dim ActualDir As String = GetRemoteIP()
                                    If dr("BanValidIP") And Direccion <> ActualDir Then
                                        'Se exige que la conexion sea desde un IP especifico y no es así
                                        Logon.Sesion.Err = "Esta PC no ha sido configurada apropiadamente en la red. Contacte al administrador del sistema."
                                    Else
                                        Dim GrupoID As Integer = dr("GrupoID")
                                        Logon.Sesion.Banca = dr("BancaID")
                                        Logon.Sesion.Sesion = DateDiff(DateInterval.Second, Today, Now)
                                        Logon.Sesion.LastTck = 0
                                        dr.Close()
                                        cmd.Parameters.Clear()
                                        cmd.CommandText = "Select Count(*) as Total From Tloterias A INNER JOIN MPrecios B ON A.LoteriaID=B.LoteriaID Where LotActivo='1' And B.GrupoID=0" & GrupoID & " And EsquemaID=0" & EsquemaID
                                        dr = cmd.ExecuteReader
                                        Dim Total As Integer = 100
                                        If dr.Read Then
                                            Total = (dr("total") - 1)
                                        End If
                                        ReDim Logon.Loterias(Total)
                                        cmd.CommandText = "Select A.LotNombre, ISNULL(A.NombreResumido,'') NombreResumido, ISNULL(A.Imagen,'') Imagen,B.*,CASE WHEN sp.LoteriaIdDestino IS NULL THEN 0 ELSE 1 END AS EsSuper From Tloterias A INNER JOIN MPrecios B ON A.LoteriaID=B.LoteriaID" &
                                                            " LEFT JOIN MPremioSuperPale sp ON A.LoteriaID=sp.LoteriaIdDestino AND sp.Activo=1" &
                                                            " Where LotActivo='1' And B.GrupoID=0" & GrupoID & " And EsquemaID=0" & EsquemaID
                                        dr.Close()
                                        dr = cmd.ExecuteReader
                                        Dim n As Integer = 0
                                        Do While dr.Read
                                            Logon.Loterias(n) = New MAR_Loteria
                                            Logon.Loterias(n).Numero = dr("LoteriaID")
                                            Logon.Loterias(n).Nombre = dr("LotNombre")
                                            Logon.Loterias(n).NombreResumido = dr("NombreResumido")
                                            Logon.Loterias(n).Imagen = dr("Imagen")
                                            Logon.Loterias(n).PrecioQ = If(dr("EsSuper"), 0, dr("PreCostoQ"))
                                            Logon.Loterias(n).PrecioP = dr("PreCostoP")
                                            Logon.Loterias(n).PrecioT = If(dr("EsSuper"), 0, dr("PreCostoT"))
                                            Logon.Loterias(n).PagoQ1 = If(dr("EsSuper"), 0, dr("PrePagoQ1"))
                                            Logon.Loterias(n).PagoQ2 = If(dr("EsSuper"), 0, dr("PrePagoQ2"))
                                            Logon.Loterias(n).PagoQ3 = If(dr("EsSuper"), 0, dr("PrePagoQ3"))
                                            Logon.Loterias(n).PagoP1 = dr("PrePagoP1")
                                            Logon.Loterias(n).PagoP2 = dr("PrePagoP2")
                                            Logon.Loterias(n).PagoP3 = dr("PrePagoP3")
                                            Logon.Loterias(n).PagoT1 = If(dr("EsSuper"), 0, dr("PrePagoT1"))
                                            Logon.Loterias(n).PagoT2 = If(dr("EsSuper"), 0, dr("PrePagoT2"))
                                            Logon.Loterias(n).PagoT3 = If(dr("EsSuper"), -5, dr("PrePagoT3")) 'Identifica SuperPale en el cliente (-5)
                                            n += 1
                                        Loop
                                        dr.Close()

                                        For n = 0 To Logon.Loterias.Length - 1
                                            cmd.CommandText = "Select DDeHoraFin-GruDelayCierre as Cierre From MDiasDefecto a INNER JOIN TGrupos b ON a.GrupoID=b.GrupoID and A.GrupoID=0" & GrupoID & " Where EsquemaID=0 And LoteriaID=0" & Logon.Loterias(n).Numero &
                                                " Order By DDeDia"
                                            dr = cmd.ExecuteReader
                                            dr.Read()
                                            Logon.Loterias(n).CieDom = dr("Cierre")
                                            dr.Read()
                                            Logon.Loterias(n).CieLun = dr("Cierre")
                                            dr.Read()
                                            Logon.Loterias(n).CieMar = dr("Cierre")
                                            dr.Read()
                                            Logon.Loterias(n).CieMie = dr("Cierre")
                                            dr.Read()
                                            Logon.Loterias(n).CieJue = dr("Cierre")
                                            dr.Read()
                                            Logon.Loterias(n).CieVie = dr("Cierre")
                                            dr.Read()
                                            Logon.Loterias(n).CieSab = dr("Cierre")
                                            dr.Close()
                                        Next

                                        cmd.CommandText = "Update MBancas SET BanUsuarioActual=" & Logon.Sesion.Usuario &
                                            ", BanDireccionActual='" & ActualDir & "', BanSesionActual=" & Logon.Sesion.Sesion &
                                            " Where BancaID=" & Logon.Sesion.Banca
                                        cmd.ExecuteNonQuery()
                                    End If

                                Else
                                    Logon.Sesion.Err = "Esta PC ha sido desactivada. Contacte al administrador del sistema."
                                End If
                            End If
                        Else
                            Logon.Sesion.Err = "Usted no tiene permiso para utilizar este sistema en esta PC."
                        End If
                    Else
                        Logon.Sesion.Err = "Su clave a expirado. Contacte al administrador del sistema."
                    End If
                Else
                    Logon.Sesion.Err = "Su usuario ha sido desactivado. Contacte al administrador del sistema."
                End If
            Else
                Logon.Sesion.Err = "Su nombre de usuario o su clave son incorrectos."
            End If

            dr.Close()
            If Logon.Sesion.Err <> "" Then Log(Banca, "Entrada de Clave", "Intento fallido, User:" & Usuario & ", Error:" & Logon.Sesion.Err)

        End Using
    End Function

    <WebMethod()> Public Function Init2(ByVal Banca As Integer, ByVal Direccion As String) As String
        Try

            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                Dim cmd As SqlCommand = New SqlCommand("Select count(*) from MBancas where BanRegistra>getdate() and BancaID=@bc", cn)
                cmd.CommandTimeout = gCommandTimeOut
                cmd.Parameters.AddWithValue("@bc", Banca)
                If cmd.ExecuteScalar > 0 Then
                    cmd.CommandText = "Delete from HCertificados Where BancaID=@bc"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Update MBancas Set BanRegistra=getdate() where bancaid=@bc"
                    cmd.ExecuteNonQuery()
                    cmd.CommandText = "Insert into HCertificados (CerNumero,BancaID,CerHwKey,CerFecha) values (@cn,@bc,@hw,getdate())"
                    Dim CerNumero = ((Banca + Direccion + Today.Year + Today.Month + Today.Day + 4321 + Now.DayOfYear + Now.Second) * 111) Mod 1000000
                    cmd.Parameters.AddWithValue("@cn", CerNumero)
                    cmd.Parameters.AddWithValue("@hw", Direccion)
                    cmd.ExecuteNonQuery()
                    Init2 = "OK" & CerNumero
                    Log(Banca, "Registro Cambio PC", String.Format("Cambio registrado exitosamente: CerNumero {0} y HwKey {1}", CerNumero, Direccion))
                Else
                    Init2 = "No tiene autorizacion para registrar esta PC en el sistema. Comuniquese con la Central."
                    Log(Banca, "Registro Cambio PC", String.Format("No tiene autorizacion para registrar esta PC en el sistema. HwKey: {0}", Direccion))
                End If
            End Using
        Catch ex As Exception
            Init2 = "No se puede registrar esta PC actualmente, intentelo mas tarde.   Error:" & ex.ToString
            Log(Banca, "Registro Cambio PC", String.Format("Ocurrio un erro, HwKey: {0}, Error: {1}", Direccion, ex.ToString()))
        End Try
    End Function

    <WebMethod()> Public Function Init4(ByVal Sesion As MAR_Session, ByVal Code As String) As Boolean
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "SetVersion", Code)
            Dim cmd As SqlCommand = New SqlCommand("Update MBancas SET BanVersion=@code" &
                " Where BancaID=@Banca And BanSesionActual=@Sesion", cn)
            cmd.Parameters.AddWithValue("code", Code)
            cmd.Parameters.AddWithValue("Banca", Sesion.Banca)
            cmd.Parameters.AddWithValue("Sesion", Sesion.Sesion)
            cmd.CommandTimeout = gCommandTimeOut
            cmd.ExecuteNonQuery()
        End Using
        Return True
    End Function

    <WebMethod()> Public Function Logoff(ByVal Sesion As MAR_Session) As Boolean
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Cierre", "")
            Dim cmd As SqlCommand = New SqlCommand("Update MBancas SET BanUsuarioActual=0, " &
                "BanDireccionActual='', BanSesionActual=0" &
                " Where BancaID=" & Sesion.Banca & " And BanSesionActual=" & Sesion.Sesion, cn)
            cmd.CommandTimeout = gCommandTimeOut
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            dr.Close()

        End Using
        Return True
    End Function

    <WebMethod()> Public Function Init3(ByVal HardwareKey As String) As MAR_Setting
        Init3 = New MAR_Setting
        Init3.Sesion = New MAR_Session
        Try
            If HardwareKey <> "ADM" Then
                Init3.Sesion.Err = "*** Acceso denegado, intentelo desde otra PC! ***"
                Exit Function
            End If
            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                Dim stat As SqlCommand = New SqlCommand("Update HEstatusDias Set EDiVentaCerrada='1' Where EDiDiaCerrado='0' And EDiVentaCerrada='0' and EDiFecha<=GetDate() and EDiCierreVentaFecha<Getdate()", cn)
                stat.CommandTimeout = gCommandTimeOut
                stat.ExecuteNonQuery()
                stat.CommandText = "Select a.LoteriaID,LotNombre, ISNULL(NombreResumido,'') NombreResumido, ISNULL(A.Imagen,'') Imagen,EdiVentaCerrada From HEstatusDias b INNER JOIN TLoterias a ON a.LoteriaID=b.LoteriaID Where EDiDiaCerrado='0' And EdiFecha<Getdate()"
                Dim DrStat As SqlDataReader = stat.ExecuteReader
                Dim lots(20) As MAR_Loteria, cnt As Integer = 0
                Do While DrStat.Read And cnt < 20
                    lots(cnt) = New MAR_Loteria
                    lots(cnt).Numero = DrStat("LoteriaID")
                    lots(cnt).Nombre = DrStat("LotNombre")
                    lots(cnt).NombreResumido = DrStat("NombreResumido")
                    lots(cnt).Imagen = DrStat("Imagen")
                    lots(cnt).Nombre += " +" & IIf(DrStat("EdiVentaCerrada"), "Cerrada", "Abierta") & "+"
                    cnt += 1
                Loop
                DrStat.Close()

                If cnt = 0 Then
                    Init3.Sesion.Err = "*** Ninguna loteria pendiente de imprimir, inicie el sistema mas tarde ***"
                    Exit Function
                Else
                    ReDim Init3.Loterias(cnt - 1)
                    Dim n As Integer
                    For n = 0 To cnt - 1
                        Init3.Loterias(n) = lots(n)
                    Next
                    'Init3.Loterias = lots
                End If
            End Using
        Catch ex As Exception
            Init3.Sesion.Err = "Error en Servidor: " & ex.ToString
        End Try
    End Function

    <WebMethod()> Public Function Init(ByVal Banca As Integer, ByVal Direccion As String) As MAR_Array
        Init = New MAR_Array
        Try
            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                Dim HwKey As String = ""
                If InStr(Direccion, ";") > 0 Then
                    Dim str() As String = Direccion.Split(";")
                    Direccion = str(0)
                    HwKey = str(1)
                    If HwKey = "0" Then
                        Init.Err = "No se logro identificar su PC, solicite soporte tecnico."
                        Log(Banca, "Fallo Conexion", Init.Err)
                        Exit Function
                    End If
                End If
                Dim cmd As SqlCommand = New SqlCommand("Select B.RifNombre, A.GruClientFooter, A.GruPrintHeader, A.GruPrintFooter, C.BanNombre, C.BanDireccion, C.BanTelefono, C.BanMaxQuinielaLoc, C.BanMaxPaleLoc, C.BanMaxTripleLoc, " &
                    "C.BanComisionQ, C.BanComisionP, C.BanComisionT, C.BanRePrintTicketID, C.BanAnula, C.BanVLocal,C.BanTarjeta,0 AS BanGanadores, c.BanRemoteCMD, c.BanMaxSupeLoc, a.GruDosLoterias, c.BanPrintRecarga, B.RiferoID From TGrupos A INNER JOIN MRiferos B ON A.GrupoID=B.GrupoID INNER JOIN MBancas C ON B.RiferoID=C.RiferoID Where C.BancaID=0" & Banca & " And BanDireccionIP='" & Direccion & "'", cn)
                'BanGanadores siempre FALSE en el cliente para activar panel de Pago de Tickets
                cmd.CommandTimeout = gCommandTimeOut
                Dim dr As SqlDataReader = cmd.ExecuteReader()

                If dr.Read Then 'AndAlso dr("BanNombre") <> "" Then
                    Dim i As Integer
                    ReDim Init.Text(26)
                    Init.Text(0) = New String("")
                    Init.Text(0) = FormatFecha(Today, 1)
                    For i = 1 To dr.FieldCount
                        Init.Text(i) = New String("")
                        Init.Text(i) = dr(i - 1)
                    Next
                    For i = (dr.FieldCount + 1) To 24
                        Init.Text(i) = New String("")
                        Init.Text(i) = ""
                    Next
                    i = 25
                    Init.Text(i - 1) = New String("")
                    Init.Text(i - 1) = "" 'Ejecucion de comando en el cliente: FTP|Put Archivo ; CALL|Archivo ; REN|Nombres Archivo ; COPY|Parametros
                    Init.Text(i) = New String("")
                    Init.Text(i) = FormatFecha(Now, 7)
                    dr.Close()
                    cmd.CommandText = "Update MBancas Set BanFirstContact=getdate() Where BancaID=0" & Banca & " and BanFirstContact < GetDate()"
                    cmd.ExecuteNonQuery()
                    Log(Banca, "Conexion", "OK")
                Else
                    dr.Close()
                    Init.Err = "Terminal mal instalada. Solicite soporte técnico."
                    ''**//
                    'cmd.CommandText = String.Format("UPDATE MBANCAS SET BanDireccionIP='{0}' WHERE BANCAID={1} AND BANNOMBRE=''", Direccion, Banca)
                    'cmd.ExecuteNonQuery()
                    'Init.Err = String.Format("POR FAVOR ANOTE: Banca id #{0} - LLAME A LA CENTRAL 809-236-8541, 829-421-2280, 829-421.2279 o 809-224-2202. Recuerde decir que usted es la banca id #{0}.", Banca)
                    ''**//
                    Log(Banca, "Fallo Conexion, IP: " & Direccion, Init.Err)
                End If

                ReDim Init.Llave(0)
                cmd.CommandText = "Select CerNumero from HCertificados where BancaID=0" & Banca & " and CerHwKey=0" & HwKey
                dr = cmd.ExecuteReader
                If dr.Read Then
                    Init.Llave(0) = dr("CerNumero")
                Else
                    dr.Close()
                    cmd.CommandText = "Select CerNumero from HCertificados where BancaID=0" & Banca
                    dr = cmd.ExecuteReader
                    If dr.Read And HwKey = "" Then
                        Init.Err = "Esta banca ya fue actualizada a la version mas reciente, no puede volver a utilizar la version anterior."
                        Init.Llave(0) = ""
                    Else
                        Init.Llave(0) = ""
                    End If
                End If
                dr.Close()

            End Using
        Catch ex As Exception
            Init.Err = ex.ToString
        End Try
    End Function

    Public Function PlaceBetLoader(ByVal Sesion As MAR_Session, ByVal Apuesta As MAR_Bet, ByVal Solicitud As Double, ByVal ParaPasar As Boolean) As MAR_Bet
        If Not ParaPasar OrElse Sesion.Err <> "NewDataLoader" Then
            Return New MAR_Bet
        End If

        'Select Case Sesion.Banca
        '    Case 250, 251, 252, 277, 257, 50, 71, 245, 19, 52, 155, 64, 87
        '        'OK: OLE, INDEPEND, SOTO, FREDDY, AUGUSTO, AGUSTIN, V.CONZUELO, ZEUS
        '    Case Else
        '        Return New MAR_Bet
        'End Select

        If Now > CType(FormatFecha(Today, 1) & IIf(Today.DayOfWeek <> DayOfWeek.Sunday, " 5:30pm", " 3:30pm"), DateTime) Then
            PlaceBetLoader = New MAR_Bet
            PlaceBetLoader.Err = "Too late for this, try later."
            Log(Sesion.Banca, "DataLoader", "Fallo, fuera de horario. Para fecha: " & Apuesta.StrFecha)
            Return PlaceBetLoader
        End If

        Dim CronoInit As DateTime = Now()
        PlaceBetLoader = New MAR_Bet

        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If Sesion.Banca > 0 And Apuesta.StrFecha <> "" Then
                If Apuesta.Items.Length = 0 Then
                    PlaceBetLoader.Err = "Su jugada no tiene ningún número."

                    Log(Sesion.Banca, "DataLoader", "Fallo, Sol:" & Apuesta.Solicitud & ", Err:" & PlaceBetLoader.Err)
                    Exit Function
                End If

                PlaceBetLoader = Apuesta
                PlaceBetLoader.Solicitud = Solicitud
                Dim bitm As MAR_BetItem
                Apuesta.Costo = 0
                For Each bitm In Apuesta.Items
                    Apuesta.Costo += bitm.Costo
                Next

                '****** Confirma Duplicidad ******
                Dim dr2 As SqlDataReader, cmd2 As SqlCommand
                cmd2 = New SqlCommand("Select TicNumero From HTickets Where BancaID=0" & Sesion.Banca & " and TicSolicitud=0" & Solicitud, cn)
                cmd2.CommandTimeout = gCommandTimeOut
                dr2 = cmd2.ExecuteReader
                If dr2.Read Then 'Ya existe el ticket. Buscalo.
                    PlaceBetLoader.Err = "Ya existe el ticket. No duplicado."
                    dr2.Close()

                    Log(Sesion.Banca, "DataLoader", "Fallo, Sol:" & Apuesta.Solicitud & ", Err:" & PlaceBetLoader.Err)
                    Exit Function
                End If
                dr2.Close()
                '************************************

                Dim TrscTicket As SqlTransaction
                Try
                    Dim cmd As SqlCommand = New SqlCommand("Select A.GrupoID, A.RiferoID, B.BancaID From MRiferos A Inner Join MBancas B ON A.RiferoID=B.RiferoID And B.BancaID=0" & Sesion.Banca, cn)
                    TrscTicket = cn.BeginTransaction(IsolationLevel.ReadCommitted, "GrabaTicket")
                    cmd.CommandTimeout = gCommandTimeOut
                    cmd.Transaction = TrscTicket
                    Dim dr As SqlDataReader = cmd.ExecuteReader
                    dr.Read()
                    cmd.CommandText = "Insert into HTickets (GrupoID,RiferoID,BancaID,LoteriaID,UsuarioID,TicFecha,TicCosto,TicSolicitud,TicCedula,TicNulo,TicketID) Values (" &
                        dr("GrupoID") & "," & dr("RiferoID") & "," & Sesion.Banca & "," & Apuesta.Loteria & "," & Sesion.Usuario & ",'" & Apuesta.StrFecha & "'," & Apuesta.Costo & "," & Solicitud & "," & DateDiff(DateInterval.Second, CronoInit, Now) & ",0," & Solicitud & ")"
                    dr.Close()
                    cmd.ExecuteNonQuery()

                    Apuesta.Ticket = Solicitud

                    dr.Close()
                    Dim SegCounter As Integer = (Apuesta.Loteria & Solicitud)
                    For Each bitm In Apuesta.Items
                        If bitm.QP = "C" Then bitm.QP = "Q"
                        If bitm.QP = "F" Then bitm.QP = "P"
                        cmd.CommandText = "INSERT INTO HTicketDetalle (TicketDetalleID,TicketID,TDeQP,TDeNumero,TDeCantidad,TDeCosto) Values (" &
                            SegCounter & "," & Apuesta.Ticket & ",'" & bitm.QP & "','" & bitm.Numero & "'," & bitm.Cantidad & "," & bitm.Costo & ")"
                        cmd.ExecuteNonQuery()
                        SegCounter += 1
                    Next
                    TrscTicket.Commit()
                    'PlaceBet = Apuesta
                    cmd.CommandText = "execute dbo.CalcLoad @Fecha='" & Apuesta.StrFecha & "', @Loteria=" & Apuesta.Loteria & ", @Banca=" & Sesion.Banca & "; Update MBancas Set BanLastContact=getdate() Where BancaID=0" & Sesion.Banca
                    cmd.ExecuteNonQuery()
                    Log(Sesion.Banca, "Loader", "OK " & Apuesta.Solicitud & "=>" & SegCounter.ToString)
                    PlaceBetLoader = New MAR_Bet
                    PlaceBetLoader.Err = ""
                Catch ex As System.Exception
                    'TrscTicket.Rollback()
                    PlaceBetLoader.Err = "La apuesta no pudo ser procesada adecuadamente. Solicite soporte técnico." & Chr(13) & Chr(13) & "Mensaje: " & ex.ToString
                End Try
            Else
                PlaceBetLoader.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            End If

        End Using
        If PlaceBetLoader.Err <> "" Then Log(Sesion.Banca, "DataLoader", "Fallo, Sol:" & Apuesta.Solicitud & ", Err:" & PlaceBetLoader.Err)
    End Function

    <WebMethod()> Public Function PlaceMultiBet(ByVal Sesion As MAR_Session, ByVal MultiApuesta As MAR_MultiBet) As MAR_MultiBet
        Dim res As New MAR_MultiBet, resHeaders As New List(Of MAR_BetHeader)

        If ValidSesion(Sesion) Then
            Dim LosSupers As New List(Of Integer)

            Dim EsVersion85 As Boolean = False
            Dim FalloUnTicket As Boolean = False

            Using cn As New SqlConnection(StrSQLCon)
                Dim dr As SqlDataReader
                Dim cmd = New SqlCommand("SELECT LoteriaIdDestino FROM MPremioSuperPale WHERE Activo=1", cn)
                cmd.CommandTimeout = gCommandTimeOut
                cn.Open()
                dr = cmd.ExecuteReader
                While dr.Read
                    LosSupers.Add(dr("LoteriaIdDestino"))
                End While
                dr.Close()

                cmd.CommandText = "SELECT COUNT(*) FROM MBancas WHERE BanVersion='8.5' AND BancaID=@bca"
                cmd.Parameters.AddWithValue("bca", Sesion.Banca)
                EsVersion85 = (cmd.ExecuteScalar() > 0)

            End Using

            For Each iHead In MultiApuesta.Headers
                Dim iApuesta As New MAR_Bet With {.Cedula = iHead.Cedula,
                                                  .Cliente = iHead.Cliente,
                                                  .Costo = iHead.Costo,
                                                  .Grupo = iHead.Grupo,
                                                  .Loteria = iHead.Loteria,
                                                  .Nulo = iHead.Nulo,
                                                  .Pago = iHead.Pago,
                                                  .Solicitud = iHead.Solicitud,
                                                  .StrFecha = iHead.StrFecha,
                                                  .StrHora = iHead.StrHora,
                                                  .Ticket = iHead.Ticket,
                                                  .TicketNo = iHead.TicketNo}

                If LosSupers.Contains(iApuesta.Loteria) Then
                    iApuesta.Items = (From itm In MultiApuesta.Items Where itm.QP = "P"
                                      Select New MAR_BetItem With {.Cantidad = itm.Cantidad,
                                                                   .Costo = itm.Costo,
                                                                   .Loteria = itm.Loteria,
                                                                   .Numero = itm.Numero,
                                                                   .Pago = itm.Pago,
                                                                   .QP = itm.QP}).ToArray
                Else
                    iApuesta.Items = (From itm In MultiApuesta.Items
                                      Select New MAR_BetItem With {.Cantidad = itm.Cantidad,
                                                                   .Costo = itm.Costo,
                                                                   .Loteria = itm.Loteria,
                                                                   .Numero = itm.Numero,
                                                                   .Pago = itm.Pago,
                                                                   .QP = itm.QP}).ToArray
                End If

                Dim iApuestaRes As New MAR_Bet
                Try
                    If EsVersion85 AndAlso FalloUnTicket Then
                        Throw New Exception("Debe actualizar este sistema. Version 8.5 tiene un fallo en multiples loterias.")
                    Else
                        iApuestaRes = PlaceBet(Sesion, iApuesta, iApuesta.Solicitud, False)
                    End If
                Catch ex As Exception
                    iApuestaRes.Err = ex.Message
                End Try

                If iApuestaRes.Err IsNot Nothing AndAlso iApuestaRes.Err <> String.Empty Then
                    iApuestaRes.TicketNo = String.Empty
                    iApuestaRes.Ticket = 0
                    FalloUnTicket = True ' <-- Para manejar bug en 8.5
                    iApuestaRes.Cedula = CRCBet(iApuesta).Cedula ' <-- Para manejar bug en 8.5.1
                End If

                resHeaders.Add(New MAR_BetHeader With {.Cedula = iApuestaRes.Cedula,
                                                       .Cliente = iApuestaRes.Cliente,
                                                       .Costo = iApuestaRes.Costo,
                                                       .Grupo = iApuestaRes.Grupo,
                                                       .Loteria = iApuestaRes.Loteria,
                                                       .Nulo = iApuestaRes.Nulo,
                                                       .Pago = iApuestaRes.Pago,
                                                       .Solicitud = iApuestaRes.Solicitud,
                                                       .StrFecha = iApuestaRes.StrFecha,
                                                       .StrHora = iApuestaRes.StrHora,
                                                       .Ticket = iApuestaRes.Ticket,
                                                       .TicketNo = iApuestaRes.TicketNo})

            Next

            If EsVersion85 AndAlso FalloUnTicket Then
                For Each iHeader In resHeaders
                    iHeader.Ticket = 0
                    iHeader.TicketNo = String.Empty
                Next
            End If

        Else
            res.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
        End If

        res.Headers = resHeaders.ToArray
        Return res

    End Function

    <WebMethod()> Public Function PlaceBet(ByVal Sesion As MAR_Session, ByVal Apuesta As MAR_Bet, ByVal Solicitud As Double, ByVal ParaPasar As Boolean) As MAR_Bet
        If ParaPasar Then
            If Sesion.Err = "NewDataLoader" Then
                PlaceBetLoader(Sesion, Apuesta, Solicitud, True)
            ElseIf Sesion.Err = "dataLoader" Then
                Return New MAR_Bet
            End If
        End If

        Dim CronoInit As DateTime = Now()
        PlaceBet = New MAR_Bet
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If ValidSesion(Sesion) Then
                Dim stat As SqlCommand = New SqlCommand("Select EstatusDiaID From HEstatusDias Where EDiDiaCerrado='0' And LoteriaID=0" & Apuesta.Loteria & " and EDiVentaCerrada='0' And EDiFecha<=GetDate() and EDiInicioVentaFecha<=Getdate() and EDiCierreVentaFecha>=Getdate() and GrupoID IN " &
                    "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & Sesion.Banca & ")", cn)
                stat.CommandTimeout = gCommandTimeOut
                Dim DrStat As SqlDataReader = stat.ExecuteReader
                If Not DrStat.Read Then
                    stat.CommandText = "Update HEstatusDias Set EDiVentaCerrada='1' Where EDiDiaCerrado='0' And EDiVentaCerrada='0' and EDiFecha<=GetDate() and EDiCierreVentaFecha<Getdate() And LoteriaID=0" & Apuesta.Loteria & " And GrupoID IN (Select GrupoID From MRiferos a INNER JOIN MBancas b ON A.RiferoID=B.RiferoID Where BancaID=" & Sesion.Banca & ")"
                    DrStat.Close()
                    If stat.ExecuteNonQuery() > 0 Then
                        'Lista de numeros **//
                    End If
                    PlaceBet.Err = "Las ventas han sido cerradas por el día de hoy para esta loteria."
                    Exit Function
                Else
                    stat.CommandText = "Update HEstatusDias Set EDiVentaIniciada='1' Where EDiDiaCerrado='0' And EDiVentaIniciada='0' and EDiFecha<=GetDate() and EDiInicioVentaFecha<=Getdate() And  EDiCierreVentaFecha>=Getdate() And LoteriaID=0" & Apuesta.Loteria & " And GrupoID IN (Select GrupoID From MRiferos a INNER JOIN MBancas b ON A.RiferoID=B.RiferoID Where BancaID=" & Sesion.Banca & ")"
                    DrStat.Close()
                    stat.ExecuteNonQuery()
                End If
                If Apuesta.Items.Length = 0 Then
                    PlaceBet.Err = "Su jugada no tiene ningún número."

                    Exit Function
                End If

                '******* Es super palet? *******
                stat.CommandText = "Select * From MPremioSuperPale Where Activo=1 AND LoteriaIdDestino=0" & Apuesta.Loteria
                DrStat = stat.ExecuteReader
                Dim EsSuperPalet = DrStat.Read
                DrStat.Close()
                '*******************************

                PlaceBet = Apuesta
                PlaceBet.Solicitud = Solicitud
                Dim bitm As MAR_BetItem, VitmRes As String
                If Not ParaPasar Then
                    For Each bitm In Apuesta.Items
                        bitm.Loteria = Apuesta.Loteria
                        If EsSuperPalet AndAlso (bitm.QP = "T" OrElse bitm.QP = "Q") Then
                            VitmRes = "Ya no hay disponibilidad para el " & bitm.Numero & " (SuperPale)"
                            bitm.Cantidad = 0
                        Else
                            VitmRes = VerifyItem(Sesion, bitm, EsSuperPalet)
                        End If
                        If VitmRes <> "OK" Then
                            PlaceBet.Err += Chr(13) + VitmRes
                        End If
                    Next
                End If

                '****** Confirma Duplicidad ******
                Dim dr2 As SqlDataReader, cmd2 As SqlCommand, OldTicNumero As String
                cmd2 = New SqlCommand("Select TicNumero From DTickets Where BancaID=0" & Sesion.Banca & " and TicSolicitud=0" & Solicitud, cn)
                cmd2.CommandTimeout = gCommandTimeOut
                dr2 = cmd2.ExecuteReader
                If dr2.Read Then 'Ya existe el ticket. Buscalo.
                    OldTicNumero = dr2("TicNumero")
                    dr2.Close()

                    PlaceBet = CRCBet(GetBet(Sesion, OldTicNumero))
                    Exit Function
                End If
                dr2.Close()
                '************************************

                If PlaceBet.Err <> "" Then
                    PlaceBet.Err = "Debe corregir las siguientes jugadas: " + PlaceBet.Err
                    Log(Sesion.Banca, "Fallo Ticket", "Sol:" & Apuesta.Solicitud & ", Err:" & PlaceBet.Err)

                    PlaceBet = CRCBet(PlaceBet)
                    Exit Function
                End If

                Dim TrscTicket As SqlTransaction
                Try
                    Dim cmd As SqlCommand = New SqlCommand("Select A.GrupoID, A.RiferoID, B.BancaID From MRiferos A Inner Join MBancas B ON A.RiferoID=B.RiferoID And B.BancaID=0" & Sesion.Banca, cn)
                    cmd.CommandTimeout = gCommandTimeOut
                    TrscTicket = cn.BeginTransaction(IsolationLevel.ReadCommitted, "GrabaTicket")
                    cmd.Transaction = TrscTicket
                    Dim dr As SqlDataReader = cmd.ExecuteReader
                    dr.Read()
                    cmd.CommandText = "Insert into DTickets (GrupoID,RiferoID,BancaID,LoteriaID,UsuarioID,TicFecha,TicCosto,TicSolicitud,TicCedula,TicNulo) Values (" &
                        dr("GrupoID") & "," & dr("RiferoID") & "," & Sesion.Banca & "," & Apuesta.Loteria & "," & Sesion.Usuario & ",GetDate()," & Apuesta.Costo & "," & Solicitud & "," & DateDiff(DateInterval.Second, CronoInit, Now) & ",1); " &
                        "SELECT SCOPE_IDENTITY()"
                    dr.Close()
                    cmd.CommandText = "Select TicketID,TicNumero,TicFecha From DTickets Where TicketID=0" & cmd.ExecuteScalar
                    dr = cmd.ExecuteReader
                    dr.Read()
                    Apuesta.StrFecha = FormatFecha(dr("TicFecha"), 1)
                    Apuesta.StrHora = FormatFecha(dr("TicFecha"), 7)
                    Apuesta.Ticket = dr("ticketid")
                    Apuesta.TicketNo = dr("ticnumero")
                    Apuesta.Nulo = False
                    dr.Close()
                    Dim SegCounter As Double = 0
                    For Each bitm In Apuesta.Items
                        If bitm.QP = "C" Then bitm.QP = "Q"
                        If bitm.QP = "F" Then bitm.QP = "P"
                        cmd.CommandText = "INSERT INTO DTicketDetalle (TicketID,TDeQP,TDeNumero,TDeCantidad,TDeCosto) Values (" &
                            Apuesta.Ticket & ",'" & bitm.QP & "','" & bitm.Numero & "'," & bitm.Cantidad & "," & bitm.Costo & ")"
                        cmd.ExecuteNonQuery()
                        SegCounter += (Val(bitm.Numero) * bitm.Cantidad)
                    Next
                    TrscTicket.Commit()
                    Apuesta.Cliente = BuildTicketFooter(Sesion, Apuesta)
                    'PlaceBet = Apuesta
                    cmd.CommandText = "Update MBancas Set BanLastContact=getdate() Where BancaID=0" & Sesion.Banca
                    cmd.ExecuteNonQuery()
                    Log(Sesion.Banca, "Ticket", "OK " & Apuesta.TicketNo & "=>" & SegCounter.ToString)
                Catch ex As System.Exception
                    'TrscTicket.Rollback()
                    PlaceBet.Err = "La apuesta no pudo ser procesada adecuadamente. Solicite soporte técnico." & Chr(13) & Chr(13) & "Mensaje: " & ex.ToString
                End Try
            Else
                PlaceBet.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            End If

        End Using
        PlaceBet = CRCBet(PlaceBet)
        If PlaceBet.Err <> "" Then Log(Sesion.Banca, "Fallo Ticket", "Sol:" & Apuesta.Solicitud & ", Err:" & PlaceBet.Err)
    End Function

    <WebMethod()> Public Function GetProducts(ByVal Sesion As MAR_Session) As MAR_Producto()
        Dim Result() As MAR_Producto
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim cmd As SqlDataAdapter = New SqlDataAdapter("Select a.SuplidorID,SupNombre,ProductoID,ProNombre,ProPrecio,SupInstrucciones From PMSuplidores a inner join PMProductos b ON a.SuplidorID=b.SuplidorID Where SupActivo=1 and ProActivo=1 Order by SupNombre, ProPrecio", cn)
            cmd.SelectCommand.CommandTimeout = gCommandTimeOut
            Dim dt As DataTable = New DataTable, rw As DataRow, n As Integer = 0
            cmd.FillSchema(dt, SchemaType.Source)
            cmd.Fill(dt)
            ReDim Result(dt.Rows.Count - 1)
            For Each rw In dt.Rows
                Result(n) = New MAR_Producto
                Result(n).Precio = rw("ProPrecio")
                Result(n).Producto = rw("ProNombre")
                Result(n).ProID = rw("ProductoID")
                Result(n).SupID = rw("SuplidorID")
                Result(n).Suplidor = rw("SupNombre")
                Result(n).Instruccion = rw("SupInstrucciones")
                n += 1
            Next

        End Using
        Return Result
    End Function

    <WebMethod()> Public Function ValidWinner(ByVal Sesion As MAR_Session, ByVal TckNumero As String, ByVal TicPin As String, ByVal Pagar As Boolean) As MAR_ValWiner
        ValidWinner = New MAR_ValWiner
        ValidWinner.Aprobado = -1
        ValidWinner.Ticket = TckNumero
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If Sesion.Err = "ADMINVALIDASOURCECODE125487" OrElse ValidSesion(Sesion) Then

                Try
                    Dim bancaDelTicket = TckNumero.Split("-")(0)
                    If Sesion.Banca <> bancaDelTicket Then
                        Dim puedePagarRemoto = MAR.BusinessLogic.Code.ProductosConfigLogic.PuedePagarRemoto(Sesion.Banca, bancaDelTicket)
                        If Not puedePagarRemoto Then
                            ValidWinner.Mensaje = "No tiene permisos para realizar este pago remoto."
                            Log(Sesion.Banca, "PagaTicket", ValidWinner.Mensaje)
                            Exit Function
                        End If
                    End If
                Catch ex As Exception
                End Try

                Dim HD = "D"
                Dim tckID As Integer
                Dim dr As SqlDataReader, cmd As New SqlCommand("Select t.BancaID,t.TicketID,t.TicNumero,t.TicSolicitud,t.TicPago,t.TicPagado,t.PagoID,t.TicFecha,t.LoteriaID,t.TicNulo,ISNULL(ir.Retenido,0) as Retenido " &
                                                               "From Dtickets t LEFT OUTER JOIN DImpuestoRetenido ir ON ir.TicketID=t.TicketID where t.TicNumero=@tck Order by t.TicNulo", cn)
                cmd.CommandTimeout = gCommandTimeOut
                cmd.Parameters.AddWithValue("@tck", TckNumero)
                dr = cmd.ExecuteReader
                If Not dr.Read Then
                    dr.Close()
                    HD = "H"
                    cmd.CommandText = "Select t.BancaID,t.TicketID,t.TicNumero,t.TicSolicitud,t.TicPago,t.TicPagado,t.PagoID,t.TicFecha,t.LoteriaID,t.TicNulo,ISNULL(ir.Retenido,0) as Retenido " &
                                                               "From Htickets t LEFT OUTER JOIN HImpuestoRetenido ir ON ir.TicketID=t.TicketID where t.TicNumero=@tck Order by t.TicNulo"
                    dr = cmd.ExecuteReader
                    If Not dr.Read Then
                        ValidWinner.Mensaje = "El ticket numero " & TckNumero & " no existe en el sistema."
                        If Not dr.IsClosed Then dr.Close()
                        Log(Sesion.Banca, "PagaTicket", ValidWinner.Mensaje)
                        Exit Function
                    End If
                End If
                tckID = dr("TicketID")
                Dim lotID = CInt(dr("LoteriaID"))
                ValidWinner.Monto = (dr("ticpago") - dr("retenido"))
                If dr("ticnulo") Then
                    ValidWinner.Mensaje = "El ticket numero " & TckNumero & " es un ticket NULO."
                    If Not dr.IsClosed Then dr.Close()
                    Log(Sesion.Banca, "PagaTicket", ValidWinner.Mensaje)
                    Exit Function
                End If
                If Not Pagar Then
                    If Sesion.Err <> "ADMINVALIDASOURCECODE125487" Then
                        Dim invalid_PaLasDos As Boolean = True

                        If TicPin.Trim.Length >= 8 Then
                            invalid_PaLasDos = Not ComparaPinGanador(CInt(dr("ticsolicitud")), TicPin)
                        Else
                            'Version vieja
                            invalid_PaLasDos = (GeneraPinGanador(dr("ticsolicitud")) <> TicPin)
                            If invalid_PaLasDos Then invalid_PaLasDos = (CInt(GeneraPinGanador(CInt(dr("ticsolicitud")) + 1)) <> TicPin)
                            If invalid_PaLasDos Then invalid_PaLasDos = (CInt(GeneraPinGanador(CInt(dr("ticsolicitud")) - 1)) <> TicPin)
                            If invalid_PaLasDos Then invalid_PaLasDos = (CInt(GeneraPinGanador(CInt(dr("ticsolicitud")) + 2)) <> TicPin)
                            If invalid_PaLasDos Then invalid_PaLasDos = (CInt(GeneraPinGanador(CInt(dr("ticsolicitud")) - 2)) <> TicPin)
                        End If

                        If invalid_PaLasDos Then
                            ValidWinner.Mensaje = "El numero de PIN digitado NO corresponde con este ticket."
                            If Not dr.IsClosed Then dr.Close()
                            Log(Sesion.Banca, "PagaTicket", ValidWinner.Mensaje)
                            Exit Function
                        End If
                    End If

                    If dr("ticpago") <= 0 Then
                        ValidWinner.Mensaje = "El ticket numero " & TckNumero & " NO resulto ganador en el sorteo correspondiente."
                        If Not dr.IsClosed Then dr.Close()
                        Log(Sesion.Banca, "PagaTicket", ValidWinner.Mensaje)
                        Exit Function
                    End If
                End If
                If dr("ticpagado") Then
                    ValidWinner.Mensaje = "Ya este ticket fue registrado como PAGADO."
                    cmd.CommandText = "Select PagFecha,a.BancaID,a.BanContacto,PagMonto, ISNULL(u.UsuUsername,'---') as Usuario " &
                                      "From HPagos a left join MUsuarios u on u.UsuarioID=a.PagUsuario " &
                                      "Where PagoID=@pag"
                    cmd.Parameters.Clear()
                    cmd.Parameters.AddWithValue("@pag", dr("PagoID"))
                    dr.Close()
                    dr = cmd.ExecuteReader
                    If dr.Read Then
                        ValidWinner.Mensaje += "  En fecha: " & FormatFecha(dr("pagfecha"), 3) & " " & FormatFecha(dr("pagfecha"), 7) & ". Pago registrado en: " & dr("BanContacto") & " (ID: " & dr("BancaID") & ") por usuario: " & dr("Usuario") & "."
                    End If
                Else
                    If EstaCerrandoDia(lotID) Then
                        ValidWinner.Mensaje = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                        Log(Sesion.Banca, "PagaTicket", ValidWinner.Mensaje)
                        Exit Function
                    End If

                    If Not Pagar Then
                        ValidWinner.Mensaje = "El ticket numero " & TckNumero & " es ganador de $" & Format(ValidWinner.Monto, "##,###,##0")
                        ValidWinner.Aprobado = 0
                    Else
                        Dim LaBancaPagando = Sesion.Banca
                        If Sesion.Err = "ADMINVALIDASOURCECODE125487" Then
                            LaBancaPagando = dr("BancaID")
                            Sesion.Banca = 0
                        End If


                        cmd.CommandText = "UPDATE " & HD & "Tickets Set TicPagado=1 Where TicketID=0" & tckID &
                                          ";Insert into HPagos Select top 1 Getdate() as PagFecha," & LaBancaPagando & "," & If(Sesion.Banca = 0, "'Oficina Administrativa'", "BanContacto") & ",@MPago as PagMonto,'" & Sesion.Usuario &
                                          "' as PagUsuario,'" & dr("TicNumero") & "' as TicNumero,'" & FormatFecha(dr("TicFecha"), 1) & "' as EdiFecha, 0" &
                                          dr("LoteriaID") & " as LoteriaID From MBancas where BancaID=0" & LaBancaPagando

                        cmd.Parameters.Clear()
                        cmd.Parameters.AddWithValue("@MPago", ValidWinner.Monto)
                        dr.Close()
                        'cmd.ExecuteNonQuery()
                        cmd.CommandText &= "; SELECT SCOPE_IDENTITY()"
                        ValidWinner.Aprobado = cmd.ExecuteScalar
                        ValidWinner.Mensaje = "El pago de $" & Format(ValidWinner.Monto, "##,###,##0") & " para el ticket ganador numero " & TckNumero & " ha sido aprobado."
                    End If
                End If
                If Not dr.IsClosed Then dr.Close()
                Log(Sesion.Banca, "PagaTicket", ValidWinner.Mensaje)

            Else
                ValidWinner.err = "Su sesion de trabajo no es valida. Salga del sistema y vuelva a entrar!"
            End If
        End Using
    End Function

    <WebMethod()> Public Sub ConfirmTck(ByVal Sesion As MAR_Session)
        If Not ValidSesion(Sesion, True) Then Exit Sub
        ConfirmaTicket(Sesion.LastTck, Sesion.Banca)
    End Sub

    <WebMethod()> Public Sub ConfirmMultiTck(ByVal Sesion As MAR_Session, Tcks As Integer())
        If Not ValidSesion(Sesion, True) Then Exit Sub
        If Tcks Is Nothing OrElse Tcks.Length = 0 Then Exit Sub
        For Each tck In Tcks
            ConfirmaTicket(tck, Sesion.Banca)
        Next
    End Sub

    Private Sub ConfirmaTicket(tck As Integer, banca As Integer)
        If tck = 0 Then Exit Sub
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim cmd As SqlCommand = New SqlCommand("Update DTickets Set TicNulo='0' Where TicketID=@TK and TicNulo='1' and TicCliente=''", cn)
            cmd.Parameters.AddWithValue("TK", tck)
            cmd.CommandTimeout = gCommandTimeOut
            If cmd.ExecuteNonQuery > 0 Then
                cmd.CommandText = "exec ActualizaListaDia @TK"
                cmd.ExecuteNonQuery()
                Log(banca, "Ticket", "Recepcion confirmada TckID:" & tck)
            End If
        End Using
    End Sub

    <WebMethod()> Public Function SetVentaFuera(ByVal VFuera As MAR_VentaFuera) As String
        SetVentaFuera = "OK"
        Try
            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                'If VFuera.Loteria = "N" Then VFuera.Loteria = "1" Else VFuera.Loteria = "2"
                Dim cmd As SqlCommand = New SqlCommand("Delete from HResumen Where ResVFuera='1' and BancaID=0" & VFuera.Banca & " And LoteriaID=0" & VFuera.Loteria & " And EDIFecha = '" & VFuera.FechaDia & "'", cn)
                cmd.CommandTimeout = gCommandTimeOut
                Dim trs As SqlTransaction
                trs = cn.BeginTransaction(IsolationLevel.ReadCommitted, "VFuera")
                cmd.Transaction = trs
                cmd.ExecuteNonQuery()
                cmd.CommandText = "INSERT INTO HResumen SELECT top 1 d.EDiFecha, c.GrupoID, a.RiferoID, a.BancaID, d.LoteriaID, a.BanComisionQ, a.BanComisionP, a.BanComisionT, d.PremioQ1 AS Primero, d.PremioQ2 AS Segundo, " &
                    "d.PremioQ3 AS Tercero, 0" & VFuera.QVendido & " AS CVQuinielas,  " &
                    "(0" & VFuera.QVendido & " * b.PreCostoQ) AS VQuinielas, (0" & VFuera.PVendido & " / b.PreCostoP) AS CVPales,  " &
                    "0" & VFuera.PVendido & " AS VPales, (0" & VFuera.TVendido & " / b.PreCostoT) AS CVTripletas,  " &
                    "0" & VFuera.TVendido & " AS VTripletas, 0" & VFuera.SacoPrimera & " AS CPrimero, 0" & VFuera.SacoSegunda & " AS CSegundo,  " &
                    "0" & VFuera.SacoTercera & " AS CTercero, (0" & VFuera.SacoPale & "/b.PrePagoP1) AS CPales, (0" & VFuera.SacoTriple & "/b.PrePagoT1) AS CTripletas, (0" & VFuera.SacoPrimera & "*b.PrePagoQ1) AS MPrimero,  " &
                    "(0" & VFuera.SacoSegunda & "*b.PrePagoQ2) AS MSegundo, (0" & VFuera.SacoTercera & "*b.PrePagoQ3) AS MTercero, 0" & VFuera.SacoPale & " AS MPales, 0" & VFuera.SacoTriple & " AS MTripletas, 1 as ResVFuera, 0 as MPagado, 0 as CVTickets " &
                    "FROM MBancas a INNER JOIN " &
                    "MRiferos c ON a.RiferoID = c.RiferoID and BancaID='0" & VFuera.Banca & "' INNER JOIN " &
                    "dbo.HEstatusDias d ON d.LoteriaID='0" & VFuera.Loteria & "' AND d.EDiFecha = '" & VFuera.FechaDia & "' " &
                    "INNER JOIN MPrecios b ON d.LoteriaID=b.LoteriaID and d.GrupoID=b.GrupoID and a.EsquemaID=b.EsquemaID"
                SetVentaFuera = IIf(cmd.ExecuteNonQuery() > 0, "OK", "Cero records afectados!")
                trs.Commit()
            End Using
        Catch ex As Exception
            SetVentaFuera = ex.Message
        End Try
    End Function

    <WebMethod()> Public Function GetMensajeDestinos(ByVal Sesion As MAR_Session) As MAR_Bancas
        GetMensajeDestinos = New MAR_Bancas
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If Sesion.Err <> "**InternalRequest**" AndAlso Not ValidSesion(Sesion) Then
                Exit Function
            End If
            Dim cmd As SqlCommand, dr As SqlDataReader, n As Integer = 0
            cmd = New SqlCommand("Select B.BancaID,B.BanContacto,(case when (DATEDIFF(mi,B.BanAlive,GetDate())<=G.GruKeepAlive) and (B.BanDireccionActual<>'') then 1 else 0 end) as Viva From MBancas B, TGrupos G, MBancas C WHERE B.RiferoID=C.RiferoID and C.BancaID=@ban", cn)
            cmd.CommandTimeout = gCommandTimeOut
            cmd.Parameters.AddWithValue("@ban", Sesion.Banca)
            dr = cmd.ExecuteReader
            Dim LasBancas = New List(Of MAR_Banca)
            Do While dr.Read
                LasBancas.Add(New MAR_Banca)
                LasBancas(n) = New MAR_Banca
                LasBancas(n).Banca = dr("BancaID")
                LasBancas(n).BanContacto = dr("BanContacto")
                LasBancas(n).BanOnline = dr("Viva")
                n += 1
            Loop
            dr.Close()
            GetMensajeDestinos.Bca = LasBancas.ToArray()

        End Using

        ReversaPinsPendientes()
    End Function

    <WebMethod()> Public Function PlaceAdmMsj(ByVal Sesion As MAR_Session, ByVal Mensaje As MAR_Mensaje) As Integer
        PlaceAdmMsj = -1
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If Not ValidSesion(Sesion) Then
                Exit Function
            End If
            Dim cmd As SqlCommand = New SqlCommand("", cn)
            cmd.CommandTimeout = gCommandTimeOut
            cmd.Parameters.AddWithValue("@Tipo", Mensaje.Tipo)
            cmd.Parameters.AddWithValue("@Asunto", Mensaje.Asunto)
            cmd.Parameters.AddWithValue("@Origen", Mensaje.Origen)
            cmd.Parameters.AddWithValue("@Mensaje", Mensaje.Contenido)
            cmd.Parameters.AddWithValue("@Destino", Mensaje.Destino)
            PlaceAdmMsj = 0

            If Strings.Left(Mensaje.Tipo, 1) = "I" Then
                cmd.CommandText = "Insert into DMensajes Select @Tipo,@Asunto,@Mensaje,Getdate(),@Destino,'O',BancaID,@Origen,0 From MBancas Where BancaID IN (0" & Mensaje.Destino & "0)"
            Else
                cmd.CommandText = "Insert into DMensajes Select @Tipo,@Asunto,@Mensaje,Getdate(),@Destino,'O',BancaID,@Origen,0 From MBancas"
            End If
            PlaceAdmMsj = cmd.ExecuteNonQuery()
        End Using
    End Function

    <WebMethod()> Public Function PlaceMensaje(ByVal Sesion As MAR_Session, ByVal Mensaje As String) As Integer
        PlaceMensaje = -1
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If Not ValidSesion(Sesion) Then
                Exit Function
            End If
            Try
                Dim cmd As SqlCommand = New SqlCommand("SELECT BancaID,BanContacto from MBancas Where BancaID=0" & Sesion.Banca, cn)
                cmd.CommandTimeout = gCommandTimeOut
                Dim dr As SqlDataReader = cmd.ExecuteReader
                Dim orig As String = ""
                If dr.Read Then orig = dr("BanContacto")
                dr.Close()

                cmd = New SqlCommand("SELECT UsuUserName from MUsuarios Where UsuarioID=0" & Sesion.Usuario, cn)
                cmd.CommandTimeout = gCommandTimeOut
                dr = cmd.ExecuteReader
                If dr.Read Then orig = Left(String.Format("{0} ({1})", orig, dr("UsuUserName")), 50)
                dr.Close()

                cmd.CommandText = "Insert into DMensajes (MenTipo,MenAsunto,MenContenido,MenDireccion,BancaID,MenOrigen) values (" &
                                        "'Entrante','" & GetRemoteIP() &
                                        "',@Mensaje,'I',@Banca,@Origen)"
                cmd.Parameters.AddWithValue("@Banca", Sesion.Banca)
                cmd.Parameters.AddWithValue("@Mensaje", Mensaje)
                cmd.Parameters.AddWithValue("@Origen", orig)
                cmd.ExecuteNonQuery()
                PlaceMensaje = 0

                cmd.CommandText = "Select Distinct DesIPAddress From HDestinos Where Datediff(mi,DesAlive,getdate())<=(Select Max(GruKeepAlive)+1 from TGrupos)"
                cmd.Parameters.Clear()
                dr = cmd.ExecuteReader
                Do While dr.Read
                    Try
                        If Envia(New IPEndPoint(Dns.GetHostEntry(dr("DesIPAddress").ToString).AddressList(0), 7009), orig, 0, Mensaje) Then PlaceMensaje += 1
                    Catch ex As Exception
                    End Try
                Loop
            Catch ex As Exception
                Log(Sesion.Banca, "Mensaje", "No se pudo grabar el mensaje: " & ex.Message)
            End Try
        End Using
    End Function

    <WebMethod()> Public Function GetMensaje(ByVal Sesion As MAR_Session) As MAR_Mensajes
        GetMensaje = New MAR_Mensajes
        Try
            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                If Not ValidSesion(Sesion) Then
                    GetMensaje.Err = "ERROR: Su sesión no es válida. Salga del sistema y entre nuevamente."
                    Exit Function
                End If
                Dim cmd As SqlCommand

                If Sesion.Err <> "POS" Then
                    cmd = New SqlCommand("Select MAX(GruKeepAlive) from TGrupos", cn)
                    cmd.CommandTimeout = gCommandTimeOut
                    GetMensaje.NewInterval = cmd.ExecuteScalar
                    cmd.CommandText = "Update MBancas Set BanAlive=getdate() where BancaID=0" & Sesion.Banca
                    cmd.ExecuteNonQuery()
                End If

                cmd = New SqlCommand("Select * from dmensajes where MenLeido=0 and MenDireccion='O' and BancaID=0" & Sesion.Banca, cn)
                Dim rmen As SqlDataReader = cmd.ExecuteReader, mensj(100) As MAR_Mensaje, cntMsj As Integer
                Do While rmen.Read
                    mensj(cntMsj) = New MAR_Mensaje
                    mensj(cntMsj).Asunto = rmen("MenAsunto")
                    mensj(cntMsj).Contenido = rmen("MenContenido")
                    mensj(cntMsj).Origen = rmen("MenOrigen")
                    mensj(cntMsj).Asunto = rmen("MenAsunto")
                    mensj(cntMsj).Fecha = FormatFecha(rmen("MenFecha"), 1)
                    mensj(cntMsj).Hora = FormatFecha(rmen("MenFecha"), 7)
                    cntMsj += 1
                    If cntMsj >= 100 Then Exit Do
                Loop
                rmen.Close()

                If cntMsj > 0 Then
                    Dim n As Integer
                    ReDim GetMensaje.msj(cntMsj - 1)
                    For n = 0 To cntMsj - 1
                        GetMensaje.msj(n) = mensj(n)
                    Next
                    cmd.CommandText = "UPDATE Dmensajes set MenLeido=1 where MenDireccion='O' and BancaID=0" & Sesion.Banca
                    cmd.ExecuteNonQuery()
                End If

            End Using
        Catch ex As Exception
            GetMensaje.Err = "ERROR: " & ex.Message
        End Try
    End Function

    <WebMethod()> Public Function PlaceMensaje2(ByVal Sesion As MAR_Session, ByVal Mensaje As String, DestinoBancaID As Integer) As MAR_Mensajes2
        PlaceMensaje2 = New MAR_Mensajes2
        Try
            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                If Not ValidSesion(Sesion) Then
                    Exit Function
                End If

                Dim orig = String.Format("{0}.{1}", Sesion.Banca, Sesion.Usuario)
                Dim cmd = New SqlCommand("Insert into DMensajes (MenTipo,MenAsunto,MenContenido,MenDireccion,BancaID,MenOrigen) values (" &
                                        "'Entrante','" & GetRemoteIP() &
                                        "',@Mensaje,@Direccion,@Banca,@Origen)", cn)
                cmd.Parameters.AddWithValue("@Banca", If(DestinoBancaID = 0, Sesion.Banca, DestinoBancaID))
                cmd.Parameters.AddWithValue("@Mensaje", Mensaje)
                cmd.Parameters.AddWithValue("@Direccion", If(DestinoBancaID = 0, "I", "O"))
                cmd.Parameters.AddWithValue("@Origen", orig)
                cmd.ExecuteNonQuery()

                PlaceMensaje2.Enviado = True

            End Using

            Dim mens = GetMensaje2(New MAR_Session With {.Banca = Sesion.Banca, .Err = "**InternalRequest**"}, False, DestinoBancaID, False)
            If mens.Err Is Nothing OrElse mens.Err.Trim.Length = 0 Then
                PlaceMensaje2.msj = mens.msj
            End If

        Catch ex As Exception
            PlaceMensaje2.Err = "No se pudo grabar el mensaje: " & ex.Message
            PlaceMensaje2.Enviado = False
            Log(Sesion.Banca, "Mensaje", "No se pudo grabar el mensaje: " & ex.Message)
        End Try

    End Function

    <WebMethod()> Public Function GetMensaje2(ByVal Sesion As MAR_Session, SoloSinLeer As Boolean,
                                              SoloDestinoBancaId As Integer?, IncluirDestinos As Boolean) As MAR_Mensajes2
        GetMensaje2 = New MAR_Mensajes2
        Try
            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                If Sesion.Err <> "**InternalRequest**" AndAlso Not ValidSesion(Sesion) Then
                    GetMensaje2.Err = "ERROR: Su sesión no es válida. Salga del sistema y entre nuevamente."
                    Exit Function
                End If

                Dim cmd As SqlCommand
                If Sesion.Err <> "**InternalRequest**" Then
                    cmd = New SqlCommand("Select MAX(GruKeepAlive) from TGrupos", cn)
                    cmd.CommandTimeout = gCommandTimeOut
                    GetMensaje2.NewInterval = cmd.ExecuteScalar
                    cmd.CommandText = "Update MBancas Set BanAlive=getdate() where BancaID=0" & Sesion.Banca
                    cmd.ExecuteNonQuery()
                End If

                If Not SoloDestinoBancaId.HasValue Then
                    cmd = New SqlCommand("SELECT M.*,L.SinLeerTotal,L.BancaDestinoID FROM DMensajes M " &
                                        "JOIN (Select MAX(MensajeID) MaxMensajeID, SUM(CASE WHEN MenLeido=0 AND MenDireccion='O' AND BancaID=@ban THEN 1 ELSE 0 END) SinLeerTotal, " &
                                        "	  CASE WHEN MenDestino<>'' OR MenDireccion='I' OR ISNUMERIC(MenOrigen)<>1 THEN 0 WHEN FLOOR(MenOrigen)=@ban THEN BancaID ELSE FLOOR(MenOrigen) END AS BancaDestinoID " &
                                        "                    FROM DMensajes " &
                                        "		WHERE (BancaID=@ban OR (ISNUMERIC(MenOrigen)=1 AND FLOOR(MenOrigen)=@ban))  " &
                                        "GROUP BY CASE WHEN MenDestino<>'' OR MenDireccion='I' OR ISNUMERIC(MenOrigen)<>1 THEN 0 WHEN FLOOR(MenOrigen)=@ban THEN BancaID ELSE FLOOR(MenOrigen) END) L  " &
                                        "ON M.MensajeID=L.MaxMensajeID  " &
                                        "ORDER BY M.MensajeID DESC;", cn)
                Else
                    If SoloDestinoBancaId.Value = 0 Then
                        cmd = New SqlCommand("SELECT M.*,(CASE WHEN MenLeido=1 THEN 0 ELSE 1 END) SinLeerTotal, CASE WHEN MenDireccion='I' THEN BancaID Else 0 END as BancaDestinoID FROM DMensajes M " &
                                             "WHERE (MenDireccion='I' OR MenDestino<>'') AND BancaID=@ban " &
                                             "ORDER BY M.MensajeID DESC;" &
                                             "UPDATE DMensajes SET MenLeido=1 " &
                                             "WHERE MenLeido=0 AND BancaID=@ban AND MenDireccion='O' AND MenDestino<>'';", cn)
                    Else
                        cmd = New SqlCommand("SELECT M.*,(CASE WHEN MenLeido=1 THEN 0 ELSE 1 END) SinLeerTotal,CASE WHEN BancaID=@des THEN @ban Else @des END as BancaDestinoID FROM DMensajes M " &
                                             "WHERE ISNUMERIC(MenOrigen)=1 AND ((BancaID=@ban AND FLOOR(MenOrigen)=@des) OR (BancaID=@des AND FLOOR(MenOrigen)=@ban)) " &
                                             "ORDER BY M.MensajeID DESC;" &
                                             "UPDATE DMensajes SET MenLeido=1 " &
                                             "WHERE MenLeido=0 AND BancaID=@ban AND ISNUMERIC(MenOrigen)=1 AND FLOOR(MenOrigen)=@des;", cn)
                        cmd.Parameters.AddWithValue("des", SoloDestinoBancaId.Value)
                    End If
                End If
                cmd.Parameters.AddWithValue("ban", Sesion.Banca)
                cmd.CommandTimeout = gCommandTimeOut

                Dim rmen As SqlDataReader = cmd.ExecuteReader, mensj As New List(Of MAR_Mensaje2), cntMsj As Integer = 0
                Do While rmen.Read
                    mensj.Add(New MAR_Mensaje2)
                    mensj(cntMsj).BancaID = rmen("BancaDestinoID")
                    mensj(cntMsj).Destino = rmen("MenDestino")
                    mensj(cntMsj).Contenido = rmen("MenContenido")
                    mensj(cntMsj).Origen = rmen("MenOrigen")
                    mensj(cntMsj).Asunto = rmen("MenAsunto")
                    mensj(cntMsj).Fecha = FormatFecha(rmen("MenFecha"), 1)
                    mensj(cntMsj).Hora = FormatFecha(rmen("MenFecha"), 7)
                    mensj(cntMsj).Leido = CBool(rmen("MenLeido"))
                    mensj(cntMsj).SinLeerTotal = CInt(rmen("SinLeerTotal"))
                    cntMsj += 1
                    If cntMsj >= 100 Then Exit Do
                Loop
                rmen.Close()
                GetMensaje2.msj = mensj.ToArray()

            End Using

            If IncluirDestinos Then
                Dim destinos = GetMensajeDestinos(New MAR_Session With {.Banca = Sesion.Banca, .Err = "**InternalRequest**"})
                GetMensaje2.Bca = destinos.Bca
            End If

        Catch ex As Exception
            GetMensaje2.Err = "ERROR: " & ex.Message
        End Try
    End Function

    <WebMethod()> Public Function CuentaMensaje(ByVal Sesion As MAR_Session) As MAR_MensajesCount
        CuentaMensaje = New MAR_MensajesCount
        Try
            Using cn As New SqlConnection(StrSQLCon)
                cn.Open()
                If Not ValidSesion(Sesion) Then
                    CuentaMensaje.Err = "ERROR: Su sesión no es válida. Salga del sistema y entre nuevamente."
                    Exit Function
                End If
                Dim cmd As SqlCommand

                cmd = New SqlCommand("Select MAX(G.GruKeepAlive) Interval,COUNT(DISTINCT M.MensajeID) MsjCount FROM TGrupos G, DMensajes M " &
                                     "WHERE M.MenLeido=0 and M.MenDireccion='O' and M.BancaID=@ban;" &
                                     "Update MBancas Set BanAlive=getdate() where BancaID=@ban;", cn)
                cmd.CommandTimeout = gCommandTimeOut
                cmd.Parameters.AddWithValue("ban", Sesion.Banca)

                Dim rmen As SqlDataReader = cmd.ExecuteReader()
                If rmen.Read Then
                    CuentaMensaje.NewInterval = CInt(rmen("Interval"))
                    CuentaMensaje.MsjNuevos = CInt(rmen("MsjCount"))
                Else
                    CuentaMensaje.Err = "ERROR: Fallo el conteo de mensajes"
                End If
                rmen.Close()
            End Using
        Catch ex As Exception
            CuentaMensaje.Err = "ERROR: " & ex.Message
        End Try
    End Function

#End Region

#Region "Public Reporting"

    <WebMethod()> Public Function Ganadores(ByVal Sesion As MAR_Session, ByVal Loteria As Integer, ByVal Fecha As String) As MAR_Ganadores
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Listado de ganadores")
            Ganadores = New MAR_Ganadores
            If Not ValidSesion(Sesion) Then
                Ganadores.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If EstaCerrandoDia(Loteria) Then
                Ganadores.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Dim stat As SqlCommand = New SqlCommand("Select a.BancaID,a.BanGanadores from MBancas a inner join MBancas b ON Left(a.bancontacto,4)=Left(b.bancontacto,4) where b.bancaid=0" & Sesion.Banca, cn)
            stat.CommandTimeout = gCommandTimeOut

            '**//
            'If InStr(UCase(StrSQLCon), "DATA007") <= 0 And InStr(UCase(StrSQLCon), "DATA004") <= 0 Then
            stat.CommandText = "Select a.BancaID,a.BanGanadores from MBancas a where A.bancaid=0" & Sesion.Banca
            'End If

            Dim DrStat As SqlDataReader = stat.ExecuteReader, strBanca As String = String.Empty, cntBancas As Integer = 0, veGanad As Boolean = True
            Do While DrStat.Read
                strBanca += DrStat("BancaID") & ","
                If veGanad Then veGanad = DrStat("BanGanadores")
                cntBancas += 1
            Loop
            DrStat.Close()

            If Not veGanad Then
                Ganadores.Err = "Su banca no tiene autorizacion para ver el listado de tickets ganadores. Use el Reporte de Ventas para ver los numeros ganadores de el sorteo seleccionado."
                Exit Function
            End If

            If cntBancas > 0 Then
                strBanca = "(" & Left(strBanca, strBanca.Length - 1) & ")"
            Else
                strBanca = ""
            End If


            stat.CommandText = "Select * From HEstatusDias Where (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") and EDiFecha='" & Fecha & "' And EDiPremiosDentro='1' and GrupoID IN " &
                "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID in " & strBanca & ")"
            DrStat = stat.ExecuteReader
            If DrStat.Read Then
                If Loteria > 0 Then
                    Ganadores.Primero = DrStat("PremioQ1")
                    Ganadores.Segundo = DrStat("PremioQ2")
                    Ganadores.Tercero = DrStat("PremioQ3")
                End If
                Ganadores.Fecha = Fecha
                Dim DH As String, cmd As SqlCommand, dr As SqlDataReader, FechaCierre As String
                If Loteria = 0 OrElse DrStat("EDiDiaCerrado") Then
                    DH = "H"
                Else
                    DH = "D"
                End If
                FechaCierre = FormatFecha(DateAdd(DateInterval.Hour, 6, DrStat("EDiCierreVentaFecha")), 2)
                DrStat.Close()
                cmd = New SqlCommand("Select count(*) as Total From " & DH & "Tickets" &
                                        If(DH = "D", " WITH (NOLOCK)", "") &
                                        " Where TicNulo='0' And TicFecha>='" & Fecha &
                                        "' And TicFecha<='" & FechaCierre & "'" &
                                        " And BancaID in " & strBanca & " And (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") And TicPago>0", cn)
                cmd.CommandTimeout = gCommandTimeOut
                cmd.CommandTimeout = 120
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If dr("total") > 0 Then
                        ReDim Ganadores.Tickets(dr("total") - 1)
                        dr.Close()
                        cmd.CommandText = "Select t.*,ISNULL(ir.Retenido,0) as Retenido From " & DH & "Tickets t" &
                                            If(DH = "D", " WITH (NOLOCK)", "") &
                                            " Left Outer Join " & DH & "ImpuestoRetenido ir ON ir.TicketID=t.TicketID " &
                                            "Where t.TicNulo='0' And t.TicFecha>='" & Fecha & "' And t.TicFecha<='" & FechaCierre & "'" & IIf(veGanad, "", "") &
                                            " And t.BancaID in " & strBanca & " And (0=" & Loteria & " OR t.LoteriaID=0" & Loteria & ") And t.TicPago>0 Order by t.TicNumero"
                        dr = cmd.ExecuteReader
                        Dim n, i As Integer
                        For n = 0 To Ganadores.Tickets.Length - 1
                            dr.Read()
                            Ganadores.Tickets(n) = New MAR_Bet
                            Ganadores.Tickets(n).Costo = dr("TicCosto")
                            Ganadores.Tickets(n).StrFecha = FormatFecha(dr("TicFecha"), 1)
                            Ganadores.Tickets(n).StrHora = FormatFecha(dr("TicFecha"), 7)
                            Ganadores.Tickets(n).TicketNo = dr("TicNumero")
                            Ganadores.Tickets(n).Loteria = dr("LoteriaID")
                            Ganadores.Tickets(n).Pago = (dr("Retenido") * -1)
                            Ganadores.Tickets(n).Ticket = dr("TicketID")
                        Next
                        dr.Close()
                        For n = 0 To Ganadores.Tickets.Length - 1
                            cmd.CommandText = "Select Count(*) as Total From " & DH & "TicketDetalle" &
                                                If(DH = "D", " WITH (NOLOCK)", "") & " Where TDePago>0 And TicketID=0" & Ganadores.Tickets(n).Ticket
                            dr = cmd.ExecuteReader
                            If dr.Read Then
                                If dr("total") > 0 Then
                                    ReDim Ganadores.Tickets(n).Items(dr("Total") - 1)
                                    dr.Close()
                                    cmd.CommandText = "Select * From " & DH & "TicketDetalle" &
                                                        If(DH = "D", " WITH (NOLOCK)", "") & " Where TDePago>0 And TicketID=0" & Ganadores.Tickets(n).Ticket
                                    dr = cmd.ExecuteReader
                                    For i = 0 To Ganadores.Tickets(n).Items.Length - 1
                                        dr.Read()
                                        Ganadores.Tickets(n).Items(i) = New MAR_BetItem
                                        Ganadores.Tickets(n).Items(i).Cantidad = dr("TDeCantidad")
                                        Ganadores.Tickets(n).Items(i).Costo = dr("TDeCosto")
                                        Ganadores.Tickets(n).Items(i).Numero = dr("TDeNumero")
                                        Ganadores.Tickets(n).Items(i).Pago = dr("TDePago")
                                        Ganadores.Tickets(n).Items(i).QP = dr("TDeQP")
                                        Ganadores.Tickets(n).Pago += dr("TDePago")
                                    Next
                                    dr.Close()
                                End If
                            End If
                        Next
                        'Else
                        '    dr.Close()
                        '    Ganadores.Err = "No se encontraron ganadores en la fecha seleccionada."
                        '    Exit Function
                    End If
                End If
            Else
                Ganadores.Err = "Los premios todavia no han sido registrados para la fecha seleccionada."
            End If


            If (Ganadores.Err Is Nothing OrElse Ganadores.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
                Ganadores.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

        End Using
    End Function

    <WebMethod()> Public Function Ganadores2(ByVal Sesion As MAR_Session, ByVal Loteria As Integer, ByVal Fecha As String) As MAR_Ganadores
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Listado de ganadores")
            Ganadores2 = New MAR_Ganadores
            If Not ValidSesion(Sesion) Then
                Ganadores2.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (Ganadores2.Err Is Nothing OrElse Ganadores2.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
                Ganadores2.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Dim stat As SqlCommand = New SqlCommand("Select a.BancaID,a.BanGanadores from MBancas a inner join MBancas b ON Left(a.bancontacto,4)=Left(b.bancontacto,4) where b.bancaid=0" & Sesion.Banca, cn)
            stat.CommandTimeout = gCommandTimeOut

            stat.CommandText = "Select a.BancaID,a.BanGanadores from MBancas a where A.bancaid=0" & Sesion.Banca

            Dim DrStat As SqlDataReader = stat.ExecuteReader, strBanca As String = String.Empty, cntBancas As Integer = 0, veGanad As Boolean = True
            Do While DrStat.Read
                strBanca += DrStat("BancaID") & ","
                If veGanad Then veGanad = DrStat("BanGanadores")
                cntBancas += 1
            Loop
            DrStat.Close()

            'If Not veGanad Then
            '    Ganadores2.Err = "Su banca no tiene autorizacion para ver el listado de tickets ganadores. Use el Reporte de Ventas para ver los numeros ganadores de el sorteo seleccionado."
            '    Exit Function
            'End If

            If cntBancas > 0 Then
                strBanca = "(" & Left(strBanca, strBanca.Length - 1) & ")"
            Else
                strBanca = ""
            End If


            stat.CommandText = "Select * From HEstatusDias Where (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") and EDiFecha='" & Fecha & "' and EDiVentaCerrada='1' And EDiPremiosDentro='1' and GrupoID IN " &
                "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID in " & strBanca & ")"
            DrStat = stat.ExecuteReader
            If DrStat.Read Then
                If Loteria > 0 Then
                    Ganadores2.Primero = DrStat("PremioQ1")
                    Ganadores2.Segundo = DrStat("PremioQ2")
                    Ganadores2.Tercero = DrStat("PremioQ3")
                End If
                Ganadores2.Fecha = Fecha
                Dim DH As String, cmd As SqlCommand, dr As SqlDataReader, FechaCierre As String
                If Loteria = 0 OrElse DrStat("EDiDiaCerrado") Then
                    DH = "H"
                Else
                    DH = "D"
                End If
                FechaCierre = FormatFecha(DateAdd(DateInterval.Hour, 1, DrStat("EDiCierreVentaFecha")), 2)
                DrStat.Close()
                cmd = New SqlCommand("Select count(*) as Total From " & DH & "Tickets" &
                                        If(DH = "D", " WITH (NOLOCK)", "") & " Where TicNulo='0' And TicFecha>='" & Fecha &
                                        "' And TicFecha<='" & FechaCierre & "'" &
                                        " And BancaID in " & strBanca & " And (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") And TicPago>0", cn)
                cmd.CommandTimeout = gCommandTimeOut
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If dr("total") > 0 Then
                        ReDim Ganadores2.Tickets(dr("total") - 1) 'Total count para limitar For..Loop
                        Dim ListaGanadores As New List(Of MAR_Bet)
                        dr.Close()
                        cmd.CommandText = "Select t.*,ISNULL(ir.Retenido,0) as Retenido From " & DH & "Tickets t" &
                                            If(DH = "D", " WITH (NOLOCK)", "") &
                                            " Left Outer Join " & DH & "ImpuestoRetenido ir ON ir.TicketID=t.TicketID " &
                                            "Where t.TicNulo='0' And t.TicFecha>='" & Fecha & "' And t.TicFecha<='" & FechaCierre & "'" &
                                            " And t.BancaID in " & strBanca & " And (0=" & Loteria & " OR t.LoteriaID=0" & Loteria & ") And t.TicPago>0 Order by t.TicNumero"
                        dr = cmd.ExecuteReader
                        Dim n, i As Integer
                        For n = 0 To Ganadores2.Tickets.Length - 1
                            dr.Read()
                            Dim TckG = New MAR_Bet
                            TckG.Costo = dr("TicCosto")
                            TckG.StrFecha = FormatFecha(dr("TicFecha"), 1)
                            TckG.StrHora = FormatFecha(dr("TicFecha"), 7)
                            TckG.TicketNo = dr("TicNumero")
                            TckG.Loteria = dr("LoteriaID")
                            TckG.Pago = (dr("Retenido") * -1)
                            TckG.Ticket = dr("TicketID")
                            TckG.Solicitud = IIf(dr("TicPagado"), 5, IIf(veGanad, 4, -1))
                            'SolicitudID
                            ' 5: siempre mostrar como pagado
                            ' 4: mostrar sin total cuando autorizado a ver pendientes de pago
                            '-1: ocutar detalles pero incluir total
                            ListaGanadores.Add(TckG)
                        Next
                        dr.Close()
                        For n = 0 To Ganadores2.Tickets.Length - 1
                            cmd.CommandText = "Select Count(*) as Total From " & DH & "TicketDetalle" &
                                                If(DH = "D", " WITH (NOLOCK)", "") &
                                                " Where TDePago>0 And TicketID=0" & ListaGanadores(n).Ticket
                            dr = cmd.ExecuteReader
                            If dr.Read Then
                                If dr("total") > 0 Then
                                    ReDim ListaGanadores(n).Items(dr("Total") - 1)
                                    dr.Close()
                                    cmd.CommandText = "Select * From " & DH & "TicketDetalle" &
                                                        If(DH = "D", " WITH (NOLOCK)", "") &
                                                        " Where TDePago>0 And TicketID=0" & ListaGanadores(n).Ticket

                                    Dim tckGClone As MAR_Bet = Nothing 'Clone de tickets pendientes
                                    dr = cmd.ExecuteReader
                                    For i = 0 To ListaGanadores(n).Items.Length - 1
                                        dr.Read()
                                        ListaGanadores(n).Items(i) = New MAR_BetItem
                                        ListaGanadores(n).Items(i).Cantidad = dr("TDeCantidad")
                                        ListaGanadores(n).Items(i).Costo = dr("TDeCosto")
                                        ListaGanadores(n).Items(i).Numero = dr("TDeNumero")
                                        ListaGanadores(n).Items(i).Pago = dr("TDePago")
                                        ListaGanadores(n).Items(i).QP = dr("TDeQP")
                                        If ListaGanadores(n).Solicitud = 4 Then 'Puede verlo, pero mueve el monto del total al clone como pendiente de pago
                                            If tckGClone Is Nothing Then 'clone para total pendiente de pago sin detalles
                                                tckGClone = New MAR_Bet With {.Solicitud = -1,
                                                                            .Ticket = ListaGanadores(n).Ticket,
                                                                            .Costo = ListaGanadores(n).Costo,
                                                                            .StrFecha = ListaGanadores(n).StrFecha,
                                                                            .StrHora = ListaGanadores(n).StrHora,
                                                                            .TicketNo = ListaGanadores(n).TicketNo,
                                                                            .Loteria = ListaGanadores(n).Loteria,
                                                                            .Items = (New List(Of MAR_BetItem)).ToArray,
                                                                            .Pago = ListaGanadores(n).Pago}
                                                ListaGanadores(n).Pago = 0
                                            End If
                                            tckGClone.Pago += dr("TDePago") 'Acumula total en el clone

                                        Else 'Marcado como pagado o pendiente de pago y oculto
                                            ListaGanadores(n).Pago += dr("TDePago")
                                        End If
                                    Next
                                    dr.Close()
                                    If tckGClone IsNot Nothing Then ListaGanadores.Add(tckGClone) 'El clone no pasara por el For..Loop pues index excede n-1

                                End If
                            End If
                        Next

                        Ganadores2.Tickets = ListaGanadores.ToArray 'Listado final incluyendo clones

                    End If
                End If
            Else
                Ganadores2.Err = "Los premios todavia no han sido registrados para la fecha seleccionada."
            End If
        End Using

        If (Ganadores2.Err Is Nothing OrElse Ganadores2.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
            Ganadores2.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function


    <WebMethod()> Public Function Ganadores3(ByVal Sesion As MAR_Session, ByVal Loteria As Integer, ByVal Fecha As String) As MAR_Ganadores
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Listado de ganadores y sin reclamar")
            Ganadores3 = New MAR_Ganadores
            If Not ValidSesion(Sesion) Then
                Ganadores3.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (Ganadores3.Err Is Nothing OrElse Ganadores3.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
                Ganadores3.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If




            Dim bancaConfig = DAL.GetTabla("Select a.BancaID,a.BanGanadores,ISNULL(b.Modo,0) as DiasSinReclamo from MBancas a LEFT JOIN SWebProductoConfig b on b.Opcion='DIAS_RECAUDACION_DINERO_NO_RECLAMADO' AND b.Activo=1 AND b.WebProductoID=0 WHERE A.bancaid=@ban",
                                           ({New Pair("ban", Sesion.Banca)}).ToList())
            Dim veGanad As Boolean = False, diasSinReclamar As Integer = 0
            If bancaConfig.Rows.Count > 0 Then
                veGanad = bancaConfig.Rows(0)("BanGanadores")
                diasSinReclamar = bancaConfig.Rows(0)("DiasSinReclamo")
            End If

            Dim diasVenta = DAL.GetTabla("Select MAX(PremioQ1) PremioQ1,MAX(PremioQ2) PremioQ2,MAX(PremioQ3) PremioQ3,CAST(MAX(CAST(EDiDiaCerrado AS INT)) AS BIT) EDiDiaCerrado,MAX(EDiCierreVentaFecha) EDiCierreVentaFecha " &
                                         "From HEstatusDias Where (0=@lot OR LoteriaID=@lot) and EDiFecha=@fec and EDiVentaCerrada=1 And EDiPremiosDentro=1",
                                         ({New Pair("fec", CDate(Fecha)), New Pair("lot", Loteria)}).ToList())
            If diasVenta.Rows.Count > 0 Then
                If Loteria > 0 Then
                    Ganadores3.Primero = diasVenta.Rows(0)("PremioQ1")
                    Ganadores3.Segundo = diasVenta.Rows(0)("PremioQ2")
                    Ganadores3.Tercero = diasVenta.Rows(0)("PremioQ3")
                End If
                Ganadores3.Fecha = Fecha

                Dim DH As String, cmd As SqlCommand, dr As SqlDataReader,
                    FechaCierre As DateTime,
                    FechaSinReclamar As Date? = IIf(diasSinReclamar = 0, Nothing, DateAdd(DateInterval.Day, -1 * diasSinReclamar, CDate(Fecha)))
                If Loteria = 0 OrElse diasVenta.Rows(0)("EDiDiaCerrado") Then
                    DH = "H"
                Else
                    DH = "D"
                End If

                FechaCierre = DateAdd(DateInterval.Hour, 1, diasVenta.Rows(0)("EDiCierreVentaFecha"))

                Dim TicGanadores = DAL.GetTabla("Select t.*,ISNULL(ir.Retenido,0) as Retenido " &
                                                 "From " & DH & "Tickets t" &
                                                 If(DH = "D", " WITH (NOLOCK)", "") &
                                                 " Left Outer Join " & DH & "ImpuestoRetenido ir ON ir.TicketID=t.TicketID " &
                                                 "Where t.TicNulo='0' And t.TicFecha>=@fec And t.TicFecha<=@fecCierre" &
                                                 " And t.BancaID=@ban And (0=@lot OR t.LoteriaID=@lot) And t.TicPago>0 Order by t.TicNumero",
                                             ({New Pair("fec", CDate(Fecha)), New Pair("lot", Loteria),
                                               New Pair("fecCierre", FechaCierre), New Pair("ban", Sesion.Banca)}).ToList())

                Dim ListaGanadores = New List(Of MAR_Bet)
                If TicGanadores.Rows.Count > 0 Then
                    For n = 0 To TicGanadores.Rows.Count - 1
                        Dim row = TicGanadores.Rows(n)
                        Dim TckG = New MAR_Bet With {
                            .Costo = row("TicCosto"),
                            .StrFecha = FormatFecha(row("TicFecha"), 1),
                            .StrHora = FormatFecha(row("TicFecha"), 7),
                            .TicketNo = row("TicNumero"),
                            .Loteria = row("LoteriaID"),
                            .Pago = (row("TicPago") - row("Retenido")),
                            .Ticket = row("TicketID"),
                            .Solicitud = IIf(row("TicPagado"), 5, IIf(veGanad, 3, 4))
                        }
                        'SolicitudID
                        ' 6: ticket ganador sin reclamar
                        ' 5: siempre mostrar como pagado
                        ' 4: pendiente de pago, ocultar pago porque no esta autorizado a verlo.
                        ' 3: pendiente de pago
                        ListaGanadores.Add(TckG)
                    Next
                End If

                If FechaSinReclamar.HasValue Then
                    Dim premioSinReclamar = DAL.GetTabla("Select MAX(PremioQ1) PremioQ1,MAX(PremioQ2) PremioQ2,MAX(PremioQ3) PremioQ3,CAST(MAX(CAST(EDiDiaCerrado AS INT)) AS BIT) EDiDiaCerrado,MAX(EDiCierreVentaFecha) EDiCierreVentaFecha " &
                                                         "From HEstatusDias Where (0=@lot OR LoteriaID=@lot) and EDiFecha=@fec and EDiVentaCerrada=1 And EDiPremiosDentro=1",
                                                         ({New Pair("fec", FechaSinReclamar.Value), New Pair("lot", Loteria)}).ToList())

                    If premioSinReclamar.Rows.Count > 0 AndAlso Not IsDBNull(premioSinReclamar.Rows(0)(0)) Then
                        Dim TicSinReclamar = DAL.GetTabla("Select t.*,ISNULL(ir.Retenido,0) as Retenido " &
                                     "From " & DH & "Tickets t" &
                                     If(DH = "D", " WITH (NOLOCK)", "") &
                                     " Left Outer Join " & DH & "ImpuestoRetenido ir ON ir.TicketID=t.TicketID " &
                                     " Left Outer Join HPagos tp ON tp.PagoID=t.PagoID AND DATEDIFF(DD,tp.EdiFecha,tp.PagDia)<=@DiasReclamo " &
                                     "Where (t.TicPagado=0 OR tp.PagoID IS NULL) And t.TicNulo='0' And t.TicFecha>=@fec And t.TicFecha<=@fecCierre" &
                                     " And t.BancaID=@ban And (0=@lot OR t.LoteriaID=@lot) And t.TicPago>0 Order by t.TicNumero",
                                 ({New Pair("fec", FechaSinReclamar.Value),
                                   New Pair("lot", Loteria),
                                   New Pair("DiasReclamo", diasSinReclamar),
                                   New Pair("fecCierre", DateAdd(DateInterval.Hour, 1, premioSinReclamar.Rows(0)("EDiCierreVentaFecha"))),
                                   New Pair("ban", Sesion.Banca)}).ToList())

                        If TicSinReclamar.Rows.Count > 0 Then
                            For n = 0 To TicSinReclamar.Rows.Count - 1
                                Dim row = TicSinReclamar.Rows(n)
                                Dim TckG = New MAR_Bet With {
                                    .Costo = row("TicCosto"),
                                    .StrFecha = FormatFecha(row("TicFecha"), 1),
                                    .StrHora = FormatFecha(row("TicFecha"), 7),
                                    .TicketNo = row("TicNumero"),
                                    .Loteria = row("LoteriaID"),
                                    .Pago = (row("TicPago") - row("Retenido")),
                                    .Ticket = row("TicketID"),
                                    .Solicitud = 6
                                }
                                'SolicitudID
                                ' 6: ticket ganador sin reclamar O reclamado tarde (-1 *)
                                ' 5: siempre mostrar como pagado
                                ' 4: pendiente de pago, ocultar total si no esta autorizado.
                                ListaGanadores.Add(TckG)
                            Next
                        End If
                    End If

                    Dim TicReclamadoTarde = DAL.GetTabla("Select t.*,ISNULL(ir.Retenido,0) as Retenido " &
                                                         "From " & DH & "Tickets t" &
                                                         If(DH = "D", " WITH (NOLOCK)", "") &
                                                         " Inner Join HPagos tp ON tp.PagoID=t.PagoID AND DATEDIFF(DD,tp.EdiFecha,tp.PagDia)>@DiasReclamo" &
                                                         " Left Outer Join " & DH & "ImpuestoRetenido ir ON ir.TicketID=t.TicketID " &
                                                         "Where t.TicNulo='0' And tp.PagDia=@fec" &
                                                         " And t.BancaID=@ban And (0=@lot OR t.LoteriaID=@lot) And t.TicPago>0 Order by t.TicNumero",
                                                     ({New Pair("fec", CDate(Fecha)),
                                                       New Pair("lot", Loteria),
                                                       New Pair("DiasReclamo", diasSinReclamar),
                                                       New Pair("ban", Sesion.Banca)}).ToList())

                    If TicReclamadoTarde.Rows.Count > 0 Then
                        For n = 0 To TicReclamadoTarde.Rows.Count - 1
                            Dim row = TicReclamadoTarde.Rows(n)
                            Dim TckG = New MAR_Bet With {
                                .Costo = row("TicCosto"),
                                .StrFecha = FormatFecha(row("TicFecha"), 1),
                                .StrHora = FormatFecha(row("TicFecha"), 7),
                                .TicketNo = row("TicNumero"),
                                .Loteria = row("LoteriaID"),
                                .Pago = (-1 * (row("TicPago") - row("Retenido"))),
                                .Ticket = row("TicketID"),
                                .Solicitud = 6
                            }
                            'SolicitudID
                            ' 6: ticket ganador sin reclamar O reclamado tarde (-1 *)
                            ' 5: siempre mostrar como pagado
                            ' 4: pendiente de pago, ocultar total si no esta autorizado.
                            ListaGanadores.Add(TckG)
                        Next
                    End If
                End If

                Ganadores3.Tickets = ListaGanadores.OrderBy(Function(g) g.Solicitud).ThenBy(Function(g) g.TicketNo).ToArray 'Listado final incluyendo clones
            Else
                Ganadores3.Err = "Los premios todavia no han sido registrados para la fecha seleccionada."
            End If
        End Using

        If (Ganadores3.Err Is Nothing OrElse Ganadores3.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
            Ganadores3.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    <WebMethod()> Public Function ListaTickets(ByVal Sesion As MAR_Session, ByVal Loteria As Integer, ByVal Fecha As String) As MAR_Ganadores
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Listado de tickets")
            ListaTickets = New MAR_Ganadores
            If Not ValidSesion(Sesion) Then
                ListaTickets.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (ListaTickets.Err Is Nothing OrElse ListaTickets.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
                ListaTickets.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Dim stat As SqlCommand = New SqlCommand("Select * From HEstatusDias Where (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") and EDiFecha='" & Fecha & "' and GrupoID IN " &
                "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & Sesion.Banca & ")", cn)
            stat.CommandTimeout = gCommandTimeOut
            Dim DrStat As SqlDataReader = stat.ExecuteReader
            If DrStat.Read Then
                If Loteria > 0 Then
                    ListaTickets.Primero = DrStat("PremioQ1")
                    ListaTickets.Segundo = DrStat("PremioQ2")
                    ListaTickets.Tercero = DrStat("PremioQ3")
                End If
                ListaTickets.Fecha = Fecha
                Dim DH As String, cmd As SqlCommand, dr As SqlDataReader, FechaCierre As String
                If Loteria = 0 OrElse DrStat("EDiDiaCerrado") Then
                    DH = "H"
                Else
                    DH = "D"
                End If
                FechaCierre = FormatFecha(DateAdd(DateInterval.Hour, 1, DrStat("EDiCierreVentaFecha")), 2)
                DrStat.Close()
                cmd = New SqlCommand("Select count(*) as Total From " & DH & "Tickets" &
                                        If(DH = "D", " WITH (NOLOCK)", "") &
                                        " Where TicFecha>='" & Fecha & "' And TicFecha<='" & FechaCierre & "' And BancaID=0" & Sesion.Banca & " And (0=" & Loteria & " OR LoteriaID=0" & Loteria & ")", cn)
                cmd.CommandTimeout = gCommandTimeOut
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If dr("total") > 0 Then
                        ReDim ListaTickets.Tickets(dr("total") - 1)
                        dr.Close()
                        cmd.CommandText = "Select a.TicketID,Max(TicNulo*1) TicNulo,Max(LoteriaID) as LoteriaID, Max(TicNumero) TicNumero, Max(TicFecha) TicFecha, Max(TicCosto) TicCosto,Sum(Case When d.BanGanadores=1 then TDePago else 0 End) TDePago From " & DH & "Tickets a" &
                                            If(DH = "D", " WITH (NOLOCK)", "") & " INNER JOIN " & DH & "Ticketdetalle b" &
                                            If(DH = "D", " WITH (NOLOCK)", "") & " ON a.TicketID=b.TicketID" &
                                            " INNER JOIN MBancas d ON d.BancaID=a.BancaID" &
                                            " Where TicFecha>='" & Fecha & "' And TicFecha<='" & FechaCierre & "' And a.BancaID=0" & Sesion.Banca & " And (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") Group by a.TicketID Order by TicNumero"
                        dr = cmd.ExecuteReader
                        Dim n As Integer
                        For n = 0 To ListaTickets.Tickets.Length - 1
                            dr.Read()
                            ListaTickets.Tickets(n) = New MAR_Bet
                            ListaTickets.Tickets(n).Costo = dr("TicCosto")
                            ListaTickets.Tickets(n).StrFecha = FormatFecha(dr("TicFecha"), 1)
                            ListaTickets.Tickets(n).StrHora = FormatFecha(dr("TicFecha"), 7)
                            ListaTickets.Tickets(n).TicketNo = dr("TicNumero")
                            ListaTickets.Tickets(n).Loteria = dr("LoteriaID")
                            ListaTickets.Tickets(n).Pago = dr("TDePago")
                            ListaTickets.Tickets(n).Ticket = dr("TicketID")
                            ListaTickets.Tickets(n).Nulo = dr("TicNulo")
                        Next
                    End If
                    dr.Close()
                End If
            Else
                ListaTickets.Err = "No se encuentra venta para la fecha seleccionada."
            End If
        End Using

        If (ListaTickets.Err Is Nothing OrElse ListaTickets.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
            ListaTickets.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    <WebMethod()> Public Function ListaPines(ByVal Sesion As MAR_Session, ByVal Fecha As String) As MAR_Pines
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Listado de pines")
            ListaPines = New MAR_Pines
            If Not ValidSesion(Sesion) Then
                ListaPines.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (ListaPines.Err Is Nothing OrElse ListaPines.Err.Length = 0) AndAlso EstaCerrandoDia() Then
                ListaPines.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Dim stat As SqlCommand = New SqlCommand("Select MIN(EdiDiaCerrado*1) as EdiDiaCerrado, MAX(EDiCierreVentaFecha) as EDiCierreVentaFecha From HEstatusDias Where EDiFecha='" & Fecha & "' and GrupoID IN " &
                "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & Sesion.Banca & ") having not MAX(EDiCierreVentaFecha) is null", cn)
            stat.CommandTimeout = gCommandTimeOut
            Dim DrStat As SqlDataReader = stat.ExecuteReader
            If DrStat.Read Then
                ListaPines.Fecha = Fecha
                Dim DH As String, cmd As SqlCommand, dr As SqlDataReader, FechaCierre As String
                If DrStat("EDiDiaCerrado") = 1 Then
                    DH = "PHPines"
                Else
                    DH = "PVPines"
                End If
                FechaCierre = FormatFecha(DateAdd(DateInterval.Hour, 1, DrStat("EDiCierreVentaFecha")), 2)
                DrStat.Close()
                cmd = New SqlCommand("Select Count(*) as Total From " & DH & " Where PinFecha >= '" & Fecha & "' And PinFecha < '" & FechaCierre & "' And PinNulo=0 And BancaID=0" & Sesion.Banca, cn)
                cmd.CommandTimeout = gCommandTimeOut
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If dr("total") > 0 Then
                        ReDim ListaPines.Pines(dr("total") - 1)
                        dr.Close()
                        cmd.CommandText = "Select * From " & DH & " Where PinFecha >= '" & Fecha & "' And PinFecha < '" & FechaCierre & "' And PinNulo=0 And BancaID=0" & Sesion.Banca
                        dr = cmd.ExecuteReader
                        Dim n As Integer
                        For n = 0 To ListaPines.Pines.Length - 1
                            dr.Read()
                            ListaPines.Pines(n) = New MAR_Pin
                            ListaPines.Pines(n).Producto = New MAR_Producto
                            With ListaPines.Pines(n)
                                .Costo = dr("PinCosto")
                                .Flag = dr("PinFlag")
                                .Pin = dr("PinID")
                                .Producto.ProID = dr("ProductoID")
                                .Producto.SupID = dr("SuplidorID")
                                .Producto.Producto = dr("ProNombre")
                                .Producto.Suplidor = dr("SupNombre")
                                .Serie = dr("PinNumero")
                                .StrFecha = FormatFecha(dr("PinFecha"), 1)
                                .StrHora = FormatFecha(dr("PinFecha"), 7)
                            End With
                        Next
                    End If
                    dr.Close()
                End If
            Else
                ListaPines.Err = "No se encuentra venta para la fecha seleccionada."
            End If
        End Using

        If (ListaPines.Err Is Nothing OrElse ListaPines.Err.Length = 0) AndAlso EstaCerrandoDia() Then
            ListaPines.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    <WebMethod()> Public Function RptVenta(ByVal Sesion As MAR_Session, ByVal Loteria As Integer, ByVal Fecha As String) As MAR_RptVenta
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Reporte de ventas")
            RptVenta = New MAR_RptVenta
            If Not ValidSesion(Sesion) Then
                RptVenta.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (RptVenta.Err Is Nothing OrElse RptVenta.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
                RptVenta.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Dim stat As SqlCommand = New SqlCommand("Select * From HEstatusDias Where (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") and EDiFecha='" & Fecha & "'", cn)
            stat.CommandTimeout = gCommandTimeOut
            Dim DrStat As SqlDataReader = stat.ExecuteReader, FechaCierre As String, Cantidad As Integer = 0
            If DrStat.Read Then
                Dim DH As String
                If Loteria = 0 OrElse DrStat("EDiDiaCerrado") Then
                    DH = "HResumen"
                Else
                    DH = "VDiaVentas"
                End If

                RptVenta.Fecha = FormatFecha(DrStat("EDiFecha"), 1)
                RptVenta.TicketsNulos = (New List(Of MAR_Bet)).ToArray
                If Loteria = 0 OrElse IsDBNull(DrStat("PremioQ1")) Then
                    RptVenta.Primero = ""
                    RptVenta.Segundo = ""
                    RptVenta.Tercero = ""
                Else
                    RptVenta.Primero = DrStat("PremioQ1")
                    RptVenta.Segundo = DrStat("PremioQ2")
                    RptVenta.Tercero = DrStat("PremioQ3")
                End If

                FechaCierre = FormatFecha(DrStat("EDiCierreVentaFecha"), 2)
                stat.CommandText = "Select * From " & DH & " Where EDiFecha='" & FormatFecha(DrStat("EDiFecha"), 1) & "' AND (0=" & Loteria & " OR LoteriaID=0" & Loteria & ") And BancaID=0" & Sesion.Banca
                DrStat.Close()
                DrStat = stat.ExecuteReader
                If DrStat.Read Then
                    RptVenta.ComisionPorcQ = DrStat("BanComisionQ")
                    RptVenta.ComisionPorcP = DrStat("BanComisionP")
                    RptVenta.ComisionPorcT = DrStat("BanComisionT")
                    Do While True
                        RptVenta.CPrimero += DrStat("CPrimero")
                        RptVenta.CSegundo += DrStat("CSegundo")
                        RptVenta.CTercero += DrStat("CTercero")
                        RptVenta.MPrimero += DrStat("MPrimero")
                        RptVenta.MSegundo += DrStat("MSegundo")
                        RptVenta.MTercero += DrStat("MTercero")
                        RptVenta.MPales += DrStat("MPales")
                        RptVenta.CntNumeros += DrStat("CVQuinielas")
                        RptVenta.Numeros += DrStat("VQuinielas")
                        RptVenta.Pales += DrStat("VPales")
                        RptVenta.Tripletas += DrStat("VTripletas")
                        RptVenta.MTripletas += DrStat("MTripletas")
                        If (Not DrStat.Read) Then Exit Do
                    Loop
                    RptVenta.Comision += Math.Round(((RptVenta.Pales * RptVenta.ComisionPorcP) + (RptVenta.Numeros * RptVenta.ComisionPorcQ) + (RptVenta.Tripletas * RptVenta.ComisionPorcT)) / 100, 0)
                    DH = IIf(Strings.Left(DH, 1) = "V", "D", "H")
                    DrStat.Close()
                    stat.CommandText = "Select Count(*) as Total From " & DH & "Tickets" &
                                            If(DH = "D", " WITH (NOLOCK)", "") & " Where TicNulo='1' And TicCliente<>'' And TicFecha>='" &
                                            Fecha & "' And TicFecha<='" & FechaCierre & "' And BancaID=0" & Sesion.Banca & " And (0=" & Loteria & " OR LoteriaID=0" & Loteria & ")"
                    DrStat = stat.ExecuteReader
                    If DrStat.Read Then
                        ReDim RptVenta.TicketsNulos(DrStat("Total") - 1)
                    End If
                    DrStat.Close()
                    stat.CommandText = "Select * From " & DH & "Tickets" &
                                            If(DH = "D", " WITH (NOLOCK)", "") & " Where TicNulo='1' And TicCliente<>'' And TicFecha>='" &
                                            Fecha & "' And TicFecha<='" & FechaCierre & "' And BancaID=0" & Sesion.Banca & " And (0=" & Loteria &
                                            " OR LoteriaID=0" & Loteria & ") Order by TicNumero"
                    DrStat = stat.ExecuteReader
                    Do While DrStat.Read
                        RptVenta.TicketsNulos(Cantidad) = New MAR_Bet
                        With RptVenta.TicketsNulos(Cantidad)
                            .Costo = DrStat("TicCosto")
                            .StrFecha = FormatFecha(DrStat("TicFecha"), 1)
                            .StrHora = FormatFecha(DrStat("TicFecha"), 7)
                            .TicketNo = DrStat("TicNumero")
                            .Loteria = DrStat("LoteriaID")
                            .Ticket = DrStat("TicketID")
                        End With
                        Cantidad += 1
                    Loop
                End If
            Else
                RptVenta.Err = "No se encuentran informaciones para la fecha seleccionada."
            End If
        End Using

        If (RptVenta.Err Is Nothing OrElse RptVenta.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
            RptVenta.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    <WebMethod()> Public Function RptSumaVta(ByVal Sesion As MAR_Session, ByVal Fecha As String) As MAR_RptSumaVta
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Reporte suma de ventas")
            RptSumaVta = New MAR_RptSumaVta
            If Not ValidSesion(Sesion) Then
                RptSumaVta.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (RptSumaVta.Err Is Nothing OrElse RptSumaVta.Err.Length = 0) AndAlso EstaCerrandoDia() Then
                RptSumaVta.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Try
                RptSumaVta.Reglones = FillRenglon(Sesion.Banca, Fecha, Nothing, Nothing)
                RptSumaVta.Fecha = Fecha
                If RptSumaVta.Reglones Is Nothing OrElse RptSumaVta.Reglones.Length = 0 Then RptSumaVta.Err = "No se han calculado las ventas para la fecha seleccionada."
            Catch ex As Exception
                RptSumaVta.Err = ex.ToString
                Exit Function
            End Try
        End Using

        If (RptSumaVta.Err Is Nothing OrElse RptSumaVta.Err.Length = 0) AndAlso EstaCerrandoDia() Then
            RptSumaVta.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    <WebMethod()> Public Function RptSumaVtaFec(ByVal Sesion As MAR_Session, ByVal FecDesde As String, ByVal FecHasta As String) As MAR_RptSumaVta
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Reporte suma de ventas por fecha " & FecDesde & "->" & FecHasta)
            RptSumaVtaFec = New MAR_RptSumaVta
            If Not ValidSesion(Sesion) Then
                RptSumaVtaFec.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (RptSumaVtaFec.Err Is Nothing OrElse RptSumaVtaFec.Err.Length = 0) AndAlso EstaCerrandoDia() Then
                RptSumaVtaFec.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Try
                RptSumaVtaFec.Reglones = FillRenglon(Sesion.Banca, Nothing, FecDesde, FecHasta)
                RptSumaVtaFec.Fecha = FormatFecha(Today, 1)
                If RptSumaVtaFec.Reglones Is Nothing OrElse RptSumaVtaFec.Reglones.Length = 0 Then RptSumaVtaFec.Err = "No se han calculado las ventas para las fechas seleccionadas."
            Catch ex As Exception
                RptSumaVtaFec.Err = ex.ToString
                Exit Function
            End Try

        End Using

        If (RptSumaVtaFec.Err Is Nothing OrElse RptSumaVtaFec.Err.Length = 0) AndAlso EstaCerrandoDia() Then
            RptSumaVtaFec.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    <WebMethod()> Public Function RptSumaVtaFec2(ByVal Sesion As MAR_Session, ByVal FecDesde As String, ByVal FecHasta As String) As MAR_RptSumaVta2
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Reporte suma de ventas por fecha " & FecDesde & "->" & FecHasta)
            RptSumaVtaFec2 = New MAR_RptSumaVta2
            If Not ValidSesion(Sesion) Then
                RptSumaVtaFec2.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (RptSumaVtaFec2.Err Is Nothing OrElse RptSumaVtaFec2.Err.Length = 0) AndAlso EstaCerrandoDia() Then
                RptSumaVtaFec2.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Try
                RptSumaVtaFec2.Reglones = FillRenglon(Sesion.Banca, Nothing, FecDesde, FecHasta)
                RptSumaVtaFec2.Fecha = FormatFecha(Today, 1)
                If RptSumaVtaFec2.Reglones Is Nothing OrElse RptSumaVtaFec2.Reglones.Length = 0 Then RptSumaVtaFec2.Err = "No se han calculado las ventas para las fechas seleccionadas."
            Catch ex As Exception
                RptSumaVtaFec2.Err = ex.ToString
                Exit Function
            End Try

        End Using

        If (RptSumaVtaFec2.Err Is Nothing OrElse RptSumaVtaFec2.Err.Length = 0) AndAlso EstaCerrandoDia() Then
            RptSumaVtaFec2.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    Private Function FillRenglon(Banca As Integer, ByVal Dia As String, Desde As String, Hasta As String) As Mar_ReglonVta()
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim dr As SqlDataReader
            Dim result As New List(Of Mar_ReglonVta)

            '***------------- Sorteos de Loteria
            Dim cmd = New SqlCommand("Select * From FBancaSumaLoterias( " & Banca & ",@dia,@des,@has)", cn)
            cmd.CommandTimeout = gCommandTimeOut
            cmd.Parameters.AddWithValue("dia", If(Dia IsNot Nothing AndAlso Dia.Trim.Length > 0, CDate(Dia), DBNull.Value))
            cmd.Parameters.AddWithValue("des", If(Desde IsNot Nothing AndAlso Desde.Trim.Length > 0, CDate(Desde), DBNull.Value))
            cmd.Parameters.AddWithValue("has", If(Hasta IsNot Nothing AndAlso Hasta.Trim.Length > 0, CDate(Hasta).AddDays(1), DBNull.Value))
            dr = cmd.ExecuteReader

            While dr.Read
                Dim fr = New Mar_ReglonVta()
                fr.Reglon = dr("LotNombre")
                fr.VentaBruta = (dr("VentaBruta"))
                fr.Comision = (dr("Comision"))
                fr.Saco = (dr("Saco"))
                fr.Resultado = Math.Round(fr.VentaBruta - fr.Comision, 0) - fr.Saco
                fr.Fecha = FormatFecha(dr("EDiFecha"), 1)
                result.Add(fr)
            End While
            dr.Close()

            '***------------- Recargas, Productos, Servicios y Sorteos Especiales
            cmd.CommandText = "Select * From FBancaSumaProductos( " & Banca & ",@dia,@des,@has)"
            dr = cmd.ExecuteReader

            While dr.Read
                Dim fr = New Mar_ReglonVta()
                fr.Reglon = dr("SupNombre")
                fr.VentaBruta = dr("VTarjetas")
                fr.Comision = dr("Comision")
                fr.Saco = 0
                fr.Resultado = Math.Round(fr.VentaBruta - fr.Comision, 0) - fr.Saco
                fr.Fecha = FormatFecha(dr("VenFecha"), 1)
                result.Add(fr)
            End While
            dr.Close()


            '***------------- Pagos Remotos y Ganadores No Reclamados
            'Tickets Sin Reclamar
            Dim config = DAL.GetTabla("Select Modo from SWebProductoConfig Where Opcion='DIAS_RECAUDACION_DINERO_NO_RECLAMADO' AND Activo=1 AND WebProductoID=0")
            Dim diasSinReclamar As Integer = 0
            If config.Rows.Count > 0 Then
                diasSinReclamar = config.Rows(0)("Modo")
            End If

            'Pagos Remotos
            cmd.CommandText = "Select * From FFinanzaBancaConsolida4('(" & Banca & ")',NULL,NULL,NULL,NULL,@dia,@des,@has,NULL)"
            dr = cmd.ExecuteReader
            Do While dr.Read
                Dim fr = New Mar_ReglonVta()
                fr.Reglon = "Pago Remoto"
                fr.VentaBruta = 0.001
                fr.Resultado = dr("PagoEnOtra") + dr("PagoDeOtra")
                fr.Fecha = FormatFecha(dr("EDiFecha"), 1)
                fr.Saco = 0
                If fr.Resultado <> 0 Then result.Add(fr)
                If diasSinReclamar > 0 Then
                    Dim fr2 = New Mar_ReglonVta()
                    fr2.Reglon = "Sin Reclamar"
                    fr2.VentaBruta = 0.001
                    fr2.Resultado = dr("PagosPendiente")
                    fr2.Fecha = FormatFecha(dr("EDiFecha"), 1)
                    fr2.Saco = 0
                    If fr2.Resultado <> 0 Then result.Add(fr2)
                End If
            Loop
            dr.Close()

            Return result.OrderBy(Function(x) x.Reglon).ThenBy(Function(y) CDate(y.Fecha)).ToArray()

        End Using
    End Function

    <WebMethod()> Public Function VentaNumero(ByVal Sesion As MAR_Session, ByVal Loteria As Integer, ByVal Fecha As String) As MAR_VentaNumero
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Listado de venta por numero")
            VentaNumero = New MAR_VentaNumero
            If Fecha <> "LOCALADMIN" Then
                If Not ValidSesion(Sesion) Then
                    VentaNumero.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                    Exit Function
                End If
            End If

            If (VentaNumero.Err Is Nothing OrElse VentaNumero.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
                VentaNumero.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Dim stat As SqlCommand = New SqlCommand("Select * From HEstatusDias Where (0=" & Loteria & " OR LoteriaID=0" & Loteria & ")" & IIf(Fecha = "LOCALADMIN", " and EDiDiaCerrado=0", " and EDiFecha='" & Fecha & "'") & " And GrupoID IN " &
                "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & Sesion.Banca & ")", cn)
            stat.CommandTimeout = gCommandTimeOut
            Dim DrStat As SqlDataReader = stat.ExecuteReader
            If DrStat.Read Then
                Dim DH As String
                VentaNumero.Fecha = FormatFecha(DrStat("EDiFecha"), 1)
                VentaNumero.Loteria = DrStat("LoteriaID")
                If Loteria = 0 OrElse DrStat("EDiDiaCerrado") Then
                    DH = "H"
                Else
                    DH = "D"
                End If
                Dim i As Integer, n As Integer, tot2 As Integer = 0, tot As Integer = 0, ds As DataSet = New DataSet, da As SqlDataAdapter, rw() As DataRow
                da = New SqlDataAdapter("Select TDeNumero,TDeQP, Sum(TDeCantidad) as Cantidad, Sum(TDeCosto) as Apostado, Sum(TDePago) as Pagado From " &
                        DH & "TicketDetalle A" &
                        If(DH = "D", " WITH (NOLOCK)", "") & " INNER JOIN " & DH & "TICKETS B" &
                        If(DH = "D", " WITH (NOLOCK)", "") & " ON A.TicketID=b.TicketID and b.TicNulo='0' and (0=" & Loteria & " OR b.LoteriaID=0" & Loteria & ") " &
                        "Where TicFecha>='" & FormatFecha(DrStat("EDiFecha"), 1) & "' And TicFecha<'" & FormatFecha(DateAdd(DateInterval.Day, 1, DrStat("EDiFecha")), 1) & "' ", cn)
                da.SelectCommand.CommandTimeout = gCommandTimeOut
                DrStat.Close()
                stat.CommandText = "Select UsuarioID from MUsuarios Where UsuNivel>=85 and UsuarioID=0" & Sesion.Usuario
                DrStat = stat.ExecuteReader
                If Not DrStat.Read And Fecha <> "LOCALADMIN" Then da.SelectCommand.CommandText += "And b.BancaID=0" & Sesion.Banca & " "
                DrStat.Close()
                da.SelectCommand.CommandText += "Group by TDeQP,TDeNumero Order by CASE TDeQP WHEN 'Q' THEN 0 WHEN 'P' THEN 1 ELSE 2 END,TDeNumero"
                da.Fill(ds, "VNum")
                i = ds.Tables("VNum").Rows.Count - 1
                ReDim VentaNumero.Numeros(i)
                rw = ds.Tables("VNum").Select()
                For n = 0 To rw.Length - 1
                    VentaNumero.Numeros(n) = New MAR_BetItem
                    VentaNumero.Numeros(n).Cantidad = rw(n)("Apostado")
                    VentaNumero.Numeros(n).Numero = rw(n)("TDeNumero")
                    VentaNumero.Numeros(n).QP = rw(n)("TDeQP")
                    VentaNumero.Numeros(n).Costo = rw(n)("Apostado")
                    VentaNumero.Numeros(n).Pago = rw(n)("Pagado")
                Next
                If n = -1 Then VentaNumero.Err = "No se encuentran los numeros de la fecha seleccionada."
            Else
                VentaNumero.Err = "No se encuentran los numeros de la fecha seleccionada."
            End If

        End Using

        If (VentaNumero.Err Is Nothing OrElse VentaNumero.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
            VentaNumero.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    <WebMethod()> Public Function VentaNumero3(ByVal HardwareKey As String, ByVal Loteria As Integer) As MAR_VentaNumero
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(0, "Reporte", "Listado general de venta por numero")
            VentaNumero3 = New MAR_VentaNumero
            If HardwareKey <> "ADM" Then
                VentaNumero3.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                Exit Function
            End If

            If (VentaNumero3.Err Is Nothing OrElse VentaNumero3.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
                VentaNumero3.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
                Exit Function
            End If

            Dim stat As SqlCommand = New SqlCommand("Select * From HEstatusDias Where LoteriaID=0" & Loteria & " and EDiDiaCerrado=0", cn)
            stat.CommandTimeout = gCommandTimeOut
            Dim DrStat As SqlDataReader = stat.ExecuteReader
            If DrStat.Read Then
                Dim DH As String
                VentaNumero3.Fecha = FormatFecha(DrStat("EDiFecha"), 1)
                VentaNumero3.Loteria = DrStat("LoteriaID")
                If DrStat("EDiDiaCerrado") Then
                    DH = "H"
                Else
                    DH = "D"
                End If
                Dim i As Integer, n As Integer, tot2 As Integer = 0, tot As Integer = 0, ds As DataSet = New DataSet, da As SqlDataAdapter, rw() As DataRow
                da = New SqlDataAdapter("Select TDeNumero,TDeQP, Sum(TDeCantidad) as Cantidad, Sum(TDeCosto) as Apostado, Sum(TDePago) as Pagado From " &
                        DH & "TicketDetalle A" &
                        If(DH = "D", " WITH (NOLOCK)", "") & " INNER JOIN " & DH & "TICKETS B" &
                        If(DH = "D", " WITH (NOLOCK)", "") & " ON A.TicketID=b.TicketID and b.TicNulo='0' and b.LoteriaID=0" & Loteria & " " &
                        "Where TicFecha>='" & FormatFecha(DrStat("EDiFecha"), 1) & "' And TicFecha<'" &
                        FormatFecha(DateAdd(DateInterval.Day, 1, DrStat("EDiFecha")), 1) & "' ", cn)
                da.SelectCommand.CommandTimeout = gCommandTimeOut
                DrStat.Close()
                da.SelectCommand.CommandText += "Group by TDeQP,TDeNumero Order by CASE TDeQP WHEN 'Q' THEN 0 WHEN 'P' THEN 1 ELSE 2 END,TDeNumero"
                da.Fill(ds, "VNum")
                i = ds.Tables("VNum").Rows.Count - 1
                ReDim VentaNumero3.Numeros(i)
                rw = ds.Tables("VNum").Select()
                For n = 0 To rw.Length - 1
                    VentaNumero3.Numeros(n) = New MAR_BetItem
                    VentaNumero3.Numeros(n).Cantidad = rw(n)("Apostado")
                    VentaNumero3.Numeros(n).Numero = rw(n)("TDeNumero")
                    VentaNumero3.Numeros(n).QP = rw(n)("TDeQP")
                    VentaNumero3.Numeros(n).Costo = rw(n)("Apostado")
                    VentaNumero3.Numeros(n).Pago = rw(n)("Pagado")
                Next
                If n = -1 Then VentaNumero3.Err = "No se encuentran los numeros de la fecha seleccionada."
            Else
                VentaNumero3.Err = "No se encuentran los numeros de la fecha seleccionada."
            End If
        End Using

        If (VentaNumero3.Err Is Nothing OrElse VentaNumero3.Err.Length = 0) AndAlso EstaCerrandoDia(Loteria) Then
            VentaNumero3.Err = "La central esta en proceso de cierre de dia. Por favor espere unos minutos y vuelva a intentar."
            Exit Function
        End If

    End Function

    Private Function EstaCerrandoDia(Optional pLoteriaID As Integer = 0) As Boolean
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim cmd As New SqlCommand("SELECT EstatusDiaID FROM HEstatusDias WHERE EDiDiaCerrado=1 AND EDiFecha>DATEADD(dd,-7,GETDATE()) AND " &
                                      "LoteriaID=ISNULL(@lot,LoteriaID) AND EDiCierreDiaFecha>=DATEADD(mi,-3,GETDATE())", cn)
            cmd.Parameters.Add(New SqlParameter("lot", If(pLoteriaID = 0, DBNull.Value, pLoteriaID)))
            Dim dReader = cmd.ExecuteReader()
            EstaCerrandoDia = dReader.Read
            dReader.Close()
        End Using
    End Function

    <WebMethod()> Public Function ListaTicketPagoRemoto(ByVal Sesion As MAR_Session, ByVal Fecha As String) As MAR_Ganadores
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Listado de tickets pago remoto")
            Dim result = New MAR_Ganadores
            If Not ValidSesion(Sesion) Then
                result.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            Else
                Dim stat As SqlCommand = New SqlCommand("SELECT * FROM FTicketsPagoRemotoBanca(0" & Sesion.Banca & ",'" & Fecha & "') Order By TicNumero", cn)
                stat.CommandTimeout = gCommandTimeOut
                Dim DrStat As SqlDataReader = stat.ExecuteReader
                If DrStat.Read Then
                    result.Fecha = Fecha
                    Dim TheTickets = New List(Of MAR_Bet)
                    Do While True
                        Dim rTicket = New MAR_Bet
                        rTicket.Costo = DrStat("TicCosto")
                        rTicket.StrFecha = FormatFecha(DrStat("PagFecha"), 1)
                        rTicket.StrHora = FormatFecha(DrStat("PagFecha"), 7)
                        rTicket.TicketNo = DrStat("TicNumero")
                        rTicket.Pago = DrStat("Saco")
                        rTicket.Cedula = DrStat("PagadoPor")
                        rTicket.Cliente = DrStat("BanContacto")
                        rTicket.Ticket = DrStat("BancaID")
                        TheTickets.Add(rTicket)
                        If Not DrStat.Read Then Exit Do
                    Loop
                    result.Tickets = TheTickets.ToArray
                    DrStat.Close()
                Else
                    result.Err = "No se encuentran pagos remotos de tickets."
                End If
            End If
            ListaTicketPagoRemoto = result
        End Using

    End Function

#End Region

#Region "Private WS Loteria"

    Private Function VerifyItem(ByVal Sesion As MAR_Session, ByRef Item As MAR_BetItem, ByVal EsSuperPalet As Boolean) As String
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            VerifyItem = "OK"
            Dim SQLCmdGlobales As String, SQLCmdBanca As String, SQLCmdTicket As String, SQLCmdControles As String, SQLCmdControlRifero As String, SQLCmdZona As String, SQLCmdLimDinero As String, Limite As Integer = Item.Cantidad

            SQLCmdControlRifero = "Select Sum(D.LDiCantidad) as Vendido, MIN(A.ConLimite) AS Limite From (DControles A INNER JOIN DControlDetalle B ON A.ControlID=B.ControlID) " &
                "LEFT OUTER JOIN DListaDia D ON B.LoteriaID=D.LoteriaID And B.RiferoID=D.RiferoID AND B.CDeQP=D.LDiQP AND B.CDeNumero=D.LDiNumero " &
                "Where B.CDeNumero='" & Item.Numero & "' And B.LoteriaID=" & Item.Loteria & " And B.RiferoID IN " &
                "(Select RiferoID From MBancas Where BancaID=" & Sesion.Banca & ")"

            SQLCmdControles = "Select Sum(D.LDiCantidad) as Vendido, MIN(A.ConLimite) AS Limite From (DControles A INNER JOIN DControlDetalle B ON A.ControlID=B.ControlID) " &
                "LEFT OUTER JOIN DListaDia D ON B.LoteriaID=D.LoteriaID And A.GrupoID=D.GrupoID AND B.CDeQP=D.LDiQP AND B.CDeNumero=D.LDiNumero " &
                "And D.GrupoID IN " &
                "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON B.RiferoID=A.RiferoID Where B.BancaID=" & Sesion.Banca & ") " &
                "Where B.CDeNumero='" & Item.Numero & "' And B.LoteriaID=" & Item.Loteria & " And B.RiferoID='0'"

            SQLCmdLimDinero = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, (SELECT SUM(CAST(CONFIGVALUE AS int)) FROM MBANCASCONFIG WHERE bancaid = 0" & Sesion.Banca & " and ConfigKey IN ('LIMITE_BANCA_DINERO','BANCA_EXTRA_CREDITO') AND Activo = 1) AS Limite " &
                 "From MBancas A LEFT OUTER JOIN DListaDia D ON A.BancaID=D.BancaID JOIN MBancasConfig C on A.Bancaid = C.bancaid " &
                 "Where A.BanMaxQuiniela>=0 And A.BancaID=0" & Sesion.Banca & " and c.ConfigKey = 'LIMITE_BANCA_DINERO' AND c.Activo = 1"

            Dim qp As String = " del numero ", cmd As SqlCommand = New SqlCommand("Select * From TGrupos", cn), dr As SqlDataReader,
                LimGlo As Boolean = True, LimNum As Boolean = True, LimTicBan As Boolean = True, LimGloBan As Boolean = True, LimNumRif As Boolean = True, LimPorDinero As Boolean = False
            cmd.CommandTimeout = gCommandTimeOut
            If Item.QP = "P" Then qp = " del Pale "
            If Item.QP = "T" Then qp = " de la Tripleta "
            If Item.QP = "F" Then qp = " del Pega3 Fijo "
            If Item.QP = "C" Then qp = " del Pega3 Combo "
            If Item.QP = "C" Then Item.QP = "Q"
            If Item.QP = "F" Then Item.QP = "P"

            dr = cmd.ExecuteReader
            Select Case Item.QP
                Case "Q"  'Querys para quinielas
                    SQLCmdGlobales = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, ISNULL(MIN(A.DDeMaxQuiniela),0) AS Limite From MDiasDefecto A LEFT OUTER JOIN " &
                        "DListaDia D ON A.LoteriaID=D.LoteriaID And A.GrupoID=D.GrupoID And A.EsquemaID='0' And " &
                        "D.LDiQP='Q' AND D.LDiNumero='" & Item.Numero & "' " &
                        "Where A.DDeDia=DATEPART(dw,Getdate()) And a.DDeMaxQuiniela>=0  And A.LoteriaID=0" & Item.Loteria

                    SQLCmdBanca = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, MIN(A.BanMaxQuiniela) AS Limite From MBancas A LEFT OUTER JOIN " &
                        "DListaDia D ON A.BancaID=D.BancaID And D.LDiQP='Q' AND D.LDiNumero='" & Item.Numero & "' And D.LoteriaID=0" & Item.Loteria &
                        " Where A.BanMaxQuiniela>=0 And A.BancaID=0" & Sesion.Banca

                    SQLCmdZona = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, MIN(A.MaxQuiniela) AS Limite From MZonaLimites A INNER JOIN MZonas B ON A.ZonaID=B.ZonaID LEFT OUTER JOIN " &
                        "MRiferos R ON R.ZonaID=A.ZonaID FULL JOIN DListaDia D ON A.LoteriaID=D.LoteriaID And R.RiferoID=D.RiferoID And D.LDiQP='Q' AND D.LDiNumero='" & Item.Numero & "'" &
                        " Where B.Activa=1 And A.MaxQuiniela>=0 And A.LoteriaID=0" & Item.Loteria &
                        " And A.ZonaID=(Select ZonaID From MRiferos rr Inner Join MBancas bb ON rr.RiferoID=bb.RiferoID Where bb.BancaID=0" & Sesion.Banca & ")"

                    SQLCmdTicket = "Select 0 as Vendido, MIN(C.BanMaxQuinielaTic) AS Limite From MBancas C INNER JOIN " &
                        "(SELECT DISTINCT 0" & Sesion.Banca & " as BancaID,B.CDeNumero FROM DControles A INNER JOIN DControlDetalle B ON A.ControlID=B.ControlID WHERE CDeQP='Q' and B.CDeNumero='" & Item.Numero & "' and LoteriaID=0" & Item.Loteria & ") D " &
                        "ON C.BancaID=D.BancaID " &
                        "Where C.BanMaxQuinielaTic>=0 And C.BancaID=0" & Sesion.Banca

                Case "P"  'Querys para Pales
                    SQLCmdGlobales = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, ISNULL(MIN(A.DDeMaxPale),0) AS Limite From MDiasDefecto A LEFT OUTER JOIN " &
                        "DListaDia D ON A.LoteriaID=D.LoteriaID And A.GrupoID=D.GrupoID And A.EsquemaID='0' And " &
                        "D.LDiQP='P' AND D.LDiNumero='" & Item.Numero & "' " &
                        "Where A.DDeDia=DATEPART(dw,Getdate()) And a.DDeMaxPale>=0  And A.LoteriaID=0" & Item.Loteria

                    If EsSuperPalet Then
                        SQLCmdBanca = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, MIN(A.BanMaxSupe) AS Limite From MBancas A LEFT OUTER JOIN " &
                            "DListaDia D ON A.BancaID=D.BancaID And D.LDiQP='P' AND D.LDiNumero='" & Item.Numero & "' And D.LoteriaID=0" & Item.Loteria &
                            " Where A.BanMaxSupe>=0 And A.BancaID=0" & Sesion.Banca
                    Else
                        SQLCmdBanca = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, MIN(A.BanMaxPale) AS Limite From MBancas A LEFT OUTER JOIN " &
                            "DListaDia D ON A.BancaID=D.BancaID And D.LDiQP='P' AND D.LDiNumero='" & Item.Numero & "' And D.LoteriaID=0" & Item.Loteria &
                            " Where A.BanMaxPale>=0 And A.BancaID=0" & Sesion.Banca
                    End If

                    SQLCmdZona = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, MIN(A.MaxPale) AS Limite From MZonaLimites A INNER JOIN MZonas B ON A.ZonaID=B.ZonaID LEFT OUTER JOIN " &
                                    "MRiferos R ON R.ZonaID=A.ZonaID FULL JOIN DListaDia D ON A.LoteriaID=D.LoteriaID And R.RiferoID=D.RiferoID And D.LDiQP='P' AND D.LDiNumero='" & Item.Numero & "'" &
                                    " Where B.Activa=1 And A.MaxPale>=0 And A.LoteriaID=0" & Item.Loteria &
                                    " And A.ZonaID=(Select ZonaID From MRiferos rr Inner Join MBancas bb ON rr.RiferoID=bb.RiferoID Where bb.BancaID=0" & Sesion.Banca & ")"

                    SQLCmdTicket = "Select 0 as Vendido, MIN(C.BanMaxPaleTic) AS Limite From MBancas C INNER JOIN " &
                        "(SELECT DISTINCT 0" & Sesion.Banca & " as BancaID,B.CDeNumero FROM DControles A INNER JOIN DControlDetalle B ON A.ControlID=B.ControlID WHERE CDeQP='P' and B.CDeNumero='" & Item.Numero & "' and LoteriaID=0" & Item.Loteria & ") D " &
                        "ON C.BancaID=D.BancaID " &
                        "Where C.BanMaxPaleTic>=0 And C.BancaID=0" & Sesion.Banca

                Case "T"  'Querys para Tripletas
                    SQLCmdGlobales = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, ISNULL(MIN(A.DDeMaxTriple), 0) AS Limite From MDiasDefecto A LEFT OUTER JOIN " &
                        "DListaDia D ON A.LoteriaID=D.LoteriaID And A.GrupoID=D.GrupoID And A.EsquemaID='0' And " &
                        "D.LDiQP='T' AND D.LDiNumero='" & Item.Numero & "' " &
                        "Where A.DDeDia=DATEPART(dw,Getdate()) And a.DDeMaxTriple>=0  And A.LoteriaID=0" & Item.Loteria

                    SQLCmdBanca = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, MIN(A.BanMaxTriple) AS Limite From MBancas A LEFT OUTER JOIN " &
                        "DListaDia D ON A.BancaID=D.BancaID And D.LDiQP='T' AND D.LDiNumero='" & Item.Numero & "' And D.LoteriaID=0" & Item.Loteria &
                        " Where A.BanMaxTriple>=0 And A.BancaID=0" & Sesion.Banca

                    SQLCmdZona = "Select ISNULL(Sum(D.LDiCantidad), 0) as Vendido, MIN(A.MaxTripleta) AS Limite From MZonaLimites A INNER JOIN MZonas B ON A.ZonaID=B.ZonaID LEFT OUTER JOIN " &
                                    "MRiferos R ON R.ZonaID=A.ZonaID FULL JOIN DListaDia D ON A.LoteriaID=D.LoteriaID And R.RiferoID=D.RiferoID And D.LDiQP='T' AND D.LDiNumero='" & Item.Numero & "'" &
                                    " Where B.Activa=1 And A.MaxTripleta>=0 And A.LoteriaID=0" & Item.Loteria &
                                    " And A.ZonaID=(Select ZonaID From MRiferos rr Inner Join MBancas bb ON rr.RiferoID=bb.RiferoID Where bb.BancaID=0" & Sesion.Banca & ")"

                    SQLCmdTicket = "Select 0 as Vendido, MIN(C.BanMaxTripleTic) AS Limite From MBancas C INNER JOIN " &
                        "(SELECT DISTINCT 0" & Sesion.Banca & " as BancaID,B.CDeNumero FROM DControles A INNER JOIN DControlDetalle B ON A.ControlID=B.ControlID WHERE CDeQP='T' and B.CDeNumero='" & Item.Numero & "' and LoteriaID=0" & Item.Loteria & ") D " &
                        "ON C.BancaID=D.BancaID " &
                        "Where C.BanMaxTripleTic>=0 And C.BancaID=0" & Sesion.Banca

                Case Else
                    SQLCmdGlobales = String.Empty
                    SQLCmdBanca = String.Empty
                    SQLCmdTicket = String.Empty
                    SQLCmdZona = String.Empty

            End Select


            If dr.Read Then
                LimGlo = dr("GruLimiteGlo")
                LimGloBan = dr("GruLimiteGloBanca")
                LimTicBan = dr("GruLimiteTicBanca")
                LimNum = dr("GruLimiteNum")
                LimNumRif = dr("GruLimiteNumRifero")
                LimPorDinero = dr("GruLimiteBanDinero")
            End If
            dr.Close()

            If LimGlo Then 'Limites Globales activados
                cmd.CommandText = SQLCmdGlobales
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If Not IsDBNull(dr("VENDIDO")) And Not IsDBNull(dr("Limite")) Then
                        If (dr("vendido") + Item.Cantidad) > dr("Limite") Then
                            Limite = dr("Limite") - dr("vendido")
                            If Limite <= 0 Then
                                VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (global)"
                                Item.Cantidad = 0
                            Else
                                VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (global)"
                                Item.Cantidad = Limite
                            End If
                            dr.Close()
                            Exit Function
                        End If
                    Else
                        If Not IsDBNull(dr("limite")) Then
                            If Item.Cantidad > dr("Limite") Then
                                Limite = dr("Limite")
                                If Limite <= 0 Then
                                    VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (global)"
                                    Item.Cantidad = 0
                                Else
                                    VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (global)"
                                    Item.Cantidad = Limite
                                End If
                                dr.Close()
                                Exit Function
                            End If
                        End If
                    End If
                End If
                dr.Close()
            End If
            If LimGloBan Then 'Limite global por banca
                cmd.CommandText = SQLCmdBanca
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If Not IsDBNull(dr("VENDIDO")) And Not IsDBNull(dr("Limite")) Then
                        If (dr("vendido") + Item.Cantidad) > dr("Limite") Then
                            Limite = dr("Limite") - dr("vendido")
                            If Limite <= 0 Then
                                VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (banca)"
                                Item.Cantidad = 0
                            Else
                                VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (banca)"
                                Item.Cantidad = Limite
                            End If
                            dr.Close()
                            Exit Function
                        End If
                    Else
                        If Not IsDBNull(dr("limite")) Then
                            If Item.Cantidad > dr("Limite") Then
                                Limite = dr("Limite")
                                If Limite <= 0 Then
                                    VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (banca)"
                                    Item.Cantidad = 0
                                Else
                                    VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (banca)"
                                    Item.Cantidad = Limite
                                End If
                                dr.Close()
                                Exit Function
                            End If
                        End If
                    End If
                End If
                dr.Close()
            End If

            'Limites por Zona
            cmd.CommandText = SQLCmdZona
            dr = cmd.ExecuteReader
            If dr.Read Then
                If Not IsDBNull(dr("VENDIDO")) And Not IsDBNull(dr("Limite")) Then
                    If (dr("vendido") + Item.Cantidad) > dr("Limite") Then
                        Limite = dr("Limite") - dr("vendido")
                        If Limite <= 0 Then
                            VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (zona)"
                            Item.Cantidad = 0
                        Else
                            VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (zona)"
                            Item.Cantidad = Limite
                        End If
                        dr.Close()
                        Exit Function
                    End If
                Else
                    If Not IsDBNull(dr("limite")) Then
                        If Item.Cantidad > dr("Limite") Then
                            Limite = dr("Limite")
                            If Limite <= 0 Then
                                VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (zona)"
                                Item.Cantidad = 0
                            Else
                                VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (zona)"
                                Item.Cantidad = Limite
                            End If
                            dr.Close()
                            Exit Function
                        End If
                    End If
                End If
            End If
            dr.Close()

            If LimNumRif Then 'Por numero por rifero
                cmd.CommandText = SQLCmdControlRifero
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If Not IsDBNull(dr("VENDIDO")) And Not IsDBNull(dr("Limite")) Then
                        If (dr("vendido") + Item.Cantidad) > dr("Limite") Then
                            Limite = dr("Limite") - dr("vendido")
                            If Limite <= 0 Then
                                VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (controlado)."
                                Item.Cantidad = 0
                            Else
                                VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (controlado)"
                                Item.Cantidad = Limite
                            End If
                            dr.Close()
                            Exit Function
                        End If
                    Else
                        If Not IsDBNull(dr("limite")) Then
                            If Item.Cantidad > dr("Limite") Then
                                Limite = dr("Limite")
                                If Limite <= 0 Then
                                    VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (controlado)"
                                    Item.Cantidad = 0
                                Else
                                    VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (controlado)"
                                    Item.Cantidad = Limite
                                End If
                                dr.Close()
                                Exit Function
                            End If
                        End If
                    End If
                End If
                dr.Close()
            End If
            If LimNum Then 'Limite general por numero
                cmd.CommandText = SQLCmdControles
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If Not IsDBNull(dr("VENDIDO")) And Not IsDBNull(dr("Limite")) Then
                        If (dr("vendido") + Item.Cantidad) > dr("Limite") Then
                            Limite = dr("Limite") - dr("vendido")
                            If Limite <= 0 Then
                                VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (limitado)"
                                Item.Cantidad = 0
                            Else
                                VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (limitado)"
                                Item.Cantidad = Limite
                            End If
                            dr.Close()
                            Exit Function
                        End If
                    Else
                        If Not IsDBNull(dr("limite")) Then
                            If Item.Cantidad > dr("Limite") Then
                                Limite = dr("Limite")
                                If Limite <= 0 Then
                                    VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (limitado)"
                                    Item.Cantidad = 0
                                Else
                                    VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (limitado)"
                                    Item.Cantidad = Limite
                                End If
                                dr.Close()
                                Exit Function
                            End If
                        End If
                    End If
                End If
                dr.Close()
            End If
            If LimTicBan Then 'Limite particular por ticket
                cmd.CommandText = SQLCmdTicket
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If Not IsDBNull(dr("VENDIDO")) And Not IsDBNull(dr("Limite")) Then
                        If (dr("vendido") + Item.Cantidad) > dr("Limite") Then
                            Limite = dr("Limite") - dr("vendido")
                            If Limite <= 0 Then 'esto nunca debe ocurrir porque vendido=0
                                VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (ticket)"
                                Item.Cantidad = 0
                            Else
                                VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (ticket)"
                                Item.Cantidad = Limite
                            End If
                            dr.Close()
                            Exit Function
                        End If
                    Else 'esto se supone nunca debe ocurrir porque no hay outer join
                        If Not IsDBNull(dr("limite")) Then
                            If Item.Cantidad > dr("Limite") Then
                                Limite = dr("Limite")
                                If Limite <= 0 Then
                                    VerifyItem = "Ya no hay disponibilidad para el " & Item.Numero & " (ticket)"
                                    Item.Cantidad = 0
                                Else
                                    VerifyItem = "Solamente puede vender " & Limite & qp & Item.Numero & " (ticket)"
                                    Item.Cantidad = Limite
                                End If
                                dr.Close()
                                Exit Function
                            End If
                        End If
                    End If
                End If
                dr.Close()
            End If
            If LimPorDinero Then 'Limite por dinero
                cmd.CommandText = SQLCmdLimDinero
                dr = cmd.ExecuteReader
                If dr.Read Then
                    If Not IsDBNull(dr("VENDIDO")) And Not IsDBNull(dr("Limite")) Then
                        If dr("vendido") >= (dr("Limite") * 0.9) Then
                            PlaceMensaje(Sesion, "Paso el 90% del Limite Ventas por dia, con un Total Vendido de: " & dr("vendido") & ". Y su Limite es: " & dr("Limite") & ".")
                        End If
                        If (dr("vendido") + Item.Cantidad) > dr("Limite") Then
                            Limite = dr("Limite") - dr("vendido")
                            If Limite <= 0 Then 'esto nunca debe ocurrir porque vendido=0
                                VerifyItem = "Ya no hay disponibilidad para mas venta en esta banca, comunique a un administrador."
                                Item.Cantidad = 0
                            Else
                                VerifyItem = "Solamente tiene " & Limite & " disponibles para vender"
                                Item.Cantidad = Limite
                            End If
                            dr.Close()
                            Exit Function
                        End If
                    Else 'esto se supone nunca debe ocurrir porque no hay outer join
                        If Not IsDBNull(dr("limite")) Then
                            If Item.Cantidad > dr("Limite") Then
                                Limite = dr("Limite")
                                If Limite <= 0 Then
                                    VerifyItem = "Ya no hay disponibilidad para mas venta en esta banca, comunique a un administrador."
                                    Item.Cantidad = 0
                                Else
                                    VerifyItem = "Solamente tiene " & Limite & " disponibles para vender"
                                    Item.Cantidad = Limite
                                End If
                                dr.Close()
                                Exit Function
                            End If
                        End If
                    End If
                End If
                dr.Close()
            End If
        End Using
    End Function

    Private Function CRCBet(ByRef Bet As MAR_Bet) As MAR_Bet
        If Bet.Cedula <> "CRC" Then Return Bet
        Dim crc As Integer = 0
        For Each bt As MAR_BetItem In Bet.Items
            crc += ((bt.Cantidad * CInt(bt.Numero)) + Bet.Loteria + Bet.Ticket) Mod 1000
        Next
        If Bet.Err = "" Then ReDim Bet.Items(0)
        Bet.Cedula = crc.ToString
        Return Bet
    End Function

    Private Function GeneraPinGanador(ByVal Solicitud As String) As Integer
        Dim x As Integer
        GeneraPinGanador = 1
        'Exit Function
        For x = 0 To Solicitud.Length - 1
            GeneraPinGanador = GeneraPinGanador * (3 + CInt(Solicitud.Chars(x).ToString))
        Next
        GeneraPinGanador = GeneraPinGanador Mod 100000
    End Function

    Private Function ComparaPinGanador(ByVal Solicitud As String, ByVal PosiblePin As String) As Boolean

        Dim sb As New StringBuilder()
        Dim ConfirmK As Integer = 0
        For n = 0 To 2
            Dim iK = CInt(PosiblePin.Chars(n).ToString)
            sb.Append(iK.ToString)
            ConfirmK += iK
        Next

        Dim seed = 1
        Dim sb2 As New StringBuilder()
        For x = 0 To Solicitud.Length - 1
            seed = seed + (ConfirmK + CInt(Solicitud.Chars(x).ToString))
            Dim iK = seed Mod 10
            sb2.Append(iK.ToString)
        Next

        ComparaPinGanador = (PosiblePin = sb.ToString & Strings.Right(sb2.ToString, 5))

    End Function

    <WebMethod()> Public Function PlaceLocalBet(ByVal Sesion As MAR_Session, ByVal Apuesta As MAR_Bet, ByVal Solicitud As Double) As String
        Dim CronoInit As DateTime = Now()
        PlaceLocalBet = "Error desconocido."
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If ValidSesion(Sesion) Then
                Dim stat As SqlCommand = New SqlCommand("Select EstatusDiaID From HEstatusDias Where EDiDiaCerrado='1' And LoteriaID=0" & Apuesta.Loteria & " And EDiFecha='" & Apuesta.StrFecha & "' and GrupoID IN " &
                    "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & Sesion.Banca & ")", cn)
                stat.CommandTimeout = gCommandTimeOut
                Dim DrStat As SqlDataReader = stat.ExecuteReader
                If Not DrStat.Read Then
                    PlaceLocalBet = "El dia de la venta local no ha sido cerrado todavia."

                    Exit Function
                End If
                DrStat.Close()
                If Apuesta.Items.Length = 0 Then
                    PlaceLocalBet = "Su jugada no tiene ningún número."

                    Exit Function
                End If
                '*********** Verifica Duplicidad ******************
                Dim dr2 As SqlDataReader, cmd2 As SqlCommand
                cmd2 = New SqlCommand("Select TLoNumero From HTicketsLocal Where BancaID=0" & Sesion.Banca & " and TLoFecha='" & Apuesta.StrFecha & "' and LoteriaID=0" & Apuesta.Loteria, cn)
                cmd2.CommandTimeout = gCommandTimeOut
                dr2 = cmd2.ExecuteReader
                If dr2.Read Then
                    PlaceLocalBet = "OK"
                    dr2.Close()

                    Exit Function
                End If
                dr2.Close()
                '************************************

                Dim TrscTicket As SqlTransaction
                TrscTicket = cn.BeginTransaction(IsolationLevel.ReadCommitted, "GrabaTicket")
                Try
                    Dim cmd As SqlCommand = New SqlCommand("Select A.GrupoID, A.RiferoID, B.BancaID From MRiferos A Inner Join MBancas B ON A.RiferoID=B.RiferoID And B.BancaID=0" & Sesion.Banca, cn)
                    cmd.CommandTimeout = gCommandTimeOut
                    cmd.Transaction = TrscTicket
                    Dim dr As SqlDataReader = cmd.ExecuteReader
                    dr.Read()
                    cmd.CommandText = "Insert into HTicketsLocal (GrupoID,RiferoID,BancaID,LoteriaID,UsuarioID,TLoFecha,TLoCosto,TLoSolicitud,TLoCedula,TLoNulo) Values (" &
                        dr("GrupoID") & "," & dr("RiferoID") & "," & Sesion.Banca & "," & Apuesta.Loteria & "," & Sesion.Usuario & ",'" & Apuesta.StrFecha & "'," & Apuesta.Costo & "," & Solicitud & "," & DateDiff(DateInterval.Second, CronoInit, Now) & ",0)"
                    dr.Close()
                    'cmd.ExecuteNonQuery()
                    cmd.CommandText &= "; SELECT SCOPE_IDENTITY()"
                    cmd.CommandText = "Select TicketLocalID,TLoNumero,TLoFecha From HTicketsLocal Where TicketLocalID=0" & cmd.ExecuteScalar
                    dr = cmd.ExecuteReader
                    dr.Read()
                    Apuesta.StrFecha = FormatFecha(dr("TloFecha"), 1)
                    Apuesta.StrHora = FormatFecha(dr("TloFecha"), 7)
                    Apuesta.Ticket = dr("ticketlocalid")
                    Apuesta.TicketNo = Solicitud
                    Apuesta.Nulo = False
                    dr.Close()
                    Dim bitm As MAR_BetItem
                    For Each bitm In Apuesta.Items
                        cmd.CommandText = "INSERT INTO HTicketLocalDetalle (TicketLocalID,TLDQP,TLDNumero,TLDCantidad,TLDCosto) Values (" &
                            Apuesta.Ticket & ",'" & bitm.QP & "','" & bitm.Numero & "'," & bitm.Cantidad & "," & bitm.Costo & ")"
                        cmd.ExecuteNonQuery()
                    Next
                    cmd.CommandText = "Exec PagaLocal '" & Apuesta.StrFecha & "',0" & Apuesta.Loteria
                    cmd.ExecuteNonQuery()
                    TrscTicket.Commit()
                    PlaceLocalBet = "OK"

                Catch ex As System.Exception
                    TrscTicket.Rollback()
                    PlaceLocalBet = "La apuesta no pudo ser procesada adecuadamente. Solicite soporte técnico." & Chr(13) & Chr(13) & "Mensaje: " & ex.ToString
                End Try
            Else
                PlaceLocalBet = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            End If
        End Using
    End Function

    <WebMethod()> Public Function RePrint(ByVal Sesion As MAR_Session, ByVal Ticket As Integer) As MAR_Bet
        RePrint = New MAR_Bet
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Log(Sesion.Banca, "Reporte", "Intenta reimprimir el ticket ID:" & Ticket)
            If ValidSesion(Sesion) Then
                Dim stat As SqlCommand = New SqlCommand("Select EstatusDiaID From HEstatusDias D, DTickets T Where T.LoteriaID=D.LoteriaID AND D.EDiDiaCerrado='0' And D.EDiPremiosDentro='1' And D.EDiFecha<=GetDate() AND T.TicketID=0" & Ticket, cn)
                stat.CommandTimeout = gCommandTimeOut
                Dim drDia = stat.ExecuteReader
                Dim PremiosSalieron = drDia.Read
                drDia.Close()
                If Not PremiosSalieron Then
                    Dim cmd As SqlCommand = New SqlCommand("Select * From DTickets Where TicNulo='0' And TicketID=0" & Ticket & " and BancaID IN (Select BancaID From MBancas Where BancaID=0" & Sesion.Banca & " and (BanReprintTicketID=-1 OR BanReprintTicketID=0" & Ticket & "))", cn)
                    cmd.CommandTimeout = gCommandTimeOut
                    Dim dr As SqlDataReader = cmd.ExecuteReader, n As Integer = 0
                    If dr.Read Then
                        If MAR.BusinessLogic.Code.ProductosConfigLogic.PuedeReimprimirTicketLoteria(DateTime.Parse(dr("TicFecha"))) Then
                            RePrint.Grupo = dr("GrupoID")
                            RePrint.Loteria = dr("LoteriaID")
                            RePrint.Ticket = dr("TicketID")
                            RePrint.TicketNo = dr("TicNumero")
                            RePrint.Solicitud = dr("TicSolicitud")
                            RePrint.Nulo = False
                            RePrint.StrFecha = FormatFecha(dr("TicFecha"), 1)
                            RePrint.StrHora = FormatFecha(dr("TicFecha"), 7)
                            RePrint.Costo = dr("TicCosto")
                            dr.Close()
                            cmd.CommandText = "Select count(*) as Total From DTicketDetalle Where TicketID=0" & Ticket
                            dr = cmd.ExecuteReader
                            dr.Read()
                            ReDim RePrint.Items(dr("Total") - 1)
                            dr.Close()
                            cmd.CommandText = "Select * From DTicketDetalle Where TicketID=0" & Ticket
                            dr = cmd.ExecuteReader
                            Do While dr.Read
                                RePrint.Items(n) = New MAR_BetItem
                                RePrint.Items(n).Cantidad = dr("TDeCantidad")
                                RePrint.Items(n).Numero = dr("TDeNumero")
                                RePrint.Items(n).QP = dr("TDeQP")
                                RePrint.Items(n).Costo = dr("TDeCosto")
                                n = n + 1
                            Loop
                            dr.Close()
                            RePrint.Cliente = BuildTicketFooter(Sesion, RePrint)
                        Else
                            RePrint.Err = "No puede re-imprimir. Paso el tiempo establecido."
                        End If
                    Else
                        RePrint.Err = "Ese número de ticket no existe o usted no tiene permiso para re-imprimirlo."
                    End If

                Else
                    RePrint.Err = "No puede re-imprimir despues de que han salido los premios."
                End If

            Else
                RePrint.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            End If
        End Using
    End Function

    <WebMethod()> Public Function GetBet(ByVal Sesion As MAR_Session, ByVal TicketNo As String) As MAR_Bet
        GetBet = New MAR_Bet
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If ValidSesion(Sesion) Then
                Dim cmd As SqlCommand = New SqlCommand("Select * From DTickets Where TicNumero='" & TicketNo & "' Order by TicNulo", cn)
                cmd.CommandTimeout = gCommandTimeOut
                Dim dr As SqlDataReader = cmd.ExecuteReader, n As Integer = 0, DH As String = "D"
                If Not dr.Read Then
                    dr.Close()
                    DH = "H"
                    cmd.CommandText = "Select * From HTickets Where TicNumero='" & TicketNo & "' Order by TicNulo"
                    dr = cmd.ExecuteReader
                    If Not dr.Read Then
                        GetBet.Err = "Ese número de ticket no existe. Revise el número e intetelo nuevamente. " & TicketNo
                        Exit Function
                    End If
                End If
                GetBet.Grupo = dr("GrupoID")
                GetBet.Loteria = dr("LoteriaID")
                GetBet.Ticket = dr("TicketID")
                GetBet.TicketNo = dr("TicNumero")
                GetBet.Nulo = dr("TicNulo")
                GetBet.StrFecha = FormatFecha(dr("TicFecha"), 1)
                GetBet.StrHora = FormatFecha(dr("TicFecha"), 7)
                GetBet.Costo = dr("TicCosto")
                GetBet.Solicitud = dr("TicSolicitud")
                dr.Close()
                cmd.CommandText = "Select count(*) as Total From " & DH & "TicketDetalle Where TicketID=0" & GetBet.Ticket
                dr = cmd.ExecuteReader
                dr.Read()
                ReDim GetBet.Items(dr("Total") - 1)
                dr.Close()
                cmd.CommandText = "Select * From " & DH & "TicketDetalle Where TicketID=0" & GetBet.Ticket
                dr = cmd.ExecuteReader
                Do While dr.Read
                    GetBet.Items(n) = New MAR_BetItem
                    GetBet.Items(n).Cantidad = dr("TDeCantidad")
                    GetBet.Items(n).Numero = dr("TDeNumero").Trim
                    GetBet.Items(n).QP = dr("TDeQP")
                    GetBet.Items(n).Costo = dr("TDeCosto")
                    GetBet.Items(n).Pago = dr("TDePago")
                    n = n + 1
                Loop
                dr.Close()
                GetBet.Cliente = BuildTicketFooter(Sesion, GetBet)
            Else
                GetBet.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            End If
        End Using
    End Function

    <WebMethod()> Public Function Anula(ByVal Sesion As MAR_Session, ByVal TicketNo As String) As String
        Anula = String.Empty
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If ValidSesion(Sesion) Then
                Dim cmd As SqlCommand = New SqlCommand("Select a.*, b.gruminutosanula From DTickets a inner join tgrupos b ON A.GrupoID=b.GrupoID Where TicNulo='0' And TicNumero='" & TicketNo & "'", cn)
                cmd.CommandTimeout = gCommandTimeOut
                Dim dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read Then
                    If DateDiff(DateInterval.Minute, dr("TicFecha"), Now) >= dr("GruMinutosAnula") Then
                        Anula = "Este ticket no se puede anular, ya que tiene más de " & dr("GruMinutosAnula") & " minutos de realizado."
                    Else
                        Dim stat As SqlCommand = New SqlCommand("Select EstatusDiaID,LoteriaID From HEstatusDias Where LoteriaID=0" & dr("LoteriaID") & " and EDiVentaCerrada='0' And EDiFecha<=GetDate() and EDiInicioVentaFecha<=Getdate() and EDiCierreVentaFecha>=Getdate() and GrupoID IN " &
                            "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & dr("BancaID") & ")", cn)
                        stat.CommandTimeout = gCommandTimeOut
                        dr.Close()
                        Dim DrStat As SqlDataReader = stat.ExecuteReader
                        If Not DrStat.Read Then
                            stat.CommandText = "Update HEstatusDias Set EDiVentaCerrada='1' Where EDiDiaCerrado='0' And EDiVentaCerrada='0' and EDiFecha<=GetDate() and EDiCierreVentaFecha<Getdate() And LoteriaID=0" & DrStat("LoteriaID") & " And GrupoID IN (Select GrupoID From MRiferos a INNER JOIN MBancas b ON A.RiferoID=B.RiferoID Where BancaID=" & Sesion.Banca & ")"
                            DrStat.Close()
                            stat.ExecuteNonQuery()
                            Anula = "Las ventas y anulaciones ya están cerradas por hoy."
                            Log(Sesion.Banca, "Anula", "Numero:" & TicketNo & ", Result:" & Anula)
                            Exit Function
                        Else
                            stat.CommandText = "Update HEstatusDias Set EDiVentaIniciada='1' Where EDiDiaCerrado='0' And EDiVentaIniciada='0' and EDiFecha<=GetDate() and EDiInicioVentaFecha<=Getdate() And  EDiCierreVentaFecha>=Getdate() And LoteriaID=0" & DrStat("LoteriaID") & " And GrupoID IN (Select GrupoID From MRiferos a INNER JOIN MBancas b ON A.RiferoID=B.RiferoID Where BancaID=" & Sesion.Banca & ")"
                            DrStat.Close()
                            stat.ExecuteNonQuery()
                        End If

                        cmd.CommandText = "Update t  Set TicNulo='1', TicCliente=CONVERT(varchar,getdate()) FROM DTickets as t JOIN MBancas AS B ON T.BancaID = B.BancaID  Where b.BanAnula = 1 AND TicNumero = '" & TicketNo & "'"
                        If cmd.ExecuteNonQuery > 0 Then
                            cmd.CommandText = "exec ActualizaListaDia @TicNum='" & TicketNo & "'"
                            If cmd.ExecuteNonQuery() > 0 Then
                                Anula = "OK"
                            End If
                        Else
                            Anula = "El ticket no se pudo anular."
                        End If
                    End If
                Else
                    Anula = "El ticket que desea anular no existe o ya fue anulado."
                End If
                dr.Close()
            Else
                Anula = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            End If
            Log(Sesion.Banca, "Anula", "Numero:" & TicketNo & ", Result:" & Anula)
        End Using
    End Function


    <WebMethod()> Public Function Anula2(ByVal Sesion As MAR_Session, ByVal TicketNo As String, ByVal Pin As String) As String
        Dim AnulaResult = String.Empty
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If ValidSesion(Sesion) Then
                Dim cmd As SqlCommand = New SqlCommand("Select a.*, b.gruminutosanula From DTickets a inner join tgrupos b ON A.GrupoID=b.GrupoID Where TicNulo='0' And TicNumero='" & TicketNo & "'", cn)
                cmd.CommandTimeout = gCommandTimeOut
                Dim dr As SqlDataReader = cmd.ExecuteReader
                If dr.Read Then
                    If DateDiff(DateInterval.Minute, dr("TicFecha"), Now) >= dr("GruMinutosAnula") Then
                        AnulaResult = "Este ticket no se puede anular, ya que tiene más de " & dr("GruMinutosAnula") & " minutos de realizado."
                    Else
                        Dim Solicitud = CInt(dr("TicSolicitud"))

                        Dim stat As SqlCommand = New SqlCommand("Select EstatusDiaID,LoteriaID From HEstatusDias Where LoteriaID=0" & dr("LoteriaID") & " and EDiVentaCerrada='0' And EDiFecha<=GetDate() and EDiInicioVentaFecha<=Getdate() and EDiCierreVentaFecha>=Getdate() and GrupoID IN " &
                            "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & dr("BancaID") & ")", cn)
                        stat.CommandTimeout = gCommandTimeOut

                        dr.Close()
                        Dim DrStat As SqlDataReader = stat.ExecuteReader
                        If Not DrStat.Read Then
                            stat.CommandText = "Update HEstatusDias Set EDiVentaCerrada='1' Where EDiDiaCerrado='0' And EDiVentaCerrada='0' and EDiFecha<=GetDate() and EDiCierreVentaFecha<Getdate() And LoteriaID=0" & DrStat("LoteriaID") & " And GrupoID IN (Select GrupoID From MRiferos a INNER JOIN MBancas b ON A.RiferoID=B.RiferoID Where BancaID=" & Sesion.Banca & ")"
                            DrStat.Close()
                            stat.ExecuteNonQuery()
                            AnulaResult = "Las ventas y anulaciones ya están cerradas por hoy."
                            Log(Sesion.Banca, "Anula", "Numero:" & TicketNo & ", Result:" & AnulaResult)
                            Return AnulaResult
                        Else
                            stat.CommandText = "Update HEstatusDias Set EDiVentaIniciada='1' Where EDiDiaCerrado='0' And EDiVentaIniciada='0' and EDiFecha<=GetDate() and EDiInicioVentaFecha<=Getdate() And  EDiCierreVentaFecha>=Getdate() And LoteriaID=0" & DrStat("LoteriaID") & " And GrupoID IN (Select GrupoID From MRiferos a INNER JOIN MBancas b ON A.RiferoID=B.RiferoID Where BancaID=" & Sesion.Banca & ")"
                            DrStat.Close()
                            stat.ExecuteNonQuery()
                        End If

                        If Not ComparaPinGanador(Solicitud, Pin) Then
                            AnulaResult = "El pin no corresponde con el ticket que usted desea anular."
                        Else
                            cmd.CommandText = "Update t  Set TicNulo='1', TicCliente=CONVERT(varchar,getdate()) FROM DTickets as t JOIN MBancas AS B ON T.BancaID = B.BancaID  Where b.BanAnula = 1 AND TicNumero = '" & TicketNo & "'"
                            If cmd.ExecuteNonQuery > 0 Then
                                cmd.CommandText = "exec ActualizaListaDia @TicNum='" & TicketNo & "'"
                                If cmd.ExecuteNonQuery() > 0 Then
                                    AnulaResult = "OK"
                                End If
                            Else
                                AnulaResult = "El ticket no se pudo anular."
                            End If
                        End If
                    End If
                Else
                    AnulaResult = "El ticket que desea anular no existe o ya fue anulado."
                End If
                dr.Close()
            Else
                AnulaResult = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
            End If
            Log(Sesion.Banca, "Anula", "Numero:" & TicketNo & ", Result:" & AnulaResult)

            Return AnulaResult
        End Using
    End Function

#End Region

#Region "Public WS Recargas"

    <WebMethod()>
    Public Function GetSuplidores(ByVal Sesion As MAR_Session) As MAR_Suplidor()
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim sups As New List(Of MAR_Suplidor)

            If Sesion.Banca = 0 Then
                Return sups.ToArray
            End If

            Dim da As New SqlDataAdapter("Select * from PMSuplidores Where SupActivo=1 Order by SuplidorID", cn)
            Dim dt As New DataTable
            da.Fill(dt)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    Dim sup As New MAR_Suplidor
                    sup.SuplidorID = rw("SuplidorID")
                    sup.SupNombre = rw("SupNombre")
                    sup.SupInstruccion = rw("SupInstrucciones")
                    sups.Add(sup)
                Next
            End If
            Return sups.ToArray
        End Using
    End Function

    <WebMethod()>
    Public Function GetUsuarios(ByVal Sesion As MAR_Session) As MAR_Usuario()
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim usrs As New List(Of MAR_Usuario)

            If Sesion.Banca = 0 Then
                Return usrs.ToArray
            End If

            Dim da As New SqlDataAdapter("Select * from MUsuarios u Inner Join RRiferosUsuarios ru ON u.UsuarioId=ru.UsuarioId" &
                                         "  Inner Join MBancas b ON b.RiferoId=ru.RiferoID Where u.UsuActivo=1 And b.BancaId=@banca And u.UsuNivel=20" &
                                         "  Order by u.UsuarioID", cn)
            da.SelectCommand.Parameters.AddWithValue("banca", Sesion.Banca)
            Dim dt As New DataTable
            da.Fill(dt)

            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                For Each rw As DataRow In dt.Rows
                    Dim usr As New MAR_Usuario
                    usr.UsuarioID = rw("UsuarioID")
                    usr.UsuNombre = String.Format("{0} {1}", rw("UsuNombre").ToString, rw("UsuApellido").ToString)
                    usrs.Add(usr)
                Next
            End If

            Return usrs.ToArray
        End Using
    End Function

    <WebMethod()>
    Public Function GetRecarga(ByVal sesion As MAR_Session, ByVal Usuario As Integer, ByVal Clave As String, ByVal Suplidor As Integer, ByVal Numero As String, ByVal Monto As Double, ByVal Solicitud As Integer) As MAR_Pin
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim pn As New MAR_Pin
            Try
                If Not ValidSesion(sesion) Then
                    pn.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
                    Return pn
                End If

                If Not MAR.BusinessLogic.Code.ProductosConfigLogic.GetRecargasEncendidas() Then
                    pn.Err = "La venta de recargas ha sido DESACTIVADA temporalmente."
                    Return pn
                End If


                Usuario = sesion.Usuario

                Dim daFails As New SqlDataAdapter("Select ISNULL(Count(*),0) as cnt,ISNULL(Min(PinFecha),GetDate()) as primer From PDPines Where PinNulo=1 And PinFecha>=DATEADD(mi,-5,GetDate())", cn)
                Dim dtFails As New DataTable
                daFails.Fill(dtFails)
                If dtFails IsNot Nothing AndAlso dtFails.Rows.Count > 0 Then
                    Dim IsDown As Boolean = (dtFails.Rows(0)("cnt") >= 25)
                    If IsDown Then
                        pn.Err = String.Format("El suplidor de recargas esta fallando. Por favor vuelva a intentar despues de las {0}.",
                                               FormatFecha(DateAdd(DateInterval.Minute, 6, dtFails.Rows(0)("primer")), 7))
                        Return pn
                    End If
                End If

                Dim maxAmount As Double = 490
                Dim TheConfig = DAL.ConfiguracionProductos.LeeProductoConfig(0, "TOP_MONTO_RECARGAS")
                If TheConfig IsNot Nothing Then
                    maxAmount = CDbl(TheConfig.Rows(0)("Valor"))
                End If

                If Monto > maxAmount Then
                    pn.Err = String.Format("El monto maximo permitido por recarga es de {0:C2}.", maxAmount)
                    Return pn
                End If

                Dim LotCierre = 2 'Quiniela-Palet
                Dim TheConfig2 = DAL.ConfiguracionProductos.LeeProductoConfig(0, "LOTERIA_CIERRE_RECARGAS")
                If TheConfig2 IsNot Nothing Then
                    LotCierre = CInt(TheConfig2.Rows(0)("Valor"))
                End If

                Dim cmdV As New SqlCommand("Select Count(*) From HEstatusDias Where LoteriaID=@LotCierre And EDiDiaCerrado='0' And EDiFecha<=GetDate() and EDiInicioVentaFecha<=getdate() and EDiCierreVentaFecha>=Getdate()", cn)
                cmdV.Parameters.AddWithValue("LotCierre", LotCierre)
                Dim IsOpen As Boolean = (cmdV.ExecuteScalar > 0)
                If Not IsOpen Then
                    pn.Err = "Las ventas de recargas estan cerradas."
                    Return pn
                End If

                'Dim dtUsr As New DataTable
                'Dim da As New SqlDataAdapter("Select * from MUsuarios Where UsuarioID=@usuario and UsuClave=@clave and UsuNivel=20", cn)
                'da.SelectCommand.Parameters.AddWithValue("usuario", Usuario)
                'da.SelectCommand.Parameters.AddWithValue("clave", Clave)
                'da.Fill(dtUsr)
                'If dtUsr Is Nothing OrElse dtUsr.Rows.Count = 0 Then
                '    pn.Err = "Su clave no es válida o su usuario ya no tiene permiso para vender recargas. Comuniquese con la central."
                '    Return pn
                'End If

                Dim dtTot As New DataTable
                Dim da As New SqlDataAdapter("Select ISNULL(Sum(PinCosto),0) As Vendido,ISNULL(Max(BanTerminalTarj),5000) As Limite from PDPines p, MBancas b Where p.BancaID=b.BancaID AND b.BancaID=@banca AND PinNulo=0", cn)
                da.SelectCommand.Parameters.AddWithValue("banca", sesion.Banca)
                da.Fill(dtTot)
                If dtTot Is Nothing OrElse dtTot.Rows.Count = 0 OrElse If(dtTot.Rows(0)(1) <= 0, 5000, dtTot.Rows(0)(1)) < (dtTot.Rows(0)(0) + Monto) Then
                    pn.Err = "El monto solicitado excederia el limite de recargas de esta banca por el dia de hoy."
                    Return pn
                End If

                Dim dtSup As New DataTable
                da = New SqlDataAdapter("Select * from PMSuplidores Where SuplidorID=@suplidor", cn)
                da.SelectCommand.Parameters.AddWithValue("suplidor", Suplidor)
                da.Fill(dtSup)
                If dtSup Is Nothing OrElse dtSup.Rows.Count = 0 Then
                    pn.Err = "Las recargas para el suplidor solicitado no estan disponibles. Comuniquese con la central."
                    Return pn
                End If



                '----------------------------------
                Dim dtPin As New DataTable
                da = New SqlDataAdapter("Select * from PDPines Where BancaID=@banca And PinNumero=@numero And PinSecuencia=@solicitud", cn)
                da.SelectCommand.Parameters.AddWithValue("banca", sesion.Banca)
                da.SelectCommand.Parameters.AddWithValue("numero", Numero)
                da.SelectCommand.Parameters.AddWithValue("solicitud", Solicitud)
                da.Fill(dtPin)
                If dtPin IsNot Nothing AndAlso dtPin.Rows.Count > 0 Then
                    pn.Err = "La recarga solicitada esta en proceso, consulte la lista de ventas en breve para validar si la recarga fue exitosa."
                    Return pn
                End If

                '----------------------------------

                Dim dtCue As New DataTable
                da = New SqlDataAdapter("Select * from PMCuenta Where CueActiva>0 AND RiferoID IS NOT NULL AND RiferoID=(Select RiferoID From MBancas Where BancaID=0" & sesion.Banca & ")", cn)
                da.Fill(dtCue)
                If dtCue Is Nothing OrElse dtCue.Rows.Count = 0 Then
                    da = New SqlDataAdapter("Select * from PMCuenta Where CueActiva>0 AND (RiferoID IS NULL OR RiferoID<=0)", cn)
                    da.Fill(dtCue)
                    If dtCue Is Nothing OrElse dtCue.Rows.Count = 0 Then
                        pn.Err = "Las credenciales no estan disponibles o son invalidas. Comuniquese con la central."
                        Return pn
                    End If
                End If

                Dim RECARGAUSER As String = dtCue.Rows(0)("CueComercio"),
                    RECARGAPWD As String = dtCue.Rows(0)("CueServidor"),
                    RECARGATIPO As Integer = dtCue.Rows(0)("CuePuerto"),
                    RECARGANOMBRE As String = dtCue.Rows(0)("CueNombre")

                cmdV = New SqlCommand("INSERT INTO PDPines " &
                                          "(SuplidorID,CuentaID,ProductoID,PinSerial,PinNumero,PinReferencia,PinCodigo,PinNulo,PinMensaje,PinSecuencia,PinFlag,BancaID,RiferoID,GrupoID,PinFecha,PinIPAddr,PinCosto,PinImpuesto,PinComision) Values " &
                                          "(@suplidor,@usuario,0,'',@numero,'','',1,'',@solicitud,'P',@banca,0,1,getdate(),@ipaddr,@costo,@impuesto,@comision); " &
                                          "SELECT SCOPE_IDENTITY()", cn)
                With cmdV
                    .Parameters.Clear()
                    .Parameters.AddWithValue("suplidor", Suplidor)
                    .Parameters.AddWithValue("usuario", Usuario)
                    .Parameters.AddWithValue("numero", Numero)
                    .Parameters.AddWithValue("solicitud", Solicitud)
                    .Parameters.AddWithValue("banca", sesion.Banca)
                    .Parameters.AddWithValue("ipaddr", GetRemoteIP())
                    .Parameters.AddWithValue("costo", Monto)
                    .Parameters.AddWithValue("impuesto", dtSup.Rows(0)("SupImpuesto"))
                    .Parameters.AddWithValue("comision", dtSup.Rows(0)("SupComision"))
                End With


                Dim pinID As Integer = cmdV.ExecuteScalar
                Dim ElMensaje As String = String.Empty, ElSerial As String = String.Empty, MsjHeader As String = String.Empty

                If pinID = 0 Then
                    pn.Err = "Ocurrio un error inesperado accediendo a la base de datos."
                    Return pn
                End If

                If RECARGATIPO = 300 Then
                    '--------------- Union Telecard --------------
                    Dim respList As New List(Of String)
                    Try
                        respList = MARRecargaLibs.UnionTelecard.Recharge(RECARGANOMBRE, RECARGAUSER, Numero,
                                                                              Monto, Solicitud, Suplidor)
                        MsjHeader = String.Format("RQ1OK-{0};", Now.ToString)
                    Catch ex As Exception
                        respList = New List(Of String)
                        MsjHeader = String.Format("RQ1Fail-{0};", Now.ToString)
                    End Try

                    Dim Reintentar = (respList.Count <> 3)

                    If Reintentar Then
                        Try
                            respList = MARRecargaLibs.UnionTelecard.Recharge(RECARGANOMBRE, RECARGAUSER, Numero,
                                                                              Monto, Solicitud, Suplidor + 1000)

                            MsjHeader = String.Format("{1}RQ2OK-{0};", Now.ToString, MsjHeader)
                        Catch ex As Exception
                            respList = New List(Of String)
                            MsjHeader = String.Format("{1}RQ2Fail-{0};", Now.ToString, MsjHeader)
                        End Try
                    End If
                    If respList.Count = 3 Then
                        ElMensaje = String.Format("{0}#{1}#{3}# # # #{2}",
                                             respList(0),
                                             If(respList(0) = "00", "ACP", "DEN"),
                                             respList(1),
                                             respList(2))
                    End If

                ElseIf RECARGATIPO = 200 Then
                    '-------------  MIDASCARD   --------------
                    Try
                        ElMensaje = MARRecargaLibs.MidasCard.Recharge(RECARGANOMBRE, RECARGAUSER, RECARGAPWD,
                                                                      Numero, Monto, sesion.Banca, pinID, Suplidor)
                        MsjHeader = String.Format("RQOK-{0};", Now.ToString)
                    Catch ex As Exception
                        ElMensaje = String.Empty
                        MsjHeader = String.Format("RQFail-{0};", Now.ToString)
                    End Try

                    Dim Confirmar = (ElMensaje IsNot Nothing AndAlso
                                     ElMensaje.Replace("@", "").Replace("%", "").Split("#"c).Count = 7 AndAlso
                                     ElMensaje.Replace("@", "").Replace("%", "").Split("#"c)(1) = "ACP")

                    If Confirmar Then
                        'Confirma recepcion
                        Dim ElMensajeConfirm = String.Empty
                        Try
                            ElMensajeConfirm = MARRecargaLibs.MidasCard.Recharge(RECARGANOMBRE, RECARGAUSER, RECARGAPWD,
                                                                                      Numero, Monto, sesion.Banca, pinID, 999)
                            MsjHeader = String.Format("{1}RQCFRM-{0};", Now.ToString, MsjHeader)
                        Catch ex As Exception
                            ElMensajeConfirm = String.Empty
                            MsjHeader = String.Format("{1}RQCFRMFail-{0};", Now.ToString, MsjHeader)
                        End Try

                        If ElMensajeConfirm Is Nothing OrElse ElMensajeConfirm.Trim.Length = 0 OrElse
                           ElMensajeConfirm.Replace("@", "").Replace("%", "").Split("#"c).Count <> 7 OrElse
                           (ElMensajeConfirm.Replace("@", "").Replace("%", "").Split("#"c).Count > 1 AndAlso
                            ElMensajeConfirm.Replace("@", "").Replace("%", "").Split("#"c)(1) <> "ACP") Then

                            Try
                                'Reintenta confirmar recepcion en 4 segundos
                                Thread.Sleep(3000)
                                ElMensajeConfirm = MARRecargaLibs.MidasCard.Recharge(RECARGANOMBRE, RECARGAUSER, RECARGAPWD,
                                                                                     Numero, Monto, sesion.Banca, pinID, 999)
                                MsjHeader = String.Format("{1}RQCFM2-{0};", Now.ToString, MsjHeader)
                            Catch ex As Exception
                                ElMensajeConfirm = String.Empty
                                MsjHeader = String.Format("{1}RQCFM2Fail-{0};", Now.ToString, MsjHeader)
                            End Try

                            If ElMensajeConfirm Is Nothing OrElse ElMensajeConfirm.Trim.Length = 0 OrElse
                               ElMensajeConfirm.Replace("@", "").Replace("%", "").Split("#"c).Count <> 7 OrElse
                               (ElMensajeConfirm.Replace("@", "").Replace("%", "").Split("#"c).Count > 1 AndAlso
                                ElMensajeConfirm.Replace("@", "").Replace("%", "").Split("#"c)(1) <> "ACP") Then

                                'Fallo confirmacion
                                MsjHeader = String.Format("{1}NOCONFIRM-{0};", ElMensajeConfirm, MsjHeader)
                                'ElMensaje = String.Empty

                            End If

                        End If

                    End If
                Else
                    '------------- MAX RECARGAS --------------
                    Try
                        '@01#ACP#Codigo Transacción#Número Movil#Monto#Fecha y Hora#Mensaje%
                        '@01#ACP#2010051100001006#8095558888#50#20100511085639# MAX RECARGAS: Transacción realizada exitosamente, gracias por utilizar nuestros servicios$

                        ElMensaje = MARRecargaLibs.MAXRecargas.Recharge(RECARGAUSER, RECARGAPWD, Numero,
                                                                        Monto, sesion.Banca, pinID, Suplidor)
                        MsjHeader = String.Format("RQ1OK-{0};", Now.ToString)
                    Catch EX As Exception
                        ElMensaje = String.Empty
                        MsjHeader = String.Format("RQ1Fail-{0};", Now.ToString)
                    End Try

                End If

                Dim respuesta() As String = ElMensaje.Replace("@", "").Replace("%", "").Split("#"c)
                If respuesta Is Nothing OrElse respuesta.Length <> 7 Then
                    cmdV = New SqlCommand("UPDATE PDPines " &
                                             "SET PinMensaje=@mensaje " &
                                             "Where PinId=@pin", cn)
                    With cmdV
                        .Parameters.Clear()
                        .Parameters.AddWithValue("mensaje", MsjHeader)
                        .Parameters.AddWithValue("pin", pinID)
                        .ExecuteNonQuery()
                    End With

                    ReverseRecarga("ADMINRECARGASOURCECODE125487", pinID)

                    pn.Err = "El suplidor de recargas no esa disponible en estos momentos."
                    Return pn
                End If

                If respuesta(1) = "ACP" Then 'Transaccion exitosa
                    pn.Pin = pinID
                    pn.Flag = "C"
                    pn.Costo = CDbl("0" & respuesta(4).Trim)
                    pn.Numero = respuesta(3)
                    pn.Serie = respuesta(2)
                    pn.StrFecha = FormatFecha(Now, 1)
                    pn.StrHora = FormatFecha(Now, 8)
                    pn.Nulo = False
                Else
                    'MIDASCARD: Si la recarga fue denegada, envia reverso para por si las moscas.
                    pn.Flag = If(RECARGATIPO = 200, "P", "F")
                    pn.Err = respuesta(6)
                    pn.Nulo = True
                End If

                ElSerial = respuesta(2)


                cmdV = New SqlCommand("UPDATE PDPines " &
                                         "SET PinSerial=@serial,PinMensaje=@mensaje,PinFlag=@flag,PinNulo=@nulo " &
                                         "Where PinId=@pin", cn)
                With cmdV
                    .Parameters.Clear()
                    .Parameters.AddWithValue("serial", ElSerial)
                    .Parameters.AddWithValue("mensaje", String.Format("{0} {1}", MsjHeader, ElMensaje))
                    .Parameters.AddWithValue("flag", pn.Flag)
                    .Parameters.AddWithValue("pin", pinID)
                    .Parameters.AddWithValue("nulo", If(pn.Nulo, 1, 0))
                End With


                cmdV.ExecuteNonQuery()

            Catch ex As Exception
                pn.Err = ex.ToString
            End Try

            Return pn
        End Using
    End Function

    <WebMethod()>
    Public Sub ConfirmRecarga(ByVal Sesion As MAR_Session)
        'Ya no es necesario, ahora se vuelven activos al setear flag='C'
        'If Not ValidSesion(Sesion, True) Then Exit Sub
        'Using cn As New SqlConnection(StrSQLCon)
        '    cn.Open()
        '    If Sesion.LastPin = 0 Then Exit Sub
        '    Dim cmd As SqlCommand = New SqlCommand("Update PDPines Set PinNulo='0' Where PinID=@LP and PinNulo='1' and PinFlag='C' and BancaID=@BAN", cn)
        '    cmd.Parameters.AddWithValue("LP", Sesion.LastPin)
        '    cmd.Parameters.AddWithValue("BAN", Sesion.Banca)
        '    cmd.CommandTimeout = gCommandTimeOut
        '    If cmd.ExecuteNonQuery > 0 Then
        '        Log(Sesion.Banca, "Pin", "Recepcion confirmada PinID:" & Sesion.LastPin)
        '    End If
        'End Using
    End Sub

    <WebMethod()>
    Public Function ReverseRecarga(ByVal Source As String, ByVal Solicitud As Integer) As String
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If Source = "ADMINRECARGASOURCECODE125487" Then

                Dim dtPin As New DataTable
                Dim da = New SqlDataAdapter("Select * from PDPines Where PinID=@pin", cn)
                da.SelectCommand.Parameters.AddWithValue("pin", Solicitud)
                da.Fill(dtPin)
                If dtPin.Rows.Count = 0 Then
                    Return "La recarga seleccionada no puede ser localizada."
                End If

                Dim dtCue As New DataTable
                da = New SqlDataAdapter("Select * from PMCuenta Where CueActiva>0 AND RiferoID IS NOT NULL AND RiferoID=(Select RiferoID From MBancas Where BancaID=0" & dtPin.Rows(0)("BancaID") & ")", cn)
                da.Fill(dtCue)
                If dtCue Is Nothing OrElse dtCue.Rows.Count = 0 Then
                    da = New SqlDataAdapter("Select * from PMCuenta Where CueActiva>0 AND (RiferoID IS NULL OR RiferoID<=0)", cn)
                    da.Fill(dtCue)
                    If dtCue Is Nothing OrElse dtCue.Rows.Count = 0 Then
                        Return "Las credenciales no estan disponibles o son invalidas. Comuniquese con la central."
                    End If
                End If

                Dim RECARGAUSER As String = dtCue.Rows(0)("CueComercio"),
                    RECARGAPWD As String = dtCue.Rows(0)("CueServidor"),
                    RECARGATIPO As Integer = dtCue.Rows(0)("CuePuerto"),
                    RECARGANOMBRE As String = dtCue.Rows(0)("CueNombre")

                Dim resp As String = String.Empty, MsjHeader As String = String.Empty
                Try
                    If RECARGATIPO = 300 Then 'Union Telecard
                        Dim respList = MARRecargaLibs.UnionTelecard.Reversar(RECARGANOMBRE, RECARGAUSER, dtPin.Rows(0)("PinNumero"),
                                                                             dtPin.Rows(0)("PinSerial"), Solicitud, dtPin.Rows(0)("SuplidorID"))
                        If respList.Count = 3 Then
                            resp = String.Format("{0}#{1}# # # #{3}#{2}",
                                                 respList(0),
                                                 If(respList(0) = "00", "ACP", "DEN"),
                                                 respList(1),
                                                 respList(2))
                        End If
                    ElseIf RECARGATIPO = 200 Then 'MIDASCARD
                        resp = MARRecargaLibs.MidasCard.Reversar(RECARGAUSER, RECARGAPWD, dtPin.Rows(0)("BancaID"), Solicitud, RECARGANOMBRE)
                    Else
                        resp = MARRecargaLibs.MAXRecargas.Reversar(RECARGAUSER, RECARGAPWD, dtPin.Rows(0)("BancaID"), Solicitud, dtPin.Rows(0)("SuplidorID"))
                    End If
                    MsjHeader = String.Format("ReversoOK-{0};", Now.ToString)

                Catch EX As Exception
                End Try

                If resp IsNot Nothing AndAlso Not resp.Contains("@") AndAlso resp.Length > 0 Then
                    Return resp
                End If


                Dim respuesta() As String = resp.Replace("@", "").Replace("%", "").Split("#"c)
                If respuesta Is Nothing OrElse respuesta.Length <> 7 Then
                    MsjHeader = String.Format("ReversoFail-{0};", Now.ToString)
                    Dim NewResp1 = Left(String.Format(" + {0} {1}", MsjHeader, resp), 500).Trim

                    Dim cmdV As New SqlCommand("UPDATE PDPines " &
                                                 "SET PinMensaje=LEFT(RTRIM(PinMensaje)+@msj,500),PinReferencia='', " &
                                                 "PinFlag=Case When (LEN(RTRIM(PinMensaje)+@msj)>80 OR PinFecha<DateAdd(s,-119,Getdate())) AND PinFlag='P' Then 'F' Else PinFlag End " &
                                                 "Where PinFlag<>'R' AND PinId=@pin", cn)
                    With cmdV
                        .Parameters.AddWithValue("pin", Solicitud)
                        .Parameters.AddWithValue("msj", NewResp1)
                        .ExecuteNonQuery()
                    End With

                    Return "El suplidor de recargas no esta disponible en estos momentos."
                End If


                Dim NewResp2 = Left(String.Format(" + {0} {1}", MsjHeader, resp), 500)
                If respuesta(1) = "ACP" Then

                    Dim cmdV As New SqlCommand("UPDATE PDPines " &
                                                 "SET PinNulo=1,PinFlag='R',PinMensaje=LEFT(RTRIM(PinMensaje)+@msj,500),PinReferencia='' " &
                                                 "Where PinId=@pin", cn)
                    With cmdV
                        .Parameters.AddWithValue("pin", Solicitud)
                        .Parameters.AddWithValue("msj", NewResp2)
                        .ExecuteNonQuery()
                    End With

                Else
                    Dim cmdV As New SqlCommand("UPDATE PDPines " &
                                                 "SET PinMensaje=LEFT(RTRIM(PinMensaje)+@msj,500),PinReferencia='', " &
                                                 "PinFlag=Case When (LEN(RTRIM(PinMensaje)+@msj)>150 OR PinFecha<DateAdd(s,-119,Getdate())) AND PinFlag='P' Then 'F' Else PinFlag End " &
                                                 "Where PinFlag<>'R' AND PinId=@pin", cn)
                    With cmdV
                        .Parameters.AddWithValue("pin", Solicitud)
                        .Parameters.AddWithValue("msj", NewResp2)

                        .ExecuteNonQuery()

                    End With

                End If

                Return respuesta(6)

            Else
                Return String.Empty
            End If
        End Using
    End Function

    <WebMethod()>
    Public Function BalanceRecarga(ByVal Source As String, ByVal Tipo As String) As String
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()

            If Source = "ADMINRECARGASOURCECODE125487" Then

                Dim dtCue As New DataTable
                Dim da = New SqlDataAdapter("Select c.*, 'Cuenta General' as Rifero" &
                                            " from PMCuenta c" &
                                            " Where c.CueActiva>0 AND ISNULL(c.RiferoID,0)<=0", cn)
                da.Fill(dtCue)
                If dtCue Is Nothing OrElse dtCue.Rows.Count = 0 Then
                    Return "Las credenciales no estan disponibles o son invalidas. Comuniquese con la central."
                End If

                Dim sb As New StringBuilder
                For Each iRow In dtCue.Rows
                    sb.Append(GetBalanceByCuenta(iRow, Tipo))
                Next

                Return sb.ToString
            Else
                Return String.Empty
            End If
        End Using

        ReversaPinsPendientes()
    End Function

    <WebMethod()>
    Public Function BalanceRecargaRifero(ByVal Source As String, ByVal Tipo As String, ByVal pRiferoID As Integer) As String
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()

            If Source = "ADMINRECARGASOURCECODE125487" Then

                Dim dtCue As New DataTable
                Dim da = New SqlDataAdapter("Select c.*, 'Cuenta '+ISNull(r.RifNombre,'Rifero') as Rifero" &
                                            " from PMCuenta c INNER JOIN MRiferos r ON r.RiferoID=c.RiferoID" &
                                            " Where c.CueActiva>0 AND c.RiferoID=0" & pRiferoID, cn)
                da.Fill(dtCue)
                If dtCue Is Nothing OrElse dtCue.Rows.Count = 0 Then
                    Return "Las credenciales no estan disponibles o son invalidas. Comuniquese con la central."
                End If

                Dim sb As New StringBuilder
                For Each iRow In dtCue.Rows
                    sb.Append(GetBalanceByCuenta(iRow, Tipo))
                Next

                Return sb.ToString
            Else
                Return String.Empty
            End If
        End Using

        ReversaPinsPendientes()
    End Function

    Private Function GetBalanceByCuenta(ByVal iRow As DataRow, ByVal Tipo As String) As String

        Dim RECARGAUSER As String = iRow("CueComercio"),
            RECARGAPWD As String = iRow("CueServidor"),
            RECARGATIPO As Integer = iRow("CuePuerto"),
            RECARGANOMBRE As String = iRow("CueNombre")

        Dim resp As String = String.Empty, respEncendido As String = String.Empty

        If Not MAR.BusinessLogic.Code.ProductosConfigLogic.GetRecargasEncendidas() Then
            respEncendido = "Venta de Recargas DESACTIVADA en pagina de Configuracion. "
        End If

        Try
            If RECARGATIPO = 300 Then
                If Tipo.ToUpper = "MAX" Then
                    resp = " # # # # # # "
                Else
                    resp = MARRecargaLibs.UnionTelecard.Balance(RECARGANOMBRE)
                End If
            ElseIf RECARGATIPO = 200 Then
                If Tipo.ToUpper = "MAX" Then
                    resp = " # # # # # # "
                Else
                    resp = MARRecargaLibs.MidasCard.Balance(RECARGANOMBRE, RECARGAUSER, RECARGAPWD)
                End If
            Else
                If Tipo.ToUpper = "MAX" Then
                    resp = RecargasLibrary.Recargas.GetMaxRecargasBalance(RECARGAUSER, RECARGAPWD)
                Else
                    resp = RecargasLibrary.Recargas.GetTusRecargasBalance(RECARGAUSER, RECARGAPWD)
                End If
            End If
        Catch EX As Exception
        End Try

        Dim respuesta() As String = resp.Replace("@", "").Replace("%", "").Split("#"c)
        If respuesta Is Nothing OrElse respuesta.Length <> 7 Then
            Return String.Format("{0}: El suplidor de recargas no esta disponible en estos momentos.<br />{1}{2}", iRow("Rifero"), respEncendido, If(respEncendido.Length > 0, "<br />", ""))
        Else
            Return String.Format("{0}: {1}<br />{2}{3}", iRow("Rifero"), respuesta(6), respEncendido, If(respEncendido.Length > 0, "<br />", ""))
        End If
    End Function

    Private Function BuildTicketFooter(ByVal pSesion As MAR_Session, ByVal pTicket As MAR_Bet) As String
        Dim lineas As New List(Of String)

        'Genera mensaje jackpot aqui 

        If pSesion.Err = "POS" Then
            'Genera firma para POS
            lineas.Add(String.Format("Firma: {0}", GeneraTicFirma(pTicket.StrFecha, pTicket.StrHora, pSesion.Banca, pTicket.TicketNo, pTicket.Items)))
        End If

        Return String.Join("|", lineas.ToArray)
    End Function

    Friend Function GeneraTicFirma(ByVal pTicFecha As String, ByVal pTicHora As String,
                                ByVal pBanca As Integer, ByVal pTicNumero As String,
                                ByVal pJugadas As MAR_BetItem()) As String
        Try
            Dim CrosswalkSrc = "*0#Q*1#V*2#C*3#0*4#H*5#5*6#M*7#R*8#W*9#D*10#1*11#6*12#N*13#S*14#X*15#E*16#2*17#J*18#7*19#T*20#A*21#Y*22#F*23#3*24#K*25#8*26#P*27#U*28#B*29#L*30#G*31#4*32#9"
            Dim TheCrosswalk = (From p In CrosswalkSrc.Split("*"c).ToList
                                Where p.Length > 0
                                Select Key = CInt(p.Split("#"c)(0)),
                                      Value = p.Split("#"c)(1)) _
                                .ToDictionary(Function(x) x.Key, Function(y) y.Value)

            Dim rdn = (New Random(Now.Millisecond)).Next(31) + 1
            Dim Cadena = String.Format("{0}{1}{2}{3}{4}21", pTicFecha, pTicHora, pTicNumero, pBanca, rdn)
            Dim acum1 As Long = 0
            For i As Integer = 1 To Cadena.Length()
                acum1 += (i * Asc(Mid(Cadena, i, 1)))
            Next

            Dim Source = (acum1 Mod 100000).ToString.PadLeft(5, "0")

            acum1 = 0
            For j As Integer = 1 To pJugadas.Count
                Cadena = String.Format("{0}{1}{2}3", pJugadas(j - 1).Numero,
                                         rdn,
                                         CInt(Math.Ceiling(pJugadas(j - 1).Cantidad)))
                For i As Integer = 1 To Cadena.Length()
                    acum1 += Asc(Mid(Cadena, i, 1))
                Next
            Next

            Source = String.Format("{0}-{1}", Source, (acum1 Mod 10000000).ToString.PadLeft(7, "1"))

            GeneraTicFirma = String.Empty
            Dim CNT As Integer = 1, Target As String = String.Empty, iSrc As String, iSrcNo As Integer

            Do While CNT <= 13
                iSrc = Mid(Source, CNT, 1)
                If CNT = 6 Then
                    iSrcNo = rdn
                Else
                    iSrcNo = (From e In TheCrosswalk Where e.Value = iSrc Select e.Key).FirstOrDefault
                    iSrcNo = (iSrcNo + rdn + CNT) Mod 33
                End If
                CNT += 1
                GeneraTicFirma &= TheCrosswalk(iSrcNo)
            Loop

        Catch ex As Exception
            Return "- No disponible -"
        End Try
    End Function

    Private Sub ReversaPinsPendientes()
        System.Threading.ThreadPool.QueueUserWorkItem(AddressOf ReversaPinsPendientesAsync)
    End Sub

    Private Sub ReversaPinsPendientesAsync(state As Object)

        Dim dtPins As New DataTable
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim da = New SqlDataAdapter("Update PDPines Set PinReferencia='Reversando' " &
                                        "Where PinReferencia='' And PinFlag='P' And PinFecha between DateAdd(s,-800,Getdate()) and DateAdd(s,-120,Getdate()); " &
                                        "IF @@ROWCOUNT>0 Select PinID from PDPines Where PinReferencia='Reversando' " &
                                        "ELSE Select PinID from PDPines where 1=0", cn)
            da.Fill(dtPins)
        End Using

        For Each pin As DataRow In dtPins.Rows
            Try
                ReverseRecarga("ADMINRECARGASOURCECODE125487", pin("PinID"))
            Catch ex As Exception
            End Try
        Next

    End Sub

    '<WebMethod()> Public Function RePrintPin(ByVal Sesion As MAR_Session, ByVal Serie As String) As MAR_Pin
    '    Dim CronoInit As DateTime = Now()
    '    RePrintPin = New MAR_Pin
    '    cn.Open()
    '    If ValidSesion(Sesion) Then
    '        RePrintPin = GetRepin(Serie, Sesion.Banca)
    '        
    '        Exit Function
    '    Else
    '        RePrintPin.Err = "No puede reimprimir la tarjeta indicada."
    '    End If
    'End Function

    '<WebMethod()> Public Function GetPin(ByVal Sesion As MAR_Session, ByVal Producto As MAR_Producto, ByVal Solicitud As Double) As MAR_Pin
    '    Dim CronoInit As DateTime = Now()
    '    GetPin = New MAR_Pin
    '    cn.Open()
    '    If ValidSesion(Sesion) Then
    '        Try
    '            Dim stat As SqlCommand = New SqlCommand("Select EstatusDiaID From HEstatusDias Where EDiDiaCerrado='0' And EDiFecha<=GetDate() and EDiInicioVentaFecha<=getdate() and EDiCierreVentaFecha>=Getdate() and GrupoID IN " & _
    '                "(Select GrupoID From MRiferos A INNER JOIN MBancas B ON A.RiferoID=B.RiferoID Where B.BancaID=0" & Sesion.Banca & ")", cn)
    '            Dim DrStat As SqlDataReader = stat.ExecuteReader
    '            stat.CommandTimeout = gCommandTimeOut
    '            If Not DrStat.Read Then
    '                DrStat.Close()
    '                GetPin.Err = "Las ventas de tarjetas han sido cerradas por el día de hoy."
    '                Exit Function
    '            End If

    '            stat.CommandText = "Select SupNombre,ProNombre,ProPrecio,SupImpuesto,SupComision from PMSuplidores a INNER JOIN PMProductos b ON a.SuplidorID=b.SuplidorID and SupActivo=1 and ProActivo=1 Where ProductoID=0" & Producto.ProID & " and a.SuplidorID=0" & Producto.SupID
    '            DrStat.Close()
    '            DrStat = stat.ExecuteReader
    '            If Not DrStat.Read Then
    '                DrStat.Close()
    '                GetPin.Err = "La tarjeta que usted esta solicitando ya no esta disponible." & stat.CommandText
    '                Exit Function
    '            End If
    '            Dim pnCom, pnImp As Double
    '            pnCom = DrStat("SupComision")
    '            pnImp = DrStat("SupImpuesto")
    '            Producto.Precio = DrStat("ProPrecio")
    '            Producto.Producto = DrStat("ProNombre")
    '            Producto.Suplidor = DrStat("SupNombre")
    '            DrStat.Close()
    '            GetPin.Costo = Producto.Precio
    '            GetPin.Producto = Producto
    '            GetPin.Solicitud = Solicitud
    '            GetPin.Flag = "S"

    '            '****** Confirma Duplicidad ******
    '            Dim dr2 As SqlDataReader, cmd2 As SqlCommand, OldPin As String
    '            cmd2 = New SqlCommand("Select PinSerial,PinFlag,PinNulo From PDPines Where BancaID=0" & Sesion.Banca & " and PinSecuencia=0" & Solicitud & " Order by PinFlag", cn)
    '            cmd2.CommandTimeout = gCommandTimeOut
    '            dr2 = cmd2.ExecuteReader
    '            If dr2.Read Then 'Ya existe el pin.
    '                OldPin = dr2("PinSerial")
    '                If dr2("PinFlag") = "C" Then 'Buscalo en base de datos
    '                    dr2.Close()
    '                    GetPin = New MAR_Pin
    '                    GetPin = GetRepin(OldPin, Sesion.Banca)
    '                    
    '                    Exit Function
    '                Else 'Buscalo en SunCard
    '                    GetPin.Flag = "R"
    '                End If
    '            End If
    '            dr2.Close()

    '            '************************************

    '            Dim cmd As SqlCommand = New SqlCommand("Select A.GrupoID, A.RiferoID, B.BancaID From MRiferos A Inner Join MBancas B ON A.RiferoID=B.RiferoID And B.BancaID=0" & Sesion.Banca, cn)
    '            cmd.CommandTimeout = gCommandTimeOut
    '            'TrscTicket = cn.BeginTransaction(IsolationLevel.ReadCommitted, "GrabaTicket")
    '            'cmd.Transaction = TrscTicket
    '            Dim dr As SqlDataReader = cmd.ExecuteReader
    '            dr.Read()
    '            cmd.CommandText = "Insert into PDPines (GrupoID,RiferoID,BancaID,PinFecha,PinFlag,PinSecuencia,PinIPAddr,PinNulo,SuplidorID,ProductoID,PinCosto,PinImpuesto,PinComision) Values (" & _
    '                dr("GrupoID") & "," & dr("RiferoID") & "," & Sesion.Banca & ",GetDate(),'" & IIf(GetPin.Err = "", "", GetPin.Err) & "'," & Solicitud & ",'" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR") & "',1," & GetPin.Producto.SupID & "," & GetPin.Producto.ProID & ",0" & GetPin.Costo & ",0" & pnImp & ",0" & pnCom & ")"
    '            dr.Close()
    '            cmd.ExecuteNonQuery()
    '            cmd.CommandText = "Select @@Identity"
    '            cmd.CommandText = "Select PinID,PinFecha From PDPines Where PinID=0" & cmd.ExecuteScalar
    '            dr = cmd.ExecuteReader
    '            dr.Read()
    '            GetPin.StrFecha = FormatFecha(dr("PinFecha"), 1)
    '            GetPin.StrHora = FormatFecha(dr("PinFecha"), 7)
    '            GetPin.Pin = dr("PinID")
    '            GetPin.Nulo = False
    '            dr.Close()
    '            GeneraPin(Sesion, GetPin)
    '            'TrscTicket.Commit()
    '            'PlaceBet = Apuesta
    '            cmd.CommandText = "Update MBancas Set BanLastContact=getdate() Where BancaID=0" & Sesion.Banca
    '            cmd.ExecuteNonQuery()
    '        Catch ex As System.Exception
    '            GetPin.Err = "La tarjeta no pudo ser procesada adecuadamente. Solicite soporte técnico." & Chr(13) & Chr(13) & "Mensaje: " & ex.ToString
    '        End Try
    '    Else
    '        GetPin.Err = "Su sesión de trabajo no es válida o fue terminada por otra persona. Salga del sistema y entre nuevamente."
    '    End If
    '    If GetPin.Err <> "" Then
    '        Log(Sesion.Banca, "Fallo Pin", "Sol:" & GetPin.Solicitud & ", Err:" & GetPin.Err)
    '    Else
    '        Log(Sesion.Banca, "Pin", "OK " & GetPin.Serie)
    '    End If
    '    
    'End Function

    'Private Function GetRepin(ByVal Serie As String, ByVal Banca As Integer) As MAR_Pin
    '    GetRepin = New MAR_Pin
    '    Try
    '        Dim dr As SqlDataReader, cmd As SqlCommand = New SqlCommand("Select * From PVPines Where BancaID=0" & Banca & " and PinNulo=0 and PinSerial='" & Serie & "'", cn)
    '        cmd.CommandTimeout = gCommandTimeOut
    '        dr = cmd.ExecuteReader
    '        If dr.Read Then
    '            GetRepin.Pin = dr("PinID")
    '            GetRepin.Costo = dr("PinCosto")
    '            GetRepin.Numero = dr("PinNumero")
    '            GetRepin.Serie = dr("PinSerial")
    '            GetRepin.Solicitud = dr("PinSecuencia")
    '            GetRepin.StrFecha = FormatFecha(dr("PinFecha"), 1)
    '            GetRepin.StrHora = FormatFecha(dr("PinFecha"), 7)
    '            GetRepin.Flag = dr("PinFlag")
    '            GetRepin.Nulo = dr("PinNulo")
    '            GetRepin.Producto = New MAR_Producto
    '            GetRepin.Producto.Precio = dr("ProPrecio")
    '            GetRepin.Producto.Producto = dr("ProNombre")
    '            GetRepin.Producto.Suplidor = dr("SupNombre")
    '            GetRepin.Producto.ProID = dr("ProductoID")
    '            GetRepin.Producto.SupID = dr("SuplidorID")
    '        Else
    '            GetRepin.Err = "No se encontro el Pin solicitado."
    '        End If
    '    Catch ex As Exception
    '        GetRepin.Err = ex.ToString
    '    End Try
    'End Function

    'Private Sub GeneraPin(ByVal sesion As MAR_Session, ByRef pn As MAR_Pin)
    '    'Dim sk As Socket, handshake(200) As Char, leyo As Integer, TraEnvio As String, TraRecibio As String, TranID As Integer = 0, Secuen As Integer
    '    'Secuen = pn.Solicitud
    '    Dim DaCMD As SqlDataAdapter = New SqlDataAdapter("Select Top 1 a.*,b.*,a.CuentaID as Cta,c.BanTerminalTarj as BanTarjetaTerminal,c.BanSerieTarj From PMCuenta a, PDPines b INNER JOIN MBancas c ON b.BancaID=c.BancaID Where PinID=0" & pn.Pin & " and CueActiva=1", cn), dt As DataTable = New DataTable
    '    DaCMD.SelectCommand.CommandTimeout = gCommandTimeOut
    '    DaCMD.FillSchema(dt, SchemaType.Source)
    '    DaCMD.Fill(dt)
    '    Dim dr As DataRow
    '    If dt.Rows.Count > 0 Then
    '        dr = dt.Rows(0)
    '        Try
    '            pin = New RecargasLibrary.Recargas
    '            Select Case pn.Producto.SupID
    '                Case 1 'Claro
    '                    pin.RechargeClaroPhone(dr("CueNombre").ToString,dr("CueComercio").ToString, pn.Numero, pn.Costo,sesion.Banca, pn.
    '                Case 2 'Orange
    '                Case 3 'Viva
    '                Case 4 'Tricom
    '                Case Else
    '            End Select
    '            Dim res = pin

    '            Dim temp As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
    '            temp.Blocking = True
    '            temp.Connect(New IPEndPoint(Dns.GetHostEntry(dr("CueServidor").ToString).AddressList(0), dr("CuePuerto")))
    '            temp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000)
    '            temp.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000)
    '            sk = temp
    '        Catch ex As Exception
    '            'myLog.WriteEntry("Fallo: No se pudo establecer la conexion al servidor " & servidor & " puerto " & puerto & ": ", EventLogEntryType.Warning)
    '            pn.Err = "El servidor de la banca no pudo conectarse al servidor del suplidor de tarjetas."
    '            Exit Sub
    '        End Try
    '        Dim Stream As NetworkStream = New NetworkStream(sk)
    '        Dim reader As StreamReader = New StreamReader(Stream)
    '        Dim writer As StreamWriter = New StreamWriter(Stream)
    '        'Conectado


    '        TraEnvio = dr("SuplidorID") & "," & dr("CueComercio") & "," & dr("BanTarjetaTerminal") & "," & dr("BanSerieTarj") & "," & dr("ProductoID") & "," & Secuen & "," & pn.Flag
    '        writer.Write(TraEnvio)
    '        writer.Flush()
    '        Log(0, "PinTransac", "PinID:" & pn.Pin & " Envio:" & TraEnvio)
    '        leyo = reader.Read(handshake, 0, 200)
    '        TraRecibio = New String(handshake)
    '        If leyo > 0 Then
    '            Log(0, "PinTransac", "PinID:" & pn.Pin & " Recibio:" & TraRecibio)
    '            Dim Pin() As String
    '            Pin = TraRecibio.Split(","c)
    '            Dim TrCMD As SqlCommand = New SqlCommand("Update PDPines Set CuentaID=0" & dr("Cta") & ", PinSerial='" & Pin(5) & "',  PinNumero='" & Pin(4) & "', PinReferencia='" & Pin(6) & "', PinCodigo='" & Pin(9) & "', PinMensaje='" & Pin(10) & "' Where PinID=0" & pn.Pin, cn)
    '            TrCMD.CommandTimeout = gCommandTimeOut
    '            pn.Serie = Pin(5)
    '            TrCMD.ExecuteNonQuery()
    '            If Pin(9) = "00" Then 'Tenemos un pin valido
    '                Try
    '                    pn.Numero = Pin(4)
    '                    TraEnvio = Strings.Left(TraEnvio, Len(TraEnvio) - 1) & "C"
    '                    writer.Write(TraEnvio)
    '                    writer.Flush()
    '                    Log(0, "PinTransac", "PinID:" & pn.Pin & " Confirmo:" & TraEnvio)
    '                    'leyo = reader.Read(handshake, 0, 200)
    '                Catch ex As Exception
    '                    'No se pudo confirmar la venta.
    '                End Try
    '            Else
    '                pn.Err = Pin(10)
    '            End If
    '            TrCMD.CommandText = "Update PDPines Set PinFlag='C' Where PinID=0" & pn.Pin
    '            TrCMD.ExecuteNonQuery()
    '        Else
    '            Log(0, "PinTransac", "PinID:" & pn.Pin & " No Responde")
    '        End If
    '        sk.Close()
    '    End If
    'End Sub

    '<WebMethod()> Public Sub ConfirmPin(ByVal Sesion As MAR_Session)
    '    Dim LocalCall As Boolean = (cn.State = ConnectionState.Open)
    '    If Not LocalCall Then
    '        cn.Open()
    '        If Not ValidSesion(Sesion, True) Then
    '            
    '            Exit Sub
    '        End If
    '    End If
    '    If Sesion.LastPin = 0 Then Exit Sub
    '    Dim cmd As SqlCommand = New SqlCommand("Update PDPines Set PinNulo='0' Where PinID=0" & Sesion.LastPin & " and PinNulo='1' and BancaID=0" & Sesion.Banca, cn)
    '    cmd.CommandTimeout = gCommandTimeOut
    '    If cmd.ExecuteNonQuery > 0 Then
    '        Log(Sesion.Banca, "Pin", "Recepcion confirmada PinID:" & Sesion.LastPin)
    '    End If
    '    If Not LocalCall Then 
    'End Sub

#End Region

#Region "Private General"

    Private Function ValidSesion(ByRef Sesion As MAR_Session, Optional ByVal ConfirmCall As Boolean = False) As Boolean
        Dim TicketReciente As Boolean = False
        If Sesion IsNot Nothing AndAlso Sesion.Err IsNot Nothing AndAlso Sesion.Err.Contains(".") Then
            'Android Web Client remote ip
            _RemoteIP = Sesion.Err
            Sesion.Err = String.Empty
        End If

        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            If Sesion.Banca = -1 And Sesion.Sesion = (Today.DayOfWeek * Today.DayOfYear) Then
                Dim cmd As SqlCommand = New SqlCommand("Update HDestinos Set DesAlive=getdate() where DesInicio>='" & FormatFecha(Today, 1) & "' " &
                    " And DesIPAddress='" & GetRemoteIP() & "'", cn)
                cmd.CommandTimeout = gCommandTimeOut
                If cmd.ExecuteNonQuery <= 0 Then
                    cmd.CommandText = "Insert into HDestinos (DesIPAddress) values ('" & GetRemoteIP() & "')"
                    cmd.ExecuteNonQuery()
                End If
                ValidSesion = True
            Else
                Dim cmd As SqlCommand = New SqlCommand("Select BancaID From MBancas" &
                    " Where BanUsuarioActual=@USR And BanSesionActual=@SES" &
                    " AND BancaID=@BAN", cn)
                cmd.CommandTimeout = gCommandTimeOut
                cmd.Parameters.AddWithValue("USR", Sesion.Usuario)
                cmd.Parameters.AddWithValue("SES", Sesion.Sesion)
                cmd.Parameters.AddWithValue("BAN", Sesion.Banca)
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                ValidSesion = dr.Read
                dr.Close()
            End If

            If Not ConfirmCall AndAlso ValidSesion Then
                Dim cmd As SqlCommand = New SqlCommand("Select TicketID From DTickets" &
                                                      " Where TicketID=@TK AND TicNulo=1 AND TicCliente='' AND TicFecha>DATEADD(s,-30,GETDATE())", cn)
                cmd.CommandTimeout = gCommandTimeOut
                cmd.Parameters.AddWithValue("TK", Sesion.LastTck)
                Dim dr As SqlDataReader = cmd.ExecuteReader()
                TicketReciente = dr.Read
                dr.Close()
            End If
        End Using

        If TicketReciente Then ConfirmTck(Sesion)

        If Not ValidSesion Then Log(Sesion.Banca, "Sesion", "Se intento accesar con una sesion invalidad")

    End Function

    Public Shared Function FormatFecha(ByVal Fecha As Date, ByVal Formato As Integer) As String
        Select Case Formato
            Case 1 'Fecha corta universal 2002-12-31
                FormatFecha = Year(Fecha) & "-" & Month(Fecha) & "-" & Fecha.Day
            Case 2 'Fecha y hora universal 2002-12-31 11:30:00 pm
                FormatFecha = Year(Fecha) & "-" & Month(Fecha) & "-" & Fecha.Day & " " & Fecha.ToShortTimeString.Replace(".", "")
            Case 3 'Fecha corta personalizada Lunes, 31-Dic-2002
                FormatFecha = FormatDia(Fecha) & ", " & Fecha.Day & "-" & FormatMes(Fecha, True) & "-" & Year(Fecha)
            Case 4 'Fecha larga personalizada Lunes, 31 de Diciembre de 2002
                FormatFecha = FormatDia(Fecha) & ", " & Fecha.Day & "-" & FormatMes(Fecha) & "-" & Year(Fecha)
            Case 5 'Fecha corta del sistema Regional Setting
                FormatFecha = Fecha.ToShortDateString.Replace(".", "")
            Case 6 'Fecha larga del sistema Regional Setting
                FormatFecha = Fecha.ToLongDateString.Replace(".", "")
            Case 7 'Hora corta del sistema
                FormatFecha = Fecha.ToShortTimeString.Replace(".", "")
            Case 8 'Hora larga del sistema
                FormatFecha = Fecha.ToLongTimeString.Replace(".", "")
            Case Else 'Nada
                FormatFecha = ""
        End Select
    End Function

    Public Shared Function FormatDia(ByVal Fecha As Date) As String
        FormatDia = String.Empty
        Select Case Fecha.DayOfWeek
            Case DayOfWeek.Monday
                FormatDia = "Lunes"
            Case DayOfWeek.Tuesday
                FormatDia = "Martes"
            Case DayOfWeek.Wednesday
                FormatDia = "Miercoles"
            Case DayOfWeek.Thursday
                FormatDia = "Jueves"
            Case DayOfWeek.Friday
                FormatDia = "Viernes"
            Case DayOfWeek.Saturday
                FormatDia = "Sabado"
            Case DayOfWeek.Sunday
                FormatDia = "Domingo"
        End Select
    End Function

    Private Function GetRemoteIP() As String
        Dim svrRemoteIP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        If (svrRemoteIP Is Nothing OrElse svrRemoteIP = "127.0.0.1" OrElse svrRemoteIP = "::1") _
           AndAlso _RemoteIP IsNot Nothing AndAlso _RemoteIP <> String.Empty Then
            Return _RemoteIP
        Else
            Return svrRemoteIP
        End If
    End Function

    Public Sub Log(ByVal Banca As Integer, ByVal LogTipo As String, ByVal LogComment As String)
        Using cn As New SqlConnection(StrSQLCon)
            cn.Open()
            Dim cm As SqlCommand = New SqlCommand("Insert into HLog (BancaID,LogTipo,LogComentario,SecRemoteIP) values (@Banca,@Tipo,@Comment,@IP)", cn)
            cm.CommandTimeout = gCommandTimeOut
            cm.Parameters.AddWithValue("@Banca", "0" & Banca)
            cm.Parameters.AddWithValue("@Tipo", LogTipo)
            cm.Parameters.AddWithValue("@Comment", LogComment)
            cm.Parameters.AddWithValue("@IP", GetRemoteIP())
            cm.ExecuteNonQuery()
        End Using
    End Sub

    Public Shared Function FormatMes(ByVal Fecha As Date, Optional ByVal Abreviado As Boolean = False) As String
        FormatMes = String.Empty
        Select Case Fecha.Month
            Case 1
                FormatMes = "Enero"
            Case 2
                FormatMes = "Febrero"
            Case 3
                FormatMes = "Marzo"
            Case 4
                FormatMes = "Abril"
            Case 5
                FormatMes = "Mayo"
            Case 6
                FormatMes = "Junio"
            Case 7
                FormatMes = "Julio"
            Case 8
                FormatMes = "Agosto"
            Case 9
                FormatMes = "Septiembre"
            Case 10
                FormatMes = "Octubre"
            Case 11
                FormatMes = "Noviembre"
            Case 12
                FormatMes = "Diciembre"
        End Select
        If Abreviado Then FormatMes = Microsoft.VisualBasic.Left(FormatMes, 3)
    End Function

    Private Function Envia(ByVal EndPoint As IPEndPoint, ByVal Origen As String, ByVal Banca As Integer, ByVal Mensaje As String) As Boolean
        Dim stream As NetworkStream, socket As Socket, reader As StreamReader, writer As StreamWriter, Firma As String
        stream = Nothing
        Try
            Dim temp As New Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
            temp.Blocking = True
            temp.Connect(EndPoint)
            socket = temp
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 5000)
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000)
            stream = New NetworkStream(socket)
            reader = New StreamReader(stream)
            writer = New StreamWriter(stream)
            Dim handshake(6) As Char, leyo As Integer
            Try
                leyo = reader.Read(handshake, 0, 6)
                Firma = New String(handshake)
                If Not (leyo > 0 And CInt(Firma) > 0) Then ' = IIf(Banca = 0, (Now.DayOfYear * Now.DayOfWeek * 21), Banca * 21)) Then
                    socket.Close()
                    socket = Nothing
                Else
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 0)
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 0)
                End If
            Catch
                socket.Close()
                socket = Nothing
            End Try

            If socket Is Nothing Then
                Return False
            End If

            If Banca = 0 Then
                SendTalk((Today.DayOfYear * Today.DayOfYear * 21 * Today.Day).ToString.PadLeft(9, "0"), "X", writer)
            Else
                SendTalk((Banca * 21 * Banca).ToString.PadLeft(9, "0"), "X", writer)
            End If
            SendTalk(Origen, "C", writer)
            SendTalk(Mensaje, "A", writer)
            socket.Close()
            socket = Nothing
            Return True
        Catch e As Exception
            Return False
        End Try
    End Function 'Envia

    Private Sub SendTalk(ByVal newText As String, ByVal Tipo As String, ByRef writer As StreamWriter)
        Dim send As String
        send = String.Format(Tipo & "{0}:{1}", newText.Length, newText)
        writer.Write(send)
        writer.Flush()
    End Sub

    Private Function GetJsonString(ByVal pObj As Object) As String
        If pObj Is Nothing Then Return Nothing
        Try
            Dim jsonString = String.Empty
            Dim memBuffer As New MemoryStream()
            Dim jsonWriter As New Json.DataContractJsonSerializer(pObj.GetType)
            jsonWriter.WriteObject(memBuffer, pObj)
            memBuffer.Seek(0, SeekOrigin.Begin)
            Dim strReader As New StreamReader(memBuffer)
            GetJsonString = strReader.ReadToEnd()
            strReader.Close()
        Catch ex As Exception
            Return String.Format("<Serializacion a JSON fallo: {0}>", ex.ToString)
        End Try
    End Function

    Private Function ValidHostName() As Boolean
        'If GetRemoteIP() = "::1" Then Return True

        Dim thePort = HttpContext.Current.Request.Url.Port
        If thePort = 14217 Then Return True '@@@ Remover esta linea de codigo
        If thePort = _BackupPort Then Return True

        Dim theHAuthority = String.Format("{0}:{1}",
                                          HttpContext.Current.Request.Url.Host.ToLower,
                                          thePort)
        If AllowedHosts.Contains(theHAuthority) Then
            Return True
        Else
            Log(0, "Server No Autorizado",
                "Server no autorizado: " & theHAuthority & "; server autorizado para solo para: " & Strings.Join(AllowedHosts, ", "))
            Return False
        End If

    End Function

#End Region

End Class

#Region "Sub Classes"

Public Class MAR_ExistingSession
    Public SesionInfo As MAR_Setting2
    Public POSInfo As MAR_Array
    Public Err As String
End Class

Public Class MAR_Suplidor
    Public SuplidorID As Integer
    Public SupNombre As String
    Public SupInstruccion As String
End Class

Public Class MAR_Usuario
    Public UsuarioID As Integer
    Public UsuNombre As String
End Class

Public Class MAR_BaseReporte
    Public Sub New()
        Dia = PtoVta.FormatFecha(Today, 1)
        Hora = PtoVta.FormatFecha(Now, 7)
    End Sub
    Public Dia As String
    Public Hora As String
End Class

Public Class MAR_RptVenta
    Inherits MAR_BaseReporte
    Public Err, Fecha, Primero, Segundo, Tercero As String
    Public Numeros, Pales, Tripletas, Comision, ComisionPorcQ, ComisionPorcP, ComisionPorcT, CPrimero,
        CSegundo, CTercero, MPrimero, MSegundo, MTercero,
        MPales, MTripletas As Double
    Public CntTarjetas, CntNumeros, Loteria As Integer
    Public TicketsNulos() As MAR_Bet
End Class

Public Class MAR_RptSumaVta
    Inherits MAR_BaseReporte
    Public Err, Fecha As String
    Public Reglones() As Mar_ReglonVta
End Class

Public Class MAR_RptSumaVta2
    Inherits MAR_BaseReporte
    Public Err, Fecha As String
    Public Reglones() As Mar_ReglonVta
    Public RifDescuento As Double
    Public ISRRetenido As Double
End Class

Public Class Mar_ReglonVta
    Inherits MAR_BaseReporte
    Public Err, Reglon, Fecha As String
    Public VentaBruta, Comision, Saco, Resultado As Double
End Class

Public Class MAR_VentaNumero
    Inherits MAR_BaseReporte
    Public Err, Fecha As String
    Public Loteria As Integer
    Public Numeros() As MAR_BetItem
End Class

Public Class MAR_Ganadores
    Inherits MAR_BaseReporte
    Public Fecha As String
    Public Primero As String
    Public Segundo As String
    Public Tercero As String
    Public Err As String
    Public Tickets() As MAR_Bet
End Class

Public Class MAR_Pines
    Inherits MAR_BaseReporte
    Public Fecha As String
    Public Err As String
    Public Pines() As MAR_Pin
End Class

Public Class MAR_MultiBet
    Public Err As String
    Public Headers() As MAR_BetHeader
    Public Items() As MAR_BetItem
End Class

Public Class MAR_BetHeader
    Public Cliente As String
    Public Cedula As String
    Public Nulo As Boolean = False
    Public Ticket As Integer
    Public TicketNo As String
    Public Loteria As Integer
    Public Grupo As Integer
    Public StrFecha As String
    Public StrHora As String
    Public Costo As Double
    Public Pago As Double
    Public Solicitud As Double
End Class

Public Class MAR_Bet
    Public Err As String
    Public Cliente As String
    Public Cedula As String
    Public Nulo As Boolean = False
    Public Ticket As Integer
    Public TicketNo As String
    Public Loteria As Integer
    Public Grupo As Integer
    Public StrFecha As String
    Public StrHora As String
    Public Costo As Double
    Public Pago As Double
    Public Solicitud As Double
    Public Items() As MAR_BetItem
End Class

Public Class MAR_Pin
    Public Pin As Integer
    Public Err As String
    Public Producto As MAR_Producto
    Public Nulo As Boolean = False
    Public Serie As String
    Public Numero As String
    Public StrFecha As String
    Public StrHora As String
    Public Costo As Double
    Public Solicitud As Double
    Public Flag As String
End Class

Public Class MAR_BetItem
    Public Loteria As Integer
    Public Numero As String
    Public Cantidad As Double
    Public QP As String
    Public Costo As Double
    Public Pago As Double
End Class

Public Class MAR_Setting
    Public Sesion As MAR_Session = New MAR_Session
    Public Loterias() As MAR_Loteria
End Class

Public Class MAR_Setting2
    Public Sesion As MAR_Session = New MAR_Session
    Public Loterias() As MAR_Loteria2
    Public MoreOptions() As String
End Class

'Public Class MAR_Session
'    Public Banca As Integer
'    Public Usuario As Integer
'    Public Sesion As Integer
'    Public Err As String
'    Public LastTck As Integer
'    Public LastPin As Integer
'End Class

Public Class MAR_Producto
    Public SupID, ProID As Integer
    Public Suplidor, Producto, Instruccion As String
    Public Precio As Double
End Class

Public Class MAR_Array
    Public Err As String = ""
    Public Cantidad As Integer
    Public Llave() As String
    Public Text() As String
End Class

Public Class MAR_Bancas
    Public Bca() As MAR_Banca
End Class

Public Class MAR_Banca
    Public Banca As Integer
    Public BanNombre, BanContacto, BanTelefono, IPAddr As String
    Public BanPrecioQ, BanPagaQ, BanComision As Double
    Public BanOnline As Boolean
End Class

Public Class MAR_VentaFuera
    Public Err As String = ""
    Public Banca As Integer
    Public Loteria As String
    Public QVendido As Double
    Public PVendido As Double
    Public TVendido As Double
    Public SacoPrimera As Double
    Public SacoSegunda As Double
    Public SacoTercera As Double
    Public SacoPale As Double
    Public SacoTriple As Double
    Public FechaDia As String
End Class

Public Class MAR_Mensaje
    Public MensajeID As Integer
    Public Tipo As String
    Public Asunto As String
    Public Contenido As String
    Public Fecha As String
    Public Hora As String
    Public Origen As String
    Public Destino As String
End Class

Public Class MAR_Mensaje2
    Public BancaID As Integer
    Public MensajeID As Integer
    Public Tipo As String
    Public Asunto As String
    Public Contenido As String
    Public Fecha As String
    Public Hora As String
    Public Origen As String
    Public Destino As String
    Public Leido As Boolean
    Public SinLeerTotal As Integer
End Class

Public Class MAR_Mensajes
    Public Err As String = ""
    Public msj() As MAR_Mensaje
    Public NewInterval As Integer
End Class

Public Class MAR_Mensajes2
    Public Err As String = ""
    Public Enviado As Boolean
    Public msj() As MAR_Mensaje2
    Public Bca() As MAR_Banca
    Public NewInterval As Integer
End Class

Public Class MAR_MensajesCount
    Public Err As String = ""
    Public MsjNuevos As Integer
    Public NewInterval As Integer
End Class

Public Class MAR_Loteria
    Public Numero, CieLun, CieMar, CieMie, CieJue, CieVie, CieSab, CieDom As Integer
    Public Nombre As String
    Public NombreResumido As String
    Public Imagen As String
    Public PrecioQ, PrecioP, PrecioT, PagoQ1, PagoQ2, PagoQ3, PagoP1, PagoP2, PagoP3, PagoT1, PagoT2, PagoT3 As Double
End Class

Public Class MAR_Loteria2
    Public Numero, CieLun, CieMar, CieMie, CieJue, CieVie, CieSab, CieDom As Integer
    Public Nombre As String
    Public NombreResumido As String
    Public Imagen As String
    Public Oculta As Boolean
    Public PrecioQ, PrecioP, PrecioT, PagoQ1, PagoQ2, PagoQ3, PagoP1, PagoP2, PagoP3, PagoT1, PagoT2, PagoT3 As Double
    Public MoreOptions() As String
End Class

Public Class MAR_ValWiner
    Inherits MAR_BaseReporte
    Public Aprobado As Integer
    Public Ticket, Mensaje, err As String
    Public Monto As Double
End Class

#End Region
