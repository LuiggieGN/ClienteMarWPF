using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Flujo.Entities.WebClient.POCO;
using MAR.BusinessLogic.Flujo.WebClient.Helpers.Base;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    [Serializable()]
    public class SubSetBancasBalace : PagerFromProcedure<BancaBalanceActualRecord>
    {
        public SubSetBancasBalace():base("[flujo].[Sp_GetBancasBalances]")
        {

        }





    }
}
