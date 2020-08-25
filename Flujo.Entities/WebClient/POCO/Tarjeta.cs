using System;
using System.Collections.Generic; 

namespace Flujo.Entities.WebClient.POCO
{
    public class Tarjeta
    {
        public string              Pin    { get; set; }
        public List<SecurityToken> Tokens { get; set; }
    }
}
