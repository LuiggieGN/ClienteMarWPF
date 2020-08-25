using System;

using Flujo.Entities.WebClient.POCO;
using MAR.BusinessLogic.Flujo.WebClient.Helpers.Base;
using Flujo.DataAccess.FlujoRepositories.WebClient.Helpers;

namespace MAR.BusinessLogic.Flujo.WebClient.Helpers
{
    [Serializable()]
    public class SubSetMovimientoAdministrativos : PagerFromQuery<MovimientoDatos>
    {
        public SubSetMovimientoAdministrativos() : base(SelectView.VMOVIMIENTOS__ADMINISTRATIVOS)
        {
        }
    }
}