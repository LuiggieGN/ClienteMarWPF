 

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
            catch  
            {
                cuenta.MAR_Setting2 = null;
            }

            return cuenta;
        }

 


    }
}
