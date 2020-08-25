namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_Producto
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VP_Producto()
        {
            VP_Comisiones = new HashSet<VP_Comisiones>();
            VP_HTransaccion = new HashSet<VP_HTransaccion>();
            VP_ProductoCampo = new HashSet<VP_ProductoCampo>();
            VP_ProductoConfig = new HashSet<VP_ProductoConfig>();
            VP_Transaccion = new HashSet<VP_Transaccion>();
        }

        [Key]
        public int ProductoID { get; set; }

        [Required]
        public string Nombre { get; set; }

        public string Referencia { get; set; }

        public bool Activo { get; set; }

        public int CuentaTipoID { get; set; }

        public int LogicaKey { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_Comisiones> VP_Comisiones { get; set; }

        public virtual VP_CuentaTipo VP_CuentaTipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_HTransaccion> VP_HTransaccion { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_ProductoCampo> VP_ProductoCampo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_ProductoConfig> VP_ProductoConfig { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_Transaccion> VP_Transaccion { get; set; }
    }
}
