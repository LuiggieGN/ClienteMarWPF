Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

Imports System.Data.SqlClient
Imports MAR
Imports Newtonsoft.Json.Linq
Imports MAR.DataAccess.Tables.DTOs
Imports MarConnectCliente.RequestModels

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="mar.do")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class mar_bingo
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function CallJuegaMaxIndexFunction(pMetodo As Integer, pSesion As MAR_Session, pParams As Object(), pSolicitud As Integer) As MAR_BingoResponse
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
                Return New MAR_BingoResponse() With {.OK = False, .Mensaje = "Su sesion de trabajo no es valida"}
            End If

            Select Case pMetodo
                Case 1 'FunctionName.JuegaMasIndex_RealizarVenta 
                    ' POST
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim dTicket = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of DTicket)(paramsSplited(0).ToString())
                    Dim dQpMar = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of Integer())(paramsSplited(1).ToString())
                    Dim dCantQpMarlton = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of Integer())(paramsSplited(2).ToString())
                    result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.RealizarVenta_JuegaMasNueva(printWidth, dTicket, pSesion.Banca, pSesion.Usuario, pSolicitud, riferoID, pSesion.Banca, direccion)

                Case 2 'FunctionName.JuegaMasIndex_RealizarVentaFree
                    '    ' POST
                    Dim paramsSplitedFree = JArray.Parse(pParams(0).ToString())
                    Dim dTicketFree = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of DTicket)(paramsSplitedFree(0).ToString())
                    Dim dQpMarFree = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of Integer())(paramsSplitedFree(1).ToString())
                    Dim dCantQpMarltonFree = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of Integer())(paramsSplitedFree(2).ToString())
                    Dim numemroTicketFree = paramsSplitedFree(3).ToString()
                    Dim serialTicketFree = paramsSplitedFree(4).ToString()
                    Dim theMonto = Decimal.Parse(paramsSplitedFree(5).ToString())

                    If theMonto > 0 Then
                        result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.RealizarPago_JuegaMas(pSesion.Banca, numemroTicketFree, serialTicketFree, pSesion.Usuario, theMonto, printWidth, pSesion.Banca, direccion, riferoID)
                        If result.OK AndAlso result.GetType().GetProperty("PrintData") IsNot Nothing Then
                            Dim resultTicGratis = MAR.BusinessLogic.Code.JuegaMasIndexLogic.RealizarVenta_JuegaMasNuevaFree(printWidth, dTicketFree, pSesion.Banca, pSesion.Usuario, pSolicitud, riferoID, pSesion.Banca, direccion, serialTicketFree)
                            If resultTicGratis.OK AndAlso resultTicGratis.GetType().GetProperty("PrintData") IsNot Nothing Then
                                CType(result.PrintData, List(Of String())).AddRange(CType(resultTicGratis.PrintData, List(Of String())))

                                If resultTicGratis.GetType().GetProperty("Ticket") IsNot Nothing Then
                                    result = New With {
                                        .OK = result.OK,
                                        .PrintData = result.PrintData,
                                        .Ticket = resultTicGratis.Ticket,
                                        .Mensaje = resultTicGratis.Mensaje
                                    }
                                End If

                            End If
                        End If
                    Else
                        result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.RealizarVenta_JuegaMasNuevaFree(printWidth, dTicketFree, pSesion.Banca, pSesion.Usuario, pSolicitud, riferoID, pSesion.Banca, direccion, serialTicketFree)
                    End If

                Case 3 'FunctionName.JuegaMasIndex_GetVendidosHoy 
                    ' GET
                    result = MAR.BusinessLogic.Code.BingoIndexLogic.GetTicket_VendidosHoy(riferoID, pSesion.Banca)

                Case 4 'FunctionName.JuegaMasIndex_GetTicketDetalle :    
                    ' GET
                    Dim theTransaccionId = Integer.Parse(pParams(0).ToString())
                    result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.GetTicketDetalle(theTransaccionId)

                Case 5 'FunctionName.JuegaMasIndex_GetQuickPick :    
                    ' GET
                    result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.JugarQuickPick_Marlton(printWidth, pSesion.Banca, pSesion.Usuario, riferoID, pSesion.Banca, direccion)
                Case 6 'FunctionName.JuegaMasIndex_SetSuplidor :    
                    ' Get
                    result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.SetSuplidor("Marlton JuegaMas")

                Case 7 'FunctionName.JuegaMasIndex_CancelarBillete :    
                    ' POST
                    Dim theArray = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String())(pParams(0).ToString())
                    Dim theTcktNo = Integer.Parse(theArray(0))
                    Dim theTcktPin = theArray(1)
                    result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.CancelarJuegaMas(theTcktNo, theTcktPin, pSesion.Banca)

                Case 8 'FunctionName.JuegaMasIndex_GetEstadoBillete :    
                    ' POST
                    Dim theArray1 = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String())(pParams(0).ToString())
                    Dim theTcktNo1 = Integer.Parse(theArray1(0))
                    Dim theTcktSerial = theArray1(1)
                    result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.ValidarJuegaMas(theTcktNo1, pSesion.Banca, theTcktSerial)

                Case 9 'FunctionName.JuegaMasIndex_AplicarPago :    
                    ' POST
                    Dim theArray2 = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String())(pParams(0).ToString())
                    Dim theTcktNo2 = Integer.Parse(theArray2(0))
                    Dim theTcktPin2 = theArray2(1)
                    Dim theMonto = Decimal.Parse(theArray2(2))
                    result = MAR.BusinessLogic.Code.BingoIndexLogic.RealizarPago_Bingo(pSesion.Banca, theTcktNo2, pSesion.Usuario, theMonto, printWidth, pSesion.Banca, direccion, riferoID)
                Case 10 'FunctionName.JuegaMasIndex_ReimprimeTicket :    
                    ' POST
                    Dim theTransaccionId = Integer.Parse(pParams(0).ToString())
                    result = MAR.BusinessLogic.Code.JuegaMasIndexLogic.ReimprimirTicket(theTransaccionId, printWidth, pSesion.Banca, pSesion.Usuario, riferoID, pSesion.Banca, direccion)

                Case 11 ' FunctionName.Global_JuegaMasRegistraBanca : 
                    ' POST
                    MAR.BusinessLogic.Code.JuegaMasIndexLogic.RegistraBancaEnMarlton(pSesion.Banca)
                    result = "OK"
                Case 12 ' FunctionName.Global_JuegaMasRegistraBanca : 
                    ' POST
                    Dim pfecha = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String())(pParams(0).ToString())(0)
                    result = MAR.BusinessLogic.Code.ReportesIndexLogic.ReportePremios_PorFecha(pSesion.Banca, pSesion.Banca, 2, DateTime.Parse(pfecha.ToString()))

                Case 13 'FunctionName.BingoIndexLogic.GetProductosDisponibles 
                    ' GET
                    result = MAR.BusinessLogic.Code.BingoIndexLogic.GetProductosDisponibles()

                Case 14 'FunctionName.BingoIndexLogic.RealizarVenta 
                    ' POST
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim producto = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of DataAccess.EFRepositories.ProductosRepository.ProductoViewModel)(paramsSplited(0).ToString())
                    result = MAR.BusinessLogic.Code.BingoIndexLogic.RealizaVenta(printWidth, pSesion.Banca, pSesion.Usuario, riferoID, pSesion.Banca, direccion, producto, "", "")

                Case 15 'FunctionName.BingoIndexLogic.ConsultaPago 
                    ' POST
                    Dim paramsSplited = JArray.Parse(pParams(0).ToString())
                    Dim refBingo = paramsSplited(0).ToString()
                    result = MAR.BusinessLogic.Code.BingoIndexLogic.ResultadoSorteos(refBingo, riferoID)








                    'SORTEOS MAR DESDE AQUI. DESDE AQUI ARRIBA PODRE BORRAR TODO CUANDO MUEVA********************************************************************************

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

                Case 19 'Consulta pago ganador
                    ' GET
                    Dim ticket = (pParams(0).ToString())
                    Dim pin = (pParams(1).ToString())
                    result = MAR.BusinessLogic.Code.SorteosMar.PagoGanadorLogic.ConsultaPagoGanador(pSesion.Banca, riferoID, ticket, pin)

                Case 20 'PAGO GANADOR CINCO MINUTOS
                    ' GET
                    Dim ticket = (pParams(0).ToString())
                    Dim pin = (pParams(1).ToString())
                    Dim saco = (pParams(2).ToString())
                    Dim jugadas = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of DetalleJugadasPago)(pParams(3).ToString())
                    result = MAR.BusinessLogic.Code.SorteosMar.PagoGanadorLogic.PagoGanador(pSesion.Banca, direccion, bancaNombre, printWidth, riferoID, ticket, pin, jugadas, pSesion.Usuario)

                Case 21 'Anula Apuesta
                    ' GET
                    Dim ticket = (pParams(0).ToString())
                    Dim pin = (pParams(1).ToString())
                    result = MAR.BusinessLogic.Code.SorteosMar.AnulacionLogic.Anulacion(ticket, pSesion.Banca, pin, riferoID)
            End Select

            Return New MAR_BingoResponse() With {
                .OK = True,
                .Mensaje = msj,
                .Respuesta = Newtonsoft.Json.JsonConvert.SerializeObject(result)
            }

        Catch ex As Exception
            Return New MAR_BingoResponse() With {
                .OK = False,
                .Mensaje = "Ocurrio un error procesando su transacción.",
                .Err = ex.ToString()
                }
        End Try

    End Function

    Public Class MAR_BingoResponse
        Public OK As Boolean
        Public Mensaje As String
        Public Respuesta As String
        Public Err As String
    End Class

End Class