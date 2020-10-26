using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using FlujoCustomControl.ViewModels.CommonViews;
using Flujo.Entities.WpfClient.ResponseModels;


using Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers;


namespace FlujoCustomControl.ViewModels
{
   public  class MovimientoPagerViewModel : Pager<MovimientoResponseModel>
    {    

        public MovimientoPagerViewModel(Dictionary<string, object> parametros,  string query, string defualtSortColumn, int pPageSize) :base(parametros, query, defualtSortColumn, pPageSize)
        {
        }

    }
}
