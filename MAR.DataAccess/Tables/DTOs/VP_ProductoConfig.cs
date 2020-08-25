namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VP_ProductoConfig
    {
        [Key]
        public int ProductoConfigID { get; set; }

        [Required]
        [StringLength(200)]
        public string ConfigKey { get; set; }

        public string ConfigValue { get; set; }

        public bool Activo { get; set; }

        public int ProductoID { get; set; }

        public virtual VP_Producto VP_Producto { get; set; }
    }
}
