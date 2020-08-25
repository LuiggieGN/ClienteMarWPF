using System;
using System.Collections.Generic;

namespace Flujo.Entities.WebClient.POCO
{
    public class TarjetaRegistradaEstado
    {
        public bool TarjetaFueRegistrada { get; set; }
        public List<TarjetaFormDataError> Errores { get; set; }
    }
}
