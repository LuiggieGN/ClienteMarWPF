namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("PMProductos")]
    public partial class PMProducto
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductoID { get; set; }

        [Required]
        [StringLength(50)]
        public string ProNombre { get; set; }

        public double ProPrecio { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SuplidorID { get; set; }

        public bool ProActivo { get; set; }

        public virtual PMSuplidore PMSuplidore { get; set; }
    }
}
