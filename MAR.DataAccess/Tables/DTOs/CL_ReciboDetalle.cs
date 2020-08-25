namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CL_ReciboDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ReciboDetalleID { get; set; }

        public int? ReciboID { get; set; }

        public int TransaccionID { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipo { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        [Column(TypeName = "money")]
        public decimal Ingreso { get; set; }

        [Column(TypeName = "money")]
        public decimal? Impuesto { get; set; }

        [Column(TypeName = "money")]
        public decimal? Descuento { get; set; }
    }
}
