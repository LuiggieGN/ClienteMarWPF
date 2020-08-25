using System;
using System.Linq;
using System.Collections.Generic;

using Flujo.Entities.WebClient.POCO;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    public class UniqueTokenHelper
    {
        public const int dMin    = 0;      // -Inclusivo
        public const int dMax    = 9999;   // -Inclusivo
        public const int N40Tokens = 40;     // -Numeros de tokens a generar.    

        public static List<SecurityToken> GetUnique40Tokens()
        {
            List<SecurityToken> LosTokens = new List<SecurityToken>();  Random ram = new Random(); string strToken = "";

            int Generados = 0;

            while ( Generados < N40Tokens)
            {

                if ( LosTokens.Count < 1 )
                {
                    ++Generados;  LosTokens.Add(new SecurityToken() { Posicion = Generados , Token = ram.Next(dMin, dMax + 1).ToString("D4") });
                }
                else
                {
                    strToken = ram.Next(dMin, dMax + 1).ToString("D4");

                    if (
                         ! ( LosTokens.Any( w => w.Token.Equals(strToken) ) )                       
                       )
                    {
                        ++Generados;
                        LosTokens.Add(new SecurityToken() { Posicion = Generados, Token = strToken });
                    }
                }

                if (Generados == N40Tokens) break;

            } return LosTokens;
        }
    }
}
