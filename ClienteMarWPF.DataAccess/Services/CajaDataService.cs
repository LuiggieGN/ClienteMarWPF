

using System;
using System.Linq;
using System.Globalization;

using System.Threading.Tasks;
using System.Collections.Generic;

using MAR.AppLogic.MARHelpers;

using ClienteMarWPF.Domain.Helpers;
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Services.CajaService;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.DataAccess.Services.Helpers;

using FlujoService;

namespace ClienteMarWPF.DataAccess.Services
{
    public class CajaDataService : ICajaService
    {
        public static SoapClientRepository soapClientesRepository;
        private static mar_flujoSoapClient efectivoSoapCliente;

        static CajaDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            efectivoSoapCliente = soapClientesRepository.GetCashFlowServiceClient(false);
        }

        public SupraMovimientoEnBancaResultDTO RegistrarMovimientoEnBanca(SupraMovimientoEnBancaDTO movimiento)
        {
            try
            {
                if (movimiento == null)
                {
                    throw new Exception("Movimiento inválido.");
                }

                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(movimiento));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_RegistrarMovimientoEnBanca, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al registrar el movimiento");
                }

                var result = JSONHelper.CreateNewFromJSONNullValueIgnore<SupraMovimientoEnBancaResultDTO>(llamada.Respuesta);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public SupraMovimientoDesdeHastaResultDTO RegistrarMovimientoDesdeHasta(SupraMovimientoDesdeHastaDTO transferencia)
        {
            try
            {
                if (transferencia == null)
                {
                    throw new Exception("Movimiento inválido.");
                }

                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(transferencia));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_RegistrarMovimientoDesdeHasta, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al registrar la transferencia");
                }

                var result = JSONHelper.CreateNewFromJSONNullValueIgnore<SupraMovimientoDesdeHastaResultDTO>(llamada.Respuesta);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public MultipleDTO<PagerResumenDTO, List<MovimientoDTO>> LeerMovimientos(MovimientoPageDTO paginaRequest)
        {
            try
            {
                if (paginaRequest == null)
                {
                    throw new Exception("Pagina Invalida");
                }

                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(paginaRequest));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_LeerMovimientos, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error en la lectura de la pagina");
                }

                var result = JSONHelper.CreateNewFromJSONNullValueIgnore<MultipleDTO<PagerResumenDTO, List<MovimientoDTO>>>(llamada.Respuesta);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public decimal LeerCajaBalance(int cajaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(cajaid)); 

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_LeerCajaBalance, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la lectura de balance de caja");
                }

                var balanceDeCajaAlConsultar = JSONHelper.CreateNewFromJSONNullValueIgnore<decimal>(llamada.Respuesta);

                return balanceDeCajaAlConsultar;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public CajaDTO LeerCajaDeUsuarioPorUsuarioId(int usuarioid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(usuarioid));

                var call_Uno = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_LeerCajaDeUsuarioPorUsuarioId, toSend);

                if (call_Uno == null || call_Uno.OK == false)
                {
                    throw new Exception("Hubo un error en procesar la operacion");
                }

                var caja = JSONHelper.CreateNewFromJSONNullValueIgnore<CajaDTO>(call_Uno.Respuesta);

                return caja;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }//fin de clase
}
































