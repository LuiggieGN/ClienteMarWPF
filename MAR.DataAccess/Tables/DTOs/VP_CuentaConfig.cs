namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_CuentaConfig
    {
        [Key]
        public int CuentaConfigID { get; set; }

        [Required]
        [StringLength(200)]
        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public bool Activo { get; set; }

        public int CuentaID { get; set; }

        public virtual VP_Cuenta VP_Cuenta { get; set; }
    }
}
