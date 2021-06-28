

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;

using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.Services.BancaService;
using ClienteMarWPFWin7.Domain.Exceptions;
using ClienteMarWPFWin7.Domain.Helpers;

using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.FlujoService;
using System.Collections.ObjectModel;
using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;

namespace ClienteMarWPFWin7.Data.Services
{
    public class BancaDataService : IBancaService
    {
        public static SoapClientRepository soapClientesRepository;
        private static mar_flujoSoapClient efectivoSoapCliente;

        static BancaDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            efectivoSoapCliente = soapClientesRepository.GetCashFlowServiceClient(false);
        }

        public Task<BancaDTO> Create(BancaDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<BancaDTO> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(BancaDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(BancaDTO entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRange(List<BancaDTO> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BancaDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public BancaConfiguracionDTO LeerBancaConfiguraciones(int bancaid)
        {
            try
            {
                string bancaidSerializado = JSONHelper.SerializeToJSON(bancaid);

                ArrayOfAnyType diccionarioParametros = new ArrayOfAnyType();
                diccionarioParametros.Add(bancaidSerializado);

                MAR_FlujoResponse servicioClienteRespuesta = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerBancaConfiguraciones, diccionarioParametros);

                if (servicioClienteRespuesta != null && servicioClienteRespuesta.OK == true)
                {
                    return JSONHelper.CreateNewFromJSONNullValueIgnore<BancaConfiguracionDTO>(servicioClienteRespuesta.Respuesta);
                }
                else
                {
                    if (servicioClienteRespuesta == null)
                    {
                        throw new BancaConfiguracionesException("No se pudo establecer conexión al servicio de MAR.", "Control Efectivo Configuración");
                    }
                    else
                    {
                        throw new BancaConfiguracionesException("Ha ocurrido un error al obtener la Configuracion de Banca", "Control Efectivo Configuración");
                    }
                }
            }
            catch
            {
                throw new BancaConfiguracionesException("Ha ocurrido un error al obtener la Configuracion de Banca", "Control Efectivo Configuración");
            }
        } 

        public decimal LeerBancaMontoReal(int bancaid)
        {
            try
            {
                var bancaidSerializado = JSONHelper.SerializeToJSON(bancaid);
                var segmento1 = new ArrayOfAnyType();
                segmento1.Add(bancaidSerializado);

                MAR_FlujoResponse call_Uno = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerBancaLastCuadreId, segmento1);

                if (call_Uno == null || call_Uno.OK == false)
                {
                    return -1;
                }

                MAR_FlujoResponse call_Dos = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerBancaLastTransaccionesApartirDelUltimoCuadre, segmento1);

                if (call_Dos == null || call_Dos.OK == false)
                {
                    return -1;
                }


                int? cuadreid = JSONHelper.CreateNewFromJSONNullValueIgnore<int?>(call_Uno.Respuesta);
                var transaciones = JSONHelper.CreateNewFromJSONNullValueIgnore<List<BancaControlEfectivoTransaccionDTO>>(call_Dos.Respuesta);


                decimal montoreal = 0, totIngresos = 0, totEgresos = 0;

                if (transaciones != null && transaciones.Count > 0)
                {
                    totIngresos = transaciones.Where(b => b.IngresoOEgreso.Equals("i")).Sum(b => b.MontoAcumulado);
                    totEgresos = transaciones.Where(b => b.IngresoOEgreso.Equals("e")).Sum(b => b.MontoAcumulado);
                }

                if (cuadreid == null || (!cuadreid.HasValue))
                {
                    montoreal = totIngresos - totEgresos;
                }
                else
                {

                    var cuadreidSerializado = JSONHelper.SerializeToJSON(cuadreid.Value);
                    var segmento2 = new ArrayOfAnyType();
                    segmento2.Add(cuadreidSerializado);

                    MAR_FlujoResponse call_Tres = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerBancaCuadrePorCuadreId, segmento2);

                    if (call_Tres == null || call_Tres.OK == false)
                    {
                        return -1;
                    }

                    var cuadre = JSONHelper.CreateNewFromJSONNullValueIgnore<CuadreDTO>(call_Tres.Respuesta);

                    montoreal = (cuadre.Balance + totIngresos) - totEgresos;
                }

                return montoreal;

            }
            catch
            {
                return -1;
            }

        } 


