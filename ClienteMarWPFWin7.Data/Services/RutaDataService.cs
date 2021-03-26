

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
using ClienteMarWPFWin7.Domain.Services.RutaService;
using ClienteMarWPFWin7.Domain.Exceptions;

using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.FlujoService;

namespace ClienteMarWPFWin7.Data.Services
{
    public class RutaDataService : IRutaService
    {
        public static SoapClientRepository soapClientesRepository;
        private static mar_flujoSoapClient efectivoSoapCliente;

        static RutaDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            efectivoSoapCliente = soapClientesRepository.GetCashFlowServiceClient(false);
        }

        public RutaAsignacionDTO LeerGestorAsignacionPendiente(int gestorUsuarioId, int bancaId)
        {
            try
            {
                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(gestorUsuarioId));
                toSend.Add(JSONHelper.SerializeToJSON(bancaId));

                var call_Uno = efectivoSoapCliente.CallControlEfectivoFunciones((int)EfectivoFunciones.Ruta_LeerGestorAsignacionPendiente, toSend);

                if (call_Uno == null || call_Uno.OK == false)
                {
                    throw new Exception("Hubo un error en procesar la operacion");
                }

                var RutaAsignacionPendiente = JSONHelper.CreateNewFromJSONNullValueIgnore<RutaAsignacionDTO>(call_Uno.Respuesta);

                return RutaAsignacionPendiente;
            }
            catch (Exception e)
            {
                throw e;
            };
        }


    }//fin de clase
}
































