using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MAR.DataAccess.ViewModels
{
    public class BaseViewModel
    {

        public class SqlDataResult
        {
            public object Result { get; set; }
            public bool OK { get; set; }
        }
        public static string GeneraPinGanador(int pSol)
        {
            var sb = new System.Text.StringBuilder();
            int ConfirmK = 0;
            var rdn = new Random(pSol);
            for (var n = 1; n < 4; n++)
            {
                var iK = rdn.Next(9);
                sb.Append(iK.ToString());
                ConfirmK += iK;
            }

            int seed = 1;
            var stringBuilder2 = new System.Text.StringBuilder();
            for (var x = 0; x < pSol.ToString().Length; x++)
            {
                seed = seed + (ConfirmK + Convert.ToInt32(pSol.ToString()[x].ToString()));
                var iK = seed % 10;
                stringBuilder2.Append(iK.ToString());
            }
            return sb.ToString() + stringBuilder2.ToString().Substring(stringBuilder2.Length - 5);
        }

       
        public static bool ComparaPinGanador(int pSol, string pPin)
        {
            var stringBuilder = new System.Text.StringBuilder();
            int ConfirmK = 0;
            var rdn = new Random(pSol);
            for (var n = 1; n < 4; n++)
            {
                var iK = rdn.Next(9);
                stringBuilder.Append(iK.ToString());
                ConfirmK += iK;
            }

            int seed = 1;
            var stringBuilder2 = new System.Text.StringBuilder();
            for (var x = 0; x < pSol.ToString().Length; x++)
            {
                seed = seed + (ConfirmK + Convert.ToInt32(pSol.ToString()[x].ToString()));
                var iK = seed % 10;
                stringBuilder2.Append(iK.ToString());
            }
            string keyGen = stringBuilder + stringBuilder2.ToString().Substring(stringBuilder2.Length - 5);
            return (pPin == keyGen);
        }
    }
}
