namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DWebProductoDetalle")]
    public partial class DWebProductoDetalle
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PinID { get; set; }

        public int WebProductoID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(500)]
        public string FieldKey { get; set; }

        [Required]
        [StringLength(5000)]
        public string FieldValue { get; set; }
    }
}
