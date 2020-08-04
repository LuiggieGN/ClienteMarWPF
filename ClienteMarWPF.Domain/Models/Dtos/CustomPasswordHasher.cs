using System;
using Microsoft.AspNetCore.Identity;

using ClienteMarWPF.Domain.Models.Entities;

namespace ClienteMarWPF.Domain.Models.Dtos
{
    public class PersonalizedPasswordHasher : IPasswordHasher<Usuario>
    {
        public string HashPassword(Usuario user, string password)
        {

            //** Aqui vala implementacion que crea el hash del password

            //** CODIGO PARA CREAR EL HASH


            return string.Empty;
        }

        public PasswordVerificationResult VerifyHashedPassword(Usuario user, string hashedPassword, string providedPassword)
        {
             return  hashedPassword == HashPassword(user, providedPassword) 
                    ? PasswordVerificationResult.Success 
                    : PasswordVerificationResult.Failed;
        }

 




    }
}
