using System;
using Dapper;
using System.Linq;
using System.Collections.Generic;

using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers.Base
{
    [Serializable()]
    public class Pager<T>
    {

        public   int           Start             { get; set; }
        public   int           PageSize          { get; set; }
        private  string        SourceQuery       { get; set; }
        public   int           TotalDeRecords    { get; set; }
        public   List<T>       ColeccionRecords  { get; set; }

        public Pager( string  query ) : this ( query, null)  {      }

        public Pager( string query,  DynamicParameters p)
        {
            this.SourceQuery         = query;

            if (query != string.Empty)
            {
                AplicaConsulta(query, p);
            }
        }

        public virtual List<T> GetPaginacion(int start, int pageSize)
        {
            this.Start        = start;

            this.PageSize  = pageSize;

            if (ColeccionRecords == null)  return new List<T>();

            List<T> ColeccionSubRecords = ColeccionRecords.Skip((start - 1) * pageSize).Take(pageSize).ToList();

            return ColeccionSubRecords;                        

        }

        public void AplicaConsulta(string query, DynamicParameters p)
        {
            try
            {
                this.SourceQuery = query;

                List<T> __result = PagingRepository<T>.GetAllRecords(query, p);

                this.ColeccionRecords = __result;

                this.TotalDeRecords = this.ColeccionRecords == null ? 0 : this.ColeccionRecords.Count;

            }
            catch (Exception ex)
            {
                this.ColeccionRecords = new List<T>();

                this.TotalDeRecords = 0;                
            }

        }

        public void AplicaConsultaFromProcedure(string pStoredProcedure, DynamicParameters p)
        {
            try
            {
                this.SourceQuery = pStoredProcedure;

                List<T> __result = PagingRepository<T>.GetAllRecordsFromProcedure(pStoredProcedure, p);

                this.ColeccionRecords = __result;

                this.TotalDeRecords = this.ColeccionRecords == null ? 0 : this.ColeccionRecords.Count;

            }
            catch (Exception ex)
            {
                this.ColeccionRecords = new List<T>();

                this.TotalDeRecords = 0;
            }

        }

    }
}
