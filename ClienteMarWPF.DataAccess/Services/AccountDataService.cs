 

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;

using ClienteMarWPF.Domain.Services.AccountService;
using ClienteMarWPF.DataAccess.Services.Helpers;

using MarPuntoVentaServiceReference;

namespace ClienteMarWPF.DataAccess.Services
{
    public class AccountDataService : IAccountService
    {

        public  static SoapClientRepository SoapClientesRepository; 
        private static PtoVtaSoapClient MarCliente;
        static AccountDataService()
        {
            SoapClientesRepository = new SoapClientRepository();
            MarCliente = SoapClientesRepository.GetMarServiceClient(false);
        }


        public CuentaDTO Logon2(string usuario, string clave, int bancaid, string ipaddress)
        {
            CuentaDTO cuenta = new CuentaDTO();
            cuenta.UsuarioDTO = new UsuarioDTO();
            cuenta.UsuarioDTO.UsuUserName = usuario;
            cuenta.UsuarioDTO.UsuClave = clave;

            try
            {
                
                cuenta.MAR_Setting2 = MarCliente.Logon2(usuario, clave, bancaid, ipaddress);
            }
            catch (Exception ex) 
            { 
                cuenta.MAR_Setting2 = null;
            }

            try
            {
                var sorteos = new SorteosDataService();
                HaciendaService.MAR_Session sessionHacienda = new HaciendaService.MAR_Session();
                if (cuenta.MAR_Setting2 != null)
                {
                    MAR_Session sessionPuntoVenta = cuenta.MAR_Setting2.Sesion;
                    sessionHacienda.Banca = sessionPuntoVenta.Banca;
                    sessionHacienda.Usuario = sessionPuntoVenta.Usuario;
                    sessionHacienda.Sesion = sessionPuntoVenta.Sesion;
                    sessionHacienda.Err = sessionPuntoVenta.Err;
                    sessionHacienda.LastTck = sessionPuntoVenta.LastTck;
                    sessionHacienda.LastPin = sessionPuntoVenta.LastPin;
                    sessionHacienda.PrinterSize = sessionPuntoVenta.PrinterSize;
                    sessionHacienda.PrinterHeader = sessionPuntoVenta.PrinterHeader;
                    sessionHacienda.PrinterFooter = sessionPuntoVenta.PrinterFooter;
                    var sorteosdisponibles = sorteos.GetSorteosDisponibles(sessionHacienda);
                    SessionGlobals.GetLoteriasDisponibles(cuenta.MAR_Setting2.Loterias, sorteosdisponibles);
                    SessionGlobals.GenerateNewSolicitudID(cuenta.MAR_Setting2.Sesion.Sesion);
                }

            }
            catch (Exception)
            {


            }

            return cuenta;
        }

 


    }
}
