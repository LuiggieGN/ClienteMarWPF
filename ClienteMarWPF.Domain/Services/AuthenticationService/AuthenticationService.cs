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

        public async Task<CuentaUsuario> Login(string username, string password)
        {

            CuentaUsuario cuentaUsuario = await _accountService.GetByUserName(username);

            if (cuentaUsuario == null)  
            {
                throw new UserNotFoundException(username);
            }

            PasswordVerificationResult passwordResult = _passwordHasher.VerifyHashedPassword(cuentaUsuario.UsuarioHolder, string.Empty, string.Empty);

            if (passwordResult != PasswordVerificationResult.Success)
            {
                throw new InvalidPasswordException(username, password);
            }

            return cuentaUsuario;

        }// fin de metodo Login( )



    }// fin de clase AuthenticationService
}// fin de namespace AuthenticationService
