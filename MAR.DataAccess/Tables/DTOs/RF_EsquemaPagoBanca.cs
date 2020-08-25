namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_EsquemaPagoBanca
    {
        [Key]
        public int EsquemaBancaID { get; set; }

        public int BancaID { get; set; }

        public int EsquemaPagoID { get; set; }

        public DateTime FechaActivo { get; set; }

        public bool Activo { get; set; }
    }
}
