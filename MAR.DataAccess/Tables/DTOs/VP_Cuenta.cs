namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_Cuenta
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VP_Cuenta()
        {
            VP_CuentaConfig = new HashSet<VP_CuentaConfig>();
            VP_Suplidor = new HashSet<VP_Suplidor>();
        }

        [Key]
        public int CuentaID { get; set; }

        [Required]
        public string Nombre { get; set; }

        public int LogicaKey { get; set; }

        public bool Activo { get; set; }

        public int CuentaTipoID { get; set; }

        public virtual VP_CuentaTipo VP_CuentaTipo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_CuentaConfig> VP_CuentaConfig { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VP_Suplidor> VP_Suplidor { get; set; }
    }
}
