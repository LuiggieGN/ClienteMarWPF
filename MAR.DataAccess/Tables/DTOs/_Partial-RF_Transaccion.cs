using System;
using System.Collections.Generic;


namespace MAR.DataAccess.Tables.DTOs
{
    public partial class RF_Transaccion
    {
        //New Properties and Navegations
        public ICollection<RF_TransaccionJugada> RFTransaccionJugadas { get; set; }

        public RF_Transaccion()
        {
            RFTransaccionJugadas = new HashSet<RF_TransaccionJugada>();
            RF_TransaccionDetalle = new HashSet<RF_TransaccionDetalle>();
        }
    }
}
