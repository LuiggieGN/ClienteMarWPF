
using MAR.DataAccess.ControlEfectivoRepositories;
using MAR.DataAccess.Tables.ControlEfectivoDTOs;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Code.ControlEfectivo
{
    public static class BancaLogic
    {
        public static BancaConfiguracionDTO LeerBancaConfiguraciones(int bancaid)
        {
            try
            {
                return BancaRepository.LeerBancaConfiguraciones(bancaid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static int? LeerBancaLastCuadreId(int bancaid)
        {
            try
            {
                return BancaRepository.LeerBancaLastCuadreId(bancaid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static List<BancaControlEfectivoTransaccionDTO> LeerBancaLastTransaccionesApartirDelUltimoCuadre(int bancaid)
        {
            try
            {
                return BancaRepository.LeerBancaLastTransaccionesApartirDelUltimoCuadre(bancaid);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static CuadreDTO LeerBancaCuadrePorCuadreId(int cuadreid)
        {
            try
            {
                return BancaRepository.LeerBancaCuadrePorCuadreId(cuadreid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool BancaUsaControlEfectivo(int bancaid, bool incluyeConfig)
        {
            try
            {
                return BancaRepository.BancaUsaControlEfectivo(bancaid, incluyeConfig);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static decimal LeerDeudaDeBanca(int bancaid)
        {
            try
            {
                return BancaRepository.LeerDeudaDeBanca(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static int LeerBancaInactividad(int bancaid)
        {
            try
            {
                return BancaRepository.LeerBancaInactividad(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<MarOperacionDTO> LeerBancaMarOperacionesDia(int bancaid, string strDia)
        {
            try
            {
                DateTime fcompleta;

                try
                {
                    fcompleta = DateTime.ParseExact(strDia,"yyyyMMdd",CultureInfo.InvariantCulture);
                }
                catch  
                {
                    fcompleta = DateTime.MinValue;
                } 
                return BancaRepository.LeerBancaMarOperacionesDia(bancaid,fcompleta);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static string LeerBancaRemoteCmdCommand(int bancaid)
        {
            try
            {
                return BancaRepository.LeerBancaRemoteCmdCommand(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool LeerEstadoBancaEstaActiva(int bancaid)
        {
            try
            {
                return BancaRepository.LeerEstadoBancaEstaActiva(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static decimal LeerVentaDeHoyDeLoterias(int bancaid)
        {
            try
            {
                return BancaRepository.LeerVentaDeHoyDeLoterias(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static decimal LeerVentaDeHoyDeProductos(int bancaid)
        {
            try
            {
                return BancaRepository.LeerVentaDeHoyDeProductos(bancaid);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static bool LeerBancaTicketFueAnulado(string noTicket)
        {
            try
            {
                return BancaRepository.LeerBancaTicketFueAnulado(noTicket); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





    }//fin de clase
}
