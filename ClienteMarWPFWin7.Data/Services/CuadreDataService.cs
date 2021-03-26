

using System;
using System.Linq;
using System.Globalization;

using System.Threading.Tasks;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;

using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.Services.CuadreService;
using ClienteMarWPFWin7.Domain.Exceptions;

using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.FlujoService;

namespace ClienteMarWPFWin7.Data.Services
{
    public class CuadreDataService : ICuadreService
    {
        public static SoapClientRepository soapClientesRepository;
        private static mar_flujoSoapClient efectivoSoapCliente;

        static CuadreDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            efectivoSoapCliente = soapClientesRepository.GetCashFlowServiceClient(false);
        }

        private CuadreRegistroResultDTO AplicarCuadre(CuadreDTO cuadre, CuadreGestorAccion accion)
        {
            try
            {
                if (cuadre == null) throw new Exception("Modelo de cuadre inválido.");

                bool acciontoSend = accion == CuadreGestorAccion.Retirar;

                var cuadretoSend = new CuadreDTO();
                cuadretoSend.CajaID = cuadre.CajaID;
                cuadretoSend.Tipo = cuadre.Tipo;
                cuadretoSend.UsuarioCaja = cuadre.UsuarioCaja;
                cuadretoSend.CajaOrigenID = cuadre.CajaOrigenID;
                cuadretoSend.UsuarioOrigenID = cuadre.UsuarioOrigenID;
                cuadretoSend.BalanceMinimo = cuadre.BalanceMinimo;
                cuadretoSend.MontoPorPagar = cuadre.MontoPorPagar;
                cuadretoSend.MontoContado = cuadre.MontoContado;
                cuadretoSend.MontoRetirado = cuadre.MontoRetirado;
                cuadretoSend.MontoDepositado = cuadre.MontoDepositado;
                cuadretoSend.AuxMensajeroNombre = cuadre.AuxMensajeroNombre;

                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(cuadretoSend));
                toSend.Add(acciontoSend.ToString());

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Cuadre_Registrar, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar el cuadre");
                }

                var cuadreRegistroResult = JSONHelper.CreateNewFromJSONNullValueIgnore<CuadreRegistroResultDTO>(llamada.Respuesta);

