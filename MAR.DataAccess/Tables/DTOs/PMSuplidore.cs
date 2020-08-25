namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class PMSuplidore
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PMSuplidore()
        {
            PMProductos = new HashSet<PMProducto>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SuplidorID { get; set; }

        [Required]
        [StringLength(50)]
        public string SupNombre { get; set; }

        [Column(TypeName = "text")]
        public string SupComentario { get; set; }

        [Required]
        [StringLength(200)]
        public string SupInstrucciones { get; set; }

        public double SupImpuesto { get; set; }

        public double SupComision { get; set; }

        public bool SupActivo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PMProducto> PMProductos { get; set; }
    }
}
