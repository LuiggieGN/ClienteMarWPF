﻿

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
using ClienteMarWPFWin7.Domain.Services.CajaService;
using ClienteMarWPFWin7.Domain.Exceptions;

using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.FlujoService;

namespace ClienteMarWPFWin7.Data.Services
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


        public List<MovimientoDTO> LeerMovimientosNoPaginados(MovimientoPageDTO paginaRequest)
        {
            try
            {
                if (paginaRequest == null)
                {
                    throw new Exception("Pagina Invalida");
                }

                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(paginaRequest));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_LeerMovimientosNoPaginados, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error en la lectura de la pagina");
                }

                var result = JSONHelper.CreateNewFromJSONNullValueIgnore<List<MovimientoDTO>>(llamada.Respuesta);

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

        public decimal LeerCajaBalanceMinimo(int cajaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(cajaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_LeerCajaBalanceMinimo, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la lectura del balance minimo");
                }

                var balanceMinimo = JSONHelper.CreateNewFromJSONNullValueIgnore<decimal>(llamada.Respuesta);

                return balanceMinimo;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool SetearCajaDisponibilidad(CajaDisponibilidadDTO disponibilidad)
        {
            try
            {
                if (disponibilidad == null)
                {
                    throw new Exception("Configuración de disponibilidad inválida.");
                }

                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(disponibilidad));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_SetearCajaDisponibilidad, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al configurar la disponibilidad de caja");
                }

                var result = JSONHelper.CreateNewFromJSONNullValueIgnore<bool>(llamada.Respuesta);

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool LeerCajaExiste(int cajaid)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(cajaid));

                var llamada = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Caja_LeerCajaExiste, toSend);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al procesar la operacion de lectura");
                }

                var existe = JSONHelper.CreateNewFromJSONNullValueIgnore<bool>(llamada.Respuesta);

                return existe;
            }
            catch (Exception e)
            {
                throw e;
            }
        }






    }//fin de clase
}
































