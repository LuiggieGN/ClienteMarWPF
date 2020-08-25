using System;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;

using Flujo.Entities.WebClient.POCO;
using Flujo.Entities.WebClient.ViewModels;

using MAR.BusinessLogic.Flujo.WebClient.Helpers.Base;

using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;

using System.Globalization;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    [Serializable()]
    public class TarjetaPager : Pager<TarjetaViewModel>
    {
        public List<TarjetaViewModel> Seleccion { get; set; }

        public string Search { get; set; }


        public TarjetaPager():base("", null)
        {
            Seleccion = new List<TarjetaViewModel>();

            Search = "";
        }

        public void ClearSeleccion()
        {
            if (Seleccion != null && Seleccion.Count > 0)
            {
                Seleccion.Clear();
            }
        }


        public List<TarjetaViewModel> GetPaginacion(int start, int pageSize, string search)
        {
            string findStr = Regex.Replace(search, @"\s+", " ").Trim();

            if (findStr.Equals(string.Empty) || findStr.Equals(" "))
            {
                this.Search = "";

                base.AplicaConsulta(SelectView.SelectTarjetas, null);

                this.Seleccion = ColeccionRecords.Skip((start - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                this.Search = search;

                if (ColeccionRecords == null)
                {
                    base.AplicaConsulta(SelectView.SelectTarjetas, null);
                }

                this.Seleccion = ColeccionRecords.Where(

                     x => (x.Clave         != null && x.Clave.ToLower().Contains(Search.ToLower()))
                       || (x.Propietario   != null && x.Propietario.ToLower().Contains(Search.ToLower()))
                       || (x.TipoDocumento != null && x.TipoDocumento.ToLower().Contains(Search.ToLower()))
                       || (x.NoDocumento   != null && x.NoDocumento.ToLower().Contains(Search.ToLower()))

                ).ToList();

                base.TotalDeRecords = this.Seleccion.Count();

                this.Seleccion = Seleccion.Skip((start - 1) * pageSize).Take(pageSize).ToList();

            }

            this.Start = start;

            this.PageSize = pageSize;

            return Seleccion;
        }


    }
}
