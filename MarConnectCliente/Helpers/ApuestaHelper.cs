using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarConnectCliente.Helpers
{
    public class ApuestaHelper
    {
        public static string SeparaJugadaConGuion(string pJugada)
        {
            int guionesCount = pJugada.Length / 3;
            string jugadaSeperada = "";
            string primera = pJugada.Substring(0, 2);
            string segunda = pJugada.Length>2 ? "-" + pJugada.Substring(2, 2): "";
            string tercera = pJugada.Length > 4 ? "-" + pJugada.Substring(4, 2) : "";


            jugadaSeperada = primera + segunda + tercera;


            return jugadaSeperada;
        }
    }
}
