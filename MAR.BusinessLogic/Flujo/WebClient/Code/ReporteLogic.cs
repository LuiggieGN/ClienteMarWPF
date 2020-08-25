
using System;
using System.Collections.Generic;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using Flujo.Entities.WebClient.POCO;

namespace MAR.BusinessLogic.Flujo.WebClient.Code
{
    public static class ReporteLogic
    {
        
        public static List<SobranteFaltanteRecord> GetSumatoriaSobranteYFaltante(string pBanca, string pRifero, string pCajeraResponsable, string pTipoArqueo, DateTime pFInico, DateTime pFFin)
        {
            try
            {
                List<SobranteFaltanteRecord> ListadoTotales = ReportRepositorio.GetSumatoriaSobranteYFaltante(pBanca, pRifero, pCajeraResponsable, pTipoArqueo, pFInico, pFFin);

                if (ListadoTotales == null)
                {
                    ListadoTotales = new List<SobranteFaltanteRecord>();
                }

                return ListadoTotales;
            }
            catch (Exception ex)
            {
                return new List<SobranteFaltanteRecord>();
            }

        }

        public static List<string> GetCajeraReponsableFaltanteSobrante(string pSearch)
        {
            try
            {
                List<string> responsables = ReportRepositorio.GetCajeraReponsableFaltanteSobrante(pSearch);

                return responsables;
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }

        public static Dictionary<string, CategoriaBalance> GetBalancesTotalesPorCategoria()
        {
            Dictionary<string, CategoriaBalance> DiccionarioCategoria = new Dictionary<string, CategoriaBalance>();

            List<CategoriaBalance> ListaCategoria = ReportRepositorio.GetBalancesTotalesPorCategoria();

            if (ListaCategoria != null && ListaCategoria.Count > 0)
            {
                foreach (CategoriaBalance item in ListaCategoria)
                {
                    DiccionarioCategoria.Add(item.Categoria, item);
                }
            }
            return DiccionarioCategoria;
        }
    }
}
