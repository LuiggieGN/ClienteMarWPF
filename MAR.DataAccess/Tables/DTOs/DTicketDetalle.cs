namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DTicketDetalle")]
    public partial class DTicketDetalle
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DTicketDetalle()
        {
            DBilleteDetalles = new HashSet<DBilleteDetalle>();
        }

        [Key]
        public int TicketDetalleID { get; set; }

        public int TicketID { get; set; }

        [Required]
        [StringLength(1)]
        public string TDeQP { get; set; }

        [Required]
        [StringLength(20)]
        public string TDeNumero { get; set; }

        public double TDeCantidad { get; set; }

        [Column(TypeName = "money")]
        public decimal TDeCosto { get; set; }

        [Column(TypeName = "money")]
        public decimal TDePago { get; set; }

        [Required]
        [StringLength(2)]
        public string TDePagoTipo { get; set; }

        public string TDeLlave { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DBilleteDetalle> DBilleteDetalles { get; set; }
    }
}
