

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
using ClienteMarWPFWin7.Domain.Services.MultipleService;
using ClienteMarWPFWin7.Domain.Exceptions;

using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.FlujoService;

namespace ClienteMarWPFWin7.Data.Services
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
































