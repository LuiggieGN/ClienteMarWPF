namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CL_Recibo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
       

        [Key]
        public int ReciboID { get; set; }

        [Required]
        [StringLength(20)]
        public string Referencia { get; set; }

        [StringLength(50)]
        public string Serie { get; set; }

        public int SolicitudID { get; set; }

        public DateTime Fecha { get; set; }

        public int BancaID { get; set; }

        public int UsuarioID { get; set; }

        public int? ClienteID { get; set; }

        [Column(TypeName = "money")]
        public decimal Ingresos { get; set; }

        [Column(TypeName = "money")]
        public decimal? Impuestos { get; set; }

        [Column(TypeName = "money")]
        public decimal? Descuentos { get; set; }

        public int MonedaID { get; set; }
        public bool Activo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CL_ReciboDetalle_Extra> CL_ReciboDetalle_Extra { get; set; }
    }
}
