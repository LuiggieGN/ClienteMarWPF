namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_TransaccionDetalle
    {
        [Key]
        public int TransaccionDetalleID { get; set; }

        public int TransaccionID { get; set; }

        public int SorteoCampoID { get; set; }

        [Required]
        public string Referencia { get; set; }

        public string ValorText { get; set; }

        public double? ValorMonto { get; set; }

        [Column(TypeName = "money")]
        public decimal? Ingreso { get; set; }

        [Column(TypeName = "money")]
        public decimal? Descuento { get; set; }

        public virtual RF_Transaccion RF_Transaccion { get; set; }
    }
}
