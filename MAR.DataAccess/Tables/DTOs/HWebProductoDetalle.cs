namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HWebProductoDetalle")]
    public partial class HWebProductoDetalle
    {
        [Key]
        public int WebProductoDetalleID { get; set; }

        public int PinID { get; set; }

        public int WebProductoID { get; set; }

        [Required]
        [StringLength(500)]
        public string FieldKey { get; set; }

        [Required]
        [StringLength(5000)]
        public string FieldValue { get; set; }
    }
}
