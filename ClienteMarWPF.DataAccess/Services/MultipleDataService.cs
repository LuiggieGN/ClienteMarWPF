

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
using ClienteMarWPF.Domain.Services.MultipleService;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.DataAccess.Services.Helpers;

using FlujoService;

namespace ClienteMarWPF.DataAccess.Services
{
    public class MultipleDataService : IMultipleService
    {
        public static SoapClientRepository soapClientesRepository;
        private static mar_flujoSoapClient efectivoSoapCliente;

        static MultipleDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            efectivoSoapCliente = soapClientesRepository.GetCashFlowServiceClient(false);
        }

        public MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO> LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario(string pin)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(pin);

                var call_Uno = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Multiple_LeerUsuarioSuCajaYSuTarjetaPorPinDeUsuario, toSend);

                if (call_Uno == null || call_Uno.OK == false)
                {
                    throw new Exception("Hubo un error en procesar la operacion");
                }

                var multi = JSONHelper.CreateNewFromJSONNullValueIgnore<MultipleDTO<MUsuarioDTO, CajaDTO, TarjetaDTO>>(call_Uno.Respuesta);

                return multi;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }//fin de clase
}
































