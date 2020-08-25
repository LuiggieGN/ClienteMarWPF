using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MAR.DataAccess.Tables.DTOs;

namespace MAR.DataAccess.EFRepositories.RFRepositories
{
    public class RFEsquemaRepository
    {
        //El siguiente codigo garantiza que la libreria del provider the SQLClient para Entity Framework sea copiado cuando se haga Publish
        //Referencia: http://stackoverflow.com/questions/21175713/no-entity-framework-provider-found-for-the-ado-net-provider-with-invariant-name
        private volatile Type _sqlProviderDependency;
        public RFEsquemaRepository()
        {
            _sqlProviderDependency = typeof(System.Data.Entity.SqlServer.SqlProviderServices);
        }
    }
}
