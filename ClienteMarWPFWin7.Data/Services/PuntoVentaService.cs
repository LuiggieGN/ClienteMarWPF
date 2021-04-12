

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;

using ClienteMarWPFWin7.Domain.Services.PuntoVentaService;
using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;
using ClienteMarWPFWin7.Domain.Enums;


namespace ClienteMarWPFWin7.Data.Services
{
    public class PtoVaService : IPtoVaService
    {
        public static SoapClientRepository SoapClientesRepository;
        private static PtoVtaSoapClient MarPuntoDeVentaService;
        static PtoVaService()
        {
            SoapClientesRepository = new SoapClientRepository(); MarPuntoDeVentaService = SoapClientesRepository.GetMarServiceClient(false);
        }

        public InicioPCResultDTO IniciarPC(int bancaid, string bancaip_And_Hwkey)
        {
            var result = new InicioPCResultDTO();

            try
            {
                result.InicioPCResponse = MarPuntoDeVentaService.Init(bancaid, bancaip_And_Hwkey);
                result.SevidorConexion = ServicioMarConexion.Conectado;
            }
            catch  
            {
                result.InicioPCResponse = new MAR_Array();
                result.InicioPCResponse.Err = "Ha ocurrido un error al establecer conexión con el servicio de MAR. verificar conexión de internet";
                result.SevidorConexion = ServicioMarConexion.NoConectado;
            }
            return result;
        }


        public RegistroPCResultDTO RegistraCambioPC(int bancaid, string hwkey)
        {
            var registroTerminal = new RegistroPCResultDTO();

            try
            {
                string response = MarPuntoDeVentaService.Init2(bancaid, hwkey)?.Trim() ?? null;

                if (response == null || response == string.Empty)
                {
                    registroTerminal.FueExitoso = false;
                    registroTerminal.Mensaje = string.Empty;
                    registroTerminal.CertificadoNumero = "0";
                }
                else
                {
                    if (!response.Substring(0, 2).Equals("OK"))
                    {
                        registroTerminal.FueExitoso = false;
                        registroTerminal.Mensaje = response;
                        registroTerminal.CertificadoNumero = "0";
                    }
                    else
                    {
                        registroTerminal.FueExitoso = true;
                        registroTerminal.Mensaje = response;
                        registroTerminal.CertificadoNumero = response.Substring(2);
                    }
                }
            }
            catch
            {
                registroTerminal.FueExitoso = false;
                registroTerminal.Mensaje = "Ha ocurrido un error al procesar la operación";
                registroTerminal.CertificadoNumero = "0";
            }

            return registroTerminal;
        }










    }
}
