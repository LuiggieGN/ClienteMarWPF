using System;
 
using ClienteMarWPF.Domain.Enums;

namespace ClienteMarWPF.Domain.Helpers
{

    public static class CuadreHelper
    {
        public static string Get(CuadreTipo t)
        {
            switch (t)
            {
                case CuadreTipo.Inicial:
                    return "INICIO";
                case CuadreTipo.Manual:
                    return "MANUAL";
                case CuadreTipo.Sistema:
                    return "SISTEMA";
                default:
                    throw new Exception("Este tipo de cuadre no existe");
            }//fin de switch

        }//fin de metodo

    }// fin de clase






}
