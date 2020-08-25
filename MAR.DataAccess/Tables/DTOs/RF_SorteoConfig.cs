namespace MAR.DataAccess.Tables.DTOs
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RF_SorteoConfig
    {
        [Key]
        public int SorteoConfigID { get; set; }

        public int SorteoID { get; set; }

        [Required]
        [StringLength(200)]
        public string ConfigKey { get; set; }

        [Required]
        public string ConfigValue { get; set; }

        public bool Activo { get; set; }
    }
}
