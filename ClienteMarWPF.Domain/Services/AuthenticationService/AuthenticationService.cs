using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using ClienteMarWPF.Domain.Models.Dtos;
using ClienteMarWPF.Domain.Models.Entities;
using ClienteMarWPF.Domain.Exceptions;

using ClienteMarWPF.Domain.Services.CuentaUsuarioService;


namespace ClienteMarWPF.Domain.Services.AuthenticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICuentaUsuarioService _cuentaServicio;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public AuthenticationService(ICuentaUsuarioService cuentaServicio, IPasswordHasher<Usuario> passwordHasher)
        {
            _cuentaServicio = cuentaServicio;
            _passwordHasher = passwordHasher;
        }

        public async Task<CuentaUsuario> Login(string username, string password)
        {

            CuentaUsuario cuentaUsuario = await _cuentaServicio.ConsultaCuentaUsuarioPorNombreUsuario(username);

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
