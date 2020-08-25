namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_Suplidor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VP_Suplidor()
        {
            VP_HTransaccion = new HashSet<VP_HTransaccion>();
            VP_Transaccion = new HashSet<VP_Transaccion>();
        }

        [Key]
        public int SuplidorID { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Referencia { get; set; }

        public bool Activo { get; set; }

        public int CuentaID { get; set; }

        public double Comision { get; set; }

        public double Impuesto { get; set; }

        public virtual VP_Cuenta VP_Cuenta { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_HTransaccion> VP_HTransaccion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_Transaccion> VP_Transaccion { get; set; }
    }
}
