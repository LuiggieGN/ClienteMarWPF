namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_HTransaccion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VP_HTransaccion()
        {
            VP_HTransaccionDetalle = new HashSet<VP_HTransaccionDetalle>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HTransaccionID { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TransaccionID { get; set; }

        [Required]
        [StringLength(20)]
        public string Referencia { get; set; }

        [StringLength(50)]
        public string ReferenciaCliente { get; set; }

        public int ReciboID { get; set; }

        public int SolicitudID { get; set; }

        public int ProductoID { get; set; }

        public int SuplidorID { get; set; }

        public int CuentaID { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaDescuento { get; set; }

        [Required]
        [StringLength(250)]
        public string Estado { get; set; }

        [Column(TypeName = "money")]
        public decimal Ingresos { get; set; }

        [Column(TypeName = "money")]
        public decimal? Descuentos { get; set; }

        public bool Activo { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int BancaID { get; set; }

        public int UsuarioID { get; set; }

        public virtual VP_Producto VP_Producto { get; set; }

        public virtual VP_Suplidor VP_Suplidor { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_HTransaccionDetalle> VP_HTransaccionDetalle { get; set; }
    }
}
