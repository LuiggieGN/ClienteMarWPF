using System;
using System.Collections.Generic;
 

namespace Flujo.Entities.WebClient.POCO
{
    public class TarjetaFormData
    {
       public ConsultaUsuarioBalance UsuarioDatos { get; set; }

       public List<SecurityToken> Tokens { get; set; }

    }
}
