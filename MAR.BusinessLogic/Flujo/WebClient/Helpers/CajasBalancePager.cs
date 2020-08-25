using System;
using Dapper;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using Flujo.Entities.WebClient.POCO;
using MAR.BusinessLogic.Flujo.WebClient.Helpers.Base;

using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    [Serializable()]
    public class CajasBalancePager : Pager<ConsultaBalanceCaja>
    {
        public CajasBalancePager(int pUserID, int? pRiferoID) : base(SelectView.SelectCajasBalance, BuildSelectParameter(pUserID, pRiferoID))
        {
            this.ColeccioRecordSeleccion = new List<ConsultaBalanceCaja>();
            this.Search = "";
        }

        private static DynamicParameters BuildSelectParameter(int pUserID, int? pRiferoID)
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@UserID", pUserID);
            p.Add("@RiferoID", null);
            return p;
        }

        public List<ConsultaBalanceCaja> ColeccioRecordSeleccion
        {
            get;
            set;
        }

        public string Search { get; set; }

        public void ClearColeccionRecordSeleccion()
        {
            if (ColeccioRecordSeleccion != null && ColeccioRecordSeleccion.Count > 0)
            {
                this.ColeccioRecordSeleccion.Clear();
            }
        }

        public List<ConsultaBalanceCaja> GetPaginacion(int start, int pageSize, string search)
        {

            string findStr = Regex.Replace(search, @"\s+", " ").Trim();

            if (ColeccionRecords == null) return new List<ConsultaBalanceCaja>();

            if (findStr.Equals(string.Empty) || findStr.Equals(" "))
            {
                this.Search = "";

                this.ColeccioRecordSeleccion = ColeccionRecords.Skip((start - 1) * pageSize).Take(pageSize).ToList();

                base.TotalDeRecords = this.ColeccionRecords.Count();
            }
            else
            {
                this.Search = search;

                this.ColeccioRecordSeleccion = ColeccionRecords.Where(x => x.BancaNombreOTipoUsuarioNombre != null && x.BancaNombreOTipoUsuarioNombre.StartsWith(findStr, StringComparison.InvariantCultureIgnoreCase)).ToList();

                base.TotalDeRecords = this.ColeccioRecordSeleccion.Count();

                this.ColeccioRecordSeleccion = ColeccioRecordSeleccion.Skip((start - 1) * pageSize).Take(pageSize).ToList();

            }

            this.Start = start;
            this.PageSize = pageSize;

            return this.ColeccioRecordSeleccion;

        }


    }
}