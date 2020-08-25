namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_ResumenVenta
    {
        [Key]
        public int ResumenVentaID { get; set; }

        public int BancaID { get; set; }

        public int? EsquemaPagoID { get; set; }

        public int SorteoDiaID { get; set; }

        public DateTime Fecha { get; set; }

        [Column(TypeName = "money")]
        public decimal Aposto { get; set; }

        [Column(TypeName = "money")]
        public decimal Comision { get; set; }

        [Column(TypeName = "money")]
        public decimal Saco { get; set; }
    }
}
