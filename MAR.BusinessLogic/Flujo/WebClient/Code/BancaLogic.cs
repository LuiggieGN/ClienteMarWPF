using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Flujo.Entities.WebClient.Enums;
using Flujo.Entities.WebClient.POCO;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public static class BancaLogic
    {

        public static Banca GetBancaPorCajaID(int pCajaID)
        {
            try
            {
                Banca b = BancaRepositorio.GetBancaPorCajaID(pCajaID);
                return b;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public static List<Banca> GetBancas(string pSereach, List<int> pListadoBancaANoBuscar)
        {
            try
            {
                List<Banca> bancas = BancaRepositorio.GetBancas(pSereach, pListadoBancaANoBuscar);

                return bancas;
            }
            catch (Exception ex)
            {

                return new List<Banca>(); 
            }

        }
 
        public static List<BancaRifero> GetBancaRiferos(string pSereach) 
        {
            List<BancaRifero> LaColleccionDeBancasYRiferos = BancaRepositorio.GetBancasYSuRifero(pSereach);

            return LaColleccionDeBancasYRiferos;
        }


        public static List<BancaRuta> GetBancasActivas()
        {
            List<BancaRuta> LaColleccionDeBancas = BancaRepositorio.GetBancasActivas();

            return LaColleccionDeBancas;
        }


        public static List<BancaBalance> GetBancasBalancePorNombre(string pNombre)
        {
            List<BancaBalance> ConsultaDeColleccionDeBancas = BancaRepositorio.GetBancasBalancePorNombre(pNombre);

            return ConsultaDeColleccionDeBancas;

        }


        public static bool BancaYCajaSonValidos(int pBancaID, int pCajaID)
        {
            bool EstanRelacionados = BancaRepositorio.BancaYCajaSonValidos(pBancaID, pCajaID);

            return EstanRelacionados;

        }




    }
}
