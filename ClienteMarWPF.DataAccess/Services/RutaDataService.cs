

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
using ClienteMarWPF.Domain.Services.RutaService;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.DataAccess.Services.Helpers;

using FlujoService;

namespace ClienteMarWPF.DataAccess.Services
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
