                return cuadreRegistroResult;
            }
            catch (Exception e)
            {
                throw e;
            }

        }// fin de metodo AplicarCuadre( )
    
        private bool AplicarBancaRecorrido(BancaDTO banca, CuadreRegistroDTO ope, CuadreRegistroResultDTO resultado)
        {
            bool cuadreFueEnlazado = false;

            if (
                 banca != null
                && ope != null
                && ope.Cuadre != null
                && ope.RutaAsignacion != null
                && ope.RutaAsignacion.RutaRecorridoDTO != null
                && resultado != null
             )
            {
                try
                {
                    string gestorUsuarioIdSerializado = JSONHelper.SerializeToJSON(ope.Cuadre.UsuarioOrigenID); // Serializando el usuario id del gestor enviado
                    var toSendUsuarioIdSerializado = new ArrayOfAnyType();
                    toSendUsuarioIdSerializado.Add(gestorUsuarioIdSerializado);


                    var call_Uno = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_LeerCajaDeUsuarioPorUsuarioId, toSendUsuarioIdSerializado);

                    if (call_Uno == null || call_Uno.OK == false)
                    {
                        throw new Exception("Hubo un error en procesar la operacion");
                    }

                    var cajaGestor = JSONHelper.CreateNewFromJSONNullValueIgnore<CajaDTO>(call_Uno.Respuesta);

                    int totalBancasRecorrer = ope.RutaAsignacion.RutaRecorridoDTO.Terminales.Count();

                    for (int indiceTerminal = 0; indiceTerminal < totalBancasRecorrer; indiceTerminal++)
                    {
                        if (ope.RutaAsignacion.RutaRecorridoDTO.Terminales[indiceTerminal].BancaID == banca.BancaID)
                        {
                            ope.RutaAsignacion.RutaRecorridoDTO.Terminales[indiceTerminal].FueRecorrida = true;
                            ope.RutaAsignacion.RutaRecorridoDTO.Terminales[indiceTerminal].BalanceAlCuadreGestor = cajaGestor.BalanceActual;
                            break;

                        }//fin de if
                    }//fin de for

                    #region Enlazar Cuadre Con Asignacion De Ruta

                    int totalBancasRecorridas = ope.RutaAsignacion.RutaRecorridoDTO.Terminales.Count(b => b.FueRecorrida);

                    string el_bancaid_es_la_ultima_localidad_recorridaToSend = JSONHelper.SerializeToJSON(banca.BancaID);
                    string recorridoEstadoToSend = totalBancasRecorridas == totalBancasRecorrer ? "Completada" : "En progreso";
                    string orden_recorrido_actualizacionToSend = JSONHelper.SerializeToJSON(ope.RutaAsignacion.RutaRecorridoDTO);
                    string cuadreid_a_enlazarToSend = JSONHelper.SerializeToJSON(resultado.CuadreID.Value);
                    string cajaid_de_la_banca_que_esta_siendo_recorridaToSend = JSONHelper.SerializeToJSON(ope.Cuadre.CajaID);
                    string rutaidToSend = JSONHelper.SerializeToJSON(ope.RutaAsignacion.RutaId);

                    var toSend = new ArrayOfAnyType();
                    toSend.Add(recorridoEstadoToSend);
                    toSend.Add(el_bancaid_es_la_ultima_localidad_recorridaToSend);
                    toSend.Add(orden_recorrido_actualizacionToSend);
                    toSend.Add(cuadreid_a_enlazarToSend);
                    toSend.Add(cajaid_de_la_banca_que_esta_siendo_recorridaToSend);
                    toSend.Add(rutaidToSend);

                    var call_Dos = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Cuadre_EnlazarCuadreConRuta, toSend);

                    if (call_Dos == null || call_Dos.OK == false)
                    {
                        throw new Exception("Hubo un error al enlazar el cuadre con asignacion de ruta");
                    }

                    cuadreFueEnlazado = JSONHelper.CreateNewFromJSONNullValueIgnore<bool>(call_Dos.Respuesta);

                    #endregion

                }//fin de try
                catch
                {
                    cuadreFueEnlazado = false;
                }// fin de catch 

            }//fin de if 

            return cuadreFueEnlazado;
        }// fin de metodo AplicarBancaRecorrido( )
    
        public CuadreRegistroResultDTO Registrar(BancaDTO banca, CuadreRegistroDTO ope, bool enablePrinting, out string toPrint)
        {
            toPrint = "";

            try
            {
                if (banca == null || ope == null || ope.Cuadre == null)
                {
                    throw new Exception("Operaciòn de cuadre inválida");
                }

                var resultado = AplicarCuadre(ope.Cuadre, ope.CuadreGestorAccion);

                if (resultado.FueProcesado)
                {
                    bool enlaceFueExitoso = AplicarBancaRecorrido(banca, ope, resultado);

                    GenerarCuadreImpresion(banca, ope, resultado, enablePrinting,out toPrint);

                }//fin de if

                return resultado;
            }
            catch
            {
                return new CuadreRegistroResultDTO() { Error = "Ha ocurrido un error al procesar la operaciòn de cuadre", FueProcesado = false };
            }

        }// fin de metodo Registrar()

        private void GenerarCuadreImpresion(BancaDTO banca, CuadreRegistroDTO ope, CuadreRegistroResultDTO cuadreAplicacionResultado, bool enablePrinting, out string toPrint)
        {
            toPrint = "";
            if (
                    enablePrinting == true
                &&  banca != null
                && ope != null
                && ope.Cuadre != null
                && cuadreAplicacionResultado != null
             )
            {
                var cuadreDocumentToPrint = new CuadreToPrintHelper();
                cuadreDocumentToPrint.BanContacto = banca.BanContacto;
                cuadreDocumentToPrint.BanDireccion = banca.BanDireccion;
                cuadreDocumentToPrint.BalanceDeCajaSegunSistema = cuadreAplicacionResultado.MontoReal.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                cuadreDocumentToPrint.EfectivoContadoEnCaja = ope.Cuadre.MontoContado.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                if (cuadreAplicacionResultado.MontoReal <= 0)
                {
                    if (ope.Cuadre.MontoContado == 0)
                    {
                        cuadreDocumentToPrint.FaltanteSobrante = ArqueoDeCajaResultado.Faltante;
                        cuadreDocumentToPrint.MontoFaltanteOMontoSobrante = (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                        cuadreDocumentToPrint.AUX_MontoFaltanteOMontoSobrante = 0;
                    }
                    else if (ope.Cuadre.MontoContado > 0)
                    {
                        ope.Cuadre.AuxMontoAFavor = ope.Cuadre.MontoContado;
                        cuadreDocumentToPrint.FaltanteSobrante = ArqueoDeCajaResultado.Sobrante;
                        cuadreDocumentToPrint.MontoFaltanteOMontoSobrante = ope.Cuadre.AuxMontoAFavor.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                        cuadreDocumentToPrint.AUX_MontoFaltanteOMontoSobrante = ope.Cuadre.AuxMontoAFavor;
                    }
                }
                else
                {
                    if ((cuadreAplicacionResultado.MontoReal - ope.Cuadre.MontoContado) < 0)
                    {
                        ope.Cuadre.AuxMontoAFavor = -1 * (cuadreAplicacionResultado.MontoReal - ope.Cuadre.MontoContado);
                        cuadreDocumentToPrint.FaltanteSobrante = ArqueoDeCajaResultado.Sobrante;
                        cuadreDocumentToPrint.MontoFaltanteOMontoSobrante = ope.Cuadre.AuxMontoAFavor.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                        cuadreDocumentToPrint.AUX_MontoFaltanteOMontoSobrante = ope.Cuadre.AuxMontoAFavor;
                    }
                    else if ((cuadreAplicacionResultado.MontoReal - ope.Cuadre.MontoContado) >= 0)
                    {
                        ope.Cuadre.MontoFaltante = cuadreAplicacionResultado.MontoReal - ope.Cuadre.MontoContado;
                        cuadreDocumentToPrint.FaltanteSobrante = ArqueoDeCajaResultado.Faltante;
                        cuadreDocumentToPrint.MontoFaltanteOMontoSobrante = (ope.Cuadre.MontoFaltante).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                        cuadreDocumentToPrint.AUX_MontoFaltanteOMontoSobrante = ope.Cuadre.MontoFaltante;
                    }
                }

                cuadreDocumentToPrint.Responsable = ope.Cuadre.UsuarioCaja;
                cuadreDocumentToPrint.BalanceFinalCaja = cuadreAplicacionResultado.NuevoBalance.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));

                if (ope.CuadreGestorAccion == CuadreGestorAccion.Depositar)
                {
                    ope.Cuadre.MontoRetirado = null;
                    cuadreDocumentToPrint.EsUnDeposito = true;
                    cuadreDocumentToPrint.BanMontoRetiradoODepositado = ope.Cuadre.MontoDepositado.HasValue ? ope.Cuadre.MontoDepositado.Value.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) : (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    cuadreDocumentToPrint.RecibidoPor = ope.Cuadre.UsuarioCaja;
                }
                else
                {
                    ope.Cuadre.MontoDepositado = null;
                    cuadreDocumentToPrint.EsUnDeposito = false;
                    cuadreDocumentToPrint.BanMontoRetiradoODepositado = ope.Cuadre.MontoRetirado.HasValue ? ope.Cuadre.MontoRetirado.Value.ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US")) : (0).ToString("$#,##0.00", CultureInfo.CreateSpecificCulture("en-US"));
                    cuadreDocumentToPrint.RecibidoPor = ope.Cuadre.AuxMensajeroNombre;
                }

                cuadreDocumentToPrint.CuadreTransaccion = cuadreAplicacionResultado.CuadreRef;
                cuadreDocumentToPrint.FechaTransaccion = cuadreAplicacionResultado.Fecha.ToString("dd MMMM yyyy hh:mm tt", new CultureInfo("es-ES"));
                toPrint = DocumentToPrintGeneratorHelper.GetCuadreDocument(cuadreDocumentToPrint);

            }//fin de if


        }//fin de metodo GenerarCuadreImpresion( )


    }//fin de clase
}
































