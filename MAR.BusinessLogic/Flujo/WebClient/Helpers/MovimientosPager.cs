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
    public class MovimientosPager : Pager<MovimientoDatos>
    {
        public List<MovimientoDatos> Seleccion { get; set; }    

        public string Search { get; set; }

        public MovimientosPager() : base("", null)
        {
            this.Seleccion = new List<MovimientoDatos>( );      this.Search     = "";
        }

        public void ClearSeleccion()
        {
            if (Seleccion != null && Seleccion.Count > 0)
            {
                Seleccion.Clear();
            }
        }


        public List<MovimientoDatos> GetPaginacionFromDataQueryBefore(int start, int pageSize)
        {
            if (this.ColeccionRecords == null)
            {
                return new List<MovimientoDatos>();
            }

            this.Seleccion = ColeccionRecords.Skip((start) * pageSize).Take(pageSize).ToList();
            this.Start = start;
            this.PageSize = pageSize;
            return this.Seleccion;

        }

        public List<MovimientoDatos> GetPaginacion(int start, int pageSize,  int pCajaID, DateTime f_Inicio,  DateTime f_Fin, string pCategoriaOperacion)
        {
 
                this.Search = "";
                DynamicParameters p = new DynamicParameters();
                p.Add("@CajaID", pCajaID);
                p.Add("@FechaInicio", f_Inicio.ToString("yyyyMMdd"));
                p.Add("@FechaFin", f_Fin.Date.ToString("yyyyMMdd"));
                p.Add("@CategoriaOperacion", pCategoriaOperacion);
            base.AplicaConsultaFromProcedure("[flujo].[Sp_ConsultaCajaMovimientos]", p);
                this.Seleccion = ColeccionRecords.Skip((start) * pageSize).Take(pageSize).ToList();
           

                this.Start = start;
                this.PageSize = pageSize;

                return this.Seleccion;
        }



    }
}
