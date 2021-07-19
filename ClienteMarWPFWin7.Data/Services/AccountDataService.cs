

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using ClienteMarWPFWin7.Domain.Models.Dtos;
using ClienteMarWPFWin7.Domain.Models.Entities;
using ClienteMarWPFWin7.Domain.Services.AccountService;
using ClienteMarWPFWin7.Data.Services.Helpers;

using ClienteMarWPFWin7.Domain.MarPuntoVentaServiceReference;

namespace ClienteMarWPFWin7.Data.Services
{
    public class AccountDataService : IAccountService
    {

        public static SoapClientRepository SoapClientesRepository;
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
                var sessionHacienda = new ClienteMarWPFWin7.Domain.HaciendaService.MAR_Session();
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

                var cincoMinutoService = new CincoMinutosDataService();
                var productos = cincoMinutoService.GetProductosDisponibles(cuenta);
                SessionGlobals.Productos = productos;
                SessionGlobals.cuentaGlobal = cuenta;

                var MoreOptions = cuenta.MAR_Setting2.MoreOptions.ToList();
                if (MoreOptions.Contains("BANCA_VENDE_CINCOMINUTOS|TRUE"))
                {
                    SessionGlobals.permisos = true;
                }
                else
                {
                    SessionGlobals.permisos = false;
                }

            }
            catch (Exception)
            {


            }

            return cuenta;
        }




    }
}
