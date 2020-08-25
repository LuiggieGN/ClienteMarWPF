using System;
using System.Linq;
using System.Collections.Generic;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;

using MAR.BusinessLogic.Flujo.WebClient.Code;

namespace MAR.BusinessLogic.Flujo.WebClient.Code.Validations
{
    public static class TarjetaValidacion
    {
        public static List<TarjetaFormDataError> GetErrors(TarjetaFormData tarjeta)
        {
            List<TarjetaFormDataError> errores = new List<TarjetaFormDataError>();

            if (tarjeta == null)
            {
                errores.Add( new TarjetaFormDataError() { Codigo = 500, Error = "Tarjeta inválida." });

                return errores;
            }

            if (tarjeta.UsuarioDatos ==  null || tarjeta.UsuarioDatos.Posicion  <= 0)
            {
                errores.Add(new TarjetaFormDataError() { Codigo = 700, Error = "Usuario Inválido." });
            }

            if (tarjeta.Tokens == null || tarjeta.Tokens.Count != 40 || tarjeta.Tokens.Any( x => x.Token.Contains("x")))
            {
                errores.Add(new TarjetaFormDataError() { Codigo = 701, Error = "Generar tokens de tarjeta." });
            }

            if (errores.Any())
            {
                return errores;
            }

            if (UsuarioTokenLogic.UsuarioTieneTarjeta( tarjeta.UsuarioDatos.Posicion ))
            {
                errores.Add( new TarjetaFormDataError() {  Codigo= 800, Error = "Al usuario ya se le fue asignado una tarjeta"});
            }

            return errores;

        }
    }
}
