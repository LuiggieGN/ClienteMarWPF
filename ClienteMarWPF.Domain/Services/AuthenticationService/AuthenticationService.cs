using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Exceptions;
using ClienteMarWPF.Domain.Services.AccountService;


namespace ClienteMarWPF.Domain.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAccountService _accountService;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public AuthenticationService(IAccountService accountService,
                                     IPasswordHasher<Usuario> passwordHasher)
        {
            _accountService = accountService;
            _passwordHasher = passwordHasher;
        }

        public CuentaDTO Logon2(string usuario, string clave, int bancaid, string ipaddress)
        {

            CuentaDTO cuentaDto = _accountService.Logon2(usuario, clave, bancaid, ipaddress);

            if (cuentaDto == null ||
                cuentaDto.MAR_Setting2 == null ||
                cuentaDto.MAR_Setting2.Sesion == null ||
                cuentaDto.MAR_Setting2.Sesion.Err != null
                )
            {
                throw new UserNotFoundException(cuentaDto?.MAR_Setting2?.Sesion?.Err ?? "No se pudo establecer conexión al servicio de MAR.", usuario);
            }



            //PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(cuentaUsuario.UsuarioHolder, string.Empty, string.Empty);

            //if (passwordResult != PasswordVerificationResult.Success)
            //{
            //    throw new InvalidPasswordException(username, password);
            //}

            return cuentaDto;

        }// fin de metodo Login( )



    }// fin de clase AuthenticationService
}// fin de namespace AuthenticationService
