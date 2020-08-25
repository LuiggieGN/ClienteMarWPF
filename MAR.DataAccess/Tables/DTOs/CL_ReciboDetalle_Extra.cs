namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CL_ReciboDetalle_Extra
    {
        [Key]
        public int ReciboDetalleID { get; set; }

        public int ReciboID { get; set; }

        public int ReciboCampoID { get; set; }

        [Required]
        public string Referencia { get; set; }

        public string ValorText { get; set; }

        public double? ValorMonto { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ingreso { get; set; }

        [Column(TypeName = "money")]
        public decimal? Descuento { get; set; }

        public virtual CL_Recibo CL_Recibo { get; set; }
    }
}
