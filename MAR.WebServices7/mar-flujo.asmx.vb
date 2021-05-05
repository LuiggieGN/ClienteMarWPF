Imports MAR
Imports Newtonsoft.Json

Imports System.Threading.Tasks
Imports System.Globalization
Imports System.ComponentModel
Imports System.Web.Services
Imports System.Web.Services.Protocols

Imports Flujo.Entities.WpfClient.Enums
Imports Flujo.Entities.WpfClient.RequestModel
Imports Flujo.Entities.WpfClient.ResponseModels

Imports MAR.BusinessLogic.Code.ControlEfectivo



' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="mar.do")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class mar_flujo
    Inherits System.Web.Services.WebService

    <WebMethod()>
    Public Function CallFlujoIndexFunction(pMetodo As Integer, pSesion As MAR_Session, pParams As Object()) As MAR_FlujoResponse

        Try
            Dim printWidth = 40
            Dim riferoID = 0
            Dim direccion = ""
            Dim result = Nothing
            Dim msj As String = Nothing

            'Dim bancaData = DAL.GetTabla("SELECT RiferoID,BanDireccion FROM MBancas WHERE BancaID=@ban AND BanUsuarioActual=@usr AND BanSesionActual=@ses",
            '                         ({New Pair("ban", pSesion.Banca), New Pair("usr", pSesion.Usuario), New Pair("ses", pSesion.Sesion)}).ToList())
            'If bancaData.Rows.Count > 0 Then
            '    riferoID = bancaData.Rows(0)("RiferoID")
            '    direccion = bancaData.Rows(0)("BanDireccion")
            'Else
            '    Return New MAR_FlujoResponse() With {.OK = False, .Mensaje = "Su sesion de trabajo no es valida"}
            'End If

            Select Case pMetodo 'HAY CUADRE INICIAL    
                Case 1
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.EstaBancaPoseeCuadreInical(bancaid)

                Case 2
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.GetBancaLastCuadre_ID(bancaid)

                Case 3
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.GetTransaccionesDesdeUltimoCuadre(bancaid)

                Case 4
                    ' POST
                    Dim cuadreId = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.GetCuadre(cuadreId)

                Case 5
                    ' POST
                    Dim cuadre = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of CuadreModel)(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.GuardarCuadre(cuadre)

                Case 6
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.GetMontoTotalizadoTiketsPendientesDePagoSinReclamar(bancaid)

                Case 7
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.BancaRepositorio.GetBanca(bancaid)


                Case 8
                    ' POST
                    Dim cuadreModel = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of Flujo.Entities.WpfClient.RequestModel.CuadreModel)(pParams(0).ToString())
                    Dim esUnRetiro = Boolean.Parse(pParams(1).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.Procesar(cuadreModel, esUnRetiro)

                'Case 9
                '    ' POST
                '    Dim caja = Integer.Parse(pParams(0).ToString())
                '    Dim entrada = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(1).ToString())
                '    Dim montoAfavor = Decimal.Parse(pParams(2).ToString())
                '    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.RegistarMovimientoCuadre(caja, entrada, montoAfavor)
                'Case 10
                '    ' POST
                '    Dim bancaid = Integer.Parse(pParams(0).ToString())
                '    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.AsentarTransaccionesDesdeUltimoCuadre(bancaid)

                Case 11
                    ' POST
                    Dim cajaId = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.UsuarioRepositorio.GetUsuarioResponsableDeCaja(cajaId)

                Case 12
                    ' POST
                    Dim pDocumento = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(0).ToString())
                    Dim tipoDoc = Integer.Parse(pParams(1).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.UsuarioRepositorio.BuscarUsuarioPor(pDocumento, tipoDoc)

                Case 13
                    ' POST
                    Dim pDocumento = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(0).ToString())
                    Dim ptipo = Integer.Parse(pParams(1).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.UsuarioRepositorio.BuscarMensajeroUsuarioPor(pDocumento, ptipo)
                Case 14
                    ' POST
                    Dim usuarioId = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.TokenSeguridadRepositorio.ObtenerUnSoloTokenDeFormaAleatoria(usuarioId)
                Case 15
                    ' POST
                    Dim usuarioId = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.TokenSeguridadRepositorio.ConsultarTokensDeSeguridadPor(usuarioId)

                Case 16
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.GetBancaCajaID(bancaid)

                Case 17
                    ' POST
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.UsuarioRepositorio.GetFirstSurperUsuario()

                Case 21
                    ' POST
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.MovimientoRepositorio.ConsultarCategoriaTiposIngresos()

                Case 22
                    ' POST
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.MovimientoRepositorio.ConsultarCategoriasTiposEgresos()

                Case 23
                    ' POST
                    Dim startIndex = Integer.Parse(pParams(0).ToString())
                    Dim itemCount = Integer.Parse(pParams(1).ToString())
                    Dim sortColumn = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(2).ToString())
                    Dim ascending = Boolean.Parse(pParams(3).ToString())
                    Dim sourceQuery = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(4).ToString())
                    Dim sourceQueryParams = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(pParams(5))
                    Dim cajaId = Integer.Parse(pParams(6).ToString())
                    Dim fechaDesde = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(7).ToString())
                    Dim fechaHasta = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(8).ToString())

                    Dim dic As Dictionary(Of String, Object) = New Dictionary(Of String, Object)
                    dic.Add("@CajaID", cajaId)
                    dic.Add("@FechaInicio", fechaDesde)
                    dic.Add("@FechaFin", fechaHasta)
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.PagerRepositorio.GetProcedurePagingResult(Of MovimientoResponseModel)(startIndex, itemCount, sortColumn, ascending, sourceQuery, dic)



                Case 25
                    ' POST
                    Dim pBancaID = Integer.Parse(pParams(0).ToString())
                    Dim pBancaUsuarioID = Integer.Parse(pParams(1).ToString())
                    Dim pMonto = Decimal.Parse(pParams(2).ToString(), Globalization.NumberStyles.Any, CultureInfo.InvariantCulture)
                    Dim pDescripcion = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(3).ToString())
                    Dim pIngresoOEgreso = Integer.Parse(pParams(4).ToString())
                    Dim pTipoIngresoOTipoEgreso = Integer.Parse(pParams(5).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.MovimientoRepositorio.InsertarMovimientoSinUsuarioAutorizado(pBancaID, pBancaUsuarioID, pMonto, pDescripcion, pIngresoOEgreso, pTipoIngresoOTipoEgreso)

                Case 26
                    ' POST
                    Dim pBancaID = Integer.Parse(pParams(0).ToString())
                    Dim pBancaUsuarioID = Integer.Parse(pParams(1).ToString())
                    Dim pMonto = Decimal.Parse(pParams(2).ToString(), Globalization.NumberStyles.Any, CultureInfo.InvariantCulture)
                    Dim pDescripcion = AppLogic.MARHelpers.JSONHelper.CreateNewFromJSON(Of String)(pParams(3).ToString())
                    Dim pIngresoOEgreso = Integer.Parse(pParams(4).ToString())
                    Dim pTipoIngresoOTipoEgreso = Integer.Parse(pParams(5).ToString())
                    Dim pUsuarioExternoId = Integer.Parse(pParams(6).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.MovimientoRepositorio.InsertaMovimientoConUsuarioAutorizado(pBancaID, pBancaUsuarioID, pUsuarioExternoId, pMonto, pDescripcion, pIngresoOEgreso, pTipoIngresoOTipoEgreso)

                Case 27
                    ' POST

                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    Dim disponible = Boolean.Parse(pParams(1).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.ConfigurarCajaDisponibilidad(bancaid, disponible)

                Case 30
                    ' POST
                    Dim cajaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.GetCajaBalanceActual(cajaid)

                Case 31
                    ' POST
                    Dim usuarioid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.GetCajaVirtual(usuarioid)

                Case 32
                    ' POST
                    Dim cajaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.GetCajaBalanceMinimo(cajaid)
                Case 33
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.CajaFueAsignadaABanca(bancaid)

                Case 34
                    ' POST
                    Dim bancaid = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CajaRepositorio.CreaCajaABanca(bancaid)

                Case 40
                    Dim pin = pParams(0).ToString()
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.UsuarioRepositorio.GetUsuarioByPin(pin)

                Case 41
                    Dim gestorId = Integer.Parse(pParams(0).ToString())
                    Dim bancaIdQueGestorTransita = Integer.Parse(pParams(1).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.UsuarioRepositorio.GetGestorAsignacionPendiente(gestorId, bancaIdQueGestorTransita)
                Case 42
                    Dim usuarioId = Integer.Parse(pParams(0).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.UsuarioRepositorio.GetUsuarioTarjeta(usuarioId)

                Case 43
                    Dim rutaEstado = pParams(0).ToString()
                    Dim rutaUltimaLocalidad = Integer.Parse(pParams(1).ToString())
                    Dim rutaOrdenRecorrido = pParams(2).ToString()
                    Dim cuadreId = Integer.Parse(pParams(3).ToString())
                    Dim bancaCajaId = Integer.Parse(pParams(4).ToString())
                    Dim rutaId = Integer.Parse(pParams(5).ToString())
                    result = Flujo.DataAccess.FlujoRepositories.WpfClient.DapperRepositories.CuadreRepositorio.EnlazaCuadreConAsignacion(rutaEstado, rutaUltimaLocalidad, rutaOrdenRecorrido, cuadreId, bancaCajaId, rutaId)

            End Select

            Return New MAR_FlujoResponse() With {
                .OK = True,
                .Mensaje = msj,
                .Respuesta = Newtonsoft.Json.JsonConvert.SerializeObject(result)
            }

        Catch ex As Exception
            Return New MAR_FlujoResponse() With {
                .OK = False,
                .Mensaje = "Ocurrio un error procesando su transacción.",
                .Err = ex.ToString()
                }
        End Try
    End Function

    <WebMethod()>
    Public Function CallControlEfectivoFunciones(metodo As Integer, parametros As Object()) As MAR_FlujoResponse

        Try
            Dim result = Nothing

            Select Case metodo

#Region "Cajas"

                Case 1000
                    Dim jsonMovimiento = parametros(0).ToString()
                    result = CajaLogic.RegistrarMovimientoEnBanca(jsonMovimiento)
                Case 1001
                    Dim jsonTransferencia = parametros(0).ToString()
                    result = CajaLogic.RegistrarTransferencia(jsonTransferencia)
                Case 1002
                    Dim jsonPaginaRequest = parametros(0).ToString()
                    result = CajaLogic.LeerMovimientos(jsonPaginaRequest)
                Case 1003
                    Dim cajaid = Integer.Parse(parametros(0).ToString())
                    result = CajaLogic.LeerCajaBalance(cajaid)
                Case 1004
                    Dim usuarioid = Integer.Parse(parametros(0).ToString())
                    result = CajaLogic.LeerCajaDeUsuarioPorUsuarioId(usuarioid)
                Case 1005
                    Dim jsonCajaDisponibilidad = parametros(0).ToString()
                    result = CajaLogic.SetearCajaDisponibilidad(jsonCajaDisponibilidad)
                Case 1007
                    Dim cajaid = Integer.Parse(parametros(0).ToString())
                    result = CajaLogic.LeerCajaBalanceMinimo(cajaid)
                Case 1008
                    Dim jsonPaginaRequest = parametros(0).ToString()
                    result = CajaLogic.LeerMovimientosNoPaginados(jsonPaginaRequest)
#End Region

#Region "Bancas"
                Case 2000
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerBancaLastCuadreId(bancaid)

                Case 2001
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerBancaLastTransaccionesApartirDelUltimoCuadre(bancaid)

                Case 2002
                    Dim cuadreid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerBancaCuadrePorCuadreId(cuadreid)

                Case 2003
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    Dim incluyeConfig = Boolean.Parse(parametros(1).ToString())
                    result = BancaLogic.BancaUsaControlEfectivo(bancaid, incluyeConfig)

                Case 2004
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerDeudaDeBanca(bancaid)

                Case 2005
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerBancaConfiguraciones(bancaid)

                Case 2006
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerBancaInactividad(bancaid)

                Case 2007
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    Dim strdia = parametros(1).ToString()
                    result = BancaLogic.LeerBancaMarOperacionesDia(bancaid, strdia)

                Case 2008
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerBancaRemoteCmdCommand(bancaid)

                Case 2009
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerEstadoBancaEstaActiva(bancaid)

                Case 2010
                    Dim bancaid = Integer.Parse(parametros(0).ToString())
                    result = BancaLogic.LeerVentaDeHoyDeLoterias(bancaid)
#End Region

#Region "Cuadre"
                Case 3000
                    Dim jsonCuadre = parametros(0).ToString()
                    Dim esUnRetiro = Boolean.Parse(parametros(1).ToString())
                    result = CuadreLogic.Registrar(jsonCuadre, esUnRetiro)

                Case 3001
                    Dim rutaEstado = parametros(0).ToString()
                    Dim rutaUltimaLocalidad = Integer.Parse(parametros(1).ToString())
                    Dim rutaOrdenRecorrido = parametros(2).ToString()
                    Dim cuadreId = Integer.Parse(parametros(3).ToString())
                    Dim bancaCajaId = Integer.Parse(parametros(4).ToString())
                    Dim rutaId = Integer.Parse(parametros(5).ToString())
                    result = CuadreLogic.EnlazarRutaConCuadre(rutaEstado, rutaUltimaLocalidad, rutaOrdenRecorrido, cuadreId, bancaCajaId, rutaId)
#End Region

#Region "Tie"
                Case 4000
                    result = TieLogic.LeerTiposAnonimos()
#End Region

#Region "Multiple"
                Case 8000
                    Dim pin = parametros(0).ToString()
                    result = MultipleLogic.LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(pin)
#End Region

#Region "Ruta"
                Case 9000
                    Dim gestorusuarioid = Integer.Parse(parametros(0).ToString())
                    Dim bancaid = Integer.Parse(parametros(1).ToString())
                    result = RutaLogic.LeerGestorAsignacionPendiente(gestorusuarioid, bancaid)
#End Region

            End Select

            Return New MAR_FlujoResponse() With {
                .OK = True,
                .Mensaje = String.Empty,
                .Respuesta = JsonConvert.SerializeObject(result)
            }

        Catch ex As Exception
            Return New MAR_FlujoResponse() With {
                .OK = False,
                .Mensaje = "Ocurrio un error procesando su transacción.",
                .Err = ex.ToString()
                }
        End Try
    End Function








    Public Class MAR_FlujoResponse
        Public OK As Boolean
        Public Mensaje As String
        Public Respuesta As String
        Public Err As String
    End Class
End Class

Public Class MAR_Session
    Public Banca As Integer
    Public Usuario As Integer
    Public Sesion As Integer
    Public Err As String
    Public LastTck As Integer
    Public LastPin As Integer
    Public PrinterSize As Integer
    Public PrinterHeader As String
    Public PrinterFooter As String
End Class