        public bool BancaUsaControlEfectivo(int bancaid, bool incluyeConfig)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));
                toSend.Add(JSONHelper.SerializeToJSON(incluyeConfig));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_UsaControlEfectivo, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la operacion");
                }

                var usaControlEfectivo = JSONHelper.CreateNewFromJSONNullValueIgnore<bool>(llamada.Respuesta);

                return usaControlEfectivo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public decimal LeerDeudaDeBanca(int bancaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerDeudaDeBanca, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la lectura de la deuda de la banca");
                }

                var deuda = JSONHelper.CreateNewFromJSONNullValueIgnore<decimal>(llamada.Respuesta);

                return deuda;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int LeerInactividad(int bancaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerInactividad, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la lectura de la deuda de la banca");
                }

                var inactividad = JSONHelper.CreateNewFromJSONNullValueIgnore<int>(llamada.Respuesta);

                return inactividad;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public List<MarOperacionDTO> LeerBancaMarOperacionesDia(int bancaid, string strdia_yyyyMMdd)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));
                toSend.Add(strdia_yyyyMMdd);

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerBancaMarOperacionesDia, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la lectura de operaciones en MAR");
                }

                var operaciones = JSONHelper.CreateNewFromJSONNullValueIgnore<List<MarOperacionDTO>>(llamada.Respuesta);

                return operaciones;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string LeerBancaComandoRemoteCmd(int bancaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerBancaRemoteCmdCommand, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la lectura del comando remote del command promt");
                }

                var comando = JSONHelper.CreateNewFromJSONNullValueIgnore<string>(llamada.Respuesta);

                return comando;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool LeerEstadoBancaEstaActiva(int bancaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerEstadoBancaEstaActiva, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la lectura del estado banca esta activa");
                }

                var laBancaEstaActiva = JSONHelper.CreateNewFromJSONNullValueIgnore<bool>(llamada.Respuesta);

                return laBancaEstaActiva;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public decimal LeerVentaDeHoyDeLoterias(int bancaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerVentaDeHoyDeLoterias, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al leer la venta de hoy para esta banca");
                }

                var totalvendido = JSONHelper.CreateNewFromJSONNullValueIgnore<decimal>(llamada.Respuesta);

                return totalvendido;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public decimal LeerVentaDeHoyDeProductos(int bancaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerVentaDeHoyDeProductos, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al leer la venta de hoy para esta banca");
                }

                var totalvendido = JSONHelper.CreateNewFromJSONNullValueIgnore<decimal>(llamada.Respuesta);

                return totalvendido;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool LeerBancaTicketFueAnulado(string noTicket)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(noTicket);

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerTicketFueAnulado, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al leer si el ticket fue anulado enviando no. de ticket");
                }

                var ticketFueAnulado = JSONHelper.CreateNewFromJSONNullValueIgnore<bool>(llamada.Respuesta);

                return ticketFueAnulado;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public bool Rel(int bancaid, string hwkey)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));
                toSend.Add(hwkey);
                toSend.Add(SendQuery.Reload);

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_Rel, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Error al realizar funcion rel");
                }

                var funcionFueEjecutada = JSONHelper.CreateNewFromJSONNullValueIgnore<bool>(llamada.Respuesta);

                return funcionFueEjecutada;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public ObservableCollection <TicketDTO> LeerTicketsHoy(int bancaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(bancaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Banca_LeerTicketsHoy, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al leer tickets de ventas de hoy para esta banca");
                }

                ObservableCollection<TicketDTO> ticketsDeHoy = JSONHelper.CreateNewFromJSONNullValueIgnore<ObservableCollection<TicketDTO>>(llamada.Respuesta);

                return ticketsDeHoy;
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }
    }//fin de clase
}
































 