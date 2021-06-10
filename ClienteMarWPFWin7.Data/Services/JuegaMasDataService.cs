using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using MAR.AppLogic.MARHelpers;

using ClienteMarWPFWin7.Domain.Helpers;
using ClienteMarWPFWin7.Domain.Enums;
using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Dtos.EfectivoDtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.Services.JuegaMasService;
using ClienteMarWPFWin7.Domain.Exceptions;

using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.JuegaMasService;
using System.Windows.Forms;

namespace ClienteMarWPFWin7.Data.Services
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


        public MAR_JuegaMasResponse LeerReporteEstadoDePremiosJuegaMas(MAR_Session sesion, DateTime Fecha)
        {
            try
            {

                var toSend = new ArrayOfAnyType();
                string[] arrayString = new string[1];
                arrayString[0] = Convert.ToDateTime(Fecha).ToString("yyyy-MM-dd HH:mm:ss.fff",CultureInfo.InvariantCulture);


                toSend.Add(JSONHelper.SerializeToJSON(arrayString));// Parametro a enviar al servicio
                
                SessionGlobals.GenerateNewSolicitudID(sesion.Sesion, true);

                var llamada = juegaMasSoapCliente.CallJuegaMaxIndexFunction((int)JuegaMasFunciones.ReporteListadoPremio, sesion, toSend, (int)SessionGlobals.SolicitudID);

                if (llamada == null || llamada.OK == false)
                {
                    throw new Exception("Ha ocurrido un error al leer los tipos ingresos o tipo egresos anonimos");
                }

                MAR_JuegaMasResponse repuesta = new MAR_JuegaMasResponse() {OK=llamada.OK,Mensaje=llamada.Mensaje,Err=llamada.Err,Respuesta=llamada.Respuesta };
                
                //var anonimos = JSONHelper.CreateNewFromJSONNullValueIgnore<List<object>>(llamada.Respuesta);

                return repuesta;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return new MAR_JuegaMasResponse();

            }// fin de catch

        }// fin de metodo LeerTiposAnonimos()



    }//fin de clase
}
































