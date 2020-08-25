namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_Transaccion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
       

        [Key]
        public int TransaccionID { get; set; }

        [Required]
        [StringLength(50)]
        public string Referencia { get; set; }

        [StringLength(50)]
        public string Serie { get; set; }

        public int ReciboID { get; set; }

        public int SolicitudID { get; set; }

        public int SorteoDiaID { get; set; }

        public int EsquemaPagoID { get; set; }

        public DateTime FechaIngreso { get; set; }

        public DateTime? FechaPago { get; set; }

        [Required]
        [StringLength(20)]
        public string Estado { get; set; }

        [Column(TypeName = "money")]
        public decimal Ingresos { get; set; }

        [Column(TypeName = "money")]
        public decimal? Pagos { get; set; }

        public string Notas { get; set; }

        public bool Activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RF_TransaccionDetalle> RF_TransaccionDetalle { get; set; }
    }
}
