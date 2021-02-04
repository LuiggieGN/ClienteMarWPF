using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using MAR.AppLogic.MARHelpers;

using ClienteMarWPF.Domain.Helpers;
using ClienteMarWPF.Domain.Enums;
using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Services.JuegaMasService;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.DataAccess.Services.Helpers;

using JuegaMasService;

namespace ClienteMarWPF.DataAccess.Services
{
    public class JuegaMasDataService : IJuegaMasService
    {
        public static SoapClientRepository soapClientesRepository;
        private static JuegaMasSoapClient juegaMasSoapCliente;

        static JuegaMasDataService()
        {
            soapClientesRepository = new SoapClientRepository();
            juegaMasSoapCliente = soapClientesRepository.GetJuegaMasServiceClient(false);
        }


        public List<object> LeerReporteJuegaMas(MAR_Session sesion)
        {
            try
            {

                var toSend = new ArrayOfAnyType();
                toSend.Add(JSONHelper.SerializeToJSON(0)); // Parametro a enviar al servicio

                SessionGlobals.GenerateNewSolicitudID(sesion.Sesion, true);

                var llamada = juegaMasSoapCliente.CallJuegaMaxIndexFunction((int)JuegaMasFunciones.ReporteJuegaMas, sesion, toSend, (int)SessionGlobals.SolicitudID);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al leer los tipos ingresos o tipo egresos anonimos");
                }

                var anonimos = JSONHelper.CreateNewFromJSONNullValueIgnore<List<object>>(llamada.Respuesta);

                return anonimos;
            }
            catch
            {
                return new List<object>();

            }// fin de catch

        }// fin de metodo LeerTiposAnonimos()



    }//fin de clase
}
































