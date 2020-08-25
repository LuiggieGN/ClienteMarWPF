using Flujo.Entities.WebClient.POCO;
using MAR.BusinessLogic.Flujo.WebClient.Helpers.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    [Serializable()]
    public class SubSetMensajerosBalance : PagerFromProcedure<MensajeroBalanceActualRecord>
    {
        public SubSetMensajerosBalance():base("[flujo].[Sp_GetMensajerosBancaBalance]")
        {

        }

    }
}
