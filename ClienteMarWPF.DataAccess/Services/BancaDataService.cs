

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;

using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Services.BancaService;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.DataAccess.Services.Helpers;

using FlujoService;


namespace ClienteMarWPF.DataAccess.Services
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

        public Task<Banca> Create(Banca entity)
        {
            throw new NotImplementedException();
        }

        public Task<Banca> Get(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Update(Banca entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Delete(Banca entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> AddRange(List<Banca> entities)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Banca>> GetAll()
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

        }//fin de metodo LeerBancaConfiguraciones( )

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

        }// fin de metodo LeerBancaMontoReal( );



    }//fin de clase
}
































 