Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports MAR
Imports System.Data.SqlClient
Imports Newtonsoft.Json.Linq

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class mar_hacienda
    Inherits System.Web.Services.WebService


#Region "Private Members and Conexion"

    Friend Shared StrSQLCon As String = ConfigReader.ReadString(MAR.Config.ConfigEnums.DBConnection2)
    Private AllowedHosts As String() = ConfigReader.ReadStringArray(MAR.Config.ConfigEnums.AllowedWebHosts)
    Private _BackupPort As Integer = CInt(ConfigReader.ReadString(MAR.Config.ConfigEnums.ServiceBackupPort))
    Private gCommandTimeOut As Integer = 210
    Private _RemoteIP As String
#End Region
    Private Function GetRemoteIP() As String
        Dim svrRemoteIP = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        If (svrRemoteIP Is Nothing OrElse svrRemoteIP = "127.0.0.1" OrElse svrRemoteIP = "::1") _
           AndAlso _RemoteIP IsNot Nothing AndAlso _RemoteIP <> String.Empty Then
            Return _RemoteIP
        Else
            Return svrRemoteIP
        End If
    End Function

    <WebMethod()>
    Public Function CallHaciendaFuncion(pMetodo As Integer, pSesion As MAR_Session, pParams As Object()) As MAR_HaciendaResponse
        Try
            Dim printWidth = pSesion.PrinterSize
            Dim printHeader = pSesion.PrinterHeader
            Dim printFooter = pSesion.PrinterFooter
            Dim riferoID = 0
            Dim direccion = ""
            Dim bancaNombre = ""
            Dim result = Nothing
            Dim msj As String = Nothing

            Dim bancaData = DAL.GetTabla("SELECT RiferoID,BanDireccion,BanNombre FROM MBancas WHERE BancaID=@ban AND BanUsuarioActual=@usr AND BanSesionActual=@ses",
                                     ({New Pair("ban", pSesion.Banca), New Pair("usr", pSesion.Usuario), New Pair("ses", pSesion.Sesion)}).ToList())
            If bancaData.Rows.Count > 0 Then
                riferoID = bancaData.Rows(0)("RiferoID")
                direccion = bancaData.Rows(0)("BanDireccion")
                bancaNombre = bancaData.Rows(0)("BanNombre")
            Else
                Return New MAR_HaciendaResponse() With {.OK = False, .Mensaje = "Su sesion de trabajo no es valida"}
            End If

            Select Case pMetodo
                Case 1
                    ' POST INICIO DE DIA Y APUESTA
                    Dim diaIniciado = False
                    Dim autorizacionHacienda = ""
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim ticket = paramsSplited(0).ToString()
                    Dim bancaid = Integer.Parse(paramsSplited(1).ToString())
                    Try
                        diaIniciado = MAR.BusinessLogic.Code.Hacienda.InicioDiaLogic.IniciaDia(bancaid)
                    Catch ex As Exception
                        Log(bancaid, "HaciendaFalloInicioDia", ex.ToString())
                    End Try
                    'Hacienda** Aqui enviamos la apuesta a Hacienda si el dia esta iniciado
                    If diaIniciado Then
                        result = MAR.BusinessLogic.Code.Hacienda.ApuestaLogic.Apuesta(ticket)
                    End If

                Case 2
                    ' POST ANULACION
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim TicketNo = paramsSplited(0).ToString()
                    Dim bancaid = Integer.Parse(paramsSplited(1).ToString())
                    MAR.BusinessLogic.Code.Hacienda.AnulacionLogic.Anulacion(TicketNo, bancaid)

                Case 3
                    ' POST PAGA GANADOR
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim ticket = paramsSplited(0).ToString()
                    result = MAR.BusinessLogic.Code.Hacienda.PagoGanadorLogic.PagoGanador(ticket)

                Case 16
                    ' POST
                    Dim ticketModel = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of DataAccess.EFRepositories.SorteosMar.ApuestaRepository.TicketModel)(pParams(0).ToString())
                    Dim producto = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of DataAccess.EFRepositories.ProductosRepository.ProductoViewModel)(pParams(1).ToString())
                    result = MAR.BusinessLogic.Code.SorteosMar.ApuestaLogic.Apuesta(ticketModel, pSesion.Banca, pSesion.Usuario, riferoID, producto, direccion, printWidth, bancaNombre, printHeader, printFooter)

                Case 17
                    ' GET
                    result = MAR.BusinessLogic.Code.SorteosMar.SorteosMarIndexLogic.GetProductosDisponibles()

                Case 18 'FunctionName.JuegaMasIndex_GetVendidosHoy 
                    ' GET
                    result = MAR.BusinessLogic.Code.SorteosMar.SorteosMarIndexLogic.GetTicket_VendidosHoy(riferoID, pSesion.Banca, DataAccess.Tables.Enums.DbEnums.Productos.CincoMinutos)


                    ' LOTO DOM*****************************************************************
                Case 19
                    ' POST APUESTA LOTODOM
                    Dim autorizacionHacienda = ""
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim ticket = paramsSplited(0).ToString()
                    Dim bancaid = Integer.Parse(paramsSplited(1).ToString())
                    result = MAR.BusinessLogic.Code.SorteosLotoDom.ApuestaLogic.Apuesta(ticket, riferoID)
                Case 20
                    ' POST ANULACION
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim TicketNo = paramsSplited(0).ToString()
                    Dim bancaid = Integer.Parse(paramsSplited(1).ToString())
                    MAR.BusinessLogic.Code.SorteosLotoDom.AnulacionLogic.Anulacion(TicketNo, bancaid)

                    'Case 21
                    '    ' POST CONSULTA GANADOR
                    '    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    '    Dim TicketNo = Integer.Parse(paramsSplited(0).ToString())
                    '    Dim bancaid = Integer.Parse(paramsSplited(1).ToString())
                    '    MAR.BusinessLogic.Code.SorteosLotoDom.PagoGanadorLogic.ConsultaTicketGanador(TicketNo, bancaid)

                Case 21
                    ' Reload Sorteos Disponibles
                    result = MAR.BusinessLogic.Code.SorteosLogic.GetSorteosDisponibles()














                    ' SERVICIOS DE FLUJO PARA EL CLIENTE WINDOWS
                Case 300
                    ' POST
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim bancaid = Integer.Parse(paramsSplited(0).ToString())
                    Dim cajaid = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.GetBancaCajaID(bancaid)
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.GetCajaBalanceActual(cajaid)















            End Select

            Return New MAR_HaciendaResponse() With {
                .OK = True,
                .Mensaje = msj,
                .Respuesta = Newtonsoft.Json.JsonConvert.SerializeObject(result)
            }

        Catch ex As Exception
            Return New MAR_HaciendaResponse() With {
                .OK = False,
                .Mensaje = "Ocurrio un error procesando su transacción.",
                .Err = ex.ToString()
                }
        End Try

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
    Public Class MAR_HaciendaResponse
        Public OK As Boolean
        Public Mensaje As String
        Public Respuesta As String
        Public Err As String
    End Class
End Class