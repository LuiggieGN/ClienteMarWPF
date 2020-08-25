using System;
using System.Collections.Generic;


namespace MAR.DataAccess.Tables.DTOs
{
    public partial class CL_Recibo
    {
        //New Properties and Navegations
        public ICollection<RF_Transaccion> RF_Transacciones { get; set; }

        public CL_Recibo()
        {
            RF_Transacciones = new List<RF_Transaccion>();
            CL_ReciboDetalle_Extra = new HashSet<CL_ReciboDetalle_Extra>();
        }
    }
}
