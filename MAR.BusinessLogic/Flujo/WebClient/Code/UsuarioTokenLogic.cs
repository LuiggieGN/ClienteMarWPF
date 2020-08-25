using System;
using System.Linq;
using System.Collections.Generic;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public class UsuarioTokenLogic
    {
        
        //  true  : Si el usuario ya posee tarjeta
        //  false : No el usuario no posee tarjeta

        public static bool UsuarioTieneTarjeta(int pUsuarioID)
        {
            int TarjetaUsuarioID = UsuarioRepositorio.GetUsuarioTarjetaID(pUsuarioID);

            if (TarjetaUsuarioID == -1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static AsignacionTarjetaEstado ConsultaTarjeta(int pUsuarioID)
        {
            AsignacionTarjetaEstado ConsultaTarjeta = new AsignacionTarjetaEstado();

            ConsultaTarjeta.PoseeTarjeta = UsuarioTieneTarjeta(pUsuarioID);

            if (ConsultaTarjeta.PoseeTarjeta)
            {
                ConsultaTarjeta.Tarjeta = TarjetaRepositorio.GetTarjeta(pUsuarioID);

                if (ConsultaTarjeta.Tarjeta == null)
                {
                    ConsultaTarjeta.PoseeTarjeta = false;
                }
            }
            else
            {
                ConsultaTarjeta.Tarjeta = null;
            }

            return ConsultaTarjeta;
        }
        
        public static bool CrearTarjetaUsuario(int pUsuarioID, List<SecurityToken> pTokens)
        {
            bool TarjetaFueCreada = TarjetaRepositorio.CrearTarjeta(pUsuarioID, pTokens);
                
            return TarjetaFueCreada;
        }

        public static bool ActualizarTarjetaTokens(int pUsuarioID, List<SecurityToken> pTokens)
        {
            if (pTokens != null && pTokens.Count == 40)
            {
                bool TokensFueronActualizados = TarjetaRepositorio.ActualizarTarjeta(pUsuarioID, pTokens);

                return TokensFueronActualizados;
            }
            else
            {
                return false;
            }
        }

    }
}
