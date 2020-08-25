using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flujo.DataAccess.FlujoRepositories.WpfClient.Helpers
{
    public static class SelectView
    { 

        public static  string SelectRangoFechaMovimientos
        {
            get
            {
                string query = @"[flujo].[Sp_ConsultaCajaMovimientos]";

                return query;
            }
         }

    }
}
