using System;
using Flujo.DataAccess.FlujoRepositories.WebClient.DapperRepositories;
using System.Collections.Generic;
namespace MAR.BusinessLogic.Flujo.WebClient.Helpers.Base
{
    [Serializable()]
    public class PagerFromQuery<T>
    {
        public int Start { get; set; }
        public int PageSize { get; set; }
        public int TotalDeRecords { get; set; } = 0;
        public List<T> ColeccionRecords { get; set; }
        public string Query { get; set; }


        public PagerFromQuery(string query)
        {
            this.Query = query;
        }


        public virtual List<T> GetPaginacion(int start, int pageSize, Dictionary<string, object> queryParams)
        {
            this.Start = start * pageSize;
            this.PageSize = pageSize;

            Dictionary<string, object>
            DictionaryParams = new Dictionary<string, object>();
            DictionaryParams.Add("@StartRowIndex", this.Start);
            DictionaryParams.Add("@MaximumRows", this.PageSize);

            if (queryParams != null)
            {
                foreach (KeyValuePair<string, object> item in queryParams)
                {
                    DictionaryParams.Add(item.Key, item.Value);
                }
            }


            int TotalRecordsAPaginar;
            this.ColeccionRecords = PagingRepository<T>.GetAllRecordsFromQuery(this.Query, DictionaryParams, out TotalRecordsAPaginar);
            this.TotalDeRecords = TotalRecordsAPaginar;
            return this.ColeccionRecords;

        }
    }
}
