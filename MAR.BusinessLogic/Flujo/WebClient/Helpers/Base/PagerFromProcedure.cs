using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers.Base
{
    [Serializable()]
    public class PagerFromProcedure<T>
    {
        public int Start { get; set; }
        public int PageSize { get; set; }
        public int TotalDeRecords { get; set; } = 0;
        public List<T> ColeccionRecords { get; set; }
        public string StoredProcedureName { get; set; }


        public PagerFromProcedure(string storedProcedureName)
        {
            this.StoredProcedureName = storedProcedureName;
        }


        public virtual List<T> GetPaginacion(int start, int pageSize , Dictionary<string, object> storedProcedureParams)
        {
            this.Start    = start * pageSize;
            this.PageSize = pageSize;

            Dictionary<string, object> 
            DictionaryProcedureParams= new Dictionary<string, object>();
            DictionaryProcedureParams.Add("@StartRowIndex", this.Start);
            DictionaryProcedureParams.Add("@MaximumRows", this.PageSize);

            if (storedProcedureParams != null)
            {
                foreach (KeyValuePair<string, object> item in storedProcedureParams)
                {
                    DictionaryProcedureParams.Add(item.Key, item.Value);
                }
            }


            int TotalRecordsAPaginar;


            this.ColeccionRecords = PagingRepository<T>.GetAllRecordsFromProcedure(this.StoredProcedureName, DictionaryProcedureParams,  out TotalRecordsAPaginar);


            this.TotalDeRecords = TotalRecordsAPaginar;


            return this.ColeccionRecords;

        }


    }
}